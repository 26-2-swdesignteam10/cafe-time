Generated with OpenAI 내장 image_gen 도구. Style reference: `Assets/_Project/Art/UI/Pos/Panel.png`, `Assets/_Project/Art/UI/Stations/PickupPanel.png`, `Assets/_Project/Art/UI/Stations/PickupSlot.png`, `Assets/_Project/Art/UI/Pos/PayButton.png`, `Assets/_Project/Art/UI/Pos/ConfirmButton.png`, `Assets/_Project/Art/UI/Pos/DeleteButton.png`, `Assets/_Project/Art/UI/CafeHud/ToastNoCustomer.png`, `Assets/_Project/Art/UI/Quit/Message.png`, `Assets/_Project/Art/Icons/Items/HotCoffee.png`, `Assets/_Project/Art/Sprites/Cafe/StationEspresso.png`.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Style references: previously viewed PickupPanel, PickupSlot, StationEspresso and HotCoffee. One asset only. No captions, no watermark. 

기기 공통 조건: 정면 시점(트레이는 약간 위에서), 왼쪽 위 조명, 2~3단 명암 블록, 다크 커피 외곽선, 문구가 없는 독립 스프라이트. LABEL은 생성한 글자 모양을 유지하며 글자 높이 56px, 크림 외곽선 3px, 외곽선 바깥 투명 여백 8px로 정규화했다.

- Panel.png: 정상 생성된 퇴식구 패널을 참조하여 제목만 정확한 "설거지실"로 변경. 동일한 프레임, 금색 선, 커피콩 두 개, 완전히 불투명한 빈 크림 내부 유지. 중앙이 투명해진 앞선 생성본은 폐기.
- Sink.png: Canvas 1280x760. Front view stainless sink basin with a single curved faucet centered at back. Empty broad central basin where game places one cup. No cup, no objects, no text.
- Sponge.png: A small game inventory SPRITE of a kitchen washing SPONGE. 300 x 200 PNG with transparent background. Chunky traditional PIXEL ART, like a 1990s SNES sprite, hard-edged pixels. Simple rectangular yellow foam sponge with green scouring layer underneath, seen slightly from above. Very clear thick dark brown #380F00 solid outline. Flat solid color blocks, yellow #F1C97D upper layer, green #1F362B bottom layer. 3 yellow shades, 2 green shades. No gradients, glow, ambient occlusion or soft shadow. Centered, symmetric 12px padding. No text.
- Stain.png: Canvas 440x440. Overlay sprite only: scattered irregular brown coffee stain pixel blobs for a dirty cup. Brown shapes with approximately 70 percent opacity (alpha 179), transparent everywhere else. No cup, no outline, no text, no shadow. Stepped pixel edges. Keep stains inside even 16px margins.
- DryingRack.png: Exact canvas 1120x600 (display 560x300). Front-facing wooden dish drying rack with precisely SIX empty cup slots in ONE horizontal row. Slot centers in PNG pixels at x=120,296,472,648,824,1000, all cup bottom support lines at y=460. Those positions must be exact. Slots evenly spaced 176px. No cups, no plates, no text. Warm walnut and honey trim, dark coffee 4px outline, crisp stepped pixels, top-left light, flat blocks. Rack feet within canvas, even outer padding.
- WaitingCrate.png: Canvas 600x480. Front view warm grey plastic crate holding a few visibly dirty cream coffee cups and small plates, brown coffee marks. No text. Dark coffee 4px outline, top-left light, 2-3 flat shade blocks. Even transparent padding.
- LabelWaiting.png: TEXT ONLY LABEL. Exact Korean text "설거지 대기". Single line, preserve spaces and punctuation. Bold pixel lettering matching Quit/Message.png and ToastNoCustomer.png. Dark coffee #380F00 fill, cream #FCF0CA 3px outline. S glyph fill height 56px. Tight crop, even 8px transparent padding. No panel, button, background, ornaments, glow, blur or shadow.
- LabelDrying.png: TEXT ONLY LABEL. Exact Korean text "건조 중". Single line, preserve spaces and punctuation. Bold pixel lettering matching Quit/Message.png and ToastNoCustomer.png. Dark coffee #380F00 fill, cream #FCF0CA 3px outline. S glyph fill height 56px. Tight crop, even 8px transparent padding. No panel, button, background, ornaments, glow, blur or shadow.
- Hint.png: TEXT ONLY LABEL. Exact Korean text "수세미를 좌우로 끌어 얼룩을 지우세요". Single line, preserve spaces and punctuation. Bold pixel lettering matching Quit/Message.png and ToastNoCustomer.png. Dark coffee #380F00 fill, cream #FCF0CA 3px outline. S glyph fill height 56px. Tight crop, even 8px transparent padding. No panel, button, background, ornaments, glow, blur or shadow.

## 제작·검수 기록 (2026-11-04)

- SFR-063~065 요청 에셋. 최종 파일은 RGBA PNG이며 원본은 Codex generated_images에 보존했다.
- 생성 도구가 요청 크기를 그대로 출력하지 않아 System.Drawing으로 최근접 픽셀 샘플링, 투명 여백, 알파 경계를 정규화했다. LABEL은 생성 글자의 다크 커피 잉크 마스크를 사용하고 새 폰트로 대체하지 않았다. 글자 높이는 외곽선을 제외한 잉크 경계로 측정한다(56px). 외곽선 포함 높이 62px, PNG 높이 78px.
- LABEL의 고정 문구를 최종 PNG에서 글자별로 대조했다. 쉼표와 띄어쓰기를 포함하며 한 줄이다.
- 패널은 외부만 투명하고 프레임과 크림 내부는 불투명하다. 제목·커피콩을 제외한 내부는 비었다.
- Stain.png의 유효 얼룩 픽셀 알파는 179/255(약 70.2%)로 정규화했다. 기기용 회색, 수세미용 노랑·초록, 물의 옅은 청회색을 기능상 보조색으로 사용했다.
- Sponge.png는 여러 차례 재생성·테두리 수정 요청에도 부드러운 갈색 효과가 남았다. 최종 선택한 정면 생성본(`exec-2d7cd388-b580-43aa-aaf3-c627e8ac5bbc.png`)의 노랑·초록 물체 마스크를 유지한 채 바깥 효과를 제거하고, 노랑 3색·초록 2색으로 정규화한 후 다크 커피 4px 외곽선을 적용했다. 최종 PNG는 300×200, 사방 투명 여백 12px, 블러 없이 알파 0/255다. 위 Sponge 프롬프트는 최종 재생성 시도이며, 선택본 프롬프트는 “One yellow-green kitchen sponge UI sprite, exact canvas 300x200. Front view with thin top visible. Thick DARK COFFEE #380F00 crisp 4px SOLID OUTLINE around entire silhouette. Yellow foam upper 2/3, green scrub bottom 1/3. Only 2-3 FLAT shade blocks, NO gradients, NO cast shadow, NO glow or halo, NO text, no blur. Object fills canvas with even 12px transparent padding.”였다.
- DryingRack.png는 생성한 6개 빈 슬롯을 최근접 좌표 매핑으로 표시 x=60,148,236,324,412,500(PNG x=120,296,472,648,824,1000)에 맞췄다. 컵 바닥 지지선은 표시 y=230(PNG y=460)이다. 컵은 없다.
- .meta 파일은 직접 생성하지 않았다. Unity 6000.6.4f1에서 임포트가 필요하다. 패널의 원본 3200px 폭을 유지하려면 Max Size 4096, Sprite (2D and UI)/Single, Point, Mip Maps 끔, Alpha Is Transparency 켬을 사용한다. LABEL 표시는 PNG의 절반 크기다.


## PR #8 교체 (2026-11)

Codex(AI 에이전트)로 다시 만들어 교체한 것으로 보인다. 도구 확인과 파일별 프롬프트는 작성자에게 받아 보충한다.

- 교체한 파일: Sponge.png


## 2026-11-11 패널 제목·커피콩 제거

- 도구: Python 3 / Pillow. 단순 지우기·채우기 후처리이며 새 원화 생성은 없다.
- 스타일 참조: 이 폴더의 기존 `Panel.png`, `docs/ART_STYLE.md`. ADR-0019(승인)의 기기 창 제목 제거 요청을 적용했다. 새 ADR은 없다.
- `Panel.png` 작업 지시: “상단 중앙의 기존 제목과 양옆 커피콩만 지우고 주변 크림 바탕으로 이음매 없이 복원한다. 원래 프레임·금테·나머지 픽셀·캔버스 크기·알파를 보존한다.”
- 후처리: 원본 px 기준 사각형 `(890, 211, 2314, 486)`(오른쪽·아래 끝 제외) 안에서만 수정했다. 같은 높이의 좌우 크림색을 보간하고 아래쪽 빈 바탕의 미세 질감 표본을 더해 자연스럽게 채웠다. 이 사각형 밖 RGBA 픽셀이 원본과 완전히 일치하는지 검사했다. 전체 알파와 파일 크기도 원본과 같다.
- 결과: Panel.png — 3200×1800px. 지운 제목 외 새 글자는 없다.
- `.meta`, 코드, 씬, 프리팹, 에셋 정의는 수정하지 않았다. 픽업대·퇴식구 패널은 수정하지 않았다.


## 2026-11-12 일러스트 교체 (샷 내리기 창 스타일)

- 도구: Python 3.13 + Pillow 12 + numpy 2.2 로 도형을 2~3배 슈퍼샘플링해 직접 그린 절차적 일러스트(생성형 이미지 도구 없음). Lanczos 로 축소하고 알파 6 미만 헤이즈를 지웠으며, 완전 투명 픽셀의 RGB 는 0. 제빙기 창(`../IceMaker/Generation.md` 2026-11-12)과 같은 스크립트·재질 함수를 썼고, 작업 스크립트(`draw_dish.py`)와 미리보기는 Assets 밖 scratchpad 에만 두었다.
- 스타일 참조: `docs/ART_STYLE.md` 1장의 영업 중 기기 작업 창 예외, `../ShotRoom/Panel.png`(브러시드 스테인리스), `../Water/Panel.png`·`HotZone.png`(작업대 배경, 크롬 수전, 밝은 오크 띠), `../IceMaker/` 2026-11-12 그림(차콜 외곽선·블루 포인트). 이번 기록이 위의 과거 도트 생성 지시보다 우선한다.
- 공통 지시: “샷 내리기·정수기·제빙기 창과 같은 부드러운 2D 일러스트. 차콜 #2B292C 외곽선, 중립 브러시드 스테인리스와 크롬, 밝은 오크 띠, 왼쪽 위 빛. 도트 아님, 사진 아님, 글자 없음, 투명 배경.”

### 파일별 지시·후처리

| 파일 | 지시 / 후처리 | 최종 PNG 크기 |
|---|---|---|
| Panel.png | `../Water/Panel.png`(2200×1400)을 높이 1800 으로 Lanczos 확대(2829 폭)한 뒤, 좌우 1200px 은 그대로 두고 가운데 429px 띠만 800px 로 늘여 3200 폭을 만들었다. 벽(안쪽 테두리 안, 작업대 윗면 위) 에만 서브웨이 타일(132×66px, 줄눈 5px, 반 칸 엇갈림, 위 모서리 밝게)을 55% 덧칠해 설거지실 벽으로 했다. 왼쪽 위 빛줄기는 유지. 내부 알파 255. | 3200×1800 |
| Sink.png | “앞에서 약간 위로 본 스테인리스 싱크대: 사다리꼴 상판, 그 안의 개수대(뒷벽 그늘, 바닥에 고인 연한 하늘색 물과 반사 줄, 배수구), 상판 뒤 오른쪽의 크롬 구스넥 수전(레버, 출수구 끝이 개수대 가운데 x 340·y 116), 앞 모서리 오크 띠, 문짝 홈 두 줄과 크롬 손잡이가 있는 앞판.” 표시 680×560. | 1360×1120 |
| SinkFront.png | Sink.png 에서 표시 y 392 아래(앞 테두리·오크 띠·앞판)만 남기고 나머지를 투명하게 한 덮개. 잔 아래끝이 앞 테두리 뒤에 잠겨 보이게 잔 위에 겹친다. | 1360×1120 |
| WaitingCrate.png | “위에서 비스듬히 본 연한 청회색 플라스틱 설거지 바구니. 둥근 테두리, 어두운 안쪽, 손잡이 구멍과 세로 골이 있는 앞벽.” 잔은 그리지 않는다(게임이 대기 잔 아이콘을 안에 넣는다). 표시 340×230. | 680×460 |
| WaitingCrateFront.png | 같은 바구니의 앞벽·앞 테두리만 그린 덮개 (앞벽 윗면 y 118). 바구니 안 잔 아래쪽을 가린다. | 680×460 |
| DryingRack.png | “크롬 철사 2단 건조대: 양쪽 기둥과 위 손잡이 레일, 각 단에 뒷레일·바닥 철사·칸막이 고리 2개·앞레일, 아래 오크 물받이.” 앞레일 윗면 y 176 / 356, 칸 가운데 x 70 / 190 / 310 (표시 380×400). 잔은 그리지 않는다. | 760×800 |
| Sponge.png | “노란 스펀지(구멍 무늬) + 초록 수세미 층, 위에 비누 거품 몇 덩이.” 표시 170×110 캔버스를 150×97 로 표시. | 340×220 |
| Suds.png | “흰 비누 거품 더미: 원 26개를 겹치고 아래쪽에 연한 청회색 그늘, 흰 반사점, 연한 청회색 외곽선. 가운데가 높은 둔덕.” 잔 아래쪽에 겹쳐 문지를수록 진해진다. | 340×220 |
| Bubble.png | “비누 방울 하나: 거의 투명한 안쪽, 무지개빛(분홍·하늘·청록) 테두리, 왼쪽 위 흰 반사 호, 작은 반사점.” | 128×128 |
| Sparkle.png | “네 갈래 별 반짝임: 흰색·연노랑, 부드러운 빛무리.” | 128×128 |
| LabelWaiting.png | 생성형 원화 없음. 정확히 `설거지 대기`. 프로젝트의 `Art/Fonts/Galmuri11.ttf` 를 원래 크기(12px)로 그린 뒤 5배 NEAREST 확대(채움 높이 55px), 다크 커피 #380F00 채움 / 크림 #FCF0CA 3px 외곽선, 사방 투명 여백 8px. | 337×77 |
| LabelDrying.png | 위와 같은 방법으로 정확히 `건조 중`. | 222×77 |

- `Stain.png`, `Hint.png` 는 수정하지 않았고 화면에서 쓰지 않는다. 얼룩은 같은 자리에 겹친 더러운 잔 아이콘(`Icons/Items/Dirty*.png`)의 투명도로 표시한다.
- 헹굼 물줄기는 `../Water/WaterStream0~7.png` 를 그대로 쓴다.
- 두 라벨의 한글 철자·띄어쓰기를 확인했다. `중` 의 ㅜ 세로획은 Galmuri11 원래 모양(1px → 5px)이다.

### 배치 (2026-11-12)

1600×900 창의 왼쪽 위를 (0,0)으로 하는 **화면 px**. 배경의 작업대 윗면은 y 576~635, 앞 모서리 635~671, 아래 수납장 판은 y 674~869 이다. `StationUiBuilder.BuildDishwash` 가 이 표를 따른다.

| 요소 | x | y | w | h | 비고 |
|---|---:|---:|---:|---:|---|
| WaitingCrate (+Front) | 60 | 385 | 340 | 230 | 바닥 y≈607, 작업대 위 |
| 대기 잔 3개 | 바구니 기준 가운데 (85,85) (170,85) (255,85) | | 130 | 130 | 8° / -6° / 10° 기울임 |
| LabelWaiting | 가운데 (230, 690) | | | | |
| Waiting 글자 (TMP) | 130 | 712 | 200 | 48 | “3개” |
| Sink (+SinkFront) | 450 | 300 | 680 | 560 | 작업대 앞에 선 싱크대 |
| 싱크 잔 | 싱크 기준 가운데 (340, 312) | | 220 | 220 | 바닥이 앞 테두리(y 398) 뒤 |
| Suds | 잔 가운데 기준 (0, -42) | | 190 | 123 | |
| Rinse (물줄기) | 싱크 기준 322 | 116 | 36 | 190 | 수전 끝에서 잔 안까지 |
| Sponge | 싱크 기준 가운데 (400, 388.5) | | 150 | 97 | 좌우 ±170, 위아래 ±24 따라 움직임 |
| DryingRack | 1160 | 215 | 380 | 400 | 물받이 아래끝 y≈609 |
| 건조대 칸 6개 | 건조대 기준 가운데 x 70/190/310, y 155(윗단)/335(아랫단) | | 110 | 110 | 180° 회전(거꾸로 엎음) |
| LabelDrying | 가운데 (1350, 690) | | | | |

- 모든 요소는 닫기 버튼 예약 영역 (1500,36,64,64)과 겹치지 않는다.
- Unity 임포트: 새 파일(`SinkFront`, `WaitingCrateFront`, `Suds`, `Bubble`, `Sparkle`)은 Unity 가 `.meta` 를 만든다. `CafeTime/Build Cafe Scene` 이 `StationUiBuilder.PrepareAll` 로 이 폴더를 Sprite / Single / **Bilinear** 로 맞춘다.

## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| Hint.png | 수세미를 좌우로 끌어 얼룩을 지우세요 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 924×78 → **952×78 (폭 확대)** |
| LabelDrying.png | 건조 중 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 206×78 → **208×78 (폭 확대)** |
| LabelWaiting.png | 설거지 대기 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 320×78 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.

## 2026-11-12 main 병합 정리

- LabelDrying·LabelWaiting 은 위 Jua 재조판본을 쓴다 (일러스트 교체 절의 Galmuri11 조판은 ADR-0026 에 따라 대체됨).
