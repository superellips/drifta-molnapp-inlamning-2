# Drifta molnapplikationer - Inlämning 2

av Erika Bladh-Öström

## Kluster-komponenter

Ett kubernets kluster består dels av ett _control plane_ och av en eller flera _worker nodes_, dessa två delar innehåller olika komponenter. En beskrivning av dessa komponenter finns i [kuberentes dokumentationen](https://kubernetes.io/docs/concepts/overview/components/).

### Control plane

**kube-apiserver:** Den centrala komponent som tillgängliggör klustrets HTTP-baserade programmeringsgränssnitt, det är mot detta gränssnitt som vi interagerar med klustret. Denna komponent går att skala horizontellt vid behov.

**etcd:** Är en databas med nyckel-värde-par av klusterdata, jag tänker att dessa kan ses lite som _klustrets miljövariabler_.

**kube-scheduler:** Komponent som kollar efter hemlösa poddar och säger vid behov åt tillgängliga _worker nodes_ att ge dom ett hem.

**kube-controller-manager:** Komponent som hanterar många olika typer av _controllers_ och som försöker genomföra olika processer för att uppnå det önskade tillståndet inom klustret. Exempel på _controllers_ är bland andra: 

- Node Controller, som är ansvarig för att hålla koll på tillgängliga _nodes_ och agera om något skulle gå fel med dom.
- Job Controller, som är ansvarig för att dom engångsjobb som tilldelas klustret genomförs.

**cloud-controller-manager (optional):** Komponent som lägger till cloudspecifika funktioner som är kopplade till molnleverantörens API:er och tjänster. Detta finns inte tillgängligt när vi kör ett kluster lokalt på våra egna maskiner.

### Worker node

**kubelet:** Den komponent som ser till att containrar körs som pods och som håller koll på att dom beter sig korrekt.

**Container runtime:** Komponent som ansvarar för att kubernetes kan köra containrar effektivt och som hanterar deras livscykler i klustret.

**kube-proxy (optional):** Komponent som ansvarar för att nätverksanslutningarna till _worker nodes_ och som ser till att _service_ koncepten inom kuberenets fungerar.

### Tillägg

Utöver dessa grundläggande komponenter så finns det andra komponenter som kan utöka funktionaliteten med till exempel domännamnssystem som kan användas innuti klustret eller verktyg centraliserad loggning och övervakning av resurs användning.

## Kubernetes objekt

### Poddar

**Pod:** En abstraktion som slår in en (eller flera) container så att kuberentes kan hantera containrar av olika slag på samma vis.

**Replica set:** Är ett objekt som försöker försäkra att ett visst antal poddar kör. Det används sällan själv utan är framförallt menat som en komponent i en _deployment_.

**Deployment:** Är ett objekt som försöker försäkra att ett visst antal poddar kör och som ger funktioner så som _rollbacks_ och _rolling updates_. Försäkran av att poddar körs uppnås genom att ett _replica set_ skapas som en del av en deployment.

**Stateful set:** Liknar _replica set_ men ger också en försäkran om att skapade poddar tilldelas en stabil och persistent nätverksidentitet, det används främst i dom tillfällen som en vill ha någon form av data persistens (så som för databaser).

### Services

Services används parallelt med poddar för att på olika vis i slutändan exponera dom inneslutna containerna till nätverk.

**ClusterIP:** Öppnar upp nätverksgrejjerna till poddar så att dom kan nås inom klustret.

**Node Port (?):** Öppnar upp en port på alla nodes som kopplas mot en _cluster ip_ inom klustret så att det går att ansluta mot dessa utifrån.

**Load Balancer:** Kopplar ihop lastbalanseringstjänster från en molnplattform med det klusterinterna nätverket så att trafik mot ett gäng poddar fördelas rättvist.

**Ingress:** Är som en kombination av en webserver implementation (t.ex. nginx) som agerar som _controller_ och en ingress resurs. Controllerna lyssnar efter förändringar i ingress resursen och ser till att det som beskrivs upprätthålls. En _ingress controller_ tänker jag liknar en reverse proxy och verkar i grunden bara vara en webserver som konfigureras med innehållet i ingress objekt.

### Config Map

En config map lägger till lite nyckel-värde-par i _etcd_ varifrån dom kan används lite som klustrets miljövariabler och kan användas för att t.ex. styra containrars beteenden (genom att exempelvis beskriva hur deras _connection string_ till en databas behöver se ut).

### Namespace

Används för att separera olika kubernetes resurser under olika namn, t.ex. så kan en separera två stycken olika applikationer inom samma kluster genom att placera dom under olika namn.

### Job

Ett objekt som ser till att poddar som har till syfte att genomföra en process bara en gång skapas och sedan plockas bort.

## Verktygslådan

Här är ett urval av verktyg som kan användas för att administerar ett kubernetes kluster. Jag känner inte att jag riktigt förstår vad frågeställningen om _användbara kommandon_ har för syfte och det kommer nog framgå av vilka exempel jag har valt ut. För att inte behöva upprepa mig så är följande två dom, för mig, mest användbara oavsett verktyg:

- `<tool> --help`
- `<tool> <command> --help`

### kubectl

Det primära verktyget som används för att agera mot ett kluster, det ansluter alltså till _kube-apiserver_ för att åstadkomma det vi säger åt det att göra. Några användbara kommandon är:

- `kubectl config get-contexts`
- `kubectl config use-context <context-name>`

### helm

Ett verktyg som kombinerar flera olika beskrivna kubernetes objekt och sedan kör upp dessa tillsammans (vilket kallas för en _chart_). Det går även att separera ut delar som variabler för att kunna köra upp saker med olika val beroende på i vilken miljö en arbetar mot (t.ex. att använda nodeport när det körs lokalt men använda loadbalancer när det körs på en molnplattform som har stöd för det). Helm verkar inte installeras med autocompletion men det går att åstadkomma så här (för bash):

```bash
# Skriv output from helm till en av konfigurationsfilerna för bash, 
# t.ex. .bashrc (eller till någon annan fil som .bashrc inkluderar).
helm completion bash > ~/.bashrc
# Läs in förändringen för att använda completion
. ~/.bashrc
```

### argocd

Verktyg för att interagera med en argocd server som kör i ett kluster, dess funktioner finns också tillgängliga i ett webbgränssnitt men hur det används är svårare att beskriva i kod. Likt helm så kan autocompletion fixas med någon variant av:

`argocd completion bash > ~/.bashrc`

## Driftsättning

### Utgångspunkt

Mitt mål är att med ArgoCD och github actions driftsätta applikationen från grupparbetet (_tipsrundan_). Jag kommer primärt att utforma min lösning för ett lokalt kluster på min laptop, mitt kluster kommer vara skapat med _minikube_, min känsla är att kubernetes kluster fungerar i grunden likadant oavsett var eller vem som tillhandhåller det. Valet av _minikube_ är resultatet av att docker desktop i min mening varit ganska klumpigt under linux i jämförelse med docker cli. 

Det jag presenterar i min guide bör ses som en grund som ska anpassas för att nå en mer komplett lösning, exempelvis driftsättning på ett kluster tillhandahållet av en molnleverantör. 

Jag kommer anta att du som läser det här redan har god förståelse för docker, git och andra kringliggande teknologier, jag kommer inte ge detaljerade instruktioner för installation och konfiguration av verktyg utan förväntar mig att du kan hitta den informationen på egen hand. I praktiken så är det framförallt Lars som är min målgrupp.

### Verktyg

Dom verktyg som jag primärt har lutat mig på för att lösa den här uppgiften:

- Debian (OS)
- VS Code
- git & Github
- docker cli
- dotnet
- minikube
- kubectl
- helm
- argocd

### Genomförande

#### Hitta ett kluster att arbeta mot

Jag börjar med att skapa ett lokalt kluster med `minikube start`, jag kommer utgå ifrån att du kan hitta ett kluster du kan arbeta mot. Vi säkerställ med `kubectl config current-context` att vi vänder oss mot det kluster som vi förväntar oss.

#### Stoppa in källkoden i ett nytt repository

Jag har skapat ett nytt repository på github (och utgår från att du också gör det) och kommer i detta lägga till källkoden för _Tipsrundan_:

```bash
# Clone repository
git clone https://github.com/CuriosityFanClub/tipsrundan.git
# Remove some files and folders so as to not confuse git & github
rm -rf tipsrundan/.git tipsrundan/.github
# Create a commit
git add tipsrundan/* && git commit -m "Adds the tipsrundan application." && git push
```

#### Github actions workflow

Vi behöver för vårat github repository tillåta github actions att göra commits genom att navigera till _settings_ för det och under _actions_ och sedan _general_ ändra _Workflow permissions_ till _Read and write permissions_.(`settings->actions->general->Workflow permissions => Read and write permissions`)

Vi kommer också vilja ha en workflow-fil i `.github/workflows/docker-image.yml`. Min utgångspunkt är följande:

```yaml
name: Builds and pushes application image to github container registry

on:
  push:
    branches:
      - main
    paths:
      - 'tipsrundan/**'
  workflow_dispatch:

jobs:
  build:
    runs-on: ubuntu-latest
    permissions:
      packages: write
      contents: write

    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
    
      - name: Authenticate with github registry
        uses: docker/login-action@v1
        with:
          registry: ghcr.io
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Build and push image
        run: |
          cd tipsrundan
          docker build -t ghcr.io/${{ github.repository_owner }}/tipsrundan:${{ github.sha }} .
          docker push ghcr.io/${{ github.repository_owner }}/tipsrundan:${{ github.sha }}

      - name: Configure git
        run: |
          git config --global user.name "GitHub Actions"
          git config --global user.email "github-actions@github.com"
          git config pull.rebase false
          git fetch
          git checkout prod
          git merge origin/main --allow-unrelated-histories -X theirs
        
      - name: Update manifests
        run: |
          sed -i 's|ghcr.io/${{ github.repository_owner }}/tipsrundan:.*|ghcr.io/${{ github.repository_owner }}/tipsrundan:${{ github.sha }}|' manifests/tipsrundan-deployment.yml
    
      - name: Commit and push updated manifests
        run: |
          git add manifests/tipsrundan-deployment.yml
          git commit -m "Automatic update of manifests done within github actions for commit: ${{ github.sha }}"
          git push
```

Med ett definierat workflow så behöver vi också lite filer för kubernetes, fler av dessa finns senare i dokumentet men här är en som vi kan börja med för att se om allting fungerar (observera att din github användare behöver fyllas i istället för `[GH_USER]`), `manifests/tipsrundan-deployment.yml`:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: tipsrundan-webapp
spec:
  replicas: 1
  selector:
    matchLabels:
      app: tipsrundan-webapp
  template:
    metadata:
      labels:
        app: tipsrundan-webapp
    spec:
      containers:
      - name: tipsrundan-container
        image: ghcr.io/[GH_USER]/tipsrundan:latest
        ports:
        - containerPort: 80
```

Sist men inte minst behöver vi se till att allt detta pushas till github:

```bash
# Add and commit
git add . && git commit -m "Updates the workflow and manifests."
# Push to the main branch
git push
# Create and checkout a branch named prod (to be used later)
git branch prod && git checkout prod
# Ensure that github is aware of this branch aswell
git push
# And checkout the main branch again
git checkout main
```

#### Container registry

Jag har hittills inte hittat någon bra lösning för att skapa detta automatiskt och samtidigt göra det _public_, jag löst det genom att göra `docker build` och `docker push` lokalt (se `docker-image.yml` för ungefär vad dessa var) och sedan genom webläsaren ändra tillgängligheten.

#### Installera och konfigurera argocd

ArgoCD kommer att leva innuti vårat kluster och kommer att kunna interagera med det innifrån. Det kommer också att lyssna efter förändringar i vårat repository och utifrån dessa försöka låta kubernetes nå det tillstånd som vi önskar.

För att installera argocd i klustret:

```bash
# Create a namespace for argocd
kubectl create namespace argocd
# Apply the argo project installation file
kubectl apply -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml -n argocd
```

Installationen kommer evntuellt att ta en stund, men när den är färdig kan vi logga in med argocds cli enligt följande steg:

Exponera argocd-server i ett shell med: `kubectl port-forward svc/argocd-server 8080:443 -n argocd`

Och sedan logga in från ett annat med: `argocd login localhost:8080 --username admin --password $(argocd admin initial-password -n argocd | head -n 1) --insecure`

Efter detta bör vi byta adminanvändarens lösenord med: `argocd account update-password`

Sedan vill vi skapa en ny applikation i ArgoCD med: `argocd app create tipsrundan --repo https://github.com/[GH_USER]/[REPO].git --revision prod --path manifests --dest-namespace default --dest-server https://kubernetes.default.svc --sync-policy auto --auto-prune --self-heal` (använd dina uppgifter för användarnamn och repository)

#### Avslutningsvis

Som avslutning så behöver min lösning också följande filer under `manifests`:

`tipsrundan-service.yml` (updaterad för att använda configmap)

```bash
apiVersion: apps/v1
kind: Deployment
metadata:
  name: tipsrundan-webapp
spec:
  replicas: 1
  selector:
    matchLabels:
      app: tipsrundan-webapp
  template:
    metadata:
      labels:
        app: tipsrundan-webapp
    spec:
      containers:
      - name: tipsrundan-container
        image: ghcr.io/superellips/tipsrundan:testing
        ports:
        - containerPort: 8080
        envFrom: 
        - configMapRef:
            name: app-config
```

`tipsrundan-service.yml`

```bash
apiVersion: v1
kind: Service
metadata:
  name: tipsrundan-service
spec:
  selector:
    app: tipsrundan-webapp
  ports:
  - protocol: TCP
    port: 80
    targetPort: 80
  type: NodePort
```

`tipsrundan-configmap.yml`

```bash
apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
data:
  DbImplementation: "MongoDb"
  MongoDbConnection: "mongodb://mongodb-service:27017"
```

`mongodb-statefulset.yml`

```bash
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: mongodb
spec:
  selector:
    matchLabels:
      app: mongodb
  replicas: 1
  serviceName: mongodb-service
  template:
    metadata:
      labels:
        app: mongodb
    spec:
      containers:
      - name: mongodb
        image: mongo:latest
        ports:
        - containerPort: 27017
        volumeMounts:
        - name: mongodb-data
          mountPath: /data/db
  volumeClaimTemplates:
  - metadata:
      name: mongodb-data
    spec:
      accessModes:
        - ReadWriteOnce
      resources:
        requests:
          storage: 1Gi
```

`mongodb-service.yml`

```bash
apiVersion: v1
kind: Service
metadata:
  name: mongodb-service
spec:
  selector:
    app: mongodb
  ports:
    - protocol: TCP
      port: 27017
      targetPort: 27017
```


Sedan gör vi en förändring i `tipsrundan/src/Tipsrundan.Web/Pages/Home/Index.cshtml` och lägger till en ny commit och pushar till github. Detta bör starta ett nytt jobb i Github actions och (tids nog) leda till att ArgoCD updaterar vilken image som används för vår applikations container.

Vi kan verifiera att förändringen har gått igenom med `kubectl port-forward tipsrundan-webapp-[ID_FROM_ARGOCD] 8081:8080` och navigera till `localhost:8081`.

### Reflektioner

Jag känner att jag i min lösning lyckats uppnå det jag ville, men jag vill också erkänna att det är en ganska inkomplett lösning. Primärt för att jag inte inkluderat någon tydlig väg att exponera applikationen utan bara förlitat mig på `kubectl port-forward`. En av anledningarna till detta är att jag ville låta lösningen vara mer allmängiltig. Möjliga vägar för att exponera den skulle vara att låta `tipsrundan-service` vara av typen `load-balancer`, eller att använda en _ingress_ och _ingress-controller_. Båda dessa alternativ är möjliga både lokalt med _minikube_ samt på google cloud, men jag är osäker på om det är möjligt lokalt med det kluster docker desktop kan skapa.

Sedan så känner jag att jag kompromissat lite med säkerheten genom att arbeta med både ett publikt repository samt container registry. Båda dessa tror jag hade kunnat göras privata genom att använda instruktionerna i [Private Repositories](https://argo-cd.readthedocs.io/en/stable/user-guide/private-repositories/) samt [Authentication in ArogCD image updater](https://argocd-image-updater.readthedocs.io/en/stable/basics/authentication/#authentication-in-argo-cd-image-updater).

En del som jag känner mig nöjd med är att ArgoCD vänder sig till en annan branch än `main` och att github actions updaterar manifesten i en separat branch. Min känsla är att detta med fördel kan användas i kombination med branch regler för att minimera risken att ArgoCD försöker hämta en image med en tag som inte finns. Det separarerar också ut dom commits som updaterar `tipsrundan-deployment.yml` till en separat branch.

Lösningen har dock samma svaghet när det kommer till persistens som även fanns med den driftsättning vi hade för grupparbetet. Detta är något jag gärna hade lyckats förbättra men jag har inte riktigt haft tid att kolla igenom om det finns någon bra lösning i kubernetes för att mer permanent persistera från ett volume claim. Men jag har sett att det finns många (många) funktioner som jag gärna skulle ha möjlighet att utforska i framtiden, bland annat [Horizontal Pod Autoscaling](https://kubernetes.io/docs/tasks/run-application/horizontal-pod-autoscale/).

## Mina filer

Dessa bör alla finnas närvarande i det zip-arkiv som bör bifogas tillsammans med den här inlämningen, dom är för många för att i sin helhet kunna inkluderas här.
