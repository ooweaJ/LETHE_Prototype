# 06. Unity 구현 계획 / 마일스톤 / 데이터 구조

최종 갱신: 2026-06-12 · 대상 씬: `Assets/_dev/Scenes/Dev_Prototype_v1.unity`

이 문서는 위 기획(00~05)을 Unity로 옮기는 개발 실행 계획이다. 핵심 전략: **HTML 코어를 먼저 이식해 게임 셸을 세우고, 그 위에 잔향/공명/궁극을 얹고, 마지막에 8종으로 넓힌다.** 8종을 placeholder로 동시에 늘리는 방식은 금지.

## 0. 핵심 전환 (기존 진행 대비)

- 기존: 8기억·8잔향·4궁극을 placeholder로 동시 구현(브레드스 우선) → 어디도 HTML 퀄에 못 미침.
- 변경: **1코어 수직 슬라이스를 HTML 이상 퀄로 먼저 증명** → 그 다음 확장.
- 이유: 퀄 격차의 원인은 엔진이 아니라 노력 분산 + HTML 검증값 미이식.

## 1. 데이터 구조 (ScriptableObject)

`PrototypeGameManager` 하드코딩 분기는 폐기 대상. 아래를 `_dev/Data` 자산으로 등록한다.

### Definition

```text
WeaponDefinition   { id, displayName, role, range, damage, interval, arcDegrees, targetingMode,
                     echoSizeScale, echoDamageScale, echoProcStyle, ultimatePattern }
                   // targetingMode: Nearest(쌍검) | DensestArc(대검) — 02_COMBAT 트리거 모델
                   // echoSizeScale / echoDamageScale: 온힛 잔향의 크기·피해 배수
                   // echoProcStyle: MultiSmall(쌍검) | SingleHeavy(대검)
                   // ultimatePattern: ManyFast(쌍검) | FewHeavy(대검)
MemoryDefinition   { id, displayName, role, cooldown, tags[], levelBonus, matchingEchoId, activeBehavior }
EchoDefinition     { id, sourceMemoryId, displayName, levelScaling, awakenedEffect, resonanceRider, triggerFamily }
EchoLevelData      { level, behavior }   // +1/+3/+5 차이
EchoSynergyDefinition { id, requiredEchoIds[2], displayName, effect }
EnemyDefinition    { id, displayName, hp, speed, damage, radius, score, role(shooter/splitter/healer) }
FeedbackProfile    { hitstop, flash, shake, sfxHook }
RewardPoolDefinition { levelUpStatPool, memoryPool }
```

수치는 [02](LETHE_DESIGN_02_COMBAT.md)/[03](LETHE_DESIGN_03_MEMORY_ECHO.md)/[04](LETHE_DESIGN_04_BALANCE.md) 표에서 그대로 채운다.

### Runtime / Service

```text
RunBuildState { activeMemories(id->level), echoes(id->level), unlockedSynergies, overcharge }
MemoryInventory · EchoInventory
ForgetService · ResonanceService · UltimateEchoService · RewardService
HitResolver · EchoTriggerRouter · EchoProcLimiter
PoolService · FeedbackService · DebugStateInjector
```

규칙: 신규 기억/잔향 추가 시 `HitResolver`/`ForgetService`/`EchoInventory`를 수정하지 않는다. 새 트리거 패밀리가 필요할 때만 core event layer를 확장한다.

## 2. 마일스톤

### M0. HTML 코어 이식 계약 고정
- 00~05 문서가 source of truth로 등록(완료).
- `src/game.js` 검증 수치를 ScriptableObject로 옮길 매핑 확정.

### M1. 게임 셸 (HTML parity) — 최우선
- 플레이어 이동/카메라/아레나.
- 쌍검 기본공격(사거리 86, 피해 15, 간격 0.36, 호 119°).
- 적 4종 스폰/추격 + 적 스케일링.
- 압박 페이즈 곡선([01](LETHE_DESIGN_01_RUN_LOOP.md)).
- XP/레벨업 곡선 + 레벨업 화면([04](LETHE_DESIGN_04_BALANCE.md)/[05](LETHE_DESIGN_05_UI_UX.md)).
- HUD(HP·타이머·기억 슬롯·잔향 패널·다음 망각 후보).
- 초반 생존 보정(×0.24 → 320s 램프).
- **수락: HTML과 동등한 게임 셸이 돈다.**

### M2. 1코어 수직 슬라이스 (HTML 이상 퀄)
- 쌍검 + 굶주린 칼무리 + 피의 반사.
- 첫 보스(2050) → 첫 망각 → 망각 결과 화면 → 결손 생존 → 보충/공명.
- 칼무리 잔향 +5, 혈반 잔향 +5, 궁극 1개(피의 칼폭풍).
- juice 레이어: hitstop, 데미지 팝업, trail, shake, SFX hook.
- **수락: "Unity도 HTML만큼 된다"가 증명됨.**

### M3. 대검 추가
- 장송대검(사거리 128, 피해 42, 간격 1.02, 호 151°).
- 같은 기억/잔향이 무기별로 다른 리듬/형태.

### M4. 8기억 / 8잔향 확장
- 나머지 6기억 활성 +1/+3/+5 동작.
- 6잔향 +1/+3/+5 + 트리거 패밀리.
- 전용 VFX(line renderer 의존 제거).

### M5. 4궁극 완성
- 처형 각인, 정지 추적, 성채 파문 추가.
- 각 궁극 조건/발동/VFX/cap.

### M6. Complete Prototype Gate
- 두 무기 + 8기억 + 8잔향 + 4궁극 디버그 검증.
- 6~8분 플레이 또는 60~120초 압축 스모크.
- jaewoo hands-on GO/ITERATE/NO-GO.

## 3. 수락 테스트

### 휴먼 리뷰 질문
- 초반 전투·성장이 망각 전까지 재미있는가?
- 가장 키운 기억을 잃는 것이 아쉬운가?
- 잔향이 약화판이 아니라 형태 변화로 보이는가?
- 공명은 다시 얻는 설렘이 있는가?
- 4궁극 중 하나가 목표로 삼고 싶은가?
- **Unity 게임 셸이 HTML 수준인가?**
- 쌍검/대검 리듬 차이가 느껴지는가?

### 자동/MCP 체크 (매 마일스톤)
- Unity compile errors 0, console errors 0, scene missing refs 0.
- Play Mode 스모크.
- `npm run report` / `report:check` pass.

## 4. 비범위

상점, 메타 성장, 다중 지역 완성, 최종 보스, 출시급 UI/사운드 전체, 빌드 배포, `Assets/Lethe` 승격. 단, 대검·8기억·8잔향·4궁극은 범위 안.

## 5. 구현 우선순위 (한 줄 요약)

```text
게임 셸(HTML 이식) → 1코어 수직 슬라이스(HTML 이상 퀄) → 대검 → 8종 확장 → 4궁극
역할이 보이면 polish, 안 보이면 sprite를 고쳐도 기준이 안 잡힌다.
```
