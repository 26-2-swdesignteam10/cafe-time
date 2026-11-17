# 베이커리 미니 스프라이트 생성 기록

- 날짜: 2026-11-17.
- 도구: OpenAI 내장 `image_gen` (8종을 각각 별도 호출, `transparent_background=true`), Pillow (알파 경계 정리, 최근접 축소, 디더링 없는 20색 정리, 최종 캔버스 배치).
- 스타일 참조: `Assets/_Project/Art/Icons/Ingredients/Dessert*.png`의 각 해당 음식 이미지(생김새·음식색), `Assets/_Project/Art/Sprites/Cafe/Booth.png` (맵 가구 시점·픽셀 표현). 아래 각 호출에 해당 디저트 이미지와 Booth를 직접 전달했다. DessertBagle은 기존 파일명의 철자를 그대로 참조했으며, 새 파일은 요청한 BakeryBagel이다.
- 진열대 스타일 연계: `../BreadShelf.png`, `Assets/_Project/Art/Title/CafeInterior.png`. 두 이미지는 직접 열어 화풍과 색을 확인했다. 생성 입력 이미지는 위의 디저트와 Booth 두 장이다.
- 스케일: 1 tile = 128px. 최종 파일은 모두 **48×40px RGBA**, 투명 배경, 사방 최소 2px 여백, 하단 중앙 정렬. 빵은 각각 한 개 또는 한 조각뿐이며 접시·배경·장식·글자가 없다.

## 파일별 실제 프롬프트

### BakeryStrawberryCake.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertStrawberryCake.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny triangular strawberry layer cake SLICE, cream and golden sponge, pink strawberry cross sections in middle, ONE small strawberry on cream on top. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```

### BakeryCroissant.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertCroissant.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny golden crescent croissant, simple curved silhouette, 3 broad laminated segments. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```

### BakeryBagel.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertBagle.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny round sesame bagel with a clearly visible center hole and a thin cream cheese filling, no second bagel. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```

### BakeryWaffle.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertWaffle.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny square waffle piece, 3 by 3 clearly defined square grid indentations, a tiny cream butter dab, honey gold. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```

### BakerySandwich.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertSandwich.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny triangular sandwich HALF only, cream toast triangle, simple green lettuce, red tomato and pink ham layers. Do not copy the two stacked sandwiches in reference. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```

### BakeryCroqueMonsieur.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertCroqueMonsieur.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny square croque monsieur sandwich, golden toasted square bread, pale melted cheese top, pink ham in center, few green herb pixels. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```

### BakeryCookie.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertCookie.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny round chocolate chip cookie, golden baked biscuit, four large dark chocolate chips. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```

### BakeryHoneyBread.png

입력 참조: `Assets/_Project/Art/Icons/Ingredients/DessertHoneyBread.png`, `Assets/_Project/Art/Sprites/Cafe/Booth.png`.

```text
Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. Orthographic front-facing 3/4 top-down RPG map view, horizontal front edges horizontal, verticals vertical, NOT isometric. Light upper-left. Crisp pixel art with simple 2-3 flat shade blocks, no gradient, blur, antialiasing, grain, dither, text, logos, plate, ground, shadow or scenery. Reference image is appearance reference only; simplify to one small item, not an icon panel. ONE tiny cube of honey toast bread, square golden bread block, honey drips, ONE simple cream swirl on top. FINAL canvas 48 x 40 pixels RGBA, transparent background, at least 2px clear padding all sides, object bottom center aligned with base at y37 top-origin. Single food object only, no serving plate. Final dark coffee #380F00 stepped outline approximately 2px. Broad readable shapes, minimal details, cute at tiny size, NOT a large detailed illustration. Object must fit 44 x36 inner pixels. One piece only. Match reference natural food colors and 3/4 camera.
```


## 최종 규격 처리와 검사

- 생성본 알파를 128 기준으로 0/255로 정리한 뒤 내용 경계로 크롭했다.
- 종횡비를 보존해 42×34px 이내로 최근접 축소했다. 생성 음식색을 기준으로 디더링 없이 20색으로 정리했으며 알파를 보존했다. 생성된 외곽선에 다크 커피 #380F00을 바깥으로 1px 보강하여 최종 약 2px 외곽선을 맞췄다. 보강 후 내용 영역은 44×36px 이내다. 투명 픽셀 RGB는 0이다.
- 48×40px 투명 캔버스에서 x=(48-내용 너비)//2, 위쪽 y=38-내용 높이에 배치했다. 아래 투명 여백은 정확히 2px이고 나머지 방향은 최소 2px다. 비율에 따라 위쪽·좌우 여백은 더 클 수 있다.
- 최종 PNG를 다시 읽어 크기, RGBA 모드, 알파 0/255, 투명 여백, 하단 중앙 정렬을 검사하고 8종을 직접 열어 음식 구분과 한 개 구성을 확인했다.
- 진열대 칸에 최대 6개를 배치할 때는 3×2 밀집 배치를 기준으로 투명 여백과 앞뒤 빵을 일부 겹친다. 칸 좌표는 `../Generation.md`의 「빵 진열대 재제작 (2026-11-17)」 표를 사용한다.
- Unity 임포트 및 실제 맵 배치는 수행하지 않았다. Unity 6000.6.4f1에서 임포트하여 `.meta`를 생성하고 Sprite/Single, PPU 128, Point, Mip Maps 끔, Alpha Is Transparency 켬, Bottom Center를 확인한다.
