Generated with OpenAI built-in ImageGen (기존 생성 원본) + Python 3 / Pillow (2026-11-04 후처리). Style reference: ../Pos/Panel.png, ../Stations/PickupPanel.png, ../Stations/PickupSlot.png, ../Pos/PayButton.png, ../Pos/ConfirmButton.png, ../Pos/DeleteButton.png, ../CafeHud/ToastNoCustomer.png, ../../Icons/Items/HotCoffee.png, ../../Sprites/Cafe/StationEspresso.png, ../Quit/Message.png, ../Espresso/Panel.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Bold single-line Korean pixel lettering, exact text, flat fill and outline colors.

## 2026-11-04 최종 보정 — SFR-044~051

최종 저장 시 완전 투명 픽셀의 RGB도 0으로 초기화했다. 저장소 검사 `bash .github/scripts/check-repo.sh`는 통과했다(추적 파일 855개). 새 에셋의 .meta 생성과 실제 Unity 화면 검수는 에디터 임포트 시 진행한다.

- Panel.png: 3200×1800, “음료 제조실”. 원래 프레임·금테·제목·콩을 보존하면서 내부 번짐을 #FAEAC2 / alpha 255로 교체.
- Shelf.png: 1840×1320. 기존 생성 판재를 NEAREST로 정렬하여 상단 y=300/620/940/1260, 판재 높이 44px. 빈 4단 선반과 양쪽 지지대.
- ShelfSlot.png: 240×240. 224×224 정사각 외형 및 사방 8px 투명 여백.
- ShelfSlotHover.png: 같은 슬롯의 실루엣에서 테두리를 #C57D1F / #F1C97D로 강조. 흐릿한 광원 없음.
- Scale.png: 720×420. 기존 생성 이미지를 구간별 NEAREST로 조정하여 판재 상단 y=80, LCD 내부 x=180..540 / y=240..360에 정렬. 글자 없음.
- EmptyButton.png: 440×160, “비우기”. 크림 보조 버튼, 사방 8px 투명 여백.
- CompleteButton.png: 520×180, “완성”. 허니 앰버 주요 버튼, 사방 8px 투명 여백.
- PourHint.png: 단일 행 “우유·탄산수는 누르고 있는 동안 부어집니다”. 글자 획 높이 56px. #380F00 글자 / #FCF0CA 외곽선.
- Tooltip.png: 680×192. 프레임 안쪽을 #5D2608 / alpha 255 단색으로 채워 번짐 제거.
- ResultPanel.png: 1280×760, 작은 제목 “판정 결과”. 크림 내부 #FAEAC2 / alpha 255.
- GradePerfect.png: “완벽”, 글자 획 72px. #D9A03A 글자 / #380F00 외곽선.
- GradeGood.png: “좋음”, 글자 획 72px. #380F00 글자 / #FCF0CA 외곽선.
- GradeBad.png: “나쁨”, 글자 획 72px. #380F00 글자 / #FCF0CA 외곽선.
- GradeFailed.png: “실패한 음료”, 글자 획 72px. #A3261B 글자 / #FCF0CA 외곽선.

후처리: 기존 라벨 획 마스크 추출 → NEAREST로 획 높이 정규화 → 지정 단색 및 3px 외곽선 재구성 → 외곽선 경계부터 사방 8px 투명 여백. PNG는 RGBA, 알파 0/255. 글자 높이는 외곽선 제외 기준이다. 한글 음절·공백·가운데점·단일 행을 눈으로 확인했다. .meta는 생성·수정하지 않았다.

---

Generated with built-in ImageGen. Style reference: ../Pos/Panel.png, ../Stations/PickupPanel.png, ../Stations/PickupSlot.png, ../Pos/PayButton.png, ../Pos/ConfirmButton.png, ../Pos/DeleteButton.png, ../CafeHud/ToastNoCustomer.png, ../../Icons/Items/HotCoffee.png, ../../Sprites/Cafe/StationEspresso.png, ../Quit/Message.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: ui-mockup. ONE isolated sprite, no extra objects or words. Match supplied STYLE reference, dark coffee #380F00 stepped outlines, 2-3 flat shade blocks, light top-left. 

???: 2026-11-04. SFR-044~055. ?? ?? ?? ???? ?? ???? ???. ?? ? ??? ???? ?? ??? ?? ? ?? ?? ??? ????? ?? ??? ????. ?? ?? ?? ?? ??? ?? ???? ???.

- Panel.png: ?? 3200?1800, ?? ??? ????, ??? 2?, ???? ???, ??? ?? ??. ?? ??/?? ?? ???.
- Shelf.png: 1840?1320, ? ???? ?? 4?, PNG ?? y=300/620/940/1260 ??. ??? ?? ?? ???.
- ShelfSlot.png: 240?240, ?? ???? ? ?? ???? ??, ???? ?? ? ?? ?? ?.
- ShelfSlotHover.png: Change ONLY the square frame into HOVER state. Preserve exactly same square shape, silhouette, centered position and empty cream interior. Highlight beveled border with honey gold bright block highlights, warm honey outline rim, crisp pixel glow not blur. Transparent outside. No text. Target 240x240.
- Scale.png: 720?420, ?? ?? ?? ??, ?? ??? ?? PNG y=80, ? ?? LCD x=180..540/y=240..360 ??. ?? ?? ?? ???.
- EmptyButton.png: 440?160, ?? ?? ?? ??, ??? ??? ?????.
- CompleteButton.png: 520?180, ?? ?? ?? ?? ??, ??? ??? ????.
- PourHint.png: LABEL text-only single line exact text "??????? ??? ?? ?? ?????". Spell ? ? ? ? ? ? ? SPACE ? ? ? SPACE ? ? SPACE ? ? SPACE ? ? ? ? ?. NO panel or ornaments, nothing except letters. All Korean glyphs exactly 56px high, dark coffee #380F00 fill, cream #FCF0CA 3px outline, 8px even transparent padding. Canvas tight wide around text.
- Tooltip.png: Canvas 680x192 PNG, dark walnut rounded rectangular empty tooltip background, stepped pixel corners, dark coffee outline, subtle bevel, uniform dark walnut #5D2608 opaque center. No text, no ornaments, 8px even transparent padding.
- ResultPanel.png: Change ONLY title to exact "?? ??". Make title small, two small beans. Keep rest of interior entirely uniform solid cream, fully opaque! Remove any shading or stains in cream, cream has constant color #FAEAC2 and alpha 255. Walnut frame unchanged. Exterior transparent. Target 1280x760.
- GradePerfect.png: LABEL text-only sprite, exact Korean text "??". Bold pixel Korean letters, gold #D9A03A fill and dark coffee #380F00 outline 3px thick. Korean glyph body height exactly 72px (M), all glyphs same height. Tight crop with 8px even transparent padding. Single line, exact spaces. No panel, button, decoration, beans, shadows, texture or background, ONLY text. Match reference letter shapes.
- GradeGood.png: LABEL text-only sprite, exact Korean text "??". Bold pixel Korean letters, dark coffee #380F00 fill and cream #FCF0CA outline 3px thick. Korean glyph body height exactly 72px (M), all glyphs same height. Tight crop with 8px even transparent padding. Single line, exact spaces. No panel, button, decoration, beans, shadows, texture or background, ONLY text. Match reference letter shapes.
- GradeBad.png: LABEL text-only sprite, exact Korean text "??". Bold pixel Korean letters, dark coffee #380F00 fill and cream #FCF0CA outline 3px thick. Korean glyph body height exactly 72px (M), all glyphs same height. Tight crop with 8px even transparent padding. Single line, exact spaces. No panel, button, decoration, beans, shadows, texture or background, ONLY text. Match reference letter shapes.
- GradeFailed.png: LABEL text-only sprite, exact Korean text "??? ??". Bold pixel Korean letters, deep brick red #A3261B fill and cream #FCF0CA outline 3px thick. Korean glyph body height exactly 72px (M), all glyphs same height. Tight crop with 8px even transparent padding. Single line, exact spaces. No panel, button, decoration, beans, shadows, texture or background, ONLY text. Match reference letter shapes.

?? ???: ?? ?? ??? ??? ??, ??? ?? ??, ?? ??? ? ????, ?? ? ??? ?? ??? ?? ??. .meta? Unity?? ????.

## PR #8 교체 (2026-11)

Codex(AI 에이전트)로 다시 만들어 교체한 것으로 보인다. 도구 확인과 파일별 프롬프트는 작성자에게 받아 보충한다.

- 교체한 파일: Scale.png

## 2026-11-09 영업 중 일러스트 교체

도구: 이전 작업에서 OpenAI 내장 `image_gen.imagegen`으로 생성한 원화와 PowerShell `System.Drawing`의 고품질 축소·합성. 새 생성 요청은 429 사용 한도에 걸려 원화 재사용만 했다. 스타일 참조: `../ShotRoom/Panel.png`, `../ShotRoom/Generation.md`, 이 폴더의 기존 PNG(정확한 크기와 문구), `StationUiBuilder.BuildDrinkRoom`(배치).

공통 프롬프트(기존 원화 제작 시 사용):

> Premium hand-painted 2D illustration for a cozy Korean cafe game. Clean charcoal contours, tactile neutral brushed stainless steel, matte black handles, light oak, warm greige wall, restrained sage accents. Soft upper-left light, crisp highlights and coherent material rendering. No photographic finish, no vector-like flat shapes, no text inside AI art.

파일별 원화·프롬프트 및 수정:

- `Panel.png` — `exec-6d1c9ab6-7fcd-46df-8d32-e0c8623887c8.png`: 목재 프레임, 그릭 벽, 바닥에 닿는 오크 하부장과 상판이 연결된 빈 카페 작업대. 3200×1800으로 확대하고 원래 문구 `음료 제조실`을 상단에 넣었다.
- `Shelf.png` — `exec-731cca30-5c56-4410-9a1b-b3cfcca8863c.png`: 스테인리스 지지대가 있는 4단 오크 선반. 원화의 투명 배경을 유지해 1840×1320으로 축소했다.
- `ShelfSlot.png`, `ShelfSlotHover.png` — `exec-c1fd11c5-f01c-4e72-aa25-00cd4dfb5a9b.png`: 위에서 본 빈 중성 스테인리스 트레이와 오크 받침. 동일한 몸체를 240×240에 넣고 Hover는 세이지 색만 강조했다.
- `Scale.png` — `exec-02ba407a-d553-4213-bfae-9d3d81194cfd.png`: 브러시드 스테인리스 저울, 검정 LCD와 버튼, 받침발. 720×420으로 축소했다.
- `EmptyButton.png`, `CompleteButton.png`, `Tooltip.png` — `exec-ef47f3fa-24a4-49b8-9628-4fea533f0c0f.png`: 세이지 무광 에나멜 버튼 몸체. 원래 문구 `비우기`, `완성`을 버튼에 넣고 Tooltip은 빈 몸체로 썼다. 각각 기존 캔버스 크기를 유지했다.
- `ResultPanel.png` — `exec-15377f8b-553f-4ba3-9508-7d6bdb375728.png`: 목재 프레임과 빈 그릭 벽·오크 카운터 장면. 1280×760으로 축소하고 원래 문구 `판정 결과`를 넣었다.
- `PourHint.png`, `GradePerfect.png`, `GradeGood.png`, `GradeBad.png`, `GradeFailed.png` — 기존 PNG의 문구 `우유·탄산수는 누르고 있는 동안 부어집니다`, `완벽`, `좋음`, `나쁨`, `실패한 음료`를 Galmuri11로 재조판했다. 어두운 커피색 `#380F00` 채움과 크림색 `#FCF0CA` 외곽선, 투명 배경을 적용했다.

모든 PNG의 파일명과 캔버스 크기를 유지했다. 2배 배치 미리보기는 프로젝트 밖 `%TEMP%/cafe-time-illustration-preview/DrinkRoom2x.png`에 저장했다. 선반·저울의 받침과 슬롯 위치, 문구를 확인했다. 빌더 좌표 `(scale: 1100,300; cup icon: 96,42)`에서는 컵 아이콘이 저울 상판보다 떠 보인다. 빌더는 이번 범위 밖이므로 수정하지 않았다.


## 2026-11-11 제목 제거·큰 컵·버튼 라벨 분리

- 도구: OpenAI 내장 `image_gen.imagegen`(컵 6종 시트, 한글 라벨 시트), Python 3 / Pillow(크롭, 공통 외형 합성, 단순 바탕 복원, 마스크 색상 정리, 크기·여백·배치 검수).
- 스타일 참조: `../ShotRoom/Panel.png`, `Scale.png`, `../../Icons/Items/CupCeramic.png`, `../../Icons/Items/CupPlastic.png`, 기존 `EmptyButton.png`·`CompleteButton.png`, `docs/ART_STYLE.md`의 기기 창 일러스트 예외. 기존 도자기 컵처럼 오른쪽 손잡이와 받침을 유지했다.
- 생성 원본 폴더: `C:/Users/k9425/.codex/generated_images/01a1c935-9f02-72b2-a560-6cd09db60a52/`. 원본을 보존했다.
- 컵 공통 프롬프트: “Six production sprites, exact 3-column 2-row sheet, no labels. Polished hand-painted 2D cafe game art, clean thin warm charcoal outlines, refined highlights, soft ceramic shading, translucent clear plastic, NOT pixel art. Same slightly elevated front camera, visible top ellipse, upper-left light, same geometry/scale/position per vessel. Ceramic mug with right handle and saucer in top row, lidless tapered takeaway cup with rolled rim in bottom row. Empty / clear water / light coffee-brown mixture from left to right. Truly transparent background, no cast shadow, no logos or text, generous gutters.” 원본 `exec-c11cbf3c-e025-433e-a9ee-096725500f39.png`.

### 파일별 프롬프트·후처리

| 파일 | 작업 지시 / 프롬프트 | 최종 크기 |
|---|---|---|
| Panel.png | 기존 상단 제목만 제거. 원본 `(1638,105)-(2270,237)` 안에서 같은 높이 좌우 벽색을 보간하고, 아래쪽 빈 벽의 미세 질감을 복사해 빛 번짐과 벽 표면을 복원. 이 사각형 밖 프레임·금테·나머지 RGBA 픽셀은 원본 그대로. | 3200×1800 |
| BigCupCeramic.png | White ceramic mug with right handle and saucer matching CupCeramic, empty. | 512×640 |
| BigCupCeramicWater.png | Same ceramic mug filled with clear hot water, very subtle steam. | 512×640 |
| BigCupCeramicMixing.png | Same ceramic mug with light brown coffee/milk mixture, no garnish. | 512×640 |
| BigCupPlastic.png | Transparent tapered disposable plastic cup, rolled rim, no lid/straw, empty. | 512×640 |
| BigCupPlasticWater.png | Same clear cup filled with clear cold water. | 512×640 |
| BigCupPlasticMixing.png | Same clear cup filled with caramel-brown mixed drink. | 512×640 |
| EmptyButton.png | 비우기 글자만 지우고 주변 세이지 에나멜 색으로 채움. 수정 영역 `(87,32)-(284,104)` 밖의 모양·베벨·색·알파 그대로. | 440×160 |
| CompleteButton.png | 완성 글자만 지우고 같은 세이지 바탕 복원. 수정 영역 `(147,40)-(286,116)` 밖의 모양·베벨·색·알파 그대로. | 520×180 |
| LabelEmpty.png | Text only exact Korean “비우기”, Galmuri11-like bold squared pixel Hangul, cream fill, dark coffee outline, no button/background/shadow. | 192×80 |
| LabelComplete.png | Text only exact Korean “완성”, same lettering style/colors, no button/background/shadow. | 156×88 |

- 컵 후처리: 시트를 셀별로 크롭하고, 낮은 알파 헤이즈를 제거했다. 각 종류의 빈 컵 외형을 공통으로 재사용하고 생성된 내용물 영역만 마스크로 합성했다. 도자기의 손잡이·받침·외벽, 플라스틱의 외부 윤곽·바닥은 상태가 바뀌어도 고정된다. 뜨거운 물에는 생성 원화의 김을 추가했다. Lanczos로 크기를 맞추고 공통 512×640 RGBA 캔버스에 배치했다. 도자기 전체 폭 448px, 플라스틱 폭 360px, 공통 가로 중심 x=256, 바닥 경계 y=632(캔버스 아래 8px 투명 여백)다. 김을 위한 위쪽 공간을 남겼다. 글자는 없다.
- 라벨 공통 생성 프롬프트: “Two text-only Korean sprites, exact top row 비우기 / bottom row 완성. Match Galmuri11 very bold square pixel Hangul of supplied buttons. Flat cream #FCF0CA fill, dark coffee #380F00 outline, transparent letter counters and exterior. No button, panel, ornaments, glow or shadow.” 원본 `exec-dfc8d3c2-0241-4c67-9c42-9325b3073481.png`.
- 라벨 후처리: 생성된 크림 글자 마스크만 추출하고 최근접 리샘플링으로 외곽선 포함 높이를 각각 64px·72px(각 원본 버튼 높이의 40%)로 맞췄다. 크림 #FCF0CA 채움 / 다크 커피 #380F00 3px 외곽선, 사방 정확히 8px 투명 여백. 절반 크기로 얹으면 글자 높이는 32px·36px이다. 실제 버튼 위 가운데에 얹어 `비 우 기`, `완 성`의 철자를 눈으로 대조했다.

### 배치 (2026-11-11)

1600×900 창 왼쪽 위 기준 화면 px. PNG는 아래 w/h 전체 캔버스를 사용한다. 컵은 종류·상태와 무관하게 같은 RectTransform을 쓴다. 선반 위치는 요청대로 유지한다.

| 요소 | x | y | w | h |
|---|---:|---:|---:|---:|
| 재료 선반 Shelf (유지) | 100 | 200 | 920 | 660 |
| 저울 Scale | 1110 | 460 | 360 | 210 |
| 큰 컵 BigCup 6종 공통 | 1162 | 184 | 256 | 320 |
| 저울 LCD 무게 글자 영역 | 1200 | 580 | 180 | 60 |
| 비우기 빈 버튼 | 1050 | 712 | 220 | 80 |
| 비우기 라벨 LabelEmpty | 1112 | 732 | 96 | 40 |
| 완성 빈 버튼 | 1290 | 707 | 260 | 90 |
| 완성 라벨 LabelComplete | 1381 | 730 | 78 | 44 |

- 저울 중심과 컵 캔버스 중심은 x=1290. 상판 윗면은 `460 + 80×0.5 = 500`, 컵 바닥은 `184 + 632×0.5 = 500`이므로 지지선이 일치한다. 컵 캔버스 아래 투명 여백은 화면 4px이다.
- 무게 글자 영역은 지정된 원본 LCD `x180..540, y240..360`을 0.5배로 환산했다. 값은 TextMeshPro로 가운데 정렬하며 PNG에 굽지 않는다.
- 두 버튼 중심 높이는 y=752이고 라벨 중심도 각각 일치한다. 선반과 오른쪽 조작부 사이 간격을 두었고, 닫기 버튼 `(1500,36)`과 양손 HUD `(640..960,y>780)`를 피했다.
- 미리보기: 프로젝트 밖 `%TEMP%/cafe-time-ui-20261111/LayoutCeramicWater.png`, `LayoutPlasticMixing.png`, `ButtonReview.png`. 무게 `320 g`와 HUD 예약 영역 표시는 미리보기 전용이다. 실제 코드·씬에 배치를 적용하지 않았다.

### 검수·Unity 임포트

- 기존 PNG의 파일명·픽셀 크기·전체 알파를 유지하고 지정 복원 사각형 밖 픽셀의 완전 일치를 검사했다. 새 컵 RGBA·공통 크기·하단 여백, 라벨 사방 8px 여백, 한글 철자를 확인했다.
- Unity 6000.6.4f1로 돌아가 새 PNG를 임포트한다. 컵·빈 버튼·패널은 Sprite / Single / Bilinear / Mip Maps 끔 / Alpha Is Transparency 켬, 글자 라벨은 Point 권장. 3200px 패널은 Max Size 4096을 확인한다. 새 PNG의 `.meta`는 Unity가 생성하도록 둔다.
- `.meta`, `.cs`, `.unity`, `.prefab`, `.asset`은 직접 생성·수정하지 않았다. 기존 승인 ADR-0019의 요청을 이행했으며 새 ADR은 없다.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| GradeBad.png | 나쁨 | 72px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 180×94 |
| GradeFailed.png | 실패한 음료 | 72px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 365×94 → **389×94 (폭 확대)** |
| GradeGood.png | 좋음 | 72px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 148×94 → **155×94 (폭 확대)** |
| GradePerfect.png | 완벽 | 72px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 163×94 |
| LabelComplete.png | 완성 | 56px | #FCF0CA | #380F00 3px | Pillow Jua 4배 안티앨리어싱 조판 | 156×88 |
| LabelEmpty.png | 비우기 | 56px | #FCF0CA | #380F00 3px | Pillow Jua 4배 안티앨리어싱 조판 | 192×80 |
| PourHint.png | 우유·탄산수는 누르고 있는 동안 부어집니다 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 989×78 → **1063×78 (폭 확대)** |
| ResultPanel.png | 판정 결과 | 56px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1280×760 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.


## 2026-11-17 컵 내용물 런타임 합성 레이어

- 도구: OpenAI 내장 `image_gen.imagegen` (얼음·라테아트), Python 3 / Pillow (원본 픽셀 추출, 정렬 마스크, 중성 명암 텍스처, 타일, 크기 정규화, 알파 정리, 검수 합성).
- 스타일 참조: 이 폴더의 `BigCupPlastic.png`, `BigCupPlasticWater.png`, `BigCupPlasticMixing.png`, `BigCupCeramic.png`, `BigCupCeramicWater.png`, `BigCupCeramicMixing.png`. `docs/ART_STYLE.md`의 기기 작업 창 예외에 따라 부드럽고 깔끔한 회화풍을 유지한다.
- 공통 프롬프트 시작: “Production Unity sprites for a cozy Korean pixel art cafe game.” 이어 기기 창 예외인 깨끗한 일러스트, 기존 컵과 같은 시점·왼쪽 위 조명, 실제 투명 배경, 글자 없음, 컵 외형 변경 없음으로 한정했다. 도트 외곽선을 추가하지 않았다.
- 생성 원본: `C:/Users/k9425/.codex/generated_images/01a1e942-27c6-7b32-8e83-4900e95404a3/exec-9bcd71d7-6862-4309-b139-3a98b35e65dd.png` (얼음), 같은 폴더 `exec-68f03488-9d16-40be-9866-81142eba8ed5.png` (폼).

| 파일 | 크기 | 프롬프트 / 공정 |
|---|---|---|
| `CupPlasticLiquidMask.png` | 512×640 | 원본 플라스틱 내벽과 바닥을 따라 이진 알파 마스크를 작성. 상단 타원은 중심 (254,280), 가로 반지름 148, 세로 반지름 32. 흰색 RGBA/투명 이외 색 없음. |
| `LiquidShade.png` | 128×8 | 가로 회색 명암 210..255 수준, 왼쪽 약 22% 위치에 얇고 은은한 흰 반사 띠, 오른쪽 끝은 더 어둡게. 세로 모든 행이 같아 늘려도 명암 유지. |
| `LiquidSurface.png` | 400×64 | 재료색으로 곱셈 틴트할 중성 회백색 타원. 위쪽 림 그림자·테두리 반사·왼쪽 빛을 약하게 넣고 4배 해상도에서 안티앨리어싱 축소. |
| `BubblesTile.png` | 64×64 | 서로 다른 크기 작은 흰 반투명 기포 7개. 타일 경계 기포는 반대편으로 반복하여 연결. 내부는 매우 옅고 가장자리·왼쪽 반사는 조금 밝게. |
| `IceCubes.png` | 300×170 | 내장 image_gen: 기존 플라스틱 컵과 같은 약간 내려다보는 시점, 겹친 반투명 청백색 얼음 네 개, 왼쪽 위 반사, 컵·액체·그림자·글자 없이 실제 투명 배경. 생성 알파 기준으로 여백을 크롭한 뒤 비율 유지 축소, 300×170에 가운데 정렬. |
| `CupPlasticFront.png` | 512×640 | BigCupPlastic.png 원본 RGB를 그대로 복사하고 앞쪽 입술·벽 외곽선·세로 흰 반사·전면 띠·아래 림에만 감쇠 알파를 적용. 뒤쪽 입구 타원과 불투명한 회색 벽 바탕은 제외. 반사 시작은 부드럽게 페이드하여 사각 절단을 없앰. |
| `CupCeramicSurface.png` | 512×640 | BigCupCeramicMixing.png의 갈색 액면을 행별 윤곽으로 추출. 붉은 채널의 명암을 밝은 중성 회색으로 변환하여 원본 동심원·잔물결·림 그림자 보존. 손잡이·벽·받침 없음. |
| `CupCeramicFoam.png` | 512×640 | 내장 image_gen: 기존 도자기 라테와 같은 시점과 깔끔한 회화풍, 크림색 하트와 가는 두 겹의 깃털 꼬리만 생성. 갈색 액체·컵·받침·글자 없음. 생성 투명 알파 보존, 166×62로 축소하여 (155,319)에 배치. |

### 정렬·계량

좌표는 512×640 캔버스 왼쪽 위가 (0,0), y는 아래로 증가한다. x 범위는 양끝 픽셀 포함이다. UI 256×320에서는 모든 값을 0.5배 한다.

| 항목 | 캔버스 px |
|---|---|
| 플라스틱 내부 액체 바닥 | y=604, x=178..330 (153픽셀) |
| 플라스틱 바닥 위 넓은 내부 단면 | y=583, x=158..350 (193픽셀) |
| 플라스틱 최대 채움선·액면 중심선 | y=280, x=106..402 (297픽셀), 가로 중심 x=254 |
| 플라스틱 높이 계산 구간 | 604−280=324 |
| 마스크 최상단 | y=248 (최대 액면의 뒤쪽 타원 꼭대기) |
| 도자기 액면 | 중심 (238,350.5), 알파 경계 bbox [112,301,364,400), 252×99 |
| 도자기 액면의 실용 타원 규격 | 약 252×100, 원본 갈색 액면 윤곽을 그대로 추출 |
| LiquidSurface 권장 높이 | 64 (UI 32), 가로는 해당 높이의 컵 내경으로 조절 |
| 도자기 폼 배치 | [155,319,321,381), 166×62, 중심 (238,350) |

플라스틱 높이 `h=max(6,324×양(g)/330)`; 샷 1회=30g; 얼음은 높이에 포함하지 않는다. 넘침은 최대 채움선 y=280에서 제한한다. 레이어는 넣은 순서대로 바닥에서 위로 쌓고, 가장 위 레이어의 액면 중심에 LiquidSurface를 둔다. 마스크 뒤쪽 액면 꼭대기가 y=248이므로 마스크 bbox 높이(357)를 용량 계산에 쓰지 않는다. 얇은 층도 최소 6px를 유지하되 총량이 용량을 넘으면 잘라낸다.

미리보기의 층 높이: 200g=196.36px, 샷 30g=29.45px, 청 40g=39.27px. 라테·아메리카노의 액면 y=378.18, 딸기에이드 y=368.36. 픽셀 합성 시 경계를 반올림한다. 얼음 원본 300×170은 미리보기에서 210×119로 배치하고 액면보다 78px 위에서 시작한다. 얼음도 액체 마스크 안에서 클리핑한다.

### 색상과 검수

- 재료별 액체 틴트 제안은 `docs/ART_STYLE.md` 9장의 `#RRGGBBAA` 표를 따른다. 물·탄산은 낮은 알파, 우유·스무디는 높은 알파다. 티백색은 별도의 티백 두께 레이어가 아니라 우린 물 전체에 적용한다. shot의 크레마 느낌은 LiquidSurface의 밝은 테두리와 함께 표현한다.
- 기존 BigCup 6종은 수정하지 않았다. 8개 새 PNG는 RGBA, 완전 투명 픽셀 RGB=0. 정렬되는 4종(마스크·전면·도자기 액면·폼)은 모두 512×640이다.
- 프로젝트 밖 `scratchpad/cup_preview.png`는 1280×320, 왼쪽부터 우유200g→샷1회+얼음 / 딸기청40g→탄산200g+얼음 / 샷1회→찬물200g+얼음 / 도자기 핫라테+폼 / 도자기 얼그레이다. 핫라테 미리보기 틴트는 #D9AC79FF, 얼그레이는 표의 차색이다.
- 첫 합성 뒤 전면 반사의 사각 시작점과 바닥의 잘린 반사를 제거해 다시 검수했다. 용량·최소 층 높이는 이번 미리보기 기준이며 게임 밸런스 에셋은 변경하지 않았다.
- Unity 6000.6.4f1에서 새 PNG를 임포트해 `.meta`를 자동 생성한다. Sprite / Single / Full Rect / Bilinear / Mip Maps 끔 / Alpha Is Transparency 켬 / 압축 없음 권장. 정렬 에셋은 원본 투명 여백을 포함한 전체 캔버스로 사용한다. `BubblesTile`은 Wrap Repeat, uGUI Image Type=Tiled로 쓰고 타일 크기 64캔버스px(UI 32px)에 맞는 Pixels Per Unit을 확인한다. uGUI Mask의 Show Mask Graphic은 끈다.
- `.meta`, `.unity`, `.prefab`, `.asset`은 직접 생성·수정하지 않았다. 새 ADR 없음(요청된 에셋 규격 이행).

## 2026-11-17 컵 전체를 채우는 얼음 레이어

- 도구: OpenAI 내장 `image_gen.imagegen`(얼음 원화), Python 3 / Pillow(크기·좌표 정렬, 알파 감쇠, 마스크 클리핑, 투명 RGB 정리, 합성 검수).
- 스타일 참조: 같은 폴더의 `BigCupPlastic.png`, `IceCubes.png`. 기기 작업 창 예외에 맞춰 기존의 깔끔한 회화풍, 약간 내려다보는 시점과 왼쪽 위 조명을 유지했다.
- `IceFill.png` — **512×640 RGBA**. 파일별 프롬프트: 「Production Unity sprites for a cozy Korean pixel art cafe game. 기존 컵과 얼음을 참조하여, 컵·액체·그림자·글자 없이 실제 투명 배경에 컵 내부 전체를 채우는 얼음만 만든다. 옅은 청백색 반투명 얼음 약 25~30개를 서로 다른 회전으로 여러 줄 쌓는다. 최대 채움선 y≈280부터 안쪽 바닥 y≈604까지 빈 중심 없이 채우고, 위쪽 일부는 y≈255까지 올라오게 한다. 위쪽 폭 약296px에서 바닥 폭153px로 좁아지는 형태, 중심 x=254. 얇은 반사와 둥근 모서리, 뒤 음료색이 보이는 투명한 면, 도트 외곽선 없음.」
- 생성 원본: `C:/Users/k9425/.codex/generated_images/01a1e962-c7b0-7083-be6f-431a581390fb/exec-5b852c54-54ee-4c8f-9196-96f40e72fa4a.png`. 알파≥64 경계로 크롭한 뒤 296×352로 Lanczos 축소하고 전체 캔버스의 **(106,253)**에 배치했다. `CupPlasticLiquidMask.png`의 알파와 곱해 컵 안쪽에 한정했다. 최종 알파 경계는 **[110,253,400,605)**이며 y=604 바닥까지 이어진다.
- 첫 검수의 알파 0.64배보다 음료색이 잘 보이도록 최종은 생성 알파의 **0.46배**를 사용했다. 최종 최대 알파 **117/255**, 유효 픽셀 평균 약113.8/255. 완전 투명 픽셀은 RGB=0이며 마스크 밖 유효 픽셀이 없음을 검사했다. 기존 `IceCubes.png`와 컵·액체 에셋은 수정하지 않았다.
- 검수 미리보기: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/3c877158-9d17-4156-bff5-e3db1a17f799/scratchpad/ice_preview.png`, **1024×320 RGBA**, 각 컵 256×320. 왼쪽부터 **우유200g→샷30g / 딸기청40g→탄산200g / 샷30g→찬물200g / 액체 없이 얼음만**이다. 동일한 재료색·탄산 기포·기존 컵 원화를 사용했다.
- 합성: 재료마다 `h=max(6,324×g/330)`으로 y=604부터 순서대로 쌓고 y=280에서 제한한다. 마지막 액면은 높이64px 타원이며 윗선 중심에 놓는다. 액체·액면 → 전체 `IceFill` → `CupPlasticFront` 순서이며 내용물은 기존 마스크로 클리핑한다. 라테·아메리카노 액면 y≈378.18, 딸기에이드 y≈368.36이고, 얼음 위치는 액체 양과 무관하게 고정이다. 바닥부터 위까지 얼음 형태와 뒤쪽 음료색이 보이는지 네 컵 모두 눈으로 확인했다.
- Unity **6000.6.4f1**에서 PNG를 임포트해 `.meta`를 자동 생성한다. Sprite / Single / Full Rect / Bilinear / Mip Maps 끔 / Alpha Is Transparency 켬 / 압축 없음으로 확인하고, 투명 여백을 포함한 전체 캔버스를 256×320으로 정렬한다. 이번 작업은 새 에셋과 검수 합성만 추가했으며 런타임의 얼음 참조 교체는 포함하지 않는다.
- `.meta`, `.unity`, `.prefab`, `.asset`은 직접 생성·수정하지 않았다. 새 ADR 없음(요청된 에셋 규격 이행).

- 2026-11-18: 얼음은 `CupContentsView` 가 컵 안을 채워 그리게 되어(ADR-0037) 쓰지 않게 된 `IceCubes.png` 를 삭제했다. 위 프롬프트 기록은 참고용으로 남긴다.

## 스팀 피처 진입 버튼 (2026-11-19)

- 도구: OpenAI 내장 image_gen.imagegen 원화 생성, Python 3 / Pillow 크롭·리샘플링·Jua 조판·검수 합성.
- 스타일 참조: 이 폴더의 EmptyButton.png, Panel.png, Shelf.png, BigCupCeramic.png. 부드러운 기기 작업 창 일러스트, 왼쪽 위 조명, 세이지·허니 골드. 기존 PNG는 덮어쓰지 않았다.
- **SteamButton.png (660×180 RGBA)**: “Use case stylized-concept. Production Unity UI button. Match reference EXACT style of sage green enamel pill, fine honey gold double rim, softly beveled edges and upper-left highlight, realistic warm cafe illustration. One wide empty capsule button on transparent background, aspect 3:1, no text no icons. Full edge visible.” 생성 원화 알파 경계를 크롭하고 4px 여백으로 정규화한 빈 버튼이다.
- **LabelSteam.png (268×88 RGBA)**: 고정 문구 “스팀 피처”, Jua-Regular.ttf 72px, 다크 커피 글자·크림 외곽선·8px 여백. 한국어 철자 육안 확인.
- 1600×900 창 좌표(왼쪽 위 원점): 버튼/일반 클릭 영역 **(1080,80,330,90)**, 라벨 **(1178,103,134,44)**. 라벨은 원본의 절반, 버튼 중심 (1245,125)에 정렬한다.
- 선반 (100,200,920,660), 컵·저울 영역 (1110,184,360,486), 하단 비우기·완성 버튼과 겹치지 않는다. 버튼 아래끝170과 저울 영역 위끝184 사이14px, 닫기 버튼 왼쪽1500과 새 버튼 오른끝1410 사이90px.
- 스팀 창 에셋·파일별 프롬프트·HoldButton·TMP·온도계·HOT 컵 줄기 좌표: [SteamRoom 생성 기록](../SteamRoom/Generation.md).
- 검수 미리보기: C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/3c877158-9d17-4156-bff5-e3db1a17f799/scratchpad/steam/DrinkRoomPreview.png (1600×900). 기존 패널·선반·컵·저울·하단 버튼과 합성해 버튼 위치·철자·가독성을 확인했다.
- Unity 6000.6.4f1에서 임포트해 .meta를 자동 생성한다. Sprite / Single / Full Rect / Bilinear / Mip Maps 끔 / Alpha Is Transparency 켬 / 압축 없음. 코드·씬 연결은 이번 작업 범위에 포함하지 않았다. 새 ADR 없음.
