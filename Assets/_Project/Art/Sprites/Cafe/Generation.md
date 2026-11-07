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



## 모던 스타일 시안 (ADR-0016)

Wall.png(WallModern), Floor.png(FloorHall), FloorKitchen.png, StationEspresso.png 를 아래 기록대로 교체했다. 나머지 그림은 아직 이전 스타일이다.

Generated with OpenAI built-in image_gen (내장 이미지 생성 도구). Style reference: Assets/_Project/Art/Sprites/Cafe/StationEspresso.png, Wall.png, Floor.png (시점·구도·크기 비율만 참고).

Prompt set: Use case: stylized-concept. Production Unity 2D cafe game sprite, NEW clean modern minimalist pixel-art direction. High resolution drawing with deliberate crisp stepped pixel edges, only 2-3 discrete flat shade blocks per material, no blur, gradients, antialiasing, dithering, noise or painterly texture. Warm white, pale oak, neutral matte gray, stainless steel and matte black. Sage green is the only allowed accent color and is optional, used sparingly. No walnut, honey gold, brown outlines, ornament, lettering, logos, labels, watermark, ground shadows or extra props. 

- WallModern.png: ONE seamless square warm-white painted wall texture, full bleed fully opaque. Target 128x128 pixels. Very plain almost solid warm white (#F1EFE8), at most a few extremely subtle flat warm-white pixel blocks. NO panels, trim, baseboard, stripes, brick, grain, lighting gradient or borders. Must seamlessly repeat in BOTH axes, especially stacked TWO tiles high on map top wall, with no visible seam or recurring motif. All four edges same warm white, no outline, uniform illumination. Texture only, no room or perspective.
- FloorHall.png: ONE seamless square light natural oak floor texture, full bleed fully opaque. Target 128x128 pixels. Broad horizontal straight pale oak planks with extremely low contrast thin joints, sparse straight restrained pixel wood grain. Calm clean pale beige oak, NOT orange, yellow or walnut. Three or four broad plank courses with staggered end joints that wrap perfectly across boundaries. Seamless horizontally AND vertically. No outline, no bevel, no tile border, no shadow, no objects, no perspective, uniform illumination. Flat top-down texture only.
- FloorKitchen.png: ONE seamless square matte light neutral gray ceramic floor tile texture, full bleed fully opaque. Target 128x128 pixels. Simple restrained square gray tiles with narrow medium-gray grout, straight horizontal and vertical aligned orthogonal grid, four tile cells across and four down. Each ceramic cell essentially solid flat gray, slight 2-tone variation only, no speckles, no bevels, no glossy highlights, no diagonal geometry. Grout wraps precisely across all FOUR edges, periodic seamless in BOTH axes. No outline, no outer border, no objects, no perspective, uniform illumination. Flat top-down texture only.
- StationEspresso.png: ONE isolated floor-standing espresso station sprite. EXACT final aspect ratio 256x192 (4:3). One TWO-GROUP commercial stainless steel and matte black espresso machine, TWO portafilters and drip tray, ONE grinder on LEFT, both on ONE low straight rectangular matte black and stainless cabinet. No cups, no beans outside hopper, no decorative objects, no wood anywhere. Small sage-green control light only if needed. Clean straight squared modern forms. Orthographic front-facing 3/4 RPG view: front clearly visible, see roughly one third depth of tops, verticals vertical, horizontal front edges horizontal, NOT isometric diamond and NOT side facing product shot. Upper-left lighting using 2-3 discrete flat tones, no cast shadow. Thin charcoal #2B2B2B outline 2-3px per 128px tile, crisp stepped corners. Broad low proportions comparable to a 2 tile wide by 1.5 tile high station. Entire cabinet base visible. GENUINELY TRANSPARENT BACKGROUND, alpha not checkerboard drawing, no ground, wall or floor. Center horizontally, fit entire object with exactly 4px transparent padding on each side at final size; base sits at y187 with 4px bottom padding. No text or logos even on controls.

## 적용 범위

- 2026-11-05, 카페 맵 새 스타일의 1단계 시안. 사용자가 승인한 모던 미니멀 방향을 적용했다. 기존 ART_STYLE.md 1~2장의 콘셉트와 팔레트는 적용하지 않았다.
- 5장의 PNG/RGBA, 명명, 128px/칸, 3/4 시점·왼쪽 위 조명 규격과 7장의 생성 기록 형식을 따른다. 저장 위치와 개별 이미지 크기는 사용자가 지정한 이번 시안 규격이 우선한다.
- 저장소는 읽기만 했다. Assets, .meta, 씬, 프리팹, 코드, ADR은 변경하지 않았다. 기존 codex_log.txt는 보존했다.

## 파일별 수정 사항

- WallModern.png (128×128): 생성 원본을 최근접 보간으로 축소한 뒤 대표 웜 화이트 1색으로 정리했다. 원본의 매우 미세한 색 변화를 없애 가로·세로 반복과 두 칸 높이 적층에서 띠·패턴이 생기지 않게 했다. 불투명, 외곽선 없음.
- FloorHall.png (128×128): 생성 원본을 최근접 보간으로 축소, 디더링 없이 4색(목재 3색 + 이음선)으로 정리했다. 수평 판재 이음선은 32px 주기, 경계 양쪽 각 1px로 맞췄고 반대편 경계 픽셀을 일치시켰다. 불투명, 외곽선 없음.
- FloorKitchen.png (128×128): 생성 원본을 최근접 보간으로 축소, 회색 2색으로 정리했다. 생성된 줄눈을 32px 주기로 맞추고 각 경계 양쪽 1px를 줄눈으로 정렬했다. 3×3 중간 검토에서 보인 양자화 얼룩은 3×3 중앙값 필터로 제거하고 줄눈 경계를 재확인했다. 최종 이미지에는 그라데이션과 흐림이 없다. 불투명, 외곽선 없음.
- StationEspresso.png (256×192): 생성 원본의 알파 128 이상 영역을 기준으로 잘랐다. 좌우·상하 4px 여백을 모두 충족하도록 내용 영역을 248×184로 최근접 보간 축소했다. 원본 내용 비율 대비 가로 비율이 약 11.5% 넓어지는 보정이 있다. 바닥면은 y=187, 불투명 바운딩 박스는 (4,4,252,188), 아래쪽 가운데 기준이다. 원본 크기에서 9×9 중앙값 필터로 미세 색 노이즈를 줄이고, 차콜·금속 회색·커피색·세이지 9색 고정 팔레트로 디더링 없이 정리한 뒤 5×5 중앙값 필터와 최근접 축소를 적용했다. 알파는 0/255, 투명 픽셀의 RGB는 0. 세이지는 작은 기기 표시등에만 썼다. 외곽선은 최종 크기에서 대체로 2px이며 계단식 모서리를 유지했다. 외부 투영 그림자, 바닥, 추가 소품, 글자는 없다.

## 검토 이미지와 확인 결과

- CheckWallModern.png, CheckFloorHall.png, CheckFloorKitchen.png: 각각 해당 최종 타일의 3×3 반복 이미지, 384×384 RGBA PNG. 최종 파일을 반복 배치했으며 검토 이미지 자체를 따로 수정하지 않았다.
- 세 반복 이미지를 직접 열어 육안 확인했다. 벽에는 반복 띠가 없고 홀의 판재는 이어진다. 주방 줄눈은 경계에서 정렬되며, 중간 검토의 미세 얼룩을 수정한 뒤 다시 확인했다.
- 지정 크기, RGBA, 타일 알파 255, 머신 알파 0/255, 사방 4px 여백과 하단 정렬, 타일 반대편 경계 일치를 실제 PNG 픽셀로 검사했다.
- 파일 용량: WallModern.png 395B, FloorHall.png 6,872B, FloorKitchen.png 523B, StationEspresso.png 5,504B. 검토 이미지의 9개 구획이 최종 타일 픽셀과 각각 일치하는 것도 확인했다.
- `bash .github/scripts/check-repo.sh`: 검사 통과 (파일 1353개). 작업 전후 `git status --porcelain` 결과가 동일하다.
- 생성 원본은 Sources/에 별도 보존했다. 원본은 최종 크기·알파 규격의 검수 대상이 아니며 Unity용 파일은 시안 폴더 바로 아래 4개 PNG다.
- 마음에 걸리는 점: 홀 바닥의 나뭇결은 넓은 맵에서 반복감이 남을 수 있다. 머신은 여백을 맞추기 위한 가로 비율 보정이 있으므로 실제 맵에서 크기감을 확인할 필요가 있다. 이번 단계는 시안이므로 Unity 임포트와 씬 연결은 하지 않았다.



## 모던 스타일 전체 맵 그림 (ADR-0016)

이 폴더의 맵 그림을 아래 기록대로 교체·추가했다. 스타일 기준은 StationEspresso.png 이다.

Generated with OpenAI built-in image_gen (내장 이미지 생성 도구). Style reference: `Assets/_Project/Art/Sprites/Cafe/StationEspresso.png`.

크기 참조: `Assets/_Project/Art/Sprites/Player/PlayerDown0.png` (128×192). 기존 같은 이름의 카페 그림은 기기 종류·구성 확인에만 사용했다. 기준 이미지와 기존 그림 콘택트 시트를 직접 열어 확인했다.

Prompt set: Use case: style-transfer. Create ONE production sprite for a Unity modern minimalist cafe map, using the reference espresso station ONLY as strict rendering style reference, not subject. Match its crisp stepped pixel edges, thin charcoal outline ~2px at 128px scale, broad clean shapes, 2-3 discrete flat shades per material, matte gray stainless steel, matte black, warm white and pale low-saturation natural oak. Sage green only accent except natural food colors. No gradients, blur, antialiasing, dither, grain noise, text, numerals, logos, ground shadows, floor, scenery or unrequested props. Orthographic front-facing three-quarter RPG view, horizontal front edges horizontal, verticals vertical, top visible, NOT isometric. Light upper-left. Truly transparent background. Design natively for requested aspect ratio, never stretch. Entire object visible, centered, 4px safe padding at final resolution, base at bottom padding. Restrained simple geometry like reference, readable at small final size. 

## 적용 범위

2026-11-05. 요청 목록 26개 PNG를 이 폴더에 저장했다. 저장소와 Assets는 읽기만 했다. ADR 변경 및 Unity 임포트·씬 연결은 하지 않았다. ART_STYLE 1장 맵 예외와 ADR-0016의 모던 스타일을 적용했다. 아래 파일별 지시가 공통 프롬프트와 충돌하면 파일별 지시가 우선한다.

## 파일별 프롬프트와 수정 사항

- **Wall.png (128×128)**: TILE EXCEPTION: fully opaque edge-to-edge warm white wall, no outline or padding. Flat FRONT view, no perspective. Bottom exactly 16/128 height dark charcoal baseboard. Otherwise plain warm white with no texture. Seamless horizontal repeat, left and right edges identical. Will stack two tiles high. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 생성 원본의 미세 명암을 제거해 웜 화이트 #F3F1E9, 하단 16px 차콜 #292B2A 두 블록으로 정리했다. 가로 경계를 일치시켰다. 세로 반복 대상은 아니며, 두 칸 쌓으면 가운데에도 굽도리가 나타난다.
- **Floor.png (128×128)**: TILE EXCEPTION: fully opaque edge-to-edge top-down seamless natural pale desaturated oak floor, not apricot, not orange. Four broad horizontal planks, extremely sparse quiet grain, very subtle seams, no outline, no padding. Repeat seamlessly horizontally and vertically, opposite edges match. No perspective. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 최근접 축소 후 원본에서 4색 팔레트를 추출해 디더링 없이 정리하고 가로·세로 반대편 경계를 일치시켰다. 옅은 나뭇결의 반복감은 남는다.
- **FloorKitchen.png (128×128)**: TILE EXCEPTION: fully opaque edge-to-edge seamless very light matte neutral gray kitchen floor, exactly TWO BY TWO LARGE square tiles, subtle thin pale gray grout. No outline, padding, gradient, speckles, bevel or perspective. Seamless horizontal and vertical repeat. Brighter than medium gray. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 최근접 축소 후 회색 2색(212/233)으로 정리했다. 원본의 큰 2×2 구성과 연한 줄눈을 유지하고 양방향 경계를 일치시켰다.
- **Counter.png (128×128)**: TILE EXCEPTION: fully opaque edge-to-edge square repeating SERVICE COUNTER texture, straight FRONT view. Upper 30/128 height a pale light oak countertop, lower remainder matte black flat front panel. Quiet flat shades, no decorative panels, no legs or objects. No outside outline or padding. Seamless horizontal repeat. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 생성 원본 가장자리의 불필요한 검은 테두리를 잘라 제거한 뒤 축소했다. 상판은 약 30px, 하부는 무광 차콜. 가로 경계를 일치시켰다.
- **Door.png (128×128)**: TILE EXCEPTION: fully opaque edge-to-edge square ONE SINGLE glass door leaf, charcoal frame touching all four edges, pale neutral glass with just 2 flat angular reflection shapes, ONE vertical dark handle on right side. NO second leaf. Horizontal repeat two copies form entrance. No exterior scenery, outline beyond frame, padding or transparency. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 생성된 문 바깥 흰 여백을 프레임까지 잘랐다. 한 짝·오른쪽 손잡이를 유지하고 가로 경계를 일치시켰다.
- **StationDishwash.png (192×192)**: Final 192x192 square. Floor-standing compact STAINLESS STEEL SINK CABINET, sink basin on left, curved faucet, drying rack with three white cups on right. Low matte black/stainless cabinet base. ONLY these objects. 실제 호출에 `Target canvas 192x192 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationShakeBar.png (160×192)**: Final 160x192 portrait. Narrow low matte black/stainless cabinet supporting ONE blender with transparent jug and THREE unlettered fruit puree pouches. Simple silhouettes, no other equipment or props. 실제 호출에 `Target canvas 160x192 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationIceMaker.png (128×192)**: Final 128x192 narrow portrait. Floor-standing stainless ice maker, rectangular front window showing several chunky white/pale gray ice cubes, small lower ventilation slots, short feet. No stand or other objects. 실제 호출에 `Target canvas 128x192 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationCupShelf.png (128×192)**: Final 128x192 narrow portrait. Tall slim matte black OPEN FRAME shelf, three shelves holding white ceramic cups and clear plastic cups. Clear simple silhouettes, no doors or wood. 실제 호출에 `Target canvas 128x192 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationDrinkBar.png (160×192)**: Final 160x192 portrait. Low matte black/stainless cabinet with ONE small digital scale, THREE syrup pump bottles, ONE milk carton, TWO tea boxes on top. No lettering. No blender, espresso machine, upper shelves, plants or extra props. 실제 호출에 `Target canvas 160x192 pixels.`를 덧붙였다.
  - 수정: 처음에는 너무 넓고 낮게 나와 재생성했다. 높은 장 버전에서 발생한 문짝의 투명 구멍은 image_gen 수정으로 제거했으나, 최종적으로 낮은 1문 캐비닛과 큰 병 구성으로 다시 생성했다. 앞선 버전은 Sources/의 First, Previous, Tall 접미사 파일에 보관했다.
- **StationWater.png (128×192)**: Final 128x192 narrow portrait. Compact warm white and stainless water purifier with TWO taps and drip tray, resting on low matte black pedestal stand. No cups, bottle or additional props. 실제 호출에 `Target canvas 128x192 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationTowel.png (128×128)**: Final 128x128 square. COUNTERTOP OBJECTS ONLY: small matte black rack with ONE neatly folded warm white cloth hanging over bar and ONE sage spray bottle beside it. No supporting counter, base cabinet, plants or extra accessories. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationDessertCase.png (128×128)**: Final 128x128 square. COUNTERTOP ONLY, compact small glass dessert display case with thin matte black frame, ONE slice of cream cake and a few cookies on two shelves. No supporting counter or cabinet. Glass in flat pale gray highlight blocks, no continuous gradients. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationPos.png (160×128)**: Final 160x128 landscape. COUNTERTOP OBJECTS ONLY: POS terminal, large completely blank dark screen in thin matte black frame on short stand, separate compact card reader on right. No text, numbers, counter, cabinet or support furniture. 실제 호출에 `Target canvas 160x128 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationPickup.png (608×128)**: Final 608x128 EXTREMELY WIDE AND LOW, content aspect ~5:1. A LONG LOW open PICKUP SHELF in pale oak with thin matte black uprights, three open bays spanning full width, ONE matte BLACK service bell at left and THREE EMPTY rectangular pickup trays distributed evenly across long upper surface. Keep all trays empty. Long connected continuous shelf, balanced full-width composition. No cabinet or supporting counter. Need this exact very wide native composition; do not draw a normal shelf then stretch. 실제 호출에 `Target canvas 608x128 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **StationTrayReturn.png (128×128)**: Final 128x128 square. COUNTERTOP ONLY compact matte black tray return open shelf, rectangular opening and neat stack of pale gray trays. No lettering, wall, cabinet or service counter underneath. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **TrayStand.png (128×472)**: Final 128x472 VERY TALL NARROW sprite. A long vertical floor footprint oriented up-down in map, a low narrow matte black stand that supports a tray-return shelf at its far upper end. Long low pale oak top recedes upward in orthographic top-down three-quarter map view, visible long side panels and short feet. Top-down footprint is very tall narrow, NOT a tall tower furniture. No trays or return shelf, just support stand. Native composition 1:3.69. 실제 호출에 `Target canvas 128x472 pixels.`를 덧붙였다.
  - 수정: 첫 결과의 원근 수렴이 커서, 평행한 세로 변의 위에서 보는 낮은 받침으로 재생성했다. 가로 폭을 억지로 늘리지 않아 좌우 여백은 16px이다.
- **BreadShelf.png (336×656)**: Final 336x656 tall portrait. TOP-DOWN VIEW exception: very long vertical rectangular bread display island, viewed from ABOVE, long axis straight vertical, narrow pale oak frame, two columns by FOUR rows of compartments. Compartments contain sliced bread loaves, croissants, donuts, muffins, each clearly identifiable, restrained warm natural food tones. Slim matte black lower base barely visible at bottom. No text, no price labels, no people, no floor, no chairs. Native 1:1.95 footprint. 실제 호출에 `Target canvas 336x656 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **PlanterSegment.png (128×128)**: Final 128x128 square. REPEATING VERTICAL SEGMENT exception. Top-down three-quarter view of one segment of long narrow matte black rectangular planter divider filled densely with sage green leaves. Planter AND foliage continue through BOTH TOP and BOTTOM image boundaries with NO END CAPS and NO vertical margins. Left and right 4px transparent padding only. Top and bottom edges must match exactly so three copies join vertically. No ground, no shadows. 실제 호출에 `Target canvas 128x128 pixels.`를 덧붙였다.
  - 수정: 첫 결과가 긴 완제품 화분으로 나와 정사각형·조밀한 잎·끝판 없음으로 재생성했다. 비율 유지 축소와 크롭 뒤, 생성된 중앙 64px 높이 조각 및 그 상하 반전 조각을 이어 세로 주기 경계를 만들었다. 좌우 4px, 상하 여백 없음. 반사 대칭에 따른 패턴 반복감은 남는다.
- **BarSegment.png (128×160)**: Final 128x160 portrait. REPEATING HORIZONTAL SEGMENT exception. Window bar table with pale oak top spanning FULL WIDTH edge to edge, thin matte black support/legs below. Continuous identical LEFT and RIGHT boundaries, NO endcaps, NO horizontal margins. Only upper 4px transparent allowed, legs reach bottom edge. No chairs, windows, objects, floor or shadow. Horizontal front edge horizontal, top visible. Will repeat horizontally. 실제 호출에 `Target canvas 128x160 pixels.`를 덧붙였다.
  - 수정: 첫 결과가 넓은 일반 테이블로 나와 중앙 검정 다리 하나를 가진 좁은 바 조각으로 재생성했다. 양 끝 모서리를 크롭하고, 비율 유지 축소 후 상판 끝 열을 가로로 연장해 연결을 정리했다. 위 4px 투명, 좌우·하단 여백 없음.
- **Stool.png (96×128)**: Final 96x128 narrow portrait. ONE small modern bar stool, simple sage green seat and matte black slim legs with footrest. No backrest, no extra props. At 1 tile scale, slim stepped outline. 실제 호출에 `Target canvas 96x128 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **Booth.png (336×256)**: Final 336x256 landscape. TWO-PERSON BOOTH intended against RIGHT wall. On LEFT one small empty pale oak table with matte black pedestal; on RIGHT ONE sage-green upholstered sofa for two, backrest along right edge. Both clearly visible, no wall, no floor, no dishes, no chairs or people. Camera front-facing three-quarter map view, not isometric. 실제 호출에 `Target canvas 336x256 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **Table.png (256×192)**: Final 256x192 landscape. ONE four-person SQUARE cafe table ALONE. Empty pale natural oak tabletop, black steel legs. Broad square top, front edge exactly horizontal, top visible in front-facing three-quarter RPG view. NO CHAIRS, no stools, no dishes, no decor, no shadows. 실제 호출에 `Target canvas 256x192 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **ChairLeft.png (96×128)**: Final 96x128 narrow portrait. ONE modern cafe chair FACING RIGHT, for placing LEFT of table. Backrest on LEFT side, seat and knees point RIGHT. Pale oak seat and backrest, thin matte black frame/legs. Obvious right-facing side-three-quarter view, not front-facing. No table or props. 실제 호출에 `Target canvas 96x128 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **TrayOnTable.png (160×112)**: Final 160x112 landscape. ONE used matte black rectangular tray with TWO EMPTY cups: one warm white ceramic mug and one empty clear plastic cup. No table, food, napkin, dishes or other props. Top-down front-facing three-quarter view matching table. All rims clearly empty, pale gray flat glass highlights. 실제 호출에 `Target canvas 160x112 pixels.`를 덧붙였다.
  - 수정: 생성 알파를 128에서 이진화하고 내용 영역을 크롭했다. 종횡비를 보존해 최근접 보간으로 축소한 뒤 하단 중앙에 배치했다. 색상 잡음을 줄이고 제한된 무채색·오크·세이지·음식색 팔레트로 디더링 없이 정리했다.
- **ChairRight.png (96×128)**: 최종 ChairLeft.png를 Pillow ImageOps.mirror로 정확히 좌우 반전했다. 별도 생성 호출은 없으며 명암도 함께 반전된다.

## 재생성 지시

- StationDrinkBar 최종 전체 프롬프트: Use case: style-transfer. Create ONE production sprite for a Unity modern minimalist cafe map, using the reference espresso station ONLY as strict rendering style reference, not subject. Match its crisp stepped pixel edges, thin charcoal outline ~2px at 128px scale, broad clean shapes, 2-3 discrete flat shades per material, matte gray stainless steel, matte black, warm white and pale low-saturation natural oak. Sage green only accent except natural food colors. No gradients, blur, antialiasing, dither, grain noise, text, numerals, logos, ground shadows, floor, scenery or unrequested props. Orthographic front-facing three-quarter RPG view, horizontal front edges horizontal, verticals vertical, top visible, NOT isometric. Light upper-left. Truly transparent background. Design natively for requested aspect ratio, never stretch. Entire object visible, centered, 4px safe padding at final resolution, base at bottom padding. Restrained simple geometry like reference, readable at small final size. ONE floor-standing DRINK PREPARATION STATION. Design final content bounding box 152px wide and184px high. Front-facing three-quarter orthographic. Cabinet is NARROW LOW SINGLE-DOOR matte black and stainless cabinet, width152px height85px, entirely in bottom half. Tall syrup bottles and milk carton occupy top99px. On top EXACTLY one compact digital scale, THREE narrow syrup pump bottles tightly grouped, ONE milk carton and TWO small tea boxes beside it. All blank, no text. Equipment height approximately equals cabinet height. NOT a tall wardrobe, NOT a wide two-door cabinet. No long empty metal door panels, no additional shelves. Pale oak only on thin top edge. Crisp flat 2-3tone clean pixel shapes. No texture noise or glow.
- StationDrinkBar 중간 수리: `Repair this exact sprite. The right lower stainless cabinet door has a large unintended dark transparent hole and cloudy halo. Replace the entire right cabinet door surface with solid opaque uniform light gray metal with one simple flat diagonal shadow block. No hole, no glow, no noisy pixels. Keep all equipment, proportions, outline, composition unchanged. Transparent background outside object only. No transparency within cabinet. Crisp stepped pixel art.`
- TrayStand 재생성 추가: `Top-down camera, sides of very long footprint parallel vertical, NOT tapering to narrow distant end. Low floor stand seen mostly from above, not tall tower. Footprint nearly FOUR times longer than width.`
- PlanterSegment 재생성 추가: `ONE SQUARE TILE of dense leaves viewed from directly ABOVE, fill almost ENTIRE square width with planter and leaves, not a tall planter portrait. Frame sides exactly at x4 and x123. No front panel, no endcaps, no blank empty center, foliage fills whole tile. Seamless top-bottom.`
- BarSegment 재생성 추가: `ONE NARROW TALL TILE, content 128 wide x156 tall, NOT wide table. A continuous horizontal wooden bar top spans tile at y4..45 then one narrow black support centered below extending to bottom, both outer edges below tabletop transparent. No corner taper at left/right, no side/end edges. Simple infinite bar tabletop horizontal strip with single leg. No extra horizontal bottom rail.`

## 후처리와 검사

- 생성 전 원본은 Sources/에 보존했다. 일반 오브젝트는 늘리거나 찌그러뜨리지 않았다. 동일 배율 최근접 축소 뒤 투명 캔버스에 배치했다. 반복 조각은 위에 기록한 크롭·주기 연결 처리를 사용했다.
- 최종 26개 모두 지정 크기, RGBA, 알파 0/255를 실제 PNG에서 검사했다. 타일 5개는 알파 255이다. 투명 픽셀 RGB는 0이다.
- 일반 오브젝트 19개 모두 하단 정확히 4px, 나머지 방향 최소 4px 여백이다. **사방 정확히 4px 조건은 충족하지 못했다.** 아래 표는 실제 값이며, 비율 보존을 우선해 남은 공간을 투명하게 두었다.
- 타일 5개와 반복 조각 2개의 지정 방향 반대편 경계 픽셀이 완전히 같다. CheckWall/Floor/FloorKitchen/Counter/Door/PlanterSegment/BarSegment.png를 직접 열어 확인했다. 화분 끝단과 바 상판 연결부의 초기 자국을 수정한 뒤 재검수했다.
- ChairRight의 모든 RGBA 픽셀이 ChairLeft의 좌우 반전과 일치한다.
- Sheet.png는 1:1 크기로 모든 26개와 참조 에스프레소·플레이어를 모았다. 원본과 최종 시트를 직접 확인했고 음료대 및 반복 조각을 재생성했다.
- `bash .github/scripts/check-repo.sh`: 통과 (파일 1353개). 작업 전후 `git status --porcelain` 바이트 일치. 기존 사용자 변경은 그대로 두었다.
- 세부 기계 검사: Validation.json. 재현 스크립트와 프롬프트 목록: Sources/Process.py, Sources/Jobs.json.

## 남은 주의점

파일 누락은 없다. 위의 정확한 4px 여백 조건은 미충족이다. 벽은 요청대로 각 타일 하단에 굽도리가 있어 두 칸 높이 적층 시 중간 띠가 생긴다. 바닥과 화분은 넓게 배치하면 반복 무늬가 보일 수 있다. 빵 진열대는 다른 단순 기기보다 세부 묘사가 많고, 일부 금속 면에는 작은 픽셀 명암 덩어리가 남아 있다. 이 폴더 결과는 Unity에 연결하지 않았으므로 실제 씬의 점유 폭·물리 충돌·앉는 위치는 검증하지 않았다.

## 실제 여백 (왼쪽 / 위 / 오른쪽 / 아래, px)

| 파일 | 여백 |
|---|---|

| StationDishwash.png | 4 / 35 / 4 / 4 |
| StationShakeBar.png | 4 / 41 / 4 / 4 |
| StationIceMaker.png | 8 / 4 / 9 / 4 |
| StationCupShelf.png | 20 / 4 / 21 / 4 |
| StationDrinkBar.png | 13 / 4 / 14 / 4 |
| StationWater.png | 7 / 4 / 8 / 4 |
| StationTowel.png | 4 / 47 / 4 / 4 |
| StationDessertCase.png | 6 / 4 / 6 / 4 |
| StationPos.png | 4 / 26 / 4 / 4 |
| StationPickup.png | 4 / 9 / 4 / 4 |
| StationTrayReturn.png | 4 / 40 / 4 / 4 |
| TrayStand.png | 16 / 4 / 16 / 4 |
| BreadShelf.png | 46 / 4 / 46 / 4 |
| Stool.png | 9 / 4 / 10 / 4 |
| Booth.png | 4 / 37 / 4 / 4 |
| Table.png | 10 / 4 / 11 / 4 |
| ChairLeft.png | 4 / 11 / 4 / 4 |
| TrayOnTable.png | 4 / 31 / 4 / 4 |
| ChairRight.png | 4 / 11 / 4 / 4 |
- **WallPlain.png (128×128)**: 이미지 생성 없이 Pillow로 Wall.png의 벽 색 #F3F1E9(RGBA 243, 241, 233, 255)을 그대로 채운 굽도리 없는 민무늬 타일. 위쪽 벽 윗줄·옆벽용이며 저장 PNG의 128×128 RGBA, 전체 알파 255, 좌우·상하 경계 일치를 실제 검사함.


## 창가 바, 화분, 카운터 수정 (ADR-0016)

Generated with OpenAI built-in image_gen (내장 이미지 생성 도구).

스타일 기준: `C:/Users/k9425/Desktop/cafe-time/Assets/_Project/Art/Sprites/Cafe/StationEspresso.png`.
문제 확인: `../style_v2_preview_game.png`.
기존 공통 프롬프트: `../style_v2/Generation.md`.

2026-11-05. 요청된 3종만 다시 생성했다. 결과와 생성 원본은 이 style_v3 폴더에 저장했으며 저장소 파일, Unity 씬, 에셋 및 ADR은 변경하지 않았다.

## 공통 프롬프트

Use case: style-transfer. Create ONE production pixel art sprite for a Unity modern minimalist cafe map. Use the reference espresso station ONLY as strict rendering style reference, not subject. Match crisp stepped pixel edges, thin charcoal outlines about 2px at final 128px scale, broad clean shapes, 2-3 discrete flat shades per material, pale low-saturation natural oak, matte charcoal, warm white, sage green. No gradients, blur, antialiasing, dither, grain noise, text, numerals, logos, ground shadows, floor, scenery, props. Orthographic front-facing three-quarter RPG view, horizontal edges horizontal, verticals vertical, top visible, NOT isometric. Light upper-left. Design natively for requested aspect ratio. File-specific geometry overrides all generic padding. 

## 파일별 실제 최종 프롬프트

### BarSegment.png

Use case: style-transfer. Create ONE production pixel art sprite for a Unity modern minimalist cafe map. Use the reference espresso station ONLY as strict rendering style reference, not subject. Match crisp stepped pixel edges, thin charcoal outlines about 2px at final 128px scale, broad clean shapes, 2-3 discrete flat shades per material, pale low-saturation natural oak, matte charcoal, warm white, sage green. No gradients, blur, antialiasing, dither, grain noise, text, numerals, logos, ground shadows, floor, scenery, props. Orthographic front-facing three-quarter RPG view, horizontal edges horizontal, verticals vertical, top visible, NOT isometric. Light upper-left. Design natively for requested aspect ratio. File-specific geometry overrides all generic padding. BarSegment.png final target 128x160 portrait. OPAQUE rendering. ONE TILE ONLY of an infinitely long WINDOW BAR TABLE, seamless horizontal repeat. ENTIRE canvas is closed furniture surfaces edge to edge, no background or outside padding. TOP y0..55 (56 of 160px) is a wide pale oak tabletop seen from above. y56..63 is solid oak front lip thickness. y64..159 is a closed continuous pale oak modestly shaded apron panel extending all way to bottom. NO legs, NO feet, NO open space below, NO dividers, NO vertical seams, NO side outline, NO endcaps. Exactly horizontal top/front boundary. Not a complete freestanding cabinet. Quiet broad solid oak planes. NO dramatic lighting or diagonal shadows, absolutely NO gradient: only TWO or THREE FLAT solid colors for oak and a charcoal line. Sparse small wood marks optional. Entire image opaque. Left and right edges join seamlessly. Pixel art not a realistic render.

### PlanterSegment.png

Use case: style-transfer. Create ONE production pixel art sprite for a Unity modern minimalist cafe map. Use the reference espresso station ONLY as strict rendering style reference, not subject. Match crisp stepped pixel edges, thin charcoal outlines about 2px at final 128px scale, broad clean shapes, 2-3 discrete flat shades per material, pale low-saturation natural oak, matte charcoal, warm white, sage green. No gradients, blur, antialiasing, dither, grain noise, text, numerals, logos, ground shadows, floor, scenery, props. Orthographic front-facing three-quarter RPG view, horizontal edges horizontal, verticals vertical, top visible, NOT isometric. Light upper-left. Design natively for requested aspect ratio. File-specific geometry overrides all generic padding. PlanterSegment.png final 128x128. ONE vertically repeating segment of a long planter divider viewed obliquely from above. OPAQUE rendering on plain white canvas (white is padding only). The planter runs vertically across BOTH top and bottom boundaries with NO end caps, no top or bottom margin. At final scale white columns x0..3 and124..127 only, continuous matte black planter side WALLS x4..19 and108..123 (16px each), with a narrow top-facing rim and slightly shaded inner face clearly showing depth. Between walls, a few LARGE broad overlapping sage green leaf clusters in THREE discrete FLAT shades, chunky simple leaves not tiny scattered leaf texture. Leaves have clear light upper surfaces and dark overlapping undersides. Planter should read as solid rectangular raised trough with plants inside, not leaf wallpaper. 2-3 shades only, no texture noise. Top and bottom continue seamlessly. Single square tile only. No front end panel, no floor or shadows.

### Counter.png

Use case: style-transfer. Create ONE production pixel art sprite for a Unity modern minimalist cafe map. Use the reference espresso station ONLY as strict rendering style reference, not subject. Match crisp stepped pixel edges, thin charcoal outlines about 2px at final 128px scale, broad clean shapes, 2-3 discrete flat shades per material, pale low-saturation natural oak, matte charcoal, warm white, sage green. No gradients, blur, antialiasing, dither, grain noise, text, numerals, logos, ground shadows, floor, scenery, props. Orthographic front-facing three-quarter RPG view, horizontal edges horizontal, verticals vertical, top visible, NOT isometric. Light upper-left. Design natively for requested aspect ratio. File-specific geometry overrides all generic padding. Counter.png final target 128x128 square. ONE seamless horizontally repeatable service counter TILE. Fully OPAQUE edge to edge with NO background, NO margins, NO side edges or endcaps. Entire image is furniture surface. Upper exactly 36/128 of image: broad pale natural oak countertop viewed from above with visible thin front lip. Lower portion: LIGHT PALE OAK front facade with subtle vertical wooden panels, broad panels with very thin low contrast seams, NO black front band. Only bottom 6/128 is a thin charcoal toe kick. Very sparse wood marks, quiet clean oak, not orange, not dark brown. Straight horizontal top edge at canvas top, edge-to-edge across full width. No furniture legs, no handles, no doors, no objects. Left and right image edges match invisibly. Do not draw an isolated cabinet: draw only continuous repeatable surface.

## 생성 및 규격 처리

- 각 에셋을 별도 image_gen 호출로 생성했다. 모든 호출에 StationEspresso.png를 referenced_image_paths로 전달했다.
- 최초 BarSegment는 transparent_background=true로 생성했으나 내부 투명 구멍이 생겨 폐기했다. Sources/BarSegmentRejected.png와 Sources/Prompts.json에 이력과 최초 프롬프트를 보관했다. 최종 3종 원본은 불투명으로 생성하고, 사용자 지정 직사각형 여백만 최종 PNG에서 투명 처리했다.
- Sources/의 PNG는 생성 원본 그대로다. Sources/Process.py는 원본을 보존하며 최근접 보간으로만 축소한다. 디더링 없이 오크 3색과 차콜, 세이지 그린 3색과 화분 벽 명암으로 제한해 생성 원본의 미세 그라데이션을 정리했다. 투명 픽셀 RGB는 0이다.
- BarSegment: 생성 원본의 상판·앞면 두께·닫힌 하부 패널을 구간별로 맞췄다. 최종 상판 y4~59는 56px, 앞면 두께 y60~67은 8px, 닫힌 오크 하부는 y68~159다. 다리와 세로 칸 구분이 없다. 여백은 위 4px, 좌우·아래 0px.
- PlanterSegment: 원본 좌우 흰 여백을 잘라 큰 잎과 연속 화분 벽을 사용했다. 밝은 원본 테두리는 차콜로 정리했다. 벽과 테두리 영역은 양쪽 약 16px이며 큰 잎이 안쪽에 겹친다. 좌우 4px는 투명, 위아래 0px. 경계 근처 6px 범위에서 상하를 기존 팔레트로 연결했다. 상하 반사 반복은 사용하지 않았다.
- Counter: 위쪽 흰 여백을 자르고 상판을 36px, 오크 세로 패널을 86px, 얇은 차콜 걸레받이를 6px로 맞췄다. 전체 불투명이다.
- 가로 반복 2종은 끝 열을 시작 열에 맞췄다. 화분은 시작·끝 행이 일치한다. 이미지 안에 글자·숫자·로고는 없다.

## 실제 PNG 검사 및 시각 확인

| 파일 | 크기 | 알파 값 | 내용 경계 (left, top, right, bottom) | 반복 |
|---|---|---|---|---|
| BarSegment.png | 128×160 | 0, 255 | 0, 4, 128, 160 | 가로 |
| PlanterSegment.png | 128×128 | 0, 255 | 4, 0, 124, 128 | 세로 |
| Counter.png | 128×128 | 255 | 0, 0, 128, 128 | 가로 |

저장된 PNG를 다시 읽어 크기·RGBA 모드·알파·여백·반복 방향의 양끝 RGBA 일치를 검사했다. 상세 결과는 Validation.json에 있다.

- CheckBarSegment.png: 가로 5회 반복, 640×160. 직접 열어 상판과 막힌 하부의 연결을 확인했다. 기둥·울타리 형태는 없다.
- CheckPlanterSegment.png: 세로 3회 반복, 128×384. 직접 열어 잎과 벽이 상하에 이어지는지 확인했다. 큰 잎 배치의 주기적 반복은 보인다.
- CheckCounter.png: 가로 5회 반복, 640×128. 직접 열어 상판·밝은 앞판·얇은 걸레받이의 연결을 확인했다. 세로 패널 구분선은 의도된 무늬다.

확인 이미지는 알파를 밝은 중성 배경 위에 합성한 PNG이며 원본 에셋의 알파에는 영향을 주지 않는다. Unity 임포트나 실제 씬 배치는 수행하지 않았다. Point (no filter), mipmap 끔으로 사용하는 기존 규격을 유지한다.


## 벽 재작업 (ADR-0016)

# 벽 그림 v4 생성 기록

- 도구: OpenAI 내장 `image_gen` (5개 파일별 개별 호출). CLI/API는 사용하지 않았다.
- 날짜: 2026-11-05.
- 스타일 참조: `Assets/_Project/Art/Sprites/Cafe/StationEspresso.png`를 직접 열어 확인했다. 창문 호출에는 이미지 파일도 첨부했다. 벽 호출에는 확인한 스타일을 텍스트로 지정했다.
- 구조 참조: `../style_v2_preview_game.png`.
- 공통 프롬프트 참조: `../style_v2/Generation.md`. 모던 미니멀 도트 스타일을 유지하되, 이번 요청의 벽 색·불투명·여백 규격을 우선했다.
- 저장소 파일은 변경하지 않았고 `style_v3`는 읽거나 쓰지 않았다. 이번 결과는 임시 시안이며 ADR 변경과 Unity 연결은 하지 않았다.

## 파일별 실제 프롬프트

### WallPlain.png

```text
Use case: stylized-concept. Production Unity modern minimalist cafe map tile. Match the provided espresso machine style reference only in rendering: crisp restrained pixel art, broad clean flat shapes, discrete matte colors, no gradients, blur, antialiasing, grain, dithering, text, numerals, logo, shadow or props. Tile exception overrides reference object outline and padding: flat straight-on surface, no perspective, no outline, no margins, fully opaque edge-to-edge. Target final canvas 128x128 pixels. WallPlain.png: a uniform quiet warm greige painted wall face, near #D9D4C8. NOT WHITE. Almost single solid color, absolutely no texture, no trim on any edge, no framing. Horizontally seamless, all four edges same greige. This is the UPPER row of a two-row back wall.
```

### Wall.png

```text
Use case: stylized-concept. Production Unity modern minimalist cafe map tile. Match the provided espresso machine style reference only in rendering: crisp restrained pixel art, broad clean flat shapes, discrete matte colors, no gradients, blur, antialiasing, grain, dithering, text, numerals, logo, shadow or props. Tile exception overrides reference object outline and padding: flat straight-on surface, no perspective, no outline, no margins, fully opaque edge-to-edge. Target final canvas 128x128 pixels. Style reference: the espresso sprite previously shown. Wall.png: LOWER hall back-wall row. Upper 118/128 of image is absolutely uniform warm greige #D9D4C8 matching upper wall. Bottom exactly 10/128 image height is one continuous pale low-saturation natural oak baseboard, light tan #CAB58F, no black. It runs flush from left to right edge. Flat two-color game texture, no outline, no vertical joints, no wood grain, no bevel, no highlights. Horizontal repeat left and right edges identical. No trim at top.
```

### WallKitchen.png

```text
Use case: stylized-concept. Production Unity modern minimalist cafe map tile. Match the provided espresso machine style reference only in rendering: crisp restrained pixel art, broad clean flat shapes, discrete matte colors, no gradients, blur, antialiasing, grain, dithering, text, numerals, logo, shadow or props. Tile exception overrides reference object outline and padding: flat straight-on surface, no perspective, no outline, no margins, fully opaque edge-to-edge. Target final canvas 128x128 pixels. WallKitchen.png: LOWER kitchen back-wall row. Near-white matte subway ceramic rectangular tiles with very light gray thin grout. Exactly four equal horizontal courses, each course 32 final pixels high; bricks 64 final pixels wide, offset by 32 final pixels on alternating rows. Straight flat front face. Thin continuous warm charcoal #45413B finishing strip only along top edge, 2 final pixels high. NO dark baseboard at bottom. NO shadows, bevels, gloss or 3D tiles. Horizontally seamless repeating tile, left/right boundary pixel profiles exactly match. Clean large minimal tile pattern. Upper wall above is warm greige #D9D4C8; this tile begins under it.
```

### WallTop.png

```text
Use case: stylized-concept. Production Unity modern minimalist cafe map tile. Match the provided espresso machine style reference only in rendering: crisp restrained pixel art, broad clean flat shapes, discrete matte colors, no gradients, blur, antialiasing, grain, dithering, text, numerals, logo, shadow or props. Tile exception overrides reference object outline and padding: flat straight-on surface, no perspective, no outline, no margins, fully opaque edge-to-edge. Target final canvas 128x128 pixels. WallTop.png: TOP SURFACE ONLY of cafe side walls, bottom wall and kitchen partition. Entire canvas a completely solid DARK WARM CHARCOAL #45413B. Uniform flat color all pixels, no decorations or features. NO wall FRONT face, NO top highlight, NO edge lines, NO border, NO white, NO baseboard, NO noise or lighting. Must repeat seamlessly horizontally AND vertically without visible seams. Pure flat dark charcoal material swatch fills whole square.
```

### WindowHall.png

```text
Use case: style-transfer. Create ONE WindowHall.png production sprite for a Unity modern minimalist cafe map. Use the espresso machine reference as strict rendering style reference ONLY, not subject: crisp stepped pixel edges, broad simple shapes, 2-3 discrete flat shades per material, restrained desaturated palette. ONE horizontal rectangular wall window in straight front elevation, precisely TWO panes split by ONE thin vertical charcoal mullion at center. Thin matte warm charcoal outer frame. Through glass: pale muted blue sky in upper area and quiet sage green leafy tree silhouettes in lower area, simple block shapes, no other scenery. Entire window opaque including panes; ONLY outside frame truly TRANSPARENT alpha 0. Target final canvas 256x160, frame bounding box x4..251 y4..155, exact FOUR final-pixel clear margin on all four sides. Wide rectangular silhouette 248:152, no perspective, no sill protrusions, no bevel, NO SHADOW, NO text, logo, grid, extra mullions, curtains or wall. No gradients, antialiasing, blur, grain, dither or semitransparent pixels. Pixel game art matching reference, no thick black outline. Design for final 256x160 ratio.
```

## 최종 규격 정리

생성 원본은 `Sources/*.png`, 실제 프롬프트와 원본 경로는 `Sources/Jobs.json`, 정리·합성·검사 스크립트는 `Sources/Process.py`에 보존했다. 다음 처리는 Pillow로 수행했다.

- WallPlain: 생성된 거의 단색인 벽 면의 미세한 색 흔들림을 #D9D4C8로 통일. 128×128, 외곽선·여백 없음.
- Wall: 위 118px은 WallPlain과 동일한 #D9D4C8, 하단 10px 오크 굽도리는 #CAB58F로 통일. 검은 굽도리 없음.
- WallKitchen: 생성한 4줄 엇갈린 서브웨이 타일 모양을 최근접 축소하고 무광 타일 #F5F3EF / 줄눈 #E0DED8 / 마감선 #45413B로 정리. 위 마감선은 2px. 좌우 경계 픽셀을 동일하게 맞춤.
- WallTop: 생성된 짙은 윗면을 #45413B 단색으로 통일. 가로·세로 반복 가능.
- WindowHall: 생성 알파를 128 기준으로 이진화해 프레임 경계까지 크롭하고 248×152로 규격 조정한 뒤 256×160 투명 캔버스의 (4,4)에 배치. 생성본의 창 가로세로 비율은 최종 프레임 규격에 맞게 조정했다. 프레임·하늘·나무의 5색 팔레트로 정리해 그라데이션 얼룩 제거. 프레임 내부는 불투명, 프레임 외부는 알파 0. 그림자 없음.

## 합성 및 실제 검사

`CheckWall.png`는 1536×384, 12칸×3줄이며 한 칸은 128px이다. 양 끝 1칸씩은 세 줄 모두 WallTop, 안쪽 왼쪽 5칸은 주방, 오른쪽 5칸은 홀이다. 첫 줄은 WallPlain, 둘째 줄은 각각 WallKitchen/Wall, 셋째 줄은 `../style_v2/FloorKitchen.png`와 `../style_v2/Floor.png` 원본을 가져왔다. 홀 벽 (960,40)에 WindowHall을 합성했다. 정면 벽과 옆벽 윗면, 주방·홀 바닥 경계, 창문을 실제 열어 눈으로 확인했다.

`Validation.json`에 실제 PNG 검사 결과를 기록했다.

- 벽 4종 모두 128×128 RGBA, 전체 알파 255.
- 창문 256×160 RGBA, 알파 값은 0/255만 존재, 사방 여백 정확히 4px.
- 벽 4종 좌우 경계 일치, WallTop 상하 경계 일치.
- WallPlain 하단과 Wall 상단 색 일치. 주방의 경계는 의도한 2px 차콜 마감선으로 구분.
- CheckWall 1536×384 RGBA, 전체 알파 255.

Unity 임포트와 게임 내 배치는 이번 작업 범위에 포함하지 않았다.

저장소 공통 검사: `bash .github/scripts/check-repo.sh` 통과 (파일 1353개).

