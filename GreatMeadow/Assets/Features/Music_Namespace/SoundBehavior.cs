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
    }
}
