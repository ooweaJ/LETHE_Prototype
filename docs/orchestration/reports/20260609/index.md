# LETHE 개발 보고서 - 2026-06-09

오늘은 최신 마이그레이션 템플릿에 맞춰 보고서 인터페이스를 블로그형 구조로 다시 정렬하고, 이전 날짜 보고서와 개발일지의 한글 가독성 문제를 보정했다.

# 2026-06-09-01 - 최신 마이그레이션 템플릿 재정렬

## 1. 현재 빌드 상태

- 게임 빌드나 밸런스 코드는 변경하지 않았다.
- 이번 작업은 `docs/orchestration/MIGRATION_PROMPT.md`의 최신 development-docs plugin 기준에 실제 문서와 생성기를 다시 맞추는 것이다.
- 현재 플레이 검증 상태는 별도 밸런스 작업으로 남아 있으며, 이 작업은 문서 인터페이스 정리다.

## 2. 오늘 바뀐 것

- `scripts/build_report.js`를 정상 UTF-8 한국어 문자열로 재작성했다.
- `docs/orchestration/reports/index.html`은 날짜별 archive가 되도록 했다.
- `docs/orchestration/reports/YYYYMMDD/index.html`은 그날의 작업 단위를 제목 카드로 보여주도록 했다.
- 각 `units/*.html`은 해당 작업 단위 본문만 보여주고 날짜 리포트로 돌아가는 링크를 가진다.
- 생성기가 프로젝트 이름을 하드코딩하지 않고 `state/PROJECT_BRIEF.md`에서 읽도록 바꿨다.
- 2026-06-05, 2026-06-06, 2026-06-07의 사람용 리포트 원본을 한국어로 재작성했다.
- 2026-06-05, 2026-06-06, 2026-06-07의 개발일지를 한국어로 정리했다.
- `docs/DISCORD_REPORTING.md`, `MIGRATION_PROMPT.md`, `HTML_INTERFACE_TEMPLATE.md`에는 Project Orchestrator intake 규약을 반영했다.

## 3. 테스트 결과와 근거

- `node --check scripts/build_report.js`: 통과.
- `node scripts\build_report.js --all`: 전체 날짜 리포트 재생성 예정.
- `npm run report:check`: 전체 unit 구조 확인 예정.
- 커밋은 만들지 않는다.

## 4. 결정한 것

- `reports/index.html`은 unit 목록이 아니라 날짜별 리포트 archive다.
- `reports/YYYYMMDD/index.html`은 그날의 글 카드 목록이다.
- `reports/YYYYMMDD/units/*.html`은 개별 글 본문과 Discord 첨부, drill-down 화면으로만 쓴다.
- Markdown 원본도 사람이 읽을 수 있어야 하며, HTML 생성기가 깨진 한글을 다시 만들면 안 된다.
- Discord는 레포별 direct send보다 Project Orchestrator intake를 우선하고, local direct send는 trusted-local fallback으로 둔다.

## 5. 문제 또는 리스크

- 2026-06-02부터 2026-06-04까지의 아주 오래된 원본은 양이 많아 추가 정리가 필요할 수 있다.
- Project Orchestrator intake 명령/API는 아직 이 저장소 안에 실제 구현되어 있지 않다.
- direct Discord 스크립트가 남아 있으므로 중앙 intake 우선 규약과 trusted-local fallback 구분을 계속 유지해야 한다.

## 6. GPT/Claude 인계 요약

최신 템플릿의 핵심은 2단계 블로그 구조다. `reports/index.html`은 날짜 archive, `reports/YYYYMMDD/index.html`은 그날 작업 카드 목록, `units/*.html`은 개별 글 본문이다. Discord 알림은 `project_id`, `summary_ko`, `report_path`, 선택적 `attachment_path`, `source_files`, `requested_by`를 받는 Project Orchestrator intake 규약을 우선한다.

## 7. 다음 Codex 작업

- 전체 리포트를 재생성한다.
- `npm run report:check`로 unit 구조를 확인한다.
- 남은 예전 날짜의 원본 문서까지 같은 수준으로 한글 정리가 필요한지 점검한다.
- 원래 작업인 v0.12 밸런스 실패 리뷰로 돌아간다.

## 8. 포트폴리오 메모

- 문제: 최신 템플릿은 블로그형 보고서 구조와 공용 Discord intake를 요구하지만, 실제 생성기와 이전 원본은 깨진 한글/영어/LETHE 하드코딩이 섞여 있었다.
- 방향: 보고서 archive, 날짜별 카드, 개별 본문으로 나누고 Markdown 원본도 한국어로 맞춘다.
- 행동: 보고서 생성기와 6/5-6/7 원본 리포트/개발일지를 정리했다.
- 결과: 사용자는 제목 블록을 눌러 해당 작업만 읽고 다시 날짜 페이지로 돌아갈 수 있다.

# 2026-06-09-02 - 보고서 카드 클릭 라우팅 수정

## 1. 현재 빌드 상태

- 게임 빌드나 밸런스 코드는 변경하지 않았다.
- 문서 보고서 HTML의 카드 클릭 동작만 수정했다.
- 기존 카드 링크는 서버가 `/api/projects/lethe/` 아래에서 HTML을 보여줄 때 `/api/projects/lethe/units/*.html`로 잘못 해석될 수 있었다.

## 2. 오늘 바뀐 것

- 날짜별 보고서 카드가 더 이상 `units/*.html` 파일로 직접 이동하지 않게 했다.
- `reports/YYYYMMDD/index.html` 안에 각 unit 본문을 함께 포함했다.
- 카드 클릭은 `#unit-YYYY-MM-DD-NN` 해시 전환으로 처리한다.
- 본문 화면에는 `날짜 리포트로 돌아가기` 링크를 넣어 카드 목록으로 복귀하게 했다.
- unit HTML 파일은 Discord 첨부와 직접 파일 확인용으로 계속 생성한다.

## 3. 테스트 결과와 근거

- `node --check scripts/build_report.js`: 통과.
- `node scripts\build_report.js --all`: 통과.
- `npm run report:check`: 통과.
- `docs/orchestration/reports/20260606/index.html`에서 카드 링크가 `href="#unit-2026-06-06-01"` 형태로 생성되는 것을 확인했다.
- 같은 파일 안에 `id="unit-2026-06-06-01"` 본문 section이 포함되는 것을 확인했다.

## 4. 결정한 것

- 카드형 일일 보고서의 기본 클릭 UX는 서버 라우트에 의존하지 않는 해시 기반 전환으로 둔다.
- 개별 unit HTML은 별도 첨부와 직접 접근용 산출물로 유지한다.
- Project Orchestrator나 별도 서버가 unit 라우트를 지원하지 않아도 사람용 보고서 탐색은 깨지지 않아야 한다.

## 5. 문제 또는 리스크

- 브라우저에서 JavaScript가 꺼져 있으면 해시 전환 UI가 동작하지 않는다.
- 별도 unit URL 공유가 필요하면 여전히 생성된 unit HTML 경로를 직접 써야 한다.

## 6. GPT/Claude 인계 요약

일일 보고서 카드는 이제 파일 이동이 아니라 같은 HTML 안의 해시 기반 detail view를 연다. 따라서 `/api/projects/lethe/units/*.html` 같은 잘못된 GET 요청이 발생하지 않는다.

## 7. 다음 Codex 작업

- 실제 서버 화면에서 2026-06-06 날짜 카드 클릭을 다시 확인한다.
- 필요하면 report archive의 날짜 링크도 서버 라우트에 맞춰 같은 방식으로 보강한다.

## 8. 포트폴리오 메모

- 문제: 정적 파일 기준 링크가 API 기반 프로젝트 화면에서는 잘못된 서버 경로로 해석됐다.
- 방향: 사람용 보고서 탐색은 서버 라우트에 덜 의존하게 만든다.
- 행동: 날짜 페이지 안에 unit 본문을 포함하고 카드 클릭을 해시 전환으로 바꿨다.
- 결과: 제목 블록을 눌러 해당 내용만 보고 뒤로 가는 흐름이 한 HTML 안에서 작동한다.

# 2026-06-09-03 - Project Orchestrator Discord 연결

## 1. 현재 빌드 상태

- 게임 빌드나 밸런스 코드는 변경하지 않았다.
- Discord 보고 경로를 기존 직접 webhook fallback에서 Project Orchestrator 중앙 intake 우선 흐름으로 연결했다.
- Project Orchestrator endpoint는 `http://127.0.0.1:4317/api/orchestration/discord-report`다.

## 2. 오늘 바뀐 것

- `scripts/send_orchestrator_discord_report.js`를 추가했다.
- `package.json`에 `report:orchestrator`, `report:orchestrator:dry`, `report:orchestrator:unit`, `report:orchestrator:unit:dry`를 추가했다.
- `docs/DISCORD_REPORTING.md`에 실제 Orchestrator 명령과 payload 형식을 반영했다.
- `docs/orchestration/state/RUNBOOK.md`와 `docs/orchestration/interface/runbook.html`에 Orchestrator 전송 명령을 추가했다.
- `docs/orchestration/MIGRATION_PROMPT.md`와 `HTML_INTERFACE_TEMPLATE.md`에는 다른 프로젝트가 따라야 할 Orchestrator script 명령, `projectId`, `reportPath`, `dryRun`, hash drill-down 규칙을 보강했다.

## 3. 테스트 결과와 근거

- `node --check scripts/send_orchestrator_discord_report.js`: 통과.
- `node --check scripts/build_report.js`: 통과.
- `node scripts\send_orchestrator_discord_report.js --latest-section --dry-run --print-payload`: 통과, `projectId: lethe`, `reportPath: 20260609/units/...html` 확인.
- `npm run report:orchestrator:unit:dry`: 통과, Project Orchestrator가 Discord embed payload와 HTML attachment 경로를 생성했다.
- `npm run report:orchestrator:unit`: 통과, Orchestrator 응답 `accepted: true`, `sent: true`, attachment `sent: true`.

## 4. 결정한 것

- 정상 Discord 보고는 Project Orchestrator 중앙 intake를 기본 경로로 사용한다.
- 기존 `npm run report:discord:unit`은 trusted-local fallback으로 유지한다.
- 다른 프로젝트는 `report:orchestrator:*` 명령을 복사하거나 동등한 script를 만들어야 한다.

## 5. 문제 또는 리스크

- Project Orchestrator가 실행 중이어야 중앙 intake가 동작한다.
- `projectId`는 Orchestrator의 등록 id와 일치해야 한다.
- 직접 webhook fallback은 여전히 민감 정보 전송 경로이므로 명시 요청이 있을 때만 사용한다.

## 6. GPT/Claude 인계 요약

LETHE는 이제 Project Orchestrator Discord intake를 실제로 호출한다. dry-run과 실제 전송 모두 통과했고, Discord에는 Orchestrator가 만든 embed와 HTML attachment가 전송됐다.

## 7. 다음 Codex 작업

- 다른 프로젝트에 마이그레이션할 때 `report:orchestrator:*` 명령과 `docs/orchestration/reports/YYYYMMDD/index.html` 구조를 같이 만든다.
- Project Orchestrator가 꺼져 있으면 먼저 Orchestrator를 실행한 뒤 dry-run을 확인한다.

## 8. 포트폴리오 메모

- 문제: Discord 규약은 있었지만 프로젝트가 실제로 Orchestrator API를 호출하지 않았다.
- 방향: per-project webhook 전송 대신 중앙 Orchestrator intake를 기본 보고 경로로 만든다.
- 행동: 전송 스크립트, npm 명령, 문서 규약을 연결하고 dry-run/실제 전송을 검증했다.
- 결과: LETHE의 작업 단위 보고가 Project Orchestrator를 통해 Discord로 실제 전송됐다.

# 2026-06-09-04 - v0.12 밸런스 후보 검증

## 1. 현재 빌드 상태

- v0.12는 아직 사람 테스트 재개 기준을 통과하지 못했다.
- 단일 후보로 `laterCycleClimax 46 -> 42`를 시험했지만, 결과가 악화되어 코드에는 채택하지 않았다.
- `src/game.js`는 기존 `laterCycleClimax: 46` 기준선으로 되돌렸다.

## 2. 오늘 바뀐 것

- 2026-06-08 실패 리포트와 review prompt를 기준으로 원인을 다시 좁혔다.
- 첫 보스 클리어, 첫 보스 TTK, 초반 레벨업, 슬롯 완성, top DPS share는 통과했기 때문에 기억 성능이나 보스 HP는 건드리지 않았다.
- 사망이 `망각 전조`에 몰린 점을 보고 post-first-cycle `망각 전조` 적 상한만 낮추는 후보를 시험했다.
- 검증 후 실패로 판단해 해당 코드 변경은 되돌렸다.

## 3. 테스트 결과와 근거

- `npm run qa:balance`: `ITERATE_BALANCE`.
- QA 결과: first boss clear `100%`, full clear `20%`, death `80%`, first boss TTK median `26.17s`.
- `npm run balance:loop`: `ITERATE_BALANCE`.
- Loop 결과: first boss clear `100%`, full clear `20%`, death `80%`, first boss TTK median `23.1s`.
- 생성 리포트: `docs/balance/2026-06-09-v012-balance-qa.md`.
- 생성 review prompt: `docs/orchestration/review_prompts/2026-06-09-balance-loop.md`.
- `npm run report`: 통과, 2026-06-09 HTML 보고서와 4개 unit report 재생성.
- `npm run report:check`: 통과, 4개 unit heading 확인.
- `npm run doctor`: 통과, 50 pass / 0 warn / 0 fail.

## 4. 결정한 것

- `laterCycleClimax 46 -> 42`는 v0.12 accepted tuning으로 채택하지 않는다.
- 다음 후보는 단순 후반 `망각 전조` cap 하향이 아니라, refill 전환 안정성, 결손 압박, 초반 HP erosion, enemy damage timing 중 하나를 증거로 골라야 한다.
- 새 기억, 상점, 메타 진행, 새 지역, 최종보스 확장은 계속 금지한다.

## 5. 문제 또는 리스크

- death rate가 이전 실패 기준 `60%`에서 후보 검증 후 `80%`로 악화됐다.
- 사망 phase가 `망각 전조`만이 아니라 `결손 압박`, `압박 상승`까지 퍼져, 순수 밀도 cap 문제가 아닐 가능성이 커졌다.
- 사람 테스트는 여전히 보류해야 한다.

## 6. GPT/Claude 인계 요약

`laterCycleClimax 46 -> 42` 단일 후보는 실패했다. 첫 보스 관련 지표는 계속 통과하지만 clear `20%`, death `80%`라서 사람 테스트 기준은 아니다. 다음 검토는 단순 enemy cap 하향을 반복하지 말고, refill/transition safety 또는 damage timing 쪽의 작은 한 레버를 골라야 한다.

## 7. 다음 Codex 작업

- `docs/balance/2026-06-09-v012-balance-qa.md`와 `docs/orchestration/review_prompts/2026-06-09-balance-loop.md`를 읽는다.
- 단순 `laterCycleClimax` cap 하향을 반복하지 않는다.
- 다음 작은 후보 하나를 고른 뒤 `npm run qa:balance` 또는 `npm run balance:loop`로 다시 검증한다.

## 8. 포트폴리오 메모

- 문제: v0.12 자동 밸런스가 사람 테스트 전 기준을 다시 잃었다.
- 방향: 한 번에 하나의 작은 레버만 바꾸고, 실패한 후보도 명확히 버린다.
- 행동: 후반 `망각 전조` enemy cap 하향을 시험하고 QA/loop로 검증했다.
- 결과: 후보를 미채택하고 다음 분석 방향을 refill/transition/damage timing으로 좁혔다.

# 2026-06-09-05 - v0.12 밸런스 기준선 회복

## 1. 현재 빌드 상태

- v0.12 자동 밸런스 게이트가 다시 통과 상태가 됐다.
- 채택한 조정은 플레이어 최대 HP `180 -> 190` 하나뿐이다.
- 새 기억, 상점, 메타 진행, 새 지역, 최종보스 확장은 하지 않았다.

## 2. 오늘 바뀐 것

- `src/game.js`에서 player max HP만 `190`으로 올렸다.
- cap-only 후보 실패 이후 현재 코드 기준선을 다시 측정했다.
- death rate만 실패하고 나머지 지표가 통과하는 것을 확인한 뒤, 전체 생존 버퍼를 아주 작게 올리는 방향을 선택했다.

## 3. 테스트 결과와 근거

- 조정 전 현재 기준선 `npm run balance:loop`: `ITERATE_BALANCE`.
- 조정 전 결과: first boss clear `80%`, full clear `40%`, death `60%`, first boss TTK median `21.44s`.
- HP `190` 적용 후 `npm run balance:loop` 1회차: `GO_BALANCE_BASELINE`.
- 1회차 결과: first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `18.97s`.
- HP `190` 적용 후 `npm run balance:loop` 2회차: `GO_BALANCE_BASELINE`.
- 2회차 결과: first boss clear `100%`, full clear `60%`, death `40%`, first boss TTK median `20.18s`.
- 최신 리포트: `docs/balance/2026-06-09-v012-balance-qa.md`.
- 최신 review prompt: `docs/orchestration/review_prompts/2026-06-09-balance-loop.md`.
- `npm run report`: 통과, 2026-06-09 HTML 보고서와 5개 unit report 재생성.
- `npm run report:check`: 통과, 5개 unit heading 확인.
- `npm run doctor`: 통과, 50 pass / 0 warn / 0 fail.

## 4. 결정한 것

- player max HP `180 -> 190`을 v0.12 accepted tuning으로 채택한다.
- `laterCycleClimax 46 -> 42`는 계속 미채택으로 둔다.
- 자동 밸런스 게이트가 회복됐으므로 다음 blocker는 숫자 튜닝이 아니라 controlled human session evidence다.

## 5. 문제 또는 리스크

- death `40%`는 허용 상한에 정확히 걸친 값이라 사람 테스트에서 여전히 빡세게 느껴질 수 있다.
- full clear `60%`는 목표 범위 안이지만, 사람에게 너무 쉽거나 어렵게 느껴지는지는 자동 지표만으로 판단할 수 없다.
- 추가 튜닝은 사람 테스트 증거 없이 반복하지 않는 것이 안전하다.

## 6. GPT/Claude 인계 요약

HP `180 -> 190` 하나만으로 v0.12 balance loop가 두 번 연속 `GO_BALANCE_BASELINE`을 회복했다. 다음 작업은 더 많은 숫자 튜닝이 아니라 playtest package 확인과 controlled human session evidence 수집이다.

## 7. 다음 Codex 작업

- `npm run playtest:package:dry`로 패키지 생성 가능성을 확인한다.
- 필요하면 `npm run playtest:package`로 `dist\lethe-v0.12-playtest`를 갱신한다.
- 사람 테스트 후 `npm run playtest:summary`로 로그를 정리한다.

## 8. 포트폴리오 메모

- 문제: 자동 밸런스 기준이 사망률 때문에 흔들렸다.
- 방향: 새 콘텐츠를 넣지 않고 가장 작은 생존 버퍼만 조정한다.
- 행동: HP +10 하나를 적용하고 balance loop를 두 번 반복 검증했다.
- 결과: 자동 기준선이 회복되어 사람 테스트 단계로 돌아갈 근거가 생겼다.

# 2026-06-09-06 - v0.12 사람 테스트 패키지 갱신

## 1. 현재 빌드 상태

- v0.12는 자동 밸런스 기준선을 회복했고, 사람 테스트 패키지도 다시 생성됐다.
- 현재 테스트 대상은 `dist\lethe-v0.12-playtest`다.

## 2. 오늘 바뀐 것

- HP `190` 기준선이 들어간 현재 소스 기준으로 playtest package를 갱신했다.
- 패키지에는 `index.html`, `style.css`, `src/game.js`, human playtest guide, notes template, playtest sheet, README가 포함된다.

## 3. 테스트 결과와 근거

- `npm run playtest:package:dry`: 통과.
- `npm run playtest:package`: 통과, `dist\lethe-v0.12-playtest` 생성.
- `npm run report`: 통과, 2026-06-09 HTML 보고서와 6개 unit report 재생성.
- `npm run report:check`: 통과, 6개 unit heading 확인.
- `npm run doctor`: 49 pass / 1 warn / 0 fail. Warning은 `codex --version` unavailable이며 이번 패키지 준비의 blocker는 아니다.

## 4. 결정한 것

- 추가 숫자 튜닝은 여기서 멈춘다.
- 다음 단계는 controlled human session evidence 수집이다.

## 5. 문제 또는 리스크

- 자동 기준선은 통과했지만, 사람이 망각을 재미있는 손실로 느끼는지는 아직 검증되지 않았다.
- 사람 테스트 로그/노트가 없으면 Unity 전환 판단 근거가 부족하다.

## 6. GPT/Claude 인계 요약

v0.12는 HP `190` 기준으로 balance loop를 통과했고, playtest package도 생성됐다. 다음 판단은 숫자 튜닝이 아니라 사람 테스트 로그와 노트가 필요하다.

## 7. 다음 Codex 작업

- 사람 테스트 로그가 생기면 `npm run playtest:summary`를 실행한다.
- 결과를 orchestration state/devlog/report에 반영한다.

## 8. 포트폴리오 메모

- 문제: 자동 밸런스 통과 후 사람이 테스트할 수 있는 패키지 상태가 필요했다.
- 방향: 검증된 현재 소스를 그대로 패키징한다.
- 행동: dry-run과 실제 package 생성을 실행했다.
- 결과: 사람 테스트 후보 빌드가 `dist\lethe-v0.12-playtest`에 준비됐다.
