Generated with OpenAI built-in image_gen (내장 이미지 생성 도구). Style reference: Assets/_Project/Art/Title/CafeInterior.png, docs/ART_STYLE.md.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Reference image is ONLY for colors/mood/materials, not composition or amount of detail. ONE isolated world sprite. Consistent orthographic top-down 3/4 RPG camera: front face visible, see roughly one third depth of tops, verticals stay vertical, horizontal front edges horizontal (NOT isometric diamond). Top-left lighting. Dark coffee #380F00 outline about 4 pixels at target size, clean stepped pixel edges, 2-3 shade blocks, no blur or gradients. Palette cream #FAEAC2 #FCF0CA #F7DCA5 honey #C57D1F #F1C97D walnut #90471B #5D2608. Metals warm cream-gray, glass pale desaturated blue. No text, letters, logos, labels, ground, background, drop shadows or extra props. Fill the requested canvas aspect ratio with about 4px transparent padding, centered horizontally, feet/base at bottom padding. All details readable at final small size, avoid microtexture.

스케일: **1 tile = 128 px** = 1 Unity unit. 최종 PNG는 RGBA이며 파일별 지정 크기를 사용한다. 오브젝트는 하단 중앙 배치를 기준으로 사방 4px 투명 여백을 두었다. 타일 4종은 완전 불투명이다. 이미지에는 글자를 넣지 않았다.

- Floor.png (128×128): Seamless warm honey wood plank floor, horizontal planks; fully opaque, no outline, repeat on all four edges.
- Wall.png (128×128): ONE square interior WALL TILE FRONT VIEW, cream plaster upper two thirds, walnut wainscoting bottom third. Fully opaque edge-to-edge tile, no outside outline. Seamless horizontally AND vertically: edge rows must match, make a fine cream separator at the bottom boundary that continues into the next plaster tile. Simple wood panels and a honey chair rail. Do not draw room, floor or perspective.
- Counter.png (128×128): ONE square seamlessly horizontally repeatable service COUNTER TILE. Straight front view. Warm honey wood counter top visible across top 30px, walnut paneled counter front below. Fully opaque edge to edge. No outer outline. No devices or accessories. Horizontal edges match perfectly.
- Door.png (128×128): ONE square fully opaque door tile, one half of double GLASS cafe entrance door, straight front view. Walnut rectangular frame, glass warm pale blue reflecting cream highlights, small brass vertical handle. No outside scene. Two IDENTICAL copies placed side by side must look like cafe double doors. Fill all edges.
- Table.png (256×192): Small SQUARE honey-wood cafe table for four, EMPTY top, four walnut legs. Footprint 2 x 1.5 tiles, final canvas 256x192. Broad square top shown in orthographic 3/4 view, not round, not dining table elongated rectangle.
- ChairLeft.png (96×128): ONE wooden cafe chair FACING RIGHT, used on LEFT of table. Side three-quarter view clearly right-facing seat, backrest on LEFT, front of seat on RIGHT. Honey seat, walnut slatted back and legs. Final canvas 96x128.
- StationCupShelf.png (224×192): Floor-standing OPEN walnut cup shelf with two shelves filled with stacked white ceramic cups and clear plastic cups; sturdy base. No hanging ornaments. Final canvas 224x192.
- StationEspresso.png (256×192): Two-group stainless commercial espresso machine with walnut accents ON SMALL WALNUT CABINET, grinder on LEFT, two portafilters, drip tray. Floor standing station. Final canvas 256x192.
- StationIceMaker.png (192×192): Compact floor-standing stainless ice maker, square body with a front door window showing pale blue ice cubes, small lower vent, four short feet. Final canvas 192x192.
- StationWater.png (160×192): Compact cream and stainless water purifier / hot water dispenser on a SMALL WALNUT STAND, two short taps over drip tray. No text. Floor-standing station final canvas 160x192.
- StationDrinkBar.png (288×192): Floor standing walnut drink PREP COUNTER with honey worktop, small scale, syrup pump bottles, milk carton, tea boxes. All containers unlettered. Balanced uncluttered workstation. Final canvas 288x192.
- StationShakeBar.png (288×192): Floor standing compact walnut BLENDER STATION with honey worktop, TWO clear blender jugs on cream motor bases, fruit puree packs with no text. Final canvas 288x192.
- StationDishwash.png (352×192): Floor standing wide stainless SINK unit on walnut base cabinet, curved faucet, sink basin on LEFT, drying rack with ceramic cups on RIGHT. Final canvas 352x192.
- StationTowel.png (192×160): ONE compact walnut wall-mounted TOWEL RACK with a folded cream cloth draped over horizontal bar, small spray bottle on shallow shelf. Transparent background including behind frame. No floor or wall. Final canvas 192x160.
- StationPos.png (160×160): COUNTERTOP DEVICE ONLY: POS terminal with dark green blank tilted screen, small walnut base and separate compact card reader beside it. No supporting service counter, no table, no cabinet. Final canvas 160x160.
- StationPickup.png (320×160): COUNTERTOP OBJECT ONLY: low open walnut PICKUP SHELF with small brass bell and empty pickup tray, no text. Wide compact two-level shelf. Do not draw supporting counter, floor or cabinet. Final canvas 320x160.
- StationDessertCase.png (288×160): COUNTERTOP DEVICE ONLY: low rectangular GLASS DESSERT SHOWCASE with walnut trim, strawberry cake slice, golden croissants and cookies inside on two shelves. Glass box clearly encloses pastries, pale blue subtle flat glass highlights. No supporting counter, cabinet or floor. Final canvas 288x160.
- StationTrayReturn.png (288×160): COUNTERTOP OBJECT ONLY: open walnut TRAY RETURN shelf/window with rectangular opening and stack of brown trays, compact wide frame and low bottom lip. No supporting service counter, no cabinet, no wall or floor. Final canvas 288x160.
- ChairRight.png (96×128): ChairLeft.png를 정확히 좌우 반전. 형태·픽셀·여백을 동일하게 유지했다. 좌우 반전 요구에 따라 조명 명암도 함께 반전된다.

재생성 프롬프트와 수정 사항:

- Table.png: `EXACTLY ONE TABLE ALONE. Empty square honey wood tabletop and four walnut legs ONLY. NO CHAIRS ANYWHERE. The table seats four but DO NOT DRAW ANY CHAIRS.` 의자가 함께 생성된 첫 결과를 폐기했다.
- Door.png: `EXACTLY ONE SINGLE LEAF DOOR filling a square texture canvas. ONE glass pane in ONE walnut door frame with ONE brass handle on the right. Not a pair. No center mullion, no second door. Full bleed walnut frame touches all four image boundaries, NO margins, NO exterior wall, NO white border. Entire image fully opaque.` 여백이 있는 결과와 문 두 짝이 함께 그려진 결과를 폐기했다. 최종본은 한 짝이며 같은 타일 두 장을 옆으로 반복할 수 있다.
- StationDrinkBar.png: `STRICT INVENTORY: only a low walnut cabinet and honey top, ONE digital scale with blank display, THREE syrup pump bottles, ONE milk carton and TWO tea boxes. NO espresso machine, NO blender, NO plants, NO leaves, NO upper shelving, NO jars, NO cups, NO decorations.` 불필요한 기기·식물·상부 선반을 제외하고 재생성했다.
- StationShakeBar.png: `STRICT INVENTORY: low walnut cabinet with honey worktop, TWO blenders with clear jugs, THREE fruit puree pouches unlettered. NOTHING ELSE. NO plants, NO leaves, NO upper shelving, NO jars, NO bowls, NO decorations, no cups.` 추가 장식을 제외하고 재생성했다.
- StationTowel.png: `STRICT INVENTORY ONLY ONE walnut rack, ONE folded cream cloth draped neatly over bar, ONE small spray bottle. NO plants, NO leaves, NO vines, NO flowers, NO other objects or decorations.` 식물 장식을 제외하고 재생성했다.

최종 규격 정리:

- 생성된 PNG의 알파 영역을 잘라 지정 종횡비와 크기에 맞췄다. 최근접 보간과 오브젝트 24색·타일 20색 양자화로 작은 크기에서도 픽셀 경계와 명암 블록이 유지되도록 정리했다. 오브젝트 알파는 0 또는 255로 정리했다.
- 오브젝트의 불투명 바운딩 박스는 `(4, 4, width-4, height-4)`이다. ChairRight는 최종 ChairLeft를 반전해 저장했다.
- Floor와 Wall은 가로·세로, Counter와 Door는 가로 반대편 경계 픽셀을 일치시켰다. Wall은 각 타일 아래 1/3의 목재 패널이 세로로 반복되며, 아래 경계에는 크림색 이음선을 둔다.
- 금속·유리·음식에 한해 따뜻한 회색, 옅은 청회색, 딸기색 보조색을 사용했다.

검증 결과:

- 요청된 19개 PNG 모두 지정 크기 일치. 타일 4종 알파 255, 오브젝트 15종 투명 배경 및 4px 여백 확인.
- Floor/Wall 가로·세로, Counter/Door 가로 반대편 경계의 다른 픽셀 수: 0.
- 전체 콘택트 시트와 3×3 타일 반복 이미지를 만들어 육안 검토했다. 테이블·출입문·제조대·수건 선반의 구성 오류는 재생성했다. 검토용 이미지는 최종 에셋에서 제외했다.
- 기존 `.meta`, `Square.png`, `*Tile.asset`은 수정하지 않았다. 씬·프리팹·스크립트는 수정하지 않았다.
- `bash .github/scripts/check-repo.sh`: 기존 `Assets/_Project/Prefabs/OpenPrep.meta`에 대해 `.meta만 있고 원본이 없음` 오류 1건으로 실패. 이번 작업과 무관한 기존 추적 상태이므로 변경하지 않았다. 새 에셋의 `.meta`는 Unity가 임포트할 때 생성해야 한다.

Unity 확인 순서:

1. Unity 6000.6.4f1 에디터로 돌아가 새 PNG와 이 문서를 임포트한다. `.meta`는 에디터가 생성하게 한다.
2. 새 스프라이트는 Sprite (2D and UI), Single, Pixels Per Unit 128, Point 필터, Mip Maps 끔, Alpha Is Transparency 켬, Pivot Bottom Center를 확인한다. 기존 타일의 임포트 설정과 `.meta`는 이번 작업에서 변경하지 않았다.
3. Cafe 씬에서 타일 반복과 오브젝트 크기를 확인한다. 새 가구·기기의 씬 연결은 이번 이미지 제작 작업 범위에 포함하지 않았다.


## 2026-11-04 — 기기 상태 말풍선과 테이블 쟁반

Generated with OpenAI built-in image_gen (내장 이미지 생성 도구). Style reference: Assets/_Project/Art/Sprites/Cafe/Table.png; Assets/_Project/Art/Sprites/Cafe/StationEspresso.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: style-transfer. ONE isolated world sprite, no sheet, no labels or text. Match reference pixel art, 2-3 flat shade blocks, no blur, no gradients. Dark coffee #380F00 stepped outline 3-4px at FINAL small size. Palette cream #FCF0CA #FAEAC2 #F7DCA5, walnut #90471B #5D2608, honey #C57D1F. Upper-left light. Transparent RGBA background, even 4px final transparent padding. 

- WorkDone.png (96×96): ONE round cream speech bubble, short bottom-center tail, dark coffee outline, subtle honey-cream bevel; large clear GREEN CHECK MARK centered inside. NO letters. Exact FINAL canvas 96x96 pixels, NOT doubled. Design readable at this small size.
- WorkBubble.png (96×96): ONE round cream speech bubble, short bottom-center tail, dark coffee outline, subtle honey-cream bevel; small simple walnut-and-honey HOURGLASS with sand centered inside. Identical bubble family to WorkDone. Exact FINAL canvas 96x96 pixels, NOT doubled. Design readable at this small size.
- CleanRequest.png (96×96): ONE round cream speech bubble, short bottom-center tail, dark coffee outline, subtle honey-cream bevel; folded light-blue cleaning CLOTH with one cream-gold four-point SPARKLE inside. Identical bubble family to WorkDone. Exact FINAL canvas 96x96 pixels, NOT doubled. Design readable at this small size.
- TrayOnTable.png (160×112): ONE brown walnut rectangular tray containing two used cups (one cream ceramic, one clear plastic), one crumpled cream napkin and one cream dessert plate with crumbs. Orthographic top-down 3/4 cafe map view matching Table.png tabletop and StationEspresso, horizontal front edge horizontal, vertical cup sides vertical, not isometric diamond. Top-left light. NO table, legs, furniture, background or shadow. Compact low tray fully visible. Exact FINAL canvas 160x112 pixels, NOT doubled. Design readable at this small size.

최종 규격 및 검수:

- 상태 말풍선 3종은 각각 96×96, TrayOnTable은 160×112 RGBA PNG이다. 투명 배경이며 글자는 없다.
- 생성 원본의 알파 128 이상 영역을 기준으로 종횡비를 유지해 최근접 보간으로 축소했다. 최종 캔버스에서 최소 4px 여백을 확보하고 중앙 정렬했다. 알파는 0/255, 투명 픽셀 RGB는 0으로 정리했다.
- 초록 체크, 모래시계, 파란 천과 반짝임을 최종 크기에서 육안으로 확인했다. 쟁반에는 사용한 컵 2개·구겨진 냅킨·부스러기가 있는 접시가 있으며, 기존 Table과 같은 수평 앞면의 3/4 시점을 확인했다.
- 체크에는 초록색, 청소 천에는 옅은 파란색 보조색을 사용했다. 기존 월드 스프라이트와 .meta는 변경하지 않았다.
- Unity 6000.6.4f1에서 임포트해 .meta를 생성한다. Sprite (2D and UI), Single, PPU 128, Point 필터, Mip Maps 끔, Alpha Is Transparency 켬, Bottom Center Pivot을 확인한다. 실제 씬에서 말풍선 위치와 쟁반 배치를 확인한다. 씬 연결과 스크립트 변경은 포함하지 않았다.

