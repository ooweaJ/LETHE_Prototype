# 2026-06-10-07 - Unity 2D skeleton 커밋 준비

## 1. 현재 빌드 상태

Unity 2D 프로젝트가 `LETHE/` 아래 생성됐다. 이번 작업은 gameplay 구현이 아니라, Unity baseline을 안전하게 버전관리하기 위한 정리다.

## 2. 오늘 바뀐 것

- AnkleBreaker Unity MCP 서버 경로를 확인했다: `C:\jaewoo\unity-mcp-server\src\index.js`.
- Codex MCP에 `anklebreaker-unity`를 등록했다.
- Unity Editor bridge `127.0.0.1:7890` 연결을 확인했다.
- 이전 gamelovers/HTTP Unity MCP 등록인 `mcp-unity`, `unityMCP`를 제거했다.
- `.gitignore`에 Unity 생성물 제외 규칙을 추가했다.
- 이전 MCP 설정으로 보이는 `LETHE/ProjectSettings/McpUnitySettings.json`을 제거했다.
- Unity skeleton 커밋 대상은 `LETHE/Assets`, `LETHE/Packages`, `LETHE/ProjectSettings`로 좁혔다.

## 3. 테스트 결과와 근거

- `codex mcp list`: `anklebreaker-unity` 등록 확인.
- `Test-NetConnection 127.0.0.1 -Port 7890`: `TcpTestSucceeded: True`.
- Unity instance registry: `LETHE`, Unity `6000.3.10f1`, project path `C:/jaewoo/LETHE_Prototype/LETHE`, port `7890`.
- `git status --short --untracked-files=all`: Unity generated folders are ignored; project skeleton files remain as tracking candidates.

## 4. 결정한 것

- Unity MCP는 AnkleBreaker 서버를 기준으로 사용한다.
- Unity generated cache/build/editor files는 커밋하지 않는다.
- Unity 기본 skeleton은 커밋해 이후 MCP 작업의 기준점으로 삼는다.
- 첫 MCP 작업은 `LETHE_UNITY_ASSET_BINDING_PLAN.md`를 기준으로 `Assets/Lethe/` 구조와 reference art import부터 시작한다.

## 5. 문제 또는 리스크

- 현재 Codex 세션에는 AnkleBreaker MCP 도구가 아직 callable로 노출되지 않았다. 새 MCP 등록 후 세션/tool metadata reload가 필요할 수 있다.
- Unity skeleton은 기본 템플릿 상태라 실제 `Assets/Lethe/` 구조는 아직 없다.
- 플레이어/맵/적/런타임 VFX sprite는 아직 placeholder 또는 다음 imagegen pass가 필요하다.

## 6. GPT/Claude 인계 요약

Unity 프로젝트는 이제 버전관리 baseline으로 들어갈 준비가 됐다. AnkleBreaker MCP 연결은 포트 `7890`에서 확인됐고, 이전 MCP 등록은 제거됐다. 다음 구현은 Unity MCP 도구가 세션에 노출된 뒤 `LETHE_UNITY_ASSET_BINDING_PLAN.md`를 읽고 폴더/import/prefab/SO/scene 순서로 진행하면 된다.

## 7. 다음 Codex 작업

- Unity skeleton을 커밋/푸시한다.
- 새 세션 또는 MCP tool reload 후 AnkleBreaker Unity MCP 도구 노출을 확인한다.
- `Assets/Lethe/` 구조 생성과 콘셉트 이미지 reference import를 시작한다.

## 8. 포트폴리오 메모

- 문제: Unity 프로젝트를 만들었지만 생성물과 실제 프로젝트 파일이 섞이면 repo가 무거워지고 재현성이 떨어진다.
- 방향: Unity generated files를 제외하고, project skeleton만 baseline으로 버전관리한다.
- 행동: MCP 연결과 gitignore를 정리하고 Unity skeleton 커밋 범위를 확정했다.
- 결과: 이후 Unity MCP 작업을 안정적으로 쌓을 기준점이 생겼다.
