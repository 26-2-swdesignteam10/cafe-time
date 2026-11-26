using System.Collections;
using System.Collections.Generic;
using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 알바생 한 명의 하루 (ADR-0057): 맡은 일 중 급한 순서(테이블 청소 → 퇴식구 쟁반 → 설거지)로 일감을 찾아
    // 걸어가 처리하고, 할 일이 없으면 주방 대기 자리로 돌아간다. 걷기·그림은 손님(Customer)과 같은 방식
    public class StaffWorker : MonoBehaviour
    {
        StaffDefinition definition;
        Customer body;
        StaffManager manager;
        StaffManager.Place here;
        StaffManager.Place idle;
        GameBalance balance;

        public void Init(StaffDefinition staff, Customer customer, StaffManager owner, StaffManager.Place idlePlace)
        {
            definition = staff;
            body = customer;
            manager = owner;
            here = idle = idlePlace;
            balance = GameBalance.Instance;
            StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            while (true)
            {
                if (definition.Does(StaffTask.Tables) && manager.TryClaimTable(out var table))
                    yield return CleanTable(table);
                else if (definition.Does(StaffTask.Trays) && manager.TryClaimTrays())
                    yield return ClearTrays();
                else if (definition.Does(StaffTask.Dishes) && manager.TryClaimDishes())
                    yield return WashDishes();
                else if (here != idle)
                    yield return WalkTo(idle);
                else
                    yield return null;
            }
        }

        IEnumerator WalkTo(StaffManager.Place place)
        {
            body.Walk(StaffManager.Route(here, place));
            here = place;
            while (body.IsMoving) yield return null;
            body.Face(place.Facing);
        }

        // 게임 시간으로 seconds 초 동안 일한다. stop 이 true 가 되면 바로 멈춤
        IEnumerator Work(float seconds, System.Func<bool> stop)
        {
            while (seconds > 0f && !stop())
            {
                seconds -= GameClock.Delta;
                yield return null;
            }
        }

        // 청소 요청이 뜬 테이블을 닦는다. 그 사이 플레이어가 닦았으면 그냥 돌아간다
        IEnumerator CleanTable(CafeTable table)
        {
            yield return WalkTo(manager.TablePlace(table));
            yield return Work(balance.StaffCleanSeconds, () => !table.CleaningRequested);
            if (table.CleaningRequested) table.Clean();
            manager.ReleaseTable(table);
        }

        // 퇴식구 쟁반을 하나씩 정리한다: 쓰레기는 버리고, 잔·접시는 모아 설거지 대기 바구니에 넣는다
        IEnumerator ClearTrays()
        {
            yield return WalkTo(manager.TrayReturnPlace);
            var dishes = new List<TrayItemKind>();
            var slots = TrayReturn.Instance.Slots;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null) continue;
                int slot = i;
                yield return Work(balance.StaffCleanSeconds, () => slots[slot] == null);
                var taken = TrayReturn.Instance.ClearSlot(slot);
                if (taken != null) dishes.AddRange(taken);
            }
            manager.ReleaseTrays();
            if (dishes.Count == 0) yield break;
            yield return WalkTo(manager.DishPlace);
            foreach (var dish in dishes) DishStation.Instance.Enqueue(dish);
        }

        // 싱크대 앞에서 잔·접시를 닦는다. 청소도 맡은 알바생은 청소 요청이 뜨면 손을 멈추고 간다
        IEnumerator WashDishes()
        {
            yield return WalkTo(manager.DishPlace);
            var dish = DishStation.Instance;
            while (dish.InSink != null && !(definition.Does(StaffTask.Tables) && manager.HasTableWork))
            {
                dish.AutoScrub(GameClock.Delta, balance.StaffWashSeconds);
                yield return null;
            }
            manager.ReleaseDishes();
        }
    }
}
