Generated with OpenAI 내장 ImageGen 도구. Style reference: `../Pos/Panel.png`, `../Stations/PickupPanel.png`, `../Stations/PickupSlot.png`, `../Pos/PayButton.png`, `../Pos/ConfirmButton.png`, `../Pos/DeleteButton.png`, `../CafeHud/ToastNoCustomer.png`, `../Quit/Message.png`, `../../Icons/Items/HotCoffee.png`, `../../Sprites/Cafe/StationEspresso.png`.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- CeramicButton.png: 크림 나무 카드 위쪽에 흰 도자기 잔과 받침. 아래쪽 문구, 하단 수량용 100px 빈 공간. 정확한 문구: "도자기 잔 (HOT)".
- Panel.png: 호두나무 프레임과 금색 안쪽 선. 상단 중앙 제목과 커피콩 두 개. 나머지 내부는 빈 불투명 크림색. 정확한 문구: "커피잔 선반".
- PlasticButton.png: 크림 나무 카드 위쪽에 투명 플라스틱 컵. 아래쪽 문구, 하단 수량용 100px 빈 공간. 정확한 문구: "플라스틱 컵 (ICE)".


## 최종 후처리 및 검수 (2026-11-04)

- 도구: Python + Pillow. NEAREST 크기 조정, 알파 노이즈 정리, 투명 여백 보정. .meta는 생성·수정하지 않았다.
- 라벨: 원본 채움 마스크를 추출해 S=56px, M=72px, L=96px로 정규화했다. 채움/외곽선은 #380F00/#FCF0CA 단색, 외곽선 3px, 사방 투명 여백 8px이다. Ending은 LIGHT 반전색. 줄바꿈 없이 기존 단일 행을 유지했다.
- 패널 실루엣 안의 알파를 255로 채우고 빈 내부를 #FAEAC2로 평탄화했다. 바깥은 투명. 제목과 커피콩을 유지했다.
- 이미지의 문구를 글자별로 대조해 철자·공백·문장부호를 눈으로 검수했다. 글자가 없는 파일은 해당 없음. 아래 높이는 외곽선을 제외한 채움 영역 경계 측정값.

| 파일 | 최종 PNG 크기 | 라벨 글자 높이 | 철자 확인 |
|---|---|---|---|
| CeramicButton.png | 600×600 | — | 예 |
| Panel.png | 1800×1120 | — | 예 |
| PlasticButton.png | 600×600 | — | 예 |


## HOT 컵 / ICE 컵 버튼 리디자인 (2026-11-09)

- 도구: ImageGen으로 생성한 새 버튼 바탕과 새 도자기·플라스틱 잔 아이콘을 Pillow로 합성하고, Galmuri11.ttf로 라벨을 넣었다.
- 참고: ShotRoom Panel.png의 부드러운 카페 일러스트 질감. 기존 컵 선반 Panel.png는 변경하지 않았다.
- CeramicButton.png와 PlasticButton.png는 각각 600×600 RGBA. 위쪽에 컵 그림, 아래에 정확한 라벨 `HOT 컵` / `ICE 컵`을 배치하고, 맨 아래 100px를 수량 표시용 빈 공간으로 남겼다.
- 글자 채움은 #380F00, 외곽선은 #FCF0CA. 실제 UI 배치 크기 300×300으로 시각 확인했다.
- PNG .meta와 코드·UI 정의는 수정하지 않았다.


## 2026-11-09 HOT 컵·ICE 컵 공통 몸체 일러스트 정리

- 도구: OpenAI 내장 `image_gen.imagegen`(컵 원화 8장, 공용 빈 버튼 바탕 1장), 프로젝트 밖 Python/Pillow 스크립트(내용물 합성, 크기 조정, 조판, 검수).
- 스타일 참조: `docs/ART_STYLE.md` 1장 예외, `Art/UI/ShotRoom/Generation.md`, `ShotRoom/Panel.png`, `ShotRoom/PortafilterFrontEmpty.png`. 기존 컵 PNG는 형태·시점·배치 참조다.
- 공통 프롬프트: "Clean polished hand-painted 2D cafe game illustration, smooth thin charcoal contours, soft upper-left light, coherent highlights, transparent RGBA background, no stepped pixel outlines, no text, labels or logos."
- 생성 원본: `C:/Users/k9425/.codex/generated_images/01a1bf79-baba-73b0-af05-70e08263928b/` 아래 `exec-<ID>.png`.

- 공용 바탕 프롬프트: "Remove only cup and all lettering from existing rounded square cafe button; seamlessly restore warm softly illuminated cream paper center; preserve walnut beveled frame, honey-gold inner trim, upper-left light and transparent corners; completely empty interior, no text." 원본 `exec-a069c48e-d705-43b7-80b5-930b899b2949.png`.
- `CeramicButton.png`: 위 공용 바탕과 새 `CupCeramic.png` 원화를 합성하고 `Art/Fonts/Galmuri11.ttf`로 정확히 `HOT 컵`을 별도 조판했다.
- `PlasticButton.png`: 같은 공용 바탕과 새 `CupPlastic.png` 원화를 합성하고 같은 Galmuri11로 정확히 `ICE 컵`을 별도 조판했다.
- AI 그림에는 글자를 생성하지 않았다. 기존 CeramicButton의 외곽 나무 프레임을 보존하고 생성한 빈 종이 바탕을 기존 컵·글자 영역에만 부드러운 마스크로 합성했다. 두 버튼에 이 공용 바탕을 재사용했다.
- 컵은 아이콘 알파 경계를 잘라 1.5배 LANCZOS 확대하고 아래끝 y=369에 배치했다. 라벨은 80px Galmuri11, 채움 `#380F00`, 외곽선 `#FCF0CA` 3px, 라벨 위끝 y=390이다.
- 둘 다 600×600 RGBA이며 투명 모서리·중간 알파를 유지한다. y=500~599는 수량 표시용으로 컵·글자 없이 바탕과 프레임만 남겼고 공용 바탕 픽셀과 일치함을 검사했다.
- 실제 빌더 배치에 맞춰 기존 Panel을 900×560, 버튼을 각각 `(130,170)`, `(470,170)`에 300×300으로 합성했다. `%TEMP%/cafe-time-illustration-preview/CupShelfActualSize.png`에서 철자와 하단 빈 공간을 육안 검수했다.
- `Panel.png` 원본, `.meta`, 코드, 씬, 프리팹은 수정하지 않았다. 기존 Bilinear 설정을 유지한다.


## 2026-11-11 패널 제목·커피콩 제거

- 도구: Python 3 / Pillow. 단순 지우기·채우기 후처리이며 새 원화 생성은 없다.
- 스타일 참조: 이 폴더의 기존 `Panel.png`, `docs/ART_STYLE.md`. ADR-0019(승인)의 기기 창 제목 제거 요청을 적용했다. 새 ADR은 없다.
- `Panel.png` 작업 지시: “상단 중앙의 기존 제목과 양옆 커피콩만 지우고 주변 크림 바탕으로 이음매 없이 복원한다. 원래 프레임·금테·나머지 픽셀·캔버스 크기·알파를 보존한다.”
- 후처리: 원본 px 기준 사각형 `(458, 117, 1342, 264)`(오른쪽·아래 끝 제외) 안에서만 수정했다. 주변과 같은 불투명 크림 #FAEAC2로 채웠다. 이 사각형 밖 RGBA 픽셀이 원본과 완전히 일치하는지 검사했다. 전체 알파와 파일 크기도 원본과 같다.
- 결과: Panel.png — 1800×1120px. 지운 제목 외 새 글자는 없다.
- `.meta`, 코드, 씬, 프리팹, 에셋 정의는 수정하지 않았다. 픽업대·퇴식구 패널은 수정하지 않았다.

## 2026-11-12 일러스트 선반

- 도구: OpenAI 내장 `image_gen.imagegen`(배경·뒤집힌 잔·플라스틱 컵·겹침 띠 생성, 띠 연결 부분 재편집), Python 3 / Pillow(알파 크롭, Lanczos 리샘플링, 캔버스 정렬, 상태별 미리보기 합성).
- 스타일 참조: `docs/ART_STYLE.md` 1장 영업 중 기기 작업 창 예외, `../ShotRoom/Generation.md`와 `../ShotRoom/Panel.png`, `../Water/Generation.md`의 2026-11-12 일러스트 교체와 `../Water/Panel.png`. 컵 형태는 `../DrinkRoom/BigCupCeramic.png`, `../DrinkRoom/BigCupPlastic.png`를 이미지 참조로 사용했다. HOT은 기존 받침 있는 흰 도자기 잔에서 **잔만** 뒤집었고, ICE는 같은 투명 PET 컵 계열이다. 이 절이 과거 도트·버튼 배치 지시보다 우선한다.
- 공통 프롬프트: “Premium soft hand-painted 2D illustration for a cozy Korean cafe game. Clean fine charcoal contours, warm greige wall, pale honey oak, warm white porcelain and neutral clear PET, soft upper-left light, coherent materials. No pixel art, no photographic finish, no text, numbers, labels or logos. True transparent RGBA for isolated sprites, opaque interior for panel.”
- 생성 원본: `C:/Users/k9425/.codex/generated_images/01a1ce44-719d-73d0-9b2d-1a4433639a9b/`. 전체 프롬프트·합성 스크립트·미리보기는 Assets 밖 scratchpad `cupshelf-20261112/`에 보관했다. 기존 `CeramicButton.png`, `PlasticButton.png`는 변경하지 않았다.

### 파일별 프롬프트·후처리

| 파일 | 프롬프트 / 후처리 | 최종 PNG 크기 |
|---|---|---|
| Panel.png | “EMPTY realistic cafe cup shelf interior. Front-facing orthographic camera with slightly visible shelf tops. Warm walnut rounded frame with narrow gold inset, opaque warm greige wall. LEFT two horizontal floating oak shelves with subtle outer brackets. RIGHT a separate oak counter with short cabinetry below and ample empty wall for a tall plastic cup stack. No cups, dishes, text, labels, decorations, controls or close icon; keep upper-right empty.” 원본 `exec-9821faa4-c547-4536-8d85-052f8ab5d45a.png`(1402×1122)을 2000×1280으로 리샘플링하고 전체 알파를 255로 유지했다. 생성 시 요청한 선반 좌표와 실제 그림의 위치가 달라 **최종 그림의 선반 윗면에 맞춰 아래 배치를 확정**했다. | 2000×1280 |
| HotCupUpside.png | “Preserve reference white porcelain rounded bowl and right loop handle, REMOVE SAUCER and turn CUP UPSIDE DOWN. Mouth faces DOWN and rests level at bottom, smaller solid foot ring faces UP with shallow indentation. Broad drinking rim below and bowl narrows toward foot above. One cup, no saucer, surface, cast shadow, steam or text; transparent background.” 원본 `exec-1015e203-ac88-4b69-b8a5-714eaa6fefab.png`의 `(244,225)-(1232,930)`을 크롭해 208×148로 축소하고 220×200 캔버스 `(6,48)`에 배치했다. 입구 아래끝은 원본 캔버스 y≈195, 아래 투명 여백 4px다. | 220×200 |
| IceCupStackBottom.png | “ONE EMPTY upright clear thin PET cup, mouth up, broad elliptical rolled rim, tapered sides, small flat elliptical base. Match reference slightly elevated frontal view. Transparent walls transmit background, neutral cool gray fine edges and small white highlights; no lid, liquid, straw, logo, hand, floor or shadow. One cup, not a stack.” 원본 `exec-fe6dc7ec-9851-4569-b42e-b31317d34305.png`의 `(153,378)-(963,1394)`를 272×364로 축소해 280×372 캔버스 `(4,4)`에 배치했다. 원본 캔버스 바닥 y≈367, 표시 바닥 y≈442.5로 카운터 윗면 y≈443에 접촉한다. | 280×372 |
| IceCupStackRim.png | 1차: “ONLY top rolled elliptical RIM and a very short tapered translucent collar from ONE nested cup. True transparent interior; no lower cup body or bottom. Repeat upward at 18px step.” 원본 `exec-b8475dd8-f2a9-427e-acb5-6ccc017ed70e.png`는 합성에서 고리 사이가 비어 보여 사용하지 않았다. 재편집: “Keep same rolled oval lip, material and transparent background. Extend collar into continuous tapered upper CUP SIDEWALL, lower third of total component height; oval mouth occupies upper two-thirds. Continuous side edges bridge neighboring rims at 18px step in a 140px-wide stack. Translucent neutral gray sidewall, upper-left vertical highlights, curved open cut edge below; no cup base or lower rolled rim.” 최종 `exec-eeb44093-5535-43c3-bb4c-974f0c753fef.png`의 `(42,104)-(1731,792)`를 272×136으로 축소해 280×144 캔버스 `(4,4)`에 배치했다. 몸체와 같은 실제 폭 272px이며 중첩 시 짧은 옆면이 테두리 사이를 잇는다. | 280×144 |

- 크롭 좌표의 오른쪽·아래 끝은 제외한다. 투명 조각은 알파 12 미만의 생성 헤이즈만 제거했고 컵 벽의 반투명 알파와 부드러운 가장자리는 보존했다. 동적 수량, HOT/ICE 라벨, 닫기 버튼은 PNG에 넣지 않았다.

### 배치 (2026-11-12)

창 크기 **1000×640 화면 px**, 원본 **2000×1280px**. 왼쪽 위가 `(0,0)`이며 아래 x/y/w/h는 **투명 여백을 포함한 전체 PNG 캔버스**의 표시 사각형이다. 모든 그림은 원본의 0.5배로 표시한다. 이 표는 아트 연결용이며 코드·씬에는 적용하지 않았다.

| 요소 | x | y | w | h |
|---|---:|---:|---:|---:|
| Panel / 창 | 0 | 0 | 1000 | 640 |
| 닫기 버튼 예약 자리 | 900 | 36 | 64 | 64 |
| HotCup1 | 76 | 124 | 110 | 100 |
| HotCup2 | 208 | 124 | 110 | 100 |
| HotCup3 | 340 | 124 | 110 | 100 |
| HotCup4 | 472 | 124 | 110 | 100 |
| HotCup5 | 76 | 254 | 110 | 100 |
| HotCup6 | 208 | 254 | 110 | 100 |
| HotCup7 | 340 | 254 | 110 | 100 |
| HotCup8 | 472 | 254 | 110 | 100 |
| HotArea (클릭·드롭) | 60 | 120 | 540 | 245 |
| HotCount (TMP, 가운데 정렬) | 270 | 540 | 120 | 36 |
| IceBottom | 725 | 259 | 140 | 186 |
| IceRim (2번째 컵) | 725 | 241 | 140 | 72 |
| IceArea (최대 10개, 클릭·드롭) | 700 | 90 | 190 | 365 |
| IceCount (TMP, 가운데 정렬) | 735 | 540 | 120 | 36 |

- HOT은 왼쪽 위→오른쪽, 윗단→아랫단 순서다. `n`개이면 1~n번만 보이며 가져갈 때 8번부터 사라진다. 실제 잔 입구 바닥은 윗단 화면 y≈221.5, 아랫단 y≈351.5이며 두 선반 윗면에 앉는다. 캔버스 위 투명 여백은 원화의 짧고 넓은 도자기 비율을 유지하기 위한 것이므로 자동 Trim하지 않는다.
- ICE의 **step은 18 화면 px**(원본 36px). k번째 컵(k=2..10)의 띠는 `x=725`, `y=259-(k-1)×18`, `w=140`, `h=72`. 두 번째 y=241, 열 번째 y=97이다. 전체 더미 캔버스는 최대 `(725,97,140,348)`이며 예약 닫기 자리와 겹치지 않는다.
- ICE는 0개일 때 Bottom/Rim을 모두 숨긴다. 1개 이상이면 Bottom 한 장과 `n-1`개 띠를 그린다. 순서는 **배경 → Bottom → k=2,3,…,n 띠**다. 각 띠는 아래쪽 컵보다 나중에 그려 윗컵의 옆면이 아래 입구와 이어지게 한다. 4개이면 띠 y=241,223,205이며 더미가 낮아진다. 위에서 하나 가져가면 마지막 띠만 사라진다.
- HotArea와 IceArea는 수량이 0개여도 빈 컵을 되돌릴 수 있도록 고정 영역을 유지하는 연결 지시다. 클릭으로 하나 가져오기와 해당 영역으로 빈 컵을 드롭해 반환하는 동작은 코드에서 처리한다. 이번 작업은 그 코드를 변경하지 않았다.
- 수량 `n개`는 TMP **20px**, 색 `#5A3520`, 가운데 정렬 권장. 미리보기에서는 Galmuri11.ttf로 같은 위치에 별도 조판했으며 어떤 에셋에도 숫자를 굽지 않았다.

### 미리보기·검수·Unity 적용

- Assets 밖 미리보기: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/fec95591-d3a3-47b1-89d7-0c4e1896d871/scratchpad/cupshelf-20261112/`.
- `FullShelf.png`: HOT 8 + ICE 10, 1000×640. `UsedShelf.png`: HOT 3 + ICE 4, 1000×640. 배치 표의 같은 캔버스·좌표·겹침 순서로 합성했다. 닫기 표시는 예약 영역 확인용 임시 그림이고 Panel에는 없다.
- 두 상태에서 선반 접촉, HOT의 8→3 순서, ICE 띠의 연결·개수·높이 변화, 빈 아랫단의 자연스러운 모습, 수량 가독성과 닫기 영역 회피를 육안 확인했다. PNG 네 장의 RGBA/크기/알파, 패널 전체 알파 255를 검사했다. 작업 전후 해시로 기존 두 버튼과 `.meta`, `.cs`, `.unity`, `.prefab`, `.asset`의 불변을 확인했다.
- Unity **6000.6.4f1**로 돌아가 새 PNG를 임포트한다. 새 `.meta`는 Unity가 생성하도록 둔다. 네 장 모두 Sprite (2D and UI) / Single / **Bilinear** / Mip Maps 끔 / Alpha Is Transparency 켬. Max Size는 **2048 이상**, 원본 캔버스의 투명 여백을 보존한다.
- 이후 별도 코드·빌더 작업에서 이 표를 연결하고 Play에서 수량 0/1/최대 상태, 가져오기와 빈 컵 반환을 확인해야 한다. 이번 작업에서는 코드·씬·프리팹·에셋 정의와 `.meta`를 만들거나 수정하지 않았다. 기존 승인 ADR-0019의 기기 창 제목 제거 및 기존 일러스트 예외를 따른다. 새 ADR은 없다.
