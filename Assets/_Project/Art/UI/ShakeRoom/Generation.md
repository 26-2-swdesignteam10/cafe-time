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
