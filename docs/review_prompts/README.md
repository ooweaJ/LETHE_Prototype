# Review Prompts

Use this folder only when a daily report needs planning review from GPT, Claude, or another reviewer.

Recommended file name:

```text
docs/review_prompts/YYYY-MM-DD.md
```

Keep the prompt separate from the HTML report:

- The report remains the project record.
- The prompt can be copied directly into GPT or Claude.
- The same report can have different prompts for system planning, emotional review, or playtest interpretation.
- Discord can attach the prompt file only when it exists.

Suggested structure:

```markdown
# LETHE Planning Review - YYYY-MM-DD

## Target

GPT or Claude

## Context

- Current build:
- What changed:
- Test result:
- Problem:

## Questions

1. 
2. 
3. 

## Requested Output

- Verdict:
- Next 1-3 tasks:
- Do not add:
```
