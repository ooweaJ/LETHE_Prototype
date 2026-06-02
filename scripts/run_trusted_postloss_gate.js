#!/usr/bin/env node
'use strict';

const fs = require('fs');
const path = require('path');
const { spawnSync } = require('child_process');

const BLOCKER_PROMPT = 'docs/review_prompts/2026-06-02-postloss-browser-transport-blocker.md';
const RESULT_PATH = 'alpha_test/outputs/postloss-trusted-gate/latest.json';

main();

function main() {
  console.log('LETHE trusted-local post-loss QA gate');
  console.log('');
  console.log('Step 1/2: npm run qa:postloss');
  const first = runPostLossQa([]);
  if (first.status === 0) {
    writeGateResult({
      status: 'complete',
      passedStep: 'standard',
      transportFailure: false,
      first,
      nextCommand: '',
      blockerPrompt: '',
    });
    console.log('');
    console.log('Post-loss browser QA passed. WP2 Slice B can be treated as browser-proven.');
    return;
  }

  if (!isTransportFailure(first.output)) {
    writeGateResult({
      status: 'failed',
      failedStep: 'standard',
      transportFailure: false,
      first,
      nextCommand: 'Fix only the post-loss QA/flow assertion.',
      blockerPrompt: '',
    });
    printGameplayFailureGuidance(first);
    process.exit(first.status || 1);
  }

  console.log('');
  console.log('Transport failure detected. Retrying once with 30000 ms timeout.');
  console.log('');
  console.log('Step 2/2: npm run qa:postloss -- --timeout-ms 30000');
  const second = runPostLossQa(['--timeout-ms', '30000']);
  if (second.status === 0) {
    writeGateResult({
      status: 'complete',
      passedStep: 'timeout-retry',
      transportFailure: false,
      first,
      second,
      nextCommand: '',
      blockerPrompt: '',
    });
    console.log('');
    console.log('Post-loss browser QA passed after the longer timeout. WP2 Slice B can be treated as browser-proven.');
    return;
  }

  if (isTransportFailure(second.output)) {
    writeGateResult({
      status: 'blocked',
      failedStep: 'timeout-retry',
      transportFailure: true,
      first,
      second,
      nextCommand: `Use ${BLOCKER_PROMPT} before starting WP3, people testing, or new gameplay scope.`,
      blockerPrompt: BLOCKER_PROMPT,
    });
    console.error('');
    console.error('Post-loss browser QA is still blocked by Chrome/CDP transport.');
    console.error(`Use ${BLOCKER_PROMPT} before starting WP3, people testing, or new gameplay scope.`);
  } else {
    writeGateResult({
      status: 'failed',
      failedStep: 'timeout-retry',
      transportFailure: false,
      first,
      second,
      nextCommand: 'Fix only the post-loss QA/flow assertion.',
      blockerPrompt: '',
    });
    printGameplayFailureGuidance(second);
  }
  process.exit(second.status || 1);
}

function runPostLossQa(extraArgs) {
  const result = spawnSync(process.execPath, [
    'scripts/run_browser_pressure_qa.js',
    '--mode',
    'postloss',
    ...extraArgs,
  ], {
    cwd: process.cwd(),
    encoding: 'utf8',
    maxBuffer: 1024 * 1024 * 8,
  });

  if (result.stdout) process.stdout.write(result.stdout);
  if (result.stderr) process.stderr.write(result.stderr);
  if (result.error) console.error(result.error.message);

  return {
    status: typeof result.status === 'number' ? result.status : 1,
    output: [result.stdout, result.stderr, result.error?.message].filter(Boolean).join('\n'),
  };
}

function writeGateResult(result) {
  const payload = {
    generatedAt: new Date().toISOString(),
    gate: 'v0.9-postloss-trusted',
    resultPath: RESULT_PATH,
    ...result,
    first: summarizeRun(result.first),
    second: summarizeRun(result.second),
  };
  fs.mkdirSync(path.dirname(RESULT_PATH), { recursive: true });
  fs.writeFileSync(RESULT_PATH, `${JSON.stringify(payload, null, 2)}\n`, 'utf8');
  console.log(`Trusted post-loss gate result written: ${RESULT_PATH}`);
}

function summarizeRun(run) {
  if (!run) return null;
  return {
    status: run.status,
    transportFailure: isTransportFailure(run.output),
    outputTail: tailLines(run.output, 18),
  };
}

function tailLines(text, limit) {
  return String(text || '')
    .trim()
    .split('\n')
    .slice(-limit)
    .join('\n');
}

function isTransportFailure(output) {
  return [
    'BrowserQaTransportError',
    'Target.getTargets',
    'Chrome page target',
    'Chrome port page target',
    'remote-debugging-port',
    'listen EPERM',
  ].some((token) => output.includes(token));
}

function printGameplayFailureGuidance(result) {
  console.error('');
  console.error('Post-loss QA reached a non-transport failure.');
  console.error('Fix only the post-loss QA/flow assertion before WP3 or people testing.');
  if (!result.output.trim()) console.error('No command output was captured.');
}
