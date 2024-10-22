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

### kubectl

Det primära verktyget som används för att agera mot ett kluster, det ansluter alltså till _kube-apiserver_ för att åstadkomma det vi säger åt det att göra. Några användbara kommandon är:

- `kubectl --help`
- `kubectl <command> --help`
- `kubectl options`

### helm

Ett verktyg som kombinerar flera olika beskrivna kubernetes objekt och sedan kör upp dessa tillsammans (vilket kallas för en _chart_). Det går även att separera ut delar som variabler för att kunna köra upp saker med olika val beroende på i vilken miljö en arbetar mot (t.ex. att använda nodeport när det körs lokalt men använda loadbalancer när det körs på en molnplattform som har stöd för det). Några använbara kommandon är:

- `helm --help`
- `helm <command> --help`

Helm verkar inte komma automatiskt med autocompletion men har en instruktion som kan genererar kod som åstadkommer det, så här fick jag det att samarbeta med bash:

```bash
# Skriv output from helm till en av konfigurationsfilerna för bash, 
# t.ex. .bashrc (eller till någon annan fil som .bashrc inkluderar).
helm completion bash > ~/.bashrc
# Läs in förändringen för att använda completion
. ~/.bashrc
```

## Driftsättning

### Utgångspunkt

### Verktyg

Min känsla är att det har blivit fler och fler verktyg, för den här uppgiften så har jag använt några av dom vanligt förekommande (VS-Code, git & github, docker desktop & cli) utöver dessa så kommer jag också att luta mig mot:

- kind ([Kuberenetes IN Docker](https://github.com/kubernetes-sigs/kind))
- kubectl
- helm
- argocd

### Genomförande

#### Hitta ett kluster att arbeta mot



### Reflektioner
