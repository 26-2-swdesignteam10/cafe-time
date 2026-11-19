Generated with OpenAI built-in image_gen. Style reference: Assets/_Project/Art/Title/CafeInterior.png; docs/ART_STYLE.md.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Style reference input is CafeInterior.png, use ONLY its warm cozy cafe pixel aesthetic, do not depict the cafe. Orthographic top-down three-quarter farming sim character sprites, slightly see tops of heads and shoes. Chibi head about 45% of body height, consistent anatomy and outfit in every cell. Bold dark coffee #380F00 stepped pixel outline about 4px at final128x192 resolution, only 2-3 flat shade blocks per material. Cream #FAEAC2 #FCF0CA #F7DCA5; walnut #90471B #5D2608; gold #C57D1F #F1C97D. Upper-left light. True transparent alpha background, absolutely no floor, ground shadow, checkerboard, labels, text, furniture, props or borders. Crisp deliberate pixel art edges, no blur, no gradients. Each grid cell represents frame 128x192, 1 tile =128px. Characters identical scale in each cell, centered horizontally, full body, standing head top near 8px and shoes bottom at188px in final frame. Leave transparent space around silhouettes. Do not touch neighboring cells. ONE animation sprite sheet PLAYER young barista, short brown hair, cream collared shirt sleeves rolled, WALNUT BROWN bib APRON with one small cream pocket, dark trousers, brown shoes. Match second reference's simple blocky PIXEL rendering and chibi proportions exactly. Use only 2-3 shades, thick dark coffee outline. Exactly THREE COLUMNS by FOUR ROWS uniform grid, no gutters. Portrait sheet. Row1 DOWN toward camera: IDLE feet together; WALK1 character's LEFT leg (on viewer RIGHT) extended forward and opposite arm forward; WALK2 character's RIGHT leg (on viewer LEFT) extended forward and opposite arm forward. WALK2 MUST HAVE OPPOSITE LEG from WALK1. Row2 UP back view: IDLE; WALK1 left leg toward screen-top; WALK2 right leg toward screen-top. Row3 LEFT profile facing screen-left: IDLE; WALK1 wide step NEAR leg forward dark FAR leg behind; WALK2 DARK FAR leg forward and NEAR leg behind, reverse arm swing too. Row4 RIGHT profile: corresponding mirrored poses of Row3. All 12 frames same head, body proportions, clothing and colors. Keep heads and torsos stationary, only limbs move. Do not duplicate WALK1 into WALK2. Entire character fully visible in every cell. Reference1 ONLY warm cafe aesthetic; reference2 ONLY pixel rendering style, not outfit.

??: frame 128x192, 1 tile = 128 px. RGBA ?? ??, ? ?? ?? 4 px.

- PlayerSourceSheet.png: ??? ??? ? ?? ??? ?? ??. ?? ?? ??, ?? ??, ??? ???, ??? ??.
- PlayerSheet.png: ?? ?? ???? ??? ?? ??? ??? ?? ?? ??.
- PlayerDown0.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ???? ?? ??. ??.
- PlayerDown1.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ???? ?? ??. ?? ?? ??.
- PlayerDown2.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ???? ?? ??. ??? ?? ??.
- PlayerUp0.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ???. ??.
- PlayerUp1.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ???. ?? ?? ??.
- PlayerUp2.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ???. ??? ?? ??.
- PlayerLeft0.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ?? ??. ??.
- PlayerLeft1.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ?? ??. ?? ?? ??.
- PlayerLeft2.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ?? ??. ??? ?? ??.
- PlayerRight0.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ?? ???? ??? ?? ??, ??? ?? ??. ??.
- PlayerRight1.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ?? ???? ??? ?? ??, ??? ?? ??. ?? ?? ??.
- PlayerRight2.png: ?? ?? ??, ?? ??, ??? ???, ??? ??. ?? ???? ??? ?? ??, ??? ?? ??. ??? ?? ??.

??: ?? ?? ?? ? ?? 128 ?? ??? ? ??? ?? ?? ? ???? ?? ??? nearest-neighbor ?? ? 128?192 ?? ???? ?? ??/?? y=187 ??. ?? ???? ?? ?? ?? ??. ?? ???? ?? ??? ??? ?? ??? ???.

??: ??? ? ?? ?? ???? ??? ???? ????, ??? ??? ?? ? ??? ??/??? ??? ???. ?? image_gen?? ? ?? ? ? ?? ?? ??? ?? ?? ??(?? ?? ?? ??? ??? ??? ?? ??)? ??? ??? ???.

??: ?? ?? ??? ??, RGBA/?? ??, ?? ?? ?? ?1 px, ? ?? 4 px, Right=mirror(Left) ??? ???. ?? ??? ??/??/?/???? ??? ???.

| ?? | ?? ?? | ?? | ??? ?? |
|---|---|---|---|
| PlayerDown0.png | 128?192 | ? | (23, 10, 105, 188) |
| PlayerDown1.png | 128?192 | ? | (23, 11, 105, 188) |
| PlayerDown2.png | 128?192 | ? | (23, 10, 105, 188) |
| PlayerUp0.png | 128?192 | ? | (23, 9, 105, 188) |
| PlayerUp1.png | 128?192 | ? | (23, 10, 105, 188) |
| PlayerUp2.png | 128?192 | ? | (23, 9, 105, 188) |
| PlayerLeft0.png | 128?192 | ? | (21, 8, 106, 188) |
| PlayerLeft1.png | 128?192 | ? | (18, 10, 109, 188) |
| PlayerLeft2.png | 128?192 | ? | (20, 10, 107, 188) |
| PlayerRight0.png | 128?192 | ? | (22, 8, 107, 188) |
| PlayerRight1.png | 128?192 | ? | (19, 10, 110, 188) |
| PlayerRight2.png | 128?192 | ? | (21, 10, 108, 188) |

## PR #8 교체 (2026-11)

Codex(AI 에이전트)로 다시 만들어 교체한 것으로 보인다. 도구 확인과 파일별 프롬프트는 작성자에게 받아 보충한다.

- 교체한 파일: PlayerDown0.png, PlayerDown1.png, PlayerDown2.png, PlayerLeft0.png, PlayerLeft1.png, PlayerLeft2.png, PlayerRight0.png, PlayerRight1.png, PlayerRight2.png, PlayerSheet.png, PlayerSourceSheet.png, PlayerUp0.png, PlayerUp1.png, PlayerUp2.png

## 2026-11-19 캐릭터 개편

도구: OpenAI 내장 `image_gen`(투명 배경 생성·동일 인물 표정 편집), Python 3 + Pillow(사용자가 요청한 nearest-neighbor 축소·프레임 분리·발 정렬·반전·알파 정리·미리보기). API/CLI는 사용하지 않았다.

공통 시작 프롬프트: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

투명 배경은 알파 0, RGB (0,0,0)으로 정리했고, 최종 PNG의 알파는 0/255다. 글자·효과선·반짝이·체커보드·바닥 그림자·가구를 넣지 않았다. 당황 초상화의 요청된 땀방울은 유지했다. 아래 bbox는 알파 > 0의 `(left, top, right, bottom)`이며 오른쪽·아래쪽은 미포함, 원점은 왼쪽 위다.

스타일 참조(이미지 생성 입력): 기존 `Sprites/Player/PlayerSheet.png`의 도트·치비 비율, `UI/Ending/Ending04.png`, `Ending02.png`의 주인공 디자인. 손님 시트는 새 `PlayerSourceSheet.png`에 해당하는 생성 원본과 해당 손님 초상화 원본을 함께 입력했다. `docs/ART_STYLE.md`의 3/4 탑다운·128px 타일·왼쪽 위 빛·크림/월넛/골드·다크 커피 외곽선 규칙을 프롬프트에 명시했다.

인게임 공통 제작 지시: 머리가 전체 높이 약 45%인 같은 치비 비율, 최종 기준 약 4px의 `#380F00` 계열 계단식 외곽선, 재질당 2~3단계 명암, 왼쪽 위 빛. 128×192 RGBA, 아래 가운데 피벗 기준, 머리 위 여백 8px, 서기·걷기 신발 바닥 y=187. 0은 idle, 1·2는 서로 반대 발과 팔 동작. nearest-neighbor 축소 후 방향별 idle 머리의 위쪽 90행을 재사용하여 프레임 간 머리의 크기·위치를 고정했다. Right=mirror(Left).

주인공 제작 지시: 긴 웨이브 진갈색 머리(어깨 아래), 반묶음과 큰 크림 리본, 앞머리, 하이라이트가 있는 큰 갈색 눈과 볼터치, 둥근 깃·퍼프 소매 크림 블라우스, 올리브 그린 멜빵 앞치마 원피스. 갈색 신발. 대화 초상화와 동일한 인물로 보이게 한다.

### 파일별 프롬프트·제작 지시와 최종 측정

| 파일 | 최종 크기(px) | 불투명 영역 bbox | 파일별 프롬프트 / 제작 지시 |
|---|---|---|---|
| PlayerSourceSheet.png | 887×1774 | (37, 19, 852, 1748) | 생성 원본: 3열×4행, Down/Up/Left/Right 순서, 각 행 idle·왼발·오른발. RGBA 정리, 원본 해상도 유지. |
| PlayerDown0.png | 128×192 | (16, 8, 111, 188) | 카메라를 보는 정면, 서 있기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerDown1.png | 128×192 | (16, 8, 111, 188) | 카메라를 보는 정면, 왼발 앞으로·반대 팔 앞으로 걷기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerDown2.png | 128×192 | (16, 8, 111, 188) | 카메라를 보는 정면, 오른발 앞으로·반대 팔 앞으로 걷기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerUp0.png | 128×192 | (19, 8, 109, 188) | 등을 보이는 뒷면, 서 있기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerUp1.png | 128×192 | (19, 8, 109, 188) | 등을 보이는 뒷면, 왼발 앞으로·반대 팔 앞으로 걷기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerUp2.png | 128×192 | (19, 8, 109, 188) | 등을 보이는 뒷면, 오른발 앞으로·반대 팔 앞으로 걷기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerLeft0.png | 128×192 | (16, 8, 113, 188) | 화면 왼쪽 옆모습, 서 있기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerLeft1.png | 128×192 | (9, 8, 113, 188) | 화면 왼쪽 옆모습, 왼발 앞으로·반대 팔 앞으로 걷기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerLeft2.png | 128×192 | (11, 8, 113, 188) | 화면 왼쪽 옆모습, 오른발 앞으로·반대 팔 앞으로 걷기. 머리 위쪽 90행은 같은 방향 idle의 동일 픽셀로 고정. |
| PlayerRight0.png | 128×192 | (15, 8, 112, 188) | 화면 오른쪽 옆모습, 서 있기. 대응 Left 프레임을 RGBA 전체 좌우 반전. |
| PlayerRight1.png | 128×192 | (15, 8, 119, 188) | 화면 오른쪽 옆모습, 왼발 앞으로·반대 팔 앞으로 걷기. 대응 Left 프레임을 RGBA 전체 좌우 반전. |
| PlayerRight2.png | 128×192 | (15, 8, 117, 188) | 화면 오른쪽 옆모습, 오른발 앞으로·반대 팔 앞으로 걷기. 대응 Left 프레임을 RGBA 전체 좌우 반전. |
| PlayerSheet.png | 384×768 | (15, 8, 373, 764) | 최종 3열×4행, Down/Up/Left/Right 순서, 각 행 0·1·2 프레임. |

검수: 동일 인물·화풍·잘림을 정적 미리보기로 육안 확인. RGBA·투명 RGB 0·발바닥 y=187·좌우 반전·걷기 두 발의 차이·머리 고정 픽셀을 자동 검사했다. scratch의 `PortraitsComparison.png`, `PlayerWalkPreview.png`, `Customer1WalkPreview.png`~`Customer4WalkPreview.png`, `AllCharactersWalkPreview.png`, `AllWalkAnimation.gif`, `SeatingPreview.png`로 확인할 수 있다. 새 PNG의 .meta는 생성하지 않았으며 Unity 임포트와 실제 UI/Play 확인은 별도 필요하다.
