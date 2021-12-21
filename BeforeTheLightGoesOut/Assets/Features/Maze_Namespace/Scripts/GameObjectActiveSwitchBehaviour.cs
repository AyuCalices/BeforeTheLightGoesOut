using DataStructures.Variables;
using UnityEngine;
using Utils.Variables;

namespace Features.Maze_Namespace.Scripts
{
    public class GameObjectActiveSwitchBehaviour : MonoBehaviour
    {
        [SerializeField] protected FloatVariable lerpTime;

        public virtual void PrepareRenderer()
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color = new Color(color.r, color.g, color.b, 0);
                spriteRenderer.color = color;
            }
        
            gameObject.SetActive(false);
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
}
