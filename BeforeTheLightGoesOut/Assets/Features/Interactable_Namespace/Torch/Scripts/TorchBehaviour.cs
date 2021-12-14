using System.Collections;
using Features.Character_Namespace.Scripts;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Features.Interactable_Namespace.Torch.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class TorchBehaviour : InteractableBehaviour
    {
        [SerializeField] private Light2D torchLight;
    
        private Animator animator;
        private static readonly int PickUpTorch = Animator.StringToHash("PickUpTorch");
        private float maxTorchIntensity;
        private bool isLooted;

        /**
     * Pick Up Torch
     */
        public override void Interact(PlayerControllerBehaviour playerController)
        {
            animator = GetComponent<Animator>();
            animator.SetTrigger(PickUpTorch);
            torchLight.intensity = 0f;
            GetComponent<AudioSource>().enabled = false;
            playerController.GetComponentInChildren<PlayerTorchBehaviour>().RefillTorch();
            isLooted = true;
        }

        public override bool CanBeInteracted() => !isLooted;

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
    
        public override void PrepareRenderer()
        {
            base.PrepareRenderer();
            maxTorchIntensity = torchLight.intensity;
            torchLight.intensity = 0;
        }

        public override void Enable()
        {
            base.Enable();
            if (!isLooted)
            {
                StartCoroutine(ChangeLightOverTime(0, maxTorchIntensity, lerpTime.Get()));
            }
            else
            {
                animator.SetTrigger(PickUpTorch);
            }
        }

        public override void Disable()
        {
            base.Disable();
            if (!isLooted)
            {
                StartCoroutine(ChangeLightOverTime(maxTorchIntensity, 0, lerpTime.Get()));
            }
        }
    }
}
