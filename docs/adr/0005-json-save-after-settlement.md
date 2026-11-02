# ADR-0005: 진행 데이터는 JSON 파일 하나에, 하루 정산 후에만 저장한다

- 상태: 승인
- 날짜: 2026-10-28
- 관련 명세: SFR-002, SFR-071

## 맥락

명세(SFR-071)는 "하루 정산이 끝나면 자동 저장, 영업 도중 종료하면 그날 영업은 저장되지 않음"을 요구한다.
저장 슬롯은 하나이며, 이어하기 화면에 진행 DAY와 보유 자금을 보여야 한다(SFR-002).

## 결정

- `SaveData` (직렬화 클래스)를 `JsonUtility` 로 `Application.persistentDataPath/save.json` 에 저장한다.
- 플레이 중 데이터는 `SaveSystem.Current` 에 보관하고, **하루 정산 화면에서만** `SaveSystem.SaveCurrent()` 를 호출한다.
- 처음부터 시작하면 기존 파일을 지우고 새 데이터로 시작한다.
- 설정(볼륨 등)은 진행 데이터와 분리해 `PlayerPrefs` 에 저장한다 (`GameSettings`).

## 검토한 대안

- PlayerPrefs에 진행 데이터 저장: 구조가 있는 데이터(재고 목록 등)를 다루기 어렵고, Windows에서는 레지스트리에 저장되어 확인·삭제가 불편하다.
- 영업 중 수시 저장: 명세와 맞지 않는다.

## 결과

- `JsonUtility` 는 `Dictionary` 를 직렬화하지 못하므로, 목록형 데이터는 `[Serializable]` 클래스의 `List` 로 저장한다.
- `SaveData` 에 필드를 추가할 때는 기본값을 지정해 이전 저장 파일도 읽히게 한다.
- 영업 중에는 `SaveSystem.Current` 를 직접 바꾸되, 영업 도중 메인 메뉴로 나가면 저장하지 않으므로 다음 이어하기 때 파일에서 다시 읽는다.
