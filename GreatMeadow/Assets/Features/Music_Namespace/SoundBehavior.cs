using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound_Namespace.Logic
{
    public class SoundBehavior : MonoBehaviour
    {
        [SerializeField] private List<AudioSource> soundTracks;

        private int currentTrackIndex;

        private void Awake()
        {
            currentTrackIndex = Random.Range(0, soundTracks.Count);
            soundTracks[currentTrackIndex].Play();
            
            Invoke(nameof(PlayNextTrack), soundTracks[currentTrackIndex].clip.length);
        }

        private void PlayNextTrack()
        {
            if (currentTrackIndex == soundTracks.Count - 1)
            {
                currentTrackIndex = 0;
            }
            else
            {
                currentTrackIndex++;
            }
            soundTracks[currentTrackIndex].Play();
            Invoke(nameof(PlayNextTrack), soundTracks[currentTrackIndex].clip.length);
        }
        
        private IEnumerator FadeTrack(AudioSource audioSource, float toVal, float duration)
        {
            float counter = 0f;
            float startVolume = audioSource.volume;

            while (counter < duration)
            {
                if (Time.timeScale == 0)
                    counter += Time.unscaledDeltaTime;
                else
                    counter += Time.deltaTime;

                audioSource.volume = Mathf.Lerp(startVolume, toVal, counter / duration);
                yield return null;
            }
            
            gameObject.SetActive(false);
            audioSource.volume = startVolume;
        }

        public void Disable()
        {
            FadeTrack(soundTracks[currentTrackIndex], 0, 1);
        }
    }
}
