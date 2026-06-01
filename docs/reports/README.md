# Reports

이 폴더는 사람에게 보여줄 개발 보고서 원본과 HTML 결과물을 보관한다.

## Usage

Markdown 원본을 먼저 작성한다.

```bash
npm run report
```

기본 명령은 `docs/reports/2026-06-01.md`를 읽어 `docs/reports/2026-06-01.html`을 생성한다.

특정 파일을 지정하려면:

```bash
node scripts/build_report.js docs/reports/2026-06-01.md
```

## Writing Rule

보고서는 나중에 포트폴리오와 개발일지로 재사용할 수 있어야 한다. 단순 작업 목록보다 아래 흐름을 남긴다.

- 문제: 어떤 불확실성을 풀려고 했는가?
- 방향: 어떤 검증 구조를 선택했는가?
- 실행: 무엇을 만들고 테스트했는가?
- 결과: 수치와 관찰이 무엇을 말했는가?
- 다음 판단: GPT 또는 사람이 무엇을 결정해야 하는가?

