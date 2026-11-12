Generated with OpenAI 기본 내장 image_gen. Style reference: Assets/_Project/Art/UI/Quit/Panel.png, Assets/_Project/Art/UI/Settings/Panel.png, Assets/_Project/Art/UI/Quit/Message.png, Assets/_Project/Art/UI/Quit/No.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- Panel.png: SFR-070. 표시 900×700, 9:7. 호두나무 베벨 프레임, 금색 안쪽 선, 완전히 불투명한 크림 내부. 프레임 안 위쪽에 정확한 제목 "랭킹", 양옆 작은 금색 트로피. 아래는 빈 크림 공간. 프레임 위로 장식 돌출 금지.
- ServerError.png: LABEL M. 정확한 문구 "랭킹 서버에 연결할 수 없습니다.". 왼쪽 작은 연결이 끊긴 케이블 픽셀 아이콘. Quit/Message.png 계열 굵은 도트 글자, #380F00 채움과 #FCF0CA 크림 외곽선. 한 줄, 패널·배경 없음. 한글 채움 높이 72px, 사방 투명 여백 8px.
- CloseButton.png: 표시 290×62, 요청한 52:11 비율 우선. Quit/No.png 계열 호두나무 베벨 버튼, 밝은 크림 중앙, 정확한 가운데 글자 "닫기", 왼쪽 작은 문 아이콘.

내보내기: 알파 경계 크롭, 작은 분리 노이즈 제외, 최근접 보간 크기 조정. 그림 영역 알파 255, 외부 0. LABEL은 외곽선을 제외한 채움색의 유효 행 경계로 측정해 72px로 맞췄고, 사방 8px 투명 여백을 붙였다. 원본·출력의 한글과 마침표를 눈으로 대조했다. 버튼의 실제 파일 크기는 572×121로, 요청한 52:11 비율과 1~2배 범위를 충족한다. 서버 연결 구현은 이 에셋 작업의 범위에 포함되지 않는다.

Unity: 에디터에서 임포트해 .meta 자동 생성. Sprite (2D and UI), Single, Point (no filter), Mip Map 끔, Alpha Is Transparency 켬, Max Size 2048. LABEL은 PNG 크기의 1/2로 표시한다.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| ServerError.png | 랭킹 서버에 연결할 수 없습니다. | 72px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판; 기존 장식 픽셀 보존 | 985×98 → **1115×98 (폭 확대)** |
| CloseButton.png | 닫기 | 48px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 572×121 |
| Panel.png | 랭킹 | 106px | #FCF0CA | #380F00 5px | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1800×1400 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
