# MusicService - Dokumentacja

## Opis
`MusicService` to singleton service do zarządzania muzyką w grze. Service automatycznie ładuje pliki audio z folderu `Resources/Music` i losowo odtwarza je w pętli.

## Funkcjonalności
- ✅ Singleton pattern - nie niszczony przy zmianie sceny
- ✅ Automatyczne ładowanie muzyki z Resources/Music
- ✅ Losowe odtwarzanie utworów
- ✅ Kontrola głośności
- ✅ Start/Stop/Pause/Resume
- ✅ Automatyczne przejście do następnego utworu
- ✅ Eventy do reagowania na zmiany

## Instalacja

### 1. Przygotowanie plików muzycznych
Umieść pliki audio w folderze `Assets/Resources/Music/`:
```
Assets/
  Resources/
    Music/
      track1.mp3
      track2.ogg
      track3.wav
      ...
```

### 2. Użycie w kodzie

#### Podstawowe użycie:
```csharp
// Pobierz instancję
MusicService musicService = MusicService.Instance;

// Rozpocznij muzykę
musicService.StartMusic();

// Zatrzymaj muzykę
musicService.StopMusic();

// Ustaw głośność (0.0f - 1.0f)
musicService.SetVolume(0.5f);
```

#### Z eventami:
```csharp
private void Start()
{
    MusicService musicService = MusicService.Instance;
    
    // Podłącz eventy
    musicService.OnMusicChanged += OnMusicChanged;
    musicService.OnPlaybackStateChanged += OnPlaybackStateChanged;
    musicService.OnVolumeChanged += OnVolumeChanged;
}

private void OnMusicChanged(AudioClip clip)
{
    Debug.Log($"Nowy utwór: {clip.name}");
}

private void OnPlaybackStateChanged(bool isPlaying)
{
    Debug.Log($"Status odtwarzania: {isPlaying}");
}

private void OnVolumeChanged(float volume)
{
    Debug.Log($"Nowa głośność: {volume}");
}
```

## API Reference

### Metody publiczne

#### `StartMusic()`
Rozpoczyna odtwarzanie muzyki z losowym utworem.

#### `StopMusic()`
Zatrzymuje odtwarzanie muzyki.

#### `PauseMusic()`
Pauzuje odtwarzanie muzyki.

#### `ResumeMusic()`
Wznawia odtwarzanie muzyki.

#### `SetVolume(float volume)`
Ustawia głośność muzyki (0.0f - 1.0f).

#### `GetVolume()`
Zwraca aktualną głośność.

#### `IsPlaying()`
Sprawdza czy muzyka jest aktualnie odtwarzana.

#### `GetCurrentTrack()`
Zwraca aktualnie odtwarzany utwór.

#### `GetTrackCount()`
Zwraca liczbę dostępnych utworów.

### Eventy

#### `OnMusicChanged(AudioClip clip)`
Wywoływany gdy zmienia się odtwarzany utwór.

#### `OnPlaybackStateChanged(bool isPlaying)`
Wywoływany gdy zmienia się stan odtwarzania.

#### `OnVolumeChanged(float volume)`
Wywoływany gdy zmienia się głośność.

## Konfiguracja

### Inspector Settings
W komponencie MusicService możesz ustawić:
- **Default Volume**: Domyślna głośność (0.0f - 1.0f)
- **Music Folder Path**: Ścieżka do folderu z muzyką w Resources (domyślnie "Music")

## Przykłady użycia

### W GameManager
```csharp
public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // Rozpocznij muzykę na początku gry
        MusicService.Instance.StartMusic();
    }
    
    private void OnGameEnd()
    {
        // Zatrzymaj muzykę na końcu gry
        MusicService.Instance.StopMusic();
    }
}
```

### W UI Controller
```csharp
public class MusicUIController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    
    private void Start()
    {
        volumeSlider.value = MusicService.Instance.GetVolume();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }
    
    private void OnVolumeChanged(float value)
    {
        MusicService.Instance.SetVolume(value);
    }
}
```

## Uwagi techniczne

1. **Singleton Pattern**: Service używa wzorca singleton z `DontDestroyOnLoad`, więc nie będzie niszczony przy zmianie sceny.

2. **Resources Loading**: Muzyka jest ładowana z folderu `Resources/Music` przy starcie.

3. **Automatic Playlist**: Po zakończeniu utworu automatycznie przechodzi do następnego losowego utworu.

4. **Memory Management**: AudioClips są ładowane do pamięci przy starcie i pozostają tam przez cały czas działania aplikacji.

## Rozwiązywanie problemów

### Brak muzyki
- Sprawdź czy pliki audio są w folderze `Assets/Resources/Music/`
- Sprawdź czy pliki mają poprawny format (mp3, ogg, wav)
- Sprawdź Console logi

### Muzyka nie odtwarza się
- Sprawdź czy AudioSource jest poprawnie skonfigurowany
- Sprawdź czy głośność systemowa jest włączona
- Sprawdź czy nie ma błędów w Console

### Problemy z wydajnością
- Używaj kompresji audio dla plików muzycznych
- Rozważ streaming dla bardzo dużych plików
- Monitoruj użycie pamięci
