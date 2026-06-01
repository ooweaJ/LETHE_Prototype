'use strict';

function hashSeed(seed) {
  const s = String(seed ?? 'lethe');
  let h = 2166136261 >>> 0;
  for (let i = 0; i < s.length; i += 1) {
    h ^= s.charCodeAt(i);
    h = Math.imul(h, 16777619);
  }
  return h >>> 0;
}

class RNG {
  constructor(seed = 'lethe-alpha') {
    this.state = hashSeed(seed) || 0x12345678;
  }

  next() {
    // Mulberry32-ish deterministic generator.
    this.state = (this.state + 0x6D2B79F5) >>> 0;
    let t = this.state;
    t = Math.imul(t ^ (t >>> 15), t | 1);
    t ^= t + Math.imul(t ^ (t >>> 7), t | 61);
    return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
  }

  float(min = 0, max = 1) {
    return min + (max - min) * this.next();
  }

  int(min, maxInclusive) {
    return Math.floor(this.float(min, maxInclusive + 1));
  }

  bool(p = 0.5) {
    return this.next() < p;
  }

  choice(arr) {
    if (!arr.length) return undefined;
    return arr[Math.floor(this.next() * arr.length)];
  }

  shuffle(arr) {
    const out = arr.slice();
    for (let i = out.length - 1; i > 0; i -= 1) {
      const j = Math.floor(this.next() * (i + 1));
      [out[i], out[j]] = [out[j], out[i]];
    }
    return out;
  }

  weighted(items, weightFn) {
    const weights = items.map((item) => Math.max(0, Number(weightFn(item)) || 0));
    const total = weights.reduce((a, b) => a + b, 0);
    if (total <= 0) return this.choice(items);
    let roll = this.float(0, total);
    for (let i = 0; i < items.length; i += 1) {
      roll -= weights[i];
      if (roll <= 0) return items[i];
    }
    return items[items.length - 1];
  }

  noise(scale = 1) {
    return (this.next() + this.next() + this.next() - 1.5) * scale;
  }
}

module.exports = { RNG, hashSeed };
