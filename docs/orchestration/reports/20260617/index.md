# 2026-06-17-01 - A-I 코어 프로토타입 작업 구조화와 데이터 계약 보강

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 계속 현재 Unity 메인 프로토타입이다. 오늘은 바로 새 콘텐츠를 덧붙이기보다, A-I 전체 작업을 이어서 만들 수 있도록 루트 문서와 ScriptableObject 데이터 계약을 먼저 정리했다.

## 2. 오늘 바뀐 것

- 새 루트 문서 입구를 만들었다.
  - `docs/PRD.md`
  - `docs/TECH.md`
  - `docs/TASK.md`
  - `docs/TEST.md`
  - `docs/CHANGELOG.md`
- `AGENTS.md`의 작업 전 읽는 순서를 새 문서 구조에 맞게 바꿨다.
- 깨져 있던 `docs/orchestration/state/NEXT_TASKS.md`를 A-I 작업 기준으로 다시 작성했다.
- `LETHE/Assets/_dev/Scripts/Core/DefinitionTypes.cs`를 확장했다.
  - 기억 효과 종류.
  - 잔향 형태 종류.
  - 적 역할.
  - encounter spawn mode.
  - 궁극 trigger mode.
  - M2 encounter wave data.
  - `UltimateEchoDefinition`.
  - `EncounterDefinition`.
  - 확장된 `MemoryDefinition`, `EchoDefinition`, `EchoSynergyDefinition`, `EnemyDefinition`, `RewardPoolDefinition`.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 7 warning / 0 error.
- warning 7개는 기존 v0/debug 코드의 `Object.FindObjectOfType<T>()` deprecation warning이다.
- 이번 세션의 도구 목록에는 AnkleBreaker Unity MCP가 노출되지 않고 UE-MCP만 노출되어 Unity Editor compile/play-mode 검증은 아직 실행하지 못했다.

## 4. 결정한 것

- A-I 작업을 하나의 EPIC으로 보고 진행한다.
- 새 기능은 가능한 한 `Definition asset -> Runtime -> VFX/Prefab -> TEST -> report` 흐름으로 만든다.
- 기존 `EchoSynergyDefinition`은 Blood Blade Storm 호환성을 위해 유지하고, 4궁극 확장용으로 `UltimateEchoDefinition`을 추가한다.

## 5. 문제 또는 리스크

- Unity MCP가 현재 도구 목록에 보이지 않아 Editor 검증은 아직 남아 있다.
- 아직 런타임 전체가 새 데이터 계약을 읽는 것은 아니다. 오늘은 A단계의 계약을 만든 것이고, B-I에서 점진적으로 실제 런타임 연결이 필요하다.
- 기존 소스 일부 한글 기본 문자열은 터미널에서 깨져 보인다. 컴파일은 통과하지만 장기적으로는 문자열/폰트/인코딩 정리가 필요하다.

## 6. GPT/Claude 인계 요약

LETHE는 이제 A-I EPIC을 기준으로 진행한다. 첫 단위로 루트 문서 구조와 데이터 계약을 만들었다. 다음 판단 지점은 B단계, 즉 쌍검/대검 기본공격 타격감과 칼무리/혈반 잔향 가독성을 먼저 끌어올리는 것이다.

## 7. 다음 Codex 작업

- B단계 공격 타격감 / 잔향 가독성 보정 시작.
- 쌍검 2연 반달, 대검 큰 반달, 피격 플래시, 데미지 숫자, 넉백, hitstop을 새 데이터 구조 기준으로 점검한다.
- 칼무리 잔향은 적 위치 후속타, 혈반 잔향은 표식 -> 회복 실 -> 피꽃 흐름으로 정리한다.

## 8. 포트폴리오 메모

- 문제: Unity 프로토타입이 기능별로 덧붙으며 구조와 작업 기준이 흐려질 위험이 있었다.
- 방향: PRD/TECH/TASK/TEST/CHANGELOG와 orchestration을 분리해 개인 AI 게임개발 규약으로 정리했다.
- 행동: A-I EPIC을 문서화하고, Unity 콘텐츠 확장을 받을 ScriptableObject 데이터 계약을 보강했다.
- 결과: 다음 작업부터는 즉흥 구현이 아니라 데이터 계약과 작업 보드를 기준으로 진행할 수 있다.
# 2026-06-17-02 - 칼무리 후속타와 혈반 회복 실 1차 보정

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 A-I EPIC의 B단계로 들어갔다. 오늘 두 번째 단위는 전체 공격 시스템을 갈아엎는 작업이 아니라, 잔향이 화면에서 더 잘 읽히도록 칼무리/혈반의 후속 표현을 보강한 작업이다.

## 2. 오늘 바뀐 것

- 칼무리 잔향 후속타가 적 위치에서 짧은 범위 링을 먼저 보여준다.
- 혈반이 묻은 적을 다시 무기로 때리면 붉은 실이 플레이어에게 돌아오고 소량 회복한다.
- 혈반 +5 피꽃 폭발도 회복 실을 함께 보여준다.
- 적 넉백 상한을 `4.6 -> 6.2`로 올려 대검 손맛이 덜 눌리게 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 7 warning / 0 error.
- warning 7개는 기존 v0/debug 코드의 deprecation warning이다.
- Unity MCP가 현재 도구 목록에 없어 Play Mode 시각 검증은 아직 남았다.

## 4. 결정한 것

- 칼무리 잔향은 캐릭터 주변 장식이 아니라 타격 지점에서 후속타가 터지는 언어로 유지한다.
- 혈반 잔향은 단순 표식이 아니라 다시 때렸을 때 플레이어에게 돌아오는 실과 회복으로 읽히게 한다.
- 대검 피드백은 데이터값이 있어도 런타임 상한에 눌리면 체감이 죽으므로 넉백 상한을 높였다.

## 5. 문제 또는 리스크

- 링/실 VFX가 실제 화면에서 너무 많거나 방해될 수 있다. Unity Play Mode 리뷰가 필요하다.
- 혈반 회복량은 아직 1차 감각값이다. 밸런스는 I단계에서 조정해야 한다.
- Unity MCP가 안 보이는 동안은 에디터 screenshot/evidence를 남기지 못한다.

## 6. GPT/Claude 인계 요약

칼무리와 혈반 잔향이 "잔향!" 텍스트나 모호한 선이 아니라, 적 위치 후속타와 플레이어로 돌아오는 회복 실로 읽히도록 첫 보정을 넣었다. 다음 리뷰에서는 VFX가 잘 보이는지, 너무 복잡하지 않은지, 혈반 회복이 플레이 감각에 도움 되는지 확인하면 된다.

## 7. 다음 Codex 작업

- Unity MCP가 보이면 Play Mode에서 칼무리 링/혈반 실을 시각 확인한다.
- 이어서 B단계 남은 부분인 쌍검/대검 기본공격 타격감과 피격 피드백을 더 보정한다.
- 이후 C단계 M2 실제 플레이 루프 연결로 넘어간다.

## 8. 포트폴리오 메모

- 문제: 잔향이 수치상 발동해도 화면에서는 어떤 효과인지 읽히기 어려웠다.
- 방향: 잔향을 "약한 같은 효과"가 아니라 "타격 지점 후속 현상"과 "회복 실"처럼 형태가 바뀐 효과로 보여준다.
- 행동: 칼무리 range ring, 혈반 heal thread, 피꽃 thread, 넉백 cap 보정을 구현했다.
- 결과: 다음 플레이 리뷰에서 잔향 가독성을 검증할 수 있는 구체적 화면 변화가 생겼다.

# 2026-06-17-03 - M2 실제 루프 진행 HUD와 세 번째 기억 카드

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 C단계 첫 패스로 들어갔다. 이번 단위는 M2 루프를 완전히 밸런싱한 것은 아니고, 플레이어가 현재 루프의 어느 지점에 있는지 알 수 있게 HUD와 카드 선택 흐름을 보강한 작업이다.

## 2. 오늘 바뀐 것

- HUD에 M2 현재 목표/상태 문구를 추가했다.
- 레벨업 카드에서 혈반 기억을 얻은 뒤 빈 슬롯이 있으면 `멈춘 1초`를 세 번째 기억으로 고를 수 있게 했다.
- 이로써 기억 3칸 확보가 자동 리뷰 보정에만 의존하지 않게 되었다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 7 warning / 0 error.
- warning 7개는 기존 v0/debug 코드의 deprecation warning이다.
- Unity MCP가 현재 도구 목록에 없어 Play Mode 시각 검증은 아직 남았다.

## 4. 결정한 것

- C단계는 먼저 "플레이어가 루프 상태를 이해하는가"부터 해결한다.
- 기억 3칸 확보는 카드 선택으로 열어두고, 자동 리뷰 보정은 이후 실제 pacing이 안정되면 줄인다.
- +5/궁극까지의 실제 pacing 완성은 다음 C단계 작업으로 남긴다.

## 5. 문제 또는 리스크

- `멈춘 1초`의 실제 전투 효과는 아직 충분히 구현되지 않았다.
- HUD 문구가 실제 화면에서 너무 길 수 있어 Unity Play Mode 확인이 필요하다.
- 자동 리뷰 보정이 아직 남아 있으므로 C단계는 완료가 아니라 첫 연결 패스다.

## 6. GPT/Claude 인계 요약

M2 루프가 디버그 버튼 없이 읽히도록 HUD 상태 문구와 세 번째 기억 카드 경로를 추가했다. 다음에는 자동 보정 없이도 망각, 공명, +5, 궁극까지 닿는 실제 pacing을 조정해야 한다.

## 7. 다음 Codex 작업

- Unity Play Mode에서 HUD 문구가 화면을 가리지 않는지 확인한다.
- `멈춘 1초`가 세 번째 기억 카드로 정상 노출되는지 확인한다.
- 다음 C단계로 실제 +5/궁극 pacing을 디버그 주입이 아니라 플레이 루프로 연결한다.

## 8. 포트폴리오 메모

- 문제: M2 핵심 루프는 기능상 존재하지만 플레이어가 현재 목표를 놓치기 쉬웠다.
- 방향: 루프 상태를 HUD에 직접 노출하고, 기억 3칸 확보를 카드 선택으로 가능하게 만든다.
- 행동: `M2LoopText()`와 세 번째 기억 카드 경로를 추가했다.
- 결과: 다음 리뷰에서 루프 이해도와 진행감 자체를 검증할 수 있게 되었다.

# 2026-06-17-04 - 8기억 8잔향 4궁극 1차 런타임 확장

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 E/F/G 단계의 첫 런타임 확장까지 들어갔다. 아직 최종 밸런스나 전용 아트 교체가 아니라, 모든 기억·잔향·궁극이 최소한 서로 다른 전투 동작으로 존재하는지 확인하기 위한 1차 구현이다.

## 2. 오늘 바뀐 것

- 남은 활성 기억 6종에 1차 전투 동작을 추가했다.
  - 처형 섬광: 체력이 낮은 적에게 밝은 처형 타격.
  - 사냥꾼의 맹세: 가장 가까운 적에게 추적탄.
  - 파쇄의 파문: 밀집 적 주변에 파동 피해.
  - 멈춘 1초: 시간 정지 링과 짧은 동결.
  - 잿빛 방패: 방어 링과 받는 피해 감소.
  - 망각의 낙인: 무작위 적에게 보라색 낙인 피해.
- 칼무리/혈반 외 잔향 6종도 무기 타격 후속 반응을 갖게 했다.
- 기존 피의 칼폭풍 외에 3개 궁극의 최소 런타임 분기를 추가했다.
  - 파쇄의 파문 + 처형 섬광 = 파쇄 처형.
  - 멈춘 1초 + 사냥꾼의 맹세 = 정지 추적.
  - 잿빛 방패 + 망각의 낙인 = 잿빛 망각.
- HUD가 현재 준비된 궁극 이름을 표시한다.
- 레벨업 보상 카드가 아직 없는 기억 후보를 순환 노출할 수 있게 했다.

## 3. 테스트 결과와 근거

- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 0 warning / 0 error.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 4개 유닛 heading 정상.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.
- 현재 세션에서는 AnkleBreaker Unity MCP 도구가 노출되지 않아 Unity Play Mode 시각 검증은 아직 수행하지 못했다.

## 4. 결정한 것

- 8기억/8잔향/4궁극은 먼저 "다른 효과처럼 느껴지는 최소 동작"을 완성 기준으로 삼는다.
- 전용 스프라이트와 세부 밸런스는 다음 패스에서 플레이 피드백을 보고 좁힌다.
- 잔향은 약해진 복사본이 아니라 무기 타격 후속 반응으로 읽히게 유지한다.

## 5. 문제 또는 리스크

- 전용 VFX가 부족한 효과는 아직 임시 링/다이아몬드/펄스 표현에 의존한다.
- 실제 플레이에서 효과 밀도가 과하거나, 반대로 눈에 안 띌 수 있다.
- 데이터 asset화는 계약 구조만 준비되어 있고, 8기억/8잔향/4궁극 전체 asset 생성은 아직 남아 있다.

## 6. GPT/Claude 인계 요약

8기억/8잔향/4궁극의 1차 런타임이 모두 연결됐다. 다음 리뷰에서는 각 효과가 이름만 다른 것이 아니라 전투 행동과 화면 언어가 다르게 읽히는지 확인해야 한다. 특히 파쇄/정지/처형/망각 계열이 칼무리·혈반과 충분히 구분되는지가 핵심이다.

## 7. 다음 Codex 작업

- Unity Play Mode에서 8기억/8잔향/4궁극 발동을 실제 화면으로 확인한다.
- 부족한 효과부터 전용 sprite/VFX로 교체한다.
- ScriptableObject asset을 8기억/8잔향/4궁극까지 확장한다.
- M2 실제 플레이 pacing을 자동 주입 없이 60~120초 안에 닿도록 조정한다.

## 8. 포트폴리오 메모

- 문제: 완성형 프로토타입 범위가 2기억/2잔향에 머물면 게임의 핵심인 조합 가능성을 평가할 수 없었다.
- 방향: 모든 기억·잔향·궁극이 최소 동작을 갖게 만들어 리뷰 가능한 전체 구조를 먼저 세운다.
- 행동: 활성 기억, 무기 타격 잔향, 궁극 조건과 HUD 표시를 한 번에 확장했다.
- 결과: 다음 플레이 피드백은 "존재 여부"가 아니라 "가독성, 타격감, 밸런스"에 집중할 수 있게 되었다.

# 2026-06-17-05 - 8기억 8잔향 4궁극 데이터 asset 골격

## 1. 현재 빌드 상태

`Dev_Prototype_v1`은 런타임 1차 구현에 이어 `_dev/Data` 쪽에도 완성형 프로토타입의 데이터 골격을 갖췄다. 이제 2기억/2잔향만 asset으로 보이는 상태가 아니라, 8기억/8잔향/4궁극이 프로젝트 파일 구조에서도 확인된다.

## 2. 오늘 바뀐 것

- `LETHE/Assets/_dev/Data/Memories`에 누락된 6개 `MemoryDefinition` asset을 추가했다.
- `LETHE/Assets/_dev/Data/Echoes`에 누락된 6개 `EchoDefinition` asset을 추가했다.
- `LETHE/Assets/_dev/Data/Ultimates` 폴더와 4개 `UltimateEchoDefinition` asset을 추가했다.
- 각 asset에는 id, source/matching id, 기본 설명, trigger/form/effect kind, 무기별 표현 메모를 넣었다.

## 3. 테스트 결과와 근거

- asset count: Memory 8 / Echo 8 / Ultimate 4.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 통과.
- 결과: 최신 빌드 기준 legacy v0/debug warning 7개 / error 0개.
- `npm.cmd run report`: 통과.
- `npm.cmd run report:check`: 통과, 5개 유닛 heading 정상.
- `npm.cmd run report:orchestrator:unit:dry`: `fetch failed`로 실패.
- Unity MCP 검증:
  - LETHE 인스턴스 port `7890` 선택.
  - `Assets/Refresh` 성공.
  - Unity compile errors count `0`.
  - AssetDatabase에서 MemoryDefinition 8개, EchoDefinition 8개, UltimateEchoDefinition 4개 확인.
  - `Dev_Prototype_v1` 짧은 Play Mode smoke에서 console error 없음.
  - 증거 스크린샷: `LETHE/Assets/_dev/Evidence/v1_content_data_asset_play_smoke_20260617.png`.

## 4. 결정한 것

- 이번 asset은 최종 밸런스 데이터가 아니라 완성형 프로토타입의 데이터 자리표시자다.
- runtime과 asset 이름/id를 맞춰, 이후 Unity Editor에서 prefab/VFX/밸런스 수치를 연결하기 쉽게 둔다.
- `UltimateEchoDefinition`은 기존 `EchoSynergyDefinition`과 병행해 4궁극 확장용 기준으로 쓴다.

## 5. 문제 또는 리스크

- Unity Editor import는 확인됐고, 다음에는 Inspector 저장/런타임 catalog 연결 검증이 필요하다.
- 기존 2기억/2잔향 asset은 더 오래된 필드 구성을 갖고 있어, 다음 Unity import 후 Inspector 정리/저장이 필요할 수 있다.
- 런타임은 아직 이 asset 전체를 직접 소비하지 않는다. 다음 구조화 패스에서 catalog 연결이 필요하다.

## 6. GPT/Claude 인계 요약

8기억/8잔향/4궁극이 코드와 데이터 파일 양쪽에 존재한다. 다음 검토는 "이 asset taxonomy가 기획 문서의 효과 구분을 잘 담는지"와 "runtime catalog가 이 데이터를 읽도록 옮길 우선순위"를 보면 된다.

## 7. 다음 Codex 작업

- Unity Editor에서 `_dev/Data` asset import 상태를 확인한다.
- 기존 Hungry/Blood/Kalmuri/Blood asset을 새 필드 구성에 맞춰 Inspector 저장한다.
- runtime catalog가 asset 배열을 참조하도록 옮기는 구조를 검토한다.
- 이후 D단계 전용 sprite/VFX 교체와 I단계 수치 조정을 진행한다.

## 8. 포트폴리오 메모

- 문제: 런타임에는 8/8/4가 생겼지만 프로젝트 데이터 구조는 여전히 2/2/1처럼 보였다.
- 방향: Unity 프로젝트를 열었을 때도 완성형 프로토타입의 콘텐츠 범위가 바로 읽히게 만든다.
- 행동: 누락된 Memory/Echo/Ultimate asset과 meta 파일을 추가했다.
- 결과: 다음 작업자는 코드만 보지 않아도 전체 콘텐츠 축을 `_dev/Data`에서 파악할 수 있다.
