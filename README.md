# Alessandro Pistola - 285802
Progetto per il corso:

**Programmazione e modellazione a oggetti**


## TimelapseEditor - status

[![GitHub license](https://img.shields.io/github/license/alepistola/TimelapseEditor)](https://raw.githubusercontent.com/alepistola/TimelapseEditor/master/LICENSE)
[![GitHub repo size](https://img.shields.io/github/repo-size/alepistola/TimelapseEditor)](https://github.com/alepistola/TimelapseEditor)
[![Github issues](https://img.shields.io/github/issues/alepistola/TimelapseEditor)](https://github.com/alepistola/TimelapseEditor/issues)

## Specifica
Il progetto consiste nella realizzazione di un software C# per la modifica automatizzata dei frame di un timelapse.

Il software sarà in grado di analizzare le singole immagini RAW (tempo di scatto, diaframma e ISO), che compongono il timelapse, dai file contenenti i metadati (*.xmp) e automaticamente calcolare la giusta esposizione al fine di rendere fluide le transizioni di luce nel timelapse.

Il programma offre inoltre la possibilità di aggiungere una vignettatura al timelapse specificando l'intensità di quest'ultima (1-5) e poter applicare uno dei 3 preset di default.
Una volta calcolati i giusti valori di esposizione ed apportate le modifiche richieste il programma salverà tali valori nei file contenenti i metadati.

Link alla [relazione](https://github.com/alepistola/TimelapseEditor/tree/master/Result/relazione.md)

## Risultati
### Originale - Stabilizzato
![examples](https://github.com/alepistola/TimelapseEditor/blob/master/Result/Comp-1.gif)
### Originale - Stabilizzato con vignettatura
![examples](https://github.com/alepistola/TimelapseEditor/blob/master/Result/Comp2.gif)
### 3 tipologie di preset (Fairy green - Mount Dramatic - WalPyne)
![examples](https://github.com/alepistola/TimelapseEditor/blob/master/Result/Comp3.gif)

## Come scattare le foto
- Imposta la giusta esposizione ed inizia a scattare le foto finchè le immagini non risulteranno sotto-esposte o sovra-esposte
- Ricalcola la giusta esposizione e continua a scattare. Quando avrai terminato la sequenza di foto che compongono il timelapse, non preoccuparti se le foto risulteranno non omogenee con improvvisi aumenti o diminuzioni dell'esposizione, il compito del programma è proprio questo, cercare di rendere fluide queste transizioni di luce.


## Come utilizzare il programma
- Scaricare la release più recente (ed anche la cartella 'Presets' nel caso si vogliano utilizzare i preset forniti di default)
- Doppio-click sul file eseguibile (assicurandosi di eseguirlo nella stessa directory di dove è stata archiviata la cartella 'Presets')
- Seguire le istruzioni del programma
- Se l'esecuzione è andata a buon fine sarà stato creato un file con estensione 'xmp' per ogni foto che il software ha analizzato
- Aprire Photoshop Lightroom o After Effects. Nel caso abbiate scelto di utilizzare Lightroom, sarà sufficiente importare le foto, selezionarle tutte, premere il tasto destro e selezionare "metadati -> leggi metadati da file" a questo punto, le foto modificate possono essere esportate senza alcuna restrizione. Nel caso si scelga di importarle direttamente in After Effects, le immagini importate come sequenza saranno automaticamente linkate al loro file contenente i metadati e l'eventuale composizione generata dalle foto apparirà già modificata.

## Come contribuire
### Aggiungere un Adapter
Attualmente il software gestisce solamente tag xmp di tipo "crs" utilizzati per l'appunto da Camera Raw. Se si vuole aggiungere un supporto ad altre modifiche con altri tag è sufficiente creare una nuova classe concreta che implementi l'interfaccia IAdapter e IAdapterProxy (dove, banalmente, i metodi implementati richiameranno quelli dell'interfaccia IAdapter che si occupano dell'accesso fisico).
### Aggiungere nuovi tipi di modifica
Il software allo stato attuale è in grado di modificare esposizione, vignettatura e di applicare un preset, qualora si volesse aggiungere altre tipologie di modifica è sufficiente creare una classe derivata dalla classe Change e aggiungere un metodo per effettuare il salvataggio all'interfaccia ITimelapseBuilder che poi dovrà essere concretizzato nelle classi TimelapseBuilder e Timelapse. Fatto ciò, se si vogliono creare nuove forme di modifiche composte è sufficiente specificarle all'interno della classe Director.
### Calcolare la temperatura dagli exif
La lettura dei metadati avviene attraverso la libreria [metadata-extractor](https://github.com/drewnoakes/metadata-extractor-dotnet) ma al momento non c'è un modo per ottenere temperatura e tinta delle foto, questo rende impossibile l'automatizzazione del "ramping" per quanto riguarda il bilanciamento del bianco.
