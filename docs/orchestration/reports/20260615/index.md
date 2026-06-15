# LETHE 개발 보고서 - 2026-06-15

쌍검 기본공격을 플레이어 앞 부채꼴 VFX가 아니라 적 위치 발도선으로 바꾸고, 칼무리 잔향을 적중 위치 후속타로 분리했다.

# 2026-06-15-01 - 쌍검 타겟 발도선과 칼무리 후속타 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 쌍검이 허공에 베지 않고, 적이 사거리 안에 들어왔을 때 적 위치에 기본공격 발도선이 생긴다. 칼무리 잔향은 같은 프레임에 캐릭터 쪽에서 터지는 느낌이 아니라, 맞은 적 위치에서 짧게 늦게 터지는 후속타로 바뀌었다.

## 2. 오늘 바뀐 것

- `V1GameManager`의 기존 플레이어-origin 쌍검 arc VFX를 제거했다.
- 주 대상에는 `TargetLocalSlash_Primary_A/B` 두 줄의 발도선을 만든다.
- 보조 cleave 대상에는 작은 `TargetLocalSlash_Assist`를 만든다.
- 칼무리 잔향은 `PendingKalmuriFollowup` 큐에 넣었다가 0.035초 전후로 `KalmuriFollowup`을 생성한다.
- 쌍검 hitstop을 `0.025s`에서 `0.018s`로 줄여 잔향 발동 때 캐릭터가 멈추는 느낌을 낮췄다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity MCP `LETHE` 포트 `7890` 연결 확인.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke: `noAirAfter=0 targetLocalSlash=3 playerFanArc=0 kalmuriFollowup=6 hitSpark=6`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 1 unit heading ok.
- `npm.cmd run report:orchestrator:unit:dry`: `404 Not Found`, `project not found`로 실패.
- 증거:
  - `LETHE/Assets/_dev/Evidence/v1_target_local_slash_echo_followup_20260615.png`
  - `LETHE/Assets/_dev/Evidence/v1_target_local_slash_echo_followup_20260615_scene.png`

## 4. 결정한 것

- `DEC-2026-06-12-05`를 실제 코드로 반영했다.
- 기본공격 VFX의 기준점은 플레이어가 아니라 적중 대상이다.
- 잔향은 무기 히트 다음에 따라오는 후속타로 보여야 한다.

## 5. 문제 또는 리스크

- 아직 사람이 본 체감 검증은 필요하다.
- 발도선 크기, 보조 slash 수, 칼무리 지연 시간은 플레이 리뷰 후 조정할 수 있다.
- 이번 작업은 쌍검에 집중했고, 대검 리듬은 다음 구조 작업에서 확장해야 한다.
- Project Orchestrator Discord intake는 현재 `project not found`를 반환한다.

## 6. GPT/Claude 인계 요약

쌍검 VFX가 캐릭터 앞 부채꼴에서 적 위치 발도선으로 바뀌었다. 칼무리 잔향은 적중 위치 후속타로 분리됐다. 다음 판단은 이 방식이 기본공격과 잔향을 눈으로 구분하게 만드는지다.

## 7. 다음 Codex 작업

- Jaewoo 플레이 리뷰 결과를 받아 쌍검 VFX/잔향 후속타를 보정한다.
- `GO`면 대검 공격 구조와 weapon-specific echo rhythm을 준비한다.
- `ITERATE`면 slash 위치/크기, Kalmuri delay, hitstop을 먼저 조정한다.

## 8. 포트폴리오 메모

- 문제: 쌍검 기본공격이 플레이어 앞 허공 arc처럼 보여 무기와 잔향 상호작용이 약했다.
- 방향: 공격 VFX를 적중 위치로 옮기고 잔향을 후속타로 분리했다.
- 행동: target-local slash, delayed Kalmuri follow-up, 회귀 스모크를 구현했다.
- 결과: 자동 검증에서 허공 VFX 0, 타겟 발도선 3, 칼무리 후속타 6을 확인했다.

# 2026-06-15-02 - 대검을 위한 무기 리듬 구조 준비

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 기본 무기는 계속 `절단쌍검`이다. 다만 내부 전투 루프는 이제 쌍검 전용 상수/메서드가 아니라 `WeaponRuntimeSpec`을 통해 돌아간다. `F9`로 구조 확인용 `장송대검` 경로를 켤 수 있다.

## 2. 오늘 바뀐 것

- 다음 작업 목록을 정리했다.
  - 1번 플레이 리뷰는 jaewoo 판단이 필요하다.
  - 2번 쌍검 체감 보정은 리뷰 피드백 이후 정확하다.
  - 그래서 Codex가 바로 진행 가능한 3번 대검 공격 구조 준비를 실행했다.
- `V1WeaponId`, targeting mode, echo proc style, ultimate pattern enum을 추가했다.
- `WeaponRuntimeSpec`을 추가해 range/damage/interval/arc/targeting/echo scale/proc style/hitstop/shake를 한 곳에 묶었다.
- `UpdateWeapon`이 현재 무기 spec을 읽어 타겟팅, 히트 수집, 기본공격 VFX, 온힛 잔향을 처리하게 바꿨다.
- 대검 debug spec을 추가했다.
  - targeting: `DensestArc`
  - echo style: `SingleHeavy`
  - ultimate pattern: `FewHeavy`
- 칼무리 잔향 큐가 무기 spec을 들고 다니게 바꿨다.
  - 쌍검: 작은 후속타 여러 개.
  - 대검: 큰 후속타 한 번.
- 첫 스모크에서 대검 heavy 잔향이 맞은 대상마다 4번 떠서, 한 스윙당 primary hit 1회만 발동하도록 보정했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke: `noAir=0 dualSlash=3 dualOldFan=0 dualFollow=6 greatSlash=4 heavyFollow=1 multiFollowStill=0`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 2 unit headings ok.
- `npm.cmd run report:orchestrator:unit:dry`: `404 Not Found`, `project not found`로 실패.
- 증거: `LETHE/Assets/_dev/Evidence/v1_weapon_spec_greatsword_prep_20260615.png`

## 4. 결정한 것

- 대검을 쌍검 코드 복사로 만들지 않고, weapon spec으로 기본공격/잔향/궁극 리듬을 분기한다.
- `SingleHeavy`는 여러 대상이 맞아도 한 스윙당 잔향 1회만 발동한다.
- 현재 대검은 구조 확인용이며, 최종 체감 평가는 스프라이트/애니메이션/큰 참격 연출이 붙은 뒤 한다.

## 5. 문제 또는 리스크

- 대검 전용 무기 스프라이트와 공격 애니메이션은 아직 부족하다.
- F9 대검은 구조 검증용이므로, 지금 상태만으로 재미 판단하면 안 된다.
- Project Orchestrator Discord intake는 현재 `project not found`를 반환한다.

## 6. GPT/Claude 인계 요약

무기 리듬 구조가 쌍검 전용에서 spec 기반으로 바뀌었다. 쌍검은 기존 target-local/MultiSmall 경로를 유지하고, 대검은 DensestArc/SingleHeavy 경로가 검증됐다.

## 7. 다음 Codex 작업

- 우선 jaewoo가 쌍검 target-local 기본공격과 칼무리 후속타를 리뷰한다.
- 그 다음 `GO`면 M2 실제 페이싱 또는 대검 시각/애니메이션 체감 패스 중 하나를 선택한다.
- 대검을 진행한다면 큰 무기 sprite, 느린 발도/참격, 강한 hitstop/shake부터 붙인다.

## 8. 포트폴리오 메모

- 문제: 대검을 추가하려면 쌍검 코드를 복사하는 구조가 될 위험이 있었다.
- 방향: 무기 정의가 기본공격과 잔향 리듬을 결정하게 만들었다.
- 행동: `WeaponRuntimeSpec`과 debug 대검 경로를 만들고 Unity Play Mode로 검증했다.
- 결과: 쌍검은 작은 후속타 다수, 대검은 큰 후속타 1회로 분기되는 구조가 생겼다.

# 2026-06-15-03 - 통합 피드백용 v1 묶음 구현

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 한 번 플레이해서 쌍검, 대검, 망각, 공명, +5 잔향, 피의 칼폭풍, HUD, 전투 밀도를 함께 피드백할 수 있는 상태다. 기본 무기는 쌍검이고, `F9`로 대검을 켜서 비교할 수 있다.

## 2. 오늘 바뀐 것

- 쌍검 target-local 발도선을 더 크게, 더 오래, 더 선명하게 조정했다.
- 대검 visual을 쌍검 두 자루가 아니라 큰 단일 검으로 보이게 바꿨다.
- 대검 swing animation을 느리고 큰 움직임으로 분리했다.
- 대검 기본공격 VFX에 큰 참격과 shock marker를 추가했다.
- 리뷰용 M2 페이싱을 추가했다.
  - 14초: 피의 반사 획득.
  - 28초: 굶주린 칼무리 성장.
  - 42초: 피의 반사 성장.
  - 50초: 멈춘 초침으로 세 번째 기억 슬롯 채움.
  - 62초: 문지기 등장.
  - 망각 후 결손 생존 22초.
  - 공명 재획득 VFX 추가.
  - 복귀 후 칼무리/혈반 +5 각성으로 피의 칼폭풍 검증 가능.
- HUD에 +5 각성 표시와 궁극 준비 카운트를 추가했다.
- 피의 칼폭풍을 무기별로 다르게 만들었다.
  - 쌍검: 작은 혈검 다수, 빠른 회전.
  - 대검: 큰 혈검 소수, 느린 강타형 참격.
- 적 스폰 거리와 빈도를 조정해 전투가 덜 비어 보이게 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke: `resultAfterGate=True activeMemories=3 dualSlash=3 greatSlash=4 multiFollow=6 heavyFollow=1 dualStorm=6 greatStorm=3 resonance=12`.
- 스냅샷: `scene=v1 weapon=장송대검 elapsed=87.0 hp=210.0/210.0 level=1 xp=0/5 kills=5 memories=[HungryBlades:3,StoppedSecond:1,BloodReflection:2] echoes=[BloodReflection:5,HungryBlades:5] enemies=0 storm=True result=False refill=False death=False`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 3 unit headings ok.
- `npm.cmd run report:orchestrator:unit:dry`: `404 Not Found`, `project not found`로 실패.
- 증거: `LETHE/Assets/_dev/Evidence/v1_full_feedback_batch_20260615.png`

## 4. 결정한 것

- 이번 빌드는 “완성”이 아니라 한 번에 피드백받기 위한 통합 리뷰 빌드다.
- M2 루프는 리뷰용 압축 페이싱으로 열어 둔다.
- 다음 작업은 새 기능 추가가 아니라, 플레이 피드백에서 가장 약한 부분 하나를 고르는 방식으로 진행한다.

## 5. 문제 또는 리스크

- 대검 visual은 아직 절차형 임시 sprite다. 체감 방향은 확인 가능하지만 최종 아트는 아니다.
- 리뷰 페이싱은 자동 보조가 들어간다. 최종 밸런스가 아니라 감정 루프 확인용이다.
- 사운드가 없어서 hitstop/shake/VFX만으로 타격감을 판단해야 한다.
- Project Orchestrator Discord intake는 현재 `project not found`를 반환한다.

## 6. GPT/Claude 인계 요약

v1은 이제 쌍검/대검, 망각/공명, +5 잔향, 피의 칼폭풍을 한 번에 볼 수 있는 통합 리뷰 빌드다. 다음은 jaewoo가 실제 플레이로 가장 약한 지점을 1~3개 골라주는 것이다.

## 7. 다음 Codex 작업

- jaewoo 통합 플레이 리뷰를 받는다.
- 다음 구현은 공격 판독성, 페이싱/밸런스, UI 가독성, 아트 교체 중 하나만 고른다.
- 리뷰 없이 추가 확장은 하지 않는다.

## 8. 포트폴리오 메모

- 문제: 여러 작은 개선이 흩어져 있어 매번 피드백하기 어려웠다.
- 방향: 한 번에 플레이 가능한 통합 리뷰 빌드를 만들었다.
- 행동: 무기 체감, M2 페이싱, 공명/각성/궁극 표시, 전투 밀도를 묶어 구현했다.
- 결과: 87초 스모크에서 망각, 공명, +5 잔향, 무기별 피의 칼폭풍까지 도달했다.

# 2026-06-15-04 - 시작 무기 선택과 타격 피드백 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 런 시작 전에 `절단쌍검` 또는 `장송대검`을 고르는 화면이 먼저 나온다. 선택 후에는 뱀서류처럼 무기가 자동으로 발동하고, 잔향은 그 무기 타격 지점에서 후속타로 붙는다.

## 2. 오늘 바뀐 것

- 시작 무기 선택 오버레이를 추가했다.
  - `1`: 절단쌍검.
  - `2`: 장송대검.
  - 카드 클릭으로도 시작 가능.
- 런은 무기를 고르기 전에는 진행되지 않게 바꿨다.
- `F9`는 시작 후 비교용/debug 무기 전환으로 유지했다.
- 대검 임시 박스 visual을 절차형 대검 실루엣으로 교체했다.
- 쌍검 기본공격 VFX를 더 날카로운 적 위치 발도선으로 바꿨다.
- 대검 기본공격 VFX를 큰 참격 + 충격 표식으로 바꿨다.
- 적 피격 넉백과 타격 반응을 강화했다.
  - 쌍검은 빠른 다단 밀림.
  - 대검은 느리고 크게 밀어내는 한 방.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `beforeOverlay=True`
  - `afterOverlay=False`
  - `dualSlash=5`
  - `dualSpark=4`
  - `dualKnock=1.78`
  - `greatSlash=5`
  - `greatShock=1`
  - `greatKnock=5.53`
- 스냅샷: `scene=v1 weapon=장송대검 elapsed=0.0 hp=210.0/210.0 level=1 xp=0/5 kills=1 memories=[HungryBlades:1] echoes=[] enemies=7 storm=False result=False refill=False death=False`.
- 증거: `LETHE/Assets/_dev/Evidence/v1_weapon_select_hit_feedback_20260615.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 4 unit headings ok.
- `npm.cmd run report:orchestrator:unit:dry`: `404 Not Found`, `project not found`로 실패.

## 4. 결정한 것

- LETHE는 뱀서처럼 무기 자동 발동 구조를 따른다.
- 다만 잔향은 독립 자동 스킬이 아니라 “무기 타격 지점에서 터지는 후속타/변형”으로 읽히게 한다.
- 무기 선택은 캐릭터 선택 이전 단계 대신, 현재 프로토타입에서는 런 시작 무기 선택으로 둔다.

## 5. 문제 또는 리스크

- 대검은 아직 최종 아트가 아니라 절차형 실루엣이다.
- 넉백이 강해졌기 때문에 적 밀도/접촉 피해/사냥 속도는 jaewoo 플레이 후 다시 봐야 한다.
- 사운드가 없어서 타격감의 절반은 아직 빠져 있다.
- Project Orchestrator Discord intake는 현재 `project not found`를 반환한다.

## 6. GPT/Claude 인계 요약

이번 패스는 무기 선택 부재와 약한 타격 상호작용을 먼저 보정했다. 다음 리뷰에서는 시작 선택 UX, 쌍검 발도선, 대검 한 방감, 적 넉백이 실제 플레이에서 자연스러운지 확인해야 한다.

## 7. 다음 Codex 작업

- jaewoo가 `Dev_Prototype_v1`에서 시작 무기를 각각 골라 비교한다.
- 약하면 다음 중 하나를 고른다.
  - 적 피격/넉백 보정.
  - 무기 스프라이트 전용 아트 교체.
  - 칼무리 잔향 후속타 타이밍 보정.
  - 적 밀도/사냥 템포 보정.

## 8. 포트폴리오 메모

- 문제: 무기 정체성과 타격 반응이 약해서 게임의 첫 10초가 평가 기준으로 부족했다.
- 방향: 시작 무기 선택과 적중 지점 중심 VFX로 플레이어가 무기 차이를 바로 읽게 했다.
- 행동: 선택 UI, 무기별 VFX, 넉백 수치, 대검 실루엣을 한 패스로 보정했다.
- 결과: 스모크에서 선택 오버레이와 두 무기 타격 VFX/넉백 차이가 확인됐다.
