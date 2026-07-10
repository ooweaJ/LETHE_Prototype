# LETHE Portfolio Site Plan

Last updated: 2026-07-10

## Goal

Show LETHE as a project that started from planning and moved into a playable Unity prototype. The portfolio page should not read like a document dump. It should read as a case study: what problem was defined, what design decisions were made, how the prototype was built, how feedback changed the work, and what is playable now.

## Recommended Public Structure

1. Portfolio case-study page
   - Short, polished public story.
   - Uses the title key art as the first visual.
   - Links to deeper planning and technical documents instead of exposing every internal note.

2. Planning / game-design document
   - Explains why LETHE exists as a game concept.
   - Covers player fantasy, run loop, memory/forgetting/echo/resonance/ultimate structure, weapon identity, enemy/boss direction, VFX readability goals, and future final-boss direction.

3. Technical design document
   - Explains how the Unity prototype is built.
   - Covers project structure, core runtime classes, data-driven content definitions, VFX spawning/pooling, debug tools, QA commands, optimization constraints, and evidence-based iteration.

## Portfolio Page Flow

1. Hero
   - Use `LETHE/Assets/_dev/Art/Sprites/UI/spr_lethe_portfolio_title_01.png`.
   - One-line hook: "A memory-loss survivor action prototype built from design docs to Unity combat slice."

2. Project Overview
   - Genre: 2D survivor-like action roguelite prototype.
   - Role: planning, system design, Unity implementation, VFX direction, QA/reporting pipeline.
   - Current state: playable `_dev` prototype, still in visual/combat iteration.

3. Design Problem
   - The main problem is not just killing enemies. The player must feel growth, loss, regret, recovery, and eventual power through the memory-to-echo loop.

4. Design Solution
   - Memories are active growth.
   - Forgetting converts lost power into Echoes.
   - Echoes must become visible combat events, not passive stat text.
   - Resonance and Ultimate Echoes should make weak/passive memories worth chasing later.

5. Technical Implementation
   - Unity 2D `_dev` slice.
   - Data assets drive weapons, memories, echoes, and VFX tuning.
   - Debug panel supports fast weapon/memory/echo/boss testing.
   - QA menus and repeatable checks protect readability and performance.

6. Iteration Evidence
   - Show before/after examples:
     - Kalmuri Echo changed from generic flying blade to blue convergence/rift identity.
     - Blood Echo on Greatsword changed into two thin crescent follow-up slashes.
     - Boss attacks moved toward readable raid-style telegraphs.
     - Intro art added to match the mood instead of a plain UI shell.

7. Current Result
   - The project now has a playable combat shell, weapon selection intro, memory/echo experimentation, boss/debug routes, and portfolio-ready key art.

8. Next Steps
   - Direct-play review.
   - Finish GDD/TDD public versions.
   - Continue VFX clarity and boss-pattern polish.
   - Later: final boss based on the key art direction below.

## Final Boss Memo

The last boss should visually match the current key art instead of becoming a generic monster. Future direction:

- Working concept: "Lethe Gatekeeper" or "Forgotten Gate Sovereign".
- Silhouette: colossal ruined-gate figure, crown/halo, staff or suspended key, robe-like broken stone mass.
- Color language: cyan memory shards on one side, crimson echo/blade energy on the other, black water reflection underneath.
- Mood: ceremonial, ancient, and judgment-like, as if the boss is guarding the river of forgotten memories.
- Gameplay direction later: raid-style telegraphs with memory judgment patterns, not just higher HP.
- Scope note: do not implement until the current Unity `_dev` slice gets an explicit final-boss scope change.

## Document Direction

The planning document should make the game feel intentional. The technical document should make the implementation feel credible. The portfolio page should connect them into one readable story:

Problem -> Design choice -> Unity implementation -> Test evidence -> Iteration -> Current result.

## Next Codex Task

Draft two public-facing documents from the existing source docs:

- `LETHE_Game_Design_Document.md`
- `LETHE_Technical_Design_Document.md`

Then condense them into a portfolio case-study section that can be placed directly on the portfolio site.
