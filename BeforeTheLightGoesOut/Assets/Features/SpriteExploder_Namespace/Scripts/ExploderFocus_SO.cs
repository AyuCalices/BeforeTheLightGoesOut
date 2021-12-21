using UnityEngine;

namespace Features.Simple_Sprite_Exploder_Without_Physics.Scripts
{
    [CreateAssetMenu]
    public class ExploderFocus_SO : ScriptableObject
    {
        private SpriteRenderer spriteExploderParent;
        private SpriteExploderBehaviour spriteExploder;

        public void SetExploderFocus(SpriteRenderer spriteExploderParent, SpriteExploderBehaviour spriteExploder)
        {
            this.spriteExploderParent = spriteExploderParent;
            this.spriteExploder = spriteExploder;
        }
    
        public void ExplodeSprite()
        {
            spriteExploderParent.enabled = false;
            spriteExploder.ExplodeSprite();
        }
    }
}
