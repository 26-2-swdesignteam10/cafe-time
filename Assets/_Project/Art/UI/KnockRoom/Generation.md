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

## 2026-11-09 Puck 원기둥 형태 수정

- 도구: OpenAI image_gen.imagegen. 스타일·재질 참조: `Puck.png`, `PortafilterKnock.png`, `Residue.png`.
- `Puck.png` (400×260): 바스켓에서 빠져나온 짙은 에스프레소 퍽을 비스듬한 옆 시점으로 표현했다. 넓은 타원형 윗면과 높이가 분명한 옆면을 가진 원기둥, 거의 검정에 가까운 커피 갈색, 약한 젖은 광택, 미세한 입자와 가는 균열을 사용했다. AI 원화는 투명 배경으로 생성했다.
- 후처리: 원화의 저알파 배경 헤이즈를 제거하고 본체를 잘라 380×240으로 맞춘 뒤, 투명 캔버스 중앙에 10px 여백으로 배치했다.
- 실제 표시 크기 기준 미리보기: `KnockRoomPuckFall2x.png` (200×130 표시의 2배).

## 2026-11-09 커피 퍽 짙은 원기둥 재작화

- 도구: OpenAI 내장 `image_gen.imagegen` 편집 1회. 크기 조정·미리보기 합성·검증에는 프로젝트 밖 Python/Pillow를 사용했다.
- 스타일 참조: `../ShotRoom/Panel.png`, `../ShotRoom/PortafilterFrontUsed.png`. 형태·재질 참조: 이 폴더의 기존 `Puck.png`, `PortafilterKnock.png`, `Residue.png`. ART_STYLE.md 1장의 기기 작업 창 예외에 따라 도트 격자 없는 부드러운 일러스트로 제작했다.
- `Puck.png` 프롬프트: “Redraw the single isolated spent espresso coffee puck. Premium clean hand-painted smooth 2D illustration, soft upper-left light, coherent material shading, no pixel-art grid. One solid compact cylindrical plug of wet coffee grounds fallen intact from the basket; broad elliptical flat top visible at an oblique side angle, substantial vertical side wall about 40 percent of projected height. Fine organic granular texture, delicate hairline cracks, restrained damp glints. Much darker than the original: dominant near-black espresso brown #1A0F0A to #2B1A12, side nearly black, subtle tonal modeling, no bright brown or orange highlights. Centered on a 400:260 transparent canvas, nearly full width with about 10px margins. No ground, cast shadow, halo, loose objects, text or watermark. Readable at 200×130.”
- 후처리: 생성 원화(1555×1011)의 알파 1 이하인 배경 헤이즈만 투명 처리하고, 알파 경계 `(97,53)-(1445,949)`를 잘라 Lanczos로 380×240에 맞췄다. 400×260 RGBA 캔버스의 `(10,10)`에 중앙 배치했다. 별도 색상 보정은 하지 않았다. 최종 알파 범위 0~255, 본체 경계 `(10,10)-(390,250)`를 확인했다. `.meta`와 코드·씬·프리팹은 수정하지 않았다.
- 실제 표시 크기 미리보기: `%TEMP%/cafe-time-illustration-preview/PuckActualSize.png` (200×130), `KnockRoomPuckActualSize.png` (1536×864 패널에 퍽을 200×130으로 합성). 축소는 Bilinear로 확인했다.