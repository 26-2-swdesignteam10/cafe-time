using CafeTime.Core;
using UnityEngine;

namespace CafeTime.Cafe
{
    // 손님용 빵 진열대 (SFR-056, ADR-0034). 칸마다 남은 재고만큼 작은 빵 그림을 늘어놓는다.
    // 진열대가 여러 개면 재고를 번갈아 나눠 담는다 (1개째 → 0번, 2개째 → 1번, ...).
    public class BakeryShelfView : MonoBehaviour
    {
        [SerializeField] string[] stockIds;   // 칸 순서대로 담을 재고 id
        [SerializeField] Sprite[] sprites;    // stockIds[i] 칸에 놓을 작은 빵 그림
        [SerializeField] Rect[] cells;        // 칸 안쪽 선반 영역 (진열대 기준 로컬 좌표, 1칸 = 1)
        [SerializeField] int shelfIndex;
        [SerializeField] int shelfCount = 1;
        [SerializeField] int perCell = 6;     // 한 칸에 보여 줄 최대 개수
        [SerializeField] int columns = 3;     // 한 줄에 놓는 개수 (앞줄부터 채움)
        [SerializeField] float backRowLift = 0.3f; // 뒷줄을 칸 높이의 몇 배만큼 올릴지

        SpriteRenderer[][] slots;
        int[] shown;

        void Start()
        {
            slots = new SpriteRenderer[cells.Length][];
            shown = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                slots[i] = new SpriteRenderer[perCell];
                for (int s = 0; s < perCell; s++) slots[i][s] = CreateSlot(i, s);
            }
        }

        SpriteRenderer CreateSlot(int cell, int index)
        {
            var rect = cells[cell];
            int row = index / columns, col = index % columns;
            // 뒷줄은 반 칸 오른쪽으로 어긋나게 놓아 쌓인 느낌을 준다. 그림이 칸 밖으로 나가지 않게 간격을 정함
            float half = sprites[cell] != null ? sprites[cell].bounds.extents.x : 0f;
            float spacing = (rect.width - half * 2) / (columns - 0.5f);
            float x = rect.xMin + half + spacing * (col + (row % 2 == 0 ? 0f : 0.5f));
            float y = rect.yMin + rect.height * backRowLift * row;

            var sr = new GameObject($"{stockIds[cell]} {index + 1}").AddComponent<SpriteRenderer>();
            sr.transform.SetParent(transform, false);
            sr.transform.localPosition = new Vector3(x, y, 0);
            sr.sprite = sprites[cell];
            sr.sortingOrder = 100 - row; // 진열대 그림(SortingGroup 안 0)보다 앞, 앞줄이 뒷줄보다 앞
            sr.enabled = false;
            return sr;
        }

        void Update()
        {
            var save = SaveSystem.Current;
            if (save == null) return;
            for (int i = 0; i < cells.Length; i++)
            {
                int count = Mathf.Min(perCell, OnThisShelf(save.GetStock(stockIds[i])));
                if (count == shown[i]) continue;
                for (int s = 0; s < perCell; s++) slots[i][s].enabled = s < count;
                shown[i] = count;
            }
        }

        int OnThisShelf(int stock) => stock > shelfIndex ? (stock - shelfIndex + shelfCount - 1) / shelfCount : 0;
    }
}
