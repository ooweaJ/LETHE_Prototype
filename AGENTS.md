# LETHE Project Agent Rules

이 저장소에서 Codex는 Unity 2D 게임개발, 리소스 제작, AnkleBreaker Unity MCP 작업, C# 구현, 프리팹/씬 조립, 검증, 기록 갱신을 담당한다.

현재 목표는 HTML 프로토타입에서 검증한 LETHE의 기억/망각/잔향 구조를 Unity 2D 게임 slice로 옮겨, `절단쌍검 + 칼무리 잔향 + 혈반 잔향 + 피의 칼폭풍`이 실제 전투, 리소스, 프리팹, 디버그 루프에서 작동하는지 검증하는 것이다.

## Core Workflow

1. 구현 전 현재 파일과 문서를 확인한다.
2. Unity 작업은 `Assets/_dev`에서 먼저 진행한다. 첫 slice가 통과하기 전에는 `Assets/Lethe`로 승격하지 않는다.
3. Unity Editor 조작은 AnkleBreaker Unity MCP를 우선 사용한다. 직접 Unity HTTP bridge를 호출하지 않는다.
4. 리소스가 필요한 작업은 `docs/design/LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`의 파일명/프리팹/클래스 매트릭스를 따른다. imagegen은 리소스 제작 수단이지 전체 개발 목표가 아니다.
5. 코드, 리소스, 문서 변경 후 가능한 검증을 실행한다.
6. 결과를 `docs/orchestration/state/STATUS.md`에 반영하고, 필요한 경우 `docs/CODEX_STATUS.md`는 상세 레거시 아카이브로만 갱신한다.
7. 의미 있는 작업 단위가 끝나면 AI용 기록은 `docs/orchestration/devlog/YYYY-MM-DD.md`, 사람용 보고는 `docs/orchestration/reports/YYYYMMDD/index.md`에 갱신한다. 기존 `YYYYMMDD.md` devlog는 레거시 연속성 기록으로 유지한다.
8. 보고서 HTML은 Markdown 원본을 기준으로 생성한다.
9. 의미 있는 구현/밸런스/문서화 단위가 끝나면 `npm.cmd run report`와 `npm.cmd run report:check`로 Markdown 원본과 HTML 보고서를 갱신/검증한다. PowerShell에서 `npm.ps1` 실행 정책에 막히면 `npm.cmd`를 사용한다.
10. Discord 알림이 필요하면 Project Orchestrator의 중앙 Discord intake를 우선 사용한다. 중앙 intake가 아직 연결되지 않은 경우에만 이 저장소의 `npm run report:discord:unit:dry`로 본문/첨부를 확인하고, 사용자가 명시적으로 요청했거나 로컬 신뢰 환경일 때 `npm run report:discord:unit`을 fallback으로 실행한다. 실패하면 원인과 다음 실행 명령을 devlog/report에 남긴다.
11. 검증과 보고까지 끝난 의미 있는 작업 단위는 사용자가 금지하지 않은 경우 Conventional Commit으로 커밋하고, 작업 트리가 clean이며 공유해도 안전하면 `git push`까지 완료한다. push 실패 시 원인과 다음 명령을 사용자에게 보고한다.
12. 테스트 결과를 바탕으로 기획 수정 또는 방향 결정이 필요하면 `docs/orchestration/review_prompts/`에 Claude/GPT 전달 프롬프트를 남긴다. 기존 `docs/review_prompts/`는 마이그레이션 전 레거시 기록으로만 본다.

## Unity Game Development Workflow

첫 Unity slice는 아래 순서로 진행한다.

1. **규약/문서 확인**: `AGENTS.md`, orchestration state, `LETHE_UNITY_ECHO_SYSTEM_PRD.md`, `LETHE_UNITY_SLICE_ASSET_PRODUCTION_PLAN.md`, `LETHE_UNITY_ASSET_BINDING_PLAN.md`를 읽는다.
2. **개발 루트 확인**: AnkleBreaker MCP로 Unity `LETHE` 인스턴스와 `Assets/_dev` 인식을 확인한다.
3. **프로젝트 구조 생성**: `_dev/Art`, `_dev/Prefabs`, `_dev/Scripts`, `_dev/Data`, `_dev/Scenes` 하위 구조를 만든다.
4. **리소스 파이프라인**: player, enemy, map, weapon, echo VFX, ultimate VFX 이미지를 필요한 순서로 만든다. 현재 첫 단계는 기본 판독 이미지 5개다.
5. **런타임 기반 구현**: `RunBuildState`, `WeaponDefinition`, `MemoryDefinition`, `EchoDefinition`, `HitEvent`, `HitResolver`, `EchoTriggerRouter`, `PoolService` 같은 핵심 C# 구조를 만든다.
6. **기본 전투 루프**: player 이동/anchor, test enemy, dual blades, hitbox, damage/flash를 연결한다.
7. **기억/망각/잔향 루프**: 활성 칼무리, 혈반 잔향, 잔향 레벨, +5 각성, 공명, 피의 칼폭풍을 구현한다.
8. **프리팹/씬 조립**: `Dev_EchoSlice.unity`에 player, arena, enemies, weapon, echo prefabs, debug panel을 배치한다.
9. **디버그 루프**: debug panel에서 기본 쌍검, 칼무리 +1, 칼무리 +5, 혈반 +5, 피의 칼폭풍 상태를 즉시 전환할 수 있게 한다.
10. **검증/리뷰**: Unity console, MCP asset/scene 확인, 필요 시 테스트 또는 스크린샷, jaewoo 1인 리뷰로 GO/ITERATE/NO-GO를 판단한다.
11. **승격 판단**: `_dev` slice가 GO일 때만 구조와 리소스를 `Assets/Lethe`로 승격한다.

## Development Docs Plugin

This project uses `docs/orchestration/` as the shared personal development-docs plugin. Existing LETHE-specific rules in this `AGENTS.md` remain the top-level rules; orchestration files organize state, tasks, decisions, devlogs, reports, and evidence so Codex and the user can resume work quickly.

Before meaningful work, read:

1. `docs/orchestration/README.md`
2. `docs/orchestration/state/STATUS.md`
3. `docs/orchestration/state/CURRENT_TASK.md`
4. `docs/orchestration/state/NEXT_TASKS.md`
5. `docs/orchestration/state/PROMPT_CONTEXT.md`
6. `docs/orchestration/state/RUNBOOK.md`
7. `docs/orchestration/state/SCOPE_GUARD.md`

Use orchestration files as follows:

- `interface/index.html`: human-facing status dashboard.
- `interface/command.html`: human-facing next-instruction block.
- `interface/runbook.html`: human-facing operating-procedure block.
- `state/PROJECT_BRIEF.md`: project identity, goals, tech stack, and portfolio framing.
- `state/STATUS.md`: whole-project current state, latest verification, blockers, and next major step.
- `state/CURRENT_TASK.md`: one active work unit, done criteria, related files, open questions, and verification commands.
- `state/NEXT_TASKS.md`: top five next work candidates only.
- `state/PROMPT_CONTEXT.md`: stable context Codex should keep in mind each session.
- `state/RUNBOOK.md`: commands and procedures for test, build, report, package, recovery, and external handoff.
- `state/SCOPE_GUARD.md`: explicit non-goals and things not to build yet.
- `state/DECISION_LOG.md`: index of important technical, product, and AI-direction decisions.
- `devlog/`: AI/internal daily work logs.
- `reports/`: user-facing HTML progress reports. Keep `reports/index.html` as a blog-like newest-first date archive. Keep each `reports/YYYYMMDD/index.html` as that day's unit-card page, and place detailed unit reports under that date's `units/` folder.
- `reports/YYYYMMDD/index.md`: Markdown source used by the current local report generator.
- `reports/YYYYMMDD/index.html`: human-facing daily report page.
- `reports/YYYYMMDD/units/`: detailed work-unit Markdown/HTML/summary files.
- `review_prompts/`: current prompts prepared for Claude/GPT/Codex review.
- `review_responses/`: current saved AI review responses.
- `evidence/`: useful test outputs, screenshots, logs, benchmark summaries, or links.
- `templates/`: reusable document, report, review, and task templates.
- `legacy/`: archived or pointer-only legacy docs after migration.

After meaningful work:

- Update `state/STATUS.md`, `state/CURRENT_TASK.md`, or `state/NEXT_TASKS.md` as needed.
- Record internal process details in `devlog/YYYY-MM-DD.md`.
- Record Korean user-facing daily summaries in `reports/YYYYMMDD/index.md`.
- Regenerate `reports/index.html` so it lists date journal pages newest-first with title, date, and short summary.
- Regenerate `reports/YYYYMMDD/index.html` so it shows title blocks/cards for that day's units and links to individual unit pages.
- Record durable decisions in `state/DECISION_LOG.md`.
- Keep `state/NEXT_TASKS.md` short, usually no more than five active candidates.
- Regenerate or update `interface/` HTML when state changes.
- Keep Markdown as the AI source of truth; generated HTML is the human-facing project interface generated from Markdown.
- If Discord notification is needed, submit the short Korean summary, report path, optional attachment path, and source file list to Project Orchestrator's central Discord intake instead of sending directly from this project. Use this project's direct Discord script only as an explicit trusted-local fallback.
- Do not create new source-of-truth files under legacy `docs/reports/`, `docs/devlog/`, `docs/review_prompts/`, or `docs/review_responses/`.
- Do not delete legacy docs during adoption unless the user explicitly asked for migration; summarize, move, or link them from orchestration files.

## Role Split

- Codex: Unity `_dev` 게임 slice 구현, 리소스 제작, AnkleBreaker MCP 조작, C# 코드, 프리팹/씬, 테스트, git, report generation, status updates.
- Claude/GPT: 테스트 결과 해석, 기획 수정, 디자인 방향, 우선순위 판단, Unity slice GO/ITERATE/NO-GO 리뷰.

## Report Requirements

Every daily report should preserve these eight items:

1. 현재 빌드 상태.
2. 오늘 바뀐 것.
3. 테스트 결과와 근거.
4. 결정한 것.
5. 문제 또는 리스크.
6. GPT/Claude 인계 요약.
7. 다음 Codex 작업.
8. 포트폴리오 메모: 문제, 방향, 행동, 결과.

## Git Convention

Use Conventional Commits.

Examples:

- `feat: 기억 자동 발동 구현`
- `fix: 보스 처치 후 질문 UI 오류 수정`
- `docs: 오늘 개발 보고서 갱신`
- `test: AI 알파테스트 시나리오 추가`
- `chore: gitignore 정리`

Commit after a coherent unit is implemented and verified. Push after the working tree is clean and the commit is safe to share.

## Scope Guard

Until GPT or the user explicitly changes scope, do not add:

- Meta progression.
- Shop systems.
- Final boss.
- New weapons or memories outside the first slice.
- Multi-region run structure.

The current Unity slice goal is to verify whether `절단쌍검 + 칼무리 잔향 + 혈반 잔향 + 피의 칼폭풍` feels readable and exciting enough to justify promotion from `Assets/_dev` to `Assets/Lethe`. The forgetting loop should feel regrettable rather than irritating, and the echo loop should read through VFX/action before text.

## Claude Code Automation

- Claude Code may be used after AI or human tests to interpret results, revise planning, and decide what Codex should implement next.
- Claude answers should be saved under `docs/orchestration/review_responses/YYYY-MM-DD-claude.md`.
- Claude should not edit project files during automated planning; it is called with tools disabled.
- Codex reads the saved Claude response, updates `docs/orchestration/state/NEXT_TASKS.md`, then implements the selected work.
- Discord is only a status/attention channel. Markdown files are the source of truth. Prefer Project Orchestrator for Discord delivery.

## Legacy HTML Prototype

- HTML prototype work is now legacy unless the user explicitly asks to return to it.
- Do not start HTML autopilot, balance loops, or new HTML gameplay implementation as the default path.
- Existing HTML tests and reports remain historical evidence and may be run only when needed for regression comparison or report generation.
