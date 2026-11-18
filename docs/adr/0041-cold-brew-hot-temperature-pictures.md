# ADR-0041: 콜드브루 HOT 추가, 포스기 카드 그림을 HOT/ICE 에 맞춰 바꾼다

- 상태: 승인
- 날짜: 2026-11-18
- 관련 명세: SFR-026, SFR-044~051

## 맥락

2026-11-18 사용자 요청:

- 콜드브루·콜드브루라떼도 HOT/ICE 를 고를 수 있게 한다 (ADR-0038 에서는 ICE 고정이었다).
- 포스기 커피 카드가 온도와 상관없이 한 가지 그림만 보여 준다. HOT/ICE 를 바꿀 때마다 그 온도에 맞는 그림으로 바꾼다. 그림이 없으면 Codex 가 그린다.

사용자가 질문에 답해 정한 것:

- 콜드브루 HOT = 콜드브루 원액 + **뜨거운 물**, 콜드브루라떼 HOT = 콜드브루 원액 + **우유** (얼음 없음)
- 바닐라라떼·헤이즐넛라떼처럼 카페라떼 그림을 같이 쓰던 메뉴도 **메뉴마다 따로** 그린다

## 결정

- `cold_brew`, `cold_brew_latte` 를 HOT·ICE 모두 가능으로 바꾸고 HOT 레시피를 추가한다 (기존 `GameBalance.asset` 은 `EnsureCatalog` 가 새 레시피를 채운다). ADR-0038 의 "콜드브루·콜드브루라떼는 ICE 고정" 부분을 대체한다.
- 메뉴 그림 표 `CafeUiBuilder.MenuPictures` 를 `(id, hot, ice)` 로 바꾼다. `ItemIcons.ForMenu` 는 온도에 맞는 그림을 고르고, 그 온도 그림이 없으면 다른 온도 그림을 쓴다. 손 칸·픽업대의 완성 음료도 같은 규칙을 따른다.
- `TemperatureSelector.Changed` 이벤트로 포스기가 카드 그림을 다시 그린다. 온도가 고정된 메뉴는 고정 온도의 그림이다.
- 새 그림: `ColdBrewHot`, `ColdBrewLatteHot`, `VanillaLatteHot/Ice`, `HazelnutLatteHot/Ice` (Art/UI/DrinkResult, 판정 결과 화면과 레시피북도 같이 쓴다).

## 검토한 대안

- 콜드브루 HOT 을 콜드브루 원액 + 뜨거운 우유로: 콜드브루라떼와 재료 조합이 같아져 판정이 구분하지 못한다.
- 바닐라·헤이즐넛은 카페라떼 그림 재사용: 사용자가 메뉴마다 따로 그리기를 골랐다.

## 결과

- 커피 9종 모두 HOT/ICE 선택이 된다.
- 그림이 하나뿐인 메뉴(음료·스무디·차)는 표에서 다른 온도 칸을 비워 둔다(null).
- 새 메뉴를 추가할 때는 `MenuPictures` 와 판정 결과 표(`StationUiBuilder`)에 HOT·ICE 그림을 같이 넣는다.
