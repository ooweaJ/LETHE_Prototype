# Playtest Summaries

This folder stores generated summaries from human playtest JSON logs.

Generate the latest summary with:

```bash
npm run playtest:summary
```

Use `npm run playtest:summary:dry` to preview the summary without writing files.

The generated summary is evidence for the next Claude/GPT planning prompt. Raw human JSON logs stay under `playtest_logs/` and are ignored by git.
