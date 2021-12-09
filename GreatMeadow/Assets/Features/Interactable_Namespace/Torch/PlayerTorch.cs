using DataStructures.Variables;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils.Event_Namespace;

public class PlayerTorch : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D torchLight;
    [SerializeField] private GameEvent onLoadLoseMenu;
    
    [Header("Balancing")]
    [SerializeField] private float startTorchDuration = 30f;
    [SerializeField] private float highestTorchBrightness = 0.5f;
    [SerializeField] private float lowestTorchBrightness = 0.1f;
    [SerializeField] private float torchBrightness = 1.3f;
    
    private float currentTorchDuration;
    private bool gameIsLost;
    
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
        if (currentTorchDuration <= 0 && !gameIsLost)
        {
            gameIsLost = true;
            LoadLoseMenu();
        }
    }

    //If the torch goes out the lose screen will be shown.
    private void LoadLoseMenu()
    {
        onLoadLoseMenu.Raise();
    }
}
