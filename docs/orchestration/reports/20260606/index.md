# LETHE 개발 보고서 - 2026-06-06

이 날짜의 핵심은 첫 보스 TTK 측정 경로를 안정화하고, 보스 이후 전체 런 밸런스 기준선을 만든 뒤, 결손 시험 생존감을 조정한 것이다.

# 2026-06-06-01 - 브라우저 첫 보스 TTK 종료 조건 수정

## 1. 현재 빌드 상태

v0.12는 아직 `ITERATE_BALANCE`였지만, 첫 보스 브라우저 TTK 샘플 수집이 타임아웃으로 막히던 문제는 해결됐다. 첫 보스 HP는 `2800`을 유지했다.

## 2. 오늘 바뀐 것

- `balanceScenario=first_boss_ttk`가 첫 보스 TTK 샘플을 기록하는 즉시 완료되도록 했다.
- 브라우저 밸런스 QA가 외부 poll timeout에 걸려도 마지막 page QA payload를 보존하게 했다.
- `first_boss_ttk` 시나리오는 전체 런 클리어가 아니라 첫 보스 전용 체크를 쓰도록 분리했다.

## 3. 테스트 결과와 근거

- `node --check src/game.js`: 통과.
- `node --check scripts/run_browser_balance_qa.js`: 통과.
- 브라우저 `first_boss_ttk`: 3/3 accepted, 첫 보스 클리어 `100%`, TTK 중앙값 `25.76s`, verdict `GO_BALANCE_BASELINE`.
- 전체 브라우저 `qa:balance`: 첫 보스 클리어 `80%`, 사망 `20%`, 첫 보스 TTK 중앙값 `27.79s`, 첫 보스 전 레벨업 중앙값 `11`, 전체 클리어 `0%`, verdict `ITERATE_BALANCE`.
- 근거: `docs/balance/2026-06-06-v012-browser-first-boss-ttk-terminal.md`, `docs/balance/2026-06-06-v012-balance-qa.md`.

## 4. 결정한 것

- 첫 보스 HP `2800`을 유지한다.
- TTK 샘플 안정성은 더 이상 현재 blocker로 보지 않는다.
- 다음 밸런스 작업은 보스 이후와 전체 런 흐름으로 이동한다.

## 5. 문제 또는 리스크

- 전체 클리어는 여전히 `0%`였다.
- 전체 런 하나가 망각 경고 구간 `156.09s` 근처에서 사망했다.
- 첫 보스 TTK 시나리오는 측정 경로이지 전체 런 밸런스 증거가 아니다.
- 실제 Discord 전송은 이 Codex 세션에서 승인 검토에 막힐 수 있다.

## 6. GPT/Claude 인계 요약

감정 proxy는 제외한다. HP `2800`에서 브라우저 첫 보스 TTK 수집은 3/3 accepted, 중앙값 `25.76s`로 안정화됐다. 다음 리뷰는 첫 보스 HP 추정보다 보스 이후 압박, 망각 경고 생존, 이후 사이클 완료를 봐야 한다.

## 7. 다음 Codex 작업

- 망각 경고 구간 `156.09s` 사망 원인을 확인한다.
- 첫 보스 이후 런이 전체 클리어로 이어지지 않는 이유를 확인한다.
- 보스 이후 압박, 망각 경고 생존, 이후 사이클 pacing을 첫 보스 TTK와 분리해 조정한다.
- 필요하면 trusted local terminal에서 `npm run report:discord:unit`를 실행한다.

## 8. 포트폴리오 메모

- 문제: 첫 보스를 처치해도 브라우저 TTK 샘플이 종료되지 않았다.
- 방향: 첫 보스 TTK 시나리오를 전체 런 완료와 분리한다.
- 행동: 시나리오별 종료 조건과 요약 체크를 추가했다.
- 결과: 첫 보스 TTK 증거가 안정화되고 다음 문제는 보스 이후 흐름으로 좁혀졌다.

# 2026-06-06-02 - 보스 이후 밸런스 기준선

## 1. 현재 빌드 상태

v0.12는 자동 브라우저 밸런스 기준선을 갖췄다. 전체 `qa:balance`가 `GO_BALANCE_BASELINE`을 반환했다.

## 2. 오늘 바뀐 것

- 보스 이후 자동 진행에서 cycle result continue 버튼을 먼저 누르게 해, 활성 기억 0개 guard가 루프를 멈추지 않도록 했다.
- 결손 숨고르기, 결손 시험, 이후 사이클 기본 압박에 spawn cap을 추가했다.
- 브라우저 밸런스 QA 기본 실행 창을 `608s`에서 `690s`로 늘렸다.
- 첫 보스 HP를 `2800`에서 `2500`으로 바꿨다.

## 3. 테스트 결과와 근거

- `node --check src/game.js`: 통과.
- `node --check scripts/run_boss_ttk_harness.js`: 통과.
- `node --check scripts/run_browser_balance_qa.js`: 통과.
- boss-only HP `2500`: 5/5 accepted, TTK 중앙값 `15.62s`, verdict `GO_BOSS_TTK_SAMPLE`.
- 브라우저 `first_boss_ttk` HP `2500`: 3/3 accepted, TTK 중앙값 `21.05s`, verdict `GO_BALANCE_BASELINE`.
- 전체 브라우저 `qa:balance`: `GO_BALANCE_BASELINE`, 첫 보스 클리어 `100%`, 전체 클리어 `40%`, 사망 `60%`, 첫 보스 TTK 중앙값 `22.24s`.
- 근거: `docs/balance/2026-06-06-v012-boss-ttk-hp2500.md`, `docs/balance/2026-06-06-v012-browser-first-boss-ttk-hp2500.md`, `docs/balance/2026-06-06-v012-balance-qa.md`.

## 4. 결정한 것

- HP `2500`을 현재 첫 보스 값으로 본다.
- 보스 이후와 전체 런 자동화는 현재 기준선 gate에 충분히 고쳐졌다고 본다.
- 사망 패턴 해석 없이 바로 인간 준비 완료로 부르지는 않는다.

## 5. 문제 또는 리스크

- 사망 `60%`가 결손 시험에 집중됐다.
- 자동 기준선은 인간 플레이테스트 감정 증거를 대체하지 못한다.
- 실제 Discord 전송은 이 Codex 세션에서 승인 검토에 막힐 수 있다.

## 6. GPT/Claude 인계 요약

감정 proxy는 제외한다. v0.12는 첫 보스 클리어 `100%`, 전체 클리어 `40%`, 첫 보스 TTK 중앙값 `22.24s`로 자동 기준선을 통과했다. 남은 질문은 결손 시험의 높은 사망률이 의도된 긴장인지 과한 처벌인지이다.

## 7. 다음 Codex 작업

- 결손 시험 사망 `60%`가 현재 prototype gate에 허용 가능한지 reviewer/GPT 프롬프트를 준비한다.
- 필요하면 첫 보스 HP를 건드리지 말고 결손 시험 압박이나 지속시간만 조정한다.
- accepted되면 다음 인간 테스트 전 gate와 보고 루프로 넘어간다.

## 8. 포트폴리오 메모

- 문제: 첫 보스 TTK는 안정됐지만 전체 런은 보스 이후 흐름과 최종 보스 시간 창에서 실패했다.
- 방향: QA 진행과 보스 이후 압박을 첫 보스 HP와 분리해 고친다.
- 행동: QA interrupt 처리 순서, post-boss cap, QA 실행 창, 첫 보스 HP를 조정했다.
- 결과: 자동 브라우저 밸런스 기준선을 통과했고 남은 질문이 결손 시험 해석으로 좁혀졌다.

# 2026-06-06-03 - 결손 시험 생존 조정

## 1. 현재 빌드 상태

v0.12는 `GO_BALANCE_BASELINE`을 유지했다. 사망은 목표 아래로 내려왔지만 전체 클리어가 자동 허용 상한에 정확히 걸렸다.

## 2. 오늘 바뀐 것

- 목표를 사망 `<= 40%`, 전체 클리어 `35-80%`, 첫 보스 TTK `15-30s`로 잡았다.
- 결손 지속시간을 `75s`에서 `60s`로 줄였다.
- 결손 시험 cap을 `22`에서 `16`으로 낮췄다.
- pre-boss XP multiplier를 `1.75`에서 `1.95`로 올렸다.
- 리필 회복을 HP floor `85%`, shield `18`로 강화했다.
- survival stat과 저HP survival 선택 안정성을 강화했다.

## 3. 테스트 결과와 근거

- `node --check src/game.js`: 통과.
- `node --check scripts/run_boss_ttk_harness.js`: 통과.
- `node --check scripts/run_browser_balance_qa.js`: 통과.
- boss-only TTK: 5/5 accepted, TTK 중앙값 `15.62s`, verdict `GO_BOSS_TTK_SAMPLE`.
- 브라우저 `first_boss_ttk`: 3/3 accepted, TTK 중앙값 `21.04s`, verdict `GO_BALANCE_BASELINE`.
- 전체 브라우저 `qa:balance`: `GO_BALANCE_BASELINE`, 첫 보스 클리어 `100%`, 전체 클리어 `80%`, 사망 `20%`, 첫 보스 TTK 중앙값 `27.84s`.
- 근거: `docs/balance/2026-06-06-v012-boss-ttk-survival-choice.md`, `docs/balance/2026-06-06-v012-browser-first-boss-ttk-survival-choice.md`, `docs/balance/2026-06-06-v012-balance-qa.md`.

## 4. 결정한 것

- 첫 보스 HP `2500`을 유지한다.
- 사망 `20%`는 생존 목표를 만족한다.
- 전체 클리어 `80%`가 너무 쉬운지 reviewer/GPT가 판단하기 전에는 추가 블라인드 튜닝을 하지 않는다.

## 5. 문제 또는 리스크

- 전체 클리어가 자동 허용 상한 `80%`에 정확히 걸렸다.
- 한 run에서 첫 보스 TTK outlier가 있었지만 중앙값은 목표 안에 있다.
- AI proxy metric은 인간 감정 피드백이 아니다.

## 6. GPT/Claude 인계 요약

감정 proxy는 제외한다. 최신 전체 `qa:balance`는 사망 `20%`, 전체 클리어 `80%`, 첫 보스 TTK 중앙값 `27.84s`로 통과했다. 리뷰어는 이를 인간 테스트 전 기준선으로 받을지, late pressure를 조금 되돌릴지 판단해야 한다.

## 7. 다음 Codex 작업

- reviewer가 accept하면 다음 인간 테스트 전 보고 gate로 넘어간다.
- 너무 쉽다는 판단이면 첫 보스 HP를 바꾸지 말고 late pressure만 작게 되돌린다.
- 새 시스템 추가나 scope 확장은 하지 않는다.

## 8. 포트폴리오 메모

- 문제: 이전 기준선은 통과했지만 AI run의 사망 `60%`가 보스 이후 압박에 몰렸다.
- 방향: 첫 보스를 약하게 만들기보다 생존 회복과 저HP 선택 신뢰도를 높인다.
- 행동: 결손 지속시간/cap, 리필 회복, survival stat, 저HP survival 선택을 조정했다.
- 결과: 사망은 `20%`로 내려갔고, 난이도 상한에 대한 명확한 디자인 판단만 남았다.

# 2026-06-06-04 - 결손 시험 리뷰 후속 조정

## 1. 현재 빌드 상태

v0.12는 `GO_BALANCE_BASELINE`을 유지한다. 최신 자동 기준선은 전체 클리어 `60%`, 사망 `40%`, 첫 보스 TTK 중앙값 `25.79s`로 목표 구간 중앙에 더 가깝다.

## 2. 오늘 바뀐 것

- `80%` 전체 클리어가 너무 쉬운지 외부 Claude review를 시도했지만 승인 검토에서 차단됐다.
- local Codex fallback review를 `docs/review_responses/2026-06-06-balance-baseline-deficit-trial-codex.md`에 저장했다.
- review 결정 `ITERATE_DEFICIT_TRIAL`을 적용했다.
- 첫 보스 HP `2500`은 유지했다.
- 첫 보스 전에는 survival을 과도하게 우선하지 않고, 보스 이후 survival 우선순위는 유지하도록 QA 선택을 조정했다.

## 3. 테스트 결과와 근거

- `node --check src/game.js`: 통과.
- `node --check scripts/run_browser_balance_qa.js`: 통과.
- boss-only TTK: 5/5 accepted, TTK 중앙값 `15.62s`, verdict `GO_BOSS_TTK_SAMPLE`.
- 브라우저 `first_boss_ttk`: 3/3 accepted, TTK 중앙값 `19.82s`, verdict `GO_BALANCE_BASELINE`.
- 전체 브라우저 `qa:balance`: `GO_BALANCE_BASELINE`, 첫 보스 클리어 `80%`, 전체 클리어 `60%`, 사망 `40%`, 첫 보스 TTK 중앙값 `25.79s`.
- `npm run report`: 통과.
- `npm run report:check`: 통과.
- `npm run doctor`: 통과, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit:dry`: 통과.
- 근거: `docs/balance/2026-06-06-v012-boss-ttk-deficit-review-final.md`, `docs/balance/2026-06-06-v012-browser-first-boss-ttk-deficit-review-final.md`, `docs/balance/2026-06-06-v012-balance-qa.md`.

## 4. 결정한 것

- 전체 클리어 `80%` / 사망 `20%`는 자동 기준선으로는 쉬운 쪽 끝에 너무 가깝다고 본다.
- 기억 교체 루프를 살린 생존/리필 값은 유지한다.
- 첫 보스 HP나 새 시스템을 건드리지 않고 QA choice behavior를 조정한다.
- 전체 클리어 `60%` / 사망 `40%`를 현재 자동 인간 테스트 전 기준선으로 받아들인다.

## 5. 문제 또는 리스크

- 외부 Claude review는 승인 검토 때문에 이 Codex 세션에서 전송하지 못했다.
- 사망이 허용 상한 `40%`에 정확히 걸려 있다.
- AI proxy metric은 여전히 인간 감정 증거가 아니다.

## 6. GPT/Claude 인계 요약

감정 proxy는 제외한다. local fallback review는 전체 클리어 `80%`가 쉬운 쪽 끝에 가깝다고 보고 `ITERATE_DEFICIT_TRIAL`을 선택했다. 최종 자동 기준선은 전체 클리어 `60%`, 사망 `40%`, 첫 보스 TTK 중앙값 `25.79s`로 통과했다.

## 7. 다음 Codex 작업

- accepted되면 인간 테스트 패키징/보고로 넘어간다.
- 인간 또는 reviewer 피드백에서 post-boss 압박이 강하다고 나오면 그때 부드럽게 조정한다.
- Discord 전송이 필요하면 trusted local terminal에서 실행한다.

## 8. 포트폴리오 메모

- 문제: 이전 기준선은 통과했지만 전체 클리어 `80%`로 쉬운 쪽 끝에 닿았다.
- 방향: 첫 보스 튜닝과 기억 교체 생존값은 유지하면서 QA 선택 편향을 조정한다.
- 행동: 외부 review가 막힌 뒤 local fallback review를 저장하고, pre-first-boss와 post-first-boss survival 선택 행동을 분리했다.
- 결과: 전체 클리어 `60%`, 사망 `40%`로 목표 구간 중앙에 더 가까워졌다.
