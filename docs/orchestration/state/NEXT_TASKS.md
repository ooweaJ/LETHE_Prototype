# Next Tasks

Keep this file short. Detailed history belongs in `docs/TASK.md`, `docs/orchestration/devlog/`, `state/DECISION_LOG.md`, or evidence files.

Current source of truth:

- Product direction: `docs/PRD.md`
- Technical structure: `docs/TECH.md`
- Active work board: `docs/TASK.md`
- Test gate: `docs/TEST.md`
- Detailed design: `docs/design/README.md` and `docs/design/LETHE_DESIGN_00..07`

## 1. A단계 데이터 계약 마무리

- Priority: highest
- Include: `MemoryDefinition`, `EchoDefinition`, `UltimateEchoDefinition`, `EnemyDefinition`, `EncounterDefinition`, `RewardPoolDefinition`이 A-I EPIC을 담을 수 있는지 확인한다.
- Done: `dotnet build LETHE/Assembly-CSharp.csproj --nologo` 통과, 기존 weapon data asset 유지, 다음 B-I 작업이 이 계약을 기준으로 내려갈 수 있음.

## 2. B단계 공격 타격감 / 잔향 가독성 보정

- Priority: high
- Include: 쌍검 2연 반달, 대검 큰 반달, 흰색 피격 플래시, 데미지 숫자, 넉백, hitstop, 칼무리 후속타, 혈반 표식/회복실/피꽃.
- Done: jaewoo가 무기별 타격감과 잔향 후속타를 화면만 보고 구분할 수 있음.

## 3. C단계 M2 실제 플레이 루프 연결

- Priority: high
- Include: XP, 카드 선택, 기억 강화, 최고 레벨 망각, 잔향, 공명, +5, 피의 칼폭풍을 디버그 없이 60~120초 안에 도달하도록 연결한다.
- Done: debug smoke가 아니라 일반 플레이 흐름에서 핵심 감정 루프가 보임.

## 4. D단계 주요 스프라이트/VFX 교체

- Priority: high
- Include: 플레이어 4방향 idle/move, 쌍검/대검, 적 2~3종, 칼무리/혈반 기억 및 잔향, 피의 칼폭풍.
- Done: 캐릭터/무기/기억/잔향이 임시 도형이 아니라 평가 가능한 게임 이미지로 보임.

## 5. E-G단계 콘텐츠 확장

- Priority: medium
- Include: 기억 8종, 잔향 8종, 궁극 4종을 데이터 에셋과 런타임 효과로 확장한다.
- Done: 카드 선택과 실제 전투에서 8기억/8잔향/4궁극을 테스트할 수 있음.
