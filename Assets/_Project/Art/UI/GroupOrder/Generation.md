# 단체 주문 이벤트 UI (ADR-0042)

## 2026-11-19

- 도구: Python / Pillow + `Assets/_Project/Art/Fonts/Jua-Regular.ttf` 조판. AI 이미지 생성은 쓰지 않았다.
- 스타일 참조: `Assets/_Project/Art/UI/Quit/Yes.png`, `No.png` (대화창 패널은 `Quit/Panel.png` 를 그대로 쓴다), 기존 라벨 규격(docs/ART_STYLE.md 4장).

| 파일 | 문구 | 만든 방법 | 크기 |
|---|---|---|---|
| AcceptButton.png | 수락 | `Quit/Yes.png` 의 아이콘·글자 영역(x 330~1420, y 225~520)을 줄마다 깨끗한 크림색(x 285~320 중앙값)으로 덮고, Jua 170px `#380F00` 로 가운데 조판. 오른쪽 잎 장식은 유지 | 2060×763 |
| DeclineButton.png | 거절 | `Quit/No.png` 에 같은 방법(x 330~1640) | 2060×763 |
| LabelGroupOrder.png | 단체 주문 | 투명 라벨. 채움 `#380F00` 50px, `#FCF0CA` 외곽선 3px, 여백 8px | 228×72 |

- 검수: 완성 PNG에서 한글 철자와 잘림을 눈으로 확인했다. 원본 Quit 그림은 수정하지 않았다.
- 임포트: HUD 도트 그림이라 `CafeUiBuilder` 가 Point 필터로 임포트한다.
