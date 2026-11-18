# 아트 스타일 가이드

카페타임의 UI·스프라이트를 만들 때(사람이든 AI든) 따르는 기준이다. 결정 배경은 [ADR-0008](adr/0008-ai-generated-art-assets.md).
이 문서의 규칙이 바뀌면 같은 PR에서 이 문서를 고친다.

## 1. 콘셉트

- **아늑한 한국풍 도트 카페**: 따뜻한 나무 가구, 크림색 종이, 커피·잎사귀 장식
- 고해상도 이미지에 **계단식 픽셀 외곽선**을 쓴 "도트풍" 일러스트 (저해상도 원본 도트를 확대한 방식이 아님)
- 부드러운 그라데이션보다 2~3단계 명암 블록을 쓴다
- **카페 맵 그림은 예외**: 도트는 유지하되 깔끔하고 모던한 미니멀 스타일로 그린다 (웜 화이트 벽, 밝은 오크 바닥, 스테인리스·무광 검정 기기, 포인트 색은 세이지 그린 하나, 차콜색 얇은 외곽선). 2장 팔레트는 UI에만 적용한다. 시안 진행 중 ([ADR-0016](adr/0016-cafe-map-layout-and-modern-style.md))
- **영업 중 기기 작업 창은 예외**: 도트 규칙을 적용하지 않은 부드러운 일러스트로 그리고 Bilinear 필터로 표시한다. 기준은 샷 내리기 창(`Art/UI/ShotRoom`)이고, 대상 폴더는 `Art/UI/` 의 `ShotRoom`, `KnockRoom`, `CupShelf`, `IceMaker`, `Water`, `DrinkRoom`, `DrinkTools`, `DrinkResult`, `ShakeRoom`, `TrayReturn`, `Dishwash`, `Pos`, `Stations` 이다. 글자는 게임 폰트(4장)를 쓴다. 영업 HUD, 일시정지 창, 아이템·재료 아이콘은 도트 그대로다 (2026-11-09 사용자 요청, 2026-11-12 재료 작업 창 `DrinkTools` 포함). HUD 의 주문 내역·레시피 버튼(`Art/UI/OrderBook/OrdersButton`, `RecipeButton`, `Badge`)도 현대적인 부드러운 일러스트로 바꿨다 (2026-11-17 사용자 요청). 재료 작업 창은 재료마다 전용 도구 그림을 두며, 공용 그림과 같은 캔버스·기하로 그린다 (ADR-0038)
- **커피잔 아이콘도 예외**: `Art/Icons/Items` 의 `CupCeramic*`, `CupPlastic*`, `DirtyCeramic`, `DirtyPlastic` 은 샷 내리기 창과 같은 깔끔한 일러스트로 그리고 Bilinear로 표시한다. 게임 안 이름은 도자기 잔 → **HOT 컵**, 플라스틱 컵 → **ICE 컵** (2026-11-09 사용자 요청). 샷 컵 `ShotGlass`, `ShotGlassFull` 도 같은 예외로, 스테인리스 샷 피처 일러스트다 (2026-11-17 사용자 요청)

## 2. 팔레트

기존 에셋(`Art/UI/Quit`, `Art/UI/Settings`, `Art/Title`)에서 추출한 색이다. 새 에셋은 이 범위 안에서 만든다.

| 역할 | 색 | 쓰는 곳 |
|---|---|---|
| 크림 (바탕) | `#FAEAC2` | 패널 내부, 종이 |
| 밝은 크림 | `#FCF0CA` | 하이라이트, 보조 버튼 중앙 |
| 진한 크림 | `#F7DCA5` | 크림 영역 그림자 |
| 허니 앰버 | `#DF7015` | 주요 버튼(확인·예) 중앙 |
| 허니 골드 | `#C57D1F` | 금색 장식선, 테두리 안쪽 선 |
| 연한 골드 | `#F1C97D` | 버튼 하이라이트 |
| 호두나무 | `#90471B` | 나무 프레임 |
| 진한 호두나무 | `#5D2608` | 프레임 그림자 |
| 다크 커피 | `#380F00` | 외곽선, 이미지 속 글자 |
| UI 텍스트 (TMP) | `#5A3520` | TextMeshPro 기본 글자색 |
| 칠판 그린 | `#1F362B` | 칠판형 패널 (게임 시작 창) |
| 칠판 나무틀 | `#C08858` | 칠판 테두리 |

화면별 강조색(예: 정산 화면의 야경 남색)은 해당 화면 폴더의 `Generation.md` 에 적는다.

## 3. 형태 규칙

- **패널**: 직사각형 호두나무 프레임 + 불투명 크림 내부 + 금색 안쪽 선. 프레임 위로 튀어나오는 장식은 피한다 (레이아웃 계산이 어려워짐). 내부는 완전히 불투명하게 해서 뒤 화면이 비치지 않게 한다.
- **버튼**: 입체감 있는(베벨) 나무 버튼. 주요 동작은 허니 앰버, 보조·취소 동작은 밝은 크림 중앙. 왼쪽에 작은 아이콘, 오른쪽에 작은 잎 장식을 둘 수 있다.
- **아이콘**: 정사각형 캔버스, 다크 커피 외곽선, 투명 배경. 같은 묶음(예: 재료 아이콘)은 크기·시점·외곽선 두께를 맞춘다.
- **장식**: 커피콩, 커피 잎, 김이 나는 잔. 과하게 쓰지 않는다.

## 4. 글자

- 게임 폰트: **Jua** (`Art/Fonts/Jua-Regular.ttf`, TMP는 `Art/Fonts/Jua SDF.asset`). 이미지 속 글자도 Jua로 조판한다. 2026-11-12 Galmuri11에서 변경 ([ADR-0026](adr/0026-game-font-jua.md))
  - 금액은 `12,000원` 처럼 숫자 뒤에 "원"을 붙인다. Jua에 `₩` 글자가 없어 네모로 깨지기 때문 (2026-11-18 사용자 확정, 화면 전체 적용)
- **UI는 도형(Unity 기본 박스, Outline)으로 그리지 않는다.** 창, 버튼, 입력 칸, 탭 등 모든 컴포넌트는 디자인 에셋(PNG)으로 만든다.
- **고정 문구는 전부 이미지**로 넣는다: 창 제목, 버튼 이름, 탭 이름, 항목 이름("방문 손님", "총액"), 안내·경고 문구, 확인 팝업 메시지.
  - 버튼·창 제목은 버튼·패널 이미지에 직접 그린다.
  - 나머지는 글자만 있는 투명 PNG(라벨)로 만든다. 다크 커피 Jua 글자 + 크림 외곽선 (어두운 나무 위는 반대 색, 경고는 `#A3261B`).
  - 라벨 글자 높이: S 56px · M 72px · L 96px, 사방 여백 8px. 게임에서는 PNG의 **절반 크기**로 표시한다 (`BuilderUtil.Label`).
- 이미지에 넣지 않는 글자(TextMeshPro): 금액, 수량, DAY, 닉네임, 재고, 재료 이름처럼 바뀌는 값과 긴 본문(게임 설명 내용)
- 한국어 글자가 들어간 이미지는 철자를 반드시 눈으로 확인한다.

## 5. 파일 규격

| 항목 | 규칙 |
|---|---|
| 형식 | PNG, RGBA, 투명 배경 (배경 이미지 제외) |
| 위치 | `Assets/_Project/Art/<화면 또는 기능>/` (예: `Art/UI/OpenPrep/`, `Art/Icons/Ingredients/`) |
| 파일 이름 | PascalCase 영어, 역할이 드러나게 (`BuyButton.png`, `CartPanel.png`, `IngredientMilk.png`) |
| 여백 | 가장자리에 잘림이 없도록 여백 확보, 버튼·패널은 여백을 상하좌우 균일하게 |
| 크기 | 기준 해상도 1920x1080 에서 표시될 크기의 1~2배. 패널·버튼은 기존 에셋 크기를 참고, 아이콘은 256x256 |
| 9-slice | 크기가 변하는 패널은 테두리 두께가 일정하게 그려서 9-slice(Sprite Editor Border)를 쓸 수 있게 한다 |
| 맵 그림 | 타일·가구·기기·캐릭터는 **1칸 = 128px** (Pixels Per Unit 128). 캐릭터 프레임 128x192, 기기는 칸 수 x 128. 3/4 시점, 빛은 왼쪽 위. 기준점은 타일만 가운데, 나머지는 아래쪽 가운데 (`CafeSceneBuilder` 가 임포트 설정) |

## 6. Unity 임포트 설정

| 설정 | 값 |
|---|---|
| Texture Type | Sprite (2D and UI) |
| Sprite Mode | Single (아이콘 시트는 Multiple) |
| Filter Mode | **Point (no filter)** |
| Generate Mip Maps | 끔 |
| Alpha Is Transparency | 켬 |
| Max Size | 2048 |

## 7. 생성 기록 (Generation.md)

AI로 만든 에셋 폴더에는 `Generation.md` 를 둔다. 형식:

```markdown
Generated with <도구 이름>. Style reference: <참조한 기존 에셋 경로>.

Prompt set: <공통 스타일 프롬프트>

- <파일 이름>: <파일별 프롬프트와 수정 사항>
```

예시: [`Assets/_Project/Art/UI/Quit/Generation.md`](../Assets/_Project/Art/UI/Quit/Generation.md)

### 공통 스타일 프롬프트

새 에셋을 만들 때 아래 문장으로 시작한다.

> Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

## 8. 스타일 참조 에셋

| 종류 | 참조 파일 |
|---|---|
| 크림 패널 | `Art/UI/Quit/Panel.png`, `Art/UI/Settings/Panel.png` |
| 주요 / 보조 버튼 | `Art/UI/Quit/Yes.png` / `Art/UI/Quit/No.png` |
| 큰 메뉴 버튼 | `Art/UI/GameStart/NewGame.png`, `Art/Title/MenuStart.png` |
| 칠판 패널 | `Art/UI/GameStart/Board.png` |
| 배경 일러스트 | `Art/Title/CafeInterior.png` |
| 슬라이더·탭 | `Art/UI/Settings/` |


## 9. 음료 제조실 내용물 레이어 (2026-11-17)

기존 `Art/UI/DrinkRoom/BigCup*`의 깔끔한 회화풍을 따른다. 컵 위에 전체 캔버스를 겹치는 마스크·전면 유리·도자기 액면·폼은 **512×640 RGBA**이며 UI에서는 256×320이다. 원본 컵의 투명 여백까지 유지한다. 작은 반복 텍스처는 LiquidShade 128×8, LiquidSurface 400×64, BubblesTile 64×64, IceCubes 300×170이다. 투명 픽셀의 RGB는 0이며 새 PNG의 `.meta`는 Unity가 생성한다. 상세 좌표·생성 공정은 [DrinkRoom 생성 기록](../Assets/_Project/Art/UI/DrinkRoom/Generation.md)의 2026-11-17 절에 있다.

다음은 런타임 합성용 재료색 **제안**이다. 색 형식은 sRGB `#RRGGBBAA`이며 LiquidShade·LiquidSurface의 중성 RGB와 곱한다. 물·탄산은 매우 투명하고, 우유·스무디는 거의 불투명하다. 차색은 물 전체에 적용한다. 청·과일·블루 시럽에는 재료 구별에 필요한 분홍·주황·연두·청색을 사용한다. 게임 데이터의 용량·레시피·가격은 바꾸지 않는다.

| 재료 키 | 색상 (#RRGGBBAA) | 표현 |
|---|---|---|
| `shot` | `#60351FEF` | 에스프레소·따뜻한 갈색 크레마 계열 |
| `water_hot` | `#E7F4FC24` | 뜨거운 물·매우 옅은 투명 물색 |
| `water_cold` | `#DDF3FF30` | 찬물·매우 옅은 투명 청색 |
| `milk` | `#FFF4DEFA` | 우유·불투명 크림 |
| `soda` | `#D9F5FF38` | 탄산수·매우 옅은 투명 청백색 |
| `condensed_milk` | `#FFE2A8F5` | 연유·진한 크림 |
| `syrup_plain` | `#F5E6BB65` | 일반 시럽·투명한 옅은 금색 |
| `syrup_vanilla` | `#F5D69796` | 바닐라·옅은 꿀색 |
| `syrup_caramel` | `#C47A32DC` | 카라멜·앰버 |
| `syrup_hazelnut` | `#A56B3ACB` | 헤이즐넛·견과 갈색 |
| `syrup_chocolate` | `#4C2C25F5` | 초콜릿·짙은 갈색 |
| `syrup_blue` | `#268FCAD0` | 블루 시럽·선명한 청색 |
| `cheong_strawberry` | `#CB3F52EB` | 딸기청·붉은 분홍 |
| `cheong_grapefruit` | `#EF8655DC` | 자몽청·코랄 |
| `cheong_yuja` | `#EFC24BD9` | 유자청·황금색 |
| `tea_chamomile` | `#E5BD57B8` | 캐모마일·우린 물 전체에 적용 |
| `tea_earlgrey` | `#AD682ED4` | 얼그레이·우린 물 전체에 적용 |
| `smoothie_strawberry` | `#ED8D9AFF` | 딸기 스무디·불투명 분홍 |
| `smoothie_mango` | `#FFC656FF` | 망고 스무디·불투명 노랑 |
| `smoothie_hallabong` | `#FFA23FFF` | 한라봉 스무디·불투명 주황 |
| `smoothie_melon` | `#B8D59AFF` | 멜론 스무디·불투명 연두 |
| `smoothie_cookie` | `#D6C8B6FF` | 쿠키 스무디·불투명 베이지 |
