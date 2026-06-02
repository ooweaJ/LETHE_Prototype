## 결론

- **ITERATE_BEFORE_TEST** — AI 지표는 통과 수준이나, browser-proven 미달성 상태라 사람 테스트로 넘기기 부적절하다.

## 이유

1. **AI 지표는 강하다.** alphaFunScore 0.8846, regretRate 0.8073, irritationRate 0.0104 — 망각이 짜증이 아니라 아쉬움으로 작동한다는 봇 기반 신호가 충분히 나왔다.
2. **browser-proven 미달성이 블로커다.** `npm run qa:tactical`이 Chrome CDP 타임아웃 + port EPERM으로 실패했다. 전술 집중 기능 자체가 실제 브라우저에서 동작한다는 확인이 없으면 사람 테스트를 열어서는 안 된다.
3. **구현 범위는 적절했다.** 전술 집중은 WP3 Slice A 정의 내에서 완결됐고, 스코프 이탈 흔적이 없다.
4. **Chrome transport 문제는 환경 의존적이다.** 코드 오류가 아니라 CI/로컬 환경의 CDP 포트 접근 문제이므로, trusted local 환경에서 재실행하면 해결될 가능성이 높다.
5. **`멈춘 초침`이 유독 적게 삭제됐다(14회).** 다른 기억 대비 절반 이하다. 기억 삭제 분포가 극단적으로 쏠리면 선택지가 좁아진 느낌을 줄 수 있어, 이후 사람 테스트에서 관찰 포인트로 남겨야 한다.

## 앞으로 해야 할 일

- [ ] trusted local에서 `npm run qa:tactical`을 재실행하고, PASS 여부를 `docs/CODEX_STATUS.md`에 기록한다. Chrome CDP 타임아웃이 반복되면 스크립트의 타임아웃 값을 늘리거나 headless 플래그를 조정해 원인을 좁힌다.

*(이것만이 다음 루프의 작업이다. 추가 기능은 browser-proven 이후에 논의한다.)*

## 테스트 기준

- **AI 테스트 기준:** `npm run qa:tactical` PASS, tacticalFocus 발동 로그가 QA payload에 정상 기록되는지 확인.
- **사람 플레이테스트 관찰 기준:** browser QA 통과 이후에 열린다. 통과 시 망각 직전 예측 행동, 망각 직후 감정(짜증 vs. 아쉬움), 전술 집중 단축키 발견 여부를 집중 관찰한다.

## 아직 만들지 말 것

- 새 기억 추가
- 전술 집중 UI 개선
- 새 보스/스테이지
- 기억 교환/잠금 시스템
