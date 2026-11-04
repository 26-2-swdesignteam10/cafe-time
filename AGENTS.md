# AGENTS.md

AI 코딩 에이전트(Codex, Claude Code 등)가 이 저장소에서 작업할 때 따르는 지침이다.
사람 팀원용 규칙은 [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md)에 있고, 이 문서는 에이전트가 특히 지켜야 할 것만 모았다.

## 프로젝트

- **카페타임**: 15일 동안 매일 임대료를 내며 카페를 운영하는 2D 도트 경영 게임
- Unity **6000.6.4f1** (다른 버전으로 열거나 업그레이드하지 않는다), C#, URP 2D, Input System
- 요구사항: `카페타임_요구사항명세서.pdf` — 코드와 문서의 `SFR-xxx` 번호가 명세서 항목이다
- 사용자와의 대화, 문서, 코드 주석, 커밋·PR 제목의 요약은 **한국어**로 쓴다. 식별자와 커밋 type은 영어.

## 먼저 읽을 문서

| 작업 | 읽을 문서 |
|---|---|
| 모든 작업 | 이 파일, [docs/adr/README.md](docs/adr/README.md) (기존 설계 결정 목록) |
| 코드 | [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md) 4장 코드 규칙 |
| UI·아트 에셋 | [docs/ART_STYLE.md](docs/ART_STYLE.md) |
| 수치·메뉴·레시피 | [docs/GAME_DATA.md](docs/GAME_DATA.md) |

기존 ADR과 반대되는 구현이 필요하면, 구현하기 전에 사용자에게 알리고 새 ADR로 결정을 바꾼다.

## 저장소 구조

```
Assets/_Project/
├─ Art/<기능>/        스프라이트, 폰트 (AI 생성 에셋은 같은 폴더에 Generation.md)
├─ Editor/            씬·프리팹 빌더 (CafeTime 메뉴)
├─ Prefabs/<기능>/
├─ Resources/         GameBalance 에셋만 둔다 (ADR-0009)
├─ Scenes/
└─ Scripts/<기능>/    namespace CafeTime.<기능>
docs/                 CONTRIBUTING, ADR, ART_STYLE, GAME_DATA
```

## 코드 규칙 (CI가 검사함)

- 클래스 하나 = 파일 하나, 파일 이름 = 클래스 이름
- namespace = `CafeTime.<Scripts 바로 아래 폴더 이름>`
- `Scripts/Core/` 는 다른 기능 namespace를 `using` 하지 않는다
- 아직 `main` 에 머지되지 않은 다른 브랜치의 클래스를 참조하지 않는다 (필요하면 Core에 데이터 타입을 둔다)
- 팝업·기기 화면은 `CafeTime.UI.Popup` 상속, 씬 이름은 `SceneNames` 상수, 입력은 Input System
- 명세서의 "추후 결정" 수치는 하드코딩하지 않는다 → `GameBalance` 또는 `[SerializeField]`
- 구현한 곳에 `// SFR-xxx` 주석

## Unity 파일을 다룰 때

- `.unity`, `.prefab`, `.asset` YAML을 직접 편집하지 않는다. 씬·프리팹은 `Assets/_Project/Editor/` 의 빌더 스크립트로 만들고(ADR-0006), 사용자에게 Unity 메뉴(`CafeTime/...`) 실행을 요청한다.
- 새 파일을 `Assets/` 아래에 추가하면 `.meta` 가 필요하다. `.meta` 는 Unity가 생성하므로 직접 만들지 말고, 사용자에게 Unity 에디터로 돌아가 임포트하도록 안내한다.
- 파일 이동·이름 변경은 GUID가 깨지므로 직접 하지 말고 사용자에게 Unity 안에서 하도록 요청한다.
- 다른 사람이 작업 중인 씬은 수정하지 않는다. 씬 수정이 필요하면 먼저 사용자에게 확인한다.
- `Library/`, `Temp/`, `Logs/`, `UserSettings/` 는 건드리지 않는다.

## UI·아트 에셋 작업

[docs/ART_STYLE.md](docs/ART_STYLE.md)를 따른다. 요약:

1. 기존 에셋을 스타일 참조로 지정해 생성한다.
2. 저장 위치는 `Assets/_Project/Art/<기능>/`, 파일 이름은 PascalCase 영어 (`BuyButton.png`).
3. 같은 폴더의 `Generation.md` 에 도구, 스타일 참조, 파일별 프롬프트를 추가한다.
4. 금액·수량·닉네임처럼 바뀌는 글자는 이미지에 넣지 않는다 (TextMeshPro로 표시).
5. 이미지 안에 한국어 글자를 넣었으면 철자를 확인한다.

## 결정 기록 (ADR) — 작업 중 정한 것은 바로 문서에 반영한다

작업 중에 아래에 해당하는 결정이 생기면 **같은 작업(같은 PR) 안에서** 문서를 갱신한다. 대화에만 남기고 끝내지 않는다.

| 결정 종류 | 반영할 곳 |
|---|---|
| 구조·흐름·인터페이스·저장 형식·도구·규칙처럼 여러 기능에 영향을 주거나 되돌리기 어려운 결정 | 새 ADR |
| 수치, 메뉴, 레시피, 가격 등 게임 데이터 값 | [docs/GAME_DATA.md](docs/GAME_DATA.md) (+ `GameBalance.asset`) |
| 아트 스타일, 팔레트, 에셋 규격 | [docs/ART_STYLE.md](docs/ART_STYLE.md) |
| 한 파일 안에서 끝나는 구현 세부 | 기록하지 않음 (코드 주석이면 충분) |

ADR 작성 절차:

1. `bash docs/adr/new-adr.sh "결정 제목" english-file-slug` 실행 → 다음 번호 파일(`NNNN-english-file-slug.md`)이 생기고 `docs/adr/README.md` 목록에 추가된다. slug는 필수이며 영어 소문자·숫자·하이픈으로 쓴다.
2. 맥락 / 결정 / 검토한 대안 / 결과를 채운다. 사용자가 말한 이유가 있으면 그대로 적는다.
3. 상태 규칙:
   - 에이전트가 제안한 결정 → `제안`
   - 사용자가 명시적으로 확정한 결정 → `승인` (`docs/adr/README.md` 목록의 상태도 같이 바꾼다)
4. 기존 `승인` ADR의 본문은 고치지 않는다. 결정이 바뀌면 새 ADR을 쓰고, 기존 ADR의 상태만 `대체됨 (ADR-NNNN)` 으로 바꾼다.
5. 작업을 마칠 때 사용자에게 추가·변경한 ADR 번호와 상태를 알린다.

## 작업 마무리 전 확인

- `bash .github/scripts/check-repo.sh` 통과 (코드 규칙, `.meta`, ADR 목록 동기화)
- Unity에서 확인이 필요한 항목(메뉴 실행, Play 테스트, 임포트 설정)은 사용자에게 단계별로 알린다
- 커밋, push, PR 생성은 사용자가 요청할 때만 한다. 커밋 메시지와 PR 제목은 Conventional Commits, type은 영어·요약은 한국어 (`feat: 오픈 준비 화면 구현 (SFR-010)`)
- `main` 에 직접 커밋하지 않는다 (브랜치 보호)
