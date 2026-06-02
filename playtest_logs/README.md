# Playtest Logs

Put downloaded human playtest JSON logs here before running:

```bash
npm run playtest:summary
```

Raw JSON files in this folder are ignored by git. Keep only anonymized tester IDs such as `T01`, `T02`, and session IDs such as `S01`.

The summary script reads these logs and writes:

- `docs/playtest_summaries/YYYY-MM-DD.md`
- `docs/review_prompts/YYYY-MM-DD-human-playtest.md`

Do not store personally identifiable information in this folder.
