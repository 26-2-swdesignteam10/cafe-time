# 카페타임 (Cafe Time)

15일 동안 매일 임대료를 내며 카페를 운영하는 2D 도트 경영 게임.

- 엔진: **Unity 6000.6.4f1** (팀원 모두 같은 버전 사용 — 버전이 다르면 프로젝트 파일이 바뀌어 충돌 발생)
- 언어: C# / 렌더 파이프라인: URP 2D / 입력: Input System
- 요구사항: `카페타임_요구사항명세서.pdf` (코드 주석의 `SFR-xxx` 번호가 명세서 항목)

## 처음 받았을 때

1. Unity Hub → Add → Add project from disk → 이 폴더 선택
2. `Assets/_Project/Scenes/Title.unity` 열고 ▶ Play
3. Game 뷰 해상도는 **1920x1080** 기준

## 폴더 구조

우리가 만든 파일은 전부 `Assets/_Project/` 아래에 둔다. (에셋스토어 등 외부 에셋과 섞이지 않게)

```
Assets/_Project/
├─ Art/            그림, 폰트 (Art/Fonts, Art/Sprites/<기능>, Art/UI ...)
├─ Audio/          배경음악, 효과음
├─ Prefabs/
│   ├─ UI/         여러 화면에서 같이 쓰는 UI (설정 창, 확인 팝업 ...)
│   └─ <기능>/     기능별 프리팹 (Title, Machines, Customers ...)
├─ Scenes/         Title, Story, OpenPrep, Cafe ...
├─ Scripts/
│   ├─ Core/       저장, 설정, 씬 이동 등 게임 전체 공통 (namespace CafeTime.Core)
│   ├─ UI/         공통 UI 컴포넌트: Popup, ConfirmPopup ... (namespace CafeTime.UI)
│   └─ <기능>/     기능별 코드: Title, Story, Machines ... (namespace CafeTime.<기능>)
└─ Editor/         에디터 전용 도구 (빌드에 포함되지 않음)
```

- 새 기능은 `Scripts/<기능>/`, `Prefabs/<기능>/` 폴더를 만들어 그 안에서 작업
- 빈 폴더는 git에 올라가지 않으므로 파일이 생길 때 폴더를 만든다

## 코드 규칙

- 클래스 하나 = 파일 하나, 파일 이름 = 클래스 이름
- namespace는 폴더 이름을 따른다: `CafeTime.Core`, `CafeTime.UI`, `CafeTime.Title` ...
- `Core`는 다른 기능 폴더를 참조하지 않는다 (기능 → Core 방향으로만 의존)
- Inspector에 노출할 필드는 `public` 대신 `[SerializeField] private`
- 씬 이름은 `SceneNames` 상수로만 사용 (`SceneLoader.Load(SceneNames.Story)`)
- 명세서 항목을 구현한 곳에는 주석으로 `SFR-xxx` 번호를 남긴다
- "추후 결정" 수치는 코드에 하드코딩하지 말고 `[SerializeField]` 필드로 두어 Inspector에서 조정

## 팝업 만들기

모든 팝업/기기 화면은 `Popup`을 상속한다.

- `Open()` / `Close()` 로 열고 닫기, `closeButton`에 X 버튼 연결
- ESC를 누르면 `Popup.CloseTop()` 이 가장 위의 팝업을 닫음
- `Popup.AnyOpen` 이 true면 UI가 열려 있는 상태 → 캐릭터 이동·상호작용 입력 무시 (SFR-015, 020)
- 닫을 때 추가 처리가 필요하면 `Close()`를 override

## Git 협업 규칙

- `main`에 직접 push하지 않고 기능별 브랜치 → Pull Request → 리뷰 후 merge
  - 브랜치 이름 예: `feature/title-screen`, `fix/settings-slider`
- **같은 씬(.unity)을 두 사람이 동시에 수정하지 않는다** (씬 파일 충돌은 해결이 매우 어려움)
  - 화면 요소는 프리팹으로 만들어 각자 프리팹을 수정 → 씬 충돌 최소화
  - 씬을 수정해야 하면 팀 채팅에 먼저 알리기
- `.meta` 파일은 반드시 원본 파일과 함께 커밋 (빠지면 참조가 깨짐)
- `Library/`, `Temp/`, `Logs/`, `UserSettings/` 는 커밋하지 않음 (`.gitignore`에 등록됨)

## 에디터 도구

`CafeTime` 메뉴 (`Assets/_Project/Editor/TitleSceneBuilder.cs`)

- **Setup Fonts**: TMP 기본 리소스 + 한글 폰트 에셋 생성 (처음 한 번)
- **Build Title Scene**: 시작 화면 씬과 팝업 프리팹을 코드로 생성 (처음 한 번).
  다시 실행하면 에디터에서 수정한 내용을 덮어쓰므로, 이후 수정은 씬/프리팹에서 직접 한다.

## 라이선스

- 폰트: [Galmuri](https://github.com/quiple/galmuri) — SIL Open Font License 1.1 (`Assets/_Project/Art/Fonts/Galmuri-OFL.txt`)
