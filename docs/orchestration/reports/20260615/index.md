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
