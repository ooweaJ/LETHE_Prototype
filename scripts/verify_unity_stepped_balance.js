const fs = require('fs');
const path = require('path');

const root = path.resolve(__dirname, '..');
const managerPath = path.join(root, 'LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs');
const text = fs.readFileSync(managerPath, 'utf8');

const checks = [
  ['first boss constant', /const float FirstBossSeconds = 150f;/],
  ['run hard cap', /const float RunSeconds = 1200f;/],
  ['normal deficit removed', /const float DeficitSurvivalSeconds = 0f;/],
  ['boss schedule', /BossScheduleSeconds = \{ 150f, 360f, 660f, 1020f \}/],
  ['first boss hp', /const float FirstBossHp = 1200f;/],
  ['second boss hp', /1 => 2250f,/],
  ['third boss hp', /2 => 4050f,/],
  ['fourth boss hp', /_ => 8650f/],
  ['initial xp', /int nextXp = 8;/],
  ['early xp curve', /nextXp \* 1\.32f \+ 5f/],
  ['mid xp curve', /nextXp \* 1\.20f \+ 4f/],
  ['late xp curve', /nextXp \* 1\.18f \+ 5f/],
  ['normal immediate refill', /if \(!fastDebugRun\)[\s\S]*refillOverlay = true;[\s\S]*return;/],
  ['normal pressure ignores deficit', /if \(fastDebugRun && \(refillTimer > 0f/],
  ['stepped spawn profile', /SpawnWaveProfile SteppedSpawnProfile\(\)/],
  ['stepped enemy cap', /int SteppedEnemyCap\(\)/],
  ['stepped enemy hp', /float SteppedEnemyHp\(V1EnemyKind kind\)/],
];

const failures = checks.filter(([, pattern]) => !pattern.test(text));

if (failures.length > 0) {
  console.error('Stepped balance verification failed:');
  for (const [name] of failures) {
    console.error(`- ${name}`);
  }
  process.exit(1);
}

console.log('Stepped balance verification passed.');
for (const [name] of checks) {
  console.log(`- ${name}`);
}
