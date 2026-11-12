Generated with the built-in ImageGen tool. Style reference: ../GameStart/NewGame.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- Panel: full rectangular carved walnut frame with a continuous straight top border, opaque cream parchment interior, leafy coffee sprigs, gold inner rule, steaming porcelain coffee cup crest inside the upper area; empty message and button areas; no protruding crest, no text or buttons. Final edit keeps the entire interior opaque so the menu behind the dialog cannot show through.
- Message: exact Korean text “게임을 종료할까요?” on one centered line; bold dark coffee brown pixel lettering, cream outline, symmetrical coffee bean ornaments; no panel or button.
- Yes: beveled wooden button, honey amber center, exact centered Korean text “예”; exit door icon at left and small leaf at right.
- No: beveled wooden button, pale cream center, exact centered Korean text “아니오”; back arrow at left and small leaf at right.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| Message.png | 게임을 종료할까요? | 180px | #380F00 | #FCF0CA 12px | Pillow Jua 조판 (투명 캔버스·기존 장식 보존) | 2112×745 |
| No.png | 아니요 | 170px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 2060×763 |
| Yes.png | 예 | 170px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 2060×763 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
