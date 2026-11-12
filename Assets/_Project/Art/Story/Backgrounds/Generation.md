Generated with Python / Pillow (Jua 조판), local material restoration, and built-in ImageGen where noted. Style reference: existing sprites in this folder.


## 2026-11-12 Jua 재조판 (ADR-0026)

- 도구: Python / Pillow의 Jua-Regular.ttf 조판(4배 안티앨리어싱, Lanczos 축소). 바탕이 필요한 파일은 주변 재질 표본 복원 또는 NumPy 확산 인페인트를 사용했다. 글리프를 1px 굵게 만들거나 최근접 보간하는 도트 처리는 하지 않았다.
- 스타일 참조: 이 폴더의 작업 직전 PNG, `Assets/_Project/Art/Fonts/Jua-Regular.ttf`, `docs/ART_STYLE.md` 4장, 승인 ADR-0026. 기존 Galmuri11 조판·AI 글자 설명은 이 절로 대체하며, 과거 기록은 삭제하지 않았다.
- 문구·공백·문장부호는 작업 직전 실제 PNG와 생성 기록을 대조했다. 고정 문구만 재조판하고 가변 게임 값은 새로 넣지 않았다. 글자 없는 그림·아이콘·이미 제거된 패널 제목은 그대로 보존했다.
- Jua에 없는 가운뎃점(`·`)은 같은 Jua의 마침표 글리프를 가운데 높이에 배치했다. 다른 폰트의 글리프를 섞지 않았다.
- 뉴스 3장의 DAY·주가 숫자는 원래 스토리 그림에 고정된 문구를 그대로 다시 조판했다. 게임 HUD의 동적 DAY·금액·재고 값을 새로 굽지 않았다. 캐릭터·지도·전쟁 삽화·차트·화살표·프레임은 보존했다.

| 파일 | 고정 문구 | 채움 높이 | 채움색 | 외곽선 | 처리 방식 | 캔버스 (전 → 후) |
|---|---|---:|---|---|---|---|
| NewsBackground.png | 뉴스 속보 | 54px | #FFF9ED | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 주식시장 | 52px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 폭락 | 52px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 종합지수 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 코스피 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 코스닥 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 2,148.32 | 28px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | -8.76% | 26px | #F54D62 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 2,876.41 | 27px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | -9.12% | 28px | #F54D62 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 738.59 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | -10.34% | 23px | #F54D62 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | 전쟁 발발··· 글로벌 증시 | 66px | #142051 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| NewsBackground.png | '대폭락' | 66px | #B3233C | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 뉴스 속보 | 54px | #FFF9ED | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 주식시장 동향 | 44px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 불안정 | 48px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 종합지수 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 코스피 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 코스닥 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 2,165.08 | 28px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | -3.94% | 27px | #F54D62 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 2,902.76 | 26px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | -4.21% | 28px | #F54D62 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 745.18 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | -5.67% | 26px | #F54D62 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | DAY 5 | 34px | #152753 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 해외 전쟁 상황 지속 | 24px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 해외 전쟁 장기화로 불확실성 지속··· | 55px | #142051 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News2.png | 증시 변동성 확대 | 55px | #B3233C | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 뉴스 속보 | 54px | #FFF9ED | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 주식시장 동향 | 44px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 회복세 | 48px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 종합지수 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 코스피 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 코스닥 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 2,482.31 | 28px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | +2.84% | 26px | #86EF27 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 3,116.42 | 29px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | +2.15% | 28px | #86EF27 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 812.67 | 32px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | +3.48% | 26px | #86EF27 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | DAY 14 | 32px | #152753 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 해외 전쟁 협상 국면 | 23px | #FFFFFF | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 휴전 협상 진전··· | 60px | #142051 | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |
| News3.png | 증시 회복 기대감 확대 | 60px | #B3233C | 없음 | 주변 픽셀 확산 인페인트로 옛 글자·외곽선 마스크 복원 + Pillow Jua 조판 | 1920×1080 |

- 검수: 완성 PNG에서 한글 철자·공백·문장부호와 잘림을 눈으로 확인했다. 그림 포함 PNG는 지정 글자 영역 밖의 RGBA가 원본과 일치하고 알파·캔버스 크기가 동일한지 검사했다. 투명 라벨의 높이는 모두 유지했다. `.meta`, 코드, 씬, 프리팹, `.asset`, `docs/`는 수정하지 않았다. Unity에서 재임포트한 뒤 실제 표시 크기의 가독성과 라벨 폭 증가에 따른 배치를 확인해야 한다.
