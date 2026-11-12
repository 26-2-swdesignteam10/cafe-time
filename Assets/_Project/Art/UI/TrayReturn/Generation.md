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
