Generated with OpenAI 내장 image_gen 이미지 생성 도구. Style reference: Assets/_Project/Art/UI/Quit/{Panel,Yes,No,Message}.png; Assets/_Project/Art/UI/Settings/{Panel,Close}.png; Assets/_Project/Art/UI/OpenPrep/{CatalogPanel,CartPanel,TabNormal,TabSelected,CardFrame,BuyButton}.png; Assets/_Project/Art/UI/Settlement/Receipt.png; Assets/_Project/Art/UI/Nickname/Panel.png.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front view, no mockup. Reference the existing game sprites shown in conversation: Quit/Panel, Yes, No, Message; Settings/Panel, Close; OpenPrep/CatalogPanel, CartPanel, TabNormal, TabSelected, CardFrame, BuyButton; Settlement/Receipt; Nickname/Panel. Palette cream #FAEAC2 #FCF0CA #F7DCA5, honey #DF7015 #C57D1F #F1C97D, walnut #90471B #5D2608, dark coffee #380F00. Crisp pixel steps and 2-3 shade blocks. Genuine transparent background, no checkerboard painted in, no watermark. 

파일별 프롬프트는 위 공통 프롬프트에 아래 내용을 덧붙여 개별 호출했다. PNG의 그림과 글자는 내장 도구로 생성했다. 저장 시 최근접 리샘플링으로 크기를 맞추고, 투명 영역을 잘라 사방 8px 여백을 맞췄다. 라벨 글자 높이는 크림 외곽선을 제외한 채움 영역의 세로 범위를 기준으로 측정한다. 픽업 안내는 각 행을 따로 크기 조정한 뒤 중앙 정렬하며 행 사이 간격은 16px이다.

- DessertPanel.png: 2000×1200px. Rectangular walnut panel with thin honey gold inner trim and fully opaque empty cream interior. Painted bold dark coffee pixel title "디저트 진열장" centered at top INSIDE frame, flanked by small coffee beans like Nickname/Panel. Spell 디 저 트 [space] 진 열 장. Rest completely EMPTY. 사방 균일한 투명 여백, 패널 내부 완전 불투명, 프레임 위 돌출물 없음.
- PickupPanel.png: 2200×1280px. Rectangular walnut panel with thin honey gold inner trim and fully opaque empty cream interior. Painted bold dark coffee pixel title "픽업대" centered at top INSIDE frame, flanked by small coffee beans like Nickname/Panel. Spell 픽 업 대. Rest completely EMPTY. 사방 균일한 투명 여백, 패널 내부 완전 불투명, 프레임 위 돌출물 없음.
- PickupSlot.png: 360×360px. Square wooden tray slot, beveled walnut rim, gold inner trim, fully opaque cream inset inside, completely empty; no text, no objects. 사방 균일한 투명 여백, 패널 내부 완전 불투명, 프레임 위 돌출물 없음.
- PickupHint.png: OpenAI 내장 image_gen으로 둘째 줄 생성. 스타일 참조: 기존 Assets/_Project/Art/UI/Stations/PickupHint.png. 텍스트 전용 투명 라벨. 정확한 문구 "음료·디저트를 끌어다 놓고\n주문 내역에서 영수증을 눌러 배정하세요". 파일별 프롬프트: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: text-localization. Reference image is the existing PickupHint.png, only a typography style reference. Generate ONLY a SINGLE horizontal line of Korean text, exactly "주문 내역에서 영수증을 눌러 배정하세요". Character sequence: 주 문 [space] 내 역 에 서 [space] 영 수 증 을 [space] 눌 러 [space] 배 정 하 세 요. Spell every syllable exactly. Match the reference very bold square block pixel Hangul, dark coffee #380F00 solid fill, cream #FCF0CA thin 3px outline for a target glyph fill height of 56px. Make all glyphs equally tall and aligned. Wide horizontal composition with the entire text on ONE line, never wrap. Transparent exterior and transparent letter counters. No panel, icon, ornament, shadow, texture, gradients, extra text, or speckles. This is only the replacement second line; do not reproduce the reference first line. Keep the bold square Hangul shapes closely matched to the reference. 후처리: Python 3 + Pillow로 원본 첫 줄의 RGBA 픽셀을 그대로 재사용했다. 생성한 둘째 줄의 채움 마스크를 추출하고 최근접 리샘플링으로 높이 56px에 맞춘 뒤 #380F00 단색 채움과 #FCF0CA 3px 외곽선으로 정규화했다. 둘째 줄 글자 내부 구멍과 외부 배경은 알파 0으로 처리했다. 두 줄 중앙 정렬, 원본의 행 사이 투명 간격 16px, 사방 투명 여백 8px로 합성했다. 최종 925×160px. "주 문 [공백] 내 역 에 서 [공백] 영 수 증 을 [공백] 눌 러 [공백] 배 정 하 세 요"를 시각 검수했다.
- 2026-11-10 교체: 위 `PickupHint.png`의 기존 안내를 손 칸 선택 → 트레이 배치 → 서빙 방식의 두 줄 안내로 교체했다. 새 문구·규격·후처리는 문서 끝의 「2026-11-10 픽업대 트레이」 절을 참고한다.
- LabelUnassigned.png: 텍스트 전용 투명 라벨. 정확한 문구 "미배정". 한 줄. 굵은 도트 글자, 경고색 #A3261B 채움, 크림 #FCF0CA 외곽선 3px. 글자 높이 56px, 사방 투명 여백 8px. 패널·버튼·장식 없음.
- DrinkBarPanel.png: 2600×1520px. Rectangular walnut panel with thin honey gold inner trim and fully opaque empty cream interior. Painted bold dark coffee pixel title "음료 제조실" centered at top INSIDE frame, flanked by small coffee beans like Nickname/Panel. Spell 음 료 [space] 제 조 실. Rest completely EMPTY. 사방 균일한 투명 여백, 패널 내부 완전 불투명, 프레임 위 돌출물 없음.
- ShakeBarPanel.png: 2600×1520px. Rectangular walnut panel with thin honey gold inner trim and fully opaque empty cream interior. Painted bold dark coffee pixel title "쉐이크 제조실" centered at top INSIDE frame, flanked by small coffee beans like Nickname/Panel. Spell 쉐 이 크 [space] 제 조 실. Rest completely EMPTY. 사방 균일한 투명 여백, 패널 내부 완전 불투명, 프레임 위 돌출물 없음.
- TempMakerHint.png: 텍스트 전용 투명 라벨. 정확한 문구 "(임시) 메뉴를 누르면 완성된 음료를 손에 듭니다". 한 줄. 굵은 도트 글자, 다크 커피 #380F00 채움, 크림 #FCF0CA 외곽선 3px. 글자 높이 56px, 사방 투명 여백 8px. 패널·버튼·장식 없음.

Unity 임포트: Texture Type Sprite (2D and UI), Sprite Mode Single, Filter Mode Point (no filter), Generate Mip Maps 끔, Alpha Is Transparency 켬. PNG는 표시 크기의 2배이고, 라벨은 원본의 절반 크기로 표시한다. 가로 또는 세로가 2048px를 넘는 파일은 요청한 원본 크기를 유지하도록 Max Size 4096을 사용한다. 그 외 파일은 Max Size 2048을 사용한다. `.meta`는 직접 만들지 않았으며 Unity 임포트가 필요하다.

픽업대는 SFR-056 대응 아트 에셋. 제조실 패널은 요청된 임시 음료 제조 화면용이며 값이나 메뉴 이름을 이미지에 포함하지 않는다.

최종 수정·검수:
- DessertPanel.png, PickupPanel.png, ShakeBarPanel.png: 내장 도구의 precise-object-edit로 투명 구멍과 어두운 잔상을 크림 배경으로 복구했다. 기존 한글 제목·커피콩·호두나무 프레임·금색 선을 보존했다. 최종 프롬프트 핵심: Replace all interior holes and dark fuzzy fringes with fully opaque cream #FAEAC2; preserve existing title, beans, walnut frame and gold trim; transparency only outside perimeter.
- DrinkBarPanel.png: 복구된 PickupPanel.png를 참조하여 내장 도구로 제목만 픽업대에서 정확한 음료 제조실로 변경했다. 불투명 크림 내부와 프레임·커피콩을 보존했다.
- PickupSlot.png: 내장 도구로 정사각형 나무 슬롯을 파란 외부 배경의 불투명 중간 이미지로 생성한 뒤, 같은 내장 도구의 background-extraction으로 외부 파랑만 제거했다. 최종 프롬프트 핵심: Remove ONLY saturated blue #0000FF exterior; preserve all cream interior pixels and walnut rim exactly; no internal transparency. 중간 파랑은 최종 PNG에 포함하지 않는다.
- PNG 저장 규격 보정: 최근접 크기 조정, 사방 투명 여백 8px, 프레임으로 둘러싸인 내부 알파 255 정규화. RGB 그림과 한글 글자 형태는 내장 생성·편집 도구 결과를 사용했다.
- 한글·가운뎃점·괄호·두 줄 구성을 시각 검수했다. 라벨 채움 영역 높이는 각 행 56px이며 크림 외곽선은 측정에서 제외했다.

- Preparing.png: OpenAI 내장 image_gen으로 생성. 스타일 참조: `Assets/_Project/Art/UI/Stations/LabelUnassigned.png`, `Assets/_Project/Art/UI/Pos/LabelOrderNo.png`, `Assets/_Project/Art/UI/Quit/Message.png`. 파일별 프롬프트: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE text-only LABEL sprite. EXACT Korean text "기기 화면 준비 중", SINGLE horizontal line. Character sequence: 기 기 [space] 화 면 [space] 준 비 [space] 중. Preserve exactly three spaces. Match the reference bold stepped pixel Hangul lettering; dark coffee #380F00 fill, cream #FCF0CA outline 3px. LABEL M: every Korean glyph fill height exactly 72px excluding outline. Genuine transparent RGBA background, exactly 8px transparent padding on all sides. No panels, buttons, ornaments, shadows, or other text. 후속 편집 프롬프트: Preserve the exact text and bold pixel Hangul shapes; remove stray pixels, speckles, colored fringes, texture and gradients; use only flat solid dark coffee #380F00 fill and a clean cream #FCF0CA outline, transparent elsewhere. 저장 규격 보정: 생성된 한글 채움 형태를 최근접 방식으로 글자별 72px 높이에 맞추고, 채움색을 #380F00으로 정규화한 뒤 #FCF0CA 외곽선 3px와 사방 투명 여백 8px를 적용했다. 최종 548×94px. "기기 화면 준비 중"의 일곱 글자와 세 공백을 시각 검수했다. `.meta`는 생성하지 않았다.

## 삭제됨 (2026-11-04)

- DrinkBarPanel.png, ShakeBarPanel.png, TempMakerHint.png, Preparing.png: 임시 제조실·준비 중 창을 실제 기기 화면(ADR-0014)으로 교체하면서 삭제했다. 위 기록은 이력으로 남긴다.

## 2026-11-09 자연스러운 일러스트 닫기 버튼

- 도구: OpenAI 내장 `image_gen.imagegen`으로 원화를 생성하고, 프로젝트 밖 PowerShell `System.Drawing`으로 투명 원화를 128×128에 고품질 축소했다.
- 스타일 참조: `../ShotRoom/Panel.png`.
- 공통 프롬프트: “Premium hand-painted 2D illustration for a cozy Korean cafe game. Clean charcoal contours, tactile neutral brushed stainless steel, matte black handles, light oak, warm greige wall, restrained sage accents. Soft upper-left light, crisp highlights and coherent material rendering. No photographic finish, no vector-like flat shapes, no text inside AI art.” 도트 격자·계단 외곽선은 적용하지 않았다.
- `Close.png` (128×128): “Small round muted sage enamel close button with dark beveled side, subtle cream upper-left highlight and a centered crisp dark charcoal X. No other markings. Transparent isolated.” 원화 `exec-b40fb793-96eb-47f2-a1c4-a857afa0c4fb.png`의 버튼 부분을 118×118로 줄여 사방 5px 투명 여백에 배치했다.
- 현재 빌더는 `Settings/Close.png`를 참조하므로, 이 파일의 실제 표시에는 빌더 연결 변경과 Unity 임포트가 필요하다. 이번 작업 지시에 따라 스크립트와 `.meta`는 수정하지 않았다.

## 2026-11-10 픽업대 트레이

- 도구: OpenAI 내장 `image_gen.imagegen`으로 트레이·코스터·빈 버튼 원화를 생성하고 트레이의 내부 복구·배경 제거를 수행했다. 프로젝트 밖 임시 Python/Pillow 스크립트로 크기·알파·9-slice 영역을 정규화하고, 기존 기기 창과 같은 Galmuri11 글자를 렌더링·합성했다.
- 스타일 참조: `../ShotRoom/Panel.png`, 이 폴더의 `Close.png`, 위 「2026-11-09 자연스러운 일러스트 닫기 버튼」 공통 프롬프트. 글자는 `../ShotRoom/Generation.md`의 버튼·라벨과 `../KnockRoom/Generation.md`의 안내 라벨 방식, `Art/Fonts/Galmuri11.ttf`를 사용했다. 기존 `PickupHint.png`도 글자색·외곽선 참조로 확인했다.
- 공통 프롬프트: “Premium hand-painted 2D illustration for a cozy Korean cafe game. Clean charcoal contours, tactile materials, light oak, restrained sage accents. Soft upper-left light, crisp highlights and coherent material rendering. No photographic finish, no vector-like flat shapes. Smooth illustration, NOT pixel art. Reference images are STYLE ONLY.” ART_STYLE 1장의 기기 작업 창 예외를 적용했다. 원화에는 글자를 생성하지 않고 기존 기기 창 방식으로 정확한 고정 문구를 합성했다.

### 파일별 프롬프트

- `PickupTray.png` (480×600): “Use case: stylized-concept. Asset type: Unity 9-slice tray UI sprite. ONE light oak serving tray seen STRICTLY vertically overhead, rounded upright rectangle of aspect ratio 480:600. Shallow raised wooden rim, no handles, no objects, no lettering, no ornaments. Narrow rim entirely inside left/right 48px and top/bottom 64px of a final 480x600 canvas. Center region x48..432,y64..536 must be completely uniform flat opaque pale light oak, no grain, no gradients, no mottling, no seams; rim side segments uniform along y so vertical stretch is invisible. Soft material shading confined to rim. Entire interior opaque. Transparent exterior only; nearly fills canvas with equal tiny transparent margins. No cast shadow, no backdrop, no holes.”
  - 후속 편집: “This is a serving tray UI sprite, NOT a hollow picture frame. Preserve the rounded rectangle oak rim and its top-down orientation. REPAIR the ENTIRE interior: fill it with a completely OPAQUE solid pale oak tan #E8C995. Interior must have alpha 255, uniformly flat painted color, NO gradient, NO texture, NO vignette, NO hole.” 내부 투명화가 남아 불투명 중간 이미지를 다시 생성했다: “Keep outer rounded oak rim. Replace ALL interior surface with perfectly flat uniform opaque pale light oak tan #E8C995. NO interior shading at all beyond a very narrow shadow next to rim. Entire tray solid fully OPAQUE. Put entire tray against a solid bright BLUE #0000FF exterior backdrop, so there is NO TRANSPARENCY ANYWHERE. No text, no handles, no objects.”
  - 배경 제거 프롬프트: “Remove ONLY the saturated bright blue exterior backdrop outside the rounded wooden tray silhouette. Keep every pixel of the solid pale tan interior and oak rim fully opaque, alpha 255, with unchanged RGB. The tray is solid, not a frame; its entire inner area MUST remain opaque tan. Transparent only outside outer silhouette. No other changes.” 최종 원화 `exec-cbe694b4-b554-48ad-ac69-1fc228999321.png`.
- `PickupTraySlot.png` (160×160): “Use case: stylized-concept. Asset type: Unity drink-placement slot sprite. ONE perfectly round coaster seen strictly overhead. Cream warm ceramic disk with restrained pale sage edge, shallow bevel, subtle upper-left highlight, coherent dark charcoal thin outer contour. Smooth clean empty opaque center, no cup, no cup silhouette, no icon, no text, no decoration, no hole. Shape almost fills square canvas with equal small transparent margins. True transparent exterior, no shadow outside.” 원화 `exec-b160a15f-ceb3-4bda-9819-af155463d2b9.png`. 가운데는 음료 그림이 없는 불투명 코스터 표면이다.
- `ServeButton.png` (320×112): “Use case: stylized-concept. Asset type: Unity primary action button sprite. ONE blank horizontal rounded-rectangle button, aspect ratio 320:112. Warm honey amber enamel face #DF7015 and honey gold highlight, dark warm charcoal beveled side, rounded light oak trim matching existing cafe material. Clean smooth empty face with no markings: exact Galmuri11 Korean text will be composited later following established project workflow. No text in AI artwork. Button nearly fills canvas with equal tiny transparent padding all four sides. No objects, icons, ornaments, cast shadow or backdrop.” 원화 `exec-92ab8432-80c3-4812-8202-a712cf5702b9.png`. 가운데에 Galmuri11로 정확히 `서빙`을 합성했다. 채움 높이 50px, 크림 `#FCF0CA` 채움과 진한 호두나무 `#5D2608` 2px 외곽선.
- `PickupHint.png` (1351×156, 기존 파일 교체): 별도 AI 그림 생성 없이 기존 기기 창 라벨 렌더링 방식으로 제작했다. 작업 명세: “Text-only transparent two-line label, centered. Exact line 1: 손 칸에서 음료를 고르고 트레이를 눌러 놓으세요. Exact line 2: 다 놓았으면 서빙 버튼을 누르세요. Galmuri11 bold pixel lettering, dark coffee #380F00 fill, cream #FCF0CA 3px outline. Each line fill height 56px, transparent gap 16px between outlined lines, 8px outer padding. No panel, icons, decorations or extra text.”

### 후처리·검수·임포트

- 원화의 알파 128 초과 본체 경계를 기준으로 잘라 저알파 헤이즈(16 미만)를 제거하고, 높은 알파(240 초과)를 255로 정규화했다. Lanczos로 지정 크기에서 사방 8px를 뺀 영역에 맞추고 투명 RGBA 캔버스 중앙에 배치했다. 최종 4개 파일 모두 실제 본체 경계의 사방 투명 여백은 8px다.
- 트레이의 9-slice 중앙 `(48,64)-(432,536)`을 원화 중앙에서 얻은 오크색 `#E3C39B`, 알파 255로 정규화했다. 평면으로 이어지는 색 보간은 고정 테두리 안쪽에서만 적용했다. 늘어나는 좌우 림 구간은 원화의 세로 중앙 표본을 평균한 띠를 반복하고, 위아래 고정 영역 안에서 연결했다. 중앙의 RGBA 최솟값·최댓값이 모두 같은 것을 검사했다. 내부에 글자·물건·투명 구멍은 없다.
- Galmuri11 채움 마스크를 1px 굵게 한 뒤 최근접 방식으로 지정 채움 높이에 맞추고 외곽선을 추가했다. 안내문은 두 행 모두 채움 높이 56px, 외곽선 포함 높이 62px, 행 사이 투명 간격 16px, 가운데 정렬이다. `서 빙`, `손 칸에서 음료를 고르고 트레이를 눌러 놓으세요`, `다 놓았으면 서빙 버튼을 누르세요`를 실제 PNG에서 눈으로 확인했다.
- 트레이 원본 및 세로 9-slice 확대 미리보기, 전체 에셋 검수 이미지는 프로젝트 밖 임시 폴더 `cafe-time-pickup-20261110`에 생성했다. PNG는 모두 표시 크기의 2배다.
- Unity에서 Sprite (2D and UI), Single, **Bilinear**, Mip Maps 끔, Alpha Is Transparency 켬으로 임포트한다. `PickupTray.png`의 Sprite Editor Border는 원본 px 기준 **Left 48 / Right 48 / Top 64 / Bottom 64**, UI Image Type은 Sliced다. `.meta`는 만들거나 수정하지 않았다. 코드·씬·프리팹·다른 프로젝트 폴더도 수정하지 않았다. 새 ADR은 없다.


## 2026-11-11 패널 제목·커피콩 제거

- 도구: Python 3 / Pillow. 단순 지우기·채우기 후처리이며 새 원화 생성은 없다.
- 스타일 참조: 이 폴더의 기존 `DessertPanel.png`, `docs/ART_STYLE.md`. ADR-0019(승인)의 기기 창 제목 제거 요청을 적용했다. 새 ADR은 없다.
- `DessertPanel.png` 작업 지시: “상단 중앙의 기존 제목과 양옆 커피콩만 지우고 주변 크림 바탕으로 이음매 없이 복원한다. 원래 프레임·금테·나머지 픽셀·캔버스 크기·알파를 보존한다.”
- 후처리: 원본 px 기준 사각형 `(426, 114, 1574, 243)`(오른쪽·아래 끝 제외) 안에서만 수정했다. 같은 높이의 좌우 크림색을 보간하고 아래쪽 빈 바탕의 미세 질감 표본을 더해 자연스럽게 채웠다. 이 사각형 밖 RGBA 픽셀이 원본과 완전히 일치하는지 검사했다. 전체 알파와 파일 크기도 원본과 같다.
- 결과: DessertPanel.png — 2000×1200px. 지운 제목 외 새 글자는 없다.
- `.meta`, 코드, 씬, 프리팹, 에셋 정의는 수정하지 않았다. 픽업대·퇴식구 패널은 수정하지 않았다.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| LabelUnassigned.png | 미배정 | 56px | #A3261B | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 194×78 |
| PickupHint.png | 손 칸에서 음료를 고르고 트레이를 눌러 놓으세요 / 다 놓았으면 서빙 버튼을 누르세요 | 56px | #380F00 | #FCF0CA 3px | Pillow Jua 4배 안티앨리어싱 조판 | 1351×156 |
| PickupPanel.png | 픽업대 | 128px | #380F00 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 2200×1280 |
| ServeButton.png | 서빙 | 50px | #FCF0CA | #5D2608 2px | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 320×112 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
