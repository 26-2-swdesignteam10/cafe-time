# 아트 스타일 가이드

카페타임의 UI·스프라이트를 만들 때(사람이든 AI든) 따르는 기준이다. 결정 배경은 [ADR-0008](adr/0008-ai-generated-art-assets.md).
이 문서의 규칙이 바뀌면 같은 PR에서 이 문서를 고친다.

## 1. 콘셉트

- **아늑한 한국풍 도트 카페**: 따뜻한 나무 가구, 크림색 종이, 커피·잎사귀 장식
- 고해상도 이미지에 **계단식 픽셀 외곽선**을 쓴 "도트풍" 일러스트 (저해상도 원본 도트를 확대한 방식이 아님)
- 부드러운 그라데이션보다 2~3단계 명암 블록을 쓴다

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

- 게임 폰트: **Galmuri11** (`Art/Fonts/Galmuri11 SDF.asset`)
- 이미지에 글자를 넣는 경우: 고정된 제목·버튼 이름만 (예: "구매확정", "예"). 다크 커피 굵은 도트 글자 + 크림 외곽선.
- 이미지에 넣지 않는 글자: 금액, 수량, DAY, 닉네임, 재고 등 바뀌는 값 → TextMeshPro
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
