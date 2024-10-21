# Drifta molnapplikationer - Inlämning 2

av Erika Bladh-Öström

## Kluster-komponenter

Ett kubernets kluster består dels av ett _control plane_ och av en eller flera _worker nodes_, dessa två delar innehåller olika komponenter. En beskrivning av dessa komponenter finns i [kuberentes dokumentationen](https://kubernetes.io/docs/concepts/overview/components/).

### Control plane

#### kube-apiserver

Den centrala komponent som tillgängliggör klustrets HTTP-baserade programmeringsgränssnitt, det är mot detta gränssnitt som vi interagerar med klustret. Denna komponent går att skala horizontellt vid behov.

#### etcd

Är en databas med nyckel-värde-par av klusterdata, jag tänker att dessa kan ses lite som _klustrets miljövariabler_.

#### kube-scheduler

Komponent som kollar efter hemlösa poddar och säger vid behov åt tillgängliga _worker nodes_ att ge dom ett hem.

#### kube-controller-manager

Komponent som hanterar många olika typer av _controllers_ och som försöker genomföra olika processer för att uppnå det önskade tillståndet inom klustret. Exempel på _controllers_ är bland andra: 

- Node Controller, som är ansvarig för att hålla koll på tillgängliga _nodes_ och agera om något skulle gå fel med dom.
- Job Controller, som är ansvarig för att dom engångsjobb som tilldelas klustret genomförs.

#### cloud-controller-manager (optional)

Komponent som lägger till cloudspecifika funktioner som är kopplade till molnleverantörens API:er och tjänster. Detta finns inte tillgängligt när vi kör ett kluster lokalt på våra egna maskiner.

### Worker node

#### kubelet

Den komponent som ser till att containrar körs som pods och som håller koll på att dom beter sig korrekt.

#### Container runtime

Komponent som ansvarar för att kubernetes kan köra containrar effektivt och som hanterar deras livscykler i klustret.

#### kube-proxy (optional)

Komponent som ansvarar för att nätverksanslutningarna till _worker nodes_ och som ser till att _service_ koncepten inom kuberenets fungerar.

### Tillägg

Utöver dessa grundläggande komponenter så finns det andra komponenter som kan utöka funktionaliteten med till exempel domännamnssystem som kan användas innuti klustret eller verktyg centraliserad loggning och övervakning av resurs användning.

## Kubernetes objekt



## Verktygslådan

## Driftsättning
