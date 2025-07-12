Purchase Cart Service

Servizio API che consente di calcolare il prezzo complessivo di un ordine e il relativo ammontare IVA, in base ai prodotti e alle quantità fornite.  
Il progetto è stato sviluppato in C# con ASP.NET Core 8 e utilizza **SQLite** come unico sistema di storage.

---

Funzionalità

- Calcolo automatico del prezzo e dell'IVA per ogni voce dell’ordine
- Totale ordine con somma prezzi + IVA
- Storage dei prodotti tramite database SQLite
- Restituzione di un identificativo ordine simulato (timestamp)

---

Esecuzione rapida (con Docker)

1. **Build immagine**

```bash
docker build -t mytest .
````

2. **Esegui i tre script**

```bash
docker run -v $(pwd):/mnt -p 9090:9090 -w /mnt mytest ./scripts/build.sh
docker run -v $(pwd):/mnt -p 9090:9090 -w /mnt mytest ./scripts/tests.sh
docker run -v $(pwd):/mnt -p 9090:9090 -w /mnt mytest ./scripts/run.sh
```


---

## 🔍 Verifica funzionamento API

Per testare manualmente l’API, puoi usare PowerShell. Lo script qui sotto effettua una chiamata `POST` e stampa il risultato:

```powershell
# URL dell’endpoint
$apiUrl = "http://localhost:9090/api/orders"

# Corpo JSON dell'ordine (puoi modificare i prodotti/quantità)
$body = @{
    items = @(
        @{ product_id = 1;  quantity = 1 }
        @{ product_id = 2;  quantity = 5 }
        @{ product_id = 3;  quantity = 1 }
    )
} | ConvertTo-Json -Depth 3

Write-Host "Invio richiesta POST a $apiUrl ..."
$response = Invoke-RestMethod `
                -Uri        $apiUrl `
                -Method     Post `
                -Body       $body `
                -ContentType "application/json"

Write-Host "`n--- Risposta ---"
$response | ConvertTo-Json -Depth 5
```

Esempio di risposta attesa:

```json
{
  "order_id": 1720704789,
  "order_price": 12.5,
  "order_vat": 1.25,
  "items": [
    { "product_id": 1, "quantity": 1, "price": 2, "vat": 0.2 },
    { "product_id": 2, "quantity": 5, "price": 7.5, "vat": 0.75 },
    { "product_id": 3, "quantity": 1, "price": 3, "vat": 0.3 }
  ]
}
```

---

## 📁 Struttura del progetto

* **Controllers/** — endpoint HTTP dell’API
* **Services/** — logica di calcolo degli ordini
* **Data/** — configurazione e accesso al database SQLite
* **Models/** — modelli di richiesta/risposta
* **scripts/** — build, test ed esecuzione dentro Docker
* **Migrations/** — migrazioni EF Core per generare lo schema

---

## Test

I test sono inclusi nel progetto `PurchaseCartService.Tests` e possono essere lanciati tramite lo script 

```bash
docker run -v $(pwd):/mnt -p 9090:9090 -w /mnt mytest ./scripts/tests.sh
```

La copertura attuale è la seguente:

| Test                                    | Caso verificato                                           |
| ----------------------------------------| --------------------------------------------------------- |
| `CreateOrder_CalculatesTotalsCorrectly` | Calcolo totale e IVA su due prodotti validi               |
| `CreateOrder_SkipsUnknownProducts`      | Gestione prodotto non esistente (ignorato nei calcoli)    |
| `CreateOrder_HandlesZeroQuantity`       | Gestione quantità **0** (riga presente ma price=0, vat=0) |


---

## Scelte progettuali

* La **persistenza è solo tramite SQLite**, per semplicità e portabilità.
* I dati dei prodotti sono precaricati tramite `OnModelCreating` nel `DbContext` e vengono inseriti all’avvio con `Database.Migrate()`.
* La logica di business è incapsulata nel servizio `OrderService`, separata dal controller.
* L’applicazione è strutturata per essere compatibile sia con ambienti locali che containerizzati.

---

## Note

* L’API accetta richieste su `POST /api/orders`
* Porta predefinita esposta: `9090`
* I prezzi e le VAT sono restituiti come numeri decimali (`double`) senza forzature di formato
* Il file SQLite viene creato dinamicamente nella directory `data/` al primo avvio

---

## Requisiti

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (solo se eseguito senza Docker)
* [Docker](https://www.docker.com/) 20+