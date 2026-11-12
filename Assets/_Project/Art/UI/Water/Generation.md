Generated with OpenAI 내장 ImageGen 도구. Style reference: `../Pos/Panel.png`, `../Stations/PickupPanel.png`, `../Stations/PickupSlot.png`, `../Pos/PayButton.png`, `../Pos/ConfirmButton.png`, `../Pos/DeleteButton.png`, `../CafeHud/ToastNoCustomer.png`, `../Quit/Message.png`, `../../Icons/Items/HotCoffee.png`, `../../Sprites/Cafe/StationEspresso.png`.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- ColdZone.png: 정면 정수기 구역, 상단 중앙 노즐, 하단 받침대. 파란색 강조와 물방울, 내부는 비움.
- Hint.png: 단일 행 굵은 도트 글자 전용 라벨, 다크 커피 채움과 크림 외곽선. 정확한 문구: "커피잔을 끌어다 놓고 물 받기를 누르세요".
- HotZone.png: 정면 정수기 구역, 상단 중앙 노즐, 하단 받침대. 빨간색 강조와 물방울, 내부는 비움.
- LabelCold.png: 단일 행 굵은 도트 글자 전용 라벨, 다크 커피 채움과 크림 외곽선. 정확한 문구: "차가운 물".
- LabelHot.png: 단일 행 굵은 도트 글자 전용 라벨, 다크 커피 채움과 크림 외곽선. 정확한 문구: "뜨거운 물".
- Panel.png: 호두나무 프레임과 금색 안쪽 선. 상단 중앙 제목과 커피콩 두 개. 나머지 내부는 빈 불투명 크림색. 정확한 문구: "정수기".
- WaterButton.png: 허니 앰버 베벨 나무 버튼. 정확한 문구: "물 받기".


## 최종 후처리 및 검수 (2026-11-04)

- 도구: Python + Pillow. NEAREST 크기 조정, 알파 노이즈 정리, 투명 여백 보정. .meta는 생성·수정하지 않았다.
- 라벨: 원본 채움 마스크를 추출해 S=56px, M=72px, L=96px로 정규화했다. 채움/외곽선은 #380F00/#FCF0CA 단색, 외곽선 3px, 사방 투명 여백 8px이다. Ending은 LIGHT 반전색. 줄바꿈 없이 기존 단일 행을 유지했다.
- 패널 실루엣 안의 알파를 255로 채우고 빈 내부를 #FAEAC2로 평탄화했다. 바깥은 투명. 제목과 커피콩을 유지했다.
- Hint.png: 내장 ImageGen 재생성. 참조: CafeHud/ToastNoCustomer.png. 프롬프트: 단일 행 "커피잔을 끌어다 놓고 물 받기를 누르세요", 굵은 도트 글자, 다크 커피/크림, 발광·그림자·배경·장식 없음. 결과에 위 라벨 후처리를 적용했다.
- HotZone/ColdZone: 받침대 맨 위 윤곽을 PNG y=660(표시 y=330)으로 정렬했다.
- 이미지의 문구를 글자별로 대조해 철자·공백·문장부호를 눈으로 검수했다. 글자가 없는 파일은 해당 없음. 아래 높이는 외곽선을 제외한 채움 영역 경계 측정값.

| 파일 | 최종 PNG 크기 | 라벨 글자 높이 | 철자 확인 |
|---|---|---|---|
| ColdZone.png | 760×760 | — | 해당 없음 |
| Hint.png | 873×78 | 56px | 예 |
| HotZone.png | 760×760 | — | 해당 없음 |
| LabelCold.png | 268×78 | 56px | 예 |
| LabelHot.png | 254×78 | 56px | 예 |
| Panel.png | 2200×1400 | — | 예 |
| WaterButton.png | 440×144 | — | 예 |


## 2026-11-11 패널 제목·커피콩 제거

- 도구: Python 3 / Pillow. 단순 지우기·채우기 후처리이며 새 원화 생성은 없다.
- 스타일 참조: 이 폴더의 기존 `Panel.png`, `docs/ART_STYLE.md`. ADR-0019(승인)의 기기 창 제목 제거 요청을 적용했다. 새 ADR은 없다.
- `Panel.png` 작업 지시: “상단 중앙의 기존 제목과 양옆 커피콩만 지우고 주변 크림 바탕으로 이음매 없이 복원한다. 원래 프레임·금테·나머지 픽셀·캔버스 크기·알파를 보존한다.”
- 후처리: 원본 px 기준 사각형 `(623, 138, 1576, 321)`(오른쪽·아래 끝 제외) 안에서만 수정했다. 주변과 같은 불투명 크림 #FAEAC2로 채웠다. 이 사각형 밖 RGBA 픽셀이 원본과 완전히 일치하는지 검사했다. 전체 알파와 파일 크기도 원본과 같다.
- 결과: Panel.png — 2200×1400px. 지운 제목 외 새 글자는 없다.
- `.meta`, 코드, 씬, 프리팹, 에셋 정의는 수정하지 않았다. 픽업대·퇴식구 패널은 수정하지 않았다.

## 2026-11-12 일러스트 교체

- 도구: OpenAI 내장 `image_gen.imagegen`(배경, 빨간 정수기, 파란 정수기 편집, 버튼, 물줄기 8프레임 시트), Python 3 / Pillow(크롭·Lanczos 리샘플링·공통 몸체 합성·투명 픽셀 정리·Galmuri11 라벨 조판·미리보기 합성).
- 스타일 참조: `docs/ART_STYLE.md` 1장의 영업 중 기기 창 예외, `../ShotRoom/Panel.png`, `../ShotRoom/Generation.md`의 공통 스타일과 ExtractStream0~7 규격, `../DrinkRoom/Panel.png`, `../DrinkRoom/CompleteButton.png`, `../DrinkRoom/EmptyButton.png`, `../DrinkRoom/Generation.md`의 2026-11-11 큰 컵 규격. 이번 기록이 위의 과거 도트 생성 지시보다 우선한다.
- 공통 프롬프트: “Premium soft hand-painted 2D illustration for a cozy Korean cafe game. Clean charcoal contours, neutral brushed stainless silver, light oak, warm greige wall, restrained sage enamel, soft upper-left light. Match ShotRoom and DrinkRoom materials. Not pixel art, not photographic. Fully visible edges, transparent background for isolated sprites, no extra text.”
- 생성 원본은 `C:/Users/k9425/.codex/generated_images/01a1ce2d-ff03-7640-83e2-0a6003fe8adf/`에 보존했다. 작업 스크립트와 미리보기는 Assets 밖 scratchpad의 `water-20261112/`에만 두었다.

### 파일별 프롬프트·후처리

| 파일 | 개별 프롬프트 / 후처리 | 최종 PNG 크기 |
|---|---|---|
| Panel.png | “Empty illustrated cafe workbench background, aspect 11:7. Warm walnut rounded frame, narrow gold inset, warm greige plaster wall, light oak counter, opaque interior. No machines, cups, text, heading, buttons, decor or close icon; keep upper-right corner empty.” 원본 `exec-44afd0c7-7c31-457e-8970-ff70f6efbe23.png`을 Lanczos로 리샘플링하고 전체 알파 255의 RGBA로 저장. | 2200×1400 |
| HotZone.png | “Single hot water dispenser, neutral brushed stainless body, muted RED enamel cap and small red water-drop emblem. Front-facing slightly elevated camera. Compact top housing, ONE central downward chrome nozzle, exceptionally large empty recessed cup bay with narrow side columns, wide flat dark rubber drip tray and feet. No cup, stream, steam, text or external background. True transparency.” 원본 `exec-c29edc36-f325-424b-9879-aa28cada71ba.png`(1254×1254)을 760×760으로 축소한 뒤 출수구 중심 정렬을 위해 오른쪽 8px 이동. | 760×760 |
| ColdZone.png | “Change ONLY the hot dispenser's red enamel cap and red droplet emblem to muted cool BLUE enamel. Preserve identical silhouette, scale, nozzle, empty bay, stainless, oak trim, tray, feet and lighting. Transparent; no cup, stream, steam or text.” 편집 원본 `exec-1c8c7e10-83ab-4569-82cb-ba454da6c1ed.png`. HOT 원화와 같은 몸체를 쓰고 편집 원화의 상단 캡(y0..180)과 물방울(타원 x574..652, y198..292)만 합성했다. 동일한 축소·이동을 적용해 컵·물줄기 접점을 공용화. | 760×760 |
| WaterButton.png | “Single illustrated sage enamel button matching DrinkRoom CompleteButton, honey oak slim rounded beveled rim, cream upper-left highlight. EXACT centered Korean text ‘물 받기’, bold square Galmuri11-like lettering, cream fill and dark coffee outline. No icons, ornaments or extra text.” 원본 `exec-57fc727f-8bb4-49b7-956b-920b23c7fc33.png`의 투명 외곽을 크롭하고 424×128로 축소해 440×144 캔버스 (8,8)에 배치. 문구는 생성 원화 그대로이며 실제 표시에서 `물 받기`의 철자·공백·가운데 정렬 확인. | 440×144 |
| LabelHot.png | 생성형 원화 없음. “정확히 ‘뜨거운 물’, 한 줄 Galmuri11, 다크 커피 #380F00 채움 / 크림 #FCF0CA 3px 외곽선, 배경·장식 없음.” 기존 프로젝트 `Galmuri11.ttf`로 조판, 채움 높이 56px, 사방 투명 여백 8px. | 289×78 |
| LabelCold.png | 생성형 원화 없음. 위 라벨과 동일한 규격으로 정확히 `차가운 물` 조판. | 289×78 |
| WaterStream0.png ~ WaterStream7.png | “ONE eight-frame animation sprite sheet, eight evenly spaced columns. The SAME slender continuous clear water thread, fixed center-top nozzle anchor and center-bottom small elliptical surface ripple. Pale transparent blue, traveling white highlights advancing down by one eighth cycle, seamless frame7-to-frame0. Soft cafe illustration, no steam, cups, faucets, text or dividers; true alpha.” 원본 `exec-0677eccc-52a9-4c17-b042-24e9e4e9e9c9.png`(2048×768)을 256px 폭 8셀로 분리. 각 셀 공통 y6..752를 크롭해 72×192로 축소. 마지막 위상 6·7을 첫 위상과 각각 25%·60% 블렌딩해 반복 경계를 완화. 공통 위쪽 줄기 경계 x31..41, y0이며 아래 수면 파문까지 이어진다. | 각각 72×192 (8장) |

- 투명 스프라이트는 알파 12 미만의 생성 헤이즈를 제거하고 완전 투명 픽셀의 RGB도 0으로 정리했다. 부드러운 반투명 가장자리와 물줄기의 투과감은 유지했다. 패널 내부는 완전 불투명이다.
- 컵은 `../DrinkRoom/BigCup*.png` 원본을 수정하지 않고 그대로 사용한다. HOT은 받침과 오른쪽 손잡이가 있는 도자기 잔, ICE는 투명 플라스틱 컵이다. 김은 기존 `BigCupCeramicWater.png` 안에 있는 것만 표시하며 물줄기에는 넣지 않았다.
- `Hint.png`는 수정하지 않았으며 이번 미리보기에도 표시하지 않는다. 새 ADR은 없다. 기존 승인 ADR-0019의 제목·안내 문구 제거와 기존 일러스트 스타일 예외를 따른다.

### 배치 (2026-11-12)

1100×700 창의 왼쪽 위를 (0,0)으로 하는 **화면 px**. x/y는 전체 PNG 캔버스 표시 영역의 왼쪽 위이며, w/h는 표시 크기다. 라벨은 가운데 좌표도 별도로 명시했다. 표는 제안 배치이며 코드·씬에는 적용하지 않았다.

| 요소 | x | y | w | h | 가운데 좌표 / 정수기 그림 기준 상대 좌표 |
|---|---:|---:|---:|---:|---|
| HotZone | 135 | 155 | 380 | 380 | 정수기 그림 = 놓는 자리 |
| ColdZone | 585 | 155 | 380 | 380 | 정수기 그림 = 놓는 자리 |
| Cup_Hot | 235 | 238 | 192 | 240 | HotZone 상대 (100,83,192,240) |
| Cup_Cold | 679 | 238 | 192 | 240 | ColdZone 상대 (94,83,192,240) |
| Stream_Hot | 307 | 292 | 36 | 84 | HotZone 상대 (172,137,36,84), 꼭지 (325,292) → 수면 약 (325,375) |
| Stream_Cold | 757 | 292 | 36 | 60 | ColdZone 상대 (172,137,36,60), 꼭지 (775,292) → 수면 약 (775,351) |
| LabelHot | 252.75 | 80.5 | 144.5 | 39 | **가운데 (325,100)** |
| LabelCold | 702.75 | 80.5 | 144.5 | 39 | **가운데 (775,100)** |
| HotButton | 215 | 555 | 220 | 72 | 가운데 (325,591) |
| ColdButton | 665 | 555 | 220 | 72 | 가운데 (775,591) |
| HotProgress | 215 | 641 | 220 | 24 | 버튼 아래 14px 간격 |
| ColdProgress | 665 | 641 | 220 | 24 | 버튼 아래 14px 간격 |

- 패널은 원본의 절반 크기로 표시한다. 정수기·버튼·라벨도 0.5배다. 컵은 512×640을 0.375배로 표시해 192×240이다. 기존 약 128px보다 크게 보이며 실제 실루엣 폭은 도자기 약 168px, 플라스틱 약 135px이다.
- 컵 원본의 바닥 경계 y632는 `238 + 632×0.375 = 475px`다. 정수기의 받침 중앙 윗면도 화면 y≈475px이며 양쪽 바닥이 그 위에 닿는다. 컵 캔버스 아래 투명 여백은 3px다. HOT은 손잡이 때문에 입구 중심이 캔버스 중심보다 왼쪽에 있어 ICE보다 캔버스를 오른쪽 6px 더 옮겼다.
- 물줄기는 같은 72×192 프레임을 양쪽에 쓰되 컵 안 수면 높이에 맞춰 표시 높이를 각각 84px / 60px로 조절한다. 배경 → 정수기 → 컵 → 물줄기 순으로 그린다. 출수구 안쪽에서 시작해 컵 전면 테두리 위로 흐르지 않고 물 안에서 끝난다.
- 반복 순서 0→1→2→3→4→5→6→7→0, 프레임당 **0.1초(10fps)**의 0.8초 루프를 미리보기에서 사용했다. 이는 에셋 확인용 권장값이다. 실제 적용 시 물 받는 동안만 표시하고 시작할 때 프레임 0으로 맞춘다.
- 모든 영역은 창 안쪽 테두리 안에 있으며 닫기 버튼 예약 영역 (1000,36,64,64)과 겹치지 않는다. 진행 게이지는 기존 CafeHud 그림을 미리보기에서만 220×24로 합성했으며 수정하지 않았다.

### 미리보기·검수·Unity 적용

- Assets 밖 미리보기 폴더: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/fec95591-d3a3-47b1-89d7-0c4e1896d871/scratchpad/water-20261112/`.
- `LayoutHotWater.png` / `LayoutColdWater.png`: 각각 HOT/ICE 물 받기 상태의 1100×700 실제 크기 합성. `HotContact4x.png` / `ColdContact4x.png`: 꼭지·수면·받침 접점 확대. `WaterFrames.png`: 8프레임 비교. `WaterLoop.gif`: 두 구역 공용 애니메이션 반복 미리보기. 닫기 버튼 모양은 예약 영역 확인용 임시 표시이며 에셋에 굽지 않았다.
- PNG의 RGBA·크기·투명 영역·동일 프레임 캔버스와 위끝 앵커, 패널 내부 알파 255, 컵 받침 접촉, 닫기 버튼 회피, 세 문구의 한글 철자와 공백을 확인했다.
- Unity **6000.6.4f1**로 돌아가 PNG를 임포트한다. `.meta`는 Unity가 생성하도록 둔다. 패널·정수기·버튼·물줄기는 Sprite / Single / **Bilinear** / Mip Maps 끔 / Alpha Is Transparency 켬. 패널은 Max Size **4096**. 글자 라벨은 Galmuri11 형태 보존을 위해 Point 권장.
- 이번 작업에서는 `.meta`, `.cs`, `.unity`, `.prefab`, `.asset`을 생성·수정하지 않았다. 새 프레임의 연결과 이 표의 배치 적용, Play 상태에서 물 받기 중에만 애니메이션 표시되는지 확인하는 작업은 별도 Unity 반영이 필요하다.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.
- `WaterButton.png`: 내장 ImageGen으로 `물 받기` 글자와 외곽선만 지운 세이지 바탕을 생성하고, 중앙 글자 영역만 원래 버튼에 사용했다. 나무 테두리·하이라이트·캔버스·원래 알파는 보존했다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| Hint.png | 커피잔을 끌어다 놓고 물 받기를 누르세요 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 873×78 → **1036×78 (폭 확대)** |
| LabelCold.png | 차가운 물 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 289×78 |
| LabelHot.png | 뜨거운 물 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 289×78 |
| WaterButton.png | 물 받기 | 46px | #FCF0CA | #380F00 3px | ImageGen 글자 없는 바탕 인페인트(중앙 영역만 사용) + Pillow Jua 조판 | 440×144 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
- 내장 ImageGen 편집 프롬프트 요약: “Remove ONLY the existing central Korean letters and their outlines; restore continuous matching parchment/sage material. Preserve original silhouette, framing, ornaments, highlights, layout and transparent exterior. No new text; blank base for exact Jua typesetting.”

- `WaterButton.png` 내장 ImageGen 실제 편집 프롬프트: “Use case: precise-object-edit. Remove only the centered Korean text 물 받기 and its dark outlines from this sage enamel button, restoring continuous softly textured muted sage enamel in that exact area. Keep the original wide aspect ratio, wood and gold rim, highlights, rounded silhouette, dimensions/composition and transparent exterior unchanged. No letters or new markings. Blank base for deterministic Jua font typesetting.”
- 인페인트 바탕 원본: `C:/Users/k9425/.codex/generated_images/01a1cecf-f5f9-7c41-8144-696f513874a7/exec-d4023f99-f7ff-41e1-9aee-5490c8a315b7.png`. 최종 PNG에는 필요한 글자 영역만 사용했다.
