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
| [0018](0018-pickup-trays-per-order.md) | 픽업대는 주문마다 트레이를 두고 서빙 버튼으로 내보낸다 | 대체됨 (ADR-0030 베이커리 부분, ADR-0040 서빙 버튼 부분) |
| [0019](0019-recipe-toggle-scoop-return-station-headers.md) | 레시피북 온도 선택, 빈 스쿱 자동 반납, 기기 창 제목·안내 문구 제거 | 승인 |
| [0020](0020-ten-minute-day-and-difficulty-table.md) | 하루 영업을 약 10분으로 줄이고 DAY별 난이도 표를 둔다 | 대체됨 (ADR-0021, 하루 시간 부분) |
| [0021](0021-seven-minute-day.md) | 하루 영업을 약 7분으로 더 줄인다 | 승인 |
| [0022](0022-pickup-fifo-penalty.md) | 픽업대에서 선입선출을 어기면 감점한다 | 승인 |
| [0023](0023-hide-hud-stock.md) | 영업 HUD 에 재료 재고를 표시하지 않는다 | 승인 |
| [0024](0024-drink-ingredient-tool-windows.md) | 음료 제조실 재료는 재료별 작업 창에서 넣는다 | 대체됨 (ADR-0028 청·티백 조작 부분, ADR-0035 펌프 조작 부분) |
| [0025](0025-customers-and-chairs-no-collision.md) | 손님과 의자는 플레이어와 충돌하지 않는다 | 승인 |
| [0026](0026-game-font-jua.md) | 게임 폰트를 Jua로 변경 | 승인 |
| [0027](0027-drink-result-stamp.md) | 음료 판정 결과는 완성품 그림과 완성도 도장으로 보여 준다 | 승인 |
| [0028](0028-cheong-spoon-and-tea-steep-drag.md) | 청은 숟가락을 끌어 넣고 티백은 물 담긴 컵에 끌어 담근다 | 승인 |
| [0029](0029-shake-room-drag-and-hold-blend.md) | 쉐이크 제조실은 끌어 넣고 누르고 있는 동안 갈며, 포스기는 실제 단말기 화풍으로 바꾼다 | 대체됨 (ADR-0036 뚜껑 부분) |
| [0030](0030-bakery-self-service.md) | 베이커리 8종과 손님 직접 픽업·계산 방식 | 대체됨 (ADR-0031 포스기 입력 부분, ADR-0032 진열장 위치·팝업 부분) |
| [0031](0031-bakery-manual-pos.md) | 베이커리는 손님 대사를 보고 포스기에 직접 입력한다 | 승인 |
| [0032](0032-lower-bakery-showcase.md) | 손님은 아래쪽 큰 베이커리 진열대에서 직접 픽업한다 | 승인 |
| [0033](0033-compact-front-counter.md) | 행주 걸이를 오른쪽으로 옮기고 기존 받침대 한 칸을 제거한다 | 승인 |
| [0034](0034-bakery-shelf-stock-display.md) | 빵 진열대는 재고만큼 작은 빵을 칸마다 늘어놓고 홀 세로를 채운다 | 승인 |
| [0035](0035-pump-drag-press.md) | 시럽·연유 펌프는 머리를 아래로 끌어 누른다 | 승인 |
| [0036](0036-blender-lid-step.md) | 믹서기 뚜껑은 끌어다 덮어야 갈 수 있고, 열어야 넣고 부을 수 있다 | 승인 |
| [0037](0037-cup-contents-layers.md) | 컵 내용물을 재료 층으로 겹쳐 그리기 | 승인 |
| [0038](0038-menu-revamp-fixed-temperature-four-slots.md) | 메뉴 개편, 온도 고정 음료, 손 칸 4개, 재료별 작업 창 그림 | 대체됨 (ADR-0041 콜드브루 ICE 고정 부분) |
| [0039](0039-hud-day-image-no-rent.md) | 영업 HUD에서 임대료를 빼고 DAY 그림·원 표기·평판 숫자를 쓴다 | 승인 |
| [0040](0040-pickup-bell-call.md) | 픽업대 진동벨 번호 호출로 서빙 | 승인 |
| [0041](0041-cold-brew-hot-temperature-pictures.md) | 콜드브루 HOT 추가, 포스기 카드 그림을 HOT/ICE 에 맞춰 바꾼다 | 승인 |
| [0042](0042-group-order-event.md) | DAY 4 단체 주문은 영업 중 대화창으로 수락하고 픽업대 마감 주문으로 처리한다 | 제안 |
| [0043](0043-restore-shot-recipes.md) | 샷이 빠진 커피 레시피는 에셋 동기화 때 코드 레시피로 복구한다 | 제안 |
| [0044](0044-close-at-six.md) | 영업 종료를 18:00, 입장 마감을 17:30으로 당긴다 | 승인 |
| [0045](0045-random-bell-numbers.md) | 진동벨 번호를 1~30 중 무작위로 준다 | 승인 |
| [0046](0046-steamed-milk.md) | HOT 우유 메뉴는 스팀 피처로 데운 우유를 쓴다 | 승인 |
