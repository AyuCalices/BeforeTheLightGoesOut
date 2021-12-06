using DataStructures.Variables;
using UnityEngine;

public class GameObjectActiveSwitchBehaviour : MonoBehaviour
{
    [SerializeField] protected FloatVariable lerpTime;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color = new Color(color.r, color.g, color.b, 0);
        spriteRenderer.color = color;
    }

    public virtual void Enable()
    {
        gameObject.SetActive(true);
        
        LeanTween.cancel(gameObject);
        LeanTween.alpha(gameObject, 1, lerpTime.Get());
    }

    public virtual void Disable()
    {
        LeanTween.cancel(gameObject);
        LeanTween.alpha(gameObject, 0, lerpTime.Get()).setOnComplete(() => gameObject.SetActive(false));
    }
}
