#!/usr/bin/env bash
# 새 ADR 파일을 template.md 로 만들고 목록(README.md)에 한 줄 추가한다. 상태는 '제안' 으로 시작한다.
# 사용법: bash docs/adr/new-adr.sh "결정 제목" english-file-slug
#   예:   bash docs/adr/new-adr.sh "손님 대기열은 큐로 관리한다" customer-queue
set -euo pipefail

dir="$(cd "$(dirname "$0")" && pwd)"
usage='사용법: bash docs/adr/new-adr.sh "결정 제목" english-file-slug'
title="${1:?$usage}"
slug="$(printf '%s' "${2:?$usage}" | tr '[:upper:]' '[:lower:]' | sed -E 's/[^a-z0-9]+/-/g; s/^-+|-+$//g')"
[[ -n "$slug" ]] || { echo "$usage" >&2; exit 1; }

last="$(ls "$dir" | grep -oE '^[0-9]{4}' | sort -n | tail -1 || true)"
num="$(printf '%04d' $((10#${last:-0} + 1)))"
file="$num-$slug.md"
escaped="$(printf '%s' "$title" | sed -e 's/[&/\]/\\&/g')"

sed -e "s/^# ADR-NNNN: 제목\$/# ADR-$num: $escaped/" \
    -e "s/^- 상태: .*/- 상태: 제안/" \
    -e "s/^- 날짜: .*/- 날짜: $(date +%F)/" \
    "$dir/template.md" > "$dir/$file"

# 목록 표는 README.md 의 마지막에 있어야 한다
printf '| [%s](%s) | %s | 제안 |\n' "$num" "$file" "$title" >> "$dir/README.md"

echo "생성: docs/adr/$file"
echo "목록 추가: docs/adr/README.md"
