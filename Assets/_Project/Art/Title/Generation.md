Generated with Python / Pillow (Jua 조판), local material restoration, and built-in ImageGen where noted. Style reference: existing sprites in this folder.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- `TitleSignText.png`: 내장 ImageGen으로 글자 없는 중앙 종이 바탕을 복원한 후 그 영역만 원본에 합성했다. 원래 목재·로프·잎·커피잔·커피콩·금선·알파는 보존했다. Jua의 짙은 갈색 채움과 크림·골드 이중 외곽선으로 간판 분위기를 유지했다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| MenuHowTo.png | 게임 설명 | 197px | #380F00 | 없음 | 주변 재질 표본으로 글자 영역 국소 복원 + Pillow Jua 조판 | 2119×742 |
| MenuQuit.png | 게임 종료 | 207px | #380F00 | 없음 | 주변 재질 표본으로 글자 영역 국소 복원 + Pillow Jua 조판 | 2119×742 |
| MenuRanking.png | 랭킹 | 206px | #380F00 | 없음 | 주변 재질 표본으로 글자 영역 국소 복원 + Pillow Jua 조판 | 2119×742 |
| MenuSettings.png | 설정 | 222px | #380F00 | 없음 | 주변 재질 표본으로 글자 영역 국소 복원 + Pillow Jua 조판 | 2117×743 |
| MenuStart.png | 게임 시작 | 205px | #380F00 | 없음 | 주변 재질 표본으로 글자 영역 국소 복원 + Pillow Jua 조판 | 2119×742 |
| TitleSignText.png | 카페타임 | 188px | #50240E | #FCF0CA 9px + #B58449 13px | ImageGen 중앙 바탕 인페인트 + Pillow Jua 조판 | 2172×724 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
- 내장 ImageGen 편집 프롬프트 요약: “Remove ONLY the existing central Korean letters and their outlines; restore continuous matching parchment/sage material. Preserve original silhouette, framing, ornaments, highlights, layout and transparent exterior. No new text; blank base for exact Jua typesetting.”

- `TitleSignText.png` 내장 ImageGen 실제 편집 프롬프트: “Use case: precise-object-edit. Create a text-free restoration of this exact existing Korean cafe game sign sprite. Remove ONLY the five large central Korean letters 카페타임 and their cream/gold outlines. Inpaint the erased letter area as continuous warm ivory parchment matching surrounding subtle texture and shading. Keep entire sign silhouette, position, canvas aspect ratio, wood borders, hanging straps, ivy, coffee cup at top, coffee beans, leaves, sparkles and gold divider beneath text unchanged. Preserve genuine transparent exterior. NO new text, no extra details. This is a blank base for exact Jua font composition later.”
- 인페인트 바탕 원본: `C:/Users/k9425/.codex/generated_images/01a1cecf-f5f9-7c41-8144-696f513874a7/exec-f0056cc3-085e-433c-b3c0-d863b1ec27f7.png`. 최종 PNG에는 필요한 글자 영역만 사용했다.
