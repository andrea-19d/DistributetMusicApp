## 1. Verificarea dacă Replica Set este activ

Conectare la un nod (ex: mongo1):

    docker exec -it mongo1 mongosh


Verificare status:

    rs.status()


Dacă vezi:

    PRIMARY → nodul principal
    
    SECONDARY → nod replică

Replica Set funcționează.

## 2. Testarea sincronizării datelor între noduri
   ### 2.1. Inserare pe PRIMARY

Conectează-te la PRIMARY:

    docker exec -it mongo1 mongosh


Apoi:

    use GatewayDb
    
    db.album.insertOne({
    Title: "FromPrimary",
    Year: 2025,
    createdAt: new Date()
    })
    
    db.album.find().pretty()
    

Ar trebui să vezi documentul introdus.

### 2.2. Verificare replicare pe SECONDARY

Conectează-te la un nod secundar (ex: mongo3):

    docker exec -it mongo3 mongosh


Activezi citirea din secundar:

    db.getMongo().setReadPref("secondaryPreferred")


Apoi:

    use GatewayDb
    db.album.find().pretty()


Dacă vezi documentul, replicarea funcționează corect.

## 3. Cum forțezi citirea din PRIMARY sau SECONDARY
   Citire din PRIMARY:
    
    db.getMongo().setReadPref("primary")

Citire din SECONDARY:

    db.getMongo().setReadPref("secondary")

Citire fallback (primary dacă secondary nu poate):

    db.getMongo().setReadPref("secondaryPreferred")

## 4. Cum verifici de pe ce nod se citesc datele în API

În codul tău, ai configurat:

    Read Repository → ReadPreference.SecondaryPreferred
    
    Write Repository → Primary

Testare în Postman:

### 4.1. Test GET (citire)

    GET http://localhost:8080/catalog-service/Album


Ar trebui să fie servit din SECONDARY (sau PRIMARY dacă SECONDARY nu răspunde).

### 4.2. Test POST (scriere)

    POST http://localhost:8080/catalog-service/Album
Content-Type: application/json


POST va scrie întotdeauna în PRIMARY.

## 5. Cum vezi exact cine a procesat query-urile (profiling)

Activează profiling pe PRIMARY:

    use GatewayDb
    db.setProfilingLevel(2)


Apoi vezi logurile:

    db.system.profile.find().pretty()


În interior vei vedea câmpul:

    "host" : "mongo1:27017"


sau

    "host" : "mongo2:27017"


Acesta arată clar ce nod a executat operația.

## 6. Cum testezi disponibilitatea replica set-ului
Oprești PRIMARY și vezi comportamentul:

    docker stop mongo1


    GET → funcționează în continuare (SECONDARY devine PRIMARY)
    POST → așteaptă un nou PRIMARY

După test:

    docker start mongo1

## 7. Cum ștergi datele din baza de date
Ștergere un singur document după ID:
  
    db.album.deleteOne({ _id: ObjectId("ID_UL") })

Ștergere toate documentele din colecție:

    db.album.deleteMany({})

Ștergere colecție întreagă:

    db.album.drop()

Ștergere baza de date:
use GatewayDb
db.dropDatabase()

8. Comenzi utile în Docker
Listare containere:

    docker ps

Restart serviciu:

    docker restart mongo1

Conectare la container:

    docker exec -it mongo2 mongosh

9. Comenzi rapide pentru Replica Set
Reconfigurare Replica Set:
    rs.reconfig({

       _id: "rs0",
       members: [
       { _id: 0, host: "mongo1:27017" },
       { _id: 1, host: "mongo2:27017" },
       { _id: 2, host: "mongo3:27017" }
       ]
       })

Verificare roluri:

    rs.status().members.map(x => ({ name: x.name, state: x.stateStr }))