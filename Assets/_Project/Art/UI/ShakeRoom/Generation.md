Generated with OpenAI built-in ImageGen + Python 3 / Pillow (2026-11-04 후처리). Style reference: ../Pos/Panel.png, ../Stations/PickupPanel.png, ../Stations/PickupSlot.png, ../Pos/ConfirmButton.png, ../Pos/DeleteButton.png, ../CafeHud/ToastNoCustomer.png, ../../Icons/Items/HotCoffee.png, ../../Sprites/Cafe/StationEspresso.png, ../Quit/Message.png, ../Espresso/Panel.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Bold single-line Korean pixel lettering, exact text, flat fill and outline colors.

## 2026-11-04 최종 보정 — SFR-052~055

최종 외곽 보정: 알파 이진화만으로 남는 어두운 재질 번짐은 재질 마스크와 계단형 부품 실루엣으로 제거했다. 세 믹서기에 동일한 실루엣 및 4px 커피색 경계를 적용하고 손잡이 구멍을 투명하게 유지했다. 뚜껑 오른쪽의 작은 색상 잡음을 단색 호두나무로 정리했다. 모든 출력의 완전 투명 픽셀은 RGB도 0으로 초기화했다. 공통 외형은 캔버스 중앙에 배치했다.

- Panel.png: 3200×1800, “쉐이크 제조실”. 프레임·금테·제목·콩 보존, 크림 내부 번짐을 #FAEAC2 / alpha 255로 교체.
- BlenderEmpty.png: 560×840. 내장 ImageGen으로 빈 믹서기 재생성 후 NEAREST 리사이즈 및 알파 임계값 128 이진화로 외곽 번짐 제거. 손잡이 구멍도 투명.
  - 수정 프롬프트: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: precise-object-edit. Clean this exact EMPTY blender sprite: remove ALL blurry brown halo outside machine, make silhouette edges crisp square stepped pixels with dark coffee #380F00 outline. Preserve exact front view shape, lid, right handle, EMPTY jar, amber control knob, base and colors, no text. Only one centered blender with transparent background and transparent handle hole; no cast shadow, no glow. Keep pixel geometry and same proportions. Target 560x840 portrait sprite.
- BlenderFilled.png: 560×840. 기존 생성된 딸기 원액·우유·얼음의 병 내부만 공통 외형에 마스크 합성. 뚜껑·손잡이·노브·본체 및 알파는 BlenderEmpty와 동일.
- BlenderBlended.png: 560×840. 기존 생성된 파스텔 분홍 스무디의 병 내부만 같은 외형에 합성. 세 상태의 외형·위치·알파 일치.
- BlendButton.png: 440×160, 허니 앰버 “갈기”. 사방 8px 투명 여백.
- PourButton.png: 440×160, 허니 앰버 “붓기”. 사방 8px 투명 여백.
- CupSpot.png: 440×140, 정면의 빈 코르크 받침. 사방 8px 투명 여백.
- LabelBase.png: “원액”, 글자 획 높이 56px.
- LabelMilk.png: “우유”, 글자 획 높이 56px.
- LabelCupSpot.png: “커피잔 자리”, 글자 획 높이 56px.
- Hint.png: 단일 행 “원액을 누르고, 얼음 스쿱을 믹서기로 끌어 넣으세요”, 글자 획 높이 56px. 쉼표와 공백 보존.

후처리: 라벨 획 마스크 → NEAREST → #380F00 글자 / #FCF0CA 3px 외곽선 → 외곽선 경계부터 사방 8px 투명 여백. PNG는 RGBA, 알파 0/255. 글자 높이는 외곽선 제외 기준이다. 한글 음절·쉼표·공백·단일 행을 눈으로 확인했다. .meta는 생성·수정하지 않았다.

---

Generated with built-in ImageGen. Style reference: ../Pos/Panel.png, ../Stations/PickupPanel.png, ../Stations/PickupSlot.png, ../Pos/PayButton.png, ../Pos/ConfirmButton.png, ../Pos/DeleteButton.png, ../CafeHud/ToastNoCustomer.png, ../../Icons/Items/HotCoffee.png, ../../Sprites/Cafe/StationEspresso.png, ../Quit/Message.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: ui-mockup. ONE isolated sprite, no extra objects or words. Match supplied STYLE reference, dark coffee #380F00 stepped outlines, 2-3 flat shade blocks, light top-left. 

???: 2026-11-04. SFR-044~055. ?? ?? ?? ???? ?? ???? ???. ?? ? ??? ???? ?? ??? ?? ? ?? ?? ??? ????? ?? ??? ????. ?? ?? ?? ?? ??? ?? ???? ???.

- Panel.png: Canvas 3200x1800 PNG. Walnut framed cream panel, exact painted title "??? ???" (? ? ? SPACE ? ? ?) at top center inside frame, two small beans. Rest of cream interior completely empty opaque flat #FAEAC2. Do NOT cut out cream interior. Transparent ONLY outside wood. Even 16px padding.
- BlenderEmpty.png: Canvas 560x840 PNG. ONE countertop blender, front orthographic view. Clear EMPTY glass jar with dark walnut lid and right handle, dark walnut/coffee motor base with honey amber unlettered round control. Jar, lid, base fixed shape centered, even 16px transparent padding. Glass subtle pale cream tint with block highlights, no text or measurement marks. Same pixel scale as reference device, 8px dark coffee outline. NO fruit milk ice yet.
- BlenderFilled.png: Edit ONLY jar contents: add chunky strawberry fruit puree at bottom, cream milk middle layer, chunky pale ice cubes above. Keep identical blender shape, lid, right handle, base, control knob, position and proportions. Remove all glow/shadows outside machine, outside silhouette MUST fully transparent. No text. Canvas 560x840.
- BlenderBlended.png: Edit ONLY jar contents: fill jar to 75 percent with smooth pastel pink blended smoothie, flat 2-3 shade blocks and a pale top surface. Keep identical blender shape, lid, right handle, base, control knob, position and proportions. Remove all glow/shadows outside machine, outside silhouette MUST fully transparent. No text. Canvas 560x840.
- BlendButton.png: Canvas 440x160 PNG. Honey amber primary beveled button, exact text "??" (? ?) dark coffee bold pixel letters centered. No ornaments or icons. Even 8px transparent margin.
- PourButton.png: Canvas 440x160 PNG. Honey amber primary beveled button, exact text "??" (? ?) dark coffee bold pixel letters centered. No ornaments or icons. Even 8px transparent margin.
- CupSpot.png: Canvas 440x140 PNG. ONE round cork coaster/mat viewed from FRONT, flattened shallow oval top with tiny visible thickness. Warm cork tan gold flat 2-3 shade blocks, dark coffee crisp pixel outline ~8px. Empty, no cup no text no decorations. Centered with 8px even transparent padding.
- LabelBase.png: LABEL ONLY exact Korean text "??". Single line with exact spaces and punctuation, text-only, no ornaments, no panel/button/background. Bold pixel Korean lettering, glyph body 56px tall S size, dark coffee #380F00 solid fill, cream #FCF0CA 3px outline. All syllables uniform height, spaced legibly, 8px transparent padding tightly around lettering. No bevel/shadows/texture, crisp stepped pixels. Spell every syllable exactly.
- LabelMilk.png: LABEL ONLY exact Korean text "??". Single line with exact spaces and punctuation, text-only, no ornaments, no panel/button/background. Bold pixel Korean lettering, glyph body 56px tall S size, dark coffee #380F00 solid fill, cream #FCF0CA 3px outline. All syllables uniform height, spaced legibly, 8px transparent padding tightly around lettering. No bevel/shadows/texture, crisp stepped pixels. Spell every syllable exactly.
- LabelCupSpot.png: LABEL ONLY exact Korean text "??? ??". Single line with exact spaces and punctuation, text-only, no ornaments, no panel/button/background. Bold pixel Korean lettering, glyph body 56px tall S size, dark coffee #380F00 solid fill, cream #FCF0CA 3px outline. All syllables uniform height, spaced legibly, 8px transparent padding tightly around lettering. No bevel/shadows/texture, crisp stepped pixels. Spell every syllable exactly.
- Hint.png: Text ONLY exact Korean sentence "??? ???, ?? ??? ???? ?? ????". Crucial: include visible COMMA immediately after ???, followed by one space. Maintain visible SPACES between words. Every glyph 56px high, dark coffee fill cream 3px outline, bold pixel font. Single horizontal line. No other symbols. Transparent. Crop tightly with 8px padding.

?? ???: ?? ?? ??? ??? ??, ??? ?? ??, ?? ??? ? ????, ?? ? ??? ?? ??? ?? ??. .meta? Unity?? ????.


## 2026-11-11 패널 제목·커피콩 제거

- 도구: Python 3 / Pillow. 단순 지우기·채우기 후처리이며 새 원화 생성은 없다.
- 스타일 참조: 이 폴더의 기존 `Panel.png`, `docs/ART_STYLE.md`. ADR-0019(승인)의 기기 창 제목 제거 요청을 적용했다. 새 ADR은 없다.
- `Panel.png` 작업 지시: “상단 중앙의 기존 제목과 양옆 커피콩만 지우고 주변 크림 바탕으로 이음매 없이 복원한다. 원래 프레임·금테·나머지 픽셀·캔버스 크기·알파를 보존한다.”
- 후처리: 원본 px 기준 사각형 `(686, 194, 2516, 419)`(오른쪽·아래 끝 제외) 안에서만 수정했다. 주변과 같은 불투명 크림 #FAEAC2로 채웠다. 이 사각형 밖 RGBA 픽셀이 원본과 완전히 일치하는지 검사했다. 전체 알파와 파일 크기도 원본과 같다.
- 결과: Panel.png — 3200×1800px. 지운 제목 외 새 글자는 없다.
- `.meta`, 코드, 씬, 프리팹, 에셋 정의는 수정하지 않았다. 픽업대·퇴식구 패널은 수정하지 않았다.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| Hint.png | 원액을 누르고, 얼음 스쿱을 믹서기로 끌어 넣으세요 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 886×78 → **1280×78 (폭 확대)** |
| LabelBase.png | 원액 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 140×78 |
| LabelCupSpot.png | 커피잔 자리 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 335×78 |
| LabelMilk.png | 우유 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 132×78 |
| BlendButton.png | 갈기 | 72px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 440×160 |
| PourButton.png | 붓기 | 72px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 440×160 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.


## 2026-11-12 쉐이크 제조실 일러스트 리디자인

이 절이 위의 과거 도트 규격·배치·문구보다 우선한다. **그림과 제안 배치만** 제작했으며 코드·씬에는 적용하지 않았다. 사용자 지정 동작은 승인 ADR-0029, 게임 폰트는 승인 ADR-0026을 따른다. 새 ADR은 없다.

- 도구: OpenAI 내장 `image_gen.imagegen` 원화 생성, 붓기 줄기 배경 제거 편집. Python 3 / Pillow로 투명 여백 크롭, Lanczos 크기 조정, 공통 캔버스 정렬, 생성 원화의 손잡이 재사용, 유리 전면 분리, 내용물 행별 재표본화·공통 마스킹, 애니메이션 시트 분할, Jua 조판, 미리보기 합성·검증.
- 스타일 참조: `docs/ART_STYLE.md` 1장 **기기 작업 창 일러스트 예외**, `../Water/Panel.png`, `../Water/HotZone.png`, `../Water/Generation.md`, `../DrinkTools/Generation.md`, `../DrinkRoom/Generation.md`, `../DrinkRoom/BigCupCeramic.png`, `../DrinkRoom/BigCupPlastic.png`. 부드러운 일러스트 / Bilinear 대상이며 도트 처리하지 않았다.
- 색 참조: `../DrinkResult/SmoothieStrawberry.png`의 붉은 분홍, `SmoothieMango.png`의 황금 노랑, `SmoothieHallabong.png`의 주황, `SmoothieMelon.png`의 연두, `CookieShake.png`의 크림 바탕+검은 쿠키 가루. Plain은 탁한 연한 크림색이다.
- 공통 프롬프트: “Production Unity isolated sprite, premium soft painted 2D cafe illustration matching reference Water machine and existing cups. Smooth clean charcoal contours, neutral brushed stainless steel, muted sage enamel, light oak, warm greige, soft upper-left illumination. Not pixel art, not photographic. Fully visible edges with padding, actual transparent background, no hands, no branding, no text or digits.” 아래 개별 지시를 함께 사용했다.
- 고정 글자는 `../../Fonts/Jua-Regular.ttf`로 4배 안티앨리어싱 조판 후 Lanczos 축소했다. 라벨 채움 높이 56px, 다크 커피 `#380F00` / 크림 `#FCF0CA` 3px 외곽선 / 외곽선 밖 사방 8px. 버튼 채움 높이 64px, 크림 채움 / 커피 3px 외곽선이다. 문구·띄어쓰기·마침표·느낌표를 실제 PNG에서 확인했다. LCD 가변 글자는 넣지 않았다.
- 기존 실행 원본 폴더 `C:/Users/k9425/.codex/generated_images/01a1cecf-f5f9-7c41-8144-696f513874a7/`를 확인했다. `exec-d4023f99-f7ff-41e1-9aee-5490c8a315b7.png`는 글자 없는 세이지 버튼이므로 재사용했다. `exec-f0056cc3-085e-433c-b3c0-d863b1ec27f7.png`는 도트 간판으로 이번 스타일·용도와 달라 사용하지 않았다. 쉐이크 부품 원화는 이 폴더에 없어 새로 생성했다.

### 파일별 프롬프트·후처리

| 파일 | 개별 프롬프트 / 후처리 | 최종 PNG 크기 |
|---|---|---|
| Panel.png | “Empty cafe kitchen workbench, 16:9; warm greige wall, light oak counter, slim rounded walnut frame and gold inset. Shallow raised shelf on LEFT half, spacious empty CENTER blender zone and RIGHT cup zone, empty upper-right. No objects, text, buttons or close icon.” 3200×1800으로 조정. 안쪽은 불투명, 둥근 모서리 바깥만 투명. | 3200×1800 |
| JarStrawberry.png | “Open squat wide cylindrical glass ingredient tub, strawberry puree rosy red-pink #e86c80, cream label with ripe strawberry illustration only, no scoop, lid or text.” 원화 실체를 공통 경계 x12..228, y104..312로 정렬. | 240×320 |
| JarMango.png | 같은 통, golden yellow mango #ffbe20, 망고 조각 그림 라벨. | 240×320 |
| JarHallabong.png | 같은 통, warm orange hallabong #f99b19, 꼭지가 볼록한 감귤·잎 그림 라벨. | 240×320 |
| JarMelon.png | 같은 통, pale yellow-green melon #b7d56d, 멜론 조각 그림 라벨. | 240×320 |
| JarCookie.png | 같은 통, ivory cookies-and-cream #e6d4c3 + dark cookie flecks, 검은 쿠키 그림 라벨. | 240×320 |
| ScoopInJar.png | 별도 생성 없음. ScoopStrawberry 원화의 실제 나무 손잡이·은색 자루만 크롭, 국자 머리·원액 제외, 회전해 공통 통 캔버스에 배치. 손잡이 끝은 위쪽, 자루 아래 끝은 통 입구 안쪽. | 240×320 |
| ScoopStrawberry.png | “One horizontal stainless ingredient ladle, wooden handle END on LEFT, deep bowl on RIGHT heaped with rosy pink strawberry puree and small fruit chunks. No hands, jars, falling drops or text.” 공통 손잡이 잡는 점으로 정렬. | 560×240 |
| ScoopMango.png | 같은 국자, golden yellow mango puree with mango chunks. | 560×240 |
| ScoopHallabong.png | 같은 국자, warm orange citrus puree with pulp segments. | 560×240 |
| ScoopMelon.png | 같은 국자, pale yellow-green melon puree with melon chunks. | 560×240 |
| ScoopCookie.png | 같은 국자, ivory cookies-and-cream puree with dark crushed cookie flecks. | 560×240 |
| BlenderBase.png | “ONE blender MOTOR BASE only, brushed stainless upper housing, sage lower housing, thin oak feet, dark top socket, front center large BLANK pale sage-grey rectangular LCD. No pitcher, lid, letters or digits.” 원본 지정 경계를 608×328로 조정해 (16,16)에 배치. | 640×360 |
| BlenderJar.png | “ONE empty OPEN glass blender pitcher only, no base or lid. Broad elliptical mouth, small RIGHT pouring spout, right handle, grey measurement ticks without digits, thick bottom and dark docking collar. Very transparent central glass, reflections along edges.” 원본 `(70,70)-(1135,1265)`를 664×744로 조정해 (28,20)에 배치. 전면 영역을 분리하고 내부 알파를 낮춰 빈 용기 뒤 배경이 비친다. | 720×780 |
| BlenderJarFront.png | 같은 생성 용기의 전면 유리·눈금·하이라이트만 분리. 내용물 앞에 놓아 유리 반사가 보인다. 공통 전면 마스크에 원본 알파의 62% 적용, 배경을 채우지 않음. | 720×780 |
| BlenderLid.png | 용기 입구를 참조해 “ONLY detached sage blender lid with charcoal sealing rim and central silver knob; seal the referenced mouth including right spout. No pitcher, base or text.” 뚜껑만 558×128로 조정해 용기와 같은 캔버스 (12,0)에 배치. | 720×780 |
| ChunkyStrawberry/Mango/Hallabong/Melon/Cookie/Plain.png | 맛별 **별도 생성 6장**. “Unblended ingredient fill TEXTURE ONLY, visible fruit chunks / cookie chunks, angular ice, cream milk layers and pooled puree. Flat top, full tapered mass bottom to top; NO glass, reflection, outline, container or garnish.” Plain은 우유와 얼음만. 각 행의 실제 질감 부분을 공통 내부 윤곽에 재표본화하여 위부터 아래까지 모두 채움. | 각각 390×474 |
| SmoothStrawberry/Mango/Hallabong/Melon/Cookie/Plain.png | 맛별 **별도 생성 6장**. “Fully blended thick silky smoothie fill TEXTURE ONLY, reference flavor color, tiny air bubbles, flat straight top, full tapered mass. NO glass, reflections, garnish or container.” Cookie는 연한 크림 바탕+작은 검은 가루, Plain은 탁한 연한 크림. Chunky와 동일한 공통 윤곽·크기. | 각각 390×474 |
| BlendWhirl0.png~BlendWhirl3.png | “Four equal-column animation sheet, sparse translucent WHITE/very pale grey twisting vortex ribbons and small droplets, four clockwise phases, no food, jar, glass, outline, text or backdrop.” 4등분하여 공통 내용물 크기로 정렬하고 내부 마스크 적용, 원본 알파 36%로 줄여 내용물이 비친다. | 각각 390×474 |
| SmoothieStream0.png~SmoothieStream3.png | “Four equal-column animation sheet of thick continuous viscous WHITE to PALE GREY smoothie streams, fixed center-top/center-bottom anchors, tiny bottom ripple, traveling creamy glints, no color, cup, jar or backdrop.” 첫 원화의 배경 헤이즈를 내장 ImageGen으로 제거 편집: “REMOVE all translucent background BETWEEN and OUTSIDE the streams, actual zero-alpha negative space; preserve four opaque light-grey streams, white highlights and bottom ripple.” 4등분·회색 변환·공통 크기. | 각각 96×360 |
| CupCoaster.png | “ONE empty shallow oval cork coaster, warm tan fine cork texture, slim sage enamel rim, no cup or text, no exterior shadow.” 실체를 544×124로 조정해 사방 8px 여백. | 560×140 |
| BlendButton.png | 기존 글자 없는 세이지·오크 버튼 원화 재사용, 424×144 실체와 사방 8px 여백, Jua `갈기`. | 440×160 |
| BlendButtonPressed.png | 같은 바탕·같은 캔버스, 눌림 상태는 재질 명도 82%, 실체 높이 132px·아래로 12px, 글자는 아래로 6px. Jua `갈기`. | 440×160 |
| PourButton.png | 같은 세이지·오크 버튼 바탕에 Jua `붓기`. | 440×160 |
| LabelBase.png | Jua `원액`, 라벨 공통 규격. | 133×78 |
| LabelMilk.png | Jua `우유`, 라벨 공통 규격. | 124×77 |
| LabelCupSpot.png | Jua `커피잔 자리`, 라벨 공통 규격. | 316×78 |
| Hint.png | Jua `국자와 우유 팩을 믹서기로 끌어 넣으세요. 얼음은 스쿱으로!`, 한 줄, 다크 커피+크림 외곽선. | 1464×78 |

- 통의 바닥 경계는 모두 PNG y312, 아래 투명 여백 8px. 다섯 통과 ScoopInJar는 **동일한 전체 캔버스**를 사용하고 투명 여백을 잘라 배치하지 않는다.
- 가득 뜬 국자 5종의 잡는 점: **원본 (30,100)px**, 표시 280×120에서 **(15,50)px**. 손잡이 왼쪽 끝의 잡는 영역이다. 마우스가 (mx,my)이면 전체 표시 rect는 **(mx−15,my−50,280,120)**. Unity의 정규화 pivot은 **(30/560,1−100/240) = (0.053571,0.583333)**이다. 국자 머리는 오른쪽 약 원본 (465,110), 표시 (232.5,55)에 있으나 사용자가 지정한 놓기 판정은 **마우스 좌표**를 사용한다. 별도 머리 위치 보정은 하지 않는다.
- 내용물의 공통 윤곽은 원본 좌표 **(0,0)→(389,0)→(339,473)→(50,473)**, 모든 맛·상태에서 동일하다. 최대 눈금에서 바닥까지 꽉 채운다. 중앙이 불투명하고 유리 반사·눈금은 포함하지 않는다. `Image Type=Filled, Fill Method=Vertical, Fill Origin=Bottom`으로 **크기·위치를 고정하고 fillAmount만 변경**한다. 60% 예시는 원본 위쪽 40%를 가린 상태다.
- BlenderJar/Front/Lid는 720×780 전체 캔버스를 같은 rect에 놓는다. Front의 투명 여백을 제거하면 유리·눈금이 어긋난다. Contents/Whirl도 공통 390×474 전체 캔버스 그대로 배치한다.
- 재사용: 정지·기울인 우유 팩과 MilkStream0~7은 `../DrinkTools/` 원본 그대로, ICE/HOT 빈 컵은 `../DrinkRoom/BigCupPlastic.png` / `BigCupCeramic.png` 원본 그대로 사용한다. 재사용 파일은 수정하지 않았다. 얼음 스쿱은 기존 그림을 사용하며 새로 만들지 않았다.
- **미사용**: 기존 `BlenderEmpty.png`, `BlenderFilled.png`, `BlenderBlended.png`, `CupSpot.png`. 삭제하지 않았다. 옛 단일 믹서기는 분리된 Base/Jar/Contents/Front/Lid로, 옛 CupSpot은 CupCoaster와 기존 큰 컵으로 대체한다.

### 배치 (2026-11-12)

창 **1600×900, 왼쪽 위 (0,0), x 오른쪽+, y 아래쪽+**, 단위는 **표시 px**, x/y는 투명 여백을 포함한 전체 캔버스의 왼쪽 위다. 신규 PNG는 표시 크기의 2배다. 재사용 우유 1000×800은 이 창에서 400×320으로 표시한다. 실제 코드 적용은 별도다.

| 요소 | x | y | w | h | 기준점·용도 |
|---|---:|---:|---:|---:|---|
| Panel | 0 | 0 | 1600 | 900 | 3200×1800 → 절반 표시 |
| Jar1 | 32 | 285 | 120 | 160 | JarStrawberry; ScoopInJar도 같은 rect |
| Jar2 | 152 | 285 | 120 | 160 | JarMango; ScoopInJar도 같은 rect |
| Jar3 | 272 | 285 | 120 | 160 | JarHallabong; ScoopInJar도 같은 rect |
| Jar4 | 392 | 285 | 120 | 160 | JarMelon; ScoopInJar도 같은 rect |
| Jar5 | 512 | 285 | 120 | 160 | JarCookie; ScoopInJar도 같은 rect |
| Milk | 125 | 430 | 400 | 320 | DrinkTools/MilkCarton.png 전체 1000×800, 0.4배 |
| MilkPour | 480 | 85 | 400 | 320 | DrinkTools/MilkCartonPour.png 전체 1000×800, 0.4배; 출구 (840,221) |
| MilkStream | 822 | 221 | 36 | 200 | DrinkTools/MilkStream0~7; (840,221) → 딸기60% 내부 (840,421) |
| BlenderBase | 765 | 560 | 320 | 180 | 640×360, 위쪽 소켓에 Jar 바닥 안착 |
| Weight | 847 | 626 | 164 | 39 | LCD 가변 글자 영역, TMP 중앙 정렬, PNG에는 비워 둠 |
| Jar | 750 | 205 | 360 | 390 | BlenderJar, BlenderJarFront, BlenderLid 공통 rect |
| Contents | 805 | 313 | 195 | 237 | Jar 기준 상대 (55,108,195,237); 최대 눈금=위끝 y313 |
| Cup | 1190 | 370 | 256 | 320 | BigCupPlastic / BigCupCeramic 전체512×640 → 256×320 |
| Coaster | 1178 | 660 | 280 | 70 | CupCoaster, 컵보다 먼저 그림 |
| BlendButton | 695 | 758 | 220 | 80 | 누르는 동안 BlendButtonPressed로 같은 rect 전환 |
| PourButton | 945 | 758 | 220 | 80 | 붓기 |
| Progress | 695 | 844 | 470 | 24 | 공용 게이지, 높이24 고정 |
| SmoothieStream | 1312 | 337 | 48 | 180 | PourPose에서 사용, 중심 위 (1336,337) → 컵 안 (1336,517) |
| LabelBase | 296.75 | 185 | 66.5 | 39 | Jua 고정 라벨 |
| LabelMilk | 249 | 753 | 62 | 38.5 | Jua 고정 라벨 |
| LabelCupSpot | 1239 | 710 | 158 | 39 | Jua 고정 라벨 |
| Hint | 434 | 55 | 732 | 39 | 우유 팩 따르기 자세보다 위쪽; 안내 문구 전체 한 줄 |

| Close 예약 | 1500 | 36 | 64 | 64 | 별도 기존 닫기 버튼, Panel에는 굽지 않음 |
| Tooltip 예약 | 36 | 224 | 596 | 54 | 통 위 여백, 짧은 재료 이름·설명 표시용 |
| ScoopInJar1~5 | Jar1~5와 같음 | Jar1~5와 같음 | 120 | 160 | 각 통의 rect에 공용 덧그림 |
| Lid | 750 | 205 | 360 | 390 | BlenderLid, Jar와 동일 |
| JarFront | 750 | 205 | 360 | 390 | BlenderJarFront, Jar와 동일 |
| Whirl | 805 | 313 | 195 | 237 | Contents와 동일, 같은 fillAmount로 자름 |
| JarPivot | 0.45 | 0.12 | — | — | Jar 내부 정규화, 아래쪽 y=0 |
| PourPose | 1030 | 390 | — | — | Jar pivot의 창 위치, 회전 −60° (반시계+) |

- **JarPivot**: Jar rect 안 정규화 **(0.45,0.12)**, 아래쪽 y=0. 원본 (324,686.4), 표시 (162,343.2), 정지 상태 창 좌표 **(912,548.2)**. 용기 손잡이·Contents·Front·Lid·Whirl을 같은 부모 아래 두고 함께 이동·회전한다. BlenderBase는 남는다.
- **Lid / JarFront**: Jar와 동일한 **(750,205,360,390)**. **Whirl**: Contents와 동일한 **(805,313,195,237)**, 내용물 위·Front 아래. 현재 내용물 fillAmount에 맞춰 Whirl도 같은 영역으로 잘라 빈 공간에 소용돌이가 나타나지 않게 한다.
- **레이어 순서**: Panel → Coaster → BlenderBase → 원액 통 → ScoopInJar → Milk → Cup → BlenderJar(뒤판·입구·손잡이·바닥) → Contents → Whirl → BlenderJarFront(전면 유리·눈금) → MilkStream → MilkPour → Lid → 라벨·버튼·Progress·Weight → Tooltip → Close. 우유 붓기에는 Lid를 숨긴다. 갈 때만 Lid와 Whirl을 표시한다. 끄는 동안 해당 통의 ScoopInJar만 숨기고 가득 뜬 Scoop을 마우스 위 레이어에 표시한다.
- **MilkStream 높이**는 내용물 높이에 따라 바꾼다. 60% 예시에서 수면은 `313+237×0.4=407.8`, 줄기는 그보다 약13px 아래인 y421에서 끝난다. 최대 내용물일 때는 h105 정도로 줄여 y326에서 끝내고, 빈 상태는 바닥 부근까지 늘린다. 우유 팩 출구 (900,340)×0.4+(480,85)=(840,221)는 고정한다. 우유를 놓으면 정지 rect로 복귀한다.
- **PourPose**: 용기의 **pivot 창 위치 (1030,390)**, **회전 −60°**(반시계+, 즉 오른쪽으로60° 기울임). 미리보기는 정지 Jar 자식인 내용물·Front까지 같은 변환을 적용했다. 이때 SmoothieStream rect **(1312,337,48,180)**, 위쪽을 용기 입구 안쪽에 조금 겹쳐서 접점을 숨기고 아래 파문은 ICE 컵 안쪽 y517에 둔다. 줄기는 용기보다 먼저 그리고 cup보다 뒤로 보내지 않는다. 용기와 함께 줄기 자체를 회전하지 않는다. HOT 컵은 입구가 더 낮으므로 끝점을 안쪽 약 y565로 늘리는 h228을 권장한다(시작점 동일). 실제 컵 타입에 맞춰 확인한다.
- 컵의 공통 바닥은 `370+632×0.5=686`, Coaster 윗면의 타원 내부에 놓인다. 도자기 잔도 같은 256×320 rect로 표시하며 손잡이·받침 포함 전체 그림을 보존한다.
- 원액 통 바닥 y441이 선반 위에 닿는다. 통의 실제 그림은 y337부터, 손잡이는 약 y291부터다. Tooltip 예약 y224..278은 손잡이 위에 있으며 원액 라벨 y185..224와 겹치지 않는다. 툴팁은 좌우 화면 밖으로 나가지 않게 예약 폭 안에서 위치를 제한한다.
- 애니메이션 순서: Whirl / SmoothieStream 모두 **0→1→2→3→0**, 검수용 권장 프레임당 0.1초(10fps). MilkStream은 기존0~7 규격을 재사용한다. 이는 에셋 검수값이며 게임 밸런스를 변경하지 않았다. 실제 동작 시간은 코드 담당자가 기존 데이터에 연결한다.

### 미리보기·검수·Unity 임포트

Assets 밖 스크래치 폴더:

`C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/fec95591-d3a3-47b1-89d7-0c4e1896d871/scratchpad/shake-20261112`

- `LayoutEmpty.png`: 1600×900, 빈 믹서기·정지 우유·ICE 컵.
- `LayoutMilk.png`: 1600×900, 딸기 Chunky **60%** + 기울인 우유 팩 + 기존 MilkStream. LCD `120g`는 검수 합성 전용이다.
- `LayoutPour.png`: 1600×900, PourPose 회전·이동 적용, 딸기 Smooth 내용물 + 회색 줄기를 분홍 `#ed8494`로 곱해 컵 안에 붓는 자세. 컵·팩은 재사용 원본이다.
- `LidContact.png`: 뚜껑·최대 내용물·눈금·소용돌이 접촉 확대. `AssetsReview.jpg`: 납품44장 모음. `BlendWhirlLoop.gif`, `SmoothieStreamLoop.gif`: 루프 프레임 비교용 재생. `Layout.json`은 위 배치와 동일한 좌표이며 `Manifest.json`, `Validation.json`은 규격·투명도·보존 확인용이다.
- 전체 합성3장과 에셋 모음을 눈으로 확인했다. 통 바닥·공용 국자·용기/본체 접점·내용물60%·유리 반사·뚜껑 맞물림·팩 출구/용기 내부·기울인 용기/줄기/컵 접점을 확인했다. 각 루프는 프레임별 모양과 공통 접점·크기를 확인했다. 글자 `갈기`, `붓기`, `원액`, `우유`, `커피잔 자리`, 전체 안내 문구의 철자와 공백을 확인했다.
- 신규44장 PNG는 RGBA, 모든 공통 캔버스 및 내용물 윤곽이 일치한다. 줄기는 회색만 사용하고 소용돌이는 반투명이다. 재사용 원본과 보호 파일(`.meta`, `.cs`, `.unity`, `.prefab`, `.asset`)은 직접 생성·수정하지 않았다. 기존 미사용 PNG도 삭제하지 않았다.
- 검증 결과: `Validation.json` 전체 통과, 보호 파일 1,096개 해시 일치. `bash .github/scripts/check-repo.sh` **통과 (추적 파일 1814개)**. 새 PNG 37장의 `.meta`는 Unity 임포트 전이므로 생성하지 않았다. 이 검사는 추적 파일 기준이어서 새 미추적 PNG의 `.meta`는 임포트 후 별도 확인해야 한다.
- Unity **6000.6.4f1**로 돌아가 임포트하여 **새 PNG의 .meta는 Unity가 자동 생성**하도록 한다. Sprite (2D and UI) / Single / **Bilinear** / Mip Maps 끔 / Alpha Is Transparency 켬 / Mesh Full Rect. Panel은 Max Size **4096**, 나머지2048. 기존 PNG의 `.meta`를 수정하지 않았으므로 기존 Panel·버튼·라벨도 이 설정을 Unity에서 확인한다.
- 코드 담당자는 위 표·레이어·pivot으로 연결한 뒤 Play에서 국자 숨김/복귀, 우유 출구·수면 접점, 세로 fill, 누르고 있는 동안 Lid/Whirl, 붓기 회전과 줄기 색, HOT/ICE 컵, 닫기·툴팁 공간을 확인한다. 이번 작업은 에디터 실행이나 씬 반영을 포함하지 않는다.

### 선택한 생성 원본 대응

새 원본은 `C:/Users/k9425/.codex/generated_images/01a1cf27-18de-74b2-8c19-eeda46b30928/`에 보존했다. 시트 분할·유리 전면 분리·국자 손잡이 추출은 위 표와 같다. 원화는 삭제하지 않았다.

| 최종 파일 / 시트 | 원본 |
|---|---|
| BlenderBase | `exec-e092e167-c0c2-41e4-a57f-f965d06f8ca8.png` |
| Panel | `exec-61d8c081-5877-4f1c-82df-8845892a1d4a.png` |
| BlenderJar | `exec-d2fdbf4f-b875-4613-a2a7-1ecb07a358f5.png` |
| CupCoaster | `exec-13d49334-958b-4083-a33a-600244ccb16c.png` |
| JarStrawberry | `exec-5618c737-645e-40b5-8b4d-6bc8d12d62c0.png` |
| JarMango | `exec-99e4b88b-2d77-4402-8387-1d24cfdbc714.png` |
| JarHallabong | `exec-b2d5fa5e-e029-4d62-bcd9-e93e41e36a16.png` |
| JarMelon | `exec-6c496a95-d613-499f-bebc-a9b9ae548da6.png` |
| JarCookie | `exec-81cc5df2-96b3-40ba-b2d0-1da3efe82c65.png` |
| ScoopStrawberry | `exec-cc230c96-8ac4-4ab4-8f33-045d62a01740.png` |
| ScoopMango | `exec-fa585f39-7b83-40d4-9ee0-c35334185c29.png` |
| ScoopHallabong | `exec-c5f4cf87-b197-4318-8202-6b283b834b18.png` |
| ScoopMelon | `exec-a0fd12dc-b58e-495e-8cf0-d7f24aa9b4f6.png` |
| ScoopCookie | `exec-e8d32d9b-0be0-4412-9398-180c3737654b.png` |
| ChunkyStrawberry | `exec-181cef76-0120-4481-a22b-613920d8fdcc.png` |
| ChunkyMango | `exec-80ed4e29-8917-4f18-98e3-83a79911dc1d.png` |
| ChunkyHallabong | `exec-e2946ae4-9015-4ea1-99a0-38d06624263a.png` |
| ChunkyMelon | `exec-8849bb1f-62f0-4047-a7b5-902beb0f644b.png` |
| ChunkyCookie | `exec-a81a4746-b1b8-4a62-b940-558919bd6997.png` |
| ChunkyPlain | `exec-0a9a7b2a-3d4b-4d66-a485-9d31c2e6fe53.png` |
| SmoothStrawberry | `exec-a9b469c5-3bce-48e9-9d87-35655d186bfa.png` |
| SmoothMango | `exec-aafaed50-aa04-4f0f-88ba-e3bfaa816f93.png` |
| SmoothHallabong | `exec-f80dfc32-a0d0-48c7-b011-4ac8b5d347df.png` |
| SmoothMelon | `exec-eb994a84-87ff-4180-86e3-03cc3d64ab6c.png` |
| SmoothCookie | `exec-95a3b682-18e9-4df0-b8fd-5ace0d84e447.png` |
| SmoothPlain | `exec-a7a776fe-0bd4-48cf-b263-a45c005cc9dd.png` |
| BlenderLid | `exec-e51c877c-a17a-4f06-958a-ba0fc52df642.png` |
| BlendWhirlSheet | `exec-67928887-87cc-42be-9a74-9cc074d5d600.png` |
| SmoothieStreamSheet | `exec-db4270e8-e6b7-4a50-879e-ad5bf48c0ed5.png` |
| BlendButton / BlendButtonPressed / PourButton | 앞서 지정한 기존 폴더 `exec-d4023f99-f7ff-41e1-9aee-5490c8a315b7.png` 재사용 |
