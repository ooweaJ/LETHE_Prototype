# 2026-06-22-01 - Generated VFX runtime wiring and scale pass

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 새 기능을 늘리기보다, 2026-06-21에 생성한 VFX PNG를 실제 전투 런타임에 연결해서 첫 120초 리뷰에서 타격/VFX가 자연스럽게 보이도록 만드는 패스다.

## 2. 오늘 바뀐 것

- `V1GameManager`에 생성 VFX asset path와 helper를 추가했다.
- 무기/타격 VFX를 생성 PNG 우선으로 바꿨다:
  - 쌍검 swing arc 2종.
  - 대검 cleave arc 1종.
  - cyan/red hit spark.
- 6개 유틸 기억 VFX를 전용 PNG로 연결했다:
  - Execution Flash, Hunter Oath, Shatter Wave, Stopped Second, Ashen Shield, Oblivion Brand.
- 6개 유틸 잔향 VFX를 전용 PNG로 연결했다:
  - Execution, Homing, Shockwave, TimeStop, Ashen, Brand.
- 3개 유틸 궁극 VFX를 전용 PNG로 연결했다:
  - Fracture Execution, Stasis Hunt, Ashen Oblivion.
- 1254px prompt-sheet PNG가 너무 크게 보이지 않도록 기존 전투 월드 크기에 맞춰 scale normalization을 넣었다.
- PNG가 없을 때는 기존 procedural VFX로 fallback하도록 유지했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy v0/debug warning 7개, error 0개.
- Unity MCP:
  - active scene: `Dev_Prototype_v1`.
  - `Assets/Refresh`: success.
  - compile error count: 0.
  - Play Mode smoke 시도 후 console error count: 0.
  - Play Mode stop: success.

## 4. 결정한 것

VFX 데이터 구조를 크게 바꾸지 않고, 기존 `WeaponVfxProfile`의 `SlashVfxEntry` 값을 유지하면서 sprite 해석 함수가 생성 PNG를 먼저 찾게 했다. 그래서 무기별 VFX 튜닝 값은 그대로 살리고, 화면에 나오는 결과만 generated art로 교체된다.

## 5. 문제 또는 리스크

- Unity Game/Scene capture가 여전히 solid-color 이미지로 나와서 자동 시각 증거로는 신뢰하기 어렵다.
- VFX가 컴파일/런타임 오류 없이 연결된 것은 확인했지만, 최종 크기/밝기/지속시간은 jaewoo 직접 플레이로 판단해야 한다.
- `dotnet build` 과정에서 `LETHE/LETHE.slnx`가 untracked로 생겼다. 이번 작업의 의도 변경 파일은 아니다.

## 6. GPT/Claude 인계 요약

Codex가 생성 VFX PNG를 `V1GameManager` 런타임 경로에 연결했다. 다음 리뷰는 밸런스보다 먼저 VFX가 실제 타격 위치에 붙는지, 크기가 과하지 않은지, 쌍검/대검/기억/잔향/궁극이 서로 구분되는지를 본다.

## 7. 다음 Codex 작업

jaewoo가 `Dev_Prototype_v1`에서 쌍검/대검을 각각 선택하고 첫 120초를 리뷰한다. 리뷰 후 다음 패스는 `VFX scale/timing`, `공격 손맛`, `보상 속도`, `망각 UX`, `스폰 압박`, `적/기억 판독성` 중 하나로 좁힌다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 생성된 VFX PNG가 있었지만 실제 전투 경로는 아직 procedural placeholder를 많이 쓰고 있었다.
- 방향: 데이터 구조를 크게 흔들지 않고, 기존 VFX profile/spawn 경로가 generated art를 우선 사용하도록 연결한다.
- 행동: 무기/타격, 6기억, 6잔향, 3궁극 VFX를 `V1GameManager`에 연결하고 크기 환산을 넣었다.
- 결과: 첫 120초 직접 리뷰에서 타격/VFX 판독을 볼 수 있는 상태로 이동했다.
