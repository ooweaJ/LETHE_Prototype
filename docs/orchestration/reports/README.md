# Reports

이 폴더는 사람에게 보여줄 한국어 개발 보고서 원본과 HTML 결과물을 보관한다.

날짜별 대표 페이지를 기준으로 읽는다.

```text
docs/orchestration/reports/
  index.html
  YYYYMMDD/
    index.md
    index.html
    units/
      YYYY-MM-DD-NN-slug.md
      YYYY-MM-DD-NN-slug.html
      YYYY-MM-DD-NN-slug.summary.json
```

## Usage

Markdown 원본을 먼저 작성한다.

```bash
npm run report
```

기본 명령은 최신 `docs/orchestration/reports/YYYYMMDD/index.md`를 읽어 같은 폴더의 `index.html`과 `units/`를 생성한다.

특정 파일을 지정하려면:

```bash
node scripts/build_report.js docs/orchestration/reports/20260601/index.md
```

## Writing Rule

보고서는 사용자가 바로 읽는 화면이므로 한국어로 쓴다. 나중에 포트폴리오와 개발일지로 재사용할 수 있어야 한다. 단순 작업 목록보다 아래 흐름을 남긴다.

- 문제: 어떤 불확실성을 풀려고 했는가?
- 방향: 어떤 검증 구조를 선택했는가?
- 실행: 무엇을 만들고 테스트했는가?
- 결과: 수치와 관찰이 무엇을 말했는가?
- 다음 판단: GPT 또는 사람이 무엇을 결정해야 하는가?

## Unit Rule

- 날짜별 `index.md`는 그날의 작업을 모아서 보여주는 사람용 대표 문서다.
- 각 top-level heading은 `# YYYY-MM-DD-NN - 제목` 형식을 쓴다.
- `units/`는 `npm run report`가 생성하는 상세 단위 파일이다.
- Discord 작업 단위 보고는 일일 전체 문서가 아니라 최신 `units/*.html`과 그 `.summary.json`을 기준으로 한다.
- 과거 기록 안의 예전 `docs/reports/` 경로는 역사 기록으로 남길 수 있지만, 새 보고 원본은 반드시 `docs/orchestration/reports/YYYYMMDD/index.md`다.

