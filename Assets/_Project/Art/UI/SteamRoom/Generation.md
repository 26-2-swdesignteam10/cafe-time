# 우유 스티밍 창 아트·배치 (2026-11-19)

- 날짜: **2026-11-19**.
- 도구: OpenAI 내장 **image_gen.imagegen** 원화 생성·완드 상태 편집. Python 3 / Pillow로 크롭·크기 정규화·알파 정리·Jua 조판·검수 합성.
- 스타일 참조: ../DrinkTools/Panel.png, MilkCarton.png, MilkCartonPour.png, MilkStream0~7.png, WeightPlate.png; ../DrinkRoom/BigCupCeramic.png, EmptyButton.png, Panel.png, Shelf.png; docs/ART_STYLE.md 1·4·9장.
- 기기 작업 창 예외인 **부드러운 일러스트 / Bilinear**, 왼쪽 위 조명, 크림·세이지·따뜻한 오크·스테인리스. ART_STYLE의 같은 예외 목록에 SteamRoom을 추가했다.
- PNG와 배치 설계 납품이다. 런타임·빌더·씬·프리팹·GameBalance·메뉴/레시피는 변경하지 않았다. 새 ADR 없음(요청된 아트·배치 사양 이행). 기존 PNG를 덮어쓰거나 .meta를 직접 만들지 않았다.

## 조작

사용자가 고른 **피처에 우유 붓기 → 스팀 → 컵에 붓기** 순서를 하단 왼쪽부터 배치했다. **글자가 있는 버튼 자체를 길게 누른다.** 도구 그림은 상태 표현이며 누름 영역이 아니다. 우유팩·피처가 기울어도 버튼 위치는 유지한다.

- MilkButton: 누르는 동안 피처에 우유 투입. 기존 기울인 우유팩과 기존 MilkStream 사용.
- SteamButton: 누르는 동안 완드 노브 열림·증기·온도계 상승. **60~70℃ 띠를 보고 놓는다.**
- PourButton: 누르는 동안 피처를 컵 위로 들어 기울이고 스팀 우유 줄기 표시.
- EmptyButton: 피처 바로 아래 보조 버튼. **짧게 클릭해 피처만 비운다.** 컵 비우기가 아니다.
- 닫기는 기존 공용 X. 창 제목은 넣지 않는다. 요청된 한 줄 안내는 이 새 하위 창에만 둔다.
- 피처 g·온도 ℃·저울 g는 TMP. 납품 PNG에는 예시 값이 없다. 온도계 0~90 눈금과 안내의 60~70은 고정 이미지다.
- 실제 투입/가열 속도·판정 조건·자동 종료 등 게임 수치는 이번 아트 설계에서 정하지 않는다.

## 공통 프롬프트

문서 기본 시작 문장: “Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.”

이번 기기 창에서는 위 픽셀 규칙 대신 다음 예외 프롬프트를 적용했다:

> Use case: stylized-concept. Production Unity sprites for a cozy Korean pixel art cafe game. Reference is style reference only: match smooth softly painted realistic game equipment, NOT pixels. Warm light from upper left, 3/4 view slightly above, thin warm gray outlines, brushed stainless steel with subtle beige reflections, no text, no numbers, no logos. Actual transparent background. Fully visible with padding.

고정 문구는 Jua-Regular.ttf로 조판했다. **℃ 한 글자는 Jua에 글리프가 없어 맑은 고딕 Bold로 조판**했다. 한국어·숫자는 Jua. TMP에도 ℃를 표시할 수 있는 폴백 글리프가 필요하며 폰트 에셋은 이번에 수정하지 않았다.

## 파일별 프롬프트·규격

아래 지시는 공통 스타일에 덧붙인 개별 요청이다. 크기는 최종 PNG 원본 픽셀이다.

| 파일 | 파일별 프롬프트 / 후처리 | 크기 |
|---|---|---|
| Panel.png | “EMPTY UI backdrop matching reference walnut/gold frame, cream wall and honey wood counter. Aspect 11:7. Counter back edge at 64% height, footer recess below 83%. Subtle steam-station feeling from brushed steel backsplash on far LEFT only, no equipment, objects, text, labels or buttons. Same upper-left lighting and border thickness. Fully opaque interior.” RGBA 변환·리샘플링. | 2200×1400 |
| Pitcher.png | “Single milk steaming pitcher. Broad stout conical body, rounded bottom, open oval top, spout RIGHT, handle LEFT. Empty interior clearly visible, no milk. Upright.” 알파 경계 크롭·12px 여백 공통 캔버스. | 512×512 |
| PitcherMilk.png | “Upright pitcher containing creamy white milk with tiny microfoam visible in oval mouth, identical shape to empty version.” 같은 캔버스. 담김 한 장으로 양 표현 생략. | 512×512 |
| PitcherPour.png | “Pitcher rotated clockwise 55 degrees, pouring spout points DOWN RIGHT, milk at spout but no detached stream. Entire object visible.” 컵 위로 들었을 때 별도 작은 사각형으로 표시. | 512×512 |
| SteamWand.png | “Isolated espresso machine steam wand. Tall slim chrome curved pipe from small mounting plate at TOP RIGHT, descends leftward, tip at BOTTOM CENTER. Black heatproof grip, round black valve knob. No machine body, pitcher, steam or text. Closed valve, small brass pointer vertical.” | 304×480 |
| SteamWandOn.png | 닫힌 완드를 편집 대상으로 지정: “Preserve EXACT image size, transparency, chrome pipe geometry, mounting plate, black grip and tip coordinates. ONLY rotate tiny GOLD POINTER on black knob 45 degrees clockwise, pointing up-right. No steam, text or glow.” | 304×480 |
| Steam0.png | “ONE soft white vapor animation sprite, frame 0 of four. Three delicate wispy ribbons rising from bottom center, curling left in compact S curves. Low alpha, fine soft edges dissolving at top. No objects, text, cloud blob or outline.” 알파×0.70·흰 RGB·공통 캔버스. | 256×384 |
| Steam1.png | 같은 프롬프트의 frame 1, “upwards in open curves”. 같은 후처리. | 256×384 |
| Steam2.png | 같은 프롬프트의 frame 2, “slightly right in loose curves”. 같은 후처리. | 256×384 |
| Steam3.png | 같은 프롬프트의 frame 3, “left again with wisps dispersing”. 같은 후처리. | 256×384 |
| SteamedMilkStream0.png | MilkStream0 참조: “SINGLE vertical steamed milk stream for HOT ceramic cup. Narrow continuous cream milk ribbon, foamy edges, tiny bubbles, warm #FFE8C4. No jug, cup, separate droplets or text. Same center and endpoints. Frame 0: smooth narrow.” 중앙 32~68% 크롭·헤이즈 제거·32×192 줄기를 72×192의 x20에 정렬. | 72×192 |
| SteamedMilkStream1.png | 같은 프롬프트의 frame 1, “left edge small ripple”. 같은 정렬. | 72×192 |
| SteamedMilkStream2.png | 같은 프롬프트의 frame 2, “right edge small ripple”. 같은 정렬. | 72×192 |
| SteamedMilkStream3.png | 같은 프롬프트의 frame 3, “slightly fuller creamy ribbon”. 같은 정렬. | 72×192 |
| ThermoFrame.png | “Empty vertical ivory thermometer frame, rounded corners, fine brass rim. Transparent well along LEFT half. RIGHT half empty cream for manually typeset 0 to 90 C ticks. No text, numbers, ticks, bulb or fluid.” Jua로 0~90 숫자·5℃ 보조/10℃ 주 눈금 조판. 60~70℃ 세이지 띠와 투명 채움 창 정규화. | 192×672 |
| ThermoFill.png | “Separate plain warm copper thermometer fill bar, no marks or text.” 단순 채움용이므로 Pillow로 가로 명암만 있는 불투명 막대 제작. 높이를 잘라도 이음새 없음. | 28×1080 |
| MilkButton.png | 공통 버튼 원화: “Match reference EXACT sage green enamel pill, honey gold double rim, bevel, upper-left highlight. One wide empty capsule on transparency, no text or icons.” Jua 58px **우유 붓기** 조판. | 480×128 |
| SteamButton.png | 같은 버튼 원화, Jua 58px **스팀**. | 480×128 |
| PourButton.png | 같은 버튼 원화, Jua 58px **컵에 붓기**. | 480×128 |
| EmptyButton.png | 같은 원화의 보조 버튼, Jua 46px **비우기**. | 300×88 |
| Hint.png | **피처에 우유를 붓고 60~70℃로 스팀하세요**. Jua 56px·다크 커피/크림 외곽선·8px 여백·투명 배경. | 942×72 |
| LabelPitcher.png | **피처**, Jua 56px·같은 라벨 규격. | 106×72 |
| LabelTemperature.png | **온도**, Jua 56px·같은 라벨 규격. | 106×72 |

원화 알파를 보존했으며 줄기 주변 헤이즈는 a'=max(0,a−80)×255/175로 정리했다. 완전 투명 픽셀 RGB는 0. 모든 최종 PNG는 RGBA.

## 스팀 창 배치표

**1100×700, 왼쪽 위 원점.** 기존 재료 작업 창처럼 창 전체를 **1.3배** 표시한다. 아래 좌표에 미리 1.3을 곱하지 않는다. 원본 전체 캔버스의 투명 여백을 포함하며 가시 경계만 자동 재정렬하지 않는다.

| 요소 | x | y | w | h | 비고 |
|---|---:|---:|---:|---:|---|
| Panel | 0 | 0 | 1100 | 700 | 불투명 내부 |
| Stations/Close.png | 1000 | 36 | 64 | 64 | 기존 공용 X |
| Hint | 80 | 66 | 471 | 36 | 한 줄, 1/2 표시 |
| 기존 MilkCarton | 55 | 210 | 360 | 288 | 우유 붓기 이외 |
| 기존 MilkCartonPour | 20 | 112 | 360 | 288 | 우유 붓기 중 교체 |
| 기존 MilkStream0~7 | 326 | 234 | 36 | 78 | 중심 (344,234) → 피처 입구 (344,312) |
| Pitcher / PitcherMilk | 250 | 248 | 248 | 248 | 빈 상태 / 담김 상태 교체 |
| SteamWand / SteamWandOn | 330 | 100 | 152 | 240 | 노브 상태만 교체. 끝 약 (401,326) |
| Steam0~3 | 296 | 112 | 192 | 230 | 스팀 중만 표시 |
| PitcherPour | 610 | 100 | 160 | 160 | 컵 위로 들어 옮긴 표현. 원위치 피처 숨김 |
| SteamedMilkStream0~3 | 743.25 | 190.625 | 36 | 155.375 | 아래 HOT 컵 접점 설명 참고 |
| **기존 HOT 큰 컵** | **616** | **164** | **256** | **320** | DrinkRoom/BigCupCeramic, 위치 고정 |
| **기존 무게 판** | **594** | **445** | **300** | **105** | DrinkTools/WeightPlate, 위치 고정 |
| ThermoFrame | 512 | 170 | 96 | 336 | 컵 왼쪽 |
| ThermoFill 전체 기준 | 540 | 202 | 9 | 270 | 아래→위, 프레임 투명 창으로 클리핑 |
| LabelPitcher | 270 | 499 | 53 | 36 | TMP 왼쪽 |
| LabelTemperature | 510 | 512 | 53 | 36 | TMP 위 |
| MilkButton | 64 | 598 | 240 | 64 | 주 조작 1 |
| SteamButton | 344 | 598 | 240 | 64 | 주 조작 2 |
| PourButton | 624 | 598 | 240 | 64 | 주 조작 3 |
| EmptyButton | 330 | 538 | 150 | 44 | 피처 값 아래 보조 조작 |

### HoldButton / 클릭 사각형

| 동작 | 입력 | x,y,w,h |
|---|---|---|
| 피처에 우유 붓기 | HoldButton | **64,598,240,64** |
| 스팀 | HoldButton | **344,598,240,64** |
| 컵에 붓기 | HoldButton | **624,598,240,64** |
| 피처 비우기 | 일반 클릭 | **330,538,150,44** |
| 닫기 | 일반 클릭 | **1000,36,64,64** |

움직이는 도구·증기·줄기·라벨·온도계는 raycast를 받지 않는다. HoldButton 사이 간격 40, 피처 비우기와 주 버튼 사이 간격 16.

### HOT 컵 줄기·레이어

- PitcherPour 원본 주둥이 약 **(484,290)** → 표시 **(761.25,190.625)**.
- 줄기의 원본 중심 x36을 출구에 맞추고 HOT 잔 입구 안쪽 **(761.25,346)**까지 **155.375** 길이로 표시한다. 컵 캔버스 위쪽의 투명 여백은 입구가 아니다.
- 같은 RectTransform에서 프레임만 교체해 중심과 양 끝점을 유지한다.
- 컵 그림 → 기존 CupCeramicSurface(내용물 색) → 피처·줄기. 줄기는 앞벽을 관통하지 않고 액면 안에서 끝난다.
- 원위치 피처 → 완드 → 증기 순서. 우유팩 줄기 아래끝은 피처 입구에 접한다.
- 프레임 시퀀스 제안: **0,1,2,3,2,1** 양방향. 생성 프레임의 루프 경계를 완화하는 표시 지침이며 가열 수치를 정하지 않는다.

### 온도계

- 채움 전체 사각형 **x540~549, y202~472**. 프레임 투명 창 안에 놓는다.
- **0℃ = y472 (아래), 90℃ = y202 (위)**. y(T)=472−3×T, 높이=270×clamp(T/90,0,1).
- **60℃ = y292, 70℃ = y262**. 강조 띠 y262~292.
- 막대는 아래 앵커/피벗으로 세로 fill 또는 클리핑. ThermoFrame을 위에 고정.
- 프레임 원본 기준점 **y604 (0), y64 (90)**. 주 눈금 60px(표시30), 보조 30px(표시15).
- 검수용 65℃ 채움은 y277~472로 적정 띠에 들어간다.

### TextMeshPro 동적 값

모두 Jua SDF·가운데 정렬·자동 크기 조정 끔·줄바꿈 끔. 글자 크기는 1.3배 확대 전 기준. **℃ 글리프 폴백 필요**.

| 값 | x | y | w | h | 글자 크기 | sRGB 색 | 검수 예 |
|---|---:|---:|---:|---:|---:|---|---|
| 피처 우유 g | 325 | 492 | 165 | 38 | 28 | #5A3520FF | 180 g |
| 온도 ℃ | 510 | 552 | 100 | 36 | 28 | #5A3520FF | 65 ℃ |
| 컵 저울 g | 690 | 508 | 108 | 22 | 20 | #3B2A20FF | 120 g |

동적 숫자는 검수 미리보기에만 합성했다.

## 음료 제조실 진입 버튼

**1600×900, 왼쪽 위 원점.**

| 요소 | 경로 | x | y | w | h |
|---|---|---:|---:|---:|---:|
| 버튼 및 일반 클릭 영역 | ../DrinkRoom/SteamButton.png | **1080** | **80** | **330** | **90** |
| 가운데 라벨 | ../DrinkRoom/LabelSteam.png | 1178 | 103 | 134 | 44 |

- 버튼 원본 660×180. 위 파일별 공통 버튼 프롬프트와 같은 원화. 기존 EmptyButton을 스타일 참조로 생성한 세이지/금테 캡슐.
- 라벨 원본 268×88. **스팀 피처**, Jua72px·다크 커피/크림 외곽선·투명 PNG. 버튼 중심 **(1245,125)**에 1/2 크기로 표시.
- 선반 (100,200,920,660), 컵·저울 조작 영역 (1110,184,360,486), 비우기 (1050,712,220,80), 완성 (1290,707,260,90) 유지.
- 버튼 아래끝 y170과 컵·저울 위끝 y184 사이 **14px**. 버튼 오른끝 x1410과 닫기 x1500 사이 **90px**. 선반 오른끝 x1020으로 가로도 겹치지 않는다.

## 내용물 색

docs/ART_STYLE.md 9장에 **milk_steamed = #FFE8C4FA** 추가. 기존 milk #FFF4DEFA보다 따뜻한 크림이다. 검수에서는 기존 CupCeramicSurface의 알파로 합성했다. 새 컵 그림·마스크·런타임 재료 등록은 만들지 않았다.

## 미리보기·검수

저장 폴더: C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/3c877158-9d17-4156-bff5-e3db1a17f799/scratchpad/steam/

- **SteamRoomPreview.png**: 3300×744. 1100×700 창 3개 가로 배치. 위44px는 검수용 상태 제목이며 창 좌표에 포함하지 않는다. 왼쪽 우유 붓기 / 가운데 스팀 / 오른쪽 컵에 붓기.
- **DrinkRoomPreview.png**: 1600×900. 기존 패널·선반·컵·저울·하단 버튼과 새 진입 버튼 합성. 선반 재료 아이콘은 생략. 새 버튼은 선반 전체 영역 바깥이다.
- 보조 확대본 SteamRoomDetail.png: 1100×700.
- 검수 합성 build_art.py와 원화 대응 sources.json도 같은 임시 폴더. 게임 임포트 대상이 아니다.
- 육안 확인/수정: 컵·저울 고정 좌표, 붓는 피처가 온도계를 가리던 배치, ℃ 누락 글리프, “우유 붓기 / 스팀 / 컵에 붓기 / 비우기 / 스팀 피처” 철자, 눈금과 채움 기준, 프레임 알파·크기.
- 자동 검사 통과: PNG 총25개(SteamRoom23개 + DrinkRoom2개) 모두 RGBA, 패널 내부·채움 막대 불투명, 나머지 스프라이트 투명 영역 존재, 각 애니메이션4장 크기 일치 및 서로 다른 프레임. 보조 FramesContactSheet.png에서도 증기와 줄기를 육안 확인했다.
- bash .github/scripts/check-repo.sh 통과(추적 파일2281개), 변경 문서 git diff --check 통과. 저장소 스크립트는 추적 파일 기준이므로 새 파일의 .meta 검증은 Unity 임포트 후 수행해야 한다.

## Unity에서 이어 할 일

1. **Unity 6000.6.4f1**로 돌아가 새 파일을 임포트한다. Unity가 폴더·PNG·Generation.md의 .meta를 만들게 한다.
2. Sprite / Single / Full Rect / **Bilinear** / Mip Maps 끔 / Alpha Is Transparency 켬 / 압축 없음. Panel은 Max Size4096(2200px 폭 보존), 나머지는2048.
3. 후속 구현에서 이 표대로 빌더에 배치하고 동적 값은 TMP, ℃는 폴백 글리프를 설정한다. 이번 작업은 코드/씬 연결을 포함하지 않는다.
4. Play에서 1.3배 표시, 누름/놓음, 프레임 교체, 0~90℃ 채움, 60~70℃ 띠, HOT 컵 줄기 접점, 피처 비우기·닫기를 확인한다.
