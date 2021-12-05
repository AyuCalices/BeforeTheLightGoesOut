using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils.Event_Namespace;

public class PlayerTorch : MonoBehaviour
{
    [SerializeField] private float startTorchDuration = 30f;
    [SerializeField] private float highestTorchBrightness = 0.5f;
    [SerializeField] private float lowestTorchBrightness = 0.1f;
    [SerializeField] private float torchBrightness = 1.3f;
    [SerializeField] private Light2D torchLight;
    [SerializeField] private GameEvent onLoadLoseMenu;
    
    private float currentTorchDuration;
    private bool gameIsLost;

    // Start is called before the first frame update
    void Start()
    {
        RefillTorch();
    }

    // Update is called once per frame
    public void Update()
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
    
    //The player picks up the torch.
    public void RefillTorch()
    {
        currentTorchDuration = startTorchDuration;
    }
    
    //If the torch goes out the lose screen will be shown.
    public void LoadLoseMenu()
    {
        onLoadLoseMenu.Raise();
    }
}
