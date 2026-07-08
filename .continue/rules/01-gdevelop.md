# GDevelop Rules

Quando lavori con GDevelop 5:
- Rispetta il formato ufficiale dei file JSON.
- Non inventare proprietà JSON.
- Non inventare Behavior.
- Non inventare Event.
- Non inventare Action.
- Non inventare Condition.
- Non inventare Extension.

Prima di creare JSON:
verifica la struttura esistente del progetto.

Preferenze:
1. Eventi GDevelop
2. Funzioni native
3. JavaScript Events
4. Estensioni personalizzate

Usa JavaScript solo quando gli eventi non sono sufficienti.

Quando proponi una nuova funzionalità:
separa chiaramente:
FUNZIONE ESISTENTE
oppure
NUOVA IDEA DA IMPLEMENTARE

## Modalità Compatibilità (DEFAULT)
Quando lavori su:
- JSON di scene GDevelop
- Eventi e condizioni
- Variabili e oggetti
- Estensioni esistenti

DEVI:
✅ Usare solo API ufficiali GDevelop
✅ Rispettare il formato JSON corretto
✅ Non inventare azioni/condizioni non esistenti
✅ Verificare la sintassi prima di proporre

NON DEVI:
❌ Inventare nuove feature se non richiesto esplicitamente
❌ Usare codice non supportato da GDevelop
❌ Cambiare strutture senza motivo

## Modalità Progettazione (QUANDO CHIEDI ESPPLICITAMENTE)
Quando chiedi:
- "Inventa un nuovo gameplay"
- "Progetta una nuova meccanica"
- "Crea un sistema di crafting"

PUOI:
✅ Invenire nuove meccaniche e concetti
✅ Proporre architetture innovative
✅ Suggerire nuovi oggetti o eventi

MA DEVI:
✔ Contrassegnare chiaramente le parti inventate con [INVENZIONE]
✔ Indicare cosa esiste già in GDevelop vs nuovo
✔ Proporre implementazioni realistiche