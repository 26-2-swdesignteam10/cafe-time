# ADR-0007: 씬 충돌은 프리팹 분리 + Smart Merge + 동시 수정 금지로 막는다

- 상태: 승인
- 날짜: 2026-10-29

## 맥락

Unity 씬(.unity)은 큰 YAML 파일이라 두 사람이 동시에 수정하면 git 병합이 사실상 불가능하다.

## 결정

1. 화면 요소는 프리팹으로 만들고 씬에는 프리팹 인스턴스만 둔다. 각자 자기 프리팹을 수정한다.
2. 같은 씬은 동시에 한 사람만 수정한다. 수정 전 팀 채팅에 알리고, 이슈에 수정할 씬을 적는다.
3. `.gitattributes` 에서 `.unity`, `.prefab`, `.asset` 을 `merge=unityyamlmerge` 로 지정하고, 각자 PC에서 UnityYAMLMerge 드라이버를 설정한다.
4. 텍스트 에셋은 `eol=lf` 로 고정해 줄바꿈 diff를 막는다.

## 검토한 대안

- Unity Version Control(Plastic) 잠금 기능: 저장소를 GitHub으로 관리한다는 명세(SFR-074)와 맞지 않는다.
- Git LFS 파일 잠금: 무료 용량 제한과 설정 부담이 있다.

## 결과

- 기능마다 씬을 나누는 편이 유리하다 (Title, Story, OpenPrep, Cafe, Settlement ...).
- Smart Merge를 설정하지 않은 PC에서는 일반 병합으로 동작하므로 처음 받을 때 꼭 설정한다.
