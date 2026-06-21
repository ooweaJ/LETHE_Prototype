# 2026-06-21-01 - 스프라이트 프롬프트 시트 정리와 핵심 VFX 교체

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 새 전투 기능이 아니라, 앞으로 만들 모든 스프라이트의 생성 기준을 고정하고 이미 들어간 핵심 VFX 일부를 같은 기준으로 교체한 작업이다.

## 2. 오늘 바뀐 것

- 새 제작 기준 문서 `docs/design/LETHE_SPRITE_PRODUCTION_PROMPTS.md`를 추가했다.
- 첨부 프롬프트 시트를 기반으로 공통 base prompt, 팔레트, 파일별 prompt, 제작 순서, 검증 기준을 정리했다.
- 기존 칼무리/혈반/피의 칼폭풍 핵심 VFX 8개를 새 기준으로 재생성해 같은 파일명으로 교체했다.
- chroma source PNG는 `LETHE/Assets/_dev/Art/Source/`에 보존했다.
- 검수용 contact sheet를 추가했다:
  - `LETHE/Assets/_dev/Evidence/core_vfx_prompt_sheet_refresh_20260621.png`

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, warning 0개, error 0개.
- Unity MCP:
  - selected instance: LETHE, port 7890.
  - active scene: `Dev_Prototype_v1`.
  - compile error count: 0.
  - AssetDatabase Kalmuri textures: 3.
  - AssetDatabase Blood textures: 3.
  - AssetDatabase Ultimate textures: 2.

## 4. 결정한 것

앞으로 스프라이트는 즉흥 prompt가 아니라 `LETHE_SPRITE_PRODUCTION_PROMPTS.md`를 기준으로 만든다. 기존 칼무리/혈반도 유지가 아니라 새 기준으로 교체하는 방향을 택했다.

## 5. 문제 또는 리스크

이번 검증은 파일 교체, import 인식, 컴파일 검증이다. 실제 인게임 크기, 밝기, 발동 타이밍은 Play Mode에서 따로 확인해야 한다.

## 6. GPT/Claude 인계 요약

Codex가 첨부 프롬프트 시트를 정리해 스프라이트 제작 기준 문서로 만들고, 기존 핵심 VFX 8종을 같은 기준으로 교체했다. 다음은 무기 궤적/히트 스파크와 남은 6기억/6잔향 VFX를 순서대로 채우면 된다.

## 7. 다음 Codex 작업

다음 배치는 `spr_dual_blade_swing_arc_01/02`, `spr_greatsword_cleave_arc_01`, `spr_hit_spark_cyan_01`, `spr_hit_spark_red_01` 제작이 우선이다. 그 다음 6기억 활성 VFX와 6잔향 VFX로 넘어간다.

## 8. 포트폴리오 메모

- 문제: 스프라이트 제작 기준이 흩어져 있어 기존 에셋과 새 에셋의 톤이 흔들릴 수 있었다.
- 방향: prompt source of truth를 만들고 핵심 VFX부터 같은 톤으로 교체한다.
- 행동: 제작 문서를 추가하고 칼무리/혈반/피의 칼폭풍 8개 sprite를 교체했다.
- 결과: 핵심 VFX가 더 통일된 청록 기억/붉은 혈반/궁극 혼합 톤으로 정리됐다.
