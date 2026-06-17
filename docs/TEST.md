# LETHE TEST

## Purpose

이 문서는 LETHE 작업이 성공했는지 판단하는 기준이다. 자동 검증은 회귀 확인용이고, 최종 GO/ITERATE/NO-GO는 jaewoo 플레이 체감 리뷰가 우선한다.

## Required Technical Checks

Unity/C# 작업 후 가능한 경우 아래를 실행한다.

```powershell
dotnet build LETHE/Assembly-CSharp.csproj --nologo
npm.cmd run report
npm.cmd run report:check
```

Unity MCP가 연결되어 있으면 추가로 확인한다.

- Unity compile error count = 0.
- `Dev_Prototype_v1.unity` open success.
- Play Mode smoke에서 player/enemy/weapon runtime exception 없음.
- 필요한 경우 evidence screenshot 저장.

## Current Epic Checks

### A. Data Contract

- Definition 타입이 컴파일된다.
- 새 memory/echo/enemy/ultimate/encounter 데이터를 추가해도 기존 weapon data asset이 깨지지 않는다.
- 기존 `Weapon_DualBlades`, `Weapon_Greatsword`, `VFX_Weapon_DualBlades`, `VFX_Weapon_Greatsword` 경로가 유지된다.

### B. Hit Feel / Echo Readability

- 쌍검 기본공격이 빠른 2연 반달 베기로 보인다.
- 대검 기본공격이 범위만큼 큰 반달 참격으로 보인다.
- 적은 피격 시 흰색 플래시와 데미지 숫자를 보여준다.
- 칼무리 잔향은 캐릭터 주변 잡선이 아니라 타격 지점 후속타로 보인다.
- 혈반 잔향은 표식/실/피꽃으로 보인다.
- Current B-step technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after Kalmuri range ring, Blood heal thread, Blood bloom thread, and knockback cap changes.

### C. Real M2 Loop

- 디버그 버튼 없이 60~120초 안에 망각/잔향/공명/+5/궁극 중 핵심 흐름이 보인다.
- 플레이어가 "이 기억을 키우면 다음에 잃는다"를 의식할 수 있다.
- Current C-step technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed after adding the M2 HUD objective text and the third-memory level-up card path.

### H. Human Review Gate

jaewoo 리뷰 질문:

- 기본 공격이 무기별로 재미있나?
- 망각이 아깝나, 짜증나나?
- 잔향이 실제 전투를 바꾼다고 느껴지나?
- 재획득 공명이 설레나?
- +5/궁극이 후반 보상처럼 느껴지나?
- 다음에 고칠 가장 큰 문제 1~3개는 무엇인가?

### E/F/G. Content Expansion

- Current technical check: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` passed with 0 warnings and 0 errors after adding first-pass active effects for the remaining memories, utility echo reactions, and three additional ultimate runtime branches.
- Human review still needs to confirm whether these effects read as distinct enough, because Unity MCP visual verification was not available in this session.

## Known Non-Blocking Warnings

현재 `dotnet build`에는 legacy v0/debug 코드의 `Object.FindObjectOfType<T>()` deprecation warning 7개가 남아 있다. `Dev_Prototype_v1` compile error가 아니므로 현재 EPIC 진행을 막지 않는다.
