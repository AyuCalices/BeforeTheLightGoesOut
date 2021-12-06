using DataStructures.Variables;
using Features.Character_Namespace;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Event_Namespace;

public class Hatch : InteractableBehaviour
{
    [SerializeField] private TileList_SO tileList;
    [SerializeField] private Vector2IntVariable hatchPosition;
    [SerializeField] private Vector2IntVariable hatchSpawnPos;
    [SerializeField] private Vector2IntVariable playerSpawnPos;
    [SerializeField] private IntVariable width;
    [SerializeField] private IntVariable height;
    [SerializeField] private GameEvent onLoadWinMenu;
    private Animator animator;
    private static readonly int JumpInHatch = Animator.StringToHash("JumpInHatch");

    /**
    * Set the Hatch Position at the opposite of the starting position.
    */
    public void SetHatchPosition() 
    {
        int startX = playerSpawnPos.Get().x;
        int startY = playerSpawnPos.Get().y;
           
        int endX = width.Get() - 1 - startX; 
        int endY = height.Get() - 1 - startY;

        //hatchSpawnPos.vec2Value = playerSpawnPos.vec2Value; //for testing
        hatchSpawnPos.Set(new Vector2Int(endX, endY));
        transform.position = (Vector2)hatchPosition.Get();
        tileList.GetTileAt(endX, endY).RegisterInteractable(this);
    }

    public override void Interact(PlayerController2D playerController)
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(JumpInHatch);
        playerController.GetComponent<SpriteRenderer>().enabled = false;
    }

    //gets called by an animator event
    public void LoadWinMenu()
    {
        onLoadWinMenu.Raise();
    }
}
