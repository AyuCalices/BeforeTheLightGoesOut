using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Utils.Event_Namespace;

public class PlayerTorch : MonoBehaviour
{
    
    [SerializeField] private float currentTorchDurability = 1.0f;
    [SerializeField] private float maxTorchDurability = 1f;
    [SerializeField] private float highestTorchBrightness = 1.3f;
    [SerializeField] private float lowestTorchBrighness = 0.66f;
    [SerializeField] private float torchBrightness = 1.3f;
    [SerializeField] private Light2D torchLight;
    [SerializeField] private GameEvent onLoadLoseMenu;
    private float startTorchDuration = 10.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        RefillTorch();
    }

    // Update is called once per frame
    void Update()
    {
        currentTorchDurability -= Time.deltaTime;
        Debug.Log("currentTorchDurab: " + currentTorchDurability);
        float torchDurabilityInPercent = currentTorchDurability / maxTorchDurability;
        Debug.Log("torchDurabilityInPercent: " + torchDurabilityInPercent);
        torchBrightness = Mathf.Max(lowestTorchBrighness, torchDurabilityInPercent * highestTorchBrightness);
        Debug.Log("torchBrightness: " + torchBrightness);
        torchLight.intensity = torchBrightness;
        if (currentTorchDurability <= 0)
        {
            LoadLoseMenu();
        }
    }
    
    //The player picks up the torch.
    public void RefillTorch()
    {
        currentTorchDurability = startTorchDuration;
    }
    
    //
    public void LoadLoseMenu()
    {
        onLoadLoseMenu.Raise();
    }
}
