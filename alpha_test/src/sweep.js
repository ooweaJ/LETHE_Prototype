#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { simulateBatch } = require('./simulator');
const { summarize } = require('./metrics');

function parseArgs(argv) {
  const args = {};
  for (let i = 2; i < argv.length; i += 1) {
    const item = argv[i];
    if (!item.startsWith('--')) continue;
    const key = item.slice(2);
    const next = argv[i + 1];
    if (!next || next.startsWith('--')) args[key] = true;
    else { args[key] = next; i += 1; }
  }
  return args;
}

function toNum(v, fallback) {
  const n = Number(v);
  return Number.isFinite(n) ? n : fallback;
}

function main() {
  const a = parseArgs(process.argv);
  const outDir = path.resolve(a.out || 'outputs/sweep');
  fs.mkdirSync(outDir, { recursive: true });
  const echoes = String(a.echo || '0.40,0.50,0.60,0.70,0.80').split(',').map(Number).filter(Number.isFinite);
  const uiValues = String(a.ui || '0.45,0.62,0.78').split(',').map(Number).filter(Number.isFinite);
  const rows = [];
  for (const echoPower of echoes) {
    for (const uiClarity of uiValues) {
      const options = {
        runs: toNum(a.runs, 300),
        stages: toNum(a.stages, 2),
        seed: `${a.seed || 'sweep'}:${echoPower}:${uiClarity}`,
        echoPower,
        uiClarity,
        replacementOffer: true,
        enableHumanEmotionProxy: true,
      };
      const summary = summarize(simulateBatch(options));
      rows.push({
        echoPower,
        uiClarity,
        verdict: summary.gate.verdict,
        alphaFunScore: summary.gate.alphaFunScore,
        regretRate: summary.headlineMetrics.regretRate,
        irritationRate: summary.headlineMetrics.irritationRate,
        predictionMatchRate: summary.headlineMetrics.predictionMatchRate,
        immediateQuitRate: summary.headlineMetrics.immediateQuitRate,
        avgPowerDrop: summary.headlineMetrics.avgPowerDrop,
        avgRecovery: summary.headlineMetrics.avgRecovery,
      });
      console.log(`echo=${echoPower} ui=${uiClarity} => ${summary.gate.verdict}, score=${summary.gate.alphaFunScore}`);
    }
  }
  fs.writeFileSync(path.join(outDir, 'sweep_results.json'), JSON.stringify(rows, null, 2), 'utf8');
  fs.writeFileSync(path.join(outDir, 'sweep_results.csv'), toCsv(rows), 'utf8');
}

function toCsv(rows) {
  if (!rows.length) return '';
  const headers = Object.keys(rows[0]);
  return `${headers.join(',')}\n${rows.map((r) => headers.map((h) => r[h]).join(',')).join('\n')}\n`;
}

if (require.main === module) main();
