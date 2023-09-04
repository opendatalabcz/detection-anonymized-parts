[![codecov](https://codecov.io/gh/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/branch/master/graph/badge.svg?token=C7H0CZLJZU)](https://codecov.io/gh/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis)
![Tests](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/actions/workflows/test_api.yml/badge.svg)
![Build .NET](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/actions/workflows/build_api.yml/badge.svg)
![Build and Verify PDF](https://github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis/actions/workflows/build_thesis.yml/badge.svg)
# Špecifikácia pre projekt: Detekcia anonymizovaných častí v PDF dokumentoch

## Účel a popis

- Názov softvérového diela: DAPP (**D**etector of **A**nonymized **P**arts in **P**DFs)
- Popis: DAPP je nástroj na analýzu PDF dokumentov s cieľom detekcie anonymizovaných častí. Je navrhnutý ako webová služba, ktorá prijíma vstupné dáta vo forme HTTP request s lokálnymi cestami alebo URL na PDF súbory a vráti výsledky analýzy vo formáte JSON.

## Obsah
- [Účel a popis](#účel-a-popis)
- [Obsah](#obsah)
- [Použitie](#použitie)
    - [Predpoklady](#predpoklady)
    - [Inštalácia](#inštalácia)

        -[Možnosti Príkazového riadku](#možnosti-príkazového-riadku)

        -[Príkladové príkazy](#príkladové-príkazy)

        -[Výstup](#výstup)
- [Cieľ a požiadavky](#cieľ-a-požiadavky)
	- [Funkčné požiadavky](#funkčné-požiadavky)
    - [Technické požiadavky](#technické-požiadavky)
- [Architektúra a dizajn](#architektúra-a-dizajn)
    - [Technológie a nástroje](#technológie-a-nástroje)
    - [Dátový model](#dátový-model)
    - [Užívateľské rozhranie](#užívateľské-rozhranie)
    - [Algoritmy a postupy](#algoritmy-a-postupy)
- [Testovanie a validácia](#testovanie-a-validácia)
    - [Plán testovania](#plán-testovania)
    - [Validácia výsledkov](#validácia-výsledkov)
- [Časový plán](#časový-plán)
    - [Fázy vývoja](#fázy-vývoja)
    - [Odhadované termíny milníkov](#odhadované-termíny-milníkov)
- [Konfigurácia (verzovanie)](#konfigurácia-verzovanie)
- [Predpoklady](#predpoklady)
- [Obmedzenia](#obmedzenia)

## Použitie
### Predpoklady

- Nainštalujte .NET 7.0 alebo vyšší
- Nainštalujte program imagemagick (https://imagemagick.org/)

## Inštalácia

1. Naklonujte repozitár.
   ```bash
   git clone github.com/Oranged9922/detection-anonymized-parts-in-pdfs-bachelor-thesis.git
   ```
2. Prejdite do priečinka projektu a zostavte riešenie.
   ```bash
   cd implementation/Dapp
   dotnet build
   ```
Spustite konzolovú aplikáciu (V priečinku ConsoleApp) s nasledujúcimi príkazovými možnosťami:

### Možnosti Príkazového Riadku

- `--file-location` *(povinné)*: Cesta k súboru, ktorý sa má analyzovať.
- `--return-images` *(nepovinné, predvolené=false)*: Zadajte `true`, ak chcete vrátiť obrázky.
- `--output-folder` *(nepovinné)*: Adresár, kam sa uložia obrázky. Ak nie je špecifikovaný, obrázky sa neuložia.

### Príkladové Príkazy

Analýza dokumentu bez vrátenia obrázkov:
```bash
dotnet run -- --file-location /cesta/k/súboru
```

Analýza dokumentu a vrátenie obrázkov, ale neuloženie ich:
```bash
dotnet run -- --file-location /cesta/k/súboru --return-images true
```

Analýza dokumentu, vrátenie a uloženie obrázkov:
```bash
dotnet run -- --file-location /cesta/k/súboru --return-images true --output-folder /cesta/k/výstupnému/priečinku
```

## Výstup

Konzola vypíše JSON dáta prijaté z API, vrátane ID dokumentu. Ak je špecifikovaný výstupný priečinok, obrázky budú uložené vo formáte `original_{i}.jpg` a `result_{i}.jpg`.

To je všetko. Postupujte podľa krokov, aby ste efektívne využili konzolovú aplikáciu.

## Cieľ a požiadavky
Cieľom tohto softvérového diela je vyvinúť a implementovať softvér, ktorý je schopný detekovať anonymizované časti v PDF dokumentoch. Softvér bude písaný v jazyku C# s použitím minimal API rozhrania. Výstup bude v podobe JSON formátu obsahujúceho údaje o analyzovanom PDF, ako je počet strán, percento anonymizácie na každej stránke a celková priemerná anonymizácia.

### Funkčné požiadavky

- Softvér musí byť schopný prijať HTTP request obsahujúci URL odkaz na PDF súbor alebo lokálnu cestu k súboru.
- Softvér musí byť schopný prečítať a spracovať PDF súbory.
- Softvér musí byť schopný detekovať anonymizované časti v PDF dokumentoch.
- Softvér musí byť schopný analyzovať a vypočítať percento anonymizácie na každej stránke PDF súboru a celkovú priemernú anonymizáciu.
- Softvér musí byť schopný vrátiť výsledky v špecifikovanom JSON formáte obsahujúcom počet strán, percento anonymizácie na každej stránke a celkovú priemernú anonymizáciu.
- Softvér musí byť dodaný s príslušnou dokumentáciou, užívateľskou aj vývojárskou

### Technické požiadavky

- Softvér musí byť napísaný v jazyku C#.
- Softvér bude používať minimal API rozhranie pre prijímanie requestov a vracanie výsledkov.
- Softvér musí byť kompatibilný s najnovšou verziou .NET platformy (.NET 7).
- Softvér musí podporovať spracovanie PDF súborov.
- Softvér musí podporovať formát JSON pre výstupné dáta.

## Architektúra a dizajn
### Architektúra systému:
- Klient-Server: Klient posiela požiadavky na server, ktorý spracúva PDF súbory a vracia výsledky vo formáte JSON.

### Technológie a nástroje:
- Jazyk: C#
- Framework: .NET 7
- API: Minimal API
- Spracovanie PDF: Vlastná implementácia analyzátoru pre prácu s PDF v jazyku C# 

### Dátové modely:
- Vstup: URL alebo lokálna cesta k PDF súboru
- Výstup: JSON obsahujúci informácie o počte strán, percentuálnej anonymizácii na jednotlivých stranách a celkovú priemernú anonymizáciu

### Užívateľské rozhranie:
- Webové API: Užívatelia interagujú so softvérom prostredníctvom HTTP požiadavkov

### Algoritmy a postupy:
- Vlastná implementácia algoritmu na detegovanie anonymizovaných častí v PDF súboroch (detailný popis bude doložený pri odovzdaní)

## Testovanie a validácia
### Plán testovania
- Unit testy: Overenie správnej funkcionality jendotlivých komponent
- Integračné testy: Overenie správnej interakcie medzi jednotlivými komponentami

### Validácia výsledkov
#### Testovacie dáta
- Objem testovacích PDF súborov poskytnutý zo stránok https://www.hlidacstatu.cz/
#### Validačné kritéria
- Správna detekcia anonymizovaných častí
- Správny odhad na percentuálne vyhodnotenie anonymizovania v dokumente
#### Postup validácie
- Porovnanie výsledkov s očakávanými hodnotami pre dané testovacie dáta

## Časový plán
### Odhadované termíny milníkov
- Dodanie špecifikácie: 25. 05. 2023
- Dokončenie projektu: 10. 07. 2023

## Konfigurácia (verzovanie)
- Verzovanie softvéru: Verzovanie softvéru bude zabezpečené pomocou verzovacieho systému git

## Predpoklady

- Predpokladá sa, že užívateľ poskytne platnú URL alebo lokálnu cestu k PDF súboru.
- Predpokladá sa, že vstupné PDF súbory nebudú obsahovať žiadne formy šifrovania alebo ochrany, ktoré by mohli obmedziť ich spracovanie.

## Obmedzenia

- Softvér musí byť v súlade so všetkými relevantnými zákonmi a predpismi týkajúcimi sa ochrany dát a súkromia.
- Softvér nemôže byť použitý na nelegálne účely alebo k porušovaniu ochrany dát.
- Softvér musí byť schopný efektívne spracovať a analyzovať PDF súbory, a to aj pri veľkých objemoch dát.
- Softvér musí byť navrhnutý s ohľadom na bezpečnosť a odolnosť voči rôznym typom útokov.