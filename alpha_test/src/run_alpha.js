#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { simulateBatch } = require('./simulator');
const { summarize, runsToCsv } = require('./metrics');
const { writeReports } = require('./report');

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
  const outDir = path.resolve(a.out || 'outputs');
  const options = {
    runs: toNum(a.runs, 1000),
    stages: toNum(a.stages, 2),
    seed: a.seed || '20260601',
    echoPower: toNum(a.echo, 0.50),
    uiClarity: toNum(a.ui, 0.62),
    replacementOffer: a.replacement === 'false' ? false : true,
    enableHumanEmotionProxy: a.emotion === 'false' ? false : true,
    botIds: a.bots ? String(a.bots).split(',').map((s) => s.trim()).filter(Boolean) : undefined,
  };

  console.log(`[LETHE Alpha] runs=${options.runs}, stages=${options.stages}, seed=${options.seed}, echo=${options.echoPower}, ui=${options.uiClarity}`);
  const batch = simulateBatch(options);
  const summary = summarize(batch);
  fs.mkdirSync(outDir, { recursive: true });
  fs.writeFileSync(path.join(outDir, 'raw_runs.json'), JSON.stringify(batch.runs, null, 2), 'utf8');
  fs.writeFileSync(path.join(outDir, 'summary.json'), JSON.stringify(summary, null, 2), 'utf8');
  fs.writeFileSync(path.join(outDir, 'runs.csv'), runsToCsv(batch.runs), 'utf8');
  writeReports(outDir, summary);

  console.log(`\nVerdict: ${summary.gate.verdict}`);
  console.log(`Playability: ${summary.playability.label} (${summary.playability.riskLevel})`);
  console.log(`Alpha Fun Score: ${summary.gate.alphaFunScore}`);
  console.log(`Regret: ${(summary.headlineMetrics.regretRate * 100).toFixed(1)}% / Irritation: ${(summary.headlineMetrics.irritationRate * 100).toFixed(1)}% / Prediction: ${(summary.headlineMetrics.predictionMatchRate * 100).toFixed(1)}%`);
  console.log(`Next: ${summary.playability.nextStep}`);
  console.log(`Outputs: ${outDir}`);
}

if (require.main === module) main();
