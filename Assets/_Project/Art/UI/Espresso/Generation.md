Generated with OpenAI 내장 image_gen. Style reference: `../Pos/Panel.png`, `../Stations/PickupPanel.png`, `../Stations/PickupSlot.png`, `../Pos/PayButton.png`, `../Pos/ConfirmButton.png`, `../Pos/DeleteButton.png`, `../CafeHud/ToastNoCustomer.png`, `../Quit/Message.png`, `../../Icons/Items/HotCoffee.png`, `../../Sprites/Cafe/StationEspresso.png`.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

SFR-029~036 에스프레소 머신 화면용 독립 스프라이트 21종. 기기는 정면 시점, 왼쪽 위 광원, 짙은 커피색 외곽선, 따뜻한 스테인리스와 호두나무를 사용한다. 기존 아트 스타일을 따르며 새 스타일 결정은 없다. 내장 생성 도구로 파일별 생성 및 상태 편집을 수행했다. 생성 도구가 반환한 크기는 요청 규격과 달라 Windows System.Drawing으로 최근접 좌표 변환, 알파 정리, 여백, 라벨 높이와 상태 정렬을 정규화했다. Python/Pillow는 최종 PNG의 읽기 전용 측정에만 사용했다.

- Panel.png: 3200×1800. 픽셀 호두나무 베벨 프레임, 금색 안쪽 선, 불투명 크림 내부. 상단 중앙에 정확히 "에스프레소 머신", 양옆 작은 커피콩 2개. 나머지 내부는 비워 둔다. 생성 중 내부까지 투명해지는 문제는 불투명 보드를 재생성한 뒤 내장 도구로 프레임 바깥 마젠타만 제거하여 수정했다. 사방 16px 투명 여백.
- Machine.png: 1520×840. 스테인리스와 호두나무의 2그룹 상업용 머신 정면, 드립 트레이 포함. 포터필터·잔·글자 없음. 그룹 중심 PNG x=380/1140, 하단 y=330, 트레이 상단 y=700을 기준으로 좌표 정규화. 뒤판 알파 구멍은 내장 도구로 불투명 금속 뒤판으로 재생성한 뒤 외부 배경만 제거했다.
- Grinder.png: 520×840. 원두가 보이는 상부 호퍼, 분쇄 출구와 빈 포터필터 지지 포크. 포크 기준 PNG y=600으로 정규화. 포터필터·글자 없음.
- TampStation.png: 520×320. 낮은 호두나무 받침, 검은 고무 매트와 빈 바스켓 지지부. 포터필터·탬퍼·글자 없음.
- Tamper.png: 220×300. 위쪽 둥근 나무 손잡이, 짧은 목, 아래쪽 평평한 스테인리스 베이스의 핸드 탬퍼.
- KnockBox.png: 440×360. 짙은 고무 통과 상부 가로 봉. 비어 있는 개구부가 보이는 정면.
- PortafilterEmpty.png: 440×180. 왼쪽 위로 열린 깨끗한 은색 바스켓, 오른쪽 검은 손잡이. 상태별 공통 원본.
- PortafilterGround.png: 440×180. 원본의 바스켓 내부만 느슨하게 쌓인 신선한 원두가루로 편집. 공통 좌표로 정규화 후 내용물 영역 외에는 빈 상태 원본을 유지했다.
- PortafilterTamped.png: 440×180. 동일 바스켓 림 높이의 평평하게 압축된 커피 퍽. 내용물 영역 외 공통 원본 유지.
- PortafilterUsed.png: 440×180. 동일 바스켓 내부의 짙고 젖은 사용 후 퍽과 아래 작은 방울. 내용물·방울 영역 외 공통 원본 유지.
- ShotGlassEmpty.png: 180×200. 왼쪽 주둥이와 오른쪽 작은 손잡이가 있는 빈 유리 샷 피처, 크림 반사광. 공통 원본.
- ShotGlassFull.png: 180×200. 동일 피처를 약 65% 채운 짙은 에스프레소와 황금색 크레마. 빈 피처와 공통 자르기 좌표 사용.
- CupSpot.png: 240×80. 잔 위치를 표시하는 얕고 작은 스테인리스 드립 트레이 그릴. 잔·글자 없음.
- GrindButton.png: 340×132. Pos/ConfirmButton 계열 허니 앰버 베벨 버튼. 정확한 굵은 픽셀 글자 "분쇄".
- ExtractButton.png: 340×132. 분쇄 버튼을 참조하여 글자만 정확히 "추출"로 바꾼 허니 앰버 베벨 버튼.
- LabelGrinder.png: S 라벨, 정확히 "그라인더". 최종 231×78.
- LabelTamping.png: S 라벨, 정확히 "탬핑". 최종 127×78.
- LabelKnockBox.png: S 라벨, 정확히 "넉박스". 최종 195×78.
- LabelGroup1.png: S 라벨, 정확히 "1번". 최종 116×78.
- LabelGroup2.png: S 라벨, 정확히 "2번". 최종 123×78.
- Hint.png: S 라벨, 한 줄로 정확히 "포터필터를 끌어서 옮기고, 탬퍼는 아래로 끌어 누르세요". 최종 1080×78. 쉼표와 띄어쓰기 유지.

라벨 공통: 패널·버튼·장식 없이 글자만 생성. 다크 커피 #380F00 글자 채움의 측정 높이 56px, 크림 #FCF0CA 외곽선 3px, 외곽선 포함 높이 62px, 그 바깥 사방 투명 여백 8px. 글자별 철자와 최종 PNG를 육안 확인했다. 모든 PNG는 RGBA이며 물체 바깥은 투명하다. 포터필터 상태 4종은 공통 손잡이 픽셀을 유지하며, 사용 후 방울만 아래로 추가된다.

Unity 임포트: Texture Type Sprite (2D and UI), Single, Point (no filter), Mip Maps 끔, Alpha Is Transparency 켬. 사용자가 Unity 6000.6.4f1로 돌아가 임포트하여 .meta를 생성해야 한다. Panel.png는 폭 3200px를 유지하도록 해당 파일의 Max Size를 4096으로 설정한다. 나머지는 기존 2048 설정으로 충분하다. 게임 표시 크기는 PNG의 절반이며 라벨 글자 채움은 화면에서 28px가 된다.
