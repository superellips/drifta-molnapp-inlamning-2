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

### Verktyg

Dom verktyg som jag primärt har lutat mig åt för att lösa den här uppgiften:

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

Min plan är att driftsätta applikationen [tipsrundan](https://github.com/CuriosityFanClub/tipsrundan) från grupparbetet med hjälp av github och ArgoCD.

#### Hitta ett kluster att arbeta mot

Min erfarenhet dom senaste veckorna har varit att kubernetes genom docker desktop inte har varit speciellt smidigt under linux. Som ett alternativ så har jag valt att köra kubernetes lokalt med `minikube` istället. Jag tror inte att detta innebär några faktiskta skillnader mer än att den context mot vilken jag arbetar är `minikube` istället för `docker-desktop`.

Även när det kommer till ett kluster på en molnplattform så är min känsla att skillnaderna är minimala och jag kommer inte gå in i några detaljer på hur ett sådant provisioneras. Jag kommer utgå ifrån att du lyckas hitta både ett lokalt kluster och ett kluster hos en molnleverantör att agera mot.

Säkerställ med `kubectl config current-context` att du vänder dig mot det kluster som du förväntar dig.

#### Stoppa in källkoden i ett nytt repository

Jag tänker utgå ifrån ett nytt repository på github (samt att du kan skapa detta själv) och i detta lägga till källkoden för _Tipsrundan_:

```bash
# Clone repository
git clone https://github.com/CuriosityFanClub/tipsrundan.git
# Remove some files and foleders so as to not confuse git & github
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

      - name: Update manifests
        run: |
          sed -i 's|ghcr.io/${{ github.repository_owner }}/tipsrundan:.*|ghcr.io/${{ github.repository_owner }}/tipsrundan:${{ github.sha }}' manifests/tipsrundan-deployment.yml
    
      - name: Commit and push updated manifests
        run: |
          git config --global user.name "GitHub Actions"
          git config --global user.email "github-actions@github.com"
          git add manifests/tipsrundan-deployment.yml
          git stash
          git checkout prod
          git stash pop
          git commit -m "Automatic update of manifests done within github actions for commit: ${{ github.sha }}"
          git push
```

Med ett definierat workflow så behöver vi också lite filer för kubernetes, alla dessa finns i slutet av dokumentet men här är en som vi kan börja med för att se om allting fungerar, `manifests/tipsrundan-deployment.yml`:

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
        image: ghcr.io/[DITT_GH_NAMN]/tipsrundan:latest
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

#### Installera och konfigurera argocd

ArgoCD kommer att leva innuti vårat kluster och kommer att kunna interagera med det innifrån. Det kommer också att lyssna efter förändringar i vårat repository och utifrån dessa försöka låta kubernetes nå det tillstånd som vi önskar.

För att installera argocd i klustret:

```bash
# Create a namespace for argocd
kubectl create namespace argocd
# Apply the argo project installation file
kubectl apply -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml -n argocd
```

Installationen kommer ta en stund, men när den är färdig kan vi logga in med argocds cli enligt följande steg:

Exponera argocd-server i ett shell med: `kubectl port-forward svc/argocd-server 8080:443 -n argocd`

Och sedan logga in från ett annat med: `argocd login localhost:8080 --username admin --password $(argocd admin initial-password -n argocd | head -n 1) --insecure`

Sedan vill vi skapa en ny applikation i ArgoCD med: `argocd app create tipsrundan --repo https://github.com/[GH_USER]/[REPO].git --revision prod --path manifests --dest-namespace default --dest-server https://kubernetes.default.svc --sync-policy auto --auto-prune --self-heal` (använd dina uppgifter för användarnamn och repository)

#### Avslutningsvis

Som avslutning så behöver min lösning också följande filer under `manifests`:

- tipsrundan-service.yml
- tipsrundan-configmap.yml
- mongodb-statefulset.yml
- mongodb-service.yml

### Reflektioner

Att låta github actions pusha till `prod`.
Lösningen innehåller samma svaghet när det kommer till data persistens som tidigare.
Att arbeta mot ett kluster hos en molnleverantör.
Annat som kan utforskas i kubernetes.

## Mina filer

Dessa bör alla finnas närvarande i det zip-arkiv som bör bifogas tillsammans med den här inlämningen, dom finns bara här utifall att detta arkiv inte har bifogats.
