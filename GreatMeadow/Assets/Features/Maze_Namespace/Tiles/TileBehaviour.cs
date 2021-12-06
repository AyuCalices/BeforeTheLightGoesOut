using System.Collections.Generic;
using Features.Maze_Namespace;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Variables_Namespace;

[RequireComponent(typeof(SpriteRenderer))]
public class TileBehaviour : GameObjectActiveSwitchBehaviour
{
    [SerializeField] private TileList_SO tileList;
    
    public Vector2Int position { get; private set; }
    public List<Vector2Variable> directions { get; private set; }
    
    private InteractableBehaviour interactable;
    
    public void Initialize(Tile tile)
    {
        position = tile.position;
        tileList.RegisterTile(this);
        directions = tile.directions;
        gameObject.SetActive(false);
    }

    public void RegisterInteractable(InteractableBehaviour interactable)
    {
        this.interactable = interactable;
    }
    
    public bool ContainsInteractable() => interactable != null;

    public override void Enable()
    {
        base.Enable();
        
        if (interactable != null)
        {
            interactable.Enable();
        }
    }

    public override void Disable()
    {
        base.Disable();
        
        if (interactable != null)
        {
            interactable.Disable();
        }
    }
}
