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

# 2026-06-22-02 - 피의 칼폭풍 payoff와 플레이어 이동감 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 `_dev` 검증 빌드다. 이번 작업은 새 시스템 추가가 아니라, jaewoo 리뷰에서 나온 두 가지 문제를 바로 좁혀 고친 패스다.

- 피의 칼폭풍이 칼무리와 다른 강한 보상으로 읽히지 않음.
- 캐릭터 걷기/방향 전환이 아직 부자연스러움.

## 2. 오늘 바뀐 것

- 플레이어 이동:
  - 입력값을 바로 위치에 더하지 않고 짧은 가속/감속을 거치게 했다.
  - `PlayerVisual`에 아주 작은 보행 bob/tilt를 넣었다.
  - 이동 중 무기 앵커 회전이 즉시 튀지 않도록 부드럽게 보간했다.
- 피의 칼폭풍:
  - 기존 단순 회전칼 구조를 `등장 신호 -> 지속 폭풍 압박 -> 주기적 폭발 박동`으로 나눴다.
  - 쌍검은 빠른 8방향 칼날 회전과 12칼날 burst로 바꿨다.
  - 대검은 느리지만 큰 링, 4방향 강타, 더 큰 burst damage/knockback/hitstop/camera shake를 쓴다.
  - 폭풍 중 적에게 혈반을 묻히고, 안쪽으로 끌어당기고, burst 때 바깥으로 튕기며 회복 실을 돌려준다.
  - `BeginRun`이 디버그/스모크에서 너무 이른 타이밍에 불려도 플레이어를 먼저 보장하도록 방어 코드를 넣었다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy v0/debug warning 7개, error 0개.
- Unity compile error count: 0.
- Unity console error count: 0.
- Direct M2 state injection:
  - `memories=[BloodReflection:3,HungryBlades:3]`
  - `echoes=[HungryBlades:5,BloodReflection:5]`
  - `storm=True`
- `UpdateEchoUltimate(0.12f)`를 12틱 수동 호출:
  - `bloodStormObjects=124`
  - 주변 적 정리 후 `kills=14`
  - console error 0.

## 4. 결정한 것

피의 칼폭풍은 칼무리보다 단순히 크거나 빨라지는 이펙트가 아니라, 혈반/회복/끌어당김/폭발 박동이 함께 읽히는 궁극 상태로 간다. 쌍검은 빠른 연쇄형, 대검은 큰 강타형 차이를 유지한다.

## 5. 문제 또는 리스크

MCP Play Mode 시간이 이번 세션에서 정상적으로 흐르지 않아 `elapsed=0.0`에 머물렀다. 그래서 궁극 루프는 자연 시간 재생 대신 reflection tick으로 검증했다. 최종 걷기 느낌과 폭풍 크기/밝기/밀도는 여전히 jaewoo 직접 플레이로 봐야 한다.

## 6. GPT/Claude 인계 요약

Codex가 피의 칼폭풍을 opening/pressure/burst 구조로 강화했고, 이동을 입력 직접 반영에서 짧은 가속/감속 기반으로 바꿨다. 다음 리뷰는 “궁극이 정말 좋아 보이는가”와 “걷기가 덜 뻣뻣한가”를 먼저 판단하면 된다.

## 7. 다음 Codex 작업

jaewoo가 직접 플레이한 뒤 하나만 고른다:

- 피의 칼폭풍이 아직 약하면 burst 주기/화면 흔들림/회복 실/피해량을 더 조정한다.
- 걷기가 아직 어색하면 sprite frame timing, bob/tilt 크기, 방향 전환 hysteresis를 조정한다.
- 둘 다 괜찮으면 보상 속도, 망각 UX, spawn pressure 중 하나로 넘어간다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 핵심 궁극이 상위 보상처럼 느껴지지 않고, 이동감이 프로토타입 티를 냈다.
- 방향: 새 기능을 늘리지 않고 기존 M2 코어 감각을 직접 강화한다.
- 행동: 피의 칼폭풍을 궁극 전용 리듬으로 분리하고, 플레이어 이동/보행 표현을 부드럽게 했다.
- 결과: 기술 검증상 궁극 루프가 오브젝트를 생성하고 적을 정리하며 에러 없이 동작한다. 다음은 체감 리뷰다.
# 2026-06-22-03 - 무기 실루엣과 공격 VFX 크기 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`에서 손에 든 무기 크기와 공격 자국 크기를 다시 맞췄다. 이번 조정은 jaewoo가 바로 짚은 “대검은 너무 크고, 쌍검은 너무 작다”는 체감 피드백에 대한 보정이다.

## 2. 오늘 바뀐 것

- 쌍검:
  - 손 무기 runtime scale을 `0.30~0.33`에서 `0.43~0.475`로 키웠다.
  - 위치를 캐릭터 몸 가까이 `x=±0.19`, `y=-0.035~-0.04` 쪽으로 조정했다.
  - 쌍검 slash PNG 보정값을 `0.153 -> 0.192`로 키웠다.
- 대검:
  - 손 무기 runtime scale을 `0.44~0.51`에서 `0.34~0.375`로 줄였다.
  - 위치와 스윙 이동폭을 줄여 캐릭터 실루엣을 덜 잡아먹게 했다.
  - 대검 cleave PNG 보정값을 `0.225 -> 0.182`로 줄였다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, legacy warning 7개, error 0개.
- Unity compile error count: 0.
- Play Mode transform check:
  - dual blade scale `0.430`, positions `(-0.19, -0.04)`, `(0.19, -0.04)`.
  - greatsword scale `0.340`, position `(0.03, -0.09)`.
- Unity console error count: 0.

## 4. 결정한 것

공격 스프라이트는 “없어도 게임 로직은 돌아가지만, 이 게임에는 있는 편이 맞다”고 판단한다. 쌍검/대검의 차이는 데미지 숫자보다 화면에서 먼저 읽혀야 하므로, 최종적으로는 전용 공격 스프라이트 또는 최소한 전용 slash VFX가 필요하다. 단, fallback procedural VFX는 에셋 누락 대비용으로 유지한다.

## 5. 문제 또는 리스크

자동 스크린샷 경로가 여전히 믿기 어려워서 실제 화면 크기 체감은 jaewoo 직접 플레이 확인이 필요하다. 특히 쌍검은 이제 보일 가능성이 커졌지만 너무 장난감처럼 보이지 않는지, 대검은 줄였어도 무거운지 봐야 한다.

## 6. GPT/Claude 인계 요약

Codex가 held weapon visual scale과 generated attack VFX scale을 동시에 조정했다. 다음 리뷰에서는 “쌍검이 이제 보이는가”, “대검이 캐릭터보다 과하게 커 보이지 않는가”, “공격 스프라이트가 무기 정체성을 살리는가”를 보면 된다.

## 7. 다음 Codex 작업

직접 플레이 후 무기 크기가 아직 어긋나면 수치만 1회 더 보정한다. 크기가 괜찮으면 공격 스프라이트를 제거하지 말고, 오히려 전용 slash의 밝기/수명/위치만 더 다듬는 쪽이 낫다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 무기 실루엣 크기 차이가 과해서 쌍검은 안 보이고 대검은 화면을 먹었다.
- 방향: 무기 손잡이 실루엣과 공격 자국 스케일을 동시에 맞춘다.
- 행동: 쌍검은 키우고 대검은 줄였으며, 공격 PNG 보정값도 반대로 조정했다.
- 결과: 런타임 transform 기준으로 쌍검 `0.430`, 대검 `0.340`이 확인됐고 에러 없이 동작했다.
# 2026-06-22-04 - 대검 직접 플레이 커버 문제 수정

## 1. 현재 빌드 상태

대검으로 직접 Play Mode 상태를 확인했고, jaewoo가 말한 것처럼 대검이 캐릭터를 가리는 문제가 수치로도 확인됐다. 이번 작업은 대검을 더 줄이고, 플레이어 뒤 레이어로 보내는 보정이다.

## 2. 오늘 바뀐 것

- 대검 손 무기 scale을 `0.34~0.375`에서 `0.21~0.235`로 다시 줄였다.
- 대검을 플레이어 앞 레이어가 아니라 뒤 레이어로 보냈다:
  - 대검 sorting order `30 -> 18`.
  - 플레이어 sorting order는 `20`.
- 대검 위치를 몸 중앙에서 옆으로 이동했다:
  - local position `x=0.18`, `y=-0.08`.
- 대검 swing 이동폭과 회전폭을 줄였다.
- 대검 공격 cleave PNG 보정값도 `0.182 -> 0.150`으로 줄였다.

## 3. 테스트 결과와 근거

수정 전 직접 확인:

- player bounds `(2.210, 2.210)`.
- sword bounds `(3.121, 4.995)`.
- sword/player height ratio `2.26`.
- sword sorting order `30`, player sorting order `20`.

수정 후 확인:

- sword bounds `(2.327, 2.944)`.
- sword/player height ratio `1.33`.
- sword sorting order `18`, player sorting order `20`.
- forced greatsword attack VFX `5`개, max bounds `(2.332, 2.332)`.
- Unity console error count `0`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, error 0.

## 4. 결정한 것

대검은 “크다”보다 “플레이어 앞에서 몸을 덮는다”가 더 큰 문제였다. 그래서 크기를 줄이는 것과 동시에 플레이어 뒤로 보내는 방식으로 해결했다. 공격 VFX는 전용 sprite를 유지하되, 화면을 덮지 않게 더 작게 쓴다.

## 5. 문제 또는 리스크

이제 대검이 너무 작아 보일 가능성이 생겼다. 하지만 현재 수치는 플레이어보다 살짝 큰 정도이고 뒤에 깔리므로, 직접 리뷰에서 “무겁지만 안 가린다”가 되는지 확인하면 된다.

## 6. GPT/Claude 인계 요약

Codex가 대검 플레이 상태를 수치로 확인했고, sword bounds ratio를 `2.26 -> 1.33`으로 낮췄다. 정렬도 플레이어 앞에서 뒤로 옮겼다. 다음 리뷰는 대검이 여전히 무거운지, 아니면 너무 뒤로 숨어 약해졌는지를 보면 된다.

## 7. 다음 Codex 작업

직접 플레이에서 대검이 너무 약해 보이면 크기를 다시 키우지 말고, 공격 순간의 slash/impact만 살리는 쪽으로 조정한다. 몸을 덮는 held sprite를 다시 키우는 방향은 피한다.

## 8. 포트폴리오 메모: 문제, 방향, 행동, 결과

- 문제: 대검이 캐릭터보다 2배 이상 크게 렌더되고 앞 레이어에서 몸을 가렸다.
- 방향: held weapon은 캐릭터 실루엣을 보조하고, 공격감은 slash VFX가 담당하게 분리한다.
- 행동: 대검 scale, 위치, 레이어, cleave VFX scale을 함께 줄였다.
- 결과: 대검 bounds ratio가 `2.26`에서 `1.33`으로 내려갔고 console error 없이 동작했다.