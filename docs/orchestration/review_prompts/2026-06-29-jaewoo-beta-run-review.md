# 2026-06-29 jaewoo 20분 베타런 직접 리뷰 기록지

## 목적

`Dev_Prototype_v1`을 20분 베타런 후보로 직접 플레이하고, 다음 Codex 작업 축을 하나만 고르기 위한 기록지다. 자동 QA는 기술 안정성만 확인한다. 이 문서는 손맛, 가독성, 템포, 감정 리듬을 사람이 판단하기 위한 체크리스트다.

## 플레이 전 기술 상태

- 대상 씬: `LETHE/Assets/_dev/Scenes/Dev_Prototype_v1.unity`
- Unity MCP 인스턴스: `LETHE`, port `7890`
- 재확인일: 2026-06-29
- Unity compile error count: `0`
- Unity scene missing references: `0`
- Unity asset missing references: `0`
- Unity console error count after QA: `0`
- QA PASS:
  - `LETHE/V1 Smoke/Start Dual Blades`
  - `LETHE/V1 Smoke/Start Greatsword`
  - `LETHE/V1 Smoke/M2 Loop`
  - `LETHE/V1 QA/VFX Matrix`
  - `LETHE/V1 QA/Blood Blade Storm`

참고: QA 중 `There are no audio listeners in the scene` log가 반복된다. 현재는 error가 아니므로 직접 리뷰를 막지는 않지만, 베타 플레이 polish debt로 기록한다.

## 리뷰 방법

1. `Dev_Prototype_v1`을 연다.
2. 디버그 패널은 기본적으로 숨긴 상태로 본다.
3. 쌍검으로 1회, 대검으로 1회 시작한다.
4. 가능하면 20분 클리어까지 본다. 시간이 부족하면 최소 6분까지 본다.
5. 아래 항목은 좋음/보통/문제 중 하나로 적고, 문제라면 짧은 원인을 쓴다.

## 1. 20분 런 템포

| 항목 | 기대 기준 | 판정 | 메모 |
| --- | --- | --- | --- |
| 첫 보상 | 24~30초 전후, 너무 빠르거나 느리지 않음 |  |  |
| 120초 시점 | 레벨 3~4 전후, 지루한 공백 없음 |  |  |
| 첫 문지기 | 300초, 첫 목표처럼 읽힘 |  |  |
| 첫 망각 | 5분대, 성취 후 상실로 느껴짐 |  |  |
| 600초 시점 | 레벨 9~10 전후, 빌드가 달라져 있음 |  |  |
| 궁극 완성 | 15~16분, 후반 보상처럼 느껴짐 |  |  |
| 최종 문지기 | 19~20분, 클리어 목표가 명확함 |  |  |

## 2. 무기 손맛

| 항목 | 기대 기준 | 판정 | 메모 |
| --- | --- | --- | --- |
| 쌍검 시작 카드 | 빠른 무기라는 방향이 읽힘 |  |  |
| 쌍검 기본공격 | 빠른 2연 베기처럼 보임 |  |  |
| 쌍검 타격 위치 | 적 위치 발도선으로 읽힘 |  |  |
| 대검 시작 카드 | 느리고 큰 한 방이라는 방향이 읽힘 |  |  |
| 대검 기본공격 | 넓고 무거운 반달 베기로 보임 |  |  |
| 대검 시야 가림 | 적/플레이어를 과하게 가리지 않음 |  |  |
| 대검 안정성 | 쌍검보다 지나치게 불안정하지 않음 |  |  |

## 3. 피격감과 전투 밀도

| 항목 | 기대 기준 | 판정 | 메모 |
| --- | --- | --- | --- |
| 적 피격 플래시 | 맞았다는 느낌이 즉시 보임 |  |  |
| 데미지 숫자 | 읽히되 화면을 덮지 않음 |  |  |
| 넉백 | 적이 밀려 전투 공간 반응이 있음 |  |  |
| 스폰 압박 | 초반이 비지 않고 후반이 과하게 막히지 않음 |  |  |
| 원거리 적 | 사거리 안에서 후퇴하지 않고 정지 사격함 |  |  |
| 카드 선택 중 정지 | 적/탄/전투가 완전히 멈춤 |  |  |

## 4. 기억/잔향/VFX 가독성

| id | 판정 | 문제 유형 | 메모 |
| --- | --- | --- | --- |
| `Memory_HungryBlades` / `Echo_Kalmuri` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |
| `Memory_BloodReflection` / `Echo_Blood` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |
| `Memory_ExecutionFlash` / `Echo_Execution` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |
| `Memory_HunterOath` / `Echo_Homing` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |
| `Memory_ShatterWave` / `Echo_Shockwave` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |
| `Memory_StoppedSecond` / `Echo_TimeStop` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |
| `Memory_AshenShield` / `Echo_AshenGuard` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |
| `Memory_OblivionBrand` / `Echo_Brand` |  | 안 보임 / 비슷함 / 약함 / 시끄러움 |  |

## 5. 궁극 잔향

| 궁극 | 기대 기준 | 판정 | 메모 |
| --- | --- | --- | --- |
| 피의 칼폭풍 | +5/+5 보상처럼 강하고 distinct함 |  |  |
| 파쇄 처형 | 일반 처형/파문보다 큰 보상으로 보임 |  |  |
| 정지 추적 | 시간 정지+추적 조합이 읽힘 |  |  |
| 잿빛 망각 | 방어/낙인 계열 보상으로 구분됨 |  |  |

## 6. HUD / UI

| 항목 | 기대 기준 | 판정 | 메모 |
| --- | --- | --- | --- |
| HP/XP | 필요한 순간 바로 읽힘 |  |  |
| 기억 슬롯 | 현재 빌드 상태가 이해됨 |  |  |
| 잔향 요약 | 도움이 되고 노이즈가 아님 |  |  |
| 궁극 준비 | 목표로 삼을 만큼 명확함 |  |  |
| 망각 예고 | 잃을 기억을 이해할 수 있음 |  |  |
| 망각 결과 | 아쉽지만 짜증보다 빌드 전환으로 읽힘 |  |  |

## 7. 최종 판단

하나만 선택한다.

- GO: `_dev` 상태를 유지하되 `Assets/Lethe` 승격 준비로 넘어갈 수 있다.
- ITERATE: 아래 한 축만 고친 뒤 다시 본다.
- NO-GO: 코어 방향을 다시 정해야 한다.

## 8. 다음 Codex 작업 축

ITERATE라면 하나만 고른다.

- XP cadence
- Gatekeeper HP
- weapon route balance
- reward route steering
- VFX scale/timing
- enemy pressure
- forgetting UX
- HUD readability

선택:

이유:

