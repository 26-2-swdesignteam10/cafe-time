Generated with the built-in ImageGen tool. Style reference: ../../Title/CafeInterior.png, ../Quit/Panel.png, ../Quit/Yes.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- NightBackground.png: SFR-071 하루 정산 화면의 불투명 야경 배경. CafeInterior.png의 고해상도 도트풍 일러스트와 계단식 윤곽을 참조했다. 거리에서 본 작은 한국풍 베이커리 카페 외관, 허니색 창문 조명, 컵 기호만 있는 영업 종료 표지, 전구 줄, 별이 드문 밤하늘, 조용한 거리와 화분. 밝은 창문을 양옆에 두고 중앙 출입문은 어둡고 차분하게 구성했다. 글자·숫자·인물·UI 없음. 화면 강조색은 야경 남색 #17243A. 불투명 배경은 공통 투명 배경 지시의 예외. 원본 1672×941에서 중앙을 16:9로 미세 크롭하고 1920×1080으로 최근접 리샘플링했다.
- Receipt.png: Quit/Panel.png의 크림색과 커피잔 장식을 참조한 단일 정면 종이 영수증. #FCF0CA 계열 크림 바탕, #FAEAC2·#F7DCA5 가장자리 음영, 위아래 계단식 지그재그 절취선, 작은 상단 중앙 커피잔 로고, 높이 약 2/3의 점선 하나, 옅은 그림자. 나무 프레임·글자·숫자·본문 줄·별 없이 나머지 내부를 비웠다. 종이 밖은 실제 알파 투명 배경. 원본 1024×1536을 640×960으로 최근접 축소했다.
- NextDayButton.png: Quit/Yes.png와 같은 계열의 정면 베벨 나무 버튼. #90471B 프레임, #380F00 외곽선, #5D2608 아래쪽 베벨, #DF7015 계열 허니 앰버 중앙, #C57D1F 안쪽 선, #F1C97D 상단 하이라이트. 왼쪽 작은 해 아이콘과 오른쪽 잎 장식, 중앙 약 65%는 코드로 표시할 글자를 위해 비웠다. 글자·숫자 없음, 실제 알파 투명 배경. 원본 2079×756의 상하 투명 여백을 (0,141)–(2079,581)로 크롭한 뒤 52:11로 미세 중앙 크롭하고 1040×220으로 최근접 축소했다. 오브젝트는 잘리지 않도록 유지했다.
- Star.png: 48×48 표시용 단일 정면 오각별. #380F00 계열 다크 커피 계단식 외곽선, #C57D1F·#F1C97D 계열 허니 골드 내부와 작은 크림 하이라이트. 얼굴·글자·추가 반짝임·발광 없음. 정사각형 캔버스, 투명 배경과 여백. 생성 원본 1254×1254를 96×96으로 최근접 축소했다.
- StarEmpty.png: 생성한 Star.png를 편집 대상으로 다시 입력했다. 별의 위치·크기·오각 윤곽·외곽선·투명 배경을 유지하고 내부만 #F7DCA5·#FAEAC2 계열의 채도 낮은 크림과 절제된 골드 음영으로 바꾸도록 요청했다. 내부는 구멍 없이 크림으로 채운 미획득 상태이며, 글자·발광 없음. 원본 1254×1254를 Star.png와 동일하게 96×96으로 최근접 축소했다.

검증: 다섯 PNG의 RGBA 형식·지정 크기·종횡비·알파 채널을 확인했다. 배경은 알파 255이며, 나머지 네 파일은 알파 0의 투명 여백을 포함한다. 모든 이미지는 글자와 숫자를 포함하지 않으며, 영수증과 버튼의 동적 내용은 TextMeshPro로 표시한다. 이미지 생성은 모두 내장 ImageGen을 사용했으며, 후처리는 요청된 크롭과 최근접 크기 조정만 적용했다.

Unity 임포트: 에디터로 돌아가 이 폴더를 임포트하여 .meta를 자동 생성한다. Texture Type은 Sprite (2D and UI), Sprite Mode는 Single, Filter Mode는 Point (no filter), Generate Mip Maps는 끔, Alpha Is Transparency는 켬, Max Size는 2048로 확인한다. .meta·씬·프리팹·스크립트는 이 작업에서 작성하지 않았다.


## 2026-11-02 — 하루 정산 고정 문구 라벨

도구: 내장 ImageGen. 스타일 참조: Receipt.png(크림 영수증의 배치 맥락), NextDayButton.png(허니 앰버 버튼의 배치 맥락), ../Quit/Message.png(굵은 한국어 픽셀 글자). 기존 PNG는 수정하지 않았다.

공통 생성 프롬프트:

> Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.
> Create ONE text-only LABEL sprite, no panels, no button, no decorations, no beans. Reference Quit/Message.png for bold blocky Korean pixel letter design. Do NOT draw receipt or button. All glyphs identical cap height, slightly condensed, straight baseline, bold square pixel strokes, no smooth curves, no shadows, no gradients. Single line exact Korean text. Solid dark coffee #380F00 letter fill (WARNING: deep brick red #A3261B) and cream #FCF0CA outline 3px at target scale. Actual transparent background including counters. Crop text tightly with 8px transparent padding. No other text. Clean flat two-color artwork; no yellow or white fringe, no grunge, no speckles.

파일마다 아래 문구를 따옴표로 지정하고 음절을 하나씩 나열해 철자를 지시했다. 지정 높이는 외곽선을 제외한 한글 채움 영역의 높이이며, 게임의 1/2 표시 크기에서 S 28px, M 36px, L 48px가 된다. DAY·수량·금액·평판 숫자는 포함하지 않는다.

- LabelSettlement.png: "영업 정산". L, 한글 채움 높이 96px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-0a166c7c-f06c-4226-8d1f-cab5213ed84d.png.
- LabelVisitors.png: "방문 손님". S, 한글 채움 높이 56px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-eaf6e5cd-883c-4c74-8d09-70abeebace74.png.
- LabelDrinks.png: "판매한 음료". S, 한글 채움 높이 56px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-314a7959-6e37-4e2a-a6ff-02f4cfdbeb86.png.
- LabelRevenue.png: "오늘 수익". S, 한글 채움 높이 56px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-8b76eed4-60eb-4bd7-b250-706289e117b5.png.
- LabelTips.png: "받은 팁". S, 한글 채움 높이 56px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-e9015723-b5c1-44c3-9a80-5c8b91f77984.png.
- LabelRent.png: "임대료". S, 한글 채움 높이 56px. WARNING: 딥 브릭 레드 #A3261B 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-24bd855a-e0de-4fab-bad1-c07c3c77a070.png.
- LabelMoney.png: "총 보유금". M, 한글 채움 높이 72px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-82a409ca-be97-481d-b466-366c633dce64.png.
- LabelReputation.png: "평판". S, 한글 채움 높이 56px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 생성 원본: exec-25d478a9-d349-48ef-8a19-829d551de748.png.
- WarningRent.png: "임대료를 낼 돈이 부족합니다...". S, 한글 채움 높이 56px. WARNING: 딥 브릭 레드 #A3261B 채움 + 크림 #FCF0CA 외곽선. 한 줄, 마지막 ASCII 마침표 3개와 띄어쓰기를 그대로 유지. 생성 원본: exec-bb4b7817-a279-4408-bc1b-f852bcc6ec77.png.
- LabelEnding.png: "엔딩 보기". M, 한글 채움 높이 72px. 기본: 다크 커피 #380F00 채움 + 크림 #FCF0CA 외곽선. 허니 앰버 NextDayButton.png 위에서 읽히는 다크 커피 채움과 크림 외곽선. 생성 원본: exec-cc9a6828-b5ec-4201-8b56-eec3ddf58e3c.png.

최종 출력 보정: 이미지 생성으로 만든 글자 형태를 사용했으며 폰트로 다시 그리지 않았다. 원본의 채움 영역(G < 80, 알파 > 240)을 추출해 생성 시 생긴 가장자리 색 번짐을 제외했다. 각 한글 음절의 채움 경계를 기준으로 최근접 리샘플링하여 지정 높이에 맞췄다. 가로 비례는 해당 생성본의 전체 글자 높이 기준 배율을 유지했다. 음절 간 채움 간격 8px, 띄어쓰기에는 16px를 추가했다. 마침표는 원본 비례 크기와 기준선을 유지했다. 채움은 지정한 정확한 단색, 외곽선은 사각 픽셀 팽창 3px의 #FCF0CA, 외곽선 바깥 여백은 사방 정확히 8px로 출력했다. 글자 외부는 알파 0, 채움과 외곽선은 알파 255인 RGBA PNG이다.

검증: 최종 PNG 열 개를 열어 한국어 음절·띄어쓰기·마침표를 육안 확인했다. 모든 한글 음절의 채움 경계 높이, 알파 투명도, 두 색의 정확한 RGB 값, 사방 8px 여백을 픽셀 검사했다. S PNG 높이는 78px, M은 94px, L은 118px이다. 기존 PNG 다섯 개의 SHA-256이 작업 전과 같은지 확인했다.

Unity 임포트: Unity 6000.6.4f1 에디터로 돌아가 이 폴더를 임포트해 .meta를 자동 생성한다. Texture Type = Sprite (2D and UI), Sprite Mode = Single, Filter Mode = Point (no filter), Generate Mip Maps = 끔, Alpha Is Transparency = 켬, Max Size = 2048을 확인한다. 스프라이트는 PNG의 절반 크기로 배치한다. 이 작업은 아트 파일만 제공하며 씬·프리팹·스크립트·.meta를 편집하지 않는다. 새 ADR 또는 기존 ADR 변경 없음.



## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| LabelDrinks.png | 판매한 음료 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 365×78 |
| LabelEnding.png | 엔딩 보기 | 72px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 330×94 |
| LabelMoney.png | 총 보유금 | 72px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 340×94 |
| LabelRent.png | 임대료 | 56px | #A3261B | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 191×78 |
| LabelReputation.png | 평판 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 140×78 |
| LabelRevenue.png | 오늘 수익 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 273×78 |
| LabelSettlement.png | 영업 정산 | 96px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 418×118 → **443×118 (폭 확대)** |
| LabelTips.png | 받은 팁 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 215×78 |
| LabelVisitors.png | 방문 손님 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 276×78 |
| WarningRent.png | 임대료를 낼 돈이 부족합니다... | 56px | #A3261B | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 807×78 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
