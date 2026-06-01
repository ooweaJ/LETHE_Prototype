# LETHE: 망각의 군주 HTML Alpha v0.1

기획서 `docs/LETHE_망각의_군주_완성형_기획서_v6.docx` 기준으로 만든 코어 검증용 HTML 프로토타입입니다.

목표는 완성 게임이 아니라, 보스 처치 후 “가장 의존한 기억을 잃는 경험”이 아쉬운지, 짜증나는지, 납득되는지 확인하는 것입니다.

## 실행 방법

아래 둘 중 하나로 실행할 수 있습니다.

1. `index.html`을 브라우저로 직접 열기
2. 로컬 서버로 열기

```bash
python3 -m http.server 8000
```

그 다음 브라우저에서 `http://localhost:8000`을 엽니다.

## 조작

- 이동: `WASD` 또는 방향키
- 기본 공격: 자동
- 기억 발동: 자동
- 목표: 일반 전투를 지나 보스 `기억을 씹는 자`를 처치한 뒤 질문과 설문에 답하기

## AI 플레이어블 테스트

`LETHE_alpha_ai_test_system_v01.zip`을 `alpha_test/`로 통합했습니다. 이 도구는 실제 브라우저 플레이를 렌더링하지 않고, 기획서의 전투/망각/잔향 규칙을 수치 모델로 돌려 “사람에게 보여줄 만한가”를 빠르게 점검합니다.

빠른 테스트:

```bash
npm run ai:test:quick
```

기본 테스트:

```bash
npm run ai:test
```

파라미터 스윕:

```bash
npm run ai:sweep
```

주요 산출물:

- `alpha_test/outputs/default/summary.json`
- `alpha_test/outputs/default/alpha_summary.md`
- `alpha_test/outputs/default/alpha_report.html`
- `alpha_test/outputs/default/runs.csv`
- `alpha_test/outputs/default/raw_runs.json`

리포트의 Q1/Q2는 실제 인간 감정이 아니라 봇 기반 프록시입니다. 최종 판단은 8~12명 규모의 사람 플레이테스트로 확인해야 합니다.

## 개발 기록

이 프로젝트는 Codex와 GPT의 역할을 분리해 운영합니다.

- Codex: 구현, 테스트, Git, 상태 보고
- GPT: 기획 검토, 우선순위 결정, 테스트 해석

주요 문서:

- `AGENTS.md`: Codex가 이 저장소에서 따를 작업 규칙
- `docs/CODEX_STATUS.md`: 현재 구현 상태와 테스트 결과
- `docs/GPT_REVIEW_PROMPT.md`: GPT에게 넘길 검토 프롬프트
- `docs/NEXT_TASKS.md`: 다음 작업 큐
- `docs/DECISIONS.md`: 기획/기술 결정 로그
- `docs/GIT_CONVENTION.md`: 커밋 메시지와 push 타이밍 기준
- `docs/devlog/`: 날짜별 개발일지
- `docs/reports/`: 사람용 개발 보고서 Markdown/HTML
- `docs/PLAYTEST_NOTES.md`: 사람 플레이테스트 기록

보고서 HTML 생성:

```bash
npm run report
```

## 구현 범위

- 무기 2종: 절단쌍검, 장송대검
- 기억 6종 중 3슬롯 선택
- 적 4종: 침식자, 떠도는 눈, 쪼개진 자, 공허 사제
- 보스 1종 3페이즈: 기억을 씹는 자
- 의존도 기반 망각
- 망각 시 잔향 스탯 획득
- 보스 처치 후 질문 UI
- 망각 결과 화면
- Q1/Q2 0~4 설문
- JSON 로그 다운로드

## 제외 범위

- 메타 진행
- 상점
- 최종보스
- 기억 12종 이상
- 다영역 런 구조
