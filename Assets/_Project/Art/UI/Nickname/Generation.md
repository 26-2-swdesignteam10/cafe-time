Generated with OpenAI 기본 내장 image_gen. Style reference: Assets/_Project/Art/UI/Quit/Panel.png, Assets/_Project/Art/UI/Quit/Yes.png, Assets/_Project/Art/UI/Quit/No.png, Assets/_Project/Art/UI/Quit/Message.png, Assets/_Project/Art/UI/OpenPrep/StartButton.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- Panel.png: SFR-003. 표시 820×580, 41:29 호두나무 베벨 프레임, 금색 안쪽 선, 완전히 불투명한 크림 내부. 프레임 안 위쪽에 정확한 제목 "닉네임 입력", 양옆 작은 커피콩. 아래는 빈 공간. 프레임 위로 돌출 금지.
- InputField.png: 표시 600×88, 75:11. #F7DCA5 크림색 오목한 입력 슬롯, 얇은 호두나무 테두리, 위쪽 안 그림자. 글자 없이 내부를 비운다.
- Placeholder.png: LABEL S. 정확한 문구 "닉네임을 입력하세요". 굵은 도트 글자, #9E6840 채움, 외곽선 없음, 배경·장식 없음. 한 줄, 한글 채움 높이 56px, 사방 투명 여백 8px.
- Warning.png: LABEL WARNING S. 정확한 문구 "주의! 기존에 플레이했던 데이터가 사라집니다". #A3261B 채움과 #FCF0CA 크림 외곽선. 한 줄, 배경·장식 없음, 한글 채움 높이 56px, 사방 투명 여백 8px.
- Info.png: LABEL S. 정확한 문구 "입력한 닉네임은 랭킹에 표시됩니다". #380F00 채움과 #FCF0CA 크림 외곽선. 한 줄, 배경·장식 없음, 한글 채움 높이 56px, 사방 투명 여백 8px.
- StartButton.png: 표시 290×62, 요청한 52:11 비율 우선. 호두나무 베벨, 허니 앰버 #DF7015 중앙, 정확한 글자 "시작", 왼쪽 재생 삼각형. 첫 결과의 크림색 중앙을 허니 앰버로 바꾸도록 내장 image_gen으로 재생성했다. 수정 프롬프트: Edit ONLY the center material/color of this StartButton reference. The currently pale cream face MUST become saturated honey amber #DF7015, with lighter amber #F1C97D bevel highlights. Keep exact Korean "시작", play triangle at left, walnut beveled outline, layout and shape.
- CancelButton.png: 표시 290×62, 52:11. Quit/No.png 계열 호두나무 베벨, 밝은 크림 중앙, 정확한 글자 "취소", 왼쪽 작은 뒤로 화살표.

내보내기: 생성 원본을 알파 경계로 크롭하고 작은 분리 노이즈를 제외했다. 최근접 보간으로 크기·비율을 맞췄으며, 패널 내부를 포함한 그림 영역의 알파는 255, 바깥은 0으로 정리했다. LABEL 높이는 외곽선을 제외한 채움색의 유효 행 경계로 측정하여 56px로 맞추고, 그림 경계 바깥에 사방 8px 투명 여백을 붙였다. 원본 한글과 출력 한글을 눈으로 대조했다. 버튼의 실제 파일 크기는 572×121로, 요청한 52:11 비율과 1~2배 범위를 충족한다.

Unity: 에디터에서 임포트해 .meta를 자동 생성한다. Sprite (2D and UI), Single, Point (no filter), Mip Map 끔, Alpha Is Transparency 켬, Max Size 2048. LABEL은 PNG 크기의 1/2로 표시한다.
