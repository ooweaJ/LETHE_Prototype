#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
node src/run_alpha.js --runs 1000 --stages 2 --seed 20260601 --echo 0.60 --ui 0.62 --out outputs/default
