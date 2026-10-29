#!/usr/bin/env bash
# README의 협업·코드 규칙을 PR마다 자동 검사한다. (Unity 없이 실행되는 가벼운 검사)
# 로컬 실행: bash .github/scripts/check-repo.sh
set -uo pipefail

errors=0
fail() { echo "::error::$1"; errors=$((errors + 1)); }

mapfile -t files < <(git ls-files)
declare -A tracked
for f in "${files[@]}"; do tracked["$f"]=1; done

# 1. Unity가 자동 생성하는 폴더는 커밋하지 않는다
for f in "${files[@]}"; do
  case "$f" in
    Library/*|Temp/*|Logs/*|UserSettings/*|Obj/*|obj/*|Build/*|Builds/*)
      fail "자동 생성 폴더의 파일이 커밋됨: $f" ;;
  esac
done

# 2. .meta 파일은 원본과 함께 커밋한다 (빠지면 참조가 깨짐)
declare -A dirs
for f in "${files[@]}"; do
  [[ "$f" == Assets/* && "$f" != *.meta ]] || continue
  d="$(dirname "$f")"
  while [[ "$d" == Assets/* ]]; do dirs["$d"]=1; d="$(dirname "$d")"; done
done
for f in "${files[@]}"; do
  [[ "$f" == Assets/* ]] || continue
  if [[ "$f" == *.meta ]]; then
    target="${f%.meta}"
    [[ -n "${tracked[$target]:-}" || -n "${dirs[$target]:-}" ]] || fail ".meta만 있고 원본이 없음: $f"
  else
    [[ -n "${tracked[$f.meta]:-}" ]] || fail ".meta 파일 누락: $f.meta"
  fi
done
for d in "${!dirs[@]}"; do
  [[ -n "${tracked[$d.meta]:-}" ]] || fail "폴더 .meta 파일 누락: $d.meta"
done

# 3. 큰 파일 방지 (GitHub는 100MB 초과 파일을 거부)
for f in "${files[@]}"; do
  [[ -f "$f" ]] || continue
  size=$(wc -c < "$f")
  (( size > 50 * 1024 * 1024 )) && fail "50MB를 넘는 파일: $f ($((size / 1024 / 1024))MB)"
done

# 4. 코드 규칙: 파일 이름 = 클래스 이름, namespace = 폴더 이름, Core는 다른 기능을 참조하지 않음
for f in "${files[@]}"; do
  [[ "$f" == Assets/_Project/Scripts/*/*.cs ]] || continue
  feature="$(cut -d/ -f4 <<< "$f")"
  name="$(basename "$f" .cs)"
  grep -Eq "^namespace CafeTime\.$feature\b" "$f" || fail "namespace가 CafeTime.$feature 가 아님: $f"
  grep -Eq "(class|struct|interface|enum) $name\b" "$f" || fail "파일 이름($name)과 같은 이름의 타입이 없음: $f"
  if [[ "$feature" == Core ]] && grep -E "^using CafeTime\." "$f" | grep -vq "CafeTime.Core"; then
    fail "Core가 다른 기능 폴더를 참조함: $f"
  fi
done

if (( errors > 0 )); then
  echo "검사 실패: $errors 건"
  exit 1
fi
echo "검사 통과 (파일 ${#files[@]}개)"
