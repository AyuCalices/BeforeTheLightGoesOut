using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchSound : MonoBehaviour
{
    [SerializeField] private AudioSource hatchSound;

    private void Awake()
    {
        hatchSound.Play();
    }

}
