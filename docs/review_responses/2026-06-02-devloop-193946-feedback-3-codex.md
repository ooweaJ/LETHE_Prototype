판단: 이번 구현은 `docs/NEXT_TASKS.md`의 현재 v0.9 최우선 미완료 항목과 범위 제한에 맞습니다.

`BrowserQaTransportError` 진단 추가는 새 gameplay가 아니라 “반복 transport 실패를 gameplay 실패와 구분”하는 QA gate 보강입니다. 현재 문서상 WP2 Slice B는 구현 완료이지만 browser-proven이 아니며, WP3 Slice A와 사람 테스트는 `trusted-local npm run qa:postloss` 전까지 막혀 있습니다. 이번 작업은 이 상태를 더 명확히 만든 것이므로 적합합니다.

AI test도 방향은 좋습니다. `GO_CANDIDATE`, irritation `1%` 수준, restart `0.90`, prediction match `0.8542`는 사람 테스트 진입 가능성을 지지합니다. 다만 `earlyChoiceInterest 0.6534`, `echoPivotScore 0.6554`, `postLossChallengeScore 0.6687`은 아직 약한 축이므로 Unity 전환 근거로 과장하면 안 됩니다.

**실패/리스크**

- `qa:postloss`가 아직 브라우저 증거를 만들지 못했습니다.
- 실패 지점은 gameplay assertion이 아니라 Chrome/CDP transport입니다.
- 같은 문제가 sandbox 밖 trusted local에서도 반복되면, WP3 구현 전에 환경 blocker 판단이 필요합니다.
- 현재 수치는 AI proxy이며 실제 인간 감정, 밸런스, Unity 전환 근거는 아닙니다.

**다음 루프의 가장 작은 작업 1개**

trusted local에서 `npm run qa:postloss`만 실행합니다.

조건은 단순하게 잡아야 합니다. 통과하면 WP2 Slice B를 browser-proven으로 올리고, 그 다음에만 WP3 Slice A 최소 tactical agency로 이동합니다. 같은 transport 실패가 나면 `npm run qa:postloss -- --timeout-ms 30000`을 한 번만 재시도하고, 그래도 실패하면 새 gameplay 구현 없이 `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`로 환경 blocker 판단을 받습니다.

**앞으로 해야 할 일**

1. `trusted-local npm run qa:postloss` 실행.
2. 통과 시 결과를 `CODEX_STATUS`, `NEXT_TASKS`, devlog, report에 기록.
3. 실패가 gameplay assertion이면 post-loss flow만 최소 수정.
4. 실패가 transport면 WP3, 사람 테스트, UI/밸런스 변경을 모두 보류하고 환경 blocker 판단으로 넘김.
5. 범위 확장 금지: 새 기억, 새 슬롯, 상점, 메타 성장, 새 지역, 새 적, 새 무기 추가 없음.
