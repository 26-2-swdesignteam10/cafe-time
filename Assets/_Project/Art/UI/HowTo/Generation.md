Generated with OpenAI 기본 내장 image_gen. Style reference: Assets/_Project/Art/UI/Quit/Panel.png, Assets/_Project/Art/UI/Settings/Panel.png, Assets/_Project/Art/UI/Quit/Message.png, Assets/_Project/Art/UI/Quit/No.png, Assets/_Project/Art/UI/OpenPrep/StartButton.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- Panel.png: SFR-005. 표시 1400×900, 14:9. 호두나무 베벨 프레임, 금색 안쪽 선, 완전히 불투명한 크림 내부. 프레임 안 위쪽에 정확한 제목 "게임 설명", 양옆 작은 커피잔. 본문 영역은 글자·칸 구분 없이 비운다. 프레임 위 돌출 금지.
- HeadingStory.png: LABEL M. 정확한 문구 "스토리", 왼쪽 작은 커피콩 불릿. Quit/Message.png 계열 굵은 도트 글자, #380F00 채움과 #FCF0CA 크림 외곽선. 한 줄, 패널·배경 없음. 한글 채움 높이 72px, 사방 투명 여백 8px.
- HeadingRules.png: LABEL M. 정확한 문구 "진행 규칙", 왼쪽 같은 커피콩 불릿. #380F00 채움과 #FCF0CA 크림 외곽선. 한 줄, 패널·배경 없음. 한글 채움 높이 72px, 사방 투명 여백 8px.
- HeadingFlow.png: LABEL M. 정확한 문구 "하루의 흐름", 왼쪽 같은 커피콩 불릿. #380F00 채움과 #FCF0CA 크림 외곽선. 한 줄, 패널·배경 없음. 한글 채움 높이 72px, 사방 투명 여백 8px. 생성 응답 지연 후 개별 요청으로 재생성했다.
- FlowDiagram.png: 표시 1180×110, 118:11. 둥근 나무 표지판 네 개를 같은 높이로 한 줄에 놓고 작은 허니 화살표로 연결한다. 왼쪽부터 정확한 글자 "오픈 준비" → "주문 받기" → "음료 제조 및 매장 운영" → "하루 정산". 세 번째 표지판은 더 넓게 한다. 밝은 크림 중앙, 호두나무 베벨, 굵은 다크 커피 도트 글자와 크림 외곽선. 표지판 사이와 바깥은 투명. 다른 문구·장식 없음. 첫 결과의 양 끝 테두리가 캔버스 경계에 닿아, 전체를 캔버스 너비의 85%로 축소하고 중앙 배치하도록 내장 image_gen으로 수정했다. 최종 결과에는 단계별 작은 문·주문장·음료·동전 아이콘이 포함되어 있다.

내보내기: 알파 경계 크롭, 작은 분리 노이즈 제외, 최근접 보간 크기 조정. 그림 영역 알파 255, 외부 0. LABEL은 외곽선을 제외한 채움색의 유효 행 경계로 측정해 72px로 맞췄고, 사방 8px 투명 여백을 붙였다. 원본·출력의 한글을 눈으로 대조했다. 흐름도는 표시 크기의 1.5배인 1770×165로 저장하여 Max Size 2048 안에 둔다.

Unity: 에디터에서 임포트해 .meta 자동 생성. Sprite (2D and UI), Single, Point (no filter), Mip Map 끔, Alpha Is Transparency 켬, Max Size 2048. LABEL은 PNG 크기의 1/2로 표시한다. 긴 본문은 게임의 텍스트 영역에서 표시한다.
