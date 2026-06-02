# LETHE AI Collaboration Case Study

## One-Line Summary

LETHE is an HTML prototype used to validate a roguelike forgetting loop before deciding whether the design deserves a Unity implementation.

## Problem

The first versions explained the forgetting loop, but user feedback said the early game felt too loose and lacked roguelike growth. If players are bored before the first forgetting event, the core emotional mechanic cannot be tested.

## Constraints

- Keep the prototype in HTML until the core loop is validated.
- Do not add meta progression, shops, final boss, extra memories, or multi-region structure.
- Use AI tests for repeatable proxy metrics.
- Use human tests for actual fun, regret, irritation, and Unity-transition confidence.

## AI Usage

AI is used as a planning and verification partner:

- Codex implements, tests, reports, and commits.
- Claude/GPT interpret AI and human test results.
- The AI simulator generates repeatable metrics.
- The planning pipeline turns test evidence into prompts and saves model responses.

## Developer Judgment

The team rejected broad content expansion and kept v0.5 narrow:

- denser early enemy packs,
- kill XP,
- run-only 3-choice stat growth,
- JSON `runGrowth` logging,
- no shops or permanent progression.

When automation failed, the test goal stayed fixed:

- Browser plugin failure was replaced with Chrome headless QA.
- External Claude transfer blocking was handled with prompt-only pipeline generation.
- OpenAI API fallback was removed because it did not match the user's actual workflow.

## Verification Evidence

- `npm run ai:test`: `GO_CANDIDATE`.
- `npm run ai:test:heavy`: `GO_CANDIDATE`.
- Chrome headless `?qa=fast,levelup`: level-up UI, combat resume, and `runGrowth` payload passed.
- Chrome headless with tester/session URL params: JSON payload included `playtest.testerId` and `playtest.sessionId`.
- Claude pipeline verdict: `GO_TO_HUMAN_TEST`.

## Current Result

v0.5 is ready for 5-8 human playtests. The next decision is not more implementation by default. The next decision is whether human results support HTML v0.6 iteration or Unity transition groundwork.

## Portfolio Framing

This project should not be described as “made with AI.”

Use this framing instead:

> I used AI as a controlled planning and verification partner. Codex handled implementation and tests, while Claude interpreted test evidence and helped decide whether the prototype should continue in HTML or move toward Unity. I recorded prompts, responses, failure cases, and verification evidence so the project shows not only the final build, but also how AI output was constrained, corrected, and validated.
