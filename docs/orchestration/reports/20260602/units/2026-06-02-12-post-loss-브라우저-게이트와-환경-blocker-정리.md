# 2026-06-02-12 - Post-loss 브라우저 게이트와 환경 blocker 정리

## 1. Current build status

- WP2 Slice B remains implementation-complete and scope-valid.
- Latest status is `ITERATE_BEFORE_TEST`.
- Browser proof is still missing because the managed sandbox cannot complete Chrome/CDP post-loss QA.
- WP3 Slice A, people testing, balance changes, and UI/gameplay expansion remain blocked.

## 2. What changed today

- Hardened `scripts/run_browser_pressure_qa.js`:
  - CDP pipe remains the first path,
  - pipe target timeout falls back to remote-debugging-port,
  - fallback uses OS-confirmed `127.0.0.1` port,
  - pipe/port share stable headless Chrome flags,
  - repeated transport failures are reported as `BrowserQaTransportError`.
- Added `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`.
- Added `scripts/run_trusted_postloss_gate.js` and `npm run qa:postloss:trusted`.
- The trusted gate runs standard post-loss QA, retries once with 30000 ms only for transport failures, then points to the blocker prompt.
- Added `alpha_test/outputs/postloss-trusted-gate/latest.json` result logging with `status`, `transportFailure`, run summaries, `nextCommand`, and `blockerPrompt`.

## 3. Test results and evidence

- `node --check scripts/run_browser_pressure_qa.js`: passed.
- `node --check scripts/run_trusted_postloss_gate.js`: passed.
- `npm run doctor`: 44 pass / 0 warn / 0 fail after adding trusted gate.
- `npm run doctor:deep`: 64 pass / 0 warn / 0 fail.
- `npm run qa:postloss`: failed before gameplay evaluation.
- `npm run qa:postloss -- --timeout-ms 30000`: failed before gameplay evaluation.
- `npm run qa:pressure`: failed at the same transport stage during later sandbox runs.
- `npm run qa:postloss:trusted`: failed before gameplay evaluation after standard run plus 30000 ms retry.
- Latest gate JSON recorded `status: blocked`, `transportFailure: true`.
- Failure shape:
  - pipe: `Timed out waiting for CDP response to Target.getTargets`,
  - port fallback: `listen EPERM: operation not permitted 127.0.0.1`.

## 4. Decisions made

- Treat this as a browser automation transport blocker in the managed sandbox, not proof that post-loss gameplay failed.
- Keep WP3 blocked until trusted-local browser proof passes or an explicit environment-blocker decision exists.
- Use `npm run qa:postloss:trusted` as the next single gate command.
- Keep AI proxy metrics as planning evidence only.

## 5. Problems or risks

- Trusted-local behavior outside this sandbox is still unknown.
- Repeated QA tooling fixes can consume loop time without advancing gameplay.
- AI metrics remain positive but cannot replace browser/user proof.
- If the same transport failure repeats outside this sandbox, the project needs a decision rather than more blind QA retries.

## 6. GPT handoff summary

- WP2 Slice B is code-complete and scope-valid.
- The next executable unit is sandbox-outside trusted-local `npm run qa:postloss:trusted`.
- If it passes, record WP2 Slice B as browser-proven before starting only minimal WP3 Slice A.
- If it fails on gameplay assertions, fix only the post-loss flow.
- If it fails on the same transport path, use `docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md`.

## 7. Next Codex tasks

- Run `npm run qa:postloss:trusted` on a trusted local machine outside this managed sandbox.
- Inspect `alpha_test/outputs/postloss-trusted-gate/latest.json`.
- Update `docs/CODEX_STATUS.md`, `docs/NEXT_TASKS.md`, devlog, and report based on that result.
- Keep WP3, people testing, balance, and UI/gameplay expansion closed until the gate is resolved.

## 8. Portfolio notes

- Problem: WP2 had promising AI proxy metrics but lacked browser proof because automation failed before gameplay evaluation.
- Direction: distinguish gameplay failure from environment transport failure.
- Action: added transport diagnostics, fallback hardening, trusted gate wrapper, blocker prompt, and machine-readable gate result JSON.
- Result: the blocker is now explicit and repeatable; the next decision depends on trusted-local browser evidence.
