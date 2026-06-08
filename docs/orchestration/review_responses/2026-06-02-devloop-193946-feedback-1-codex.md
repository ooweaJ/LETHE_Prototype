이번 구현은 `docs/NEXT_TASKS.md`의 v0.9 WP2 Slice B와 범위 제한에 맞습니다.

- 현재 문서상 WP2 Slice B의 요구는 “기존 전투 파라미터만 써서 최소 post-loss challenge 구현”입니다.
- 구현 결과도 새 기억, 새 슬롯, 상점, 메타, 지역, 무기 확장 없이 `결손 정비 -> 결손 압박` 흐름과 로그/AI 지표만 추가했습니다.
- AI 지표도 `GO_CANDIDATE`, 낮은 irritation, 높은 restart, 2기억 생존율 약 79%로 방향은 유효합니다.
- 다만 `qa:postloss`가 Chrome/CDP timeout으로 실패했으므로, 아직 “브라우저에서 검증 완료” 상태는 아닙니다.

핵심 판단은 `WP2 Slice B는 기획 범위 안에서 완료 후보이지만, trusted local 브라우저 QA 통과 전까지는 완전 종료로 보지 않는다`입니다.

**실패/리스크**

- `npm run qa:postloss` 실패는 Slice B 로직 실패라기보다 CDP 자동화 채널 문제로 보입니다. `qa:pressure`도 같은 지점에서 실패했기 때문입니다.
- 하지만 사람 테스트나 WP3 착수 근거로 쓰려면 post-loss 구간이 실제 브라우저에서 정상 도는지 한 번은 확인해야 합니다.
- `postLossChallengeScore` 0.67, `echoPivotScore` 0.65대는 “작동은 하지만 아주 강한 체감은 아님”에 가깝습니다. 지금 당장 튜닝하기보다는 WP3에서 플레이어 선택감을 붙인 뒤 다시 봐야 합니다.
- `Alpha Fun Score`가 0.8846/0.8879로 목표 0.89 근처지만 안정적으로 넘지는 못했습니다. 숫자상 GO 후보이지 Unity 전환 근거는 아직 아닙니다.

**다음 루프의 가장 작은 구현 작업 1개**

v0.9 Work Package 3 Slice A로, **전투 중 활성 기억 1개를 “집중 기억”으로 지정하는 최소 tactical agency**를 구현하는 것이 좋습니다.

요구 범위:

- 기존 활성 기억 2-3개 중 하나만 선택 가능.
- 새 기억, 새 슬롯, 새 성장, 상점, 메타 시스템 추가 금지.
- 선택한 기억은 짧은 시간 동안 기존 효과 발동/체감이 약간 더 잘 보이게 한다.
- 대신 해당 기억 의존도도 조금 더 올라가게 해서 “강하게 쓰면 잃을 때 아깝다”는 LETHE 핵심 감정과 연결한다.
- HUD의 기존 기억 슬롯을 클릭하거나 숫자키로 선택하는 정도로 제한한다.
- JSON/event에 `tacticalFocusChoices`, `focusedMemory`, `focusDuration`, `focusedMemoryLost` 정도만 기록한다.
- AI proxy에는 `tacticalAgencyScore` 또는 `focusRegretLink`처럼 선택과 망각 감정이 연결되는지 보는 지표를 추가한다.

**앞으로 해야 할 일**

1. trusted local에서 `npm run qa:postloss`를 먼저 재실행한다.
2. 통과하면 WP3 Slice A: 전투 중 “집중 기억” 지정만 구현한다.
3. 구현 후 `ai:test:quick`, `ai:test`, `doctor`, `report:check`, `qa:postloss` 또는 신규 `qa:focus`를 실행한다.
4. 지표 판단 기준은 `earlyChoiceInterest`, `echoPivotScore`, `regretRate`, `irritationRate`, `focusedMemoryLost`를 중심으로 본다.
5. 이 작업 후에도 사람 테스트 전에는 Unity 전환 판단을 하지 않는다. HTML에서 “내가 고른 기억을 강하게 썼고, 그래서 잃을 때 아까웠다”는 반응이 나오는지 먼저 확인해야 합니다.
