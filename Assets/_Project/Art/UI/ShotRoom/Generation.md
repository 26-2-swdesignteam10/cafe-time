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