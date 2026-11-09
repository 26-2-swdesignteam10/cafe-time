# KnockRoom 일러스트 생성 기록 — 2026-11-09

도구: OpenAI 내장 image_gen.imagegen으로 원화를 생성·편집하고, 프로젝트 밖 임시 PowerShell 스크립트의 System.Drawing으로 투명 스프라이트를 크기 맞춤·합성했다. 스타일 참조: Art/UI/ShotRoom/Panel.png, PortafilterUsed.png, PortafilterFrontEmpty.png, PortafilterFrontUsed.png. 이 창은 ShotRoom과 같은 부드러운 일러스트이며 도트 격자를 적용하지 않는다. Unity에서는 1:1 크기와 Bilinear 필터를 사용한다.

## 공통 스타일

> Premium hand-painted 2D illustration for a cozy Korean cafe game. Clean charcoal contours, tactile neutral brushed stainless steel, matte black handles, light oak, warm greige wall, restrained sage accents. Soft upper-left light, crisp highlights and coherent material rendering. No photographic finish, no vector-like flat shapes, no text inside AI art.

## 파일별 프롬프트와 후처리

- Panel.png (1536×864): “The same rounded light-oak frame, greige wall and worktop as ShotRoom; a large open neutral-stainless knock box with visible inner depth and a little spent coffee; black rubber crossbar; empty small brush tray at right; clear upper hint and upper-right close-button areas; no text, brush or portafilter.” 원화 exec-4f11aa1e-09de-4e90-a4e0-c8fe9156869c.png를 위치 보정한 exec-a62dbc13-c47c-43d4-aec1-28daa11848d3.png에서 축소했다. 막대 윗면 이미지 y=542가 되도록 세로 구간을 맞췄다.
- PortafilterKnock.png (560×220): “Large inverted side-profile portafilter, black handle left, downward basket showing a moist spent puck, neutral brushed steel, no shadow, transparent.” 원화 exec-1347b4b7-ce1b-4eaf-95fe-7027f8e5bc58.png에서 몸체를 축소하고 바스켓 중심 x≈400, 테두리 최저점 row 212에 맞췄다.
- PortafilterKnockEmpty.png (560×220): 위 포터필터의 찌꺼기만 제거해 얕은 스테인리스 내부와 미량의 가루를 남긴 편집 원화 exec-af2ed1fe-1719-482a-aa0d-927c1147907d.png를 사용했다. 내용물 타원 바깥은 PortafilterKnock.png의 픽셀을 복사해 동일한 몸체를 유지했다.
- Puck.png (160×80): “A single slightly cracked moist spent espresso puck, flattened thick round disk seen obliquely, transparent.” 원화 exec-22580798-19a4-4d1b-9494-6ceff76a81b8.png를 축소했다.
- PortafilterBrush.png (440×300): “Clean empty portafilter in oblique top view, a large elliptical basket opening, black handle down-left, transparent.” 원화 exec-9645584a-3717-4e00-be8d-8f25ac57e998.png를 축소했다.
- Residue.png (440×300): “Only scattered spent grounds and small clumps adhering to the inner wall, floor and inner edge of the same basket, transparent.” 원화 exec-0c937b5a-cd09-48e0-b083-d0b68d66de73.png에서 갈색 입자만 분리하고 바스켓 내부 타원 밖을 투명하게 했다. PortafilterBrush.png와 같은 중심에 겹친다.
- Brush.png (240×120): “Short light-oak espresso cleaning brush, dense charcoal bristles left and wooden handle right, transparent.” 원화 exec-732ad52a-cde9-41fc-a39d-ba7de9ac32ad.png를 축소했다.
- HintKnock.png (628×48), HintHarder.png (338×48), HintBrush.png (604×48): 각각 “포터필터를 아래로 세게 내려쳐요”, “더 세게 내려쳐요!”, “브러시로 남은 가루를 쓸어 내요”. Galmuri11을 채움 높이 약 36px로 렌더링하고 1px 굵게, 2px 크림 외곽선을 둘렀다. 채움 #380F00, 외곽선 #FCF0CA. 철자는 렌더링 결과를 확인했다.

프로젝트 밖 2배 미리보기: %TEMP%/cafe-time-preview-20261109/KnockRoomKnockHover2x.png, KnockRoomKnockContact2x.png, KnockRoomPuckFall2x.png, KnockRoomBrush2x.png. 접촉 미리보기의 포터필터 중심은 (0,-8)이며 바스켓 테두리 최저점과 막대 윗면이 이미지 y=542에서 만난다.

