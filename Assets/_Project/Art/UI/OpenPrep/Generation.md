Generated with the built-in ImageGen tool. Style reference: Assets/_Project/Art/UI/Quit/Panel.png, Assets/_Project/Art/UI/Quit/Yes.png, Assets/_Project/Art/UI/Quit/No.png, Assets/_Project/Art/UI/Settings/SoundNormal.png, Assets/_Project/Art/UI/Settings/SoundSelected.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Single isolated production UI sprite, front orthographic view, no perspective. Match the supplied reference sprites' chunky pixel contour, carved wood bevels and 2-3 flat shade blocks, not smooth gradients. Palette cream #FAEAC2, light cream #FCF0CA, shadow cream #F7DCA5, honey amber #DF7015, gold #C57D1F, light gold #F1C97D, walnut #90471B, shadow walnut #5D2608, dark coffee #380F00; small leaves may match reference green. No surrounding scene, no cast shadow outside the object, no watermark, no extra objects. Real transparent exterior; opaque object interior. 

- TopBar.png: TopBar.png: one extremely long thin horizontal carved walnut plank sign, width:height exactly 46:3 (1840x120 target). Gold inset inner rule, tiny coffee bean ornaments contained at both far ends, uninterrupted plain empty walnut center for DAY and money overlaid in code. No text, no lettering, no icon in the center. Draw entire 15.33:1 plank across the canvas, use transparent unused space if canvas differs, never thicken into an ordinary button. Little equal padding around all sides. 최종 PNG: 1840×120, 사방 4px 투명 여백.
- CatalogPanel.png: CatalogPanel.png: one rectangular catalog panel, width:height 60:43, target 1200x860. Straight continuous walnut frame, thin gold inset rule, fully opaque flat cream parchment interior, completely empty. Uniform frame thickness for 9-slice. No crest, no protruding ornament, no text, no icons, no subdivisions. Small equal exterior padding. 최종 PNG: 1200×860, 사방 8px 투명 여백.
- TabNormal.png: TabNormal.png: one small wide wooden tab button, width:height 18:7, target 180x70. Simple stepped rounded rectangular silhouette matching SoundNormal, pale light cream opaque center, walnut beveled rim, thin gold inner rule. Entire center empty, no text, no icon or ornaments. Little equal exterior padding. 최종 PNG: 360×140, 사방 4px 투명 여백.
- TabSelected.png: TabSelected.png: recreate the FIRST reference blank tab with IDENTICAL outer silhouette, dimensions, frame geometry, thickness and transparent margins. Change only pale cream center to warm honey amber #DF7015 with light gold #F1C97D highlight, slightly brighter, active pressed appearance with shallow inset upper bevel. Target 180x70 ratio 18:7. No text or icons. 최종 PNG: 360×140, 사방 4px 투명 여백.
- CardFrame.png: CardFrame.png: small ingredient card target 270x340 ratio 27:34. Thin walnut border ONLY 3% card width, cream flat interior, subtly rounded stepped pixel corners. The UPPER HALF contains a darker cream SQUARE blank icon area, side length 43% of entire card HEIGHT, centered horizontally, top inset 5% of height. The entire LOWER HALF is completely plain cream, generous blank text area, no inner frame around lower half. NO gold decorative flourishes, no text, no icons. Opaque interior, transparent outside. Minimal lightweight card, not a heavy ornate panel. 최종 PNG: 540×680, 사방 6px 투명 여백.
- MinusButton.png: MinusButton.png: one small SQUARE wooden quantity button, target 64x64. Simple chunky walnut beveled rim, pale cream center, thin honey gold highlight. Single centered dark coffee minus symbol '−', thick perfectly horizontal pixel stroke. No other text or ornament. Even small transparent margin. At small size must remain clearly legible. 최종 PNG: 128×128, 사방 4px 투명 여백.
- PlusButton.png: PlusButton.png: recreate the FIRST reference square minus quantity button with IDENTICAL outer silhouette, frame geometry, thickness, cream center and transparent margins. Replace only the center minus symbol by a dark coffee '+' PLUS sign, equal thick vertical and horizontal pixel strokes. Square target 64x64. No other changes, no other text or icons. 최종 PNG: 128×128, 사방 4px 투명 여백.
- CartPanel.png: CartPanel.png: one tall cafe order clipboard, width:height 30:43, target 600x860. Rectangular walnut clipboard board, small gold metal clip centered at top contained within board silhouette, opaque cream paper sheet inset with faint widely spaced straight cream-shadow horizontal ruling. Exact Korean title at top of paper, bold dark coffee pixel lettering with fine cream outline: '장바구니'. Spell syllables exactly 장 / 바 / 구 / 니. Title only in top 15 percent, paper remaining 85 percent is empty lined paper for code-drawn cart rows. No other text, no numbers, no doodles. Entire board and paper opaque, transparent outside with small even padding. 수정 프롬프트: Edit this clipboard only: keep EXACT Korean title 장바구니, the empty ruled opaque cream paper, walnut frame, palette and pixel art. Move the entire gold clip DOWN inside the top board region; make the top walnut edge a straight continuous horizontal edge with NO protruding clip or crest above it. All gold clip parts must be inside the rectangular walnut board silhouette. Preserve the generous paper text area and exact title. Tall clipboard width:height 30:43. Small even transparent padding outside board. 최종 PNG: 600×860, 사방 8px 투명 여백.
- BuyButton.png: BuyButton.png: one long low horizontal beveled walnut button, width:height 52:11, target 520x110. Honey amber #DF7015 center, warm gold inner rule and cream highlight. Exact centered Korean text '구매확정', four syllables 구 / 매 / 확 / 정, large bold dark coffee stepped pixel lettering with cream outline. Small shopping BASKET icon at left, small coffee leaf sprig at right. Same visual family as Quit/Yes reference, simple strong silhouette, all content contained in button. No additional lettering or marks, little equal transparent padding. 최종 PNG: 1040×220, 사방 6px 투명 여백.
- StartButton.png: StartButton.png: one long low horizontal beveled walnut button, width:height 52:11, target 520x110. Pale light cream #FCF0CA center, warm gold inner rule and cream highlight. Exact centered Korean text '영업 시작', syllables 영 / 업, space, 시 / 작. Large bold dark coffee stepped pixel lettering with cream outline. Small OPEN DOOR icon at left (NO English text), small coffee leaf sprig at right. Same visual family as Quit/No reference. Simple strong silhouette, all content contained in button. No additional lettering or marks, little equal transparent padding. 최종 PNG: 1040×220, 사방 6px 투명 여백.

후처리: 실제 오브젝트 경계로 투명 캔버스를 잘라낸 뒤 지정 비율로 크기를 조정했다. 계단식 픽셀 외곽선을 위해 알파 128 미만은 투명, 이상은 불투명으로 정리하고 최근접 보간을 사용했다. TopBar는 양 끝 커피콩 장식의 비율을 유지하며 중앙 구간을 가로로 확장했다.

검수: 고정 문구를 장·바·구·니 / 구·매·확·정 / 영·업 (공백) 시·작으로 눈으로 확인했다. 그 외 이름·재고·가격·수량·탭 문구·DAY는 포함하지 않았다. SFR-010.

Unity 임포트: Sprite (2D and UI), Single, Point (no filter), Generate Mip Maps 끔, Alpha Is Transparency 켬, Max Size 2048. .meta는 직접 생성하지 않았으므로 Unity에서 임포트해야 한다.

- CardFrame.png 최종 수정 프롬프트: Change only the icon placeholder of this card. Preserve EXACT 540x680 portrait card aspect, thin walnut border, empty lower half, palette and transparent exterior. Replace the current large wide inset with a SMALLER PERFECT SQUARE dark cream filled area in the UPPER HALF: 260 pixels wide by 260 pixels tall within the 540x680 composition, at x=140,y=45. Thus its bottom ends at y=305, above card midpoint y=340. No frame anywhere in lower half. No text, no icon, no ornaments. Square edges must be equal length. Entire interior opaque.


## 오픈 준비 고정 문구 스프라이트 — 2026-11-02

도구: 내장 ImageGen (CLI/API 미사용). 스타일 참조: `Assets/_Project/Art/UI/Quit/Message.png`, 같은 화면의 `TopBar.png`, `TabNormal.png`, `TabSelected.png`, `CardFrame.png`, `CartPanel.png`. 기존 PNG는 변경하지 않았다. SFR-010.

공통 생성 프롬프트:

> Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Style reference: Assets/_Project/Art/UI/Quit/Message.png as inspected in conversation: chunky bold Korean lettering, squared heavy strokes with stepped pixel corners. Related screen palette from OpenPrep/TopBar.png, TabNormal.png, TabSelected.png, CardFrame.png, CartPanel.png.

공통 제약 프롬프트:

> One isolated text-only Korean label sprite. Check every Hangul syllable and all punctuation. No panel, no button, no background or colored plate, no shadows, no underline, no other text. Actual transparent exterior and counters. Flat clean solid palette, no gradients or grain. No icons or ornaments except those explicitly requested. Single horizontal line, tightly cropped with exactly 8px transparent padding outside outermost pixels on every side. Korean ink glyph height excludes outline. Dark coffee #380F00 fill with cream #FCF0CA 3px outline by default. LIGHT: cream #FCF0CA fill with dark coffee #380F00 3px outline.

파일별 생성 프롬프트와 최종 규격:

- `TabCoffee.png`: Exact Korean text: "커피". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 130×78px, 글자 본체 56px, 사방 투명 여백 8px.
- `TabMilk.png`: Exact Korean text: "우유·액체". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 283×78px, 글자 본체 56px, 사방 투명 여백 8px.
- `TabSyrup.png`: Exact Korean text: "시럽·청". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 220×78px, 글자 본체 56px, 사방 투명 여백 8px.
- `TabTea.png`: Exact Korean text: "티". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 72×78px, 글자 본체 56px, 사방 투명 여백 8px.
- `TabSmoothie.png`: Exact Korean text: "스무디". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 198×78px, 글자 본체 56px, 사방 투명 여백 8px.
- `TabDessert.png`: Exact Korean text: "디저트". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 179×78px, 글자 본체 56px, 사방 투명 여백 8px.
- `LabelOpenPrep.png`: Exact Korean text: "오픈 준비". Korean ink glyph height EXACTLY 72 pixels excluding outline. LIGHT: cream #FCF0CA fill, dark coffee #380F00 outline, 3px thick. 최종 306×94px, 글자 본체 72px, 사방 투명 여백 8px.
- `LabelMoney.png`: Exact Korean text: "보유금". Korean ink glyph height EXACTLY 72 pixels excluding outline. LIGHT: cream #FCF0CA fill, dark coffee #380F00 outline, 3px thick. Include ONLY a tiny gold coin icon at left, at most 36px tall, vertically centered. 최종 280×94px, 글자 본체 72px, 사방 투명 여백 8px.
- `LabelStock.png`: Exact Korean text: "재고". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 139×78px, 글자 본체 56px, 사방 투명 여백 8px.
- `LabelTotal.png`: Exact Korean text: "총액". Korean ink glyph height EXACTLY 72 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. 최종 156×94px, 글자 본체 72px, 사방 투명 여백 8px.
- `CartEmpty.png`: Exact Korean text: "담은 재료가 없습니다.". Korean ink glyph height EXACTLY 56 pixels excluding outline. Faded brown #9E6840 fill, NO outline. 최종 492×72px, 글자 본체 56px, 사방 투명 여백 8px.
- `MessageBought.png`: Exact Korean text: "구매 완료!". Korean ink glyph height EXACTLY 56 pixels excluding outline. Dark coffee #380F00 fill, cream #FCF0CA outline, 3px thick. Include ONLY a tiny check mark at right, at most 28px tall, vertically centered. 최종 298×78px, 글자 본체 56px, 사방 투명 여백 8px.

- `ConfirmUnbought.png`: 아래 재시도 프롬프트로 생성한 결과를 채택했다. 첫 요청은 장시간 응답하지 않아 중단했다. 최종 2112×745px (기존 Quit/Message.png와 같은 가로세로 비율), 두 줄의 글자 본체 각각 96px, 크림 외곽선 3px. 확인창은 지정 비율을 유지하므로 일반 LABEL의 8px 타이트 크롭을 적용하지 않는다.

확인창 최종 생성 프롬프트:

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. A transparent text-only dialog message. Match the supplied reference Assets/_Project/Art/UI/Quit/Message.png: very bold chunky dark coffee #380F00 Korean pixel lettering with cream #FCF0CA outline, no ornaments. Wide canvas ratio 2112:745. Exactly TWO CENTERED LINES, preserve all spaces and punctuation:
장바구니에 구매하지 않은 재료가 있습니다.
그대로 영업을 시작할까요?
Spell each Hangul syllable correctly. Glyph ink height 96px on BOTH lines. First line wide, second line shorter and centered. No panel, no buttons, no backdrop, no coffee beans, no icons. Actual alpha transparency outside the letters. Crisp solid two-color stepped contours, no shadows, no gradients. File ConfirmUnbought.png.
```

후처리: 생성된 한글 획을 유지하며 실제 글자 본체 경계를 측정하고 최근접 보간으로 크기를 맞췄다. 생성 과정의 낮은 알파 잡티를 제거하고 지정 단색 팔레트 및 3px 계단식 외곽선으로 정리했다. 일반 LABEL은 외곽선 바깥 사방에 정확히 8px 투명 여백을 추가했다. 동전과 체크는 생성 원본의 아이콘을 분리해 각각 높이 36px, 28px로 축소했다. 확인창은 각 줄을 96px로 맞춘 뒤 2112×745 캔버스의 중앙에 배치했다. 새 글리프를 폰트로 그리거나 기존 PNG를 수정하지 않았다.

검수: 13개 파일의 한글 음절, 띄어쓰기, 가운데점(·), 마침표(.), 느낌표(!), 물음표(?)를 눈으로 대조했다. 모든 최종 PNG는 RGBA이며 실제 투명 영역이 있다. LABEL 12개의 본체 높이, 8px 여백, 지정 채움색을 픽셀 단위로 측정했다. 확인창은 두 줄 본체 높이가 각각 96px이며 가로 중앙은 1056px / 1055.5px이다. 기존 PNG 10개의 SHA-256이 작업 전후 동일함을 확인했다. 바뀌는 값은 포함하지 않았다.

| 파일 | 픽셀 크기 | 투명 | 측정 글자 본체 높이 |
|---|---|---|---|
| TabCoffee.png | 130×78 | 예 | 56px |
| TabMilk.png | 283×78 | 예 | 56px |
| TabSyrup.png | 220×78 | 예 | 56px |
| TabTea.png | 72×78 | 예 | 56px |
| TabSmoothie.png | 198×78 | 예 | 56px |
| TabDessert.png | 179×78 | 예 | 56px |
| LabelOpenPrep.png | 306×94 | 예 | 72px |
| LabelMoney.png | 280×94 | 예 | 72px |
| LabelStock.png | 139×78 | 예 | 56px |
| LabelTotal.png | 156×94 | 예 | 72px |
| CartEmpty.png | 492×72 | 예 | 56px |
| MessageBought.png | 298×78 | 예 | 56px |
| ConfirmUnbought.png | 2112×745 | 예 | 96px / 96px |

Unity 확인 순서:
1. Unity **6000.6.4f1** 에디터로 돌아가 새 PNG 13개를 임포트하고 `.meta`를 자동 생성한다. 에이전트는 `.meta`를 만들거나 수정하지 않았다.
2. 새 파일을 Sprite (2D and UI), Single, Point (no filter), Generate Mip Maps 끔, Alpha Is Transparency 켬으로 설정한다. Max Size는 ART_STYLE.md 기준인 2048을 사용한다. ConfirmUnbought의 원본 PNG는 2112×745이므로 Unity 임포트 시 Max Size 제한에 따라 축소될 수 있다.
3. 일반 LABEL을 원본 PNG의 절반 크기로 표시해 탭의 크림/앰버 양쪽과 상단의 진한 나무 위에서 확인한다. 확인창은 기존 Quit/Panel 안에서 두 줄과 문장 끝이 잘리지 않는지 확인한다. 씬·프리팹·코드는 이번 작업에서 변경하지 않았다.

새 ADR 또는 기존 ADR의 상태 변경은 없다.


## PR #8 교체 (2026-11)

Codex(AI 에이전트)로 다시 만들어 교체한 것으로 보인다. 도구 확인과 파일별 프롬프트는 작성자에게 받아 보충한다.

- 교체한 파일: LabelMoney.png
