# Splendor Game - Struktura Kodu

## Przegląd Architektury

Gra mobilna inspirowana planszową grą Splendor, zaimplementowana w Unity z wykorzystaniem wzorca MVP (Model-View-Presenter) i biblioteki R3 do programowania reaktywnego.

## Struktura Folderów

```
Assets/Scripts/
├── Core/
│   └── MVP/                    # Podstawowe interfejsy i klasy MVP
│       ├── IModel.cs          # Interfejs bazowy dla modeli
│       ├── IView.cs           # Interfejs bazowy dla widoków
│       ├── IPresenter.cs      # Interfejs bazowy dla prezenterów
│       └── BasePresenter.cs   # Bazowa klasa abstrakcyjna prezentera
├── Game/
│   ├── Data/                  # Struktury danych i typy
│   │   ├── ResourceType.cs    # Enum typów zasobów
│   │   ├── ResourceCost.cs    # Klasa kosztów zasobów
│   │   └── CardData.cs        # ScriptableObject dla kart
│   ├── Models/                # Modele gry (logika biznesowa)
│   │   ├── PlayerModel.cs     # Model gracza
│   │   └── GameStateModel.cs  # Model stanu gry
│   ├── Views/                 # Widoki UI
│   │   ├── CardView.cs        # Widok karty
│   │   ├── PlayerView.cs      # Widok gracza
│   │   └── GameBoardView.cs   # Główny widok planszy
│   ├── Presenters/            # Prezenterzy (łączą Model z View)
│   │   ├── PlayerPresenter.cs # Prezenter gracza
│   │   └── GamePresenter.cs   # Główny prezenter gry
│   └── GameController.cs      # Główny kontroler gry
├── Network/                   # Funkcjonalność multiplayer
│   └── NetworkGameController.cs # Kontroler gry sieciowej
└── README.md                  # Ta dokumentacja
```

## Kluczowe Komponenty

### 1. Wzorzec MVP

#### Model (IModel)
- Zawiera logikę biznesową i stan gry
- Używa R3 do reaktywnych właściwości
- Niezależny od Unity UI

#### View (IView)  
- Odpowiada za wyświetlanie danych
- Reaguje na zmiany w modelu
- Zawiera komponenty Unity UI

#### Presenter (IPresenter)
- Łączy Model z View
- Zarządza przepływem danych
- Obsługuje interakcje użytkownika

### 2. Modele Gry

#### PlayerModel
- Zasoby gracza (diamenty, szafiry, szmaragdy, rubiny, onyksy)
- Złote żetony (jokers)
- Zakupione karty i bonusy
- Zarezerwowane karty
- Punkty i nazwa gracza

#### GameStateModel
- Faza gry (oczekiwanie, rozgrywka, koniec)
- Gracze i aktualny gracz
- Dostępne karty na planszy (3 poziomy)
- Pula żetonów
- Logika ruchów (branie żetonów, kupowanie, rezerwacja)

### 3. Widoki

#### CardView
- Wyświetla informacje o karcie
- Koszty w zasobach
- Punkty i poziom
- Przyciski interakcji

#### PlayerView
- Zasoby i bonusy gracza
- Wynik i nazwa
- Zakupione i zarezerwowane karty
- Wskaźnik aktualnego gracza

#### GameBoardView
- Dostępne karty na stole
- Pula żetonów
- Selekcja żetonów
- Informacje o turze

### 4. Reaktywność z R3

Wszystkie modele używają reaktywnych właściwości R3:
- `ReactiveProperty<T>` - dla pojedynczych wartości
- `ReactiveCollection<T>` - dla kolekcji
- `ReadOnlyReactiveProperty<T>` - dla publicznego dostępu tylko do odczytu
- `Observable` - dla eventów

#### Przykład użycia:
```csharp
// W modelu
private readonly ReactiveProperty<int> _score = new(0);
public ReadOnlyReactiveProperty<int> Score => _score;

// W prezenterze
Model.Score
    .Subscribe(score => View.SetScore(score))
    .AddTo(_disposables);
```

### 5. Przygotowanie do Multiplayer

#### NetworkGameController
- Rozszerza GameController
- Przygotowany pod integrację z Photon Realtime
- Metody sieciowe (placeholder)
- Obsługa pokoi i graczy

## Przepływ Gry

1. **Inicjalizacja**
   - GameController tworzy GameStateModel
   - Tworzy GamePresenter łączący model z widokiem
   - Dodaje graczy

2. **Rozgrywka**
   - Model zarządza fazami gry
   - Prezenter reaguje na zmiany modelu
   - Widok aktualizuje UI
   - Interakcje użytkownika trafiają do prezentera

3. **Akcje Gracza**
   - Branie żetonów (2 tego samego lub 3 różne)
   - Kupowanie kart (płacenie zasobami + złoto)
   - Rezerwacja kart (max 3)

4. **Koniec Gry**
   - Gdy gracz osiągnie 15 punktów
   - Wygrywający ma najwyższy wynik

## Rozszerzalność

### Dodawanie Nowych Funkcji
1. **Nowy typ zasobu**: Dodaj do `ResourceType` enum
2. **Nowa mechanika**: Dodaj do odpowiedniego modelu
3. **Nowy widok**: Implementuj `IView` i stwórz prezenter

### Integracja z Photon
1. Dodaj Photon Realtime do projektu
2. Zaimplementuj metody w `NetworkGameController`
3. Dodaj serializację stanów gry
4. Obsługa synchronizacji

## Zależności

- **Unity 2022.3+**
- **R3** - Programowanie reaktywne
- **DOTween** - Animacje (już w projekcie)
- **TextMeshPro** - Tekst UI
- **Photon Realtime** - Multiplayer (do dodania)

## Następne Kroki

1. **Utworzenie prefabów UI** na podstawie widoków
2. **Dodanie animacji** z DOTween
3. **Integracja z Photon** dla multiplayer
4. **Testy jednostkowe** modeli
5. **Optymalizacja mobilna**

## Przykład Użycia

```csharp
// Tworzenie gry
var gameModel = new GameStateModel();
var gameView = FindObjectOfType<GameBoardView>();
var gamePresenter = new GamePresenter(gameModel, gameView, cards1, cards2, cards3);

// Dodawanie graczy
gamePresenter.AddPlayer("Gracz 1");
gamePresenter.AddPlayer("Gracz 2");

// Start gry
gamePresenter.StartGame();
```

Struktura jest przygotowana na łatwe rozszerzanie i utrzymanie, z jasnym podziałem odpowiedzialności między komponenty. 