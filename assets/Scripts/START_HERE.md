# 🎮 START HERE - Unity 2D Run-and-Gun Platformer

## ✅ Cosa è stato creato

Tutto il codice per il tuo gioco è pronto in `Assets/Scripts/`. Ecco la struttura completa:

```
Assets/Scripts/
├── GameManager.cs                    ← Gestore principale (NON MODIFICARE)
├── Player/
│   ├── PlayerController.cs          ← Movimento, salto, ground check
│   ├── PlayerHealth.cs              ← Sistema salute
│   └── PlayerShooter.cs             ← Sparatoria con mira mouse
├── Enemies/
│   ├── EnemyController.cs           ← Classe base nemici (astratta)
│   ├── GenericEnemy.cs              ← Nemico normale con IA
│   ├── BossEnemy.cs                 ← Boss con 3 fasi
│   ├── EnemyHealth.cs               ← Sistema salute nemici
│   └── EnemySpawner.cs             ← Spawn automatico nemici
├── Projectiles/
│   └── Bullet.cs                    ← Proiettili (giocatore + nemici)
├── Camera/
│   └── CameraFollow.cs              ← Camera che segue il player
├── UI/
│   └── HUDManager.cs                ← Interfaccia gioco
├── Collectibles/
│   └── Collectible.cs               ← Oggetti raccoglibili
├── Editor/
│   └── SpriteImporterConfig.cs      ← Utility per sprite pixel art
├── README_Setup.md                  ← Guida setup completa
└── SUMMARY.md                       ← Riepilogo tecnico
```

## 🚀 Come Iniziare (5 minuti)

### 1. Crea i Tag in Unity
In **Edit > Project Settings > Tags and Layers**:

Crea questi tag:
- `Player`
- `Enemy` 
- `Boss`
- `Collectible`
- `Hazard`
- `Ground`
- `Wall`

### 2. Crea i Layer
Nella stessa finestra, crea layer:
- `Ground` (layer 6)
- `Aim` (layer 7)

### 3. Importa gli Sprite
Trascina tutti i tuoi file PNG nella cartella `Assets/Sprites/`.

Poi vai su **Tools > Configure Pixel Art Sprites** e clicca "Configure All in Path" per impostare automaticamente:
- Filter Mode: Point (pixelato)
- Pixels Per Unit: 16

### 4. Crea il Player GameObject

Crea un GameObject vuoto chiamato **"Player"** e aggiungi questi componenti nell'ordine:

```
1. SpriteRenderer
   └── Assigna lo sprite del tuo personaggio

2. Rigidbody2D
   ├── Body Type: Dynamic
   ├── Gravity Scale: 3
   └── Collision Detection: Continuous

3. BoxCollider2D
   ├── Size: (0.8, 1.5)
   └── Offset: (0, -0.2)

4. PlayerController.cs
   ├── Move Speed: 8
   ├── Jump Force: 15
   ├── Double Jump Force: 12
   ├── Ground Check: [Crea figlio "GroundCheck" e trascinalo qui]
   ├── Ground Check Radius: 0.2
   └── Ground Layer: Ground

5. PlayerHealth.cs
   └── Max Health: 3

6. PlayerShooter.cs
   ├── Bullet Prefab: [Crea prima il prefab Bullet - vedi sotto]
   ├── Fire Point: [Crea figlio "FirePoint" e trascinalo qui]
   ├── Bullet Speed: 15
   └── Fire Rate: 0.2

7. Crea figli:
   ├── GroundCheck (GameObject vuoto)
   │   └── Posizione: (0, -0.7)
   └── FirePoint (GameObject vuoto)
       └── Posizione: (0.3, 0)
```

**Importante:** Imposta il tag del Player a "Player"!

### 5. Crea il Prefab Bullet

Crea un GameObject vuoto, chiamalo **"Bullet"**, aggiungi:

```
1. SpriteRenderer
   └── Sprite piccolo (cerchio o pallino)

2. Rigidbody2D
   ├── Body Type: Kinematic
   └── Gravity Scale: 0

3. CircleCollider2D
   ├── Radius: 0.15
   └── Is Trigger: ✓

4. Bullet.cs
   └── Lifetime: 3

Poi trascina questo GameObject nella cartella Assets per crearne il prefab.
Elimina l'istanza dalla scena.
```

### 6. Crea il GameManager

Crea un GameObject vuoto chiamato **"GameManager"**, aggiungi `GameManager.cs`:

```
1. GameManager.cs
   ├── Max Lives: 3
   ├── Score to Win: 1000
   ├── Player Prefab: [Trascina qui il prefab Player]
   └── Spawn Point: [Crea figlio "SpawnPoint" e trascinalo qui]

2. Crea figlio:
   └── SpawnPoint (GameObject vuoto)
       └── Posizione dove appare il player
```

### 7. Crea un Nemico Generico

Crea un GameObject chiamato **"GenericEnemy"**:

```
1. SpriteRenderer (sprite nemico)
2. Rigidbody2D (Dynamic, Gravity 3)
3. BoxCollider2D (Size: 0.8x1.5)
4. EnemyHealth.cs (Max Health: 3)
5. GenericEnemy.cs
   ├── Detection Range: 5
   ├── Patrol Speed: 2
   ├── Chase Speed: 4
   ├── Attack Range: 3
   ├── Fire Rate: 1.5
   ├── Enemy Bullet Prefab: [Trascina prefab Bullet qui]
   └── Fire Point: [Crea figlio e impostalo]

6. Crea figli PatrolPoint (3 GameObject vuoti)
7. Imposta tag "Enemy"
```

### 8. Configura la Camera

Seleziona la Main Camera, aggiungi `CameraFollow.cs`:

```
1. CameraFollow.cs
   ├── Target: [Trascina il Player qui]
   ├── Follow Speed: 5
   └── Offset: (0, 1.5, -10)
```

### 9. Crea una Scena di Test

Crea una nuova scena con:
- Un piano di base (Ground) con tag "Ground"
- Alcune piattaforme
- Il GameManager
- 2-3 nemici generici
- 5-10 collectible sparsi

## 🎯 Controlli

| Azione | Tasto |
|--------|-------|
| Muovi | A/D o Frecce |
| Salta | Spazio o W |
| Doppio Salto | Spazio (in aria) |
| Spara | Mouse Sinistro |

## 🔧 Personalizzazione Rapida

### Cambiare velocità giocatore:
Apri `PlayerController.cs` e modifica:
```csharp
public float moveSpeed = 8f; // Aumenta per più velocità
public float jumpForce = 15f; // Aumenta per salto più alto
```

### Cambiare difficoltà nemici:
Apri `GenericEnemy.cs` e modifica:
```csharp
public float chaseSpeed = 4f; // Velocità inseguimento
public float fireRate = 1.5f; // Frequenza sparo (meno = più veloce)
```

### Cambiare vita boss:
Apri `BossEnemy.cs` e modifica in EnemyHealth:
```csharp
public int maxHealth = 15; // Vita boss
```

## 📝 Prossimi Passi

1. **Animazioni**: Crea animator con state machine per idle, run, jump, shoot
2. **Suoni**: Aggiungi AudioSource per sparo, salto, danni
3. **Particle Effects**: Crea effetti per esplosioni e impatti
4. **UI**: Crea HUD con barre vita e punteggio
5. **Livelli**: Crea più scene con piattaforme diverse

## 🐛 Troubleshooting

**Il player non salta?**
- Verifica che GroundCheck sia sotto il player
- Controlla che il layer "Ground" sia assegnato nelle piattaforme

**I nemici non si muovono?**
- Imposta tag "Enemy" al GameObject
- Verifica che i PatrolPoint siano assegnati nell'Inspector

**La camera non segue il player?**
- Assegna il Player al campo "Target" in CameraFollow

## 📚 Documentazione Completa

Leggi `README_Setup.md` per la guida dettagliata passo-passo.

---

**Buon divertimento!** 🎮✨

Se hai domande, controlla i commenti nel codice sorgente.