# 05. UI / UX 명세 (HTML parity)

최종 갱신: 2026-06-12 · 출처: `index.html`, `src/game.js` (render/overlay 함수)

이 문서는 그동안 Unity 설계에서 누락됐던 **기본 게임 셸** 명세다. Unity 빌드는 디버그 패널 이전에 아래 화면들을 HTML 수준으로 먼저 재현해야 한다. HTML이 이미 가진 것을 기준선으로 삼는다.

## 화면 구성 (레이아웃)

HTML은 `game-layout` 안에 **전투 아레나(중앙) + 사이드 패널(우측) + 상단 바 + 전투 로그**로 구성된다.

```text
[ 상단 바: 페이즈 · 타이머 · HP ]
[ 아레나(캔버스)        | 사이드 패널            ]
[                       | - 무기 카드            ]
[                       | - 기억 슬롯            ]
[                       | - 잔향/빌드 정체성     ]
[                       | - 상세 수치 표         ]
[ 전투 로그 (하단)                              ]
```

## 1. HUD (상시 표시)

출처 id: `phaseLabel`, `timerLabel`, `hpLabel`, `slotCount`, `memorySlots`, `echoList`, `weaponCard`, `detailTable`, `combatLog`, `clarityHint`.

| 요소 | 내용 | Unity 요구 |
| --- | --- | --- |
| 페이즈 라벨 | 현재 압박 페이즈명("압박 상승" 등) | 텍스트, 페이즈 전환 시 갱신 |
| 타이머 | 경과/남은 시간 | 텍스트 |
| HP 바 | 현재/최대 HP | 바 + 수치 |
| 슬롯 카운트 | 활성 기억 N/3 | 텍스트 |
| 무기 카드 | 현재 무기 이름·역할 | 카드 |
| 기억 슬롯 | 활성 기억 아이콘 + 레벨(+N) | 슬롯 스택 |
| 잔향/빌드 패널 | 보유 잔향 목록 + 레벨 + 각성 상태 | 리스트 |
| 다음 망각 후보 | 최고 레벨 기억 표시 + 보호 잔여("보호 N킬/N초") | 강조 표시 |
| 궁극 목표 | 현재 노리는 궁극 조합 진행 | 표시 |
| 상세 수치 표 | 빌드 정체성/시너지 요약 | 표 |
| 전투 로그 | 최근 이벤트 텍스트 | 스크롤 로그 |

## 2. 시작 화면 (start-panel)

출처: `weaponChoices`, `memoryChoices`, `startRunButton`, `weaponCardHtml`, `memoryChoiceHtml`.

- 무기 선택: 절단쌍검 / 장송대검 (카드 + 역할·설명).
- 시작 기억 선택: 8기억 중 후보 제시(이름·역할·설명·태그 배지). HTML은 다중 선택 + 시너지 미리보기 제공.
- 공명 가능 기억은 미리보기 라인 표시("공명: 잔향 Lv.N → 시작 Lv.M").
- "런 시작" 버튼.

## 3. 레벨업 화면 (upgrade-panel)

출처: `showLevelUpOverlay()`, `levelUpChoiceView()`, `applyLevelUpChoice()`.

- 레벨업 시 게임 일시정지, 오버레이 표시.
- 3개 선택지 버튼: 각 버튼에 **이름 + 설명 + 효과 로그**.
  - 새 기억: "새 기억: {이름}" / "{역할} · {설명}" / "빈 기억 슬롯을 채웁니다."
  - 기억 강화: "기억 강화: {이름}" / 효과 / "Lv.N → Lv.M".
  - 런 스탯: 스탯 이름 / 설명 / 로그(예 "피해 +14%").
- 선택 시 즉시 적용 → 전투 복귀. 선택 내역 누적 기록.

## 4. 망각 결과 화면 (result-panel) — 가장 중요

출처: `forgottenTitle`, `resultSummary`, `showRefillOverlay`.

망각이 후회로 느껴지게 만드는 핵심 화면. **반드시 구현**.

- "사라진 기억"을 크게 보여준다(이름 + 레벨).
- "남은 잔향"을 함께 보여준다(잔향 이름 + 새 레벨 + 각성 여부).
- 과부하가 발생했으면 과부하 결과(폭발/게이지)도 표시.
- 결손 생존 진입 안내.

## 5. 보충 화면 (refill overlay)

출처: `updateRefillGate()`, `showRefillOverlay()`.

- 결손 생존(54초) 종료 후 표시.
- 세 선택: 잃었던 기억 재획득(공명) / 새 기억 / 기존 강화.
- 공명 선택지는 시작 레벨 보너스를 미리 보여준다.

## 6. 사망 화면 (death overlay)

출처: `showDeathOverlay()`, `restartButton`.

- 사망 시 결과 요약(생존 시간, 도달 빌드, 망각 횟수 등).
- 재시작 버튼.

## 7. 디버그 패널

출처: `debugControls`, `bindDebugControls()` (HTML) + 현재 Unity F1~F8.

- 무기 변경: 쌍검/대검.
- 기억 추가(8종), 기억 강화 +1.
- 자동 망각 실행(`debugForgetNow`).
- 잔향 level set: +1/+3/+5(`debugSetEchoCap`).
- 궁극 조건 만들기(4종).
- 60초 밸런스 스모크 시작.
- 디버그 패널은 로직을 직접 들지 않고 상태만 주입한다(`DebugStateInjector`).

## 8. 플레이테스트 설문 (선택)

출처: `question-panel`, `survey`, `sadnessScale`, `fairnessScale`, `growthFeelScale`, `memoryRecallInput`, `downloadLogButton`.

- HTML은 망각 아쉬움/공정함/성장 체감 척도와 기억 회상 입력, 로그 다운로드를 제공. Unity 사람 테스트 단계에서 재현 가치 있음(우선순위 낮음).

## UI 우선순위 (Unity 구현 순서)

1. HUD(HP·타이머·기억 슬롯·잔향 패널·다음 망각 후보).
2. 레벨업 화면.
3. 망각 결과 화면.
4. 시작 화면(무기/기억 선택).
5. 보충 화면.
6. 사망 화면.
7. 디버그 패널(이미 존재, 유지).
8. 설문(후순위).

> 원칙: 디버그 패널만으로는 "게임"이 아니다. 1~3번이 돌아가야 "틀이 나왔다"고 본다.
