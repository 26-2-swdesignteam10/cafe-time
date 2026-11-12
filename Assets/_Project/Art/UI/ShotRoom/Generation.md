# ShotRoom 일러스트 생성 기록 — 2026-11-08

도구: OpenAI 내장 `image_gen.imagegen`으로 원화를 생성·수정하고, 프로젝트 밖 임시 스크립트에서 System.Drawing으로 투명 영역을 잘라 고품질 축소·합성했다. 스타일 참조: 기존 `ShotRoom/Panel.png`의 기기 배치와 `Art/UI/CupShelf/Panel.png`의 목재 프레임. 이 창에는 도트 격자 규칙을 적용하지 않는다.

## 공통 스타일

> Premium hand-painted 2D illustration for a cozy Korean cafe game. Clean charcoal contours, tactile neutral brushed stainless steel, matte black handles, light oak, warm greige wall, restrained sage accents. Soft upper-left light, crisp highlights and coherent material rendering. No photographic finish, no vector-like flat shapes, no text inside AI art.

## 파일별 생성·수정

- `Panel.png` (1536×864): 기존 Panel을 편집 대상으로 삼아 좌표와 빈 LCD·버튼·라벨 자리를 유지했다. “Physically coherent two-group espresso machine, body-to-counter side columns, groupheads attached to underside, one full-width grated drip tray, subtle cup marks, recessed rubber tamping mat, open stainless knock box with black crossbar, two sturdy sage racks with feet touching lower shelf, continuous cabinet and frame.” 이어서 장비만 “clean neutral brushed stainless, silver gray with crisp reflected highlights, no pink, copper, gold, or water stains”로 다시 편집했다. 원화: `exec-bfa09cf1-7229-47d3-8003-2a909a4d8e0d.png`.
- `PortafilterEmpty.png`, `PortafilterGround.png`, `PortafilterTamped.png`, `PortafilterUsed.png` (각 184×72): “Single horizontal side-profile portafilter; ergonomic curved matte black handle left, shallow brushed stainless basket right, thick rim, crisp locking lugs, double spout, empty well; transparent, no shadow.” 원화 `exec-8a9ade35-9e8e-4a47-998c-3b3e4aeb4ba7.png`를 공통 몸체로 사용했다.
- `PortafilterFrontEmpty.png`, `PortafilterFrontGround.png`, `PortafilterFrontTamped.png`, `PortafilterFrontUsed.png` (각 184×88): “Front view with slight top-down elevation; visible oval opening, locking lugs, twin spouts and centered handle toward viewer; neutral stainless, transparent, no shadow.” 원화 `exec-67b7f5d0-4d73-41aa-82f1-c6402ffc4427.png`를 공통 몸체로 사용했다. 그림은 row 4~83 안에 배치했다.
- 상태별 내용물: 빈 내부는 공통 몸체 그대로 사용했다. 분쇄 가루는 “fine irregular microgranules, softly convex mound, no regular dots or lumps” (`exec-837e5632-c7a6-4877-b5d3-db1c1c127d72.png`), 다진 면은 “dense level dark coffee puck with tiny natural grain” (`exec-8cf5d1bf-4341-4b36-aae6-b8e14015a0c4.png`), 찌꺼기는 “moisture sheen, irregular organic hairline cracks, no decorative zigzag” (`exec-f51cde21-3c13-497b-b089-2869737532cb.png`) 프롬프트로 크게 생성해 각 방향의 동일한 몸체 안에 축소 합성했다.
- `PortafilterMounted.png` (140×64): “Front-facing locked portafilter, crisp left/right steel locking lugs, shallow basket, two extraction spouts, centered short black handle, transparent, no shadow.” 원화 `exec-dd70fb1e-2f36-45b5-9052-4f810907d351.png`를 축소했다.
- `Tamper.png` (92×138): “Substantial rounded charcoal handle with sage accent, steel neck, broad thick flat brushed stainless base, upper-left light, transparent, no shadow.” 원화 `exec-fbb91262-931d-4055-9bb8-539abf3a16a6.png`를 아래끝 row 136 부근으로 축소했다.
- `GrindButton.png` (170×60), `ExtractButton.png` (140×56): “Blank hand-painted muted sage enamel button, dark beveled sides, upper-left cream highlight, transparent, no text.” 원화 `exec-6d90652d-3df9-430b-9442-d98bc28b0c6c.png`를 각각 축소한 뒤 Galmuri11로 `분쇄`, `추출`을 넣었다. 크림 채움과 어두운 세이지 2px 외곽선이다.
- `LabelGrind.png` (118×48), `LabelTamp.png` (126×48), `LabelExtract.png` (130×48), `LabelKnock.png` (260×48), `LabelRack.png` (296×48): Galmuri11 글리프를 채움 높이 약 36px로 렌더링하고 1px 바깥으로 굵게 한 뒤 2px 크림 외곽선을 둘렀다. 채움 `#380F00`, 외곽선 `#FCF0CA`. 글자는 순서대로 `1 분쇄`, `2 탬핑`, `3 추출`, `4 찌꺼기 털기`, `포터필터 받침대`. 마지막 두 라벨은 잘림 없이 글자 크기를 유지하려고 원래 폭에서 각각 2px 늘렸다.

Panel의 빈 장비 자리에는 동적 스프라이트나 글자를 굽지 않았다. 합성 좌표는 `(768+x, 432-y)`를 사용했다. 프로젝트 밖 미리보기 `ShotRoomPreview/ShotRoomPreview.png`에는 각 자리의 포터필터, 장착형, `Art/Icons/Items/ShotGlass.png` 컵을 합성했고, `ShotRoomPreview/TamperLoweredPreview.png`에는 탬퍼를 60px 내려 접촉 위치를 확인했다. 생성 원화와 작업 스크립트는 프로젝트 폴더에 두지 않았다.

## 2026-11-08 앞모습·장착 그림 크기와 내용물 깊이 보정

도구: OpenAI 내장 `image_gen.imagegen`으로 기존 PNG를 편집하고, 프로젝트 밖 임시 스크립트의 System.Drawing으로 투명 영역을 축소·합성했다. 스타일 참조: 이 폴더의 `PortafilterFrontEmpty.png`, `PortafilterEmpty.png`, `PortafilterGround.png`, `PortafilterTamped.png`, `PortafilterUsed.png`, `PortafilterMounted.png`. 공통 프롬프트: 기존의 중성 브러시드 스테인리스, 부드러운 좌상단 광원, 무광 검정 손잡이, 투명 배경을 유지한다.

- `PortafilterFrontEmpty.png` (184×88): 앞모습을 재생성해 가로 중앙의 실제 폭 96px, 세로 row 8~82로 배치했다. 타원 입구의 뒤쪽 내벽과 앞쪽 금속 테두리를 분리했다. 생성 원본: `exec-bf72c265-6303-47ca-a950-51ac84e2888c.png`.
- `PortafilterFrontGround.png` (184×88): 위 빈 몸체에만 커피 영역을 합성했다. 고운 불규칙 입자가 보이는 볼록한 산, 최고 row 4. 앞쪽 금속 테두리가 가루의 아래 가장자리를 가린다. 내용물 원본: `exec-a6e4d272-85d0-480c-aeff-b35b9d2a5582.png`.
- `PortafilterFrontTamped.png` (184×88): 같은 몸체의 바스켓 안에 입구 원근과 맞춘 낮은 타원 다진 면을 넣었다. 내용물 원본: `exec-30f55e84-a4b0-44c4-9262-38d6f6bfb045.png`.
- `PortafilterFrontUsed.png` (184×88): 같은 몸체의 바스켓 안에 젖은 질감과 불규칙한 미세 균열이 있는 타원 찌꺼기 면을 넣었다. 내용물 원본: `exec-ada8f58f-82ca-43d5-86e9-14257747ef03.png`.
- `PortafilterMounted.png` (140×64): 실제 폭 120px, row 0~63. 잠금 귀와 테두리, 바스켓 옆면, 좌우 추출구, 앞으로 나온 손잡이 순으로 배치하고 내용물은 보이지 않게 했다. 생성 원본: `exec-e77f8ba0-e293-4c41-a310-bcdad229ee40.png`.

앞모습 네 장은 내용물 영역 바깥에 동일한 빈 몸체 픽셀을 사용한다. 검증은 프로젝트 밖 `shotroom-preview/FrontStates4x.png`에서 4배 확대하고 `shotroom-preview/PanelComposite.png`에서 Panel, Tamper, ShotGlass와 합성해 수행했다. 기존 옆모습 `PortafilterGround.png`는 이번 최종 결과에서 변경하지 않았다.
## 2026-11-09 분쇄·추출 애니메이션

도구: 프로젝트 밖 임시 PowerShell 스크립트의 System.Drawing으로 투명 프레임을 제작하고 2배 합성 미리보기에서 접점을 확인했다. 스타일 참조: 이 폴더의 Panel.png, PortafilterFrontEmpty.png, PortafilterMounted.png와 Art/Icons/Items/ShotGlass.png. 공통 스타일은 위의 ShotRoom 공통 프롬프트를 따른다. 프레임은 같은 형태를 유지하면서 입자와 크레마 하이라이트의 세로 위치만 순환한다.

- GrindStream0.png~GrindStream3.png (각 48×30): “Fine irregular roasted-brown microgranules falling from the grinder chute into the front-facing portafilter basket; coherent soft edges, transparent background.” 배출구 아래끝 이미지 y≈407, 바스켓 입구 y≈437에 맞춰 중심 창 좌표 (-570, 9)로 합성했다.
- ExtractStream0.png~ExtractStream3.png (각 100×52): “Two slender dark espresso threads from the mounted portafilter's twin spouts to just inside the shot glass, with restrained crema-colored highlights; transparent background.” 두 그룹헤드에 같은 프레임을 각각 중심 (-20, 13), (320, 13)으로 합성했다.

미리보기: 프로젝트 밖 %TEMP%/cafe-time-preview-20261109/ShotRoom2x.png. 기존 PNG는 수정하지 않았다.


## 2026-11-09 사용자 피드백 보정: 호퍼·분쇄·추출 줄기

도구: 내장 image_gen 편집을 시도했으나 사용 한도(HTTP 429)에 도달하여, 프로젝트 밖 임시 PowerShell/C# 스크립트와 System.Drawing으로 원본 픽셀을 참조해 국소 보정했다. 스타일 참조: 이 폴더의 기존 Panel.png, PortafilterFrontEmpty.png, PortafilterMounted.png 및 Art/Icons/Items/ShotGlass.png. 모든 프레임은 투명 RGBA이며 동일한 크기의 짝수 캔버스를 사용한다.

- `Panel.png` (1536×864): 그라인더의 투명 원두통 윗부분만 위로 약 33px 늘렸다. 기존 투명부 y≈129~210을 y≈96~210으로 확장하여 높이를 약 41% 늘리고 뚜껑을 34px 올렸다. 원두와 하단 연결부, 그라인더 몸체는 원래 픽셀을 유지했다. 수정 영역은 이미지 좌표 x≈76~318, y≈63~144다.
- `GrindStream0.png`~`GrindStream7.png` (각 56×44): 배출구 y≈407에서 앞모습 바스켓 입구 y≈437 안쪽까지 이어지는 고운 갈색 분쇄 줄기. 아래로 조금 퍼지는 중심부, 가장자리 미세 입자, 바스켓 근처의 가벼운 튐을 그렸다. 입자 및 밝은 무늬는 프레임마다 아래로 이동한다. 창 중심 좌표는 (-570, 8)이다.
- `ExtractStream0.png`~`ExtractStream7.png` (각 112×64): 장착 포터필터의 두 추출구에서 샷 잔 안쪽으로 모이는 두 갈래 갈색 줄기. 금갈색 크레마 하이라이트와 잔 안쪽의 작은 거품을 추가하고 명암 무늬를 프레임마다 아래로 이동시켰다. 두 그룹헤드에 같은 그림을 각각 창 중심 좌표 (-20, 12), (320, 12)로 배치한다.

2배 미리보기: `%TEMP%/cafe-time-shotroom-20261109/Panel2x.png`, `GrindFrames2x.png`, `ExtractFrames2x.png`. 움직임 확인: 같은 폴더의 `Grind.gif`, `Extract.gif` (각 8프레임).

## 2026-11-09 그라인더 호퍼 보정

도구: OpenAI 내장 image_gen 편집을 시도했으나 HTTP 429 사용 한도로 완료되지 않아, 프로젝트 밖 임시 C#/System.Drawing 스크립트로 원본 픽셀을 국소 보정했다. 스타일 참조: 기존 ShotRoom/Panel.png의 따뜻한 벽 조명, 투명 호퍼 반사, 금속 뚜껑.

- Panel.png: 호퍼 뒤 벽의 직사각형 패치를 주변 그라데이션과 대각 빛결로 복원했다. 늘린 유리 구간의 가로 반복 무늬를 세로 반사광과 약한 굴절로 교체하고, 양쪽 유리 외곽선을 뚜껑부터 원두 구간까지 연속된 사다리꼴로 이었다. 뚜껑 윗선의 톱니를 부드럽게 다듬었다. 호퍼 높이와 그라인더 몸체·LCD·배출구 이하, 다른 Panel 영역의 픽셀은 유지했다.

검수 미리보기: 프로젝트 밖 scratchpad의 Hopper_final4x.png(x 20~360, y 20~200, 4배)와 Panel_final_full.png.

## 2026-11-09 포터필터 받침대 단순화

- 도구: OpenAI image_gen.imagegen으로 두 받침대가 놓인 패널 장면을 편집했다. 스타일 참조: 이 폴더의 `Panel.png`, `PortafilterEmpty.png`.
- `Panel.png` (1536×864): 기존 세이지색 U자형 거치대 두 개를 낮고 단순한 스테인리스 직사각형 판과 어두운 고무 윗면으로 교체했다. 다리·기둥·장식은 두지 않았다.
- 후처리: 생성 이미지에서 받침대 주변만 잘라 기존 패널에 합성했다. 수정 범위는 왼쪽 `(520,647)-(730,757)`, 오른쪽 `(800,647)-(1020,757)`이며, 그 밖의 원본 픽셀과 알파는 유지했다.
- 포터필터 2개를 슬롯 위치에 실제 크기로 합성한 2배 미리보기: `ShotRoomRackPreview2x.png`.


## 2026-11-09 포터필터 직사각형 판 배치 재검증

- 도구: Python Pillow로 실제 표시 크기 합성 및 PNG 속성을 검사했다. 작업 시작 시 `Panel.png`에는 위의 받침대 단순화 결과가 이미 반영되어 있어 추가 이미지 생성이나 원본 덮어쓰기는 하지 않았다.
- 스타일 참조: `docs/ART_STYLE.md` 1장의 부드러운 기기 작업 창 일러스트 예외와 이 폴더의 `Panel.png`, `PortafilterEmpty.png`, `PortafilterFrontEmpty.png`.
- `Panel.png`: 추가 생성 프롬프트 없음. 낮은 스테인리스 직사각형 판과 무광 검정 윗면 두 개를 유지했다. 1536×864 RGBA, 알파 범위 220~255. 판 윗면은 이미지 y≈727~734이며 슬롯에 놓인 포터필터의 아래끝 y=726과 맞닿아 보이는 것을 확인했다. 원본 픽셀과 알파를 전부 유지했다.
- 미리보기 후처리: `PortafilterEmpty.png`를 184×72 그대로 왼쪽 위 (531, 666), (821, 666)에 합성했다. 슬롯 중심은 각각 (623, 702), (913, 702)이다. `LabelRack.png`도 중심 (768, 802)에 합성하여 판이 라벨을 가리지 않음을 확인했다. 미리보기에서만 합성했으며 Panel에는 포터필터와 글자를 넣지 않았다.
- 실제 크기 전체 미리보기: `%TEMP%/cafe-time-illustration-preview/ShotRoomRackPreview.png` (1536×864). 받침대 영역 2배 확대: 같은 폴더의 `ShotRoomRackDetail2x.png`.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 투명 라벨의 채움 높이는 S 56 / M 72 / L 96px이며, 원래 캔버스 높이와 최소 8px 투명 여백을 유지했다. 기존 높이 72px인 외곽선 라벨은 채움 50px, 높이 48px인 라벨은 채움 36px·외곽선 2px·상하 여백 4px로 기존 캔버스 높이를 우선했다. `Pos/TabDessert.png`의 높이 76px는 채움 56px·외곽선 3px·상하 여백 7px다. 해당하는 파일만 아래 표의 규격 예외가 적용된다. 폭이 부족한 라벨은 폭만 늘렸다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| LabelExtract.png | 3 추출 | 36px | #380F00 | #FCF0CA 2px | Pillow Jua 4배 안티앨리어싱 조판 | 130×48 |
| LabelGrind.png | 1 분쇄 | 36px | #380F00 | #FCF0CA 2px | Pillow Jua 4배 안티앨리어싱 조판 | 118×48 → **124×48 (폭 확대)** |
| LabelKnock.png | 4 찌꺼기 털기 | 36px | #380F00 | #FCF0CA 2px | Pillow Jua 4배 안티앨리어싱 조판 | 260×48 |
| LabelRack.png | 포터필터 받침대 | 36px | #380F00 | #FCF0CA 2px | Pillow Jua 4배 안티앨리어싱 조판 | 296×48 |
| LabelTamp.png | 2 탬핑 | 36px | #380F00 | #FCF0CA 2px | Pillow Jua 4배 안티앨리어싱 조판 | 126×48 → **130×48 (폭 확대)** |
| ExtractButton.png | 추출 | 28px | #FCF0CA | #5D2608 1px | 글자 양옆의 세이지 바탕 표본 복원 + Pillow Jua 조판 | 140×56 |
| GrindButton.png | 분쇄 | 28px | #FCF0CA | #5D2608 1px | 글자 양옆의 세이지 바탕 표본 복원 + Pillow Jua 조판 | 170×60 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
