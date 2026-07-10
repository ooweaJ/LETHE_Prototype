# 2026-07-10-01 - 프로젝트 썸네일 / 인게임 인트로 키아트 추가

## 1. 현재 빌드 상태

`Dev_Prototype_v1` 첫 화면에 LETHE 키아트 기반 인트로 배경이 들어갔다. 프로젝트 썸네일용 정사각 키비주얼도 `_dev` UI 스프라이트로 추가했다.

## 2. 오늘 바뀐 것

- `spr_lethe_project_thumbnail_01.png` 추가.
- `spr_lethe_intro_background_01.png` 추가.
- `V1GameManager.DrawLetheIntroOverlay()`가 생성된 인트로 배경을 깔고, 기존 큰 패널을 반투명 글래스 패널로 낮추도록 변경.
- `V1_ContentCatalog.asset`에 UI 스프라이트 2개 연결.
- Game View 증거 이미지 저장:
  - `LETHE/Assets/_dev/Evidence/lethe_intro_keyart_screen_20260710.png`

## 3. 테스트 결과와 근거

- Unity `Assets/Refresh`: 성공.
- Unity compilation errors: `0`.
- Unity Play Mode에서 인트로 화면 캡처 성공.
- Unity console errors after capture: `0`.
- `dotnet build LETHE/Assembly-CSharp.csproj --nologo`: 기존 레거시 경고 7개, 오류 0개.

## 4. 결정한 것

- 인트로는 별도 클릭 시작 화면보다 무기 선택 화면을 유지한다.
- 단, 배경은 LETHE 키아트로 올리고 카드/텍스트는 그 위에 얹는다.
- 프로젝트 썸네일은 현재 빌드 아이콘 강제 적용보다 `_dev` 키비주얼 자산으로 먼저 안전하게 추가한다.

## 5. 문제 또는 리스크

- 직접 플레이에서 카드 텍스트가 많다고 느껴질 수 있다.
- 현재 화면은 정적인 키아트 기반이다. 더 의식적인 시작 감성을 원하면 다음 단계에서 무기 선택 애니메이션이나 페이드/입수 연출이 필요하다.

## 6. GPT/Claude 인계 요약

LETHE 첫 화면은 이제 어두운 강, 기억 파편, 좌측 청색 쌍검, 우측 적색 대검 잔향이 보이는 키아트 위에 무기 선택 카드가 올라간다. 다음 리뷰는 “첫 화면이 충분히 게임답고 의식적인가”, “카드가 잘 읽히는가”, “텍스트를 줄여야 하는가”를 보면 된다.

## 7. 다음 Codex 작업

- jaewoo 직접 플레이 피드백에 따라 인트로 카드 텍스트량, 제목 처리, 배경 어둡기, 무기 선택 연출 중 하나만 좁게 조정한다.
- 별도 요청이 있으면 프로젝트 썸네일을 Unity Player icon 또는 외부 포트폴리오 썸네일 경로로 확장 적용한다.

## 8. 포트폴리오 메모

- 문제: 기존 첫 화면이 기능적으로는 동작하지만 LETHE의 기억/망각 감성을 충분히 만들지 못했다.
- 방향: 인트로를 설명 화면이 아니라 첫 정서 신호로 만든다.
- 행동: 키비주얼 생성, UI 자산 import, 런타임 배경 연결, Game View 증거 캡처를 한 단위로 처리했다.
- 결과: 첫 화면이 어두운 강과 기억 파편을 기반으로 한 게임 셸처럼 보이기 시작했다.
