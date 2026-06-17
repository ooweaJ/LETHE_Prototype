# LETHE TASK

## Current Epic

`Dev_Prototype_v1 Core Prototype Complete`

목표:

```text
무기 2종
+ 기억 8종
+ 잔향 8종
+ 궁극 4종
+ 실제 플레이 M2 루프
+ 타격감/VFX/스프라이트 판독성
= HTML보다 명확한 Unity 코어 프로토타입
```

## Progress

- A. 데이터 구조 정리: partially done
- B. 공격/잔향 타격감 보정: working
- C. M2 실제 플레이 루프 연결: working
- D. 주요 스프라이트/VFX 교체: pending
- E. 기억 8종 확장: pending
- F. 잔향 8종 확장: pending
- G. 궁극 4종 구현: pending
- H. 전체 플레이 리뷰: pending
- I. 밸런스/가독성 1차 튜닝: pending

## A. 데이터 구조 정리

Done criteria:

- [x] `MemoryDefinition`이 카드 설명, 효과 종류, trigger, 수치, VFX/feedback 연결을 담는다.
- [x] `EchoDefinition`이 원본 기억과 다른 잔향 형태, 무기별 반응, +5 각성을 담는다.
- [x] `UltimateEchoDefinition` 또는 호환 구조가 4궁극 조건과 무기별 표현을 담는다.
- [x] `EnemyDefinition`이 role, stat, attack, XP, spawn cost를 담는다.
- [x] `EncounterDefinition`이 M2 실제 플레이 pacing을 담는다.
- [x] `RewardPoolDefinition`이 weapons/memories/echoes/ultimates/enemies/encounters를 묶는다.
- [x] `dotnet build LETHE/Assembly-CSharp.csproj --nologo`가 통과한다.
- [ ] Unity Editor에서 새 Definition 타입 import/compile 상태를 MCP로 확인한다.

## B. 공격/잔향 타격감 보정

Done criteria:

- [ ] 쌍검은 빠른 2연 반달 베기로 읽힌다.
- [ ] 대검은 느린 큰 반달 범위 참격으로 읽힌다.
- [ ] 적 피격 흰색 플래시, 데미지 숫자, 넉백, hitstop이 충분히 보인다.
- [x] 칼무리 잔향은 적 위치 후속타 링/반달로 읽히는 1차 보정이 들어갔다.
- [x] 혈반 잔향은 표식 -> 회복 실 -> 피꽃 흐름의 1차 보정이 들어갔다.
- [ ] Unity Play Mode에서 칼무리 링과 혈반 실이 실제로 방해 없이 보이는지 확인한다.

## C. M2 실제 플레이 루프 연결

Done criteria:

- [ ] 디버그 없이 XP -> 카드 -> 기억 강화 -> 망각 -> 잔향 -> 공명 -> +5 -> 궁극 흐름에 도달한다.
- [ ] 60~120초 안에 핵심 감정 루프가 보인다.
- [x] 최고 레벨 기억이 다음 망각 후보라는 점이 HUD에서 보인다.
- [x] HUD에 M2 현재 목표/결손 생존/공명 대기/궁극 준비 상태가 보인다.
- [x] 레벨업 카드로 세 번째 기억 `멈춘 1초`를 선택해 기억 3칸을 채울 수 있다.
- [ ] 자동 리뷰 보정 없이도 +5/궁극까지 닿는 실제 pacing을 완성한다.

## D. 주요 스프라이트/VFX 교체

Done criteria:

- [ ] 플레이어 4방향 idle/move 판독 이미지가 들어간다.
- [ ] 쌍검/대검 무기 이미지가 캐릭터와 분리되어 보인다.
- [ ] 기본 적 2~3종이 역할별로 구분된다.
- [ ] 칼무리/혈반 기억과 잔향 VFX가 서로 다른 형태로 보인다.
- [ ] 피의 칼폭풍 VFX가 무기별로 다르게 보인다.

## E. 기억 8종 확장

Done criteria:

- [ ] 8개 `MemoryDefinition` asset이 존재한다.
- [ ] 8개 활성 기억이 서로 다른 플레이 감각을 가진다.
- [ ] 카드 설명과 실제 효과가 일치한다.

## F. 잔향 8종 확장

Done criteria:

- [ ] 8개 `EchoDefinition` asset이 존재한다.
- [ ] 잔향은 약한 복사본이 아니라 형태가 바뀐 효과로 보인다.
- [ ] +1~+5 성장과 +5 각성 표시가 있다.
- [ ] 무기별 잔향 표현 차이가 있다.

## G. 궁극 4종 구현

Done criteria:

- [ ] 4개 궁극 definition이 존재한다.
- [ ] 피의 칼폭풍은 칼무리 +5 + 혈반 +5로 발동한다.
- [ ] 궁극은 무기별로 다른 패턴을 가진다.
- [ ] HUD에 궁극 준비/발동 상태가 보인다.

## H. 전체 플레이 리뷰

Done criteria:

- [ ] jaewoo가 `GO`, `ITERATE`, `NO-GO` 중 하나를 줄 수 있다.
- [ ] 피드백은 1~3개 핵심 문제로 압축된다.

## I. 밸런스/가독성 1차 튜닝

Done criteria:

- [ ] 무기 DPS, 적 체력/속도/spawn, XP curve, 망각 타이밍, 잔향 proc, 궁극 조건이 1차 튜닝된다.
- [ ] `docs/TEST.md`, `docs/CHANGELOG.md`, orchestration report가 갱신된다.

## Current Next

1. A단계 데이터 계약 보강.
2. A단계 빌드 검증.
3. B단계로 넘어가 무기/잔향 타격감 보정 시작.
