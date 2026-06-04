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
