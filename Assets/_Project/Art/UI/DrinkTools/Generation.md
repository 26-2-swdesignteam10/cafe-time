# 재료 작업 창 일러스트 (2026-11-12)

- 도구: OpenAI 내장 `image_gen.imagegen` 원화 생성·상태 편집. Python 3 / Pillow로 크롭, Lanczos 리샘플링, 공통 캔버스 정렬, 알파 정리, 생성 원화 마스킹, 프레임 분할·루프 경계 블렌딩, 검수 합성을 수행했다.
- 스타일 참조: `docs/ART_STYLE.md` 1장 기기 작업 창 예외, `../Water/Generation.md`의 **2026-11-12 일러스트 교체**, `../Water/Panel.png`, `../Water/HotZone.png`, `../Water/WaterStream0.png`~`WaterStream7.png`, `../DrinkRoom/BigCup*.png`.
- 부드러운 일러스트 / Bilinear 대상이다. 공통 재료는 크림, 세이지, 중립 스테인리스, 밝은 오크, 따뜻한 호박색 유리이며 조명은 왼쪽 위다. 투명 탄산수에만 옅은 청색을 사용했다.
- 공통 프롬프트: “Production Unity isolated sprite, premium soft painted 2D cafe illustration, clean charcoal contours, soft upper-left light, neutral brushed silver, warm cream, restrained sage accents, match Water machine and existing cups. Not pixel art or photo. True alpha background, no text, letters, digits, branding, hands or cup. Fully visible with padding.”
- 재료 이름, 무게, 제목, 안내 문구, 닫기 버튼은 에셋에 넣지 않았다. 병·팩·상자·꼬리표·LCD 라벨은 비어 있다. 미리보기의 이름·125 g·닫기는 임시 합성이며 납품 PNG에 포함되지 않는다.
- 새 ADR은 없다. 기존 기기 창 일러스트 예외와 ADR-0019(승인)의 제목 제거·큰 컵 방침을 따른다. `docs/ART_STYLE.md` 예외 목록에 하위 작업 창 `DrinkTools`를 명시했다.

### 파일별 프롬프트·후처리

아래 프롬프트는 공통 스타일에 추가한 개별 지시다. 원본과 최종 파일 대응은 이 문서 하단에 기록했다.

| 파일 | 개별 프롬프트 / 최종 후처리 | PNG 크기 |
|---|---|---|
| Panel.png | “Empty cafe workbench background, aspect 11:7. Match Water panel walnut rounded frame, narrow gold inset, warm greige wall, light oak counter. No tools, cups, scale, text, headings, buttons or decorations; upper right empty; opaque interior.” 2200×1400으로 리샘플링. 저울 발이 작업면에 놓이도록 내부 x42..2157에서 카운터 표면 y896..983을 y896..1095로 늘리고, 앞 나무턱 y984..1051을 y1096..1163으로, 아래 캐비닛 y1052..1359를 y1164..1359로 재배치했다. 외곽 프레임과 상단 벽은 보존하고 알파는 전체 255. | 2200×1400 |
| PumpBottle.png | “Neutral amber glass syrup pump bottle, blank cream label, chrome pump extended to RIGHT, extra long straight horizontal arm and downward outlet over empty space, body lower left, base visible, no liquid.” 원본 공통 경계를 크롭해 1000×800에 배치. 출구가 오른쪽 잔 위에 오도록 했다. 여러 맛과 연유에 공용으로 쓰는 중립 병이다. | 1000×800 |
| PumpBottlePressed.png | PumpBottle을 편집 대상으로 “Change ONLY chrome pump head and arm: press straight DOWN, retract exposed piston about 60%; body, label, cap, base, camera and canvas identical; no liquid or hands.” 원본과 같은 크롭·배율·좌표를 적용하고 원본 y360 아래 병 몸체를 정확히 교체 복사해 라벨·병이 전환 때 흔들리지 않게 했다. | 1000×800 |
| CheongJar.png | “Open glass fruit preserve jar, muted golden amber citrus preserve, blank cream label. Silver spoon standing in jar, handle toward upper right. Body lower left, base visible, no spare lid.” 동일 변환으로 병 바닥을 y792에 맞췄다. 딸기·자몽·유자 재료 이름과 실제 종류 구분은 코드가 맡으며 그림은 공용 청 병이다. | 1000×800 |
| CheongJarScoop.png | CheongJar을 편집 대상으로 “Preserve jar, blank label and base EXACTLY. Remove upright spoon; same silver spoon lifted horizontal above jar with preserve-filled bowl at far RIGHT, over transparent area. No hand, cup or falling syrup.” 두 상태의 원본 y540 아래는 같은 병 몸체를 사용했다. 숟가락만 최종 캔버스에서 오른쪽 60px 이동해 방울이 잔 안으로 떨어지게 했다. | 1000×800 |
| TeaBox.png | “Upright cream and sage tea bag box, blank broad label and subtle botanical leaf emblem. Open top reveals orderly individual paper tea packets, one protruding packet. No scattered separate objects, base visible.” 알파 경계를 크롭하고 공용 캔버스 왼쪽에 배치, 바닥 y792. 클릭 1회당 티백 1개를 넣는 동작용 정지 그림이다. | 1000×800 |
| MilkCarton.png | “Upright cream paper milk carton, muted sage bottom, small milk-drop emblem, BLANK label. Folded gable with uncapped open spout upper RIGHT. No cap elsewhere.” 비례 축소, 공용 캔버스, 바닥 y792. | 1000×800 |
| MilkCartonPour.png | MilkCarton을 참조해 “EXACT same carton, cream/sage, blank front and drop emblem. Lift and tilt CLOCKWISE about 100 degrees; open spout points DOWN-RIGHT at far right; body extends left. No hand, liquid, cup or floor.” 원본 0.60배로 축소하고 출구 하단 기준점 원본 (1115,552)을 공용 캔버스 (900,340)에 맞췄다. 정지 자세에서 들고 기울인 상태다. | 1000×800 |
| SodaBottle.png | “Upright transparent pale blue sparkling-water glass bottle, tiny interior bubbles, blank sage label, uncapped narrow neck, no separate cap. Full base visible.” 비례 축소, 공용 캔버스, 바닥 y792. | 1000×800 |
| SodaBottlePour.png | SodaBottle을 참조해 “EXACT same transparent bottle and blank sage label, tiny bubbles. Lift and tilt CLOCKWISE110 degrees: body across upper left, narrow neck to far RIGHT, mouth points DOWN-RIGHT. No stream, cup, hand, floor or text.” 원본 0.60배, 출구 하단 원본 (1284,532)을 공용 캔버스 (940,300)에 맞췄다. | 1000×800 |
| SyrupDrip0.png~SyrupDrip3.png | “Four-frame animation strip ONLY liquid on TRANSPARENCY, four equal columns. Delicate honey amber drops descending from center top to tiny bottom ripple: elongating drop, upper-third drop, lower-third drop, bottom splash. No machine, bottle, cup, background, labels or dividers.” 1448×1086 원본을 공통 4셀로 분할해 각각 72×192. 생성된 상단 헤이즈는 액체 부분만 남기는 좁은 마스크로 제거했다. 마지막 파문은 캔버스 하단에 정렬. 청에도 같이 쓰기 위해 중립적인 옅은 꿀색을 유지했다. | 각각 72×192, 4장 |
| MilkStream0.png~MilkStream7.png | “Eight-frame animation strip of WHITE MILK ONLY on transparent background, eight equal columns, 2048×768. Same slender creamy white continuous vertical thread from center TOP to tiny elliptical bottom ripple, ivory highlights travel down 1/8 cycle. No container, machine, cup, nozzle, steam, text or dividers.” 256px 폭 8셀로 분할, 72×192로 축소. 상단 출구 중심 x36, 하단 파문 공통 경계 정렬. 마지막 6·7위상에 첫 위상 25%·60%를 블렌딩해 루프 경계 완화. | 각각 72×192, 8장 |
| SodaStream0.png~SodaStream7.png | “Eight-frame animation strip of CLEAR SPARKLING WATER ONLY on transparency. Same slender pale blue transparent thread from center TOP to tiny bottom ripple; small bubbles and white glints travel down 1/8 cycle, seamless loop. No machine, bottle, cup, nozzle, steam, text or dividers.” 1774×887 원본을 8등분, 우유와 같은 72×192·상단 중심·하단 파문 규칙 및 6·7위상 블렌딩 적용. | 각각 72×192, 8장 |
| TeaBagInCupCeramic.png | BigCupCeramic을 위치 참조로 “OVERLAY ONLY: cream tea string starts inside cup, curves to rear rim, drapes over RIGHT rim and down to small blank cream/sage paper tag outside. No cup, saucer, liquid or tea bag body, actual transparent background.” 줄+꼬리표 원화 경계를 컵 원본 (238,279)~(418,446)에 배치. | 512×640 |
| TeaBagInCupPlastic.png | BigCupPlastic을 위치 참조로 같은 지시, 플라스틱의 높은 입구에 맞게 “inside (236,300), back rim (291,186), right rim (402,260), external tag near (407,340).” 원화 경계를 (236,186)~(422,365)에 맞춰 별도 정렬. | 512×640 |
| WeightPlate.png | “Small front-facing cafe digital scale, broad flat brushed silver platform, low warm grey body, sage-grey BLANK LCD centered front, subtle oak side trim, no digits, markings or buttons. Same elevated perspective as cups, transparent.” 경계 크롭, 비례 축소해 800×280에 중앙 배치. 패널에 저울이 없으므로 이 그림을 컵 뒤에 놓는다. | 800×280 |

- 모든 투명 PNG는 생성 헤이즈 중 알파 12 미만을 제거하고 완전 투명 픽셀 RGB도 0으로 정리했다. 부드러운 반투명 외곽과 유리의 투과감은 보존했다.
- 정수기를 스타일 참조한 첫 액체 시트 3종은 정수기 몸체까지 생성되어 폐기했다. 최종 액체 시트는 액체만 지시해 재생성했다. 폐기 원화를 Assets에 넣지 않았다.
- 도구 캔버스는 모두 **1000×800**, 표시 **500×400**이다. 긴 펌프·숟가락의 출구가 잔 위에 닿도록 투명 오른쪽 공간을 포함한다. 정지 상태 5종과 펌프 눌림·청 뜨기 상태의 병 바닥은 모두 **원본 y792**, 아래 여백 **8px**이다. 우유·탄산수 따르기 상태는 들어 올린 자세이므로 바닥에서 떨어진다. 같은 재료의 재질·문양·캔버스는 유지된다.

### 배치 (2026-11-12)

1100×700 창의 **왼쪽 위 (0,0) 기준 화면 px**. x/y는 PNG 전체 캔버스 표시 영역의 왼쪽 위, w/h는 표시 크기다. 코드·씬에는 적용하지 않았다. 컵 종류에 따라 줄기 **높이만** 바꾼다.

| 요소 | x | y | w | h | 접점 / 용도 |
|---|---:|---:|---:|---:|---|
| Panel | 0 | 0 | 1100 | 700 | 원본의 0.5배 |
| Tool | 260 | 84 | 500 | 400 | 9개 도구 공용, 정지 바닥 84+792×0.5=480 |
| ToolHit | 260 | 84 | 500 | 400 | Tool과 같음, 별도 확대 없음 |
| Cup | 616 | 164 | 256 | 320 | BigCup 원본 0.5배, 바닥 164+632×0.5=480 |
| TeaBag | 616 | 164 | 256 | 320 | Cup과 완전히 동일; HOT/ICE 맞는 덧그림 선택 |
| WeightPlate | 594 | 445 | 300 | 105 | 컵보다 먼저 그림; 컵 바닥 y480이 상판 위에 닿음 |
| Stream_Pump | 710 | 180 | 36 | 160 | 눌림 출구 (728,180) → HOT 내부 (728,340) |
| Stream_Cheong | 687 | 234 | 36 | 106 | 숟가락 아래 (705,234) → HOT 내부 (705,340) |
| Stream_Milk | 692 | 254 | 36 | 47 | 팩 입구 아래 (710,254) → ICE 내부 (710,301) |
| Stream_Soda | 712 | 234 | 36 | 67 | 병 입구 아래 (730,234) → ICE 내부 (730,301) |
| Name | 260 | 40 | 220 | 38 | 중심 (370,59), TMP 재료 이름 |
| Weight | 690 | 508 | 108 | 22 | 중심 (744,519), TMP 무게; 미리보기 Galmuri11 20px |
| Close 예약 | 1000 | 36 | 64 | 64 | 기존 닫기 버튼을 코드에서 표시, 본 에셋에 없음 |

- HOT 표면 도착 y340, ICE 표면 도착 y301을 기준으로 한다. **ICE의 펌프 h121 / 청 h67**, **HOT의 우유 h86 / 탄산수 h106**으로 표시하면 된다. x/y/w는 표와 같다. 빈 컵·수면이 다른 BigCup 상태에서는 내부에서 끝나도록 높이를 조정한다.
- 레이어: Panel → WeightPlate → Tool → Cup → Stream → TeaBag → Name/Weight → Close. 도구의 오른쪽 투명 영역이 Cup과 겹치는 것은 의도한 배치이며, 출구와 액체만 잔 위에 온다.
- SyrupDrip은 0→1→2→3으로 1회 재생, 프레임당 약 0.08초(총 0.32초) 후 숨김을 미리보기 권장값으로 둔다. 마지막 파문 다음 클릭 때 0으로 돌아간다.
- MilkStream/SodaStream은 0→1→…→7→0, 프레임당 0.1초(10fps) 권장. 누르는 동안만 보이고 놓으면 숨긴다. 이는 검수용 애니메이션 제안이며 게임 수치나 코드 설정을 변경한 것은 아니다.
- 모든 사각형이 창 안에 있고 Close 예약 영역과 교차하지 않는다. 컵 하단의 4px 투명 여백과 도구 하단의 4px 표시 여백은 접촉선 계산에서 제외했다.

### 미리보기·검수·Unity 적용

- Assets 밖 작업·미리보기 폴더: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/fec95591-d3a3-47b1-89d7-0c4e1896d871/scratchpad/drinktools-20261112/`.
- 요청한 1100×700 PNG 4장: `PumpMoment.png`(눌린 펌프+방울), `TeaInCup.png`(HOT 티백 덧그림), `MilkPouring.png`, `SodaPouring.png`. `PreviewFour.png`는 4장 축소 모음이다.
- 추가 확인: `CheongCheck.png`, `TeaPlasticCheck.png`, `ToolsContact.png`, `StreamsContact.png`, `StreamsLoop.gif`, `Validation.json`. 전체 도구의 여백·바닥선, 두 컵 입구와 꼬리표, 출구와 액체, 저울 접촉, LCD 글자 영역을 눈으로 확인했다.
- 33장 PNG의 RGBA/크기/투명 모서리, 정지 도구 공통 y792, 컵 바닥 y632, 패널 알파 255, 참조 Water PNG와 BigCup PNG 21장 원본 해시 보존을 검사했다. 새 그림에는 글자가 없다.
- Unity **6000.6.4f1**로 돌아가 임포트한다. `.meta`는 Unity가 생성하도록 둔다. Sprite (2D and UI) / Single / **Bilinear** / Mip Maps 끔 / Alpha Is Transparency 켬. Panel은 Max Size **4096**, 나머지는 **2048**. Sprite Mesh는 Full Rect 권장(공통 투명 캔버스 좌표 유지).
- 위 표대로 연결한 뒤 HOT/ICE 컵을 각각 확인하고, 펌프·청은 클릭 순간만 상태/방울 표시, 우유·탄산수는 누르는 동안만 따르기/줄기 표시, 티백은 넣은 뒤 컵 종류에 맞는 덧그림 표시를 Play에서 확인한다.
- 본 작업은 `.meta`, `.cs`, `.unity`, `.prefab`, `.asset`을 생성·수정하지 않았다. 폴더와 PNG 임포트, 배치 연결은 Unity에서 별도로 수행해야 한다. 저장소 검사는 추적 파일 기준이므로 아직 미추적 상태인 새 PNG의 `.meta` 동반 여부는 검사 대상이 아니며 임포트 후 확인해야 한다.

### 선택한 생성 원본

원본 폴더: `C:/Users/k9425/.codex/generated_images/01a1ce75-1606-78b0-9e70-2c07962f1ce2/`. 원본은 보존하고 프로젝트에 후처리 결과를 복사했다.

| 최종 파일 / 시트 | 선택한 원본 |
|---|---|
| Panel | `exec-c95e8f88-6472-494a-a703-3c1e800e866c.png` |
| PumpBottle | `exec-043e4da4-65f1-4471-87ca-71f533814cb2.png` |
| PumpBottlePressed | `exec-4cba5c40-17f7-4b94-9c1f-1415d6dd7709.png` |
| TeaBox | `exec-bcfac466-39d2-4386-8ad6-3acab156e174.png` |
| CheongJar | `exec-86badee0-e2a5-416f-833d-cc6cf1a0088e.png` |
| CheongJarScoop | `exec-268ce73d-8d4b-40de-8945-34d2cfbc925c.png` |
| MilkCarton | `exec-e204d3bc-a9af-4189-87a2-133e48685b52.png` |
| SodaBottle | `exec-dd5ea5b4-34d1-433a-b211-65c60f7dcf74.png` |
| SyrupDripSheet | `exec-625927dd-4630-43f5-9616-f72ae5d13ab8.png` |
| MilkStreamSheet | `exec-3c49241f-20df-4aa9-bc7f-b6b5b47a9a97.png` |
| SodaStreamSheet | `exec-48838e84-595e-4fe0-86a7-4c0388095ad0.png` |
| MilkCartonPour | `exec-075d3b16-e8cb-4ba9-8c21-46ec89cebc19.png` |
| TeaBagInCupCeramic | `exec-b6580da4-334d-4bd8-b0b4-9c8781ca3101.png` |
| TeaBagInCupPlastic | `exec-169ecab1-a4f0-463c-ac51-dc84662e5288.png` |
| WeightPlate | `exec-2682ccbe-1d19-47ef-846c-803aa4fe6d6a.png` |
| SodaBottlePour | `exec-18fb2523-dd08-4c0c-a20d-5a6c7cc24b73.png` |
