# 카페타임 (Cafe Time)

15일 동안 매일 임대료를 내며 카페를 운영하는 2D 도트 경영 게임.

- 엔진: **Unity 6000.6.4f1** (팀원 모두 같은 버전 사용 — 버전이 다르면 프로젝트 파일이 바뀌어 충돌 발생)
- 언어: C# / 렌더 파이프라인: URP 2D / 입력: Input System
- 요구사항: `카페타임_요구사항명세서.pdf` (코드 주석의 `SFR-xxx` 번호가 명세서 항목)

## 처음 받았을 때

1. Unity Hub → Add → Add project from disk → 이 폴더 선택
2. 씬·프리팹 충돌 자동 병합(Smart Merge) 설정 — 아래 명령을 PC에서 한 번 실행
3. `Assets/_Project/Scenes/Title.unity` 열고 ▶ Play
4. Game 뷰 해상도는 **1920x1080** 기준

```bash
git config merge.unityyamlmerge.name "Unity SmartMerge"
git config merge.unityyamlmerge.driver "'C:/Program Files/Unity/Hub/Editor/6000.6.4f1/Editor/Data/Tools/UnityYAMLMerge.exe' merge -h -p --force %O %B %A %A"
git config merge.unityyamlmerge.recursive binary
```

`.unity`, `.prefab`, `.asset` 은 `.gitattributes` 에서 Smart Merge로 병합하도록 지정되어 있다. 설정하지 않으면 일반 git 병합으로 동작해 씬 충돌이 훨씬 자주 난다.

## 문서

| 문서 | 내용 |
|---|---|
| [docs/CONTRIBUTING.md](docs/CONTRIBUTING.md) | 작업 흐름(이슈 → 브랜치 → PR), 커밋·코드·Unity·에셋 규칙, CI |
| [docs/adr/](docs/adr/README.md) | 설계 결정 기록(ADR) — "왜 이렇게 만들었는가" |
| [docs/GAME_DATA.md](docs/GAME_DATA.md) | 메뉴, 레시피, 가격, 시간 등 게임 수치 (확정/임시/미정) |
| [docs/ART_STYLE.md](docs/ART_STYLE.md) | 팔레트, UI 형태, 에셋 규격, 임포트 설정 |
| [AGENTS.md](AGENTS.md) | AI 에이전트(Codex, Claude Code) 작업 지침 |

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
docs/              기여 가이드, ADR, 게임 데이터, 아트 스타일
```

- 새 기능은 `Scripts/<기능>/`, `Prefabs/<기능>/` 폴더를 만들어 그 안에서 작업
- 빈 폴더는 git에 올라가지 않으므로 파일이 생길 때 폴더를 만든다

## 에디터 도구

`CafeTime` 메뉴 (`Assets/_Project/Editor/`)

- **Setup Fonts**: TMP 기본 리소스 + 한글 폰트 에셋 생성 (처음 한 번)
- **Create Game Balance**: `Resources/GameBalance.asset` 생성. 이미 있으면 수치는 그대로 두고 재료 아이콘 연결만 갱신
- **Build Title Scene / Build Open Prep Scene / Build Settlement Scene**: 씬과 프리팹을 코드로 생성 (처음 한 번).
- **Build Title Popups**: 닉네임·게임 설명·랭킹 팝업 프리팹만 다시 생성 (Title 씬은 그대로).
- **Build Cafe Scene** (`CafeSceneBuilder.cs`, `CafeUiBuilder.cs`): 영업 씬(맵, 플레이어, 기기, 손님, HUD, 포스기·픽업대 등 기기 화면)을 코드로 생성.
  그림은 `Art/Sprites/Cafe`, `Art/Sprites/Player`, `Art/Sprites/Customers`, `Art/UI/CafeHud`, `Art/UI/Pos`, `Art/UI/Stations` 에 있다.

빌더를 다시 실행하면 에디터에서 수정한 내용을 덮어쓰므로, 빌더로 만든 뒤의 수정은 씬/프리팹에서 직접 하거나 빌더 코드를 고친다.

## 라이선스

- 폰트: [Galmuri](https://github.com/quiple/galmuri) — SIL Open Font License 1.1 (`Assets/_Project/Art/Fonts/Galmuri-OFL.txt`)
