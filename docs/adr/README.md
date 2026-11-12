# ADR 목록

설계 결정 기록(Architecture Decision Record). 작성 방법은 [ADR-0001](0001-record-architecture-decisions.md)과 [template.md](template.md)를 본다.

새 ADR 만들기 (번호와 아래 목록 추가가 자동):

```bash
bash docs/adr/new-adr.sh "결정 제목" english-file-slug
```

상태가 바뀌면 파일의 `- 상태:` 줄과 아래 표를 함께 고친다. **표는 이 파일의 마지막에 둔다** (스크립트가 끝에 줄을 추가함).

| 번호 | 제목 | 상태 |
|---|---|---|
| [0001](0001-record-architecture-decisions.md) | 설계 결정을 ADR로 기록한다 | 승인 |
| [0002](0002-unity-version-and-stack.md) | Unity 6000.6.4f1 + URP 2D + Input System을 사용한다 | 승인 |
| [0003](0003-folder-and-namespace-rules.md) | 기능별 폴더 + 폴더 이름 = namespace, Core는 단방향 의존 | 승인 |
| [0004](0004-popup-base-class.md) | 모든 팝업·기기 화면은 Popup 베이스 클래스를 상속한다 | 승인 |
| [0005](0005-json-save-after-settlement.md) | 진행 데이터는 JSON 파일 하나에, 하루 정산 후에만 저장한다 | 승인 |
| [0006](0006-editor-builder-scripts-for-ui.md) | UI 씬·프리팹의 초기 배치는 에디터 빌더 스크립트로 만든다 | 승인 |
| [0007](0007-scene-conflict-prevention.md) | 씬 충돌은 프리팹 분리 + Smart Merge + 동시 수정 금지로 막는다 | 승인 |
| [0008](0008-ai-generated-art-assets.md) | UI·아트 에셋은 AI 이미지 생성으로 만들고 생성 기록을 남긴다 | 승인 |
| [0009](0009-balance-values-in-scriptableobject.md) | 밸런스 수치는 GameBalance ScriptableObject 하나에 모은다 | 승인 (구현 예정) |
| [0010](0010-inventory-base-units.md) | 재료 재고는 기본 단위(g·개·회)로 저장한다 | 승인 (구현 예정) |
| [0011](0011-day-cycle-scene-flow.md) | 하루 사이클의 씬 흐름과 영업 결과 전달 방식 | 제안 |
| [0012](0012-branch-protection-policy.md) | main 브랜치는 PR 필수, 리뷰 승인은 선택, Repo check는 필수로 한다 | 승인 |
| [0013](0013-business-loop-structure.md) | 영업 루프 구조와 임시 제조실 | 제안 |
| [0014](0014-device-stations-and-item-model.md) | 기기 화면, 물건 모델, 레시피 판정 구조 | 제안 |
| [0015](0015-espresso-shot-integration.md) | 에스프레소 머신은 팀원의 EspressoShot 구현으로 통합한다 | 제안 |
| [0016](0016-cafe-map-layout-and-modern-style.md) | 카페 맵을 주방 축소 배치와 모던 스타일로 바꾼다 | 제안 |
| [0017](0017-order-list-modal-and-recipe-book.md) | 상단 주문표를 주문 내역 버튼·모달로 바꾸고 레시피북을 둔다 | 대체됨 (ADR-0018 픽업대 배정 부분, ADR-0019 레시피 카드 배치 부분) |
| [0018](0018-pickup-trays-per-order.md) | 픽업대는 주문마다 트레이를 두고 서빙 버튼으로 내보낸다 | 승인 |
| [0019](0019-recipe-toggle-scoop-return-station-headers.md) | 레시피북 온도 선택, 빈 스쿱 자동 반납, 기기 창 제목·안내 문구 제거 | 승인 |
| [0020](0020-ten-minute-day-and-difficulty-table.md) | 하루 영업을 약 10분으로 줄이고 DAY별 난이도 표를 둔다 | 대체됨 (ADR-0021, 하루 시간 부분) |
| [0021](0021-seven-minute-day.md) | 하루 영업을 약 7분으로 더 줄인다 | 승인 |
| [0022](0022-pickup-fifo-penalty.md) | 픽업대에서 선입선출을 어기면 감점한다 | 승인 |
| [0023](0023-hide-hud-stock.md) | 영업 HUD 에 재료 재고를 표시하지 않는다 | 승인 |
| [0024](0024-drink-ingredient-tool-windows.md) | 음료 제조실 재료는 재료별 작업 창에서 넣는다 | 대체됨 (ADR-0028, 청·티백 조작 부분) |
| [0025](0025-customers-and-chairs-no-collision.md) | 손님과 의자는 플레이어와 충돌하지 않는다 | 승인 |
| [0026](0026-game-font-jua.md) | 게임 폰트를 Jua로 변경 | 승인 |
| [0027](0027-drink-result-stamp.md) | 음료 판정 결과는 완성품 그림과 완성도 도장으로 보여 준다 | 승인 |
| [0028](0028-cheong-spoon-and-tea-steep-drag.md) | 청은 숟가락을 끌어 넣고 티백은 물 담긴 컵에 끌어 담근다 | 승인 |
| [0029](0029-shake-room-drag-and-hold-blend.md) | 쉐이크 제조실은 끌어 넣고 누르고 있는 동안 갈며, 포스기는 실제 단말기 화풍으로 바꾼다 | 승인 |
