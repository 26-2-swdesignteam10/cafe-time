Generated with OpenAI 내장 image_gen 도구. Style reference: `Assets/_Project/Art/UI/Pos/Panel.png`, `Assets/_Project/Art/UI/Stations/PickupPanel.png`, `Assets/_Project/Art/UI/Stations/PickupSlot.png`, `Assets/_Project/Art/UI/Pos/PayButton.png`, `Assets/_Project/Art/UI/Pos/ConfirmButton.png`, `Assets/_Project/Art/UI/Pos/DeleteButton.png`, `Assets/_Project/Art/UI/CafeHud/ToastNoCustomer.png`, `Assets/_Project/Art/UI/Quit/Message.png`, `Assets/_Project/Art/Icons/Items/HotCoffee.png`, `Assets/_Project/Art/Sprites/Cafe/StationEspresso.png`.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Style references: previously viewed PickupPanel, PickupSlot, StationEspresso and HotCoffee. One asset only. No captions, no watermark. 

기기 공통 조건: 정면 시점(트레이는 약간 위에서), 왼쪽 위 조명, 2~3단 명암 블록, 다크 커피 외곽선, 문구가 없는 독립 스프라이트. LABEL은 생성한 글자 모양을 유지하며 글자 높이 56px, 크림 외곽선 3px, 외곽선 바깥 투명 여백 8px로 정규화했다.

- TraySlot.png: Exact canvas 660x480. Empty shallow recessed rectangular slot in a stainless steel return counter, isolated single counter inset. Front view with small visible top surface, no items, NO text. Dark coffee #380F00 outline approximately 4px in final image, warm grey steel 2-3 shade blocks, top-left light. Even 16px transparent padding.
- Tray.png: Exact canvas 600x420. One EMPTY brown plastic rectangular cafe serving tray, seen from slightly above, front-facing symmetrical. Raised beveled rim, completely empty flat interior for game items. Dark coffee #380F00 outline approximately 4px in final image, warm browns 2-3 shade blocks, top-left light. Even 16px transparent padding. NO text, no items.
- TrashBin.png: Canvas 520x600. Front view cafe trash bin with OPEN TOP, isolated, no text. Warm grey body, deep dark opening clearly visible, short rectangular body with softly stepped corners.
- DishBin.png: Canvas 640x520. Front view wide grey plastic dish tub containing a little pale soapy water, empty of dishes. Visible open top, shallow basin, small soap bubbles.
- Panel.png: 3200×1800 PNG. PickupPanel 참조와 같은 호두나무 베벨 프레임, 금색 내부 선, 완전히 불투명한 빈 크림 내부. 상단 중앙에 정확한 제목 "퇴식구", 양옆 작은 커피콩 두 개. 나머지 내부는 비움.
- LabelTrash.png: TEXT ONLY LABEL. Exact Korean text "쓰레기통". Single line, preserve spaces and punctuation. Bold pixel lettering matching Quit/Message.png and ToastNoCustomer.png. Dark coffee #380F00 fill, cream #FCF0CA 3px outline. S glyph fill height 56px. Tight crop, even 8px transparent padding. No panel, button, background, ornaments, glow, blur or shadow.
- LabelDishBin.png: TEXT ONLY LABEL. Exact Korean text "설거지통". Single line, preserve spaces and punctuation. Bold pixel lettering matching Quit/Message.png and ToastNoCustomer.png. Dark coffee #380F00 fill, cream #FCF0CA 3px outline. S glyph fill height 56px. Tight crop, even 8px transparent padding. No panel, button, background, ornaments, glow, blur or shadow.
- Hint.png: TEXT ONLY LABEL. Exact Korean text "쓰레기는 쓰레기통, 잔과 접시는 설거지통으로 끌어 놓으세요". Single line, preserve spaces and punctuation. Bold pixel lettering matching Quit/Message.png and ToastNoCustomer.png. Dark coffee #380F00 fill, cream #FCF0CA 3px outline. S glyph fill height 56px. Tight crop, even 8px transparent padding. No panel, button, background, ornaments, glow, blur or shadow.

## 제작·검수 기록 (2026-11-04)

- SFR-059~062 요청 에셋. 최종 파일은 RGBA PNG이며 원본은 Codex generated_images에 보존했다.
- 생성 도구가 요청 크기를 그대로 출력하지 않아 System.Drawing으로 최근접 픽셀 샘플링, 투명 여백, 알파 경계를 정규화했다. LABEL은 생성 글자의 다크 커피 잉크 마스크를 사용하고 새 폰트로 대체하지 않았다. 글자 높이는 외곽선을 제외한 잉크 경계로 측정한다(56px). 외곽선 포함 높이 62px, PNG 높이 78px.
- LABEL의 고정 문구를 최종 PNG에서 글자별로 대조했다. 쉼표와 띄어쓰기를 포함하며 한 줄이다.
- 패널은 외부만 투명하고 프레임과 크림 내부는 불투명하다. 제목·커피콩을 제외한 내부는 비었다.
- .meta 파일은 직접 생성하지 않았다. Unity 6000.6.4f1에서 임포트가 필요하다. 패널의 원본 3200px 폭을 유지하려면 Max Size 4096, Sprite (2D and UI)/Single, Point, Mip Maps 끔, Alpha Is Transparency 켬을 사용한다. LABEL 표시는 PNG의 절반 크기다.



## 2026-11-12 일러스트 교체 (정면 수납장, 샷 내리기 창 스타일)

- 요청: 퇴식구 창을 에스프레소 샷 창과 같은 일러스트로, 사용자가 준 수납장 사진(오크 상판 + 검정 철제 프레임·미닫이 문, 위에 그릇을 포개어 쌓은 모습)처럼 정면에서 보이게. 설거지통에 잔이 점점 쌓이게.
- 도구: Python 3.13 + Pillow 12 + numpy 2.2 절차적 일러스트(생성형 이미지 도구 없음). 제빙기·설거지실 창과 같은 스크립트·재질 함수(`draw_tray.py`, Assets 밖 scratchpad)를 썼다. Lanczos 축소, 알파 6 미만 헤이즈 제거, 완전 투명 픽셀 RGB 0.
- 스타일 참조: `docs/ART_STYLE.md` 1장의 영업 중 기기 작업 창 예외, `../ShotRoom/Panel.png`, `../Water/Panel.png`(벽·빛줄기), `../Dishwash/` 2026-11-12 그림(크롬·오크·차콜 외곽선). 이번 기록이 위의 과거 도트 생성 지시보다 우선한다.
- 최대 수량 판단: 퇴식구 쟁반 칸 4개(SFR-060·062, `GameBalance.trayReturnSlots`) × 일행 최대 4명이 1잔씩(SFR-024) = 한 번에 돌아오는 잔 최대 **16잔**. 그래서 설거지통의 잔 자리는 4개씩 4줄(16자리)이고, 디저트 접시는 쟁반당 최대 4장이라 더미 8자리를 둔다. 이보다 많이 쌓이면 그림은 가득 찬 채로 두고 실제 개수는 설거지실 창에서 센다. (명세서 PDF는 저장소에 없어 `docs/GAME_DATA.md` 의 같은 수치로 판단했다.)

### 파일별 지시·후처리

| 파일 | 지시 / 후처리 | 최종 PNG 크기 |
|---|---|---|
| Panel.png | `../Water/Panel.png` 를 높이 1800 으로 확대하고 가운데 띠만 늘여 3200 폭으로 만든 벽 위에, 정면 수납장을 그렸다: 검정 뒤 가림판(y 440~478)과 양옆 기둥, 오크 상판 윗면(y 474~516)·앞면(516~540), 검정 철제 몸체, 왼쪽·오른쪽 열린 칸(어두운 나무 안쪽, 바닥 판), 가운데 오크 서랍 + 리벳과 바퀴가 있는 검정 미닫이 문(글자 없음). 제목·안내 글자는 넣지 않았다(라벨로 표시). 내부 알파 255. | 3200×1800 |
| Tray.png | “정면에서 약간 위로 본 갈색 플라스틱 쟁반: 밝은 윗면 사다리꼴, 앞 테두리, 손잡이 구멍 두 개.” 표시 340×44. | 680×88 |
| TraySlot.png | “상판 위 쟁반 자리를 표시하는 옅은 그늘 매트.” 표시 360×22. 쟁반이 없을 때도 보인다. | 720×44 |
| TrashBin.png | “정면에서 본 검정 브러시드 금속 쓰레기통, 크롬 테두리의 열린 입구(어두운 안), 휴지 조각 하나, 바닥 그림자.” 표시 240×250. | 480×500 |
| DishBin.png | “낮은 청회색 플라스틱 대야의 뒤판: 테두리, 어두운 안쪽, 안쪽 뒷면.” 표시 380×58. | 760×116 |
| DishBinFront.png | 같은 대야의 앞벽(알파 50% 반투명, 세로 골)과 앞 테두리. 쌓인 잔 아래쪽이 비쳐 보이게 잔 위에 겹친다. | 760×116 |
| LabelTitle.png | 생성형 원화 없음. 정확히 `퇴식구`. Galmuri11 을 원래 크기(12px)로 그린 뒤 5배 NEAREST 확대(채움 55px), 다크 커피 #380F00 / 크림 #FCF0CA 3px 외곽선, 여백 8px. ADR-0019 에 따라 퇴식구 제목은 남긴다. | 197×77 |
| LabelTrash.png | 위와 같은 방법으로 정확히 `쓰레기통`. | 257×77 |
| LabelDishBin.png | 위와 같은 방법으로 정확히 `설거지통`. | 257×77 |
| Hint.png | 위와 같은 방법으로 정확히 `쓰레기는 쓰레기통, 잔과 접시는 설거지통으로 끌어 놓으세요` (쉼표·띄어쓰기 포함 한 줄). | 1687×82 |

- 쟁반 위 물건과 쌓인 잔·접시는 기존 `Icons/Items/Dirty*.png`·`Straw`·`Napkin`·`UsedTeabag` 아이콘을 그대로 쓴다.
- 네 라벨의 한글 철자·띄어쓰기·쉼표를 확인했다.

### 배치 (2026-11-12)

1600×900 창의 왼쪽 위를 (0,0)으로 하는 **화면 px**. `StationUiBuilder.BuildTrayReturn`, `TrayReturnScreen` 이 이 표를 따른다.

| 요소 | 위치 | 크기 | 비고 |
|---|---|---|---|
| LabelTitle | 가운데 (800, 62) | | |
| Hint | 가운데 (800, 112) | | |
| 쟁반 4칸 | x = 75, 445, 815, 1185 (칸 수가 바뀌면 75~1185 를 고르게 나눔), 아래끝 y 512 | 340×44 | 아래에 TraySlot 매트 (x-10, y 498, 360×22) |
| 쟁반 위 물건 | 쟁반 윗면(쟁반 위끝 + 14)을 바닥으로, 한 줄 5개 가운데 정렬, 간격 62 | 96 | 뒷줄은 40 씩 위로, 0.94 배씩 작게. 쟁반당 최대 4잔 + 4접시 + 빨대·티백·냅킨 → 최대 4줄 |
| 쓰레기통 칸 (놓는 자리) | 62, 556 | 426×296 | 쓰레기통 그림 (155, 600, 240×250), LabelTrash 가운데 (275, 590) |
| 설거지통 칸 (놓는 자리) | 1132, 556 | 406×296 | LabelDishBin 가운데 (1335, 590) |
| 대야 뒤판·앞벽 | 1145, 794 | 380×58 | 뒤판 → 잔·접시 → 앞벽 순 |
| 잔 더미 4줄 | 가운데 x = 1190, 1262, 1334, 1406, 맨 아래 잔 위끝 y 746 | 100 | 위로 26 씩, 줄마다 4개 (HOT 잔 먼저, ICE 잔 다음) |
| 접시 더미 | 가운데 x 1478, 맨 아래 위끝 y 768 | 90 | 위로 9 씩, 최대 8장 |

- 모든 요소는 닫기 버튼 예약 영역 (1500,36,64,64)과 겹치지 않는다.
- 새 파일(`DishBinFront`, `LabelTitle`)은 Unity 가 `.meta` 를 만든다. `CafeTime/Build Cafe Scene` 이 이 폴더를 Sprite / Single / **Bilinear** 로 맞춘다.

## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| Hint.png | 쓰레기는 쓰레기통, 잔과 접시는 설거지통으로 끌어 넣으세요 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 1168×78 → **1488×78 (폭 확대)** |
| LabelDishBin.png | 설거지통 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 242×78 |
| LabelTrash.png | 쓰레기통 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 273×78 |
| Panel.png | 퇴식구 | 210px | #FCF0CA | #380F00 9px | 주변 재질 표본으로 글자 영역 국소 복원 + Pillow Jua 조판 | 3200×1800 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.

## 2026-11-12 main 병합 정리

- LabelTrash·LabelDishBin·Hint 는 위 Jua 재조판본을 쓰고, Panel.png 는 일러스트 교체본(글자 없음)을 쓴다. Jua 재조판 절의 Panel.png 행("퇴식구" 제목을 패널에 조판)은 일러스트 교체로 대체됐고, 제목은 LabelTitle.png 로 표시한다.
- LabelTitle.png 를 Jua 로 다시 조판했다: 정확히 `퇴식구`, Pillow 로 `Art/Fonts/Jua-Regular.ttf` 를 4배 크기에서 채움 #380F00 / 크림 #FCF0CA 3px 외곽선으로 그린 뒤 Lanczos 축소. 채움 높이 56px, 사방 투명 여백 8px, 179×78 (전 197×77, Galmuri11). 같은 방법으로 만든 `쓰레기통` 이 위 Jua 재조판본과 같은 모양인지 비교해 확인했다.
