using DataStructures.Variables;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundDistanceBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2IntVariable soundTarget;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.volume = Mathf.InverseLerp(3, 1, Vector2.Distance(transform.position, soundTarget.Get()));
    }
}
