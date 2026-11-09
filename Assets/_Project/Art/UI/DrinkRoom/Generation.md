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
