# Project-Specific Rules - Darkseed Platformer

## Overview
This document contains project-specific rules for the Darkseed platformer game built with GDevelop. These rules supplement the general guidelines and ensure consistency across development.

---

## Game Genre & Core Mechanics

### Platformer Requirements
- 2D side-scrolling gameplay with precise platforming
- Physics-based movement (jump, fall, run)
- Collision detection with platforms, enemies, and interactive objects
- Screen scrolling when player approaches screen edges

### Combat System
- Weapon-based combat using revolver, shotgun, rifle, and raygun
- Ammunition management system
- Reload mechanics
- Shooting animations synchronized with weapon state

---

## Player Character

### Movement States
- **0**: Idle (standing still)
- **1-3**: Walking/Running variations
- **4-5**: Jumping (upward motion)
- **6-7**: Falling (downward motion)
- **8**: Shooting (firing weapon)
- **9-10**: Special actions
- **11**: Reloading weapon
- **12**: Running and shooting

### Player Properties
- Health: Current health points
- MaxHealth: Maximum health capacity
- Armor: Damage reduction (if implemented)
- Speed: Movement velocity (varies by state)

### Animation States
Each movement state has associated animations:
- Idle animation when state = 0
- Run/Walk animations for states 1-3
- Jump/Fall animations for states 4-7
- Shooting animation for state 8
- Reload animation for state 11

---

## Weapon System

### Available Weapons
1. **Revolver**
   - Bullet type: `BulletRevolver`
   - Mag size: 6 rounds
   - Fire rate: Moderate
   - Damage: Medium
   - Reload time: Short

2. **Shotgun**
   - Bullet types: `BulletShotgunA`, `BulletShotgunB`, `BulletShotgunC`
   - Mag size: 2-4 shells (varies)
   - Fire rate: Slow (semi-auto only)
   - Damage: High (close range)
   - Reload time: Long

3. **Rifle**
   - Bullet type: `BulletRifle`
   - Mag size: Large (e.g., 30 rounds)
   - Fire rate: Fast
   - Damage: Low-Medium
   - Reload time: Medium

4. **Raygun**
   - Bullet type: `BulletRay`
   - Mag size: Variable (energy-based)
   - Fire rate: Variable
   - Damage: High (penetrates armor)
   - Special effects: Area damage, stun

### Weapon Variables
- `CurrentWeaponId`: 0=Revolver, 1=Shotgun, 2=Rifle, 3=Raygun
- `WeaponMag`: Current ammunition count for selected weapon
- `GunBulletCounter`: Tracks shots fired (used for reload trigger)
- `TextForGunAmmunition`: Display text showing current ammo

### Weapon Switching Rules
1. Player must be idle or running (not shooting/reloading)
2. Cycle through weapons using dedicated keys
3. Update `CurrentWeaponId` variable
4. Set `WeaponMag` to full for new weapon (or last remaining amount)

---

## Bullet/Projectile System

### Common Properties
All bullets share:
- Speed: Projectile velocity
- Damage: Points deducted from enemy health
- Lifetime: Seconds before auto-destruction
- Collision type: Small hitbox

### Weapon-Specific Bullets
1. **BulletRevolver**: Single projectile, moderate speed, medium damage
2. **BulletShotgunA/B/C**: Spread pattern (3-5 projectiles), short range, high damage
3. **BulletRifle**: Fast projectile, low damage, long range
4. **BulletRay**: Energy beam effect, high damage, penetrates enemies

### Bullet Management
- Destroy bullets when they hit solid objects or after lifetime expires
- Use collision events to detect enemy hits
- Apply damage and create hit effects on impact

---

## Enemy System

### Enemy Types
1. **Basic Soldier**
   - Health: Low-Medium
   - Movement: Simple patrol pattern
   - Attack: Basic projectile
   - AI: Simple chase when player detected

2. **Heavy Unit**
   - Health: High
   - Movement: Slow but resilient
   - Attack: Heavy damage projectiles
   - Armor: Reduces incoming damage

3. **Fast Unit**
   - Health: Low
   - Movement: Very fast, agile
   - Attack: Melee or rapid-fire projectile
   - Behavior: Flanking patterns

4. **Boss Enemy**
   - Multiple phases with different behaviors
   - High health pool
   - Special attacks and abilities
   - Weak points that can be exploited

### Enemy States
- **0**: Idle/Patrol
- **1**: Chase player
- **2**: Attack
- **3**: Damage/recover
- **4**: Death animation

---

## Level Design Considerations

### Platform Types
1. **Solid Platforms**: Player can stand on top
2. **One-Way Platforms**: Player falls through but cannot jump up
3. **Breakable Platforms**: Break when player lands on them
4. **Moving Platforms**: Move automatically, carry player with them

### Interactive Objects
- **Health Packs**: Restore player health
- **Ammo Crates**: Refill weapon magazine
- **Weapon Upgrades**: Switch to better weapons
- **Switches/Buttons**: Trigger level events
- **Hazards**: Damage player on contact (spikes, lava)

### Level Transitions
- Screen scrolling triggers when player approaches edges
- Optional: Loading screens for major transitions
- Save player position at transition points

---

## UI/HUD System

### HUD Elements
1. **Health Bar**
   - Visual indicator of current/max health
   - Color changes based on health percentage
   - Flash red when damaged

2. **Ammunition Display**
   - Current weapon and ammo count
   - Visual indicator for low ammo (flash color change)
   - Reload indicator when reloading

3. **Weapon Selector**
   - Icon of currently selected weapon
   - Weapon name display
   - Weapon switch preview icons

4. **Score/Points**
   - Total score accumulated
   - Bonus indicators

5. **Mini-Map** (if implemented)
   - Current area overview
   - Player position indicator
   - Objective markers

---

## Game States

### Main Game States
1. **Menu State**
   - Title screen
   - Options menu
   - Start game option

2. **Playing State**
   - Active gameplay
   - HUD visible
   - Enemy spawning active

3. **Paused State**
   - Pause menu
   - Resume/Restart/Exit options
   - All gameplay暂停

4. **Game Over State**
   - Score display
   - Restart option
   - Return to title

5. **Victory State** (if applicable)
   - Victory screen
   - Final score
   - Level completion message

---

## Variable Naming Conventions

### Global Variables
- Use PascalCase for consistency: `CurrentWeaponId`, `TotalScore`
- Prefix with category when appropriate: `PlayerHealth`, `EnemyCount`

### Object Variables
- Keep consistent across object types: `Health`, `MaxHealth`, `State`
- Use descriptive names for weapon-specific variables

### UI Text Variables
- `TextForGunAmmunition`: Current ammo display text
- `TextForWeaponName`: Weapon name display
- `TextForScore`: Score display text
- `TextForHealth`: Health percentage text

---

## Performance Guidelines

### Object Management
- Limit number of simultaneous bullets (e.g., max 20 projectiles)
- Destroy objects when out of screen or no longer needed
- Use object pooling for frequently created/destroyed objects if performance issues arise

### Collision Optimization
- Use simple collision boxes instead of pixel-perfect when possible
- Avoid complex collision shapes on moving objects
- Group similar collisions to reduce event checks

### Animation Optimization
- Use sprite atlases when possible
- Limit animation frame rate for non-critical animations
- Unload unused sprites from memory

---

## File Structure & Organization

### Project Organization
```
DarkseedProject/
├── Scenes/
│   ├── Menu.gds
│   ├── Level1.gds
│   └── GameOver.gds
├── Objects/
│   ├── Player.gdo
│   ├── Enemy*.gdo
│   └── Bullet*.gdo
├── Events/
│   ├── Player.events
│   ├── Combat.events
│   └── Level.events
└── Assets/
    ├── Sprites/
    ├── Sounds/
    └── Music/
```

### Event Sheet Organization
- Separate event sheets by functionality: `Player.events`, `Combat.events`, `Level.events`
- Group related events using collapsible groups
- Use comments to describe complex logic blocks

---

## Testing Guidelines

### Combat Testing Checklist
- [ ] All weapons fire correctly with proper ammo consumption
- [ ] Reload mechanics work for all weapons
- [ ] Weapon switching functions properly
- [ ] Bullets deal correct damage values
- [ ] Hit detection works consistently

### Platforming Testing Checklist
- [ ] Jump physics feel responsive and consistent
- [ ] Player cannot clip through platforms
- [ ] Moving platforms carry player correctly
- [ ] One-way platforms work as expected

### Performance Testing Checklist
- [ ] Maintain 60 FPS during normal gameplay
- [ ] No memory leaks with extended play sessions
- [ ] Bullet limit prevents performance degradation
- [ ] Screen scrolling is smooth and responsive

---

## Known Limitations & Workarounds

### GDevelop Limitations
1. **No Native Object Pooling**
   - Workaround: Reuse bullet objects by resetting their properties instead of creating/destroying
   
2. **Limited Advanced AI**
   - Workaround: Use variable-based state machines for enemy behavior
   
3. **No Physics Joints**
   - Workaround: Simulate joints using collision detection and movement logic

### Performance Considerations
- Limit concurrent enemies on screen
- Avoid too many particle effects simultaneously
- Optimize background layers (use parallax sparingly)

---

## Future Expansion Possibilities

### Planned Features
1. **Multiplayer Support**
   - Network synchronization for player actions
   - Server-client architecture considerations
   
2. **Power-Up System**
   - Temporary ability boosts
   - Collectible items with duration-based effects
   
3. **Mission Objectives**
   - Secondary objectives for bonus points
   - Time-based challenges

4. **Difficulty Progression**
   - Enemy stats scaling based on progress
   - Dynamic difficulty adjustment

---

## Contact & Support

For questions about project-specific rules:
1. Check this documentation first
2. Review existing event sheets and object definitions
3. Test in GDevelop debugger to verify behavior