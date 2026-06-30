const fs = require('fs');
const path = require('path');

const root = path.resolve(__dirname, '..');
const managerPath = path.join(root, 'LETHE/Assets/_dev/Scripts/PrototypeV1/V1GameManager.cs');
const text = fs.readFileSync(managerPath, 'utf8');

const checks = [
  ['first boss constant', /const float FirstBossSeconds = 150f;/],
  ['run hard cap', /const float RunSeconds = 1080f;/],
  ['normal deficit removed', /const float DeficitSurvivalSeconds = 0f;/],
  ['boss schedule', /BossScheduleSeconds = \{ 150f, 300f, 540f, 900f \}/],
  ['first boss hp', /const float FirstBossHp = 2200f;/],
  ['second boss hp', /1 => 4200f,/],
  ['third boss hp', /2 => 7600f,/],
  ['fourth boss hp', /_ => 12800f/],
  ['initial xp', /int nextXp = 8;/],
  ['early xp curve', /nextXp \* 1\.32f \+ 5f/],
  ['mid xp curve', /nextXp \* 1\.20f \+ 4f/],
  ['late xp curve', /nextXp \* 1\.18f \+ 5f/],
  ['forget returns to combat', /void ContinueAfterForgetResult\(\)[\s\S]*refillOverlay = false;[\s\S]*refillTimer = 0f;[\s\S]*망각 완료/],
  ['reacquire removed', text => !/void ReacquireLastForgotten\(\)/.test(text)],
  ['gatekeeper pattern pulse', /GatekeeperPatternPulse\(V1Enemy gatekeeper, int patternStep\)/],
  ['gatekeeper guard mitigation', /GatekeeperGuardActive[\s\S]*finalAmount \*= weaponHit \? 0\.55f : 0\.72f;/],
  ['kalmuri visual during hitstop', /UpdateHungryBladesVisualDuringHitstop\(dt\);/],
  ['health bar inverse scale', /healthRoot\.localScale = new Vector3\(sx, sy, 1f\);/],
  ['stepped spawn profile', /SpawnWaveProfile SteppedSpawnProfile\(\)/],
  ['stepped enemy cap', /int SteppedEnemyCap\(\)/],
  ['stepped enemy hp', /float SteppedEnemyHp\(V1EnemyKind kind\)/],
];

const failures = checks.filter(([, pattern]) => {
  if (typeof pattern === 'function') return !pattern(text);
  return !pattern.test(text);
});

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
