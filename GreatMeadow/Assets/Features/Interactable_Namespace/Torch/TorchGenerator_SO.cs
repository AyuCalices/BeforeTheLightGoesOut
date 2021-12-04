using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TorchGenerator_SO : ScriptableObject
{
    [SerializeField] private GameObject torchPrefab;
    
    public void InstantiateTorchAt(Vector2 position, Transform torchParent)
    {
        Debug.Log("reached torchgenerator");
        GameObject torch = Instantiate(torchPrefab, torchParent);
        torch.transform.position = position;
    }
}
