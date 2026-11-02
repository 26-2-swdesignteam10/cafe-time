# 기여 가이드 (CONTRIBUTING)

카페타임 팀원이 작업을 시작하기 전에 읽는 문서다.
프로젝트 개요와 폴더 구조는 [README](../README.md), 설계 결정의 이유는 [ADR 목록](adr/README.md)을 본다.
아트 기준은 [ART_STYLE.md](ART_STYLE.md), 게임 수치는 [GAME_DATA.md](GAME_DATA.md), AI 에이전트 지침은 [AGENTS.md](../AGENTS.md)에 있다.

## 1. 개발 환경

| 항목 | 값 |
|---|---|
| Unity | **6000.6.4f1** (팀 전원 동일 버전, [ADR-0002](adr/0002-unity-version-and-stack.md)) |
| 렌더 파이프라인 | URP 2D |
| 입력 | Input System (`UnityEngine.InputSystem`) |
| 기준 해상도 | 1920x1080 |
| IDE | Visual Studio 2022 / Rider (`.vsconfig` 참고) |

처음 받았을 때:

1. Unity Hub → Add → Add project from disk → 저장소 폴더 선택
2. Smart Merge 설정 (PC마다 한 번) — [README의 "처음 받았을 때"](../README.md) 명령 실행
3. `Assets/_Project/Scenes/Title.unity` 를 열고 ▶ Play

## 2. 작업 흐름

**이슈 → 브랜치 → PR → CI 통과 + 리뷰 1명 승인 → merge**

1. **이슈**: `기능 구현` 또는 `버그` 템플릿으로 만든다. SFR 번호, 완료 조건, **수정할 씬/프리팹**을 적는다.
2. **브랜치**: `main` 에서 만든다. `main` 은 보호되어 직접 push 할 수 없다.

   | 종류 | 이름 예 |
   |---|---|
   | 기능 | `feature/open-prep` |
   | 버그 | `fix/settings-slider` |
   | 문서 | `docs/adr-save-format` |
   | 설정·도구 | `chore/ci-cache` |

3. **커밋**: 아래 커밋 메시지 규칙을 따른다. 작은 단위로 자주 커밋한다.
4. **PR**: 템플릿(구현한 SFR, 확인 방법, 스크린샷, 체크리스트)을 채우고 본문에 `Closes #이슈번호` 를 적는다.
5. **merge**: CI `Repo check` 통과 + 팀원 1명 승인 후 merge 한다.

### 커밋 메시지

[Conventional Commits](https://www.conventionalcommits.org/ko/) 형식을 쓴다.

```
<type>: <요약 (50자 이내)>

<필요하면 본문: 무엇을, 왜>
```

| type | 용도 |
|---|---|
| `feat` | 새 기능 (SFR 구현) |
| `fix` | 버그 수정 |
| `docs` | 문서만 변경 |
| `refactor` | 동작 변화 없는 코드 정리 |
| `chore` | 빌드·설정·에셋 정리 등 기타 |
| `ci` | GitHub Actions 변경 |

예: `feat: add ingredient shop to open prep scene (SFR-010)`

## 3. Unity 작업 규칙

- **같은 씬(.unity)을 두 사람이 동시에 수정하지 않는다.** 씬을 수정해야 하면 팀 채팅에 먼저 알린다. ([ADR-0007](adr/0007-scene-conflict-prevention.md))
- 화면 요소는 **프리팹**으로 만들고 각자 자기 프리팹을 수정한다.
- `.meta` 파일은 반드시 원본과 함께 커밋한다. 파일을 옮기거나 이름을 바꿀 때는 **Unity 에디터 안에서** 한다. (탐색기에서 옮기면 GUID 참조가 깨짐)
- `Library/`, `Temp/`, `Logs/`, `UserSettings/` 는 커밋하지 않는다.
- 커밋 전에 Play 해서 Console에 에러가 없는지 확인한다.

## 4. 코드 규칙

CI가 자동 검사하는 항목에는 ✅ 표시를 했다. ([ADR-0003](adr/0003-folder-and-namespace-rules.md))

- ✅ 클래스 하나 = 파일 하나, 파일 이름 = 클래스 이름
- ✅ namespace = 폴더 이름 (`CafeTime.Core`, `CafeTime.UI`, `CafeTime.Title` ...)
- ✅ `Core` 는 다른 기능 폴더를 참조하지 않는다 (기능 → Core 방향으로만 의존)
- Inspector 노출 필드는 `public` 대신 `[SerializeField] private`
- 씬 이름은 `SceneNames` 상수로만 사용 (`SceneLoader.Load(SceneNames.Story)`)
- 명세서 항목을 구현한 곳에 `// SFR-xxx` 주석을 남긴다
- 명세서의 "추후 결정" 수치는 하드코딩하지 않는다. `[SerializeField]` 또는 밸런스 에셋으로 뺀다 ([ADR-0009](adr/0009-balance-values-in-scriptableobject.md))
- 팝업·기기 화면은 `Popup` 을 상속한다 ([ADR-0004](adr/0004-popup-base-class.md))
- 포맷: `.editorconfig` (UTF-8, 공백 4칸)

### 팝업 만들기

- `Open()` / `Close()` 로 열고 닫기, `closeButton` 에 X 버튼 연결
- ESC를 누르면 `Popup.CloseTop()` 이 가장 위의 팝업을 닫음
- `Popup.AnyOpen` 이 true면 UI가 열려 있는 상태 → 캐릭터 이동·상호작용 입력 무시 (SFR-015, 020)
- 닫을 때 추가 처리가 필요하면 `Close()` 를 override

## 5. 아트 에셋 규칙

([ADR-0008](adr/0008-ai-generated-art-assets.md))

- 위치: `Assets/_Project/Art/<기능>/` (예: `Art/UI/OpenPrep/`, `Art/Sprites/Player/`)
- 스타일: 픽셀 아트, 호두나무·허니 골드·크림 팔레트, 투명 배경, 가장자리 여백 확보
- 임포트 설정 (기존 UI 스프라이트 기준):
  - Texture Type: `Sprite (2D and UI)`
  - Filter Mode: **Point (no filter)**
  - Generate Mip Maps: 끔
  - Alpha Is Transparency: 켬
- AI로 생성한 에셋은 같은 폴더에 `Generation.md` 를 남긴다 (사용 도구, 스타일 참조, 파일별 프롬프트). 예: `Art/UI/Quit/Generation.md`
- 외부 에셋(폰트, 무료 에셋 등)은 라이선스 파일을 함께 커밋하고 README 라이선스 항목에 추가한다.
- 50MB를 넘는 파일은 CI에서 실패한다.

## 6. 설계 결정 기록 (ADR)

여러 사람에게 영향을 주거나 나중에 "왜 이렇게 했지?" 라는 질문이 나올 결정은 ADR로 남긴다. ([ADR-0001](adr/0001-record-architecture-decisions.md))

- 예: 저장 형식, 씬 구조, 공통 시스템의 인터페이스, 외부 서비스 선택
- 만들기: `bash docs/adr/new-adr.sh "결정 제목" english-slug` (번호·목록 추가 자동, 상태 `제안`)
- 결정이 생긴 작업과 **같은 PR**에 ADR을 넣는다. 확정되면 상태를 `승인` 으로 바꾼다.
- 결정을 바꿀 때는 기존 ADR을 고치지 않고 새 ADR을 만든 뒤, 기존 ADR의 상태를 `대체됨 (ADR-NNNN)` 으로 바꾼다

어떤 결정을 어디에 적는지:

| 결정 | 문서 |
|---|---|
| 구조, 흐름, 인터페이스, 저장 형식, 도구, 규칙 | 새 ADR |
| 수치, 메뉴, 레시피, 가격 | [GAME_DATA.md](GAME_DATA.md) (+ `GameBalance.asset`) |
| 아트 스타일, 팔레트, 에셋 규격 | [ART_STYLE.md](ART_STYLE.md) |

### AI 에이전트에게 작업을 시킬 때

- Codex는 저장소 루트의 [`AGENTS.md`](../AGENTS.md)를, Claude Code는 `CLAUDE.md`(→ `AGENTS.md` 참조)를 자동으로 읽는다. 에이전트용 규칙은 `AGENTS.md` 한 곳에서 관리한다.
- `AGENTS.md` 에 따라 에이전트는 작업 중 생긴 결정을 바로 ADR(`제안`)이나 GAME_DATA/ART_STYLE에 반영하고, 작업이 끝나면 추가·변경한 ADR 번호를 알려 준다.
- 에이전트가 만든 `제안` ADR은 직접 읽어 보고, 동의하면 `승인` 으로 바꾸거나 에이전트에게 바꾸라고 한다.
- PR 템플릿의 "추가·변경한 ADR" 칸에 번호를 적는다.

## 7. CI

PR마다 `.github/workflows/ci.yml` 이 실행된다.

- **Repo check** (필수): `.meta` 누락, 자동 생성 폴더 커밋, 50MB 초과 파일, 코드 규칙, ADR 목록·상태
  - 로컬에서 먼저 확인: `bash .github/scripts/check-repo.sh`
- **Unity build**: 저장소 Settings → Secrets에 `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD` 를 등록하면 Windows 빌드를 만들어 Artifacts에 올린다 (등록 전에는 건너뜀)
