Generated with OpenAI 내장 ImageGen 도구. Style reference: `../Pos/Panel.png`, `../Stations/PickupPanel.png`, `../Stations/PickupSlot.png`, `../Pos/PayButton.png`, `../Pos/ConfirmButton.png`, `../Pos/DeleteButton.png`, `../CafeHud/ToastNoCustomer.png`, `../Quit/Message.png`, `../../Icons/Items/HotCoffee.png`, `../../Sprites/Cafe/StationEspresso.png`.

Prompt set: Production Unity sprites for a cozy Korean pixel art cafe game. Crisp stepped pixel outlines; warm walnut, honey gold and cream palette; transparent backgrounds; fully visible edges with padding.

- Hint.png: 단일 행 굵은 도트 글자 전용 라벨, 다크 커피 채움과 크림 외곽선. 정확한 문구: "스쿱을 얼음통에 끌어 담고, 손 칸으로 끌어 오세요".
- IceBin.png: 정면/위에서 본 얼음이 가득 찬 스테인리스 얼음통.
- LabelIce.png: 단일 행 굵은 도트 글자 전용 라벨, 다크 커피 채움과 크림 외곽선. 정확한 문구: "얼음".
- LabelScoop.png: 단일 행 굵은 도트 글자 전용 라벨, 다크 커피 채움과 크림 외곽선. 정확한 문구: "스쿱".
- LabelSpeed.png: 단일 행 굵은 도트 글자 전용 라벨, 다크 커피 채움과 크림 외곽선. 정확한 문구: "제빙 속도".
- Panel.png: 호두나무 프레임과 금색 안쪽 선. 상단 중앙 제목과 커피콩 두 개. 나머지 내부는 빈 불투명 크림색. 정확한 문구: "제빙기".
- ScoopEmpty.png: 빈 스테인리스 스쿱, 오른쪽 손잡이.
- ScoopIce.png: 동일한 배치의 스쿱에 얼음 더미를 담은 상태.
- ScoopSpot.png: 스쿱이 없는 빈 벽걸이 받침대.


## 최종 후처리 및 검수 (2026-11-04)

- 도구: Python + Pillow. NEAREST 크기 조정, 알파 노이즈 정리, 투명 여백 보정. .meta는 생성·수정하지 않았다.
- 라벨: 원본 채움 마스크를 추출해 S=56px, M=72px, L=96px로 정규화했다. 채움/외곽선은 #380F00/#FCF0CA 단색, 외곽선 3px, 사방 투명 여백 8px이다. Ending은 LIGHT 반전색. 줄바꿈 없이 기존 단일 행을 유지했다.
- 패널 실루엣 안의 알파를 255로 채우고 빈 내부를 #FAEAC2로 평탄화했다. 바깥은 투명. 제목과 커피콩을 유지했다.
- 스쿱 두 상태의 금속 부분과 오른쪽 손잡이 배치를 맞추고 옆으로 누운 비율을 보정했다.
- 이미지의 문구를 글자별로 대조해 철자·공백·문장부호를 눈으로 검수했다. 글자가 없는 파일은 해당 없음. 아래 높이는 외곽선을 제외한 채움 영역 경계 측정값.

| 파일 | 최종 PNG 크기 | 라벨 글자 높이 | 철자 확인 |
|---|---|---|---|
| Hint.png | 946×78 | 56px | 예 |
| IceBin.png | 1040×680 | — | 해당 없음 |
| LabelIce.png | 132×78 | 56px | 예 |
| LabelScoop.png | 133×78 | 56px | 예 |
| LabelSpeed.png | 258×78 | 56px | 예 |
| Panel.png | 2200×1400 | — | 예 |
| ScoopEmpty.png | 340×220 | — | 해당 없음 |
| ScoopIce.png | 340×220 | — | 해당 없음 |
| ScoopSpot.png | 400×340 | — | 해당 없음 |
