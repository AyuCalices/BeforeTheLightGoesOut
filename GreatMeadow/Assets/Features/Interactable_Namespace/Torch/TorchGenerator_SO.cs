using UnityEngine;

[CreateAssetMenu]
public class TorchGenerator_SO : ScriptableObject
{
    [SerializeField] private GameObject torchPrefab;
    
    public void InstantiateTorchAt(Vector2 position, Transform torchParent)
    {
        GameObject torch = Instantiate(torchPrefab, torchParent);
        torch.transform.position = position;
    }
}
