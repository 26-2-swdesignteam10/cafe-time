Generated with OpenAI 내장 image_gen (image_gen.imagegen). Style reference: Assets/_Project/Art/UI/CupShelf/Panel.png (프레임), Assets/_Project/Art/Sprites/Cafe/StationEspresso.png (재질).

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

이번 창은 사용자 지정 모던 팔레트를 적용했다. 밝은 웜 그레이지 벽, 밝은 오크, 스테인리스·무광 검정, 세이지 그린 하나만 포인트. 2~3단 명암 블록, 계단식 외곽선.

기존 EspressoMachine/cafetime_shot_bg.png는 문제 확인용으로만 보았다. 생성 참조로 전달하거나 픽셀을 재사용하지 않았다. 프레임도 새로 생성했으며 CupShelf 원본 픽셀을 복사하지 않았다.

## 파일별 프롬프트 및 수정

- Panel.png: 빈 프레임·작업대, 그라인더, 머신 몸체, 그룹헤드, 트레이, 매트, 넉박스, 받침대를 각각 생성한 뒤 좌표 합성. 밝은 웜 그레이지 벽과 오크, 호두나무 프레임·금색 안쪽 선. 스테인리스·무광 검정 기기, 세이지 하나만 강조색. 배경은 글자 없음. LCD 3개는 정확한 크기의 어두운 빈 화면으로 후처리.
- PortafilterEmpty.png: 왼쪽 무광 검정 손잡이, 오른쪽 은색 바스켓인 옆모습. 빈 은색 내부. 이 생성 원본 몸체를 네 상태 공통으로 사용.
- PortafilterGround.png: 같은 몸체에 수북한 중간 갈색 가루. 생성한 Ground 상태의 마운드·알갱이 참고 후 내용 영역만 픽셀 합성.
- PortafilterTamped.png: 같은 몸체에 납작하고 매끈한 중간 갈색 압축 가루.
- PortafilterUsed.png: 같은 몸체에 짙은 찌꺼기와 균열.
- Tamper.png: 위에 검정 손잡이, 아래에 납작한 금속판. 함께 생성된 참조 기기 부분은 버리고 탬퍼만 사용.
- GrindButton.png: 세이지 베벨 버튼, 정확한 문구 “분쇄”. 주변 배경 제거·이진 알파.
- ExtractButton.png: 세이지 베벨 버튼, 정확한 문구 “추출”. 주변 작업대 조각 제거·이진 알파.
- LabelGrind.png: 정확한 문구 “1 분쇄”.
- LabelTamp.png: 정확한 문구 “2 탬핑”.
- LabelExtract.png: 정확한 문구 “3 장착 후 추출”.
- LabelKnock.png: 정확한 문구 “4 찌꺼기 털기”.
- LabelRack.png: 정확한 문구 “포터필터 받침대”.
- CheckLayout.png: 그룹헤드 포터필터(-20,90), 받침대 포터필터(290,-270), 탬퍼(-280,130), 분쇄·추출 버튼, 별도 라벨을 Panel 위에 합성. 표 영역은 반투명 파랑 사각형, 중심은 빨간 십자, 예약 영역은 주황. 반투명 표시를 불투명 바탕에 합성하여 최종 알파 255.

- 라벨 공통: 생성 글자 채움 마스크를 추출해 #380F00 채움, #FCF0CA 3px 외곽선. 채움 높이 72px, 외곽선 바깥 사방 8px 여백. 화면에서 절반 크기로 표시.
- Preview.png: 검사 사각형 없는 미리보기. ContactSheet.png: 철자·상태 비교.
- Sources/: 내장 도구 원본 20개. Components/: 합성에 사용한 분리 부품. compose.py: Pillow 합성·정규화 재현 스크립트. validate.py 및 Validation.json: 실측 검사.

## 좌표

원점은 창 중앙. 이미지 좌표=(768+x,432-y). **표에 지정된 모든 좌표를 유지했다.** 작업대 윗면 y=-80~-115, 앞면 y=-115~-190. 그라인더 x=-570, y=-90~330, 폭 240. 머신 x=150, y=130~370, 폭 700.

별도 지정이 없던 라벨의 미리보기 중심: Grind=(-570,-153), Tamp=(-280,-153), Extract=(150,-153), Knock=(600,-153), Rack=(145,-360). 넉박스 라벨만 최초 미리보기 x=620에서 x=600으로 옮겨 프레임 겹침을 피했다. **넉박스 슬롯 (620,-40)은 그대로다.**

탬퍼는 (-280,130)에 표시. 아래로 60 이동 시 이미지 아래끝 y=1이 탬핑 슬롯 윗면 y=6에 닿는다.

| 자리 | x | y | w | h |
|---|---:|---:|---:|---:|
| GrinderLCD | -570 | 150 | 190 | 60 |
| GrindButton | -570 | 70 | 170 | 60 |
| GrinderSlot | -570 | -30 | 184 | 72 |
| Grinder | -570 | 120 | 240 | 420 |
| Tamper | -280 | 130 | 92 | 138 |
| TampSlot | -280 | -30 | 184 | 72 |
| Machine | 150 | 250 | 700 | 240 |
| LCD1 | -20 | 300 | 130 | 56 |
| LCD2 | 320 | 300 | 130 | 56 |
| Extract1 | -20 | 225 | 140 | 56 |
| Extract2 | 320 | 225 | 140 | 56 |
| Head1 | -20 | 90 | 184 | 72 |
| Head2 | 320 | 90 | 184 | 72 |
| Cup1 | -20 | -40 | 130 | 130 |
| Cup2 | 320 | -40 | 130 | 130 |
| Knock | 620 | -40 | 184 | 72 |
| Rack1 | 0 | -270 | 184 | 72 |
| Rack2 | 290 | -270 | 184 | 72 |

## 실제 검사

- Panel 내부 알파 255. 투명은 바깥 프레임의 계단식 모서리에만 존재.
- 최종 투명 에셋 알파는 0/255. CheckLayout은 전체 255.
- LCD 3개 중심 픽셀 확인. 포터필터 네 상태의 바스켓 내용 영역 밖 픽셀 일치 확인.
- 라벨 5종: 채움 높이 72px, 외곽선 3px, 사방 여백 8px 실측 통과.
- 버튼 “분쇄”, “추출”과 라벨 5종을 ContactSheet 및 Preview에서 눈으로 대조해 철자 확인.

| 파일 | 크기 | 알파 |
|---|---|---|
| Panel.png | 1536×864 | [0, 255] |
| Tamper.png | 92×138 | [0, 255] |
| GrindButton.png | 170×60 | [0, 255] |
| ExtractButton.png | 140×56 | [0, 255] |
| CheckLayout.png | 1536×864 | [255] |
| PortafilterEmpty.png | 184×72 | [0, 255] |
| PortafilterGround.png | 184×72 | [0, 255] |
| PortafilterTamped.png | 184×72 | [0, 255] |
| PortafilterUsed.png | 184×72 | [0, 255] |
| LabelGrind.png | 276×94 | [0, 255] |
| LabelTamp.png | 268×94 | [0, 255] |
| LabelExtract.png | 475×94 | [0, 255] |
| LabelKnock.png | 508×94 | [0, 255] |
| LabelRack.png | 511×94 | [0, 255] |

## 생성 프롬프트 기록

아래는 도구 프롬프트 기록이다. 포터필터·버튼 항목은 결과 보존 시 요약되었으므로 위 파일별 설명에 상태·모양·문구를 별도 기록했다.

### Base

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. A blank workstation background ONLY, landscape 1536x864. Input references: CupShelf Panel for walnut frame ONLY, StationEspresso for material ONLY. Newly designed thin walnut rectangular frame 28px with stepped outer corners and fine honey-gold inner line. Inside completely opaque bright warm greige wall. NO TITLE NO TEXT NO MACHINES NO OBJECTS. Light oak counter extends entire interior width; counter top horizontal at pixel y512 through547, front fascia y547 through622; below light greige cabinet recess and a thin oak lower shelf at y746. Leave large blank wall above. No bricks, no decorative items, no panels on wall. Composition must follow those pixel heights exactly.

### Grinder

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. One standalone professional espresso grinder component, transparent background. Tall narrow width:height ratio 240:420. Fully visible black-lidded transparent bean hopper containing brown beans at top. Lower rectangular body is brushed stainless with matte charcoal sides, sage trim. Large blank metal front face reserved for LCD and button: DO NOT DRAW LCD or buttons. Central short grind chute near bottom, and an open fork below. No portafilter, no coffee cup. Orthographic straight front view, simplified 2-3 block shading, bright metal. No text. Material reference StationEspresso.

### Machine

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Standalone two-group espresso machine UPPER BODY ONLY, transparent background, wide aspect 700:240. Entire silhouette wide and rectangular, brushed stainless front, matte charcoal endcaps, thin sage accent. NO LCD NO BUTTONS NO LETTERS NO DIAL NO GROUPHEADS NO CUPS NO PORTAFILTERS. A broad blank stainless flat front reserved for separately composed controls. Short vents across very top and subtle stepped bevels; symmetrical flat front. Modern minimal pixel art. Material reference StationEspresso.

### Grouphead

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. One standalone espresso machine grouphead socket, no portafilter. Low wide rectangular stainless coupling housing, front view visible dark underside socket. Aspect184:72. Transparent. NO handle, no text.

### Tray

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. One standalone espresso machine wide drip tray with very short back support legs at both ends, stainless rim black grate. Aspect700:110, straight frontal slight top view. Two bright subtle sage square cup placement recesses spaced across top. No cups no text. Transparent.

### Rack

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. One standalone portafilter holder saddle, wide low aspect184:72. Bright sage outlined pale stainless shallow rectangular cradle on small charcoal feet, obvious pale placement center. No portafilter no text. Transparent.

### Mat

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. One standalone bright sage outlined charcoal rubber tamping mat, wide low aspect184:72, slight top view, simple flat inset highlight oval makes place to put portafilter obvious. No tools no text. Transparent.

### Tamper

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. One standalone coffee tamper, exact upright symmetrical silhouette with chunky matte black ergonomic handle ABOVE and wide flat stainless disc BELOW. Aspect92:138. No stand no coffee no text. Transparent.

### Knock

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. One standalone espresso knockbox, wide shallow aspect184:100 front view with visible top. Brushed stainless outer wall matte black interior, horizontal rubber knockbar and bright sage lip defining placement. No grounds no tools no text. Transparent.

### LabelTamp

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Typography asset ONLY. Exactly one single horizontal line Korean label VERBATIM "2 탬핑". Bold crisp square Korean pixel lettering like Galmuri11. Dark coffee #380F00 fill with thin cream #FCF0CA outline. Transparent background. No plate no button no ornament no extra text. All glyphs equal height. Clearly spell every syllable. Wide canvas fitted to text, consistent letter spacing. Final glyph height72px with 8px external padding (resized later).

### LabelExtract

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Typography asset ONLY. Exactly one single horizontal line Korean label VERBATIM "3 장착 후 추출". Bold crisp square Korean pixel lettering like Galmuri11. Dark coffee #380F00 fill with thin cream #FCF0CA outline. Transparent background. No plate no button no ornament no extra text. All glyphs equal height. Clearly spell every syllable. Wide canvas fitted to text, consistent letter spacing. Final glyph height72px with 8px external padding (resized later).

### LabelGrind

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Typography asset ONLY. Exactly one single horizontal line Korean label VERBATIM "1 분쇄". Bold crisp square Korean pixel lettering like Galmuri11. Dark coffee #380F00 fill with thin cream #FCF0CA outline. Transparent background. No plate no button no ornament no extra text. All glyphs equal height. Clearly spell every syllable. Wide canvas fitted to text, consistent letter spacing. Final glyph height72px with 8px external padding (resized later).

### LabelKnock

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Typography asset ONLY. Exactly one single horizontal line Korean label VERBATIM "4 찌꺼기 털기". Bold crisp square Korean pixel lettering like Galmuri11. Dark coffee #380F00 fill with thin cream #FCF0CA outline. Transparent background. No plate no button no ornament no extra text. All glyphs equal height. Clearly spell every syllable. Wide canvas fitted to text, consistent letter spacing. Final glyph height72px with 8px external padding (resized later).

### LabelRack

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Typography asset ONLY. Exactly one single horizontal line Korean label VERBATIM "포터필터 받침대". Bold crisp square Korean pixel lettering like Galmuri11. Dark coffee #380F00 fill with thin cream #FCF0CA outline. Transparent background. No plate no button no ornament no extra text. All glyphs equal height. Clearly spell every syllable. Wide canvas fitted to text, consistent letter spacing. Final glyph height72px with 8px external padding (resized later).

### PortafilterEmpty

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Exactly ONE side-view espresso portafilter sprite. Long matte black horizontal handle points LEFT, low stainless basket on RIGHT. Whole silhouette aspect184:72. Basket top shows EMPTY pale silver concave interior. No accessories no text. Transparent background.

### PortafilterGround

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Exactly ONE side-view espresso portafilter sprite. Long matte black horizontal handle points LEFT, low stainless basket on RIGHT. Whole silhouette aspect184:72. Basket contains a visibly HIGH MOUND of warm medium brown ground coffee, rough granular top above rim. No accessories no text. Transparent.

### PortafilterTamped

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Exactly ONE side-view espresso portafilter sprite. Long matte black horizontal handle points LEFT, low stainless basket on RIGHT. Whole silhouette aspect184:72. Basket contains a perfectly FLAT compressed medium-brown coffee disc just below rim, clearly smooth. No accessories no text. Transparent.

### PortafilterUsed

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Exactly ONE side-view espresso portafilter sprite. Long matte black horizontal handle points LEFT, low stainless basket on RIGHT. Whole silhouette aspect184:72. Basket contains very DARK nearly black used coffee puck, cracked rough wet top. No accessories no text. Transparent.

### GrindButton

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Exactly one sage-green beveled rectangular pixel-art button, aspect170:60. Large highly legible cream Korean pixel text VERBATIM "분쇄". No other letters, icon, border ornament. Transparent outside stepped corners.

### ExtractButton

Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding. For this modern espresso workstation use warm greige, light oak, stainless steel and matte black, ONLY sage green accent. 2-3 flat shade blocks, precise chunky pixel steps, no gradients, no antialiasing, no blurry painted noise. Front-facing slight top surface view. Exactly one sage-green beveled rectangular pixel-art button, aspect140:56. Large highly legible cream Korean pixel text VERBATIM "추출". No other letters, icon, border ornament. Transparent outside stepped corners.

## 작업 범위

- 요청한 scratchpad/shot_ui에만 저장. 저장소 파일·Unity YAML·.meta·ADR은 변경하지 않았다.
- 내장 image_gen 사용. CLI/API 대체 생성 없음. Python/Pillow는 요청된 부품 합성·크기·알파·라벨 정규화·검사에 사용.
- Unity 임포트는 수행하지 않았다. 추후 적용 시 Sprite, Point, Mip Map 끔, Alpha Is Transparency 켬. 라벨 게임 표시 크기는 PNG의 절반.
