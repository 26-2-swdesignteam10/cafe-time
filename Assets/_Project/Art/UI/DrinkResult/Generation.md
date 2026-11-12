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
