# 알바생 스프라이트·초상화 생성 기록

제작일: 2026-11-26

## 도구와 참조

- 생성: OpenAI 내장 `image_gen`, 투명 배경 요청. API·CLI는 사용하지 않았다.
- 후처리·검수: Python 3 + Pillow. 사용자가 요청한 시트 분할·최근접 정규화·알파 정리·좌우 반전만 적용했다.
- 규격: `docs/ART_STYLE.md`, `Sprites/Customers/Generation.md`, `Sprites/Player/Generation.md`의 기존 제작 방식.
- 이동 스타일: `../Customers/Customer1Down.png`, `Customer1Down1.png`, `Customer1Left.png`, `../Player/PlayerDown0.png`를 직접 확인했다. 생성 입력에는 Down·Left·PlayerDown0를 사용했다.
- 초상화 스타일·구도: `../Customers/Customer1Portrait.png`를 생성 입력으로 사용했다. 투명 배경, 큰 머리의 치비 도트, 어깨와 가슴 위가 보이는 구도다.
- 인물 외형: 사용자 제공 `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/8a676556-5541-47b6-ba2f-aa4ffc0de2df/scratchpad/mockup/StaffMockup.png`. UI는 복제하지 않았다.
- 초상화에는 각 직원의 생성 시트도 입력하여 머리·옷·앞치마를 맞췄다.

## 인물

- Minji(민지): 긴 갈색 웨이브 하이 포니테일, 작은 노란 머리끈, 노란 후드. 가방이나 주인공의 큰 리본은 없다.
- Doyun(도윤): 짧은 갈색 곱슬머리, 둥근 갈색 안경, 올리브 니트.
- Youngja(영자): 회색 웨이브 단발의 중년 여성, 짙은 보라 카디건과 크림 블라우스. 번 헤어는 없다.
- 세 명 모두 크림 주머니가 있는 갈색 카페 직원 앞치마, 짙은 바지, 갈색 신발을 착용한다. 뒷면에는 앞치마 끈과 매듭이 보인다. 이번 묶음에서 사용하는 의상색이며 기존 스타일 규칙을 변경하지 않는다.

## 생성 프롬프트

아래는 실제 호출에 사용한 전체 프롬프트다. 파일별 프롬프트는 해당 인물의 시트 및 그 시트에서 분할한 모든 이동 프레임에 적용된다.

### StaffMinjiSourceSheet.png → 이동 프레임

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Production animation sheet. Input 1 Customer1Down: master for pixel clusters, warm dark coffee outlines ~3-4px at final 128x192, head/body proportions. Input 2 Customer1Left: profile style. Input 3 PlayerDown0: scale and cafe outfit only, DO NOT COPY heroine hair or ribbon. Input 4 StaffMockup: character identity only, no UI. Create ONE employee, wearing WALNUT BROWN bib cafe apron with cream pocket over specified clothes, dark trousers, brown shoes. Match existing customer art, chibi head about 45% total height, crisp stepped outlines, only 2-3 flat shades per material, upper-left lighting. True transparent background, no shadow, text, labels, grid lines, props or checkerboard. EXACT 3 columns x4 rows equal grid, portrait canvas width:height=1:2. Rows DOWN front, UP back, LEFT profile screen-left, RIGHT profile screen-right. Columns idle standing feet together, walk1 left leg forward opposite arm forward, walk2 RIGHT leg forward opposite arm; visibly opposite strides. Identical head and torso position and scale within each row, only arms and legs move. Fully visible entire bodies with padding in all 12 equal cells. Intended each cell128x192, crown y8 shoe bottom y187, centered x64. Keep apron visible in front and straps tied behind in back.
Subject: Minji, diligent young Korean woman with CHESTNUT long wavy HIGH PONYTAIL, small yellow hair tie, mustard YELLOW HOODIE under brown apron. NO backpack, NO heroine ribbon, no loose unbound hair.
```

### StaffMinjiPortrait.png

```text
undefined
```

### StaffDoyunSourceSheet.png → 이동 프레임

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Production animation sheet. Input 1 Customer1Down: master for pixel clusters, warm dark coffee outlines ~3-4px at final 128x192, head/body proportions. Input 2 Customer1Left: profile style. Input 3 PlayerDown0: scale and cafe outfit only, DO NOT COPY heroine hair or ribbon. Input 4 StaffMockup: character identity only, no UI. Create ONE employee, wearing WALNUT BROWN bib cafe apron with cream pocket over specified clothes, dark trousers, brown shoes. Match existing customer art, chibi head about 45% total height, crisp stepped outlines, only 2-3 flat shades per material, upper-left lighting. True transparent background, no shadow, text, labels, grid lines, props or checkerboard. EXACT 3 columns x4 rows equal grid, portrait canvas width:height=1:2. Rows DOWN front, UP back, LEFT profile screen-left, RIGHT profile screen-right. Columns idle standing feet together, walk1 left leg forward opposite arm forward, walk2 RIGHT leg forward opposite arm; visibly opposite strides. Identical head and torso position and scale within each row, only arms and legs move. Fully visible entire bodies with padding in all 12 equal cells. Intended each cell128x192, crown y8 shoe bottom y187, centered x64. Keep apron visible in front and straps tied behind in back.
Subject: Doyun, quick young Korean man, warm chestnut short CURLY hair, large ROUND brown glasses, OLIVE GREEN knit sweater under brown apron. Friendly attentive face.
```

### StaffDoyunPortrait.png

```text
undefined
```

### StaffYoungjaSourceSheet.png → 이동 프레임

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Production animation sheet. Input 1 Customer1Down: master for pixel clusters, warm dark coffee outlines ~3-4px at final 128x192, head/body proportions. Input 2 Customer1Left: profile style. Input 3 PlayerDown0: scale and cafe outfit only, DO NOT COPY heroine hair or ribbon. Input 4 StaffMockup: character identity only, no UI. Create ONE employee, wearing WALNUT BROWN bib cafe apron with cream pocket over specified clothes, dark trousers, brown shoes. Match existing customer art, chibi head about 45% total height, crisp stepped outlines, only 2-3 flat shades per material, upper-left lighting. True transparent background, no shadow, text, labels, grid lines, props or checkerboard. EXACT 3 columns x4 rows equal grid, portrait canvas width:height=1:2. Rows DOWN front, UP back, LEFT profile screen-left, RIGHT profile screen-right. Columns idle standing feet together, walk1 left leg forward opposite arm forward, walk2 RIGHT leg forward opposite arm; visibly opposite strides. Identical head and torso position and scale within each row, only arms and legs move. Fully visible entire bodies with padding in all 12 equal cells. Intended each cell128x192, crown y8 shoe bottom y187, centered x64. Keep apron visible in front and straps tied behind in back.
Subject: Youngja, experienced middle-aged Korean woman, SILVER GRAY WAVY CHIN-LENGTH BOB hair, no bun, no ponytail, gentle mature face with small smile creases, DEEP DARK PURPLE cardigan over cream blouse under brown apron.
```

### StaffYoungjaPortrait.png

```text
undefined
```

## 후처리

1. 생성 원본은 `Staff{Name}SourceSheet.png`로 RGBA 원본 해상도를 유지해 보관했다. 생성본의 가장자리 반투명 알파도 원본에는 유지했다.
2. 원본을 동일한 3열×4행으로 분할했다. 행 순서는 Down·Up·Left·Right, 열 순서는 서기·걷기1·걷기2다. 최종 Right는 생성 셀 대신 Left의 정확한 좌우 반전을 사용한다.
3. 셀의 알파를 128 기준으로 0/255로 정리하고, 투명 픽셀 RGB를 0으로 만들었다. 불투명 영역을 추출해 최근접 보간으로 높이 180px에 맞추고 128×192 캔버스에 가로 중앙, 위 8px에 배치했다. 원래 종횡비를 보존하며 폭이 116px를 넘는 경우만 제한한다.
4. 같은 방향의 걷기1·2 상단 90행을 서기 프레임의 픽셀로 교체해 머리 크기·위치를 고정했다. Down·Up의 걷기2는 걷기1의 y=90 아래 영역을 좌우 반전하여 팔과 발이 확실하게 반대로 나오게 했다. Left는 생성된 두 걸음의 서로 다른 팔다리 자세를 사용한다.
5. Right는 대응 Left를 RGBA 전체 좌우 반전했다. 최종 신발 바닥은 y=187이며 4px 하단 투명 여백을 둔다. 서기 파일에는 숫자 0을 붙이지 않는다.
6. 정규화한 12프레임을 `Staff{Name}Sheet.png`(384×768)에 같은 3열×4행 순서로 모았다.
7. 초상화는 최근접 보간으로 320×320에 정규화하고, 알파 0/255·투명 RGB 0으로 정리했다. 머리·어깨가 잘리지 않고 가슴 위와 앞치마 끈이 보이는지 확인했다.

## 최종 파일 규격

`Name`은 `Minji`, `Doyun`, `Youngja`다.

| 파일 패턴(각 인물) | 수량 | 크기(px) | 내용 |
|---|---:|---|---|
| `Staff{Name}Down.png`, `Down1.png`, `Down2.png` | 3 | 128×192 | 정면 서기·걷기 두 장 |
| `Staff{Name}Up.png`, `Up1.png`, `Up2.png` | 3 | 128×192 | 뒷면 서기·걷기 두 장 |
| `Staff{Name}Left.png`, `Left1.png`, `Left2.png` | 3 | 128×192 | 왼쪽 서기·걷기 두 장 |
| `Staff{Name}Right.png`, `Right1.png`, `Right2.png` | 3 | 128×192 | Left의 좌우 반전 |
| `Staff{Name}Portrait.png` | 1 | 320×320 | 투명 가슴 위 초상화 |
| `Staff{Name}Sheet.png` | 1 | 384×768 | 정규화한 12프레임 시트 |
| `Staff{Name}SourceSheet.png` | 1 | 원본별 아래 참조 | 생성 원본 시트 |

- 필수 최종 PNG 39장, 보관용 원본·정규화 시트 6장, 생성 기록 1개다.
- 걷기 권장 재생 순서는 걷기1 → 서기 → 걷기2 → 서기다.
- Unity 6000.6.4f1에서 임포트 후 Sprite(Single), Point, Mip Maps 끔, Alpha Is Transparency 켬, PPU 128, 피벗 아래 가운데를 확인한다. 초상화는 UI에서 버스트 구도를 확인한다. 이번 작업에서는 임포터·코드·씬·프리팹·에셋 설정을 변경하지 않았다.
- `.meta`는 만들지 않았다. 새 파일은 사용자가 Unity로 돌아가 임포트한다.


## ?? ?? ??? ??

| ?? | ??(px) | ??? ?? bbox |
|---|---|---|
| `StaffDoyunDown.png` | 128?192 | (17, 8, 111, 188) |
| `StaffDoyunDown1.png` | 128?192 | (17, 8, 111, 188) |
| `StaffDoyunDown2.png` | 128?192 | (17, 8, 111, 188) |
| `StaffDoyunLeft.png` | 128?192 | (16, 8, 111, 188) |
| `StaffDoyunLeft1.png` | 128?192 | (16, 8, 111, 188) |
| `StaffDoyunLeft2.png` | 128?192 | (16, 8, 111, 188) |
| `StaffDoyunPortrait.png` | 320?320 | (62, 12, 257, 301) |
| `StaffDoyunRight.png` | 128?192 | (17, 8, 112, 188) |
| `StaffDoyunRight1.png` | 128?192 | (17, 8, 112, 188) |
| `StaffDoyunRight2.png` | 128?192 | (17, 8, 112, 188) |
| `StaffDoyunSheet.png` | 384?768 | (16, 8, 368, 764) |
| `StaffDoyunSourceSheet.png` | 1024?1536 | (0, 14, 1006, 1525) |
| `StaffDoyunUp.png` | 128?192 | (16, 8, 112, 188) |
| `StaffDoyunUp1.png` | 128?192 | (16, 8, 112, 188) |
| `StaffDoyunUp2.png` | 128?192 | (16, 8, 112, 188) |
| `StaffMinjiDown.png` | 128?192 | (20, 8, 108, 188) |
| `StaffMinjiDown1.png` | 128?192 | (20, 8, 108, 188) |
| `StaffMinjiDown2.png` | 128?192 | (20, 8, 108, 188) |
| `StaffMinjiLeft.png` | 128?192 | (14, 8, 113, 188) |
| `StaffMinjiLeft1.png` | 128?192 | (14, 8, 113, 188) |
| `StaffMinjiLeft2.png` | 128?192 | (14, 8, 113, 188) |
| `StaffMinjiPortrait.png` | 320?320 | (66, 7, 270, 312) |
| `StaffMinjiRight.png` | 128?192 | (15, 8, 114, 188) |
| `StaffMinjiRight1.png` | 128?192 | (15, 8, 114, 188) |
| `StaffMinjiRight2.png` | 128?192 | (15, 8, 114, 188) |
| `StaffMinjiSheet.png` | 384?768 | (14, 8, 370, 764) |
| `StaffMinjiSourceSheet.png` | 1024?1536 | (41, 7, 1000, 1519) |
| `StaffMinjiUp.png` | 128?192 | (21, 8, 106, 188) |
| `StaffMinjiUp1.png` | 128?192 | (21, 8, 106, 188) |
| `StaffMinjiUp2.png` | 128?192 | (21, 8, 106, 188) |
| `StaffYoungjaDown.png` | 128?192 | (16, 8, 111, 188) |
| `StaffYoungjaDown1.png` | 128?192 | (16, 8, 111, 188) |
| `StaffYoungjaDown2.png` | 128?192 | (16, 8, 111, 188) |
| `StaffYoungjaLeft.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaLeft1.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaLeft2.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaPortrait.png` | 320?320 | (49, 16, 267, 307) |
| `StaffYoungjaRight.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaRight1.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaRight2.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaSheet.png` | 384?768 | (16, 8, 367, 764) |
| `StaffYoungjaSourceSheet.png` | 887?1774 | (31, 15, 875, 1774) |
| `StaffYoungjaUp.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaUp1.png` | 128?192 | (18, 8, 110, 188) |
| `StaffYoungjaUp2.png` | 128?192 | (18, 8, 110, 188) |

??: 36? ?? ???? RGBA?128?192??? ? y=8?? ?? ? y=188(??? y=187)??? ????? RGB 0??? 90? ????? ??? ???Right=mirror(Left)? ?? ????. ??? 3?? 320?320??? RGB 0??? ??? ????. ??? ??? ??? ????? ????? ????????? ?????? ?? ????. ??? ?? ???? ?? ????? ????. Unity ???? ?? ?? ?? ??? ??? ????. ? ADR? ??.
