using UnityEngine;
using Services;
using Events;

namespace Examples
{
    /// <summary>
    /// Przykład integracji MusicService z istniejącą architekturą gry
    /// Możesz dodać ten kod do GameManager lub stworzyć osobny komponent
    /// </summary>
    public class MusicServiceIntegration : MonoBehaviour
    {
        [Header("Music Settings")]
        [SerializeField] private bool startMusicOnGameStart = true;
        [SerializeField] private bool stopMusicOnGameEnd = true;
        [SerializeField] private float gameVolume = 0.5f;
        [SerializeField] private float menuVolume = 0.3f;

        private MusicService musicService;

        private void Start()
        {
            musicService = MusicService.Instance;
            
            
            // Ustaw początkową głośność
            musicService.SetVolume(menuVolume);
            
            if (startMusicOnGameStart)
            {
                musicService.StartMusic();
            }
        }


        // Event handlers - dostosuj do swoich eventów
        private void OnGameStarted(GameStartedEvent gameEvent)
        {
            Debug.Log("MusicService: Gra rozpoczęta - ustawiam głośność gry");
            musicService.SetVolume(gameVolume);
            
            if (!musicService.IsPlaying())
            {
                musicService.StartMusic();
            }
        }

        private void OnGameEnded(GameEndedEvent gameEvent)
        {
            Debug.Log("MusicService: Gra zakończona");
            
            if (stopMusicOnGameEnd)
            {
                musicService.StopMusic();
            }
            else
            {
                musicService.SetVolume(menuVolume);
            }
        }

        // Publiczne metody do użycia z UI
        public void ToggleMusic()
        {
            if (musicService.IsPlaying())
            {
                musicService.StopMusic();
            }
            else
            {
                musicService.StartMusic();
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicService.SetVolume(volume);
        }

        public void PauseMusic()
        {
            musicService.PauseMusic();
        }

        public void ResumeMusic()
        {
            musicService.ResumeMusic();
        }

        // Przykład użycia w różnych scenariuszach gry
        public void OnConcertCardPlayed()
        {
            // Możesz dodać specjalne efekty dźwiękowe lub zmienić muzykę
            Debug.Log("MusicService: Karta koncertowa zagrana");
        }

        public void OnResourceCollected()
        {
            // Przykład: krótka zmiana głośności przy zbieraniu zasobów
            StartCoroutine(TemporaryVolumeChange(0.7f, 0.5f));
        }

        private System.Collections.IEnumerator TemporaryVolumeChange(float targetVolume, float duration)
        {
            float originalVolume = musicService.GetVolume();
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float currentVolume = Mathf.Lerp(originalVolume, targetVolume, t);
                musicService.SetVolume(currentVolume);
                yield return null;
            }

            musicService.SetVolume(originalVolume);
        }
    }
}
