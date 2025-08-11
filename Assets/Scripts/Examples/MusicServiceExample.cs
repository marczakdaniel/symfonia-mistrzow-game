using UnityEngine;
using Services;
using UnityEngine.UI;

namespace Examples
{
    /// <summary>
    /// Przykład użycia MusicService w grze
    /// Możesz dodać ten skrypt do GameObject w scenie, aby przetestować funkcjonalność
    /// </summary>
    public class MusicServiceExample : MonoBehaviour
    {
        [Header("UI Controls")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button stopButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Text currentTrackText;
        [SerializeField] private Text statusText;

        private MusicService musicService;

        private void Start()
        {
            // Pobierz instancję MusicService
            musicService = MusicService.Instance;
            
            // Podłącz eventy
            musicService.OnMusicChanged += OnMusicChanged;
            musicService.OnPlaybackStateChanged += OnPlaybackStateChanged;
            musicService.OnVolumeChanged += OnVolumeChanged;

            // Ustaw początkowe wartości UI
            if (volumeSlider != null)
            {
                volumeSlider.value = musicService.GetVolume();
                volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
            }

            UpdateUI();
        }

        private void OnDestroy()
        {
            if (musicService != null)
            {
                musicService.OnMusicChanged -= OnMusicChanged;
                musicService.OnPlaybackStateChanged -= OnPlaybackStateChanged;
                musicService.OnVolumeChanged -= OnVolumeChanged;
            }
        }

        // Metody wywoływane przez przyciski UI
        public void OnPlayButtonClicked()
        {
            musicService.StartMusic();
        }

        public void OnStopButtonClicked()
        {
            musicService.StopMusic();
        }

        public void OnPauseButtonClicked()
        {
            musicService.PauseMusic();
        }

        public void OnResumeButtonClicked()
        {
            musicService.ResumeMusic();
        }

        private void OnVolumeSliderChanged(float value)
        {
            musicService.SetVolume(value);
        }

        // Event handlers
        private void OnMusicChanged(AudioClip clip)
        {
            if (currentTrackText != null)
            {
                currentTrackText.text = $"Aktualny utwór: {clip.name}";
            }
            UpdateUI();
        }

        private void OnPlaybackStateChanged(bool isPlaying)
        {
            UpdateUI();
        }

        private void OnVolumeChanged(float volume)
        {
            if (volumeSlider != null)
            {
                volumeSlider.value = volume;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (statusText != null)
            {
                string status = musicService.IsPlaying() ? "Odtwarzanie" : "Zatrzymane";
                statusText.text = $"Status: {status} | Głośność: {musicService.GetVolume():F2} | Utworów: {musicService.GetTrackCount()}";
            }

            // Aktualizuj stan przycisków
            if (playButton != null) playButton.interactable = !musicService.IsPlaying();
            if (stopButton != null) stopButton.interactable = musicService.IsPlaying();
            if (pauseButton != null) pauseButton.interactable = musicService.IsPlaying();
            if (resumeButton != null) resumeButton.interactable = !musicService.IsPlaying() && musicService.GetCurrentTrack() != null;
        }

        // Przykład użycia w kodzie gry
        private void ExampleGameplayUsage()
        {
            // Rozpocznij muzykę na początku rozgrywki
            musicService.StartMusic();

            // Zmień głośność podczas gry
            musicService.SetVolume(0.3f);

            // Zatrzymaj muzykę na końcu rozgrywki
            musicService.StopMusic();
        }
    }
}
