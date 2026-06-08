# 2026-06-08-08 - Plugin-Oriented Migration Prompt Update

## 1. Current build status

LETHE v0.12 remains the current controlled human-test candidate. This work changed only orchestration migration guidance.

## 2. What changed today

- Updated `EXISTING_PROJECT_MIGRATION_PROMPT.md` for a reusable personal AI plugin-style structure.
- Defined:
  - `docs/orchestration/interface/` for human-facing HTML,
  - `docs/orchestration/state/` for AI-facing Markdown,
  - `docs/orchestration/reports/` for people-facing HTML work-unit reports,
  - `docs/orchestration/devlog/` for AI/internal Markdown continuity,
  - `docs/orchestration/legacy/` for migration maps, archived docs, and pointers.
- Added rules for migrating existing docs so legacy project-management docs stop being normal source-of-truth files.

## 3. Test results and evidence

- `npm run report`: pass, generated 8 unit reports.
- `npm run report:check`: pass, 8 report units.
- `npm run report:discord:unit:dry`: pass, latest unit 08 summary generated.
- `npm run doctor`: pass, 50 pass / 0 warn / 0 fail.
- `npm run report:discord:unit`: blocked by approval reviewer because it would send workspace-generated report content and attachments to an external Discord webhook, which is untrusted by default and not explicitly approved by the user. Next trusted-local command: `npm run report:discord:unit`.

## 4. Decisions made

- People-facing docs should be HTML-first.
- AI-facing docs should stay concise Markdown.
- Devlogs can be appended by date, but current state should still live in short `state/` files.
- Existing docs outside orchestration should be migrated, archived, linked, or pointer-only.

## 5. Problems or risks

- This did not physically move LETHE's current orchestration files yet.
- Destructive cleanup of old docs should wait for explicit approval and a migration map.
- Discord actual send was blocked by approval policy in this Codex session.

## 6. GPT handoff summary

The shared migration prompt now defines the orchestration system as a personal AI plugin structure with separate human HTML, AI Markdown, reports, devlog, and legacy migration zones.

## 7. Next Codex tasks

- Move LETHE's current `docs/orchestration/*.html` into `docs/orchestration/interface/`.
- Move current core Markdown into `docs/orchestration/state/`.
- Add `docs/orchestration/legacy/MIGRATION_MAP.md`.
- Update AGENTS and internal links.

## 8. Portfolio notes

- Problem: the orchestration interface needed a reusable cross-project folder contract.
- Direction: split human-facing and AI-facing surfaces.
- Action: updated the migration prompt.
- Result: future projects can adopt the same AI-plugin-like project dashboard convention.
