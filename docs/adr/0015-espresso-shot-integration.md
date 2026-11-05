# ADR-0015: 에스프레소 머신은 팀원의 EspressoShot 구현으로 통합한다

- 상태: 제안
- 날짜: 2026-11-05
- 관련 명세: SFR-029~036, SFR-022, SFR-023

## 맥락

에스프레소 머신(샷 내리기)이 두 벌 구현되어 있었다.

- `feature/business-loop`: `Cafe/EspressoBar`(상태) + `EspressoScreen`(화면), Codex 그림 `Art/UI/Espresso`
- main(팀원): `Scripts/EspressoShot`(포터필터 드래그, 그라인더, 탬퍼, 그룹헤드 추출), 그림 `Art/UI/EspressoMachine`, 테스트 씬 `ShotTest`

팀원 구현은 단독 테스트용이라 영업 흐름과 연결되지 않았다. 샷 결과물이 음료 제조로 넘어가지 않았고 원두 재고는 임시 `BeanInventory` 였다. 일시정지에도 멈추지 않았고 경고 문구도 자체 토스트로 띄웠다.

## 결정

사용자 결정("팀원 것으로 교체", "직접 수정 OK", "팀원 그림", 내 코드 "삭제")에 따라 팀원 구현을 영업 씬에 통합한다.

1. `EspressoMachine` 은 영업 씬에서 항상 켜진 `Business` 오브젝트에 붙는다. 분쇄·추출 타이머(`RunTimed`)도 여기서 돈다.
   - 화면을 닫아도 작업이 계속된다.
   - `IWorkStatus` 를 구현해 맵에 작업 상태를 표시한다 (SFR-022).
2. 시간은 `GameClock.Delta` 로 흐른다. 그래서 일시정지(SFR-023) 중에는 멈춘다.
   - 분쇄·추출 시간은 `GameBalance` 값을 쓴다 (5초).
   - `GameClock` 이 없는 ShotTest 씬에서는 실제 시간으로 흐른다.
3. 화면은 `Popup` 이다. ESC, 이동 잠금, `Popup.AnyOpen` 이 다른 기기 화면과 똑같이 동작한다.
   - Popup 이 없는 ShotTest 에서는 기존처럼 직접 ESC 를 처리한다.
4. 그룹헤드 아래에 샷 컵 자리 `ShotCupSlot` 을 추가한다 (SFR-034). 컵은 기존 물건 모델의 `ShotGlass` 이다.
   - 추출이 끝나면 컵이 '샷이 든 컵'이 되고, 손 칸으로 회수해 음료 제조실에서 붓는다.
   - 머신에는 컵 2개가 놓인 채로 시작한다.
5. 원두는 `Pantry`(오픈 준비에서 산 재고)에서 차감한다. 경고 문구는 게임 공통 `Toast`(`ToastMessage`)로 띄운다. `BeanInventory` 는 삭제한다.
6. 영업 씬 화면은 `StationUiBuilder.BuildEspresso` 가 ShotTest 와 같은 좌표로 만든다. 손 칸이 보이도록 0.8배로 줄인다.
7. 기존 `EspressoBar`, `EspressoScreen`, `Cafe/Portafilter*` 와 Codex 그림 `Art/UI/Espresso` 는 삭제한다.
8. ShotTest 씬은 그대로 둔다. 팀원 테스트용이며, 컵 자리가 없으면 컵 없이 추출한다.

## 검토한 대안

- **내 구현(EspressoBar) 유지, 팀원 코드 삭제**: 팀원 작업을 버리게 된다. 사용자가 교체를 선택했다.
- **팀원 파일은 그대로 두고 Cafe 쪽 어댑터로만 연결**: 일시정지, 재고, 토스트가 두 방식으로 남는다. 사용자가 직접 수정을 허락했다.

## 결과

- 에스프레소 기능이 한 벌만 남는다. 영업 흐름(재고, 일시정지, 샷 컵 → 음료)과 연결된다.
- 팀원 스크립트의 공개 필드는 유지했다. 바뀐 점은 다음과 같다.
  - `PortafilterSlot.cup` 필드를 추가했다.
  - 추출 시 컵 검사가 생겼다.
  - 원두는 Pantry 에서 차감한다.
  - 시간이 GameBalance 5초로 바뀌었다 (기존 25초).
  - 토스트가 공통 Toast 로 바뀌었다.
  - ESC 는 Input System 으로 처리한다.
  - 이 변경점을 PR 에서 팀원과 공유한다.
- `EspressoToast` 는 ShotTest 씬에만 남아 있고 코드에서는 더 이상 호출하지 않는다. ShotTest 에는 공통 Toast 가 없어서 경고 문구가 보이지 않는다.
- "이미 포터필터가 있습니다." 는 명세 문구가 아니어서 경고 없이 원래 자리로 되돌린다.
