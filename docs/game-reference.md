# Sentaur Survivors Game Reference

A complete reference for all weapons, enemies, and items in Sentaur Survivors.

## Weapons

The player starts with the Dart and unlocks additional weapons through the level-up upgrade system. Each weapon has 3 upgrade levels. All weapons are affected by global upgrade modifiers (damage, cooldown, projectile count).

### Dart

A directional projectile weapon that fires toward the player's aim.

| Stat | Value |
|---|---|
| Base Damage | 10 |
| Base Cooldown | 1.8s |
| Speed | 10 |
| Splash Radius | 0.25 |
| Knockback | 5000 |

**Upgrades:**
1. Activates the weapon — fires in a straight line
2. Now also fires backwards (+1 rear dart)
3. +50% damage

### Starfish

Spawns orbiting projectiles that rotate around the player. Starfish persist through enemy hits for their full duration.

| Stat | Value |
|---|---|
| Base Damage | 8 |
| Base Cooldown | 5s |
| Orbit Duration | 5s |
| Orbit Radius | 2 |
| Rotation Speed | 180 deg/s |

**Upgrades:**
1. Activates the weapon — orbits around you, wreaking havoc
2. +40% orbit duration
3. +30% orbit duration and -30% cooldown

### Schnitzel

An axe-like arcing projectile affected by gravity. Spins through the air and passes through multiple enemies (each enemy can only be hit once per projectile).

| Stat | Value |
|---|---|
| Base Damage | 8 |
| Base Cooldown | 2.9s |
| Speed | 5 |
| Knockback | 1000 |
| Max Lifetime | 10s |

**Upgrades:**
1. Activates the weapon — it's like an axe
2. +40% area of effect (scale)
3. +30% area of effect (scale)

### Raven

A heat-seeking weapon that automatically targets the closest enemies. Deals splash damage on impact.

| Stat | Value |
|---|---|
| Base Damage | 15 |
| Base Cooldown | 6s |
| Speed | 12 |
| Splash Radius | 1.0 |
| Detection Range | 12 |

**Upgrades:**
1. Activates the weapon — heat-seeking bomb targets closest enemy
2. +33% damage and -20% cooldown
3. +60% area of effect radius

### Global Upgrades

These upgrades affect all weapons simultaneously.

**Damage:**
1. +30% damage (x1.3)
2. +25% damage (x1.25)
3. +25% damage (x1.25)

**Cooldown:**
1. -20% cooldown (x0.8)
2. -25% cooldown (x0.75)
3. -50% cooldown (x0.5)

**Projectile Count:**
1. +1 projectile (2 total)
2. +1 projectile (3 total)
3. +2 projectiles (5 total)

## Enemies

Enemies spawn throughout the game with new types unlocking at higher levels. Every 60 seconds, newly spawned enemies gain +10 HP (cumulative).

| Enemy | HP | Damage | Speed | Score | XP | Unlock | Behavior |
|---|---|---|---|---|---|---|---|
| Sentaur | 10 | 10 | 1.5 | 10 | 10 | Level 1 | Chases player directly |
| Bug (Ant) | 25 | 12 | 1.0 | 25 | 18 | Level 3 | Chases player directly |
| Random Head | 30 | 20 | 3.0 | 50 | 25 | Level 4 | Wanders to random positions |
| Diagonal Head | 30 | 20 | 3.0 | 50 | 25 | Level 5 | Moves diagonally, bounces off barriers |
| Mantis | 50 | 15 | 1.75 | 75 | 30 | Level 7 | Chases player directly |
| Linear Head | 25 | 20 | 3.0 | 40 | 20 | Level 8 | Moves in one cardinal direction (spawns in waves) |
| Death | 99999 | 9999 | 5.0 | 0 | 0 | 10 min | Unkillable end-game threat, relentlessly chases player |

### Spawning Rules

- **Base spawn rate:** Every 2.0s, decreasing by 0.05s every 10s (minimum 0.5s)
- **Linear Head waves:** Every 30s (decreasing by 1.5s every 10s, minimum 8s); requires level >= 3
- **Wave size:** Scales with level; Diagonal Heads spawn at half the normal wave rate
- **HP scaling:** +10 HP to all newly spawned enemies every 60s
- **Death enemy:** Spawns once at 10 minutes as a game-ending timer

## Items

### Field Pickups

Pickups spawn every 15 seconds with a maximum of 7 on screen at once. A random pickup is selected from the pool each time.

| Pickup | Effect | Duration | Score | Display Text |
|---|---|---|---|---|
| Beerboot | Halves weapon cooldown (2x fire rate) | 5s | 50 | +0.5x speed! |
| Skateboard | Doubles movement speed | 10s | 50 | +2x speed! |
| Hotdog | Heals 30 HP (max 100) | Instant | 50 | +30 HP! |
| Not Hotdog | Deals 25 damage to player (trap!) | Instant | 50,000 | That was not a hotdog! |
| Mozart | Attracts all XP drops to player | Instant | 25 | — |
| Money | Score bonus only | Instant | 5,000 | +5000 points! |
| Umbrella | 50% damage resistance | 10s | 150 | +50% DMG resist! |

### XP Drops

Dropped by enemies on death. Each drop is worth 10 XP by default. Automatically moves toward the player when within 3 units. The Mozart pickup attracts all XP drops on screen.
