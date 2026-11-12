Generated with OpenAI 기본 내장 image_gen + Python Pillow 후처리. Style reference: Assets/_Project/Art/Title/TitleSignText.png, Assets/_Project/Art/Title/MenuStart.png, Assets/_Project/Art/UI/GameStart/NewGame.png, Assets/_Project/Art/UI/GameStart/Continue.png, Assets/_Project/Art/Title/CafeInterior.png, Assets/_Project/Art/UI/Quit/Message.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

SFR-003 닉네임 입력 창 재디자인. 아래는 파일별 생성 프롬프트의 요구사항 기록이다. 개별 이미지 생성 호출마다 위 공통 프롬프트와 해당 파일의 요구사항을 전달하고 기존 이미지를 스타일 참조로 첨부했다. 기존 공용 스타일·규격의 변경은 없다.

- Panel.png: 1640:1160. TitleSignText의 호두나무 베벨 프레임, 허니 골드 안쪽 선, 좌우 상단 아이비, 상단 중앙 작은 커피잔. 크림 리본 안에 정확한 제목 "닉네임 입력", 양옆 커피콩. 왼쪽 아래 작은 화분, 오른쪽 아래 하트 라테 잔. 장식은 프레임·제목·모서리에 한정. x=120~1520, y=370~1040 내부는 비어 있는 불투명 #FCF0CA. 바깥은 투명. 로프·추가 글자 금지.
- InputField.png: 1200:176. Continue 버튼 계열의 얇은 호두나무 테두리와 크림색 오목한 메모 카드. 왼쪽 끝 작은 연필, 오른쪽 끝 작은 커피콩. 중앙은 평평한 크림색 빈칸. 글자 없음.
- Placeholder.png: LABEL S. 정확한 "닉네임을 입력하세요". Quit/Message 계열의 굵은 도트 글자. #9E6840 채움, 외곽선·그림자·장식 없음. 채움 높이 56px, 투명 여백 8px, 한 줄.
- Warning.png: LABEL WARNING S. 정확한 "주의! 기존에 플레이했던 데이터가 사라집니다". #A3261B 채움, #FCF0CA 3px 외곽선. 채움 높이 56px, 투명 여백 8px, 한 줄. 장식 아이콘 없음.
- Info.png: LABEL S. 정확한 "입력한 닉네임은 랭킹에 표시됩니다". #380F00 채움, #FCF0CA 3px 외곽선. 채움 높이 56px, 투명 여백 8px, 한 줄. 장식 아이콘 없음.
- StartButton.png: 580:124. MenuStart·NewGame 버튼 계열. #DF7015 허니 앰버 중앙, #F1C97D 밝은 베벨, 호두나무·다크 커피 테두리. 왼쪽 김 나는 커피잔, 정확한 "시작"을 크림 채움·다크 커피 외곽선으로 표시, 오른쪽 작은 잎. 균일한 투명 여백.
- CancelButton.png: 580:124. Continue 계열의 크림 중앙, 호두나무 베벨과 다크 커피 테두리. 왼쪽 뒤로 화살표, 정확한 "취소"를 다크 커피 굵은 글자로 표시. 오른쪽 작은 잎, 균일한 투명 여백.

후처리: 알파 경계 크롭과 낮은 알파 번짐 제거 후 NEAREST로 패널 1640×1160, 입력칸 1200×176, 버튼 각각 580×124로 정규화했다. 하단 프레임 영역은 세로로 압축하여 소품을 예약 영역 아래에 배치했다. 크림 바탕을 합성하여 내부를 불투명하게 만들고, 예약 영역 x=120~1519, y=370~1039는 #FCF0CA 단색·알파 255로 고정했다. 입력칸 중앙도 평평한 불투명 크림으로 정리했다.

라벨은 생성된 글자의 채움 마스크를 추출하고 NEAREST로 비율을 유지하면서 채움 높이를 56px로 통일했다. 지정 색상으로 채우고 Warning·Info에 3px 크림 외곽선을 다시 입혔다. Placeholder는 외곽선이 없다. 유효 그림 바깥에 사방 8px 투명 여백을 붙였다. 제목·버튼·라벨 문구를 글자별로 눈으로 대조했다.

미리보기: 지정된 scratch 경로의 nickname_preview.png에 실제 표시 크기로 합성했다. 패널 820×580, 경고 중심 (410,205), 안내 중심 (410,255), 입력칸 (110,295)에 600×88, 시작 (110,435)·취소 (420,435)에 290×62, 라벨은 PNG의 절반 크기다. 겹침·여백·한글 가독성을 확인했다. 미리보기는 Assets에 저장하지 않았다.

Unity 확인: 기존 .meta·씬·프리팹·스크립트는 변경하지 않았다. Unity 6000.6.4f1에서 자동 재임포트 후 타이틀의 닉네임 입력 창을 열고 입력 전·후와 버튼 표시를 확인한다.

## 2026-11-11 버튼 세로 늘리기

- 대상: `StartButton.png`, `CancelButton.png`를 같은 이름으로 덮어썼다. 각각 RGBA 580×124 → 580×156이며 화면 표시 기준은 290×78이다.
- 도구: OpenAI 내장 image_gen으로 원본을 참조한 편집 시안을 생성하고, Python Pillow로 최종 후처리했다. 프롬프트는 기존 색·베벨·계단식 테두리·왼쪽 아이콘·정확한 “시작”/“취소”·오른쪽 잎을 유지하면서 몸통 높이만 32px 늘리고 글자와 장식은 비율을 유지해 세로 가운데 정렬하도록 요청했다.
- 후처리: 생성 시안에는 원본과 픽셀 차이가 있어 최종본은 기존 PNG 픽셀로 재조립했다. 글자·아이콘·잎·작은 장식은 주변 배경과의 색 차이로 마스크를 추출하고 외곽선 내부를 채워 분리했다. 분리한 자리의 배경은 같은 행의 빈 배경 픽셀로 복원했다. 몸통 중앙의 가로 한 줄을 NEAREST로 32px 높이로 복제해 삽입하고, 원본 장식 픽셀을 크기 변경 없이 아래로 16px 옮겨 합성했다. 위아래 베벨과 모서리, 좌우 테두리 두께는 유지했다.
- 검증: 두 버튼 모두 580×156, 유효 영역 `(6, 6)–(574, 150)`, 사방 투명 여백 6px를 확인했다. 분리한 글자·장식 픽셀이 원본과 같은지 좌표를 대조했다. 원본 크기와 NEAREST 절반 크기 미리보기에서 “시작”, “취소” 철자와 중앙 정렬을 눈으로 확인했다.
- 미리보기: Assets 밖 scratchpad의 `nickname-taller/ButtonsFullPreview.png`, `nickname-taller/ButtonsHalfPreview.png`에 저장했다. `.meta`, `.cs`, `.unity`, `.prefab`, `.asset`은 수정하거나 생성하지 않았다.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| Info.png | 입력한 닉네임은 랭킹에 표시됩니다 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 796×78 → **855×78 (폭 확대)** |
| Placeholder.png | 닉네임을 입력하세요 | 56px | #9E6840 | 없음 | Pillow Jua 4배 안티앨리어싱 조판 | 527×72 |
| Warning.png | 주의! 기존에 플레이했던 데이터가 사라집니다 | 56px | #A3261B | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 1040×78 → **1139×78 (폭 확대)** |
| CancelButton.png | 취소 | 56px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 580×156 |
| Panel.png | 닉네임 입력 | 82px | #380F00 | 없음 | 주변 재질 표본으로 글자 영역 국소 복원 + Pillow Jua 조판 | 1640×1160 |
| StartButton.png | 시작 | 56px | #FCF0CA | #380F00 3px | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 580×156 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
