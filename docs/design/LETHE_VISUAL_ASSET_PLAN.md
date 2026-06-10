# LETHE Unity 첫 slice 비주얼 에셋 계획

최종 갱신: 2026-06-10

## 0. 이 문서의 위치

이 문서는 Unity 2D 프로젝트를 만들기 전에, 첫 slice에서 필요한 스프라이트/VFX 이미지 방향을 정리하는 문서다.

연결 문서:

- `LETHE_ECHO_FORM_TRANSFORMATION_SPEC.md`: 활성 기억, 잔향, 공명, 궁극의 화면 형태 문법.
- `LETHE_UNITY_ECHO_SYSTEM_PRD.md`: Unity 클래스, ScriptableObject, 프리팹, 이벤트 구조.
- `LETHE_WEAPON_MEMORY_ECHO_DETAIL.md`: 기억/잔향 레벨별 동작.

이 문서는 "아트 최종 납품서"가 아니라, Unity 첫 slice에서 어떤 이미지 파츠를 만들고 어떤 프리팹에 붙일지 정하는 아트-구현 연결 문서다.

## 1. 첫 콘셉트 이미지

생성 방식:

- Codex `imagegen` built-in tool.
- 용도: 첫 echo showcase용 2D 스프라이트/VFX 콘셉트 시트.
- 원본 생성 위치: Codex 기본 생성 이미지 폴더.
- 프로젝트 보관 위치: `docs/design/assets/lethe-first-echo-showcase-concept.png`.

참조 이미지:

```text
docs/design/assets/lethe-first-echo-showcase-concept.png
```

역할:

- 최종 투명 스프라이트가 아니라, Unity 첫 slice의 시각 기준.
- 이 시트를 바탕으로 개별 칼날, 칼선, 혈반, 회복 실, 궁극 폭풍 파츠를 다시 생성하거나 잘라낸다.
- Unity MCP를 연결한 뒤에는 이 문서를 기준으로 `Assets/Lethe/Art/`와 `Assets/Lethe/Prefabs/`에 import 대상과 prefab 역할을 나눈다.

## 2. 이미지에서 사용할 파츠

| 이미지 영역 | Unity 사용처 | 프리팹 후보 | 비고 |
| --- | --- | --- | --- |
| 좌상단 쌍검 | 절단쌍검 무기 아이콘/장착 스프라이트 방향 | `Weapon_DualBlades_Runtime` | 실제 게임 스프라이트는 더 단순한 실루엣으로 재가공 |
| 상단 중앙 칼날 고리 | 활성 칼무리와 +5 칼무리 잔향 방향 | `Memory_HungryBlades_Ring`, `Echo_Kalmuri_WeaponShard` | 활성은 독립 고리, 잔향은 무기 주변 파편으로 구분 |
| 우상단 반달 칼선 | 칼무리 잔향 +1~+3 | `Echo_Kalmuri_DelayedSlash`, `Echo_Kalmuri_LingerSlash` | 쌍검 타격 뒤 0.15초 지연 베기 |
| 중앙 혈반 문양 | 혈반 표식 | `Echo_Blood_Mark` | 적 몸에 붙는 작은 표식 |
| 중앙 우측 피꽃 변화 | 혈반 +5 각성 | `Echo_Blood_Bloom` | 혈반이 터지며 주변 출혈 전파 |
| 좌하단 붉은 실 | 회복 실 | `Echo_Blood_HealThread` | 플레이어에게 되돌아오는 얇은 Line/Trail |
| 우하단 붉은 칼날 폭풍 | 피의 칼폭풍 궁극 | `Ultimate_BloodBladeStorm` | 칼날 고리와 혈반 실이 같이 도는 클라이맥스 |

## 3. Unity 에셋 import 계획

초기 폴더:

```text
Assets/Lethe/Art/Concept/
Assets/Lethe/Art/Sprites/Weapons/
Assets/Lethe/Art/Sprites/Echoes/Kalmuri/
Assets/Lethe/Art/Sprites/Echoes/Blood/
Assets/Lethe/Art/Sprites/Ultimates/
Assets/Lethe/Art/Particles/
Assets/Lethe/Art/Materials/
```

첫 import:

```text
Assets/Lethe/Art/Concept/lethe-first-echo-showcase-concept.png
```

이 파일은 원화/방향 참조다. 런타임 프리팹에 직접 붙이는 최종 sprite atlas로 쓰지 않는다.

## 4. 실제 제작할 스프라이트/VFX 목록

### 무기

| 에셋 | 용도 | 형태 |
| --- | --- | --- |
| `spr_weapon_dual_blade_left` | 왼손 쌍검 | 단독 PNG, 투명 배경 |
| `spr_weapon_dual_blade_right` | 오른손 쌍검 | 단독 PNG, 투명 배경 |
| `spr_dual_blade_swing_arc_01` | 기본 베기 궤적 | 반투명 흰색/청색 칼선 |
| `spr_dual_blade_swing_arc_02` | 강한 두 번째 베기 | 약간 긴 붉은 칼선 |

### 칼무리

| 에셋 | 용도 | 형태 |
| --- | --- | --- |
| `spr_kalmuri_orbit_blade_01` | 활성 칼무리 고리 칼날 | 작은 금속 칼날 |
| `spr_kalmuri_echo_slash_01` | 잔향 +1~+3 지연 칼선 | 반달 칼선 |
| `spr_kalmuri_echo_shard_01` | 잔향 +4 무기 주변 파편 | 희미한 칼날 파편 |
| `spr_kalmuri_launch_blade_01` | 잔향 +5 발사 칼날 | 날아가는 작은 칼 |

### 혈반

| 에셋 | 용도 | 형태 |
| --- | --- | --- |
| `spr_blood_mark_01` | 적 몸 혈반 표식 | 붉은 원형 표식 |
| `spr_blood_mark_02` | 강화 혈반 표식 | 갈라진 붉은 표식 |
| `spr_blood_bloom_01` | 혈반 +5 피꽃 | 짧은 폭발 프레임 |
| `spr_heal_thread_tip_01` | 회복 실 끝점 | 작은 붉은 빛 |

### 궁극

| 에셋 | 용도 | 형태 |
| --- | --- | --- |
| `spr_blood_blade_storm_ring_01` | 피의 칼폭풍 바닥 원 | 얇은 붉은 원 |
| `spr_blood_blade_storm_blade_01` | 폭풍 칼날 | 흰 금속 + 붉은 잔상 |
| `spr_blood_blade_storm_thread_01` | 폭풍 회복 실 | 붉은 곡선 trail |

## 5. 프리팹 연결

| 프리팹 | 필요한 에셋 |
| --- | --- |
| `Weapon_DualBlades_Runtime` | 쌍검 좌/우, 기본 swing arc |
| `Memory_HungryBlades_Ring` | orbit blade, 희미한 trail material |
| `Echo_Kalmuri_DelayedSlash` | delayed slash sprite, hit flash profile |
| `Echo_Kalmuri_WeaponShard` | weapon shard sprite |
| `Echo_Kalmuri_LaunchBlade` | launch blade sprite, pooled projectile |
| `Echo_Blood_Mark` | blood mark sprite, enemy attachment anchor |
| `Echo_Blood_HealThread` | heal thread material, tip sprite |
| `Echo_Blood_Bloom` | blood bloom particle/sprite burst |
| `Ultimate_BloodBladeStorm` | storm ring, storm blade, thread trail, red light profile |

## 6. MCP / Unity 작업 방식

Unity 프로젝트가 생기고 Unity MCP가 연결되면, Codex는 이 문서를 기준으로 다음 작업을 수행한다.

1. `Assets/Lethe/Art/Concept/`에 콘셉트 시트를 import한다.
2. `Assets/Lethe/Art/Sprites/...`에 최종 스프라이트 파츠를 배치한다.
3. `Sprite Import Settings`를 2D sprite 기준으로 맞춘다.
4. `Prefab`을 만들고 각 sprite/material/particle을 연결한다.
5. `FeedbackProfile` ScriptableObject에 hitstop, flash, sound, light 강도를 연결한다.
6. `Slice_EchoShowcase.unity`에서 debug panel로 각 파츠를 즉시 확인한다.

주의:

- MCP는 Unity Editor가 열린 뒤 import, prefab 연결, scene 배치에 쓰는 도구다.
- 이미지 생성 자체는 Codex imagegen으로 하고, Unity MCP는 생성된 이미지를 프로젝트 에셋으로 연결하는 데 쓴다.
- 최종 투명 PNG는 chroma-key 제거 또는 별도 투명 출력 절차로 만든다.

## 7. 다음 이미지 생성 우선순위

1. 투명 배경 `spr_kalmuri_echo_slash_01`: 잔향 +1~+3용 반달 칼선.
2. 투명 배경 `spr_blood_mark_01`: 적 몸에 붙는 혈반 표식.
3. 투명 배경 `spr_heal_thread_tip_01` + line/trail material 방향.
4. 투명 배경 `spr_kalmuri_launch_blade_01`: +5 발사 칼날.
5. 투명 배경 `spr_blood_blade_storm_blade_01`: 궁극 폭풍 칼날.

이 다섯 개가 있으면 첫 Unity slice에서 "잔향이 전투 행동으로 보이는지"를 빠르게 확인할 수 있다.

## 8. 현재 콘셉트 이미지 평가

좋은 점:

- 칼날, 혈반, 피실, 폭풍이 한 장에서 구분된다.
- 팔레트가 흰 금속, 어두운 잔향, 붉은 피로 명확하다.
- `피의 칼폭풍`이 1+1이 아니라 칼날과 피실이 섞인 궁극처럼 보인다.

보완할 점:

- 런타임 스프라이트로 쓰기엔 디테일이 많아 축소 시 뭉칠 수 있다.
- 최종 sprite는 더 단순한 실루엣과 높은 대비가 필요하다.
- 각 파츠는 투명 배경으로 별도 생성해야 한다.
- 혈반 표식은 적 스프라이트 위에 붙어도 읽히도록 더 큰 중심 형태가 필요하다.
