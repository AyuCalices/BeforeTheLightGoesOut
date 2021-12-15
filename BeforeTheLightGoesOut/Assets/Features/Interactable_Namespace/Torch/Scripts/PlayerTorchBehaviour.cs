using Features.GameStates;
using Features.GameStates_Namespace.Scripts.States;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using Utils.Event_Namespace;

namespace Features.Interactable_Namespace.Torch.Scripts
{
    public class PlayerTorchBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameStateController_SO gameStateController;
        [SerializeField] private Light2D torchLight;
        [SerializeField] private UnityEvent onTorchLightGone;
    
        [Header("Balancing")]
        [SerializeField] private float startTorchDuration = 30f;
        [SerializeField] private float highestTorchBrightness = 0.5f;
        [SerializeField] private float lowestTorchBrightness = 0.1f;
        [SerializeField] private float torchBrightness = 1.3f;
    
        private float currentTorchDuration;
    
        //The player picks up the torch.
        public void RefillTorch()
        {
            currentTorchDuration = startTorchDuration;
        }

        private void Start()
        {
            RefillTorch();
        }

        private void Update()
        {
            currentTorchDuration -= Time.deltaTime;
            float torchDurabilityInPercent = currentTorchDuration / startTorchDuration;
            torchBrightness = Mathf.Max(lowestTorchBrightness, torchDurabilityInPercent * highestTorchBrightness);
            torchLight.intensity = torchBrightness;

            CheckPlayerDeath();
        }

        //If the torch goes out the lose screen will be shown.
        private void CheckPlayerDeath()
        {
            if (currentTorchDuration <= 0 && gameStateController.GetState() is PlayState_SO)
            {
                onTorchLightGone.Invoke();
            }
        }
    }
}
