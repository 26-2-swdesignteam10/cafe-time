# 주인공 대화 초상화 생성 기록

## 2026-11-19 캐릭터 개편

도구: OpenAI 내장 `image_gen`(투명 배경 생성·동일 인물 표정 편집), Python 3 + Pillow(사용자가 요청한 nearest-neighbor 축소·프레임 분리·발 정렬·반전·알파 정리·미리보기). API/CLI는 사용하지 않았다.

공통 시작 프롬프트: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

투명 배경은 알파 0, RGB (0,0,0)으로 정리했고, 최종 PNG의 알파는 0/255다. 글자·효과선·반짝이·체커보드·바닥 그림자·가구를 넣지 않았다. 당황 초상화의 요청된 땀방울은 유지했다. 아래 bbox는 알파 > 0의 `(left, top, right, bottom)`이며 오른쪽·아래쪽은 미포함, 원점은 왼쪽 위다.

스타일 참조(이미지 생성 입력): `Art/UI/Ending/Ending04.png`, `Ending02.png`(따뜻한 주인공 디자인), scratch의 `7.png`, `8.png`(초상화 화풍). 엔딩 그림의 여러 인물 중 사용자가 명시한 아래 디자인을 우선했다.

인물 공통 제작 지시: 긴 웨이브 진갈색 머리(어깨 아래), 반묶음과 큰 크림 리본, 앞머리, 하이라이트가 있는 큰 갈색 눈과 볼터치, 둥근 깃·퍼프 소매 크림 블라우스, 올리브 그린 멜빵 앞치마 원피스. 예쁜 둥근 얼굴, 세련된 얼굴 비율·눈 하이라이트·머리 결. 부드러운 고해상도 도트와 따뜻한 갈색 계단식 외곽선. 가슴 위 버스트, 투명 배경, 효과선 없음. 기본 초상화를 먼저 생성하고 이를 직접 입력하여 나머지 표정만 변경했다.

최종 5장 모두 **300×420 RGBA**. 같은 캔버스·크롭 기준으로 머리 위치를 통일하고 사방 최소 8px 여백을 확보했다.

### 파일별 프롬프트·제작 지시와 최종 측정

| 파일 | 최종 크기(px) | 불투명 영역 bbox | 파일별 프롬프트 / 제작 지시 |
|---|---|---|---|
| Default.png | 300×420 | (8, 8, 292, 401) | 동일한 기본 초상화의 인물·머리 위치·리본·의상을 유지. 기본 미소, 반짝이는 갈색 눈. |
| Happy.png | 300×420 | (8, 8, 292, 400) | 동일한 기본 초상화의 인물·머리 위치·리본·의상을 유지. 눈 감고 활짝 웃음, 열린 입과 즐거운 볼터치. |
| Determined.png | 300×420 | (8, 8, 292, 400) | 동일한 기본 초상화의 인물·머리 위치·리본·의상을 유지. 집중한 눈썹·결의에 찬 눈·자신 있는 미소, 가슴 높이의 한쪽 주먹. |
| Embarrassed.png | 300×420 | (8, 8, 292, 400) | 동일한 기본 초상화의 인물·머리 위치·리본·의상을 유지. 강하게 붉어진 볼·당황한 눈썹·작게 열린 입, 이마 옆 땀방울 1개. |
| Worried.png | 300×420 | (8, 8, 292, 400) | 동일한 기본 초상화의 인물·머리 위치·리본·의상을 유지. 안쪽이 올라가고 바깥으로 내려간 걱정 눈썹, 걱정스러운 눈과 작은 입. |

검수: 동일 인물·화풍·잘림을 정적 미리보기로 육안 확인. RGBA·투명 RGB 0·발바닥 y=187·좌우 반전·걷기 두 발의 차이·머리 고정 픽셀을 자동 검사했다. scratch의 `PortraitsComparison.png`, `PlayerWalkPreview.png`, `Customer1WalkPreview.png`~`Customer4WalkPreview.png`, `AllCharactersWalkPreview.png`, `AllWalkAnimation.gif`, `SeatingPreview.png`로 확인할 수 있다. 새 PNG의 .meta는 생성하지 않았으며 Unity 임포트와 실제 UI/Play 확인은 별도 필요하다.


## 2026-11-19 초상화 치비 재작업

도구: OpenAI 내장 image_gen(각 파일별 투명 배경 생성/표정 편집), Python 3 + Pillow(최종 크기 nearest-neighbor 축소, 공통 프레임 정렬, 알파 정리, 비교 미리보기와 검증). API/CLI 미사용.

스타일 참조(모든 생성 호출에 직접 입력): 작업 폴더의 7.png, 8.png 및 Assets/_Project/Art/Sprites/Player/PlayerDown0.png. 표정 편집은 새 치비 Default 생성 원본을 추가 입력했다. 5장 모두 동일 원본 좌표 (18,94,1034,1383)를 284×360으로 축소하여 300×420 캔버스 (8,28)에 배치했다.

큰 둥근 머리·짧은 몸통·큰 반짝이는 눈·큰 머리카락 덩어리·굵은 갈색 도트 외곽선으로 단순화. 생성 알파를 128 기준 0/255로 정리하고, 알파 0의 RGB는 (0,0,0)으로 저장했다. 아래 bbox는 알파 > 0의 (left,top,right,bottom), 오른쪽·아래쪽 미포함.

| 파일 | 최종 크기(px) | bbox |
|---|---|---|
| Default.png | 300×420 RGBA | (8, 28, 291, 387) |
| Happy.png | 300×420 RGBA | (8, 28, 291, 388) |
| Determined.png | 300×420 RGBA | (8, 28, 291, 387) |
| Embarrassed.png | 300×420 RGBA | (8, 28, 292, 387) |
| Worried.png | 300×420 RGBA | (8, 29, 292, 388) |

### 파일별 실제 생성 프롬프트

#### Default.png

Use case: stylized-concept. Production transparent pixel-art portrait for a cozy Korean cafe game. Style references 7.png and 8.png: copy ONLY the simple casual chibi character rendering, never the background or UI. PlayerDown0.png supplies heroine identity and palette. MUCH more chibi and simpler than an elegant anime illustration: enormous round head, plump round cheeks, very short tiny torso, no long neck. Head including crown to chin occupies at least 60% of portrait height. Big sparkling eyes with just two clear highlights; tiny nose and mouth, flat peach blush. Bold clean warm dark-brown stepped pixel outlines, approximately 3-4px at final 300px width. Crisp clean pixel clusters, only base color plus one shadow per material, no gradients, no intricate strand shading, no fine hatching, no skin rendering. Hair is 5-7 large smooth chunky locks. Transparent actual alpha, no text, no effects, no sparkles outside eyes, no ground shadow, no checkerboard, no frame. Front-facing chest-up bust, complete hair and ribbon and shoulders with safe padding, lower torso cropped flat. Same uniform style for all nine portraits. Subject: beautiful cute young cafe heroine, long wavy DARK brown hair in half-up hairstyle, simple large cream bow atop/back of head, cream blouse with very simple rounded collar and short simplified puff sleeves, olive-green apron straps and bib. Match PlayerDown0 design exactly, not elaborate braid/curls or lace. Default expression: friendly small closed-mouth smile, big open brown eyes. Vertical 5:7 canvas intended for 300x420. Round hair/head silhouette centered horizontally, top 8%, chin around 67%, short shoulders and apron below. The face must look like a casual chibi game mascot.

#### Happy.png

Use case: stylized-concept. Production transparent pixel-art portrait for a cozy Korean cafe game. Style references 7.png and 8.png: copy ONLY the simple casual chibi character rendering, never the background or UI. PlayerDown0.png supplies heroine identity and palette. MUCH more chibi and simpler than an elegant anime illustration: enormous round head, plump round cheeks, very short tiny torso, no long neck. Head including crown to chin occupies at least 60% of portrait height. Big sparkling eyes with just two clear highlights; tiny nose and mouth, flat peach blush. Bold clean warm dark-brown stepped pixel outlines, approximately 3-4px at final 300px width. Crisp clean pixel clusters, only base color plus one shadow per material, no gradients, no intricate strand shading, no fine hatching, no skin rendering. Hair is 5-7 large smooth chunky locks. Transparent actual alpha, no text, no effects, no sparkles outside eyes, no ground shadow, no checkerboard, no frame. Front-facing chest-up bust, complete hair and ribbon and shoulders with safe padding, lower torso cropped flat. Same uniform style for all nine portraits. EDIT TARGET is image 1 the NEW CHIBI DEFAULT heroine. Change ONLY facial expression: Eyes closed into joyful upward arcs, mouth wide open laughing happily, rosy cheeks. Keep identical head silhouette, size, face position, hair, cream bow, outfit, bust framing, pixel thickness and canvas proportions. Do not copy UI from 7/8. Player design remains unchanged.

#### Determined.png

Use case: stylized-concept. Production transparent pixel-art portrait for a cozy Korean cafe game. Style references 7.png and 8.png: copy ONLY the simple casual chibi character rendering, never the background or UI. PlayerDown0.png supplies heroine identity and palette. MUCH more chibi and simpler than an elegant anime illustration: enormous round head, plump round cheeks, very short tiny torso, no long neck. Head including crown to chin occupies at least 60% of portrait height. Big sparkling eyes with just two clear highlights; tiny nose and mouth, flat peach blush. Bold clean warm dark-brown stepped pixel outlines, approximately 3-4px at final 300px width. Crisp clean pixel clusters, only base color plus one shadow per material, no gradients, no intricate strand shading, no fine hatching, no skin rendering. Hair is 5-7 large smooth chunky locks. Transparent actual alpha, no text, no effects, no sparkles outside eyes, no ground shadow, no checkerboard, no frame. Front-facing chest-up bust, complete hair and ribbon and shoulders with safe padding, lower torso cropped flat. Same uniform style for all nine portraits. EDIT TARGET is image 1 the NEW CHIBI DEFAULT heroine. Change ONLY expression and one hand: Determined eyebrows, confident smile, big open eyes. One tiny clenched fist raised beside chest, well below chin, without obscuring face or shifting head. Keep identical head silhouette, size, face position, hair, cream bow, outfit, bust framing, pixel thickness and canvas proportions. Do not copy UI from 7/8. Player design remains unchanged.

#### Embarrassed.png

Use case: stylized-concept. Production transparent pixel-art portrait for a cozy Korean cafe game. Style references 7.png and 8.png: copy ONLY the simple casual chibi character rendering, never the background or UI. PlayerDown0.png supplies heroine identity and palette. MUCH more chibi and simpler than an elegant anime illustration: enormous round head, plump round cheeks, very short tiny torso, no long neck. Head including crown to chin occupies at least 60% of portrait height. Big sparkling eyes with just two clear highlights; tiny nose and mouth, flat peach blush. Bold clean warm dark-brown stepped pixel outlines, approximately 3-4px at final 300px width. Crisp clean pixel clusters, only base color plus one shadow per material, no gradients, no intricate strand shading, no fine hatching, no skin rendering. Hair is 5-7 large smooth chunky locks. Transparent actual alpha, no text, no effects, no sparkles outside eyes, no ground shadow, no checkerboard, no frame. Front-facing chest-up bust, complete hair and ribbon and shoulders with safe padding, lower torso cropped flat. Same uniform style for all nine portraits. EDIT TARGET is image 1 the NEW CHIBI DEFAULT heroine. Change ONLY facial expression: Strong flat pink blush on cheeks, flustered raised eyebrows, big open eyes, small surprised open mouth. Exactly one small sweat drop by temple allowed. Keep identical head silhouette, size, face position, hair, cream bow, outfit, bust framing, pixel thickness and canvas proportions. Do not copy UI from 7/8. Player design remains unchanged.

#### Worried.png

Use case: stylized-concept. Production transparent pixel-art portrait for a cozy Korean cafe game. Style references 7.png and 8.png: copy ONLY the simple casual chibi character rendering, never the background or UI. PlayerDown0.png supplies heroine identity and palette. MUCH more chibi and simpler than an elegant anime illustration: enormous round head, plump round cheeks, very short tiny torso, no long neck. Head including crown to chin occupies at least 60% of portrait height. Big sparkling eyes with just two clear highlights; tiny nose and mouth, flat peach blush. Bold clean warm dark-brown stepped pixel outlines, approximately 3-4px at final 300px width. Crisp clean pixel clusters, only base color plus one shadow per material, no gradients, no intricate strand shading, no fine hatching, no skin rendering. Hair is 5-7 large smooth chunky locks. Transparent actual alpha, no text, no effects, no sparkles outside eyes, no ground shadow, no checkerboard, no frame. Front-facing chest-up bust, complete hair and ribbon and shoulders with safe padding, lower torso cropped flat. Same uniform style for all nine portraits. EDIT TARGET is image 1 the NEW CHIBI DEFAULT heroine. Change ONLY facial expression: Inner eyebrows raised in worry, big worried open eyes, small downturned mouth. No tears or sweat. Keep identical head silhouette, size, face position, hair, cream bow, outfit, bust framing, pixel thickness and canvas proportions. Do not copy UI from 7/8. Player design remains unchanged.

검수: 9장 전후 비교 PortraitsChibi.png를 작업 폴더에 저장. 최종 크기·RGBA·알파 0의 RGB 0·최소 8px 외곽 여백을 자동 검사. 원본 이름으로 덮어썼고 .meta 및 코드·씬·프리팹·에셋 파일은 수정하지 않았다. Unity에서 기존 스프라이트가 재임포트된 뒤 대화창/손님 UI의 표시 확인이 필요하다.

최종 육안 확인: PortraitsChibi.png를 직접 열어 이전 대비 큰 머리·짧은 몸·단순한 명암, 5개 표정의 구분, 9장 화풍과 외곽선의 일관성, 머리·리본·어깨 잘림 없음을 확인했다.


## 2026-11-19 첫 개편 버전으로 복원

사용자 피드백: 치비로 재작업한 주인공 대화창 초상화가 "너무너무 캐주얼하다. 그 전이 낫다". 주인공 5장만 긴 웨이브 머리·큰 크림 리본·퍼프 소매 블라우스·올리브 앞치마의 디테일한 첫 개편 일러스트로 복원했다.

복원 방법: 새 이미지 생성 없이 아래 작업 폴더의 `sources.json`과 `process_assets.py`를 사용했다. `clean`·`portrait` 함수만 그대로 재사용하여 알파 128 기준 0/255 정리 → 불투명 영역 자르기 → nearest-neighbor 비율 유지 축소 → 300×420 RGBA 캔버스에 가로 중앙·상단 8px 정렬 → 투명 픽셀 RGB 0 정리를 적용했다.

- 작업 폴더: `C:/Users/k9425/AppData/Local/Temp/claude/bd37fc10698b196c/3c877158-9d17-4156-bff5-e3db1a17f799/scratchpad/chars/`
- 원본 생성 이미지 경로:
  - `Default.png`: `C:/Users/k9425/.codex/generated_images/01a1f195-8f50-7713-ad5f-c2691b714b89/exec-d9644105-b6a7-4ef2-811e-305b374f508d.png`
  - `Happy.png`: `C:/Users/k9425/.codex/generated_images/01a1f195-8f50-7713-ad5f-c2691b714b89/exec-967a6a46-badd-4886-b34a-bbf024fa3a4f.png`
  - `Determined.png`: `C:/Users/k9425/.codex/generated_images/01a1f195-8f50-7713-ad5f-c2691b714b89/exec-dfe4a3b0-05d1-416b-ae7d-46d1ebd976f4.png`
  - `Embarrassed.png`: `C:/Users/k9425/.codex/generated_images/01a1f195-8f50-7713-ad5f-c2691b714b89/exec-6f11d932-7fde-489c-93e9-11b72e2e41a1.png`
  - `Worried.png`: `C:/Users/k9425/.codex/generated_images/01a1f195-8f50-7713-ad5f-c2691b714b89/exec-05851b38-1359-42e1-8924-f73001f37831.png`

검증: 5장 모두 당시 `asset_manifest.json`의 SHA-256 및 bbox와 일치한다. `PortraitsComparison.png` 윗줄과 복원 미리보기의 픽셀도 완전히 일치한다. 직접 열어 긴 웨이브 머리·리본·퍼프 소매·앞치마 및 표정 5종과 정렬을 확인했다. 미리보기는 위 작업 폴더의 `Restored.png`에 저장했다.

앞선 **「2026-11-19 초상화 치비 재작업」 절은 주인공 5장에 대해 폐기됨**. 해당 절은 작업 이력으로만 보존하며 현재 주인공 에셋에는 적용하지 않는다. 손님 초상화(`Customer1Portrait.png`~`Customer4Portrait.png`)는 기존 치비 버전을 유지했다. C#·.meta·씬·.asset 및 다른 에셋은 변경하지 않았다.
