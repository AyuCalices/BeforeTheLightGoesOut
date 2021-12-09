using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExploderFocus : ScriptableObject
{
    private SpriteRenderer spriteExploderParent;
    private SpriteExploderWithoutPhysics spriteExploder;

    public void SetExploderFocus(SpriteRenderer spriteExploderParent, SpriteExploderWithoutPhysics spriteExploder)
    {
        this.spriteExploderParent = spriteExploderParent;
        this.spriteExploder = spriteExploder;
    }
    
    public void ExplodeSprite()
    {
        spriteExploder.ExplodeSprite();
        spriteExploderParent.enabled = false;
    }
}
