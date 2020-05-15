# Relazione (Alessandro Pistola: 285802)

## Specifica del software
Il progetto consiste nella realizzazione di un software C# per la modifica automatizzata dei frame di un timelapse.

Il software sarà in grado di analizzare le singole immagini RAW (tempo di scatto, diaframma e ISO), che compongono il timelapse, dai file contenenti i metadati (*.xmp) e automaticamente calcolare la giusta esposizione al fine di rendere fluide le transizioni di luce nel timelapse.

Il programma offre inoltre la possibilità di aggiungere una vignettatura al timelapse specificando l'intensità di quest'ultima (1-5) e poter applicare uno dei 3 preset di default. Una volta calcolati i giusti valori di esposizione ed apportate le modifiche richieste il programma salverà tali valori nei file contenenti i metadati.

## Studio del problema
L'algoritmo alla base deve:
- Estrarre i dati Exif (tempo di scatto, diaframma e valore ISO) dalla foto.
- Analizzare i valori dell'esposizione:
  - Ogni immagine della sequenza viene confrontata con la successiva.
  - Per ogni dato exif, se è diverso dal corrispettivo della successiva, vengono calcolati gli stop di differenza e sommati insieme. Il valore di incremento viene calcolato unitariamente (tot. stop di differenza / numero delle immagini coinvolte nella modifica).
  - Ogni modifica comprende tutte le immagini dall'immagine attuale all'ultima immagine modificata.
  - Il valore di incremento di esposizione di ogni immagine è quindi *nuovo_valore = val_iniziale + (incremento * (pos_attuale - pos_iniziale))* dove (pos_attuale - pos_iniziale) rappresenta la posizione relativa della foto nella lista delle foto coinvolte nella modifica.
  - Salvataggio dei nuovi valori.
 - Aggiunta e salvataggio di eventuali vignettature o preset (una esclude l'altra poichè solitamente i preset includono una vignettatura).
 
 In fase di progettazione si è voluto costruire una struttura fortemente indipendente dal tipo di tag specifici (utilizzati da vendor come Photoshop) quindi, anche se la struttura implementata attualmente prevede solamente l'apporto di modifiche compatibili con gli standard xmp di Camera Raw, i design patterns utilizzati e l'architettura progettuale sono fortemente aperte all'aggiunta di altri Adapter (componenti che standardizzano l'accesso al file xmp) che consentono di operare su tag diversi, basta specifica delle regole di traduzione appunto nell'adapter (Informazioni più dettagliate riguardo alla contribuzione sono presenti nel [readme](https://github.com/alepistola/TimelapseEditor/) del progetto).
 
 Partendo dal basso, i **punti critici** sono: 
 - l'accesso al file e la lettura di dati exif; 
 - la crezione di un'interfaccia comune per standardizzare i valori da modificare nel file xmp;
 - la modellizzazione delle possibili modifiche apportabili ai file;
 - la creazione di un sistema che permette di comporre e combinare i 3 tipi di modifiche disponibili, in modo sicuro, generando il Timelapse.
 
 **Soluzioni adottate**
 
 L'accesso fisico al file avviene dentro la classe XmpFile dove non sono presenti i concetti rappresentati dai tag (esposizione, contrasto, saturazione), ma ogni tag rappresenta solamente una coppia di valori di tipo stringa. Per la lettura di dati exif (avviene concretamente sulla foto) si è scelto di avvalersi del pacchetto [metadata-extractor](https://github.com/drewnoakes/metadata-extractor-dotnet).
 
 I concetti relativi ai tag vengono codificati e decodificati da una classe concreta che deve implementare l'interfaccia IAdapter che appunto 'adatta' l'interfaccia utilizzata dalle modifiche (che lavorano sui dati exif e sul valore di esposizione della foto) a quella della classe XmpFile avente il solo compito di leggere e scrivere stringhe nel file. Attualmente l'unica classe concreta che implementa l'interfaccia IAdapter è la classe CameraRawXmpAdapter che converte concetti come Esposizione, contrasto ecc. in tag validi specificati dallo [standard del tag crs di XMP](https://exiftool.org/TagNames/XMP.html#crs).
 
 Durante la progettazione è emerso che l'applicazione di modifiche all'esposizione, alla vignettatura o l'aggiunta di un preset potevano essere generalizzate in un classe che comprendesse i valori comuni di quest'ultime come:
 - sequenza di immagini su cui operare;
 - numero dell'immagine da cui la modifica inizia;
 - numero dell'ultima immagine a cui le modifiche vanno applicate;
 - metodo di salvataggio sulle singole immagini;
 Tale classe è la classe Change da cui derivano le classi: ExposureChange, VignetteChange e PresetChange utilizzate dall'oggetto Timelapse.
 
 La modellizzazione delle possibili modifiche è stata ottenuta attraverso l'utilizzo del pattern builder. La classe director si occupa di gestire le varie configurazioni evitando combinazioni non permesse come l'aggiunta di un preset e l'aggiunta di una vignettatura.
 
 ## Scelte architetturali
 ### Diagramme delle classi
 Di seguito il [link](https://github.com/alepistola/TimelapseEditor/tree/master/Result/class-uml.png) all'immagine dello schema non ridimensionata.
 
 ![uml-diagramClass](https://github.com/alepistola/TimelapseEditor/tree/master/Result/class-uml-resized.png)
 ### Descrizione e motivazione dei design pattern utilizzati
 Sono stati utilizzati i seguenti design pattern:
 - **Adapter pattern**: converte l'interfaccia della classe XmpFile, utilizzata per leggere e scrivere coppie di chiavi-valori nel file fisico xmp e per leggere i dati exif dalla foto, in un'interfaccia standard che, per determinati valori, come l'esposizione, effettua il "parsing" in lettura e l'"encoding" in scrittura così che il client possa rimanere all'oscuro dell'implementazione fisica. Essendo i metodi di scrittura e lettura sul file, implementati nella classa XmpFile, indipendenti dagli specifici tag xmp che si vogliono scrivere, l'adapter pattern è stato utilizzato per riutilizzare la classe XmpFile con diversi oggetti (che dovranno implementare l'interfaccia IAdapter).
 - **Proxy pattern**: fornisce un segnaposto con il quale è possibile controllare l'accesso all'oggetto originale, in questo caso di tipo IAdapter. In questo modo, applicando il proxy pattern ad oggetti di tipo IAdapter è possibile generare un surrogato di un adapter indipendentemente dalle tipologie di tag xmp che quest'ultimo codifica e decodifica, senza dover aggiungere un proxy specifico per ogni adapter. Più nello specifico, il proxy si comporta come uno *smart proxy*, ovvero, permette il *lazy loading* delle risorse (del file fisico xmp), istanziando l'oggetto solo quando viene effettivamente richiesto ed effettua una sorta di caching dei valori già richiesti al file, limitando gli accessi sia in lettura che in scrittura.
 - **Builder pattern**: permette la creazione modulare dell'oggetto complesso Timelapse in modo indipendente dalla sua composizione. In questo modo è possibile creare diverse configurazioni (in questo caso scelte dall'utente) del prodotto finale Timelapse. Ad ognuna delle scelte possibili dell'utente (semplice stabilizzazione dell'esposizione, stabilizzazione dell'esposizione + applicazione della vignettatura oppure stabilizzazione dell'esposizione + applicazione di un preset) corrisponde una specifica configurazione all'interno della classe Director.
 ## Documentazione sull'utilizzo
 ### Come scattare le foto
- Imposta la giusta esposizione ed inizia a scattare le foto finchè le immagini non risulteranno sotto-esposte o sovra-esposte
- Ricalcola la giusta esposizione e continua a scattare. Quando avrai terminato la sequenza di foto che compongono il timelapse, non preoccuparti se le foto risulteranno non omogenee con improvvisi aumenti o diminuzioni dell'esposizione, il compito del programma è proprio questo, cercare di rendere fluide queste transizioni di luce.

### Come utilizzare il programma
- Scaricare la release più recente (ed anche la cartella 'Presets' nel caso si vogliano utilizzare i preset forniti di default)
- Doppio-click sul file eseguibile (assicurandosi di eseguirlo nella stessa directory di dove è stata archiviata la cartella 'Presets')
- Seguire le istruzioni del programma
- Se l'esecuzione è andata a buon fine sarà stato creato un file con estensione 'xmp' per ogni foto che il software ha analizzato
- Aprire Photoshop Lightroom o After Effects. Nel caso abbiate scelto di utilizzare Lightroom, sarà sufficiente importare le foto, selezionarle tutte, premere il tasto destro e selezionare "metadati -> leggi metadati da file" a questo punto, le foto modificate possono essere esportate senza alcuna restrizione. Nel caso si scelga di importarle direttamente in After Effects, le immagini importate come sequenza saranno automaticamente linkate al loro file contenente i metadati e l'eventuale composizione generata dalle foto apparirà già modificata.
 ## Use cases
