using DataStructures.Variables;
using UnityEngine;
using Utils.Variables;

[RequireComponent(typeof(AudioSource))]
public class SoundDistanceBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2IntVariable soundTarget;

    private AudioSource audioSource;
    private bool isInitialized;
    
    public void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = Mathf.InverseLerp(3, 1, Vector2.Distance(transform.position, soundTarget.Get()));
        audioSource.Play();
        isInitialized = true;
    }

    private void Update()
    {
        if (isInitialized)
        { 
            audioSource.volume = Mathf.InverseLerp(3, 1, Vector2.Distance(transform.position, soundTarget.Get()));
        }
    }
}
