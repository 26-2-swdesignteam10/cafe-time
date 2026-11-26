# 장비 업그레이드 / 알바생 고용 UI 생성 기록

- 날짜: 2026-11-26.
- 도구: OpenAI 내장 image_gen, Python 3 / Pillow 12.3.0. CLI/API는 사용하지 않았다.
- 범위: 새 UI 에셋 PNG 27개 제작. 화면 대체 기능·흐름·게임 수치·코드는 변경하지 않았다.
- 기존 파일은 참조와 복사만 했다. .meta 생성·커밋 없음. ADR 추가·변경 없음. 기존 ADR-0008과 ADR-0026을 따른다.

## 스타일·구성 참조

- 사용자 시안: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/8a676556-5541-47b6-ba2f-aa4ffc0de2df/scratchpad/mockup/UpgradeMockup.png`, 같은 폴더의 `StaffMockup.png`. 두 시안의 구성과 아트 방향을 확인했다.
- `../OpenPrep/CardFrame.png`, `BuyButton.png`, `StartButton.png`, `CatalogPanel.png`, `TabSelected.png`, `TabNormal.png`: 계단식 모서리, 나무 베벨, 금색 안쪽 선, 크림·주황 면의 기준. 생성 도구에는 파일별 대응 프레임·탭을 참조 이미지로 전달했다.
- `../Settlement/NightBackground.png`: 야경 구도, 덩굴·꽃·차양·창문 불빛. 생성 도구에 직접 전달했다.
- `../../Sprites/Cafe/StationDishwash.png`, `StationIceMaker.png`, `StationEspresso.png`, `StationPos.png`, `StationTrayReturn.png`, `StationWater.png`: 각 장비의 실제 기기 참조로 생성 도구에 직접 전달했다.
- `../../Fonts/Jua-Regular.ttf`, `../OpenPrep/Generation.md`, `../Score/Generation.md`의 Jua 재조판 절, `docs/ART_STYLE.md`.

## 공통 프롬프트

> Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

공통 요구는 단독 정면 UI 스프라이트, 나무 프레임과 금색 안쪽 선, 바깥 투명·내부 불투명, 글자·숫자 없음이다. 배경만 완전 불투명 야경이다. 장비는 기존 맵 기기의 스테인리스·무광 검정·오크 재질과 시점을 유지한다.

## 파일별 생성 프롬프트

### Background.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. Create Background.png, ONE 1920x1080 landscape fully opaque night cafe exterior background, matching supplied NightBackground reference and UpgradeMockup composition extremely closely: ivy and tiny cream flowers hanging over dark timber cafe facade, striped awnings, golden illuminated windows, dark navy night sky at upper left, warm cobbled pavement. No words, letters, numbers, UI, panels or people. Darken overall scene slightly for foreground UI readability, retain golden window lights. Reference images are STYLE AND COMPOSITION REFERENCES only. No transparency for this background.

### Panel.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Wide empty wooden main panel 1800:760, straight orange brown frame, gold inner line, flat cream interior.

### TabOn.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty active tab 640:150, stepped corners, orange face, gold inset and walnut bevel.

### TabOff.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty inactive tab 640:150, stepped corners, cream face, gold inset and walnut bevel.

### EquipmentCard.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty equipment card 260:330, upper square icon frame, lower half plain cream.

### EquipmentCardSelected.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Same equipment card silhouette and icon frame, thick orange selected outline, fully opaque cream interior.

### StageBox.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty stage description box 520:150, cream center, walnut bevel and gold inner rim.

### StageBoxDone.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty completed stage box 520:150, darker muted beige center, walnut bevel.

### StageArrow.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. One orange right-pointing chevron >, stepped pixel edges, transparent outside, no shaft.

### ButtonPrimary.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty action button 720:150, orange face, walnut bevel and gold inner rule.

### ButtonDisabled.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty disabled button 720:150, desaturated gray beige face and gray brown bevel.

### StaffCard.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty tall staff card 400:620, walnut gold frame, square portrait inset at top, plain cream lower area.

### StaffSlotPanel.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty tall staff slots panel 400:620, walnut gold frame, cream inside, no subdivisions.

### StaffSlotEmpty.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty staff slot 360:150, beige dashed pixel border, opaque cream center.

### PortraitFrame.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated production UI sprite, front orthographic view. Match reference frame materials closely: dark coffee #380F00 edges, walnut #90471B and #5D2608 beveled wood, honey gold inset line, cream #FAEAC2. Opaque object interior, actual transparent exterior with equal padding. NO text, numbers, icons, symbols, logos or watermarks. Uniform border thickness for 9-slice, flat single-color center, no texture across the center. Empty square portrait frame 200:200, narrow stepped walnut gold rim, transparent outside AND inside.

### IconDishwasher.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated equipment inventory icon for a 256x256 UI sprite: a stainless sink cabinet with curved faucet on left and a rack of inverted white cups and dishes on right. Supplied StationDishwash.png is the exact equipment identity reference: match its silhouette, materials, colors and front slightly elevated view. Retain clean minimal stainless silver, charcoal thin outlines, restrained 2-3 shade blocks. Match existing map equipment closely, no scene or enclosing frame. Equipment centered with generous transparent space around every edge. No text, numbers, logos, letters or cast shadow outside object. Real transparent background, opaque machine.

### IconIceMaker.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated equipment inventory icon for a 256x256 UI sprite: a compact stainless ice maker with dark glass upper bin filled with chunky pale blue ice cubes, lower metal front and vent slits. Supplied StationIceMaker.png is the exact equipment identity reference: match its silhouette, materials, colors and front slightly elevated view. Retain clean minimal stainless silver, charcoal thin outlines, restrained 2-3 shade blocks. Match existing map equipment closely, no scene or enclosing frame. Equipment centered with generous transparent space around every edge. No text, numbers, logos, letters or cast shadow outside object. Real transparent background, opaque machine.

### IconEspresso.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated equipment inventory icon for a 256x256 UI sprite: a black coffee grinder beside a two-group stainless espresso machine, both on a low stainless cabinet. Supplied StationEspresso.png is the exact equipment identity reference: match its silhouette, materials, colors and front slightly elevated view. Retain clean minimal stainless silver, charcoal thin outlines, restrained 2-3 shade blocks. Match existing map equipment closely, no scene or enclosing frame. Equipment centered with generous transparent space around every edge. No text, numbers, logos, letters or cast shadow outside object. Real transparent background, opaque machine.

### IconPos.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated equipment inventory icon for a 256x256 UI sprite: a black POS monitor on its pedestal and a separate small payment card terminal at right. Supplied StationPos.png is the exact equipment identity reference: match its silhouette, materials, colors and front slightly elevated view. Retain clean minimal stainless silver, charcoal thin outlines, restrained 2-3 shade blocks. Match existing map equipment closely, no scene or enclosing frame. Equipment centered with generous transparent space around every edge. No text, numbers, logos, letters or cast shadow outside object. Real transparent background, opaque machine.

### IconTrayReturn.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated equipment inventory icon for a 256x256 UI sprite: a tall narrow warm oak tray return shelving cart with four horizontal tray shelf openings and dark short base. Supplied StationTrayReturn.png is the exact equipment identity reference: match its silhouette, materials, colors and front slightly elevated view. Retain clean minimal stainless silver, charcoal thin outlines, restrained 2-3 shade blocks. Match existing map equipment closely, no scene or enclosing frame. Equipment centered with generous transparent space around every edge. No text, numbers, logos, letters or cast shadow outside object. Real transparent background, opaque machine.

### IconWater.png

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. Use case: stylized-concept. ONE isolated equipment inventory icon for a 256x256 UI sprite: a compact stainless water dispenser with black top control strip, red and blue taps, drip tray and black base. Supplied StationWater.png is the exact equipment identity reference: match its silhouette, materials, colors and front slightly elevated view. Retain clean minimal stainless silver, charcoal thin outlines, restrained 2-3 shade blocks. Match existing map equipment closely, no scene or enclosing frame. Equipment centered with generous transparent space around every edge. No text, numbers, logos, letters or cast shadow outside object. Real transparent background, opaque machine.

## Jua 조판·복사 지시

| 파일 | 정확한 문구 | 채움 높이 | 채움 / 외곽선 |
|---|---|---:|---|
| LabelTabUpgrade.png | 장비 업그레이드 | 56px | #380F00 / #FCF0CA 3px |
| LabelTabStaff.png | 알바생 고용 | 56px | #380F00 / #FCF0CA 3px |
| LabelTitleUpgrade.png | 장비 업그레이드 | 96px | #FCF0CA / #380F00 4px |
| LabelTitleStaff.png | 알바생 고용 | 96px | #FCF0CA / #380F00 4px |
| LabelMoney.png | 보유금 | 72px | #FCF0CA / #380F00 3px |
| StartButton.png | 영업 시작 | 기존 80px | 기존 Jua 조판 보존 |

- 글자는 AI로 그리지 않고 지정 Jua 폰트로 조판했다. 4배 해상도에서 글리프를 그린 뒤 채움 경계로 자르고 종횡비를 유지해 목표 채움 높이를 맞췄다. 고해상도 마스크 확장으로 외곽선을 만든 뒤 Lanczos로 축소하고 사방 8px 투명 여백을 더했다. 글리프를 임의로 굵게 만들거나 최근접 보간으로 도트화하지 않았다.
- LabelMoney는 기존 OpenPrep/LabelMoney.png의 작은 금색 동전 장식을 읽어 보존하고 새 Jua 글자 앞에 배치했다.
- StartButton은 OpenPrep/StartButton.png를 바이트 그대로 복사했다. 기존 Jua 문구는 정확히 "영업 시작"이며 공백 하나를 유지한다.
- 장비 이름·단계·금액·설명·일당·직원 이름·인원 수·별 등은 이미지에 넣지 않았다. 카드의 아이콘·초상화 틀도 비어 있다.

## 후처리

1. 알파 128 초과 본체 경계로 생성 원화를 크롭했다. 패널·카드·버튼·탭·빈 슬롯·화살표·초상화 틀은 NEAREST로 지정 캔버스보다 가로세로 16px 작은 크기로 맞춰 중앙 배치했다. 사방 완전 투명 여백은 8px이고, 복사한 StartButton은 원본의 6px이다.
2. 패널류는 외부와 이어지지 않은 투명 구멍을 채우고 본체 알파를 0/255로 정리했다. 크림 면을 #FAEAC2로 정리하고 카드 하단의 알파 구멍과 얼룩을 복원했다. PortraitFrame은 예외로 안쪽 알파 0을 유지했다.
3. 선택 탭·주요 버튼 중앙은 #DF7015, 비활성 버튼은 #C5BDAA, 완료 단계 상자는 #DCCBA6이다. 긴 버튼 중앙의 생성 얼룩·장식을 단색 다각형으로 정리했다. 생성된 나무·금색 테두리는 유지했다.
4. 장비 아이콘은 생성 그림과 알파를 보존하며 종횡비를 유지해 최대 224×224로 Lanczos 축소한 뒤 256×256 중앙에 배치했다. 가로세로 비율이 다른 기기를 억지로 정사각형으로 늘리지 않았다.
5. 배경은 1920×1080으로 Lanczos 리사이즈 후 밝기를 생성본의 76%로 낮췄다. 알파는 전부 255이고 창문 불빛·덩굴·꽃·차양을 유지했다. 이 화면의 야경 강조색은 남색·어두운 올리브·주황 창문 불빛이다.
6. 새로 처리한 PNG의 알파 0 픽셀 RGB는 0으로 정리했다. StartButton은 원본 바이트를 보존했다.

## 최종 규격

| 파일 | 최종 크기 |
|---|---|
| Background.png | 1920×1080 RGBA |
| Panel.png | 1800×760 RGBA |
| TabOn.png | 640×150 RGBA |
| TabOff.png | 640×150 RGBA |
| LabelTabUpgrade.png | 427×78 RGBA |
| LabelTabStaff.png | 314×78 RGBA |
| LabelTitleUpgrade.png | 718×120 RGBA |
| LabelTitleStaff.png | 524×120 RGBA |
| LabelMoney.png | 260×94 RGBA |
| EquipmentCard.png | 260×330 RGBA |
| EquipmentCardSelected.png | 260×330 RGBA |
| IconDishwasher.png | 256×256 RGBA |
| IconIceMaker.png | 256×256 RGBA |
| IconEspresso.png | 256×256 RGBA |
| IconPos.png | 256×256 RGBA |
| IconTrayReturn.png | 256×256 RGBA |
| IconWater.png | 256×256 RGBA |
| StageBox.png | 520×150 RGBA |
| StageBoxDone.png | 520×150 RGBA |
| StageArrow.png | 48×64 RGBA |
| ButtonPrimary.png | 720×150 RGBA |
| ButtonDisabled.png | 720×150 RGBA |
| StaffCard.png | 400×620 RGBA |
| StaffSlotPanel.png | 400×620 RGBA |
| StaffSlotEmpty.png | 360×150 RGBA |
| PortraitFrame.png | 200×200 RGBA |
| StartButton.png | 1040×220 RGBA |

## 철자·그림 검수 결과

- 완성 라벨과 StartButton을 색 배경에 합성해 눈으로 확인했다. "장 / 비 / 공백 / 업 / 그 / 레 / 이 / 드", "알 / 바 / 생 / 공백 / 고 / 용", "보 / 유 / 금", "영 / 업 / 공백 / 시 / 작"을 대조했다. 오탈자·누락·추가 문구 없음. 두 탭과 두 큰 제목의 공백은 각각 하나다.
- 싱크와 식기 랙, 얼음이 보이는 제빙기, 그라인더와 2그룹 에스프레소 머신, 포스 모니터와 카드 단말기, 오크 퇴식 선반, 빨강·파랑 토출구 정수기를 기존 기기와 대조했다. 장비 그림에 문구·숫자 없음.
- PNG 수·크기·RGBA 모드·알파 경계·투명 여백을 검사했다. 패널류 내부는 불투명하고 PortraitFrame 중앙은 완전 투명이다. 배경 알파는 전부 255이다.

## Unity 임포트 안내

1. Unity **6000.6.4f1** 에디터로 돌아가 새 PNG를 임포트하여 .meta를 자동 생성한다.
2. Sprite (2D and UI), Single, Point (no filter), Generate Mip Maps 끔, Alpha Is Transparency 켬, Max Size 2048을 사용한다.
3. Sprite Editor Border는 실제 프레임 안쪽 선을 포함하도록 설정한다. 시작값 제안(좌·하·우·상, px): Panel (96,96,96,96), EquipmentCard 계열 (32,32,32,32), StaffCard/StaffSlotPanel (38,38,38,38), Tab/Button/StageBox 계열 (48,40,48,40), StaffSlotEmpty (20,20,20,20), StartButton (300,60,240,60). 장비·알바생 카드는 상단 안쪽 틀을 포함하므로 원본 크기를 유지하고 틀 아래의 단색 영역에서만 세로 신장을 확인한다. 이 값은 .meta에 적용하지 않았다.
4. 후속 빌더 연결 시 실제 표시 크기에서 라벨 가독성, 선택·비활성 대비, 9-slice 모서리를 확인한다. 이번 작업에는 화면 연결을 포함하지 않았다.

