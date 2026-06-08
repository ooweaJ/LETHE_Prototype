# LETHE 개발 보고서 - 2026-06-07

이 날짜의 핵심은 v0.12를 인간 플레이테스트 후보로 포장하고, 밸런스 루프 기준이 실제 v0.12 목표와 맞지 않던 문제를 수정한 것이다.

# 2026-06-07-01 - v0.12 인간 플레이테스트 패키지

## 1. 현재 빌드 상태

v0.12는 현재 통제된 인간 플레이테스트 후보이다. 직전 자동 밸런스 기준선은 `GO_BALANCE_BASELINE`이며, 첫 보스 클리어 `80%`, 전체 클리어 `60%`, 사망 `40%`, 첫 보스 TTK 중앙값 `25.79s`를 기록했다.

## 2. 오늘 바뀐 것

- `docs/HUMAN_PLAYTEST_GUIDE.md`를 v0.12 기준 5-8명 통제 세션용으로 갱신했다.
- `docs/PLAYTEST_NOTES.md`에 초반 루프, 첫 보스, 망각, 결손, 리필 관찰 항목을 추가했다.
- `docs/playtest/2026-06-07-v012-human.md`를 추가했다.
- `scripts/prepare_playtest_build.js`가 패키지에 `V012_HUMAN_PLAYTEST_SHEET.md`를 포함하도록 바꿨다.

## 3. 테스트 결과와 근거

- `node --check scripts/prepare_playtest_build.js`: 통과.
- `npm run playtest:package:dry`: 통과, 출력 대상 `dist\lethe-v0.12-playtest`.
- `npm run playtest:package`: 통과, `dist\lethe-v0.12-playtest` 생성.
- 패키지 파일 확인: `HUMAN_PLAYTEST_GUIDE.md`, `PLAYTEST_NOTES_TEMPLATE.md`, `V012_HUMAN_PLAYTEST_SHEET.md`, `PLAYTEST_SUMMARY_README.md`, `README.md`, `index.html`, `style.css`, `src/`.
- `npm run report`: 통과.
- `npm run report:check`: 통과.
- `npm run doctor`: 통과, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: 통과.

## 4. 결정한 것

- 최신 자동 밸런스 기준선을 현재 인간 테스트 전 후보로 받아들인다.
- 인간 증거가 나오기 전까지 추가 수치 튜닝은 멈춘다.
- 다음 작업은 통제 세션 실행과 로그 요약으로 넘긴다.

## 5. 문제 또는 리스크

- 인간 플레이어가 사망 `40%`와 패배 후 압박을 여전히 날카롭게 느낄 수 있다.
- AI 밸런스 QA는 인간 감정 증거가 아니다.
- `dist/` 패키지는 git에서 제외되므로 공유 직전에 `npm run playtest:package`로 다시 생성해야 한다.
- 이 Codex 세션에서는 외부 Discord webhook 전송이 잠재적 데이터 반출로 판단되어 실제 전송이 차단될 수 있다.

## 6. GPT/Claude 인계 요약

감정 proxy는 제외한다. v0.12는 자동 밸런스 기준선과 인간 테스트 패키지를 갖췄다. 다음 판단은 5-8명 통제 세션에서 초반 재미, 레벨업 선택 의미, 첫 망각 반응, 결손 생존감, 리필 동기, 재시작 의사를 보고 내려야 한다.

## 7. 다음 Codex 작업

- 세션 후 다운로드한 JSON 로그를 `playtest_logs/`에 넣는다.
- `npm run playtest:summary`를 실행한다.
- 큰 튜닝이나 Unity 전환 판단 전에 생성된 인간 테스트 프롬프트를 planning pipeline으로 보낸다.

## 8. 포트폴리오 메모

- 문제: v0.12는 자동 밸런스를 통과했지만 인간 테스트 자료는 이전 v0.7 기준이 남아 있었다.
- 방향: 자동 기준선을 실제 통제 플레이테스트 패키지로 전환한다.
- 행동: 가이드, 노트, 세션 시트, 패키지 생성 스크립트를 갱신했다.
- 결과: `dist\lethe-v0.12-playtest`가 생성되어 통제 세션을 열 수 있다.

# 2026-06-07-02 - 밸런스 루프 게이트 수정

## 1. 현재 빌드 상태

v0.12는 단발 QA가 아니라 밸런스 루프에서도 `GO_BALANCE_BASELINE`으로 돌아왔다. 최종 루프 결과는 첫 보스 클리어 `100%`, 전체 클리어 `60%`, 사망 `40%`, 첫 보스 TTK 중앙값 `20.73s`이다.

## 2. 오늘 바뀐 것

- 루프 실행 전 `npm run autopilot:preflight:local`을 실행했다.
- `scripts/run_balance_loop.js`의 기본 실행 창을 `690s`로 바꾸고, PowerShell/npm 경로에서 들어오는 위치 인자를 처리하도록 수정했다.
- dry-run 출력에 실제 루프 설정이 보이도록 했다.
- `scripts/run_browser_balance_qa.js`에 사망률 최대 `<= 40%` 게이트를 추가했다.
- 첫 보스 HP, 결손 지속시간, 적 피해 스케일, 보스 이후 cap, 리필 안전 마진을 조정했다.

## 3. 테스트 결과와 근거

- `npm run autopilot:preflight:local`: 20 pass / 1 warn / 0 fail.
- `node --check src/game.js`: 통과.
- `node --check scripts/run_browser_balance_qa.js`: 통과.
- `node --check scripts/run_balance_loop.js`: 통과.
- `npm run balance:loop:dry`: 통과, `run-sec 690` 확인.
- 브라우저 `first_boss_ttk`: `GO_BALANCE_BASELINE`, 3/3 accepted, TTK 중앙값 `18.61s`.
- 최종 `balance:loop`: `GO_BALANCE_BASELINE`, 첫 보스 클리어 `100%`, 전체 클리어 `60%`, 사망 `40%`, 첫 보스 TTK 중앙값 `20.73s`.
- `npm run playtest:package`: 통과, 밸런스 변경 후 `dist\lethe-v0.12-playtest` 재생성.
- `npm run report`: 통과.
- `npm run report:check`: 통과.
- `npm run doctor`: 통과, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: 통과.
- 근거: `docs/balance/2026-06-07-v012-browser-first-boss-ttk-loop-hp2050.md`, `docs/balance/2026-06-07-v012-balance-loop.md`, `docs/review_prompts/2026-06-07-balance-loop.md`.

## 4. 결정한 것

- `balance:loop`를 밸런스 QA의 기준 소스로 본다.
- 사망 `60%`는 더 이상 GO로 허용하지 않는다.
- Discord 실제 전송은 규칙상 필요하지만, 이 Codex 세션에서 승인 검토에 막히면 원인과 trusted-local 명령을 문서화한다.

## 5. 문제 또는 리스크

- 5회 브라우저 루프는 여전히 표본이 작다. 큰 주장 전에는 10회 확인을 고려한다.
- 사망률은 허용 상한인 `40%`에 정확히 걸려 있다.
- 이 Codex 세션에서는 Discord 실제 webhook 전송이 차단될 수 있다.

## 6. GPT/Claude 인계 요약

감정 proxy는 제외한다. 밸런스 루프는 v0.12의 `690s` 창을 쓰도록 수정되었고, 사망률 `<= 40%`를 강제한다. 튜닝 후 최종 루프는 전체 클리어 `60%`, 사망 `40%`, 첫 보스 TTK 중앙값 `20.73s`로 통과했다.

## 7. 다음 Codex 작업

- Discord 전송이 필요하면 trusted local terminal에서 `npm run report:discord:unit`를 실행한다.

## 8. 포트폴리오 메모

- 문제: 단발 QA와 루프 QA가 서로 다르고, 사망 `60%`도 자동 게이트를 통과할 수 있었다.
- 방향: 루프를 밸런스 기준 소스로 만들고 사망 목표를 코드에 넣는다.
- 행동: 루프 인자와 기본값을 고치고, 사망 게이트를 추가하고, 보스 이후 압박을 재조정했다.
- 결과: 밸런스 루프가 의도한 기준으로 통과한다.
