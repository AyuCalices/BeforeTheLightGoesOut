using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject audioSource;

    public void EnableSound()
    {
        audioSource.SetActive(true);
    }

    public void DisableSound()
    {
        audioSource.SetActive(false);

    }
}
