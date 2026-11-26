# ADR-0054: AudioManager 는 빌더가 만든 프리팹으로 두고, 씬 빌더가 그 프리팹을 넣는다

- 상태: 승인
- 날짜: 2026-11-26
- 관련 명세: SFR-031, SFR-035, SFR-036, SFR-046

## 맥락

PR #25(씬별 BGM·클릭음, 커피 제조 효과음)는 `Prefabs/AudioManager.prefab` 을 추가했다. 그리고 Cafe·OpenPrep 씬을 직접 수정해 그 프리팹을 넣고, 새 효과음 7개(분쇄, 추출, 넉박스, 브러시, 스팀, 펌프, 청)는 Cafe 씬의 프리팹 오버라이드로 연결했다.

문제는 `CafeSceneBuilder` 였다. 이 빌더는 여전히 예전 효과음 13개만 가진 AudioManager 를 씬에 직접 만들었다. 그래서 `Build Cafe Scene` 을 실행하면 새 효과음이 오류 없이 사라졌다. OpenPrep 빌더도 프리팹을 넣지 않았다. 타이틀 BGM은 OpenPrep 에만 연결돼 있고 Title 씬에는 AudioManager 가 없었다.

사용자가 2026-11-26에 PR #25를 머지하고 빌더를 고치라고 결정했다.

## 결정

- `AudioManagerBuilder`(메뉴 `CafeTime/Build Audio Manager`)가 AudioManager 프리팹을 코드로 다시 만든다. 음원과 볼륨 목록은 이 빌더에만 둔다. 새 효과음 볼륨은 PR #25 작성자가 맞춘 값을 쓴다. 청 효과음은 씬에서 1로 올려 둔 값을 그대로 쓴다.
- Cafe·OpenPrep·Title 씬 빌더는 `AudioManagerBuilder.AddToScene(bgm)` 으로 프리팹을 넣는다. 넣을 때 프리팹을 먼저 다시 만들어 늘 최신 목록을 쓴다.
- 배경음악은 Cafe 는 `cafebgm`, Title·OpenPrep 은 `titlebgm` 을 쓴다. 씬마다 다른 BGM 은 프리팹 인스턴스의 `cafeBgm` 오버라이드로 넣는다.
- 새 음원을 추가할 때는 씬이 아니라 `AudioManagerBuilder` 의 목록에 추가한다.

## 검토한 대안

- `CafeSceneBuilder.BuildAudio` 목록에 새 음원 7개만 추가: 씬마다 AudioManager 정의가 흩어져 다른 씬(OpenPrep, Title)에서 같은 문제가 반복된다.
- 프리팹은 손으로 관리하고 빌더는 인스턴스만 넣기: 프리팹 YAML을 직접 고치게 되거나 Inspector 수정이 기록에 남지 않는다(ADR-0006).

## 결과

- 씬을 다시 빌드해도 효과음이 유지되고, Title 화면에서도 BGM과 클릭음이 난다.
- AudioManager 프리팹은 빌더가 덮어쓰므로 Inspector 에서 바꾼 값은 다음 빌드 때 사라진다. 볼륨 조정은 `AudioManagerBuilder` 목록에서 한다.
