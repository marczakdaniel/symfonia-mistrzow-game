using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace Services
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager _instance;
        public static MusicManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MusicManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("MusicManager");
                        _instance = go.AddComponent<MusicManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        [Header("Audio Settings")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> musicTracks = new List<AudioClip>();
        
        private int currentTrackIndex = 0;
        private bool isPlaying = false;
        private bool isInitialized = false;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSource();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void InitializeAudioSource()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            
            // Load music tracks from Resources/Music folder
            LoadMusicTracks();
            
            isInitialized = true;
        }

        private void LoadMusicTracks()
        {
            // Load all audio clips from Resources/Music folder
            AudioClip[] clips = Resources.LoadAll<AudioClip>("Music");
            musicTracks.Clear();
            musicTracks.AddRange(clips);
            
            Debug.Log($"MusicManager: Loaded {musicTracks.Count} music tracks");
        }

        private void Update()
        {
            if (!isInitialized || !isPlaying) return;

            // Check if current track has finished playing
            if (!audioSource.isPlaying && musicTracks.Count > 0)
            {
                PlayNextTrack();
            }
        }

        /// <summary>
        /// Starts playing music tracks sequentially
        /// </summary>
        public void PlayMusic()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("MusicManager: Not initialized yet");
                return;
            }

            if (musicTracks.Count == 0)
            {
                Debug.LogWarning("MusicManager: No music tracks available");
                return;
            }

            isPlaying = true;
            PlayCurrentTrack();
        }

        /// <summary>
        /// Stops playing music
        /// </summary>
        public void StopMusic()
        {
            isPlaying = false;
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// Pauses the current music track
        /// </summary>
        public void PauseMusic()
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }

        /// <summary>
        /// Resumes playing the current music track
        /// </summary>
        public void ResumeMusic()
        {
            if (audioSource != null && !audioSource.isPlaying && isPlaying)
            {
                audioSource.UnPause();
            }
        }

        /// <summary>
        /// Sets the volume of the music (0.0f to 1.0f)
        /// </summary>
        /// <param name="volume">Volume level between 0.0f and 1.0f</param>
        public void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Clamp01(volume);
            }
        }

        /// <summary>
        /// Gets the current volume level
        /// </summary>
        /// <returns>Current volume level</returns>
        public float GetVolume()
        {
            return audioSource != null ? audioSource.volume : 0f;
        }

        /// <summary>
        /// Skips to the next track
        /// </summary>
        public void PlayNextTrack()
        {
            if (musicTracks.Count == 0) return;

            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Count;
            PlayCurrentTrack();
        }

        /// <summary>
        /// Skips to the previous track
        /// </summary>
        public void PlayPreviousTrack()
        {
            if (musicTracks.Count == 0) return;

            currentTrackIndex = (currentTrackIndex - 1 + musicTracks.Count) % musicTracks.Count;
            PlayCurrentTrack();
        }

        /// <summary>
        /// Plays a specific track by index
        /// </summary>
        /// <param name="trackIndex">Index of the track to play</param>
        public void PlayTrack(int trackIndex)
        {
            if (trackIndex < 0 || trackIndex >= musicTracks.Count)
            {
                Debug.LogWarning($"MusicManager: Invalid track index {trackIndex}");
                return;
            }

            currentTrackIndex = trackIndex;
            PlayCurrentTrack();
        }

        /// <summary>
        /// Gets the current track index
        /// </summary>
        /// <returns>Current track index</returns>
        public int GetCurrentTrackIndex()
        {
            return currentTrackIndex;
        }

        /// <summary>
        /// Gets the total number of available tracks
        /// </summary>
        /// <returns>Number of tracks</returns>
        public int GetTrackCount()
        {
            return musicTracks.Count;
        }

        /// <summary>
        /// Gets the name of the current track
        /// </summary>
        /// <returns>Current track name</returns>
        public string GetCurrentTrackName()
        {
            if (currentTrackIndex >= 0 && currentTrackIndex < musicTracks.Count && musicTracks[currentTrackIndex] != null)
            {
                return musicTracks[currentTrackIndex].name;
            }
            return "No Track";
        }

        /// <summary>
        /// Checks if music is currently playing
        /// </summary>
        /// <returns>True if music is playing</returns>
        public bool IsPlaying()
        {
            return isPlaying && audioSource != null && audioSource.isPlaying;
        }

        private void PlayCurrentTrack()
        {
            if (musicTracks.Count == 0 || currentTrackIndex >= musicTracks.Count) return;

            AudioClip clip = musicTracks[currentTrackIndex];
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                Debug.Log($"MusicManager: Playing track {currentTrackIndex + 1}/{musicTracks.Count}: {clip.name}");
            }
        }

        /// <summary>
        /// Adds a music track to the playlist
        /// </summary>
        /// <param name="clip">Audio clip to add</param>
        public void AddTrack(AudioClip clip)
        {
            if (clip != null && !musicTracks.Contains(clip))
            {
                musicTracks.Add(clip);
                Debug.Log($"MusicManager: Added track: {clip.name}");
            }
        }

        /// <summary>
        /// Removes a music track from the playlist
        /// </summary>
        /// <param name="clip">Audio clip to remove</param>
        public void RemoveTrack(AudioClip clip)
        {
            if (musicTracks.Remove(clip))
            {
                Debug.Log($"MusicManager: Removed track: {clip.name}");
                
                // Adjust current track index if necessary
                if (musicTracks.Count > 0)
                {
                    currentTrackIndex = Mathf.Clamp(currentTrackIndex, 0, musicTracks.Count - 1);
                }
                else
                {
                    currentTrackIndex = 0;
                }
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
