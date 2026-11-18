# 음료 판정 결과 일러스트 (2026-11-12)

- 도구: OpenAI 내장 `image_gen.imagegen`(완성품 24종·도장 4종을 개별 생성), Python 3 / Pillow(알파 정리, 비례 리샘플링, 공통 바닥 정렬, 미리보기 합성·검증).
- 참조: `docs/ART_STYLE.md` 1장 기기 작업 창 일러스트 예외, `../DrinkRoom/Generation.md`, `../DrinkRoom/BigCupCeramic*.png`, `../DrinkRoom/BigCupPlastic*.png`, `../DrinkTools/Generation.md`, `docs/GAME_DATA.md` 4장 메뉴·임시 레시피.
- HOT 생성 입력은 `../DrinkRoom/BigCupCeramicMixing.png`, ICE 생성 입력은 `../DrinkRoom/BigCupPlasticMixing.png`. 기존 빈 컵·내용물 상태도 시각 참조했다.
- 부드러운 명암·얇은 따뜻한 외곽선·왼쪽 위 조명의 일러스트이며 **Bilinear** 대상이다. HOT은 오른쪽 손잡이와 받침이 있는 흰 도자기 잔, ICE는 말린 입구의 투명 플라스틱 컵·연한 크림색 빨대다. ICE 음료의 얼음은 스무디·쉐이크에서 갈린 질감으로 표현한다.
- 메뉴 이름·등급 변수·배경 패널은 완성품 PNG에 넣지 않는다. 도장 문구만 고정 이미지다. LatteHot/LatteIce는 카페라떼·바닐라·헤이즐넛·연유라떼 공용이고 TeaHot/TeaIce는 캐모마일·얼그레이 공용이다.
- 도장 강조색: 완벽 금색/주황, 좋음 초록, 아쉬워요 주황갈색, 실패 어두운 빨강/회색.
- 이 작업은 그림·배치 자료 제공이다. 기존 `DrinkRoom/ResultPanel.png`, 코드와 씬 연결은 수정하지 않는다. 결과 표시 전환 설계는 기존 ADR-0027(제안)을 참조하며 이 작업에서 ADR을 추가·변경하지 않았다.

### 공통 생성 프롬프트

완성품: “Use case: stylized-concept. Single production Unity cafe drink sprite with REAL alpha transparent background. Reference is EXACT vessel identity and soft illustrated style to preserve. HOT: glossy white ceramic cup with RIGHT round handle and oval white saucer, exact reference shape, visible drink surface. ICE: transparent tapered disposable PLASTIC cup with rolled rim, NO LID, same reference shape, ONE pale cream drinking straw. Slightly elevated front camera. Soft painted shading, thin warm charcoal contours, upper-left light, cute casual cafe game, not photo, not pixel art. Full vessel visible with generous padding, portrait intended 640x800. No floor shadow, backdrop, words, menu name, logo or extra props.”

도장: “Single production Unity sprite, true transparent background. Cute casual circular RUBBER STAMP IMPRESSION. Large readable ROUND HANDWRITTEN Korean lettering across center, absolutely exact spelling, no other text. Coffee bean shapes decorating double circular border, distressed ink bleed and small worn speckles, slight counterclockwise tilt about 10 degrees. Colored INK lines only, transparent interior and counters, no solid disk, paper, physical stamp handle or drop shadow. Centered square 512x512 composition with generous clear edge padding. Bold and legible when shown at 200x200.”

### 파일별 프롬프트·후처리

아래 지시는 공통 프롬프트에 더한 개별 내용이다. 한 파일마다 한 번씩 생성했으며 후처리는 아래 공통 규칙을 적용했다.

| 파일 | 개별 생성 지시 | 원본 크기 → 표시 크기 |
|---|---|---|
| LatteHot.png | warm tan coffee surface with a clear white HEART latte art, very subtle two steam wisps | 640×800 → 320×400 |
| AmericanoHot.png | dark rich brown americano with subtle golden crema ring and two faint steam wisps | 640×800 → 320×400 |
| AmericanoIce.png | dark rich brown iced americano with golden crema, clear ice cubes | 640×800 → 320×400 |
| LatteIce.png | iced latte with white milk below and tan coffee above, beautiful distinct blended layers, clear ice cubes | 640×800 → 320×400 |
| CaramelHot.png | caramel latte with tan foam and neat caramel drizzle CROSSHATCH grid on surface | 640×800 → 320×400 |
| CaramelIce.png | iced caramel latte, milk and coffee layers, ice cubes and caramel CROSSHATCH drizzle on creamy top | 640×800 → 320×400 |
| MochaHot.png | cafe mocha with light chocolate foam, dark chocolate drizzle and light cocoa powder dusting | 640×800 → 320×400 |
| MochaIce.png | iced mocha, light chocolate milk, ice cubes, chocolate drizzle and cocoa powder on creamy top | 640×800 → 320×400 |
| StrawberryLatteIce.png | pink strawberry milk, vivid strawberry fruit pulp layer at bottom, pale pink and white milk above, ice cubes | 640×800 → 320×400 |
| AdeGrapefruit.png | sparkling grapefruit ade, translucent pink-orange soda, tiny carbonation bubbles, ice and grapefruit segments with a citrus slice | 640×800 → 320×400 |
| AdeYuja.png | sparkling yuja citrus ade, translucent yellow soda, tiny carbonation bubbles, ice and yellow citrus pulp with a citrus slice | 640×800 → 320×400 |
| AdeStrawberry.png | sparkling strawberry ade, translucent bright red soda, tiny carbonation bubbles, ice and strawberry fruit pieces with a lemon slice | 640×800 → 320×400 |
| AdeBlueLemon.png | sparkling blue lemon ade, translucent vivid blue soda, tiny carbonation bubbles, ice and lemon pieces with a yellow lemon slice | 640×800 → 320×400 |
| YujaTeaHot.png | translucent yellow hot yuja preserve tea with visible small citrus pulp pieces, faint steam, no milk | 640×800 → 320×400 |
| GrapefruitTeaHot.png | translucent orange hot grapefruit preserve tea with visible small grapefruit pulp pieces, faint steam, no milk | 640×800 → 320×400 |
| TeaHot.png | clear amber hot tea, a tea bag string draped over right rim and dangling small blank cream paper tag outside, faint steam | 640×800 → 320×400 |
| TeaIce.png | clear amber iced tea, clear ice cubes, tea bag string draped over right rim and dangling small blank cream paper tag outside | 640×800 → 320×400 |
| SmoothieStrawberry.png | thick strawberry smoothie, opaque pink finely blended slushy texture, small visible strawberry garnish on rim, no loose ice cubes on top | 640×800 → 320×400 |
| SmoothieMango.png | thick mango smoothie, opaque golden yellow finely blended slushy texture, small mango wedge garnish on rim, no loose ice cubes on top | 640×800 → 320×400 |
| SmoothieHallabong.png | thick hallabong mandarin smoothie, opaque orange finely blended slushy texture, small mandarin wedge garnish on rim, no loose ice cubes on top | 640×800 → 320×400 |
| SmoothieMelon.png | thick melon smoothie, opaque pale green finely blended slushy texture, small melon wedge garnish on rim, no loose ice cubes on top | 640×800 → 320×400 |
| CookieShake.png | thick cookies and cream milkshake, ivory milkshake speckled with cookie crumbs, whipped cream swirl topped with one small dark chocolate sandwich cookie | 640×800 → 320×400 |
| FailedHot.png | cute comically failed drink, murky dull green-purple brown liquid with lumpy curdled blobs, a few silly small bubbles and tiny curly steam wisps. Whimsical mild mess, no face, no gross horror | 640×800 → 320×400 |
| FailedIce.png | cute comically failed iced drink, murky dull green-purple brown liquid with lumpy curdled blobs and ice, a few silly small foam bubbles and tiny curly vapor wisps. Whimsical mild mess, no face, no gross horror | 640×800 → 320×400 |
| StampPerfect.png | exact Korean text: 완벽! (완 with final ㄴ, 벽 with final ㄱ, !), honey GOLD and orange ink, 3 tiny sparkle stars | 512×512 → 200×200 |
| StampGood.png | exact Korean text: 좋아요 (좋 with final ㅎ, 아, 요), friendly GREEN ink | 512×512 → 200×200 |
| StampBad.png | exact Korean text: 아쉬워요 (아, 쉬, 워, 요), orange BROWN ink | 512×512 → 200×200 |
| StampFailed.png | exact Korean text: 실패 (실 with final ㄹ, 패), muted dark RED and grey ink | 512×512 → 200×200 |

### 후처리

- 생성 원본의 실제 알파를 보존하고 알파 12 미만의 미세한 헤이즈만 제거한다. 투명 픽셀의 RGB는 0으로 정리한다. 색상 배경 제거로 플라스틱 투과감을 훼손하지 않는다.
- 내용물·장식·증기를 포함한 알파 경계를 크롭한 뒤 Lanczos 비례 리샘플링한다. HOT 최대 폭 560px, ICE 최대 폭 450px, 공통 최대 높이 736px 안에서 맞춘다. 가로 중심은 x=320, **모든 완성품 바닥 경계는 y=784**(마지막 보이는 픽셀 y783), 아래 16px 투명 여백이다.
- 도장은 최대 468×468 안에 비례 축소하고 512×512 중앙에 배치한다. 생성된 기울기와 잉크 질감을 보존한다.
- 위 처리는 스크래치의 `build.py`에 기록한다. 프로젝트 안에 후처리 코드를 추가하지 않는다.

### 배치 (2026-11-12)

1600×900 창의 **중심 (0,0), y 위쪽 +**, 각 RectTransform의 pivot/anchor는 중심 (0.5,0.5)이다. x/y는 전체 PNG 캔버스 또는 글자 영역의 **중심 좌표**다.

| 요소 | x | y | w | h | 용도 |
|---|---:|---:|---:|---:|---|
| Drink | -40 | 40 | 320 | 400 | 완성품 전체 캔버스, 공통 바닥 높이 y=-152 |
| Stamp | 110 | -80 | 200 | 200 | 완성품 오른쪽 아래에 겹침, Drink 다음에 표시 |
| Name | 0 | -225 | 640 | 56 | TMP 메뉴 이름, 가운데 정렬, Jua 36px·크림 #FCF0CA |

- 미리보기의 왼쪽 위 화면 좌표: Drink=(600,210), Stamp=(810,430), Name=(480,647). Name 글자 중심은 (800,675)다.
- 레이어: 화면 어둡게 처리 → Drink → Stamp → Name. 어둡게 처리는 코드의 화면 효과이며 PNG 배경 패널을 만들지 않았다. 등장·도장 애니메이션 구현과 시간 값은 이 작업 범위에 포함하지 않는다.

### 미리보기·Unity 적용

- Assets 밖 스크래치: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/fec95591-d3a3-47b1-89d7-0c4e1896d871/scratchpad/drinkresult-20261112/`.
- 1600×900 어두운 배경 미리보기: `PreviewPerfect.png`(라떼 HOT+완벽!), `PreviewGood.png`(블루레몬에이드+좋아요), `PreviewBad.png`(망고 스무디+아쉬워요), `PreviewFailed.png`(FailedIce+실패).
- `PreviewFour.png`는 4장 축소 모음, `DrinksContact.png`·`StampsContact.png`는 전체 납품 이미지 검수용이다. 미리보기의 메뉴 이름은 임시 합성이며 완성품 PNG에는 없다.
- Unity **6000.6.4f1**로 돌아가 임포트해 .meta를 자동 생성한다. Sprite (2D and UI) / Single / **Bilinear** / Mip Maps 끔 / Alpha Is Transparency 켬 / Max Size 1024 이상 / Mesh Type Full Rect / Pivot Center.
- 코드 담당자가 위 배치와 공용 이미지 매핑을 연결한 뒤 음료 제조실·쉐이크 제조실에서 완성 결과, HOT/ICE, 4등급을 Play로 확인한다.
- .meta, .cs, .unity, .prefab, .asset 파일은 만들거나 수정하지 않는다.


### ?? ??

- 28? ?? PNG RGBA, ?? ?? ??, ?? ?????? ???? ????. ??? 24? ?? ?? ?? ?? y784?.
- 1600?900 ???? 4?? ?? ????. ICE ?? ? ???? ??? ???? Stamp ??? ?? ? x140?? **x110**?? ????. ?? ?? ??? ??? ???? ???.
- ?? ?? **??! / ??? / ???? / ??**? ??? ?? ???? ??? ????.
- `bash .github/scripts/check-repo.sh`: ?? ?? (?? ?? 1691?). ? ??? PNG???? .meta ?? ??? ? ??? ??? ???? Unity ??? ? ??? ????.
- ?? ??? ??? .meta?.cs?.unity?.prefab?.asset ? BigCup ?? 6?? ??? ????. DrinkRoom? ?? ??/??/?? PNG 7??? ?? ? ?? ??? ???? ????? ???? ???. ?? ??? ???? Validation.json? ??.

### ??? ?? ??

??? ?? ?? ??? ???? ??? ??? ????? ????. ???? Manifest.json?? ??? ?? ?? ????? ?? ??? ??.

| ?? ?? | ?? ?? |
|---|---|
| LatteHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-c2f248f3-ca38-4686-a48d-463524c02dae.png` |
| AmericanoHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-cf2aab75-29b8-4751-a167-4e401015a2ab.png` |
| AmericanoIce.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-c6a5dc36-b404-4efd-8114-6cb7bb815cd5.png` |
| LatteIce.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-4eb9f5ce-5f83-4d48-b96e-23b54cf692f6.png` |
| CaramelHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-ce006e95-927e-452a-9e83-a490e04df607.png` |
| CaramelIce.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-8fc2a380-249b-40d7-a998-57f76f873a96.png` |
| MochaHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-a4769b40-f71f-4f03-bba1-efdd05f16eea.png` |
| MochaIce.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-f3b15a35-7495-4978-bb5a-5de4695fced8.png` |
| StrawberryLatteIce.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-b21072f8-ab7e-4041-a440-baff0c5fdbcd.png` |
| AdeGrapefruit.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-68f8367e-bf9c-4468-943a-b3fe1f270892.png` |
| AdeYuja.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-f431ea11-3779-4797-b7f1-4b904e3eb29f.png` |
| AdeStrawberry.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-63c4d4c3-8b97-4ccf-8f1a-e5da5a5b1f0d.png` |
| AdeBlueLemon.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-a5b565a4-4abc-451f-815a-4c067aebb313.png` |
| YujaTeaHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-6471df99-fbd5-42c0-8a20-e00e38f9a732.png` |
| GrapefruitTeaHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-f17fc088-c1b9-4b94-a5ab-cd8a287cf027.png` |
| TeaHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-8ff3735a-fc1b-49d1-a266-ca2760100a3d.png` |
| TeaIce.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-98b6c8cd-49f3-4840-b060-4dd44e9e5c3f.png` |
| SmoothieStrawberry.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-adc5b6a7-2a9d-4e81-b206-1f01514c1c6e.png` |
| SmoothieMango.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-95705d8a-f2fc-4a4e-a5b5-cafb281b66ab.png` |
| SmoothieHallabong.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-5d5f090b-925c-43ee-9136-c523848d4834.png` |
| SmoothieMelon.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-85429431-87a1-4602-b707-2bc69ee40b11.png` |
| CookieShake.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-ee0fdcd3-9553-4fb6-802d-eaed266bf02b.png` |
| FailedHot.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-499eabbd-1e12-4070-a924-9ccb6e78d310.png` |
| FailedIce.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-7a9bf4e2-9a9b-4352-9948-2b8b0caa8bbb.png` |
| StampPerfect.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-29784b5f-ec02-48c9-b8cb-3a8a17defa21.png` |
| StampGood.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-17cf3a9b-4f09-4271-8295-2a7f9e54f8de.png` |
| StampBad.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-fdb954d5-26da-4c85-85b8-a907c799f2f1.png` |
| StampFailed.png | `C:\Users\k9425\.codex\generated_images\01a1ced0-2ca3-7862-92a5-ff4ef0c9559c\exec-327e16e6-d52f-494e-8356-08dc33c05926.png` |


## 2026-11-17 새 메뉴 완성품 7종 (작업 C)

- 도구: OpenAI 내장 `image_gen.imagegen` 파일별 개별 생성, Python 3 / Pillow 알파 정리·크기 정렬·미리보기.
- 컵·스타일 참조: `LatteHot.png`, `LatteIce.png`, `MochaIce.png`, `CookieShake.png`, `docs/ART_STYLE.md`. 같은 부드러운 일러스트·따뜻한 얇은 경계·왼쪽 위 조명·약간 내려다보는 구도, **Bilinear** 대상. 도트 처리하지 않았다.
- `CappuccinoHot.png`: 두꺼운 우유 거품과 시나몬, 오른쪽 손잡이 흰 도자기 HOT 컵·받침·증기.
- `CappuccinoIce.png`: 얼음이 보이는 커피·우유, 상단 흰 거품층과 시나몬.
- `ColdBrewIce.png`: 우유 없는 맑고 진한 갈색 콜드브루와 얼음.
- `ColdBrewLatteIce.png`: 진한 콜드브루 위층·아래 우유층과 경계의 소용돌이.
- `ChocoLatteIce.png`: 초콜릿 우유·얼음. 휘핑·드리즐 제거.
- `MatchaLatteIce.png`: 연두 말차·아래 우유층·얼음.
- `MilkShake.png`: 바닐라 아이스크림·우유·얼음을 간 크림색 밀크쉐이크·휘핑. CookieShake의 컵·왼쪽 빨대를 따르고 쿠키·검은 가루 제거.
- 모두 **640×800 RGBA**. 공통 후처리: 생성 알파12 미만 헤이즈·작은 분리 잡점 제거, 3px 열기 연산으로 작은 외곽 돌출 정리. 실체 bbox 크롭 후 Lanczos 비례 조정: HOT 최대 폭560, ICE 최대 폭450, 최대 높이736. 가로 중심 x320, **바닥 경계 y784**(마지막 픽셀 y783), 아래 투명 여백16px. 완전 투명 RGB=0.
- 결과 배치 유지: 1600×900 화면 좌상단 기준 **Drink rect=(600,210,320,400)**. 화면 중심 기준 RectTransform=(-40,40,320,400), 공통 바닥은 화면 y602. Stamp·메뉴 이름은 기존 별도 이미지/TMP로 표시.
- Unity **6000.6.4f1**에서 .meta 자동 생성. Sprite / Single / **Bilinear** / Mip Maps 끔 / Alpha Is Transparency 켬 / Full Rect / Pivot Center / Max Size1024 이상. 코드 연결 후 7종 결과·HOT/ICE 컵·도장 겹침을 Play 확인. 새 ADR·메뉴 데이터·레시피·코드·씬 변경은 없다.

### 파일별 실제 생성 프롬프트와 원본

**CappuccinoHot.png** — 참조 `LatteHot.png`

> Use case: precise-object-edit. Production Unity cafe game finished drink sprite. Reference image is the EXACT vessel, composition and soft painted illustration style. Replace the heart latte art with a thick domed layer of dense white microfoam lightly sprinkled with cinnamon powder. Keep ceramic cup, right handle, saucer, steam and perspective identical. No whipped cream spiral. Preserve thin warm contours, soft highlights, upper-left illumination, elevated front camera. Full object, generous transparent padding, portrait intended 640x800. Smooth illustrated rendering for Bilinear, NOT pixel art or photograph. REAL alpha transparency, NO hands, text, digits, logos, watermark, checkerboard, backdrop, cast shadow or extra props.

생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e957-2f2c-7ac3-a135-5ea490a79811\exec-9ca9f9e6-613b-4d81-9ae6-38ffec6b1d21.png`

**CappuccinoIce.png** — 참조 `LatteIce.png`

> Use case: precise-object-edit. Production Unity cafe game finished drink sprite. Reference image is the EXACT vessel, composition and soft painted illustration style. Iced cappuccino, milk coffee beige body with visible ice and a distinct thick white milk foam layer at top lightly dusted with cinnamon. Keep exact clear plastic cup, rolled rim, pale straw and camera. Preserve thin warm contours, soft highlights, upper-left illumination, elevated front camera. Full object, generous transparent padding, portrait intended 640x800. Smooth illustrated rendering for Bilinear, NOT pixel art or photograph. REAL alpha transparency, NO hands, text, digits, logos, watermark, checkerboard, backdrop, cast shadow or extra props.

생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e957-2f2c-7ac3-a135-5ea490a79811\exec-46ffee18-1d1b-443e-bc4e-268e0f03bced.png`

**ColdBrewIce.png** — 참조 `LatteIce.png`

> Use case: precise-object-edit. Production Unity cafe game finished drink sprite. Reference image is the EXACT vessel, composition and soft painted illustration style. Clear deep dark brown cold brew coffee with visible translucent ice cubes, no milk, no cream, no foam. Keep exact clear plastic cup, rolled rim, pale straw and camera. Preserve thin warm contours, soft highlights, upper-left illumination, elevated front camera. Full object, generous transparent padding, portrait intended 640x800. Smooth illustrated rendering for Bilinear, NOT pixel art or photograph. REAL alpha transparency, NO hands, text, digits, logos, watermark, checkerboard, backdrop, cast shadow or extra props.

생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e957-2f2c-7ac3-a135-5ea490a79811\exec-96b0836a-b5bc-4f25-bd33-712de4bfbb57.png`

**ColdBrewLatteIce.png** — 참조 `LatteIce.png`

> Use case: precise-object-edit. Production Unity cafe game finished drink sprite. Reference image is the EXACT vessel, composition and soft painted illustration style. Cold brew latte: dark brown translucent coffee upper layer with ice, ivory milk lower layer and gentle swirling boundary. Darker coffee than reference. Keep exact clear plastic cup, rolled rim, pale straw and camera. Preserve thin warm contours, soft highlights, upper-left illumination, elevated front camera. Full object, generous transparent padding, portrait intended 640x800. Smooth illustrated rendering for Bilinear, NOT pixel art or photograph. REAL alpha transparency, NO hands, text, digits, logos, watermark, checkerboard, backdrop, cast shadow or extra props.

생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e957-2f2c-7ac3-a135-5ea490a79811\exec-cace473d-72f4-441e-b258-b7a00c824b04.png`

**ChocoLatteIce.png** — 참조 `MochaIce.png`

> Use case: precise-object-edit. Production Unity cafe sprite. Input is edit target and exact soft painted style / geometry reference. Iced chocolate milk, rich milk-chocolate brown body and visible ice cubes. Flat liquid top, REMOVE whipped cream, REMOVE chocolate drizzle, no coffee. Keep exact clear plastic cup, rolled rim, pale straw and camera. Thin warm contours, soft upper-left light, smooth illustration for Bilinear, NOT pixel art or photo. Fully visible edges, REAL transparent background. NO hands, text, digits, logos, watermark, checkerboard, ground or extra props. Intended portrait canvas 640x800, preserve vessel silhouette.

생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e957-2f2c-7ac3-a135-5ea490a79811\exec-1f9c1ad3-7da9-4813-9fb6-ca9a04283d8c.png`

**MatchaLatteIce.png** — 참조 `LatteIce.png`

> Use case: precise-object-edit. Production Unity cafe sprite. Input is edit target and exact soft painted style / geometry reference. Iced matcha latte, fresh light matcha green upper layer and ivory milk lower layer, soft green milk swirls and clear ice cubes. No coffee, no garnish. Keep exact clear plastic cup, rolled rim, pale straw and camera. Thin warm contours, soft upper-left light, smooth illustration for Bilinear, NOT pixel art or photo. Fully visible edges, REAL transparent background. NO hands, text, digits, logos, watermark, checkerboard, ground or extra props. Intended portrait canvas 640x800, preserve vessel silhouette.

생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e957-2f2c-7ac3-a135-5ea490a79811\exec-2fb9e1d5-a4e7-41a2-ae65-588e2d3cec66.png`

**MilkShake.png** — 참조 `CookieShake.png`

> Use case: precise-object-edit. Production Unity cafe sprite. Input is edit target and exact soft painted style / geometry reference. Vanilla milkshake, smooth pale ivory creamy body made of blended vanilla ice cream milk and ice, plain ivory whipped cream swirl. REMOVE ALL cookie, dark crumbs and brown speckles, no extra garnish. Keep exact clear plastic cup, rolled rim, left cream straw, whipped cream silhouette and camera. Thin warm contours, soft upper-left light, smooth illustration for Bilinear, NOT pixel art or photo. Fully visible edges, REAL transparent background. NO hands, text, digits, logos, watermark, checkerboard, ground or extra props. Intended portrait canvas 640x800, preserve vessel silhouette.

생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e957-2f2c-7ac3-a135-5ea490a79811\exec-78305b27-d931-47e3-a21b-fa0935440722.png`

### 최종 검수 자료

검수 PNG는 저장소 밖 `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/1ac9cc50-e1f2-4923-a674-160ca3239d3d/scratchpad/codex/task-c/`에만 저장했다. `AssetsContact.png`는 전체15종, `SilhouetteOverlay.png`는 제조실 원본·신규·겹침·윤곽 비교, `LayoutChunky.png`·`LayoutSmooth.png`는 기존 제조실 배치 합성이다. 실제 크기·bbox·알파 비교는 `validation.json`, 프롬프트·원본 매핑은 `manifest.json`에 기록했다. 원화는 기본 생성 폴더에 보존했다.

최종 PNG를 배경 위에서 눈으로 확인했다. 손·워터마크·체커보드·이미지 글자는 없으며 실제 RGBA 투명 배경이다. .meta·.cs·.unity·.prefab·.asset은 직접 생성·수정하지 않았다.

### 최종 파일별 픽셀 배치

| PNG | 크기 | 실체 bbox (왼쪽,위,오른쪽,아래) |
|---|---|---|
| CappuccinoHot.png | 640×800 | (40, 188, 600, 784) |
| CappuccinoIce.png | 640×800 | (95, 80, 545, 784) |
| ColdBrewIce.png | 640×800 | (95, 76, 545, 784) |
| ColdBrewLatteIce.png | 640×800 | (95, 75, 545, 784) |
| ChocoLatteIce.png | 640×800 | (101, 48, 539, 784) |
| MatchaLatteIce.png | 640×800 | (95, 71, 545, 784) |
| MilkShake.png | 640×800 | (102, 48, 537, 784) |

최종 저장소 검사: bash .github/scripts/check-repo.sh 통과 (추적 파일 1993개). 이 검사는 추적 파일 기준이며 새 PNG 15개는 Unity 임포트 후 .meta 생성과 임포트 설정을 별도로 확인한다.


## 2026-11-17 초코·말차·밀크쉐이크 재제작 (작업 E)

- 최종 검사: `bash .github/scripts/check-repo.sh` 통과 (추적 파일 1993개), 작업 문서 `git diff --check` 통과. Unity Play 검수는 사용자 확인이 필요하다.
- 도구: OpenAI 내장 image_gen.imagegen으로 파일별 개별 생성·편집, Python 3 / Pillow 후처리·검수. CLI/API 대체 사용 없음.
- 스타일 참조: LatteIce.png를 초코·말차의 편집 대상으로, CookieShake.png를 밀크쉐이크의 편집 대상으로 사용했다. MochaIce.png·StrawberryLatteIce.png도 세트의 컵·빨대·화풍·크기 비교에 사용했다.
- 후처리·좌표: 생성 전체 캔버스를 640×800으로 Lanczos 조정했다. 초코·말차는 LatteIce의 정확한 외부 알파·롤드 림·오른쪽 빨대를 재사용했다. 림 마스크는 바깥 타원 (95,203,544,392)에서 안쪽 타원 (116,225,523,370)을 뺀 영역, 빨대는 (398,302),(464,79),(490,86),(423,310) 다각형이다. 외곽2px는 알파16 이상 이진 실루엣에서 계산했다. 밀크쉐이크는 CookieShake의 왼쪽 빨대·전면 림·y380 아래 몸체 알파를 보존하고 바닐라·휘핑·체리 원화를 합성했다. 공통 중심 x320, 바닥 경계 y784(마지막 픽셀 y783), 하단 여백16px. 투명 RGB=0. 기존 1600×900 결과 배치 유지: 좌상단 기준 Drink rect=(600,210,320,400), RectTransform=(-40,40,320,400).
- 검수: DrinksComparison.png에서 LatteIce / ChocoLatteIce / MochaIce / MatchaLatteIce / CookieShake / MilkShake를 나란히 놓아 확인했다. 초코의 진한 시럽 줄무늬·초코 우유·얼음·코코아와 드리즐, 말차의 진녹색 상층·흰 우유·내려오는 무늬·얼음, 밀크쉐이크의 바닐라·휘핑·작은 바닐라 빈 점·체리를 확인했다.
- Unity 적용: Unity 6000.6.4f1 자동 재임포트 후 Sprite / Single / Bilinear / Mip Maps 끔 / Alpha Is Transparency 켬 / Full Rect / Pivot Center / Max Size1024 이상을 확인한다. Play에서 컵 바닥 정렬과 도장 겹침을 확인한다.
- 검수 합성·스크립트·원본 매핑은 저장소 밖 임시 폴더에만 저장: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/1ac9cc50-e1f2-4923-a674-160ca3239d3d/scratchpad/codex/task-e/`. 후처리 build.py, 프롬프트·원본 Manifest.json, 픽셀 검사 Validation.json.
- 같은 파일 이름·같은 크기의 PNG만 교체했다. 완전한 RGBA 투명 배경이며 손·워터마크·체커보드·글자는 없다. .meta·.cs·.unity·.prefab·.asset은 직접 생성·수정하지 않았다. 기존 기록 보존, 새 ADR·게임 데이터 변경 없음.

### 파일별 프롬프트·배치

| 파일 | 크기 | 실체 bbox (좌,상,우,하; 우·하는 배타 경계) |
|---|---|---|
| ChocoLatteIce.png | 640×800 | (95, 79, 545, 784) |
| MatchaLatteIce.png | 640×800 | (95, 79, 545, 784) |
| MilkShake.png | 640×800 | (101, 46, 538, 784) |

**ChocoLatteIce.png**
- 입력: `UI/DrinkResult/LatteIce.png`
- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e973-6339-7b70-be7b-7bae3b8f6f20\exec-b3c8e50a-d00b-4848-af0a-abe3c05b666c.png`

> Use case: precise-object-edit. EDIT the provided LatteIce sprite, changing ONLY drink contents. Preserve EXACT same cup geometry, rolled transparent rim, cream straw on right, size, position, perspective, thin warm contours, lighting and smooth painted illustration style. Replace coffee and white milk with delicious rich chocolate milk, prominent thick dark chocolate syrup irregular vertical trails down INNER cup walls, clear ice cubes on top, subtle cocoa dust and thin chocolate drizzle over ice. No whipped cream. Same clear tapered plastic cup, no lid. Transparent background and same composition. Intended 640x800 with cup centered x320 bottom y784. No text, hands, watermark, checkerboard, props or floor shadow.

**MatchaLatteIce.png**
- 입력: `UI/DrinkResult/LatteIce.png`
- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e973-6339-7b70-be7b-7bae3b8f6f20\exec-62a39e98-72e1-4032-92f3-30760cbb7611.png`

> Use case: precise-object-edit. EDIT provided LatteIce sprite, change ONLY contents from coffee to iced matcha latte. Preserve EXACT cup shape, rolled transparent rim, cream straw right, scale, placement, perspective, lighting and soft painted rendering. Bright ivory milk lower half, deep rich natural green matcha upper half with gorgeous flowing green ribbons descending and mixing into white milk, visible clear ice on top tinted green, appetizing strong GREEN versus WHITE contrast. No coffee brown, whipped cream, garnish, lid. Same transparent tapered cup. Intended 640x800, centered x320 bottom y784. Real alpha transparency. No text, hands, watermark, checkerboard, backdrop or cast shadow.

**MilkShake.png**
- 입력: `UI/DrinkResult/CookieShake.png`
- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1e973-6339-7b70-be7b-7bae3b8f6f20\exec-04192fa3-148b-4acf-a03d-c440126240f6.png`

> Use case: precise-object-edit. EDIT provided CookieShake, changing ONLY contents and topping. Keep EXACT same clear plastic cup geometry, rolled rim, left cream straw, position, scale, perspective, highlights and soft painted cafe game illustration. Delicious ivory VANILLA milkshake with smooth thick creamy body, sparse tiny vanilla bean pinpoints, same sculpted whipped cream swirl, remove ALL cookie crumbs and cookie, replace cookie with one small glossy red cherry with slender stem. Vanilla shake mostly clean pale ivory, no chocolate or brown crumbs. No lid. Intended same 640x800 centered x320 bottom y784. Real alpha transparent background. No text, hands, watermark, checkerboard, props, ground shadow.


## 2026-11-17 작업 F — HOT/ICE 커피 완성품 6종

- 최종 검사: `bash .github/scripts/check-repo.sh` 통과 (추적 파일1993개), 작업 문서 `git diff --check` 통과. 새 PNG7개의 .meta 생성·임포트와 실제 Play 검수는 Unity에서 별도로 확인한다.

- 도구: OpenAI 내장 `image_gen.imagegen`으로 각 파일 개별 편집 생성 + Python 3 / Pillow 내용물 합성·알파 보존·비교 검수. CLI/API 대체 없음.
- 편집 대상: HOT 4종은 `LatteHot.png`, ICE 2종은 `LatteIce.png`. 비교 스타일 참조는 `AmericanoHot.png`, `ColdBrewIce.png`, `ColdBrewLatteIce.png`, `docs/ART_STYLE.md`. 부드러운 일러스트·Bilinear, 동일한 잔·크기·위치·조명.
- 전부 **640×800 RGBA**, 가로 중심x320, **바닥 경계y784**(마지막 픽셀y783), 아래 투명 여백16px. HOT bbox=(40,189,600,784), ICE bbox=(95,79,545,784). 기준 LatteHot/LatteIce와 **알파 차이0**.
- HOT: 생성 전체 캔버스를640×800로 Lanczos 조정하고 액면만 원본 타원(141,351)–(462,497)에 합성. ColdBrewLatteHot/HazelnutLatteHot의 생성 액면은 원본좌표(140,357,464,505), 나머지는(140,350,464,498)에서 꺼내322×147로 맞췄다. 잔·손잡이·받침·증기는 원본 RGBA를 보존했다.
- ICE: 생성 RGB를640×800로 조정하고 원본 알파를 그대로 사용. 원본 롤드 림(바깥타원95,203,544,392−안쪽타원116,225,523,370), 오른쪽 빨대 다각형(398,302),(464,79),(490,86),(423,310), 외곽2px를 원본으로 복원했다. 완전 투명 RGB=0.

| 파일 | 내용물 구분 |
|---|---|
| ColdBrewHot.png | 아메리카노보다 더 진하고 붉은 갈색의 맑은 블랙 표면, 크레마·폼·라떼아트 없음 |
| ColdBrewLatteHot.png | 균일하고 부드러운 갈색 우유 커피, 밀크 폼·라떼아트 없음 |
| VanillaLatteHot.png | 밝은 크림색 커피·바닐라 빈 점·작은 단순한 하트 |
| VanillaLatteIce.png | 밝은 크림색 우유 커피·바닐라 빈 점·얼음 위 바닐라 시럽 줄무늬 |
| HazelnutLatteHot.png | 진한 견과 갈색 커피·하트 옆 헤이즐넛 조각 |
| HazelnutLatteIce.png | 진한 견과 갈색 우유 커피·얼음 위 헤이즐넛 조각·안쪽 시럽 줄무늬 |

- 저장소 밖 `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/1ac9cc50-e1f2-4923-a674-160ca3239d3d/scratchpad/codex/task-f/`의 `DrinksComparison.png`에 기존5종과 신규6종을 나란히 배치해 눈으로 확인했다. 잔·빨대·바닥 정렬 및 메뉴별 색과 토핑 차이를 확인했다. 손·워터마크·체커보드·문자는 없으며 진짜 RGBA 투명 배경이다. 원본 파일은 덮어쓰지 않았다.
- 생성 프롬프트·원화 경로는 아래와 임시Manifest.json에, 후처리는build.py에, 픽셀 검사는Validation.json에 있다. 검수 합성 PNG는 저장소 밖에만 둔다.
- Unity6000.6.4f1에서 .meta 자동 생성, Sprite/Single/Bilinear/Mip Maps 끔/Alpha Is Transparency 켬/Full Rect/Pivot Center/Max Size1024 이상. 기존 결과 Drink rect=(600,210,320,400), 바닥화면y602를 유지한다. 별도 코드 연결 후 Play에서 6종·도장 겹침 확인. 새 ADR·게임 데이터·코드·.meta·씬·프리팹·.asset 변경 없음.

**ColdBrewHot.png**

- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1ed3d-1c3d-7672-b46c-190ecf1a7ffd\exec-5f3c0000-7d41-4d0a-adf4-3e654fc38dd5.png`

> Use case: precise-object-edit. EDIT target LatteHot.png for production Unity cafe game sprite ColdBrewHot. Change ONLY the coffee surface inside the cup to clear very dark reddish brown black cold brew coffee, darker than Americano, glossy subtle reflection, NO crema, NO foam, NO latte art. Preserve EXACT ceramic white cup, right handle, saucer, steam, size, position, perspective and smooth illustrated painted style, upper-left illumination. Same portrait 640x800 composition centered x320, base y784. Real transparent alpha background, no text hands watermark checkerboard props or floor shadow.

**ColdBrewLatteHot.png**

- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1ed3d-1c3d-7672-b46c-190ecf1a7ffd\exec-dc0fa194-2e91-4571-87fe-020e1f432171.png`

> Use case: precise-object-edit. EDIT target LatteHot.png for production Unity cafe game sprite ColdBrewLatteHot. Change ONLY the liquid inside the cup to cold brew mixed with milk: smooth uniform warm medium milk-coffee brown surface with gentle glossy highlights. NO milk foam, NO froth, NO latte art or heart, NO crema. Preserve EXACT white ceramic cup, right handle, saucer, steam, geometry, size, position, camera, lighting and smooth painted illustration style. Same 640x800 portrait composition, center x320 bottom y784. Real transparent background, no text hands watermark checkerboard props floor shadow.

**VanillaLatteHot.png**

- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1ed3d-1c3d-7672-b46c-190ecf1a7ffd\exec-ac9c71ca-473c-41b7-9b16-19ef20df0af8.png`

> Use case: precise-object-edit. EDIT supplied LatteHot.png for production Unity cafe game VanillaLatteHot. Change ONLY coffee surface to pale creamy vanilla latte noticeably lighter ivory cream than original caffe latte, tiny visible dark vanilla bean pinpoints, small simple white heart latte art. No syrup drizzle, no whipped cream. Preserve EXACT vessel geometry, white ceramic cup right handle saucer and steam if HOT, clear tapered plastic cup rolled rim and cream straw on RIGHT if ICE. Preserve original size, position, perspective, soft upper-left lighting, warm thin contours, smooth painted illustration for Bilinear. Same portrait 640x800 composition centered x320 bottom y784. Change only contents and requested topping. REAL transparent alpha exterior, no text digits logos hands watermark checkerboard props backdrop or cast shadow.

**VanillaLatteIce.png**

- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1ed3d-1c3d-7672-b46c-190ecf1a7ffd\exec-46dacbad-1b9a-421d-ba35-b0566ab773e6.png`

> Use case: precise-object-edit. EDIT supplied LatteIce.png for production Unity cafe game VanillaLatteIce. Change ONLY drink contents to pale creamy vanilla milk coffee distinctly lighter than original caffe latte. Visible ice, sparse tiny dark vanilla bean specks in creamy liquid, thin warm golden vanilla syrup ribbons across TOP ice. No whipped cream or lid. Preserve EXACT vessel geometry, white ceramic cup right handle saucer and steam if HOT, clear tapered plastic cup rolled rim and cream straw on RIGHT if ICE. Preserve original size, position, perspective, soft upper-left lighting, warm thin contours, smooth painted illustration for Bilinear. Same portrait 640x800 composition centered x320 bottom y784. Change only contents and requested topping. REAL transparent alpha exterior, no text digits logos hands watermark checkerboard props backdrop or cast shadow.

**HazelnutLatteHot.png**

- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1ed3d-1c3d-7672-b46c-190ecf1a7ffd\exec-6d3128b2-bbad-4745-9a20-332d6d6678fa.png`

> Use case: precise-object-edit. EDIT supplied LatteHot.png for production Unity cafe game HazelnutLatteHot. Change ONLY coffee surface to deep toasted hazelnut brown latte distinctly darker than original caffe latte, preserve white heart latte art and add a few small golden chopped hazelnut pieces on surface BESIDE the latte art. No whipped cream. Preserve EXACT vessel geometry, white ceramic cup right handle saucer and steam if HOT, clear tapered plastic cup rolled rim and cream straw on RIGHT if ICE. Preserve original size, position, perspective, soft upper-left lighting, warm thin contours, smooth painted illustration for Bilinear. Same portrait 640x800 composition centered x320 bottom y784. Change only contents and requested topping. REAL transparent alpha exterior, no text digits logos hands watermark checkerboard props backdrop or cast shadow.

**HazelnutLatteIce.png**

- 생성 원본: `C:\Users\k9425\.codex\generated_images\01a1ed3d-1c3d-7672-b46c-190ecf1a7ffd\exec-101fc17c-563d-4de4-97c6-c239b9efadad.png`

> Use case: precise-object-edit. EDIT supplied LatteIce.png for production Unity cafe game HazelnutLatteIce. Change ONLY drink contents to rich toasted nut brown hazelnut latte distinctly darker than original caffe latte, less white milk, visible ice, a few small golden chopped hazelnuts resting on TOP ice and thin nut-brown syrup trails inside cup. No whipped cream or lid. Preserve EXACT vessel geometry, white ceramic cup right handle saucer and steam if HOT, clear tapered plastic cup rolled rim and cream straw on RIGHT if ICE. Preserve original size, position, perspective, soft upper-left lighting, warm thin contours, smooth painted illustration for Bilinear. Same portrait 640x800 composition centered x320 bottom y784. Change only contents and requested topping. REAL transparent alpha exterior, no text digits logos hands watermark checkerboard props backdrop or cast shadow.
