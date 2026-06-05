# LETHE v0.12 Balance Automation

이 문서는 v0.12 이후 밸런스 자동 측정 루프의 기준이다. 감정 proxy, regret, irritation, Alpha Fun Score는 이 루프의 판정 기준이 아니다.

## Commands

브라우저 telemetry 기반 밸런스 QA:

```powershell
npm run qa:balance
```

dry-run:

```powershell
npm run qa:balance:dry
```

밸런스 QA를 실행하고 다음 튜닝용 프롬프트까지 생성:

```powershell
npm run balance:loop
```

dry-run:

```powershell
npm run balance:loop:dry
```

## Outputs

- `alpha_test/outputs/balance/summary.json`
- `alpha_test/outputs/balance/latest.json`
- `alpha_test/outputs/balance/run-XX.json`
- `docs/balance/YYYY-MM-DD-v012-balance-qa.md`
- `docs/review_prompts/YYYY-MM-DD-balance-loop.md`

## Metrics

밸런스 루프는 아래 수치만 본다.

- full clear rate
- death rate
- first boss clear rate
- first boss TTK median
- level-ups before first boss median
- slots filled at median
- top DPS share median
- `telemetry.dpsBySource`
- `telemetry.bossFights`

## Default Targets

- first boss clear rate: `>= 70%`
- full clear rate: `35% - 80%`
- first boss TTK median: `15 - 30s`
- level-ups before first boss median: `>= 8`
- slots filled at median: `<= 150s`
- top DPS share median: `<= 50%`

## Current Interpretation

`ITERATE_BALANCE`는 실패가 아니라 다음 튜닝 입력이다. 이 경우 `docs/review_prompts/YYYY-MM-DD-balance-loop.md`를 읽고 가장 작은 밸런스 조정 1개만 구현한다.

`GO_BALANCE_BASELINE`은 현재 수치가 기준선 범위에 들어왔다는 뜻이다. 그래도 사람 감정/재미 판정은 별도이며, 이 문서는 수치 밸런스만 다룬다.

## Guardrails

- 새 기억, 새 무기, 상점, 메타 진행, 새 지역, 최종 보스 확장은 하지 않는다.
- 보스 HP, XP 곡선, 기억 피해, 진화 proc/cap, 적 스케일링만 조정한다.
- Discord 보고와 Markdown 보고서가 source of truth다.

## Boss-Only TTK Harness

Chrome/CDP가 first boss TTK 샘플을 안정적으로 만들지 못할 때는 별도 in-process 하네스를 먼저 사용한다.

```powershell
npm run qa:boss-ttk
```

dry-run:

```powershell
npm run qa:boss-ttk:dry
```

- 목적: `balanceScenario=first_boss_ttk`와 같은 의도인 level 10, 3 active memories, full HP, first-boss only 상태에서 TTK와 focused DPS를 확보한다.
- 출력:
  - `alpha_test/outputs/boss-ttk/summary.json`
  - `alpha_test/outputs/boss-ttk/latest.json`
  - `docs/balance/YYYY-MM-DD-v012-boss-ttk-harness.md`
- 현재 기본 first boss HP: `3500`.
- 최신 accepted samples: `5/5`.
- 최신 boss-only TTK median: `21.92s`.
- 최신 focused DPS median: `159.7`.
- 근거 파일: `docs/balance/2026-06-05-v012-boss-ttk-harness-final.md`.

이 값은 보스 HP 조정 입력값이며, 사람 플레이 감정 증거 또는 전체 브라우저 밸런스 통과 증거가 아니다. HP 반영 후에는 다시 `npm run qa:balance`로 first-boss reach/clear/death를 확인한다.

### 2026-06-05 HP 2800 Follow-Up

- HP `3500` boss-only TTK passed, but browser `qa:balance` produced first boss TTK median `35.65s`, above the 15-30s target.
- Current first boss HP is now `2800`.
- Boss-only HP `2800`: 5/5 accepted samples, TTK median `17.8s`, focused DPS median `157.3`.
- Browser `first_boss_ttk` HP `2800`: 1/3 accepted sample, accepted TTK `22.59s`, 2/3 incomplete.
- Browser full `qa:balance` HP `2800`: first boss clear `60%`, death `0%`, TTK median `53.21s`, but 2/5 incomplete.
- Next gate: stabilize browser `first_boss_ttk` accepted sample count before another HP change.

### 2026-06-06 Browser First Boss TTK Terminal

- `first_boss_ttk` browser QA now exits as `complete` when the first boss TTK sample is recorded.
- Scenario-specific summary checks now exclude full-run clear, level-up, and slot-fill gates.
- Browser `first_boss_ttk`: 3/3 accepted samples, first boss clear `100%`, TTK median `25.76s`, verdict `GO_BALANCE_BASELINE`.
- Full `qa:balance`: first boss clear `80%`, death `20%`, first boss TTK median `27.79s`, full clear `0%`, verdict `ITERATE_BALANCE`.
- Keep first boss HP at `2800`; the next automation target is post-boss/full-run flow.

### 2026-06-06 Balance Baseline

- Full-run QA now uses a `690s` default run window so the `600s` scheduled boss has time to resolve.
- Post-boss pressure caps were added: deficit breath `16`, deficit trial `22`, later-cycle default `58`.
- First boss HP is now `2500`.
- Boss-only HP `2500`: 5/5 accepted, TTK median `15.62s`.
- Browser `first_boss_ttk` HP `2500`: 3/3 accepted, TTK median `21.05s`.
- Full browser `qa:balance`: verdict `GO_BALANCE_BASELINE`, first boss clear `100%`, full clear `40%`, death `60%`, first boss TTK median `22.24s`.
- Next gate: reviewer/GPT interpretation of deficit-trial death rate before human playtest.
