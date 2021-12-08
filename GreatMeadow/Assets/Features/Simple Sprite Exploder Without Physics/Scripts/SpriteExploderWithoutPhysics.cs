using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteExploderWithoutPhysics : MonoBehaviour 
{
    #region Fields & Getter/Setter
    [SerializeField] private GameObject pixelPrefab;
    public GameObject PixelPrefab 
    { 
        get { return pixelPrefab; } 
        set { pixelPrefab = value; } 
    }

    [SerializeField] private bool hasGeneratedSprite;
    public bool HasGeneratedSprite 
    { 
        get { return hasGeneratedSprite; } 
        private set { hasGeneratedSprite = value; 
        } 
    }

    [SerializeField] private bool enableFloorCollision;
    public bool EnableFloorCollision 
    { 
        get { return enableFloorCollision; } 
        set { enableFloorCollision = value; } 
    }

    [SerializeField] private float energyLostOnFloorCollision = 0.7f;
    public float EnergyLostOnFloorCollision
    {
        get { return energyLostOnFloorCollision; }
        set { energyLostOnFloorCollision = value; }
    }

    [SerializeField] private LeanTweenType leanTweenType;
    public LeanTweenType LeanTweenType
    {
        get { return leanTweenType; }
        set { leanTweenType = value; }
    }

    [SerializeField] private float minimumExplosionVelocity;
    public float MinimumExplosionVelocity 
    { 
        get { return minimumExplosionVelocity; } 
        set { minimumExplosionVelocity = value; } 
    }

    [SerializeField] private float explosionForce;
    public float ExplosionForce 
    { 
        get { return explosionForce; } 
        set { explosionForce = value; } 
    }

    [SerializeField] private float explosionRandomness;
    public float ExplosionRandomness 
    { 
        get { return explosionRandomness; } 
        set { explosionRandomness = value; } 
    }

    [SerializeField] private float pixelLifespan;
    public float PixelLifespan 
    { 
        get { return pixelLifespan; } 
        set { pixelLifespan = value; } 
    }

    [SerializeField] private float pixelLifespanRandomness;
    public float PixelLifespanRandomness 
    { 
        get { return pixelLifespanRandomness; } 
        set { pixelLifespanRandomness = value; } 
    }
    #endregion

    private SpriteRenderer _spriteRenderer;
    private int spriteWidth;
    private int spriteHeight;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GenerateSprite()
    {
        Sprite spriteToExplode = GetComponent<SpriteRenderer>().sprite;
        spriteWidth = spriteToExplode.texture.width;
        spriteHeight = spriteToExplode.texture.height;
        Sprite pixelSprite = pixelPrefab.GetComponent<SpriteRenderer>().sprite;

        if (transform.Find("ExplosionPixels") != null)
        {
            DestroyImmediate(transform.Find("ExplosionPixels").gameObject);
        }

        GameObject explosionParent = new GameObject();
        explosionParent.transform.parent = transform;
        explosionParent.transform.localPosition = Vector3.zero;
        explosionParent.transform.localScale = new Vector2(1f, 1f);

        explosionParent.name = "ExplosionPixels";

        for (int y = 0; y < spriteHeight; y++)
        {
            for (int x = 0; x < spriteWidth; x++)
            {
                if (spriteToExplode.texture.GetPixel(x, y).a != 0)
                {
                    GameObject g = Instantiate(pixelPrefab);
                    g.layer = 2;
                    Transform pixel = g.transform;
                    pixel.localPosition =
                        new Vector2(x + (pixelSprite.texture.width - spriteToExplode.texture.width) / 2f,
                            y - (spriteToExplode.texture.height - pixelSprite.texture.height) / 2f) / spriteToExplode.pixelsPerUnit;
                    Color pixelColor = new Color(spriteToExplode.texture.GetPixel(x, y).r, spriteToExplode.texture.GetPixel(x, y).g, spriteToExplode.texture.GetPixel(x, y).b, 1);
                    pixel.GetComponent<SpriteRenderer>().color = pixelColor;
                    pixel.SetParent(explosionParent.transform, false);
                    pixel.gameObject.SetActive(false);
                }
            }
        }

        hasGeneratedSprite = true;
    }

    public void ExplodeSprite()
    {
        _spriteRenderer.enabled = false;
        Transform explosionParent = transform.GetChild(0);
        explosionParent.gameObject.SetActive(true);

        //calculation to get the middle of the sprite
        float visibleSpriteMiddleY = (explosionParent.GetChild(explosionParent.childCount - 1).localPosition.y + explosionParent.GetChild(0).localPosition.y) / 2;
        float spriteMiddleX = spriteWidth / 2;
        Vector2 visibleSpriteCenter = new Vector2(spriteMiddleX, visibleSpriteMiddleY);

        //floor collsion calculations
        float lowestVisibileSpritePositionY = 0;
        if (enableFloorCollision && explosionParent.GetChild(0) != null)
        {
            lowestVisibileSpritePositionY = explosionParent.GetChild(0).localPosition.y;
        }
        
        //explosion
        for (int i = 0; i < explosionParent.childCount; i++)
        {
            Transform currentPixel = explosionParent.GetChild(i);

            //moveTime and position calculations
            float explosionForceWithRandomness;
            if (explosionRandomness != 0)
            {
                explosionForceWithRandomness = Random.Range(explosionForce - explosionRandomness, explosionForce + explosionRandomness);
            }
            else
            {
                explosionForceWithRandomness = explosionForce;
            }
            Vector2 explodeDirection = (Vector2)currentPixel.localPosition - visibleSpriteCenter;
            explodeDirection.Normalize();
            
            Vector2 newPosition = visibleSpriteCenter + (explosionForceWithRandomness * explodeDirection);
            

            //floor collision
            if (enableFloorCollision && newPosition.y < lowestVisibileSpritePositionY)
            {
                newPosition.y = lowestVisibileSpritePositionY;
                newPosition.x -= lowestVisibileSpritePositionY * explodeDirection.x * energyLostOnFloorCollision;
            }

            //moveTime calculation
            float moveTime = Random.Range(pixelLifespan - pixelLifespanRandomness, pixelLifespan + pixelLifespanRandomness);

            //Lerp & Hide after time
            LeanTween.moveLocal(currentPixel.gameObject, newPosition, moveTime).setEase(leanTweenType);
            LeanTween.alpha(currentPixel.gameObject, 0, moveTime);
        }
    }
}