# ADR 목록

설계 결정 기록(Architecture Decision Record). 작성 방법은 [ADR-0001](0001-record-architecture-decisions.md)과 [template.md](template.md)를 본다.

새 ADR 만들기 (번호와 아래 목록 추가가 자동):

```bash
bash docs/adr/new-adr.sh "결정 제목" english-file-slug
```

상태가 바뀌면 파일의 `- 상태:` 줄과 아래 표를 함께 고친다. **표는 이 파일의 마지막에 둔다** (스크립트가 끝에 줄을 추가함).

| 번호 | 제목 | 상태 |
|---|---|---|
| [0001](0001-record-architecture-decisions.md) | 설계 결정을 ADR로 기록한다 | 승인 |
| [0002](0002-unity-version-and-stack.md) | Unity 6000.6.4f1 + URP 2D + Input System을 사용한다 | 승인 |
| [0003](0003-folder-and-namespace-rules.md) | 기능별 폴더 + 폴더 이름 = namespace, Core는 단방향 의존 | 승인 |
| [0004](0004-popup-base-class.md) | 모든 팝업·기기 화면은 Popup 베이스 클래스를 상속한다 | 승인 |
| [0005](0005-json-save-after-settlement.md) | 진행 데이터는 JSON 파일 하나에, 하루 정산 후에만 저장한다 | 승인 |
| [0006](0006-editor-builder-scripts-for-ui.md) | UI 씬·프리팹의 초기 배치는 에디터 빌더 스크립트로 만든다 | 승인 |
| [0007](0007-scene-conflict-prevention.md) | 씬 충돌은 프리팹 분리 + Smart Merge + 동시 수정 금지로 막는다 | 승인 |
| [0008](0008-ai-generated-art-assets.md) | UI·아트 에셋은 AI 이미지 생성으로 만들고 생성 기록을 남긴다 | 승인 |
| [0009](0009-balance-values-in-scriptableobject.md) | 밸런스 수치는 GameBalance ScriptableObject 하나에 모은다 | 승인 (구현 예정) |
| [0010](0010-inventory-base-units.md) | 재료 재고는 기본 단위(g·개·회)로 저장한다 | 승인 (구현 예정) |
| [0011](0011-day-cycle-scene-flow.md) | 하루 사이클의 씬 흐름과 영업 결과 전달 방식 | 제안 |
| [0012](0012-branch-protection-policy.md) | main 브랜치는 PR 필수, 리뷰 승인은 선택, Repo check는 필수로 한다 | 승인 |
| [0013](0013-business-loop-structure.md) | 영업 루프 구조와 임시 제조실 | 제안 |
| [0014](0014-device-stations-and-item-model.md) | 기기 화면, 물건 모델, 레시피 판정 구조 | 제안 |
