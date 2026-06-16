# 2026-06-16-01 - 카드 선택 정지와 히트스톱 이동감 수정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 이제 카드 선택 중 적과 투사체가 멈추고, 무기/잔향 히트스톱 중에도 플레이어 쪽 이동·카메라·무기 비주얼 업데이트가 끊기지 않도록 수정됐다.

## 2. 오늘 바뀐 것

- `GameplayPaused`를 추가했다.
  - 시작 무기 선택.
  - 레벨업 카드 선택.
  - 망각 결과 화면.
  - 공명 재획득 화면.
  - 사망 화면.
- 적, 칼무리 투사체, 적 탄환이 pause 상태를 존중하게 바꿨다.
- `HitstopActive`를 추가했다.
  - 전투 히트스톱 때 적/탄은 잠깐 멈춘다.
  - 플레이어, 카메라, 무기 비주얼은 계속 업데이트된다.
- 대검 기본공격이나 칼무리 잔향 때문에 캐릭터 이동이 잠깐 끊기는 느낌을 줄였다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `pauseDistance=0.0000`
  - `unpauseDistance=0.0240`
  - `weaponAnimAfterHitstop=0.220`
  - `hitstopAfterUpdate=0.060`
  - `gameplayPaused=False`
  - `hitstopActive=True`
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 1 unit heading ok.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 카드 선택은 게임 진행을 멈추는 진짜 pause로 취급한다.
- 히트스톱은 플레이어 입력까지 막는 정지가 아니라, 적/탄/전투 연출 쪽에만 걸리는 충격 표현으로 낮춘다.

## 5. 문제 또는 리스크

- 플레이어 이동감은 개선됐지만, 실제 체감은 jaewoo가 이동하면서 대검/칼무리 잔향을 써봐야 확정된다.
- 너무 부드러워지면 타격감이 약해질 수 있다. 그 경우 카메라 흔들림, 적 피격 플래시, VFX를 더 키우는 쪽이 맞다.
- Project Orchestrator Discord intake는 현재 `fetch failed`를 반환한다.

## 6. GPT/Claude 인계 요약

카드 선택 중 적 이동과 hitstop으로 인한 캐릭터 정지감은 구조상 문제였다. pause와 hitstop을 분리했고, 독립 업데이트되는 적/탄이 이를 따르게 했다.

## 7. 다음 Codex 작업

- jaewoo가 카드 선택 중 적 정지와 이동 중 대검/칼무리 hitstop 체감을 다시 확인한다.
- 여전히 멈춘다고 느껴지면 플레이어 측 hitstop을 완전히 제거하고, 적 freeze + VFX + shake 중심으로 타격감을 만든다.

## 8. 포트폴리오 메모

- 문제: UI 선택과 전투 연출이 플레이어 제어감을 해쳤다.
- 방향: 선택 pause와 전투 hitstop을 분리했다.
- 행동: 전역 pause/freeze 플래그를 추가하고 독립 업데이트 객체들이 따르게 했다.
- 결과: 스모크에서 pause 중 적 이동 0, pause 해제 후 이동, hitstop 중 매니저 업데이트 지속을 확인했다.

# 2026-06-16-02 - 쌍검/대검 반달형 타격 VFX 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`의 기본공격 VFX가 얇은 선 중심에서 반달형 베기 중심으로 바뀌었다. 쌍검은 두 개의 작은 반달이 빠르게 겹쳐지고, 대검은 큰 반달이 주변 범위를 베었다고 읽히도록 했다.

## 2. 오늘 바뀐 것

- 쌍검 기본공격:
  - `DualBladeCrescent_A`
  - `DualBladeCrescent_B`
  - 두 개의 반달형 VFX가 타겟 위치에 엇갈려 생성된다.
- 대검 기본공격:
  - `GreatswordCrescent_Aoe`
  - `GreatswordCrescent_Primary`
  - 큰 반달 잔광과 밝은 주 베기가 함께 뜬다.
- 칼무리 잔향:
  - 후속타도 반달형 VFX를 사용하게 바꿨다.
  - 대검 칼무리 후속타는 큰 반달 1회로 읽히게 했다.
- 피의 칼폭풍 대검참도 넓은 반달형 VFX를 사용한다.
- 피격 섬광은 베기선과 겹치지 않도록 impact diamond로 바꿨다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `dualCrescent=6`
  - `kalmuriCrescent=10`
  - `greatCrescent=6`
  - `heavyKalmuri=1`
  - `shock=1`
- 증거: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_feedback_20260616.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 2 unit headings ok.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 기본공격의 첫 판독은 데미지 숫자가 아니라 베기 형태로 만든다.
- 쌍검은 작은 반달 2개, 대검은 큰 반달 1개라는 형태 규칙을 유지한다.
- 잔향도 이 무기 형태 언어를 따라가야 한다.

## 5. 문제 또는 리스크

- 아직 절차형 런타임 VFX라 최종 아트는 아니다.
- 타격감이 여전히 약하면 다음은 코드 수치보다 전용 스프라이트/사운드 placeholder가 필요하다.
- Project Orchestrator Discord intake는 현재 `fetch failed`를 반환한다.

## 6. GPT/Claude 인계 요약

타격감 부족 피드백에 따라 쌍검/대검/칼무리 후속타의 VFX 언어를 반달형으로 통일했다. 다음 리뷰에서는 쌍검이 “슥슥”, 대검이 “큰 범위 베기”로 읽히는지 확인해야 한다.

## 7. 다음 Codex 작업

- jaewoo가 쌍검/대검을 각각 플레이하며 반달형 VFX를 확인한다.
- 약하면 전용 sprite atlas 제작 또는 임시 사운드/화면 흔들림 레이어를 붙인다.

## 8. 포트폴리오 메모

- 문제: 공격이 맞아도 화면에서 베었다는 감각이 약했다.
- 방향: 무기별 타격 형태를 시각 규칙으로 고정했다.
- 행동: 쌍검/대검/잔향 VFX를 반달형으로 재구성했다.
- 결과: 스모크에서 쌍검, 대검, 칼무리 후속타 반달 VFX 생성이 확인됐다.

# 2026-06-16-03 - 반달형 참격 크기와 지속시간 보정

## 1. 현재 빌드 상태

쌍검 반달 VFX는 더 커지고 오래 남는다. 대검 반달 VFX는 두께와 채움이 줄어 부채꼴보다 얇은 참격선에 가깝게 보이도록 조정됐다.

## 2. 오늘 바뀐 것

- 쌍검 1타 반달:
  - scale `0.62 -> 0.78`.
  - lifetime `0.13s -> 0.21s`.
- 쌍검 2타 반달:
  - scale `0.54 -> 0.68`.
  - lifetime `0.15s -> 0.23s`.
- 쌍검 보조 타격:
  - scale `0.38 -> 0.50`.
  - lifetime `0.10s -> 0.15s`.
- 대검 AoE 반달:
  - scale `1.02 -> 0.88`.
  - alpha `0.40 -> 0.32`.
  - lifetime `0.28s -> 0.24s`.
- 대검 주 베기:
  - scale `0.78 -> 0.66`.
- `MakeWideCrescentSprite` 내부 채움과 glow를 줄여 부채꼴 면적감을 낮췄다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과, 7 warning / 0 error.
- Unity compile errors: `count=0`.
- Play Mode targeted smoke:
  - `dualCrescent=6`
  - `dualMaxScale=0.78`
  - `kalmuriCrescent=10`
  - `greatCrescent=6`
  - `greatMaxScale=0.88`
  - `shock=1`
- 증거: `LETHE/Assets/_dev/Evidence/v1_crescent_slash_timing_size_20260616.png`.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 3 unit headings ok.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.

## 4. 결정한 것

- 쌍검은 작고 빠른 선보다, 조금 더 오래 보이는 2연 반달이 맞다.
- 대검은 큰 면이 아니라 얇고 긴 궤적이어야 한다.
- 절차형 VFX 튜닝은 여기서 더 과하게 끌지 않고, 다음에도 약하면 전용 스프라이트로 넘어간다.

## 5. 문제 또는 리스크

- 아직 런타임 절차형 참격이라 전문 아트 느낌은 제한적이다.
- 지속시간을 늘렸기 때문에 화면에 잔상이 조금 많아질 수 있다.
- Project Orchestrator Discord intake는 현재 `fetch failed`를 반환한다.

## 6. GPT/Claude 인계 요약

쌍검은 보이지 않을 만큼 작고 짧았고, 대검은 너무 두꺼워 부채꼴처럼 보였다. 쌍검은 크기/지속시간을 올리고, 대검은 scale/alpha/내부 채움을 줄여 얇은 참격에 가깝게 보정했다.

## 7. 다음 Codex 작업

- jaewoo가 쌍검 2연 반달이 보이는지 확인한다.
- 대검이 부채꼴이 아니라 큰 참격선으로 읽히는지 확인한다.
- 여전히 약하면 전용 slash sprite 제작으로 넘어간다.

## 8. 포트폴리오 메모

- 문제: 같은 “반달형”이라도 크기와 두께가 다르면 전혀 다른 공격처럼 읽혔다.
- 방향: 쌍검은 가시성, 대검은 날카로움을 각각 보정했다.
- 행동: scale, alpha, lifetime, procedural texture fill을 조정했다.
- 결과: 스모크에서 커진 쌍검 반달과 줄어든 대검 반달 생성이 확인됐다.
