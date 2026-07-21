# Setup Guide - Unity 2D Run-and-Gun Platformer

## Struttura del Progetto

```
Assets/
├── Scripts/
│   ├── GameManager.cs              # Gestore principale del gioco
│   ├── Player/
│   │   ├── PlayerController.cs     # Controller giocatore (movimento, salto)
│   │   ├── PlayerHealth.cs         # Sistema salute giocatore
│   │   └── PlayerShooter.cs        # Sistema di sparatoria giocatore
│   ├── Enemies/
│   │   ├── EnemyController.cs      # Classe base per nemici
│   │   ├── GenericEnemy.cs         # Nemico generico con IA
│   │   └── BossEnemy.cs            # Boss con fasi e attacchi speciali
│   ├── Projectiles/
│   │   └── Bullet.cs               # Proiettili (giocatore e nemici)
│   ├── UI/
│   │   └── HUDManager.cs           # Interfaccia utente
│   └── Collectibles/
│       └── Collectible.cs          # Oggetti raccoglibili
```

## Configurazione Scene Unity

### 1. Creare Tag e Layer

**Tag da creare:**
- `Player` - Per il personaggio principale
- `Enemy` - Per tutti i nemici
- `Boss` - Per il boss (opzionale, separato da Enemy)
- `Collectible` - Per oggetti raccoglibili
- `Hazard` - Per trappole e pericoli
- `Ground` - Per piattaforme e terreno
- `Wall` - Per muri

**Layer da creare:**
- `Ground` - Per piattaforme e terreno
- `Aim` - Per il sistema di mira

### 2. Setup Player GameObject

Crea un GameObject vuoto chiamato "Player" con questi componenti:

```
Player (GameObject)
├── SpriteRenderer (con sprite del personaggio)
├── Rigidbody2D
│   ├── Body Type: Dynamic
│   ├── Gravity Scale: 3
│   └── Collision Detection: Continuous
├── BoxCollider2D
│   ├── Size: (0.8, 1.5)
│   └── Offset: (0, -0.2)
├── PlayerController.cs
│   ├── Move Speed: 8
│   ├── Jump Force: 15
│   ├── Double Jump Force: 12
│   ├── Ground Check: [Riferimento al figlio "GroundCheck"]
│   ├── Ground Check Radius: 0.2
│   └── Ground Layer: Ground
├── PlayerHealth.cs
│   └── Max Health: 3
├── PlayerShooter.cs
│   ├── Bullet Prefab: [Riferimento al prefab del proiettile]
│   ├── Fire Point: [Riferimento al figlio "FirePoint"]
│   ├── Bullet Speed: 15
│   └── Fire Rate: 0.2
├── GroundCheck (GameObject vuoto)
│   └── Posizione: (0, -0.7)
└── FirePoint (GameObject vuoto)
    └── Posizione: (0.3, 0)
```

### 3. Setup Proiettili

Crea un prefab chiamato "Bullet" con:

```
Bullet (Prefab)
├── SpriteRenderer (con sprite del proiettile)
├── Rigidbody2D
│   ├── Body Type: Kinematic
│   └── Gravity Scale: 0
├── CircleCollider2D
│   ├── Radius: 0.15
│   └── Is Trigger: true
└── Bullet.cs
    └── Lifetime: 3
```

### 4. Setup Nemici Generici

Crea un GameObject "GenericEnemy" con:

```
GenericEnemy (GameObject)
├── SpriteRenderer (con sprite del nemico)
├── Rigidbody2D
│   ├── Body Type: Dynamic
│   └── Gravity Scale: 3
├── BoxCollider2D
│   ├── Size: (0.8, 1.5)
│   └── Offset: (0, -0.2)
├── EnemyHealth.cs
│   └── Max Health: 3
├── GenericEnemy.cs
│   ├── Detection Range: 5
│   ├── Patrol Speed: 2
│   ├── Chase Speed: 4
│   ├── Attack Range: 3
│   ├── Fire Rate: 1.5
│   ├── Enemy Bullet Prefab: [Riferimento al prefab nemico]
│   └── Fire Point: [Riferimento al figlio "FirePoint"]
├── PatrolPoint1 (GameObject vuoto)
├── PatrolPoint2 (GameObject vuoto)
├── PatrolPoint3 (GameObject vuoto)
└── FirePoint (GameObject vuoto)
    └── Posizione: (0.4, 0)
```

### 5. Setup Boss

Crea un GameObject "BossEnemy" con:

```
BossEnemy (GameObject)
├── SpriteRenderer (con sprite del boss - più grande!)
├── Rigidbody2D
│   ├── Body Type: Dynamic
│   └── Gravity Scale: 3
├── BoxCollider2D
│   ├── Size: (1.5, 2.5)
│   └── Offset: (0, -0.3)
├── EnemyHealth.cs
│   └── Max Health: 15
├── BossEnemy.cs
│   ├── Max Phase: 3
│   ├── Phase Transition Health %: 0.33
│   ├── Special Attack Cooldown: 10
│   ├── Explosion Prefab: [Riferimento al prefab esplosione]
│   └── Fire Point: [Riferimento al figlio "FirePoint"]
├── SpawnPoint1 (GameObject vuoto)
├── SpawnPoint2 (GameObject vuoto)
└── FirePoint (GameObject vuoto)
    └── Posizione: (0.8, 0)
```

### 6. Setup GameManager

Crea un GameObject vuoto "GameManager" con:

```
GameManager (GameObject)
└── GameManager.cs
    ├── Max Lives: 3
    ├── Score to Win: 1000
    ├── Player Prefab: [Riferimento al prefab Player]
    └── Spawn Point: [Riferimento al figlio "SpawnPoint"]
```

### 7. Setup HUD

Crea un Canvas con UI elements:

```
Canvas (Event System)
├── ScoreText (Text)
│   └── Posizione: in alto a sinistra
├── LivesText (Text)
│   └── Posizione: in alto a destra
└── HealthBars (Image array)
    └── Posizione: in basso a sinistra
```

Aggiungi il componente `HUDManager.cs` al Canvas.

### 8. Setup Collezionabili

Crea un prefab "Collectible" con:

```
Collectible (Prefab)
├── SpriteRenderer (con sprite della moneta/oggetto)
├── CircleCollider2D
│   ├── Radius: 0.3
│   └── Is Trigger: true
└── Collectible.cs
    └── Score Value: 100
```

## Input Configuration

In **Edit > Project Settings > Input Manager**:

Verifica che questi input siano configurati:
- `Horizontal` - A/D o frecce sinistra/destra
- `Vertical` - W/S o frecce su/giù
- `Jump` - Spazio o W
- `Fire1` - Mouse sinistro o J
- `Fire2` - Mouse destro o K (opzionale)

## Animazioni Consigliate

Per ogni personaggio, crea queste animazioni:

**Player:**
- Idle (fermo)
- Run (corsa)
- Jump (salto)
- Fall (caduta)
- Shoot (sparatoria)
- Hit (colpito)
- Death (morte)

**GenericEnemy:**
- Idle (fermo)
- Walk (cammina)
- Chase (inseguimento)
- Attack (attacco)
- Hit (colpito)
- Death (morte)

**BossEnemy:**
- Idle (fermo)
- Patrol (patrolla)
- Chase (insegue)
- Attack (attacca)
- SpecialAttack (attacco speciale)
- PhaseTransition (transizione fase)
- Hit (colpito)
- Death (morte)

## Particle Effects Consigliati

Crea questi particle systems:

1. **BulletImpact** - Quando i proiettili colpiscono qualcosa
2. **EnemyDeath** - Esplosione quando un nemico muore
3. **BossExplosion** - Grande esplosione per il boss
4. **PlayerHit** - Effetto quando il giocatore viene colpito
5. **CollectiblePickup** - Effetto quando raccogli un oggetto

## Ottimizzazioni

1. **Object Pooling**: Per proiettili e nemici, usa object pooling invece di Instantiate/Destroy
2. **Layer Mask**: Usa layer mask per ottimizzare i raycast e le collisioni
3. **Animation Culling**: Disabilita animator per oggetti fuori schermo
4. **Physics Layers**: Configura la matrice di collisione per evitare calcoli inutili

## Prossimi Passi

1. Importa gli sprite nel progetto Unity
2. Crea i prefab seguendo questa guida
3. Configura le animazioni
4. Aggiungi suoni ed effetti
5. Crea una scena di test con piattaforme e nemici
6. Testa e bilancia i valori di gioco

## Note Importanti

- Tutti gli script usano `DontDestroyOnLoad` per il GameManager
- Il sistema di invincibilità previene danni continui
- I nemici hanno IA base con patrol, chase e attack states
- Il boss ha 3 fasi con comportamenti diversi
- Il punteggio può essere esteso per vincere il gioco

## Troubleshooting

**Problema: Il giocatore non salta**
- Verifica che Ground Check sia posizionato correttamente
- Controlla che il layer "Ground" sia assegnato nel PlayerController

**Problema: I nemici non si muovono**
- Verifica che i Patrol Points siano assegnati nell'Inspector
- Controlla che il tag "Player" esista e sia assegnato al giocatore

**Problema: Il boss non cambia fase**
- Verifica che EnemyHealth abbia maxHealth configurato correttamente
- Controlla che BossEnemy.cs sia attaccato al GameObject corretto

---

Per qualsiasi domanda, consulta i commenti nel codice sorgente.