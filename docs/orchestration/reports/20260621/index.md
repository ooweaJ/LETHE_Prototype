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

# 2026-06-21-02 - 남은 VFX 20종 생성 및 Unity 임포트

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드 상태다. 이번 작업은 런타임 로직 변경 없이, 남아 있던 무기/기억/잔향/궁극 VFX 스프라이트를 모두 생성해 Unity에서 Sprite로 인식되도록 정리한 작업이다.

## 2. 오늘 바뀐 것

- 남은 prompt-sheet VFX 20종을 생성했다.
- 무기/피격 VFX 5종, 액티브 기억 VFX 6종, 잔향 VFX 6종, 궁극 VFX 3종을 추가했다.
- chroma 원본은 `LETHE/Assets/_dev/Art/Source/`에 보존했다.
- 최종 투명 PNG는 `LETHE/Assets/_dev/Art/Sprites/` 하위 카테고리에 저장했다.
- 검수용 시트 `LETHE/Assets/_dev/Evidence/remaining_vfx_prompt_sheet_20260621.png`를 추가했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: warning 0개, error 0개로 통과.
- Unity MCP compile error count: 0.
- Unity AssetDatabase에서 새 VFX 20/20개 확인.
- Unity TextureImporter 설정에서 최종 PNG 20/20개가 Sprite 타입임을 확인.

## 4. 결정한 것

VFX 생성 backlog는 여기서 닫고, 다음부터는 새 이미지를 더 뽑기보다 런타임 연결과 스케일/알파/지속시간 튜닝으로 넘어간다.

## 5. 문제 또는 리스크

이미지는 생성됐지만 아직 모든 런타임 발동 경로에 연결된 것은 아니다. 특히 `brand_echo`는 원본 표식과 실루엣이 비슷해서 Play Mode에서 더 짧고 어둡게 쓰는 튜닝이 필요하다.

## 6. GPT/Claude 인계 요약

Codex가 prompt sheet 기준으로 남은 VFX 20종을 모두 생성했고, Unity Sprite import까지 확인했다. 다음 리뷰/기획 판단은 “어떤 VFX를 procedural placeholder에서 sprite로 교체할지”와 “게임 화면에서 너무 크거나 복잡한 효과를 줄일지”에 집중하면 된다.

## 7. 다음 Codex 작업

`V1GameManager`와 관련 VFX spawn/profile 경로에 새 스프라이트를 연결한다. 우선 무기 swing arc와 hit spark를 기본 공격에 연결하고, 그 다음 6기억/6잔향/3궁극을 기존 procedural VFX와 교체 또는 혼합한다.

## 8. 포트폴리오 메모

- 문제: 프로토타입의 기억/잔향 효과 다수가 procedural placeholder라 “게임답게 만든 에셋 세트”로 보이지 않았다.
- 방향: prompt sheet를 기준으로 전투 VFX 언어를 통일하고, 생성 원본과 최종 Sprite를 추적 가능하게 분리했다.
- 행동: 20개 VFX를 생성, 투명화, Unity Sprite import, 컨택트시트 검수까지 진행했다.
- 결과: 이제 LETHE의 8기억/8잔향/4궁극 구조를 시각적으로 연결할 수 있는 전용 VFX 재료가 확보됐다.
