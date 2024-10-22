# Att köra applikationen lokalt med databas

Då vi nu faktiskt kör applikationen mot en riktig databas på riktigt så tänker jag att det kanske behövs lite instruktioner för att kunna köra den ordentligt lokalt medan vi utvecklar. Nedan så kommer jag beskriva följande alternativ som bör få det att fungera:

1. Köra lokalt mot en SQLite-databas i form av en fil
2. Köra mongodb lokalt (inklusive mongo-express för att kolla in i den)

Jag skriver även ett bash-script som kör igång applikationen lokalt med `dotnet watch` samt en tillhörande databas.

## 0. Introduktion

Valet av databas configureras just nu i `Tipsrundan.Infrastructure.Configuration.DependencyInjectionHelper` i en switch som läser av miljövariabeln `DbImplementation`. Dom val som just nu är möjliga är `SqliteLocal` och `MongoDb`. För sqlite behöver vi också sätta en miljövariabel `SqliteConnectionString` som visar var vi databasen finns. För mongodb behöver vi sätta `MongoDbConnection` så att den hittar rätt.

Då våra datorer lokalt ser olika ut så kommer jag lägga till `run-app-locally.sh` i vår `.gitignore` så att vi inte skriver över varandras skript.

## 1. SQLite

För att köra lokalt med SQLite så kan vi modifiera ovannämnda miljövariabler i `appsettings.json` men det är min åsikt att det är smidigare att ha ett litet bash-skript istället, nedan är hur jag skulle skriva ett sådant:

```bash
#!/bin/bash

# Här behöver du själv ange en absolut sökväg där du vill att databasen ska sparas
sqlite_connection="Data Source=/your/path/to/quiz.db"

echo "Running sqlite"

# Exportera miljövariblerna
export DbImplementation="SqliteLocal"
export SqliteConnectionString="$sqlite_connection"

# Kör applikationen (notera att detta utgår från att detta skript ligger i roten av
# projektet).
dotnet watch --project src/TipsRundan.Web/TipsRundan.Web.csproj

echo "Script exiting."
```

## 2. MongoDB

För att köra med MongoDb så har jag skrivit följande fil för compose, den kör upp en container med MongoDB och en med MongoExpress samt konfigurera portarna så att båda är tillgängliga utifrån:

```yaml
services:
  db:
    image: mongo
    restart: always
    volumes:
      - mongodb-data:/data/db
    ports:
      - "27017:27017"
  express:
    image: mongo-express
    restart: always
    ports:
      - "8081:8081"
    environment:
      ME_CONFIG_MONGODB_SERVER: db

volumes:
  mongodb-data:
```

Och det här är hur jag skulle köra det med vår applikation:

```bash
#!/bin/bash

# This should be the same when running the applikation locally
mongo_connection="mongodb://localhost:27017"

echo "Running mongodb"

# Export environment variables
export DbImplementation="MongoDb"
export MongoDbConnection=$mongo_connection

# Start the docker continers with docker compose up
# We provide the flag '-f' directing it to the above compose file
echo "Starting docker containers"
docker compose -f Compose-local.yml up -d

# Start the application
dotnet watch --project src/TipsRundan.Web/TipsRundan.Web.csproj

# Take down the containers before exiting
# If we wanted to remove the volume we would pass '-v' after 'down'
docker compose -f Compose-local.yml down
echo "Script exiting."
```

## 3. Extra

Jag kanske är lite extra men här är båda ovanstående i ett script där vi (efter att ha skrivit våra värden för miljövaribler) kan köra `./run-app-locally.sh mongodb` för mongo och `./run-app-locally.sh sqlite` för SQLite.

```bash
#!/bin/bash

if [ "$1" == "mongodb" ] || [ "$1" == "sqlite" ]; then
    implementation=$1
else
    echo "You need to use either sqlite or mongodb."
    exit 1
fi

sqlite_connection="Data Source=/home/erika/quiz.db"
mongo_connection="mongodb://localhost:27017"


if [ "$implementation" == "mongodb" ]; then
    echo "Running mongodb"
    export DbImplementation="MongoDb"
    export MongoDbConnection=$mongo_connection
    echo "Starting docker containers"
    docker compose -f Compose-local.yml up -d
    dotnet watch --project src/TipsRundan.Web/TipsRundan.Web.csproj
    docker compose -f Compose-local.yml down
    echo "Script exiting."
elif [ "$implementation" == "sqlite" ]; then
    echo "Running sqlite"
    export DbImplementation="SqliteLocal"
    export SqliteConnectionString="$sqlite_connection"
    dotnet watch --project src/TipsRundan.Web/TipsRundan.Web.csproj
    echo "Script exiting."
fi
```