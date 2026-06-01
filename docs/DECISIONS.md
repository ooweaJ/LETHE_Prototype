# Decision Log

프로젝트 방향에 영향을 주는 결정을 기록한다.

## 2026-06-01

### D001. HTML v0.1은 코어 검증에 집중한다

- Decision: 완성 게임이 아니라 망각 1회 감정 검증용 프로토타입으로 제한한다.
- Reason: “망각됐을 때 아쉬운가, 짜증나는가”가 1차 검증 질문이다.
- Consequence: 메타 진행, 상점, 최종보스, 기억 12종 이상은 만들지 않는다.

### D002. AI 테스트는 인간 감정의 대체가 아니라 사전 필터로 사용한다

- Decision: Q1/Q2 자동값은 봇 기반 감정 프록시로만 해석한다.
- Reason: 실제 상실감과 납득감은 사람 테스트에서만 확정할 수 있다.
- Consequence: AI 테스트는 밸런스 위험, 삭제 쏠림, 피벗 가능성 확인에 사용한다.

### D003. Codex와 GPT 역할을 분리한다

- Decision: Codex는 구현/검증, GPT는 기획 판단/우선순위 결정에 사용한다.
- Reason: 코드 변경과 테스트는 Codex가 효율적이고, 상위 기획 판단은 별도 검토 루프가 필요하다.
- Consequence: `CODEX_STATUS.md`, `GPT_REVIEW_PROMPT.md`, `NEXT_TASKS.md`를 통해 반자동 협업 루프를 운영한다.

