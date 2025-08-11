using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Services
{
    public class MusicService : MonoBehaviour
    {
        private static MusicService _instance;
        public static MusicService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<MusicService>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("MusicService");
                        _instance = go.AddComponent<MusicService>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        [Header("Audio Settings")]
        [SerializeField] private float defaultVolume = 0.5f;
        [SerializeField] private string musicFolderPath = "Music";
        
        private AudioSource audioSource;
        private List<AudioClip> musicClips;
        private int currentClipIndex = -1;
        private bool isPlaying = false;
        private float currentVolume = 0.5f;

        public event Action<AudioClip> OnMusicChanged;
        public event Action<bool> OnPlaybackStateChanged;
        public event Action<float> OnVolumeChanged;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSource();
                LoadMusicClips();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeAudioSource()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.volume = defaultVolume;
            currentVolume = defaultVolume;
            
            // Nasłuchuj na zakończenie utworu
            audioSource.playOnAwake = false;
        }

        private void LoadMusicClips()
        {
            musicClips = new List<AudioClip>();
            
            // Ładuj wszystkie pliki audio z folderu Resources/Music
            AudioClip[] clips = Resources.LoadAll<AudioClip>(musicFolderPath);
            
            if (clips.Length > 0)
            {
                musicClips.AddRange(clips);
                Debug.Log($"MusicService: Załadowano {musicClips.Count} utworów muzycznych");
            }
            else
            {
                Debug.LogWarning($"MusicService: Nie znaleziono plików muzycznych w Resources/{musicFolderPath}");
            }
        }

        /// <summary>
        /// Rozpoczyna odtwarzanie muzyki z losowym utworem
        /// </summary>
        public void StartMusic()
        {
            if (musicClips.Count == 0)
            {
                Debug.LogWarning("MusicService: Brak dostępnych utworów muzycznych");
                return;
            }

            if (!isPlaying)
            {
                PlayRandomTrack();
                isPlaying = true;
                OnPlaybackStateChanged?.Invoke(true);
            }
        }

        /// <summary>
        /// Zatrzymuje odtwarzanie muzyki
        /// </summary>
        public void StopMusic()
        {
            if (isPlaying)
            {
                audioSource.Stop();
                isPlaying = false;
                OnPlaybackStateChanged?.Invoke(false);
            }
        }

        /// <summary>
        /// Pauzuje odtwarzanie muzyki
        /// </summary>
        public void PauseMusic()
        {
            if (isPlaying && audioSource.isPlaying)
            {
                audioSource.Pause();
                OnPlaybackStateChanged?.Invoke(false);
            }
        }

        /// <summary>
        /// Wznawia odtwarzanie muzyki
        /// </summary>
        public void ResumeMusic()
        {
            if (isPlaying && !audioSource.isPlaying)
            {
                audioSource.UnPause();
                OnPlaybackStateChanged?.Invoke(true);
            }
        }

        /// <summary>
        /// Ustawia głośność muzyki (0.0f - 1.0f)
        /// </summary>
        /// <param name="volume">Głośność od 0.0f do 1.0f</param>
        public void SetVolume(float volume)
        {
            currentVolume = Mathf.Clamp01(volume);
            audioSource.volume = currentVolume;
            OnVolumeChanged?.Invoke(currentVolume);
        }

        /// <summary>
        /// Zwraca aktualną głośność
        /// </summary>
        /// <returns>Aktualna głośność (0.0f - 1.0f)</returns>
        public float GetVolume()
        {
            return currentVolume;
        }

        /// <summary>
        /// Sprawdza czy muzyka jest aktualnie odtwarzana
        /// </summary>
        /// <returns>True jeśli muzyka jest odtwarzana</returns>
        public bool IsPlaying()
        {
            return isPlaying && audioSource.isPlaying;
        }

        /// <summary>
        /// Zwraca aktualnie odtwarzany utwór
        /// </summary>
        /// <returns>Aktualny AudioClip lub null</returns>
        public AudioClip GetCurrentTrack()
        {
            return currentClipIndex >= 0 && currentClipIndex < musicClips.Count ? musicClips[currentClipIndex] : null;
        }

        /// <summary>
        /// Zwraca liczbę dostępnych utworów
        /// </summary>
        /// <returns>Liczba załadowanych utworów</returns>
        public int GetTrackCount()
        {
            return musicClips.Count;
        }

        private void PlayRandomTrack()
        {
            if (musicClips.Count == 0) return;

            // Losuj nowy utwór, ale nie ten sam co poprzedni
            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, musicClips.Count);
            } while (newIndex == currentClipIndex && musicClips.Count > 1);

            currentClipIndex = newIndex;
            AudioClip clip = musicClips[currentClipIndex];
            
            audioSource.clip = clip;
            audioSource.Play();
            
            OnMusicChanged?.Invoke(clip);
            
            Debug.Log($"MusicService: Odtwarzam utwór: {clip.name}");
        }

        private void Update()
        {
            // Automatyczne przejście do następnego utworu po zakończeniu
            if (isPlaying && !audioSource.isPlaying && audioSource.time >= audioSource.clip.length - 0.1f)
            {
                PlayRandomTrack();
            }
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
