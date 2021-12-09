using System.Collections;
using System.Collections.Generic;
using DataStructures.Variables;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Event_Namespace;
using Random = UnityEngine.Random;

public class HunterBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Vector2IntVariable hunterPos;
    [SerializeField] private Vector2IntVariable playerIntPosition;
    [SerializeField] private Vector2Variable playerFloatPosition;
    [SerializeField] private TileList_SO tiles;
    [SerializeField] private PositionController posControl;
    [SerializeField] private GameEvent killPlayerEvent;
    [SerializeField] private BoolVariable playerIsKillable;

    [Header("Hunter Balancing")]
    [SerializeField] private float hunterBahviourUpdateTime;
    [SerializeField] private int playerChasePathfindingDepth = 1;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float dashSpeed;

    private TileBehaviour lastTile;
    private Animator animator;
    private bool playerIsKilled;
    private bool isInitialized;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int KillPlayer = Animator.StringToHash("KillPlayer");
    
    public void InitializeHunter()
    {
        transform.position = (Vector2)hunterPos.Get();
        lastTile = tiles.GetTileAt(hunterPos.Get());
        isInitialized = true;

        StartCoroutine(MoveUpdate());
    }
    
    //Used by an animator event
    public void PlayerDash()
    {
        float tweenTime = Vector2.Distance(playerFloatPosition.Get(), transform.position) / dashSpeed;
        LeanTween.move(gameObject, (Vector2) playerFloatPosition.Get(), tweenTime).setOnComplete(() => killPlayerEvent.Raise());
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isInitialized) return;
        
        //update hunter position
        var position = transform.position;
        hunterPos.Set(new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)));
        
        //kill animation condition
        if (hunterPos.Get() == playerIntPosition.Get() && !playerIsKilled && playerIsKillable.Get())
        {
            playerIsKilled = true;
            animator.SetTrigger(KillPlayer);
            LeanTween.cancel(gameObject);
        }
    }
    
    private IEnumerator MoveUpdate()
    {
        while (!playerIsKilled)
        {
            //set positions
            TileBehaviour currentTile = tiles.GetTileAt(hunterPos.Get());
            TileBehaviour nextTile;
            if (GetNextPathfindingPosition(currentTile, playerIntPosition.Get(), playerChasePathfindingDepth, out TileBehaviour foundTile) && playerIsKillable.Get())
            {
                nextTile = foundTile;
            }
            else
            {
                nextTile = GetRandomDestination(currentTile);
            }

            //animations
            Vector2Int path = nextTile.position - currentTile.position;
            animator.SetFloat(Horizontal, path.x);
            animator.SetFloat(Vertical, path.y);

            //execute moving
            float tweenTime = Vector2.Distance(transform.position, nextTile.position) / walkSpeed;
            LeanTween.move(gameObject, nextTile.position, tweenTime).setOnComplete(() =>
            {
                animator.SetFloat(Horizontal, 0);
                animator.SetFloat(Vertical, 0);
            });
            
            yield return new WaitForSeconds(hunterBahviourUpdateTime);
        }
    }
    
    /// <summary>
    /// A simple Pathfinding algorithm that can search from a startTileBehaviour for a targetPosition by a tile depth.
    /// </summary>
    /// <param name="startTile">The position the pathfinding will start from</param>
    /// <param name="targetPosition">The position the pathfinding is trying to reach.</param>
    /// <param name="searchDepth">The amount of tiles away from the startPosition</param>
    /// <param name="nextTile">If a path was found this parameter will return the next tile to the target. Will return null if none found</param>
    /// <returns>Returns true if the targetPosition was found with the provided depth. False if not.</returns>
    private bool GetNextPathfindingPosition(TileBehaviour startTile, Vector2Int targetPosition, int searchDepth, out TileBehaviour nextTile)
    {
        foreach (var surroundingTile in posControl.GetConnectedTiles(startTile))
        {
            if (posControl.GetPathsByDepth_ExcludeParentPositions(surroundingTile, searchDepth - 1, startTile).Find(x => x.position == targetPosition))
            {
                nextTile = surroundingTile;
                return true;
            }
        }

        nextTile = null;
        return false;
    }

    /// <summary>
    /// This method will return a random new direction a entity can walk to.
    /// It will not take the tile it came from unless there is a dead end in the maze.
    /// </summary>
    /// <param name="currentTile">The position the randomization will start from</param>
    /// <returns>Returns a random tile around the currentTile.</returns>
    private TileBehaviour GetRandomDestination(TileBehaviour currentTile)
    {
        List<TileBehaviour> surroundingTiles = posControl.GetConnectedTiles(currentTile);
        
        if (lastTile != null && surroundingTiles.Contains(lastTile) && surroundingTiles.Count > 1)
        {
            surroundingTiles.Remove(lastTile);
        }
        lastTile = currentTile;
        
        return surroundingTiles[Random.Range(0, surroundingTiles.Count)];
    }
}
