Generated with OpenAI built-in image_gen. Style reference: Assets/_Project/Art/UI/Stations/PickupPanel.png; Assets/_Project/Art/UI/Pos/Panel.png, ConfirmButton.png, DeleteButton.png; Assets/_Project/Art/UI/OpenPrep/ConfirmUnbought.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

대상: SFR-023 일시정지 메뉴. 기존 스타일 및 사용자 지정 크기를 적용했으며 새 설계 결정은 없다.

- 패널 1400×1280px, 버튼 840×192px, 확인 메시지 2112×745px. 패널·버튼은 균일한 8px 외부 투명 여백. 패널 내부는 완전 불투명 크림, 제목 아래 내용은 비어 있다.
- 제작 후처리: 최근접 보간으로 크기·여백을 맞췄다. 패널·설정 버튼의 투명 생성본은 내부까지 지워져 제외하고, 단색 시안 배경으로 재생성한 뒤 외부 배경만 알파 0으로 처리했다. 시안은 최종 화면에 표시되지 않는다.
- ConfirmMainMenu.png: 다크 커피 #380F00 + 크림 #FCF0CA. 참조의 첫 줄 y=227..327(101px), 둘째 줄 y=416..517(102px)에 맞춰 각 줄을 가운데 정렬했다. 문구는 "오늘 진행한 영업은 저장되지 않습니다." / "메인 메뉴로 돌아가시겠습니까?". 최종 한글·공백·문장부호를 눈으로 검수했다.

- Panel.png:
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE pause menu panel target1400x1280. Match PickupPanel.png, near square wooden walnut frame, gold inner line. Physical opaque CREAM PAPER completely fills whole inside. Exact title "일시정지" dark coffee painted at top center with TWO small coffee beans. Rest is uninterrupted cream paper #FAEAC2, EMPTY of content. No hole or transparent center, no vignette. Front view crisp stepped edges. Use a flat pure cyan #00FFFF backdrop OUTSIDE wooden object ONLY for later production alpha key, even padding. All interior absolutely opaque cream. No cyan inside panel.
- ContinueButton.png:
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE honey amber PRIMARY button, PNG840x192 display420x96. Exact centered bold pixel Korean "계속하기" (계 / 속 / 하 / 기). Same family as Pos/ConfirmButton.png, walnut bevel edge, honey amber #DF7015 face, #F1C97D highlight top-left, dark coffee #380F00 text. Wide short proportions840:192, even8px transparent padding. No icon, no ornament, no extra text.
- SettingsButton.png:
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE cream SECONDARY button target840x192 wide4.375:1. Match DeleteButton.png bevels and palette exactly. Exact dark coffee centered Korean "설정". Cream #FCF0CA face completely solid clean opaque, no spots or holes. Walnut bevels, crisp stepped pixels, top-left highlight. Flat pure cyan #00FFFF backdrop ONLY outside object for later production alpha key. Even8px margin, no cyan inside, no icons or ornaments.
- MainMenuButton.png:
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. ONE cream SECONDARY button, PNG840x192 display420x96. Exact centered bold pixel Korean "메인 메뉴로" (메 / 인 SPACE 메 / 뉴 / 로). Same family as Pos/DeleteButton.png but match proportions ContinueButton, walnut bevel edge, cream #FCF0CA face, dark coffee #380F00 text. Wide short proportions840:192, even8px transparent padding. No icon, no ornament, no extra text.
- ConfirmMainMenu.png:
  - Prompt: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Match attached ConfirmUnbought.png canvas2112x745 and same glyph size and positions EXACTLY. Replace its two centered lines with EXACT Korean: line1 "오늘 진행한 영업은 저장되지 않습니다." line2 "메인 메뉴로 돌아가시겠습니까?". First line y227..328, second y416..518. Flat dark coffee #380F00 fill, cream #FCF0CA thin outline, bold squared pixel Hangul. Absolutely clean 2-color text, NO shadow, distress, fringe, speckles, texture or ornaments. Alpha-transparent background, no panel. Check every syllable and question mark.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| ConfirmMainMenu.png | 오늘 진행한 영업은 저장되지 않습니다. / 메인 메뉴로 돌아가시겠습니까? | 84px | #380F00 | #FCF0CA 3px | Pillow Jua 조판 (투명 캔버스·기존 장식 보존) | 2112×745 |
| ContinueButton.png | 계속하기 | 72px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 840×192 |
| MainMenuButton.png | 메인 메뉴로 | 72px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 840×192 |
| SettingsButton.png | 설정 | 72px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 840×192 |
| Panel.png | 일시정지 | 96px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1400×1280 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
