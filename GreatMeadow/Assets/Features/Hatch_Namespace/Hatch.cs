using DataStructures.Variables;
using Features.Character_Namespace;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Event_Namespace;

public class Hatch : InteractableBehaviour
{
    [SerializeField] private TileList_SO tileList;
    [SerializeField] private Vector2IntVariable hatchPosition;
    [SerializeField] private GameEvent onLoadWinMenu;
    private Animator animator;
    private static readonly int JumpInHatch = Animator.StringToHash("JumpInHatch");

    public void Initialize()
    {
        transform.position = (Vector2)hatchPosition.Get();
        tileList.GetTileAt(hatchPosition.Get()).RegisterInteractable(this);
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
