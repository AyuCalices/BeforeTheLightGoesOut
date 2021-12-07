using System;
using System.Collections;
using Features.Character_Namespace;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Animator))]
public class InteractableTorch : InteractableBehaviour
{
    [SerializeField] private Light2D torchLight;
    
    private Animator animator;
    private static readonly int PickUpTorch = Animator.StringToHash("PickUpTorch");
    private float maxTorchIntensity;
    private bool isLooted;

    private void Awake()
    {
        maxTorchIntensity = torchLight.intensity;
        torchLight.intensity = 0;
        gameObject.SetActive(false);
    }
    
    /**
     * Pick Up Torch
     */
    public override void Interact(PlayerController2D playerController)
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(PickUpTorch);
        torchLight.intensity = 0f;
        GetComponent<AudioSource>().Stop();
        playerController.GetComponentInChildren<PlayerTorch>().RefillTorch();
        isLooted = true;
    }

    private IEnumerator ChangeLightOverTime(float fromVal, float toVal, float duration)
    {
        float counter = 0f;

        while (counter < duration)
        {
            if (Time.timeScale == 0)
                counter += Time.unscaledDeltaTime;
            else
                counter += Time.deltaTime;

            torchLight.intensity = Mathf.Lerp(fromVal, toVal, counter / duration);
            yield return null;
        }
    }

    public override void Enable()
    {
        base.Enable();
        if (!isLooted)
        {
            StartCoroutine(ChangeLightOverTime(0, maxTorchIntensity, lerpTime.floatValue));
        }
    }

    public override void Disable()
    {
        base.Disable();
        if (!isLooted)
        {
            StartCoroutine(ChangeLightOverTime(maxTorchIntensity, 0, lerpTime.floatValue));
        }
    }
}
