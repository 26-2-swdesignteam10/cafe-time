# ADR-0002: Unity 6000.6.4f1 + URP 2D + Input System을 사용한다

- 상태: 승인
- 날짜: 2026-10-28
- 관련 명세: SFR-074

## 맥락

명세서(SFR-074)는 Unity, C#, PC(키보드·마우스) 환경을 요구한다.
Unity는 에디터 버전이 다르면 프로젝트 파일(ProjectSettings, 씬 직렬화 형식)이 바뀌어 불필요한 diff와 충돌이 생긴다.

## 결정

- 에디터 버전을 **6000.6.4f1** 로 고정한다. 팀원 전원이 같은 버전을 쓴다.
- 렌더 파이프라인은 **URP 2D**, 입력은 **Input System** 패키지를 쓴다.
- 기준 해상도는 1920x1080 으로 한다.

## 검토한 대안

- Built-in 렌더 파이프라인: 2D 조명·픽셀 퍼펙트 지원이 URP 2D보다 약하고, 신규 기능이 URP 중심으로 나온다.
- 구 Input Manager(`Input.GetKey`): 명세의 "나중에 누른 키 우선"(SFR-015) 같은 처리가 번거롭고, 신규 프로젝트에서는 권장되지 않는다.

## 결과

- 버전 업그레이드는 팀 합의 후 한 사람이 별도 PR로 진행한다 (새 ADR 작성).
- 스크립트에서 입력은 `UnityEngine.InputSystem` (`Keyboard.current`, `Mouse.current` 또는 Input Actions)으로만 읽는다.
