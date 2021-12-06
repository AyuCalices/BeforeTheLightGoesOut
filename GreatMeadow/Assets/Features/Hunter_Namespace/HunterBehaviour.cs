using System.Collections;
using System.Collections.Generic;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Variables_Namespace;

public class HunterBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2Variable hunterPos;
    [SerializeField] private Vector2Variable playerPos;
    [SerializeField] private TileList_SO tiles;
    [SerializeField] private PositionController posControl;
    
    [SerializeField] private float hunterTileSwapTime;
    [SerializeField] private float hunterBahviourUpdateTime;
    
    [SerializeField] private int playerChasePathfindingDepth = 1;

    private TileBehaviour lastTile;
    
    public void Initialize()
    {
        transform.position = hunterPos.vec2Value;
        lastTile = tiles.GetTileAt(Mathf.RoundToInt(hunterPos.vec2Value.x), Mathf.RoundToInt(hunterPos.vec2Value.y));

        StartCoroutine(Move());
    }
    
    private bool GetNextPathfindingPosition(TileBehaviour from, Vector2Int targetPosition, int searchDepth, out TileBehaviour nextTile)
    {
        foreach (var surroundingTile in posControl.GetConnectedTiles(from))
        {
            if (posControl.GetPathsByDepth_ExcludeParentPositions(surroundingTile, searchDepth - 1, from).Find(x => x.position == targetPosition))
            {
                nextTile = surroundingTile;
                return true;
            }
        }

        nextTile = null;
        return false;
    }

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

    private IEnumerator Move()
    {
        while (true)
        {
            TileBehaviour currentTile = tiles.GetTileAt(Mathf.RoundToInt(hunterPos.vec2Value.x), Mathf.RoundToInt(hunterPos.vec2Value.y));

            Vector2Int playerPosition = new Vector2Int(Mathf.RoundToInt(playerPos.vec2Value.x),
                Mathf.RoundToInt(playerPos.vec2Value.y));

            if (hunterPos.vec2Value != playerPosition)
            {
                TileBehaviour nextTile;
                if (GetNextPathfindingPosition(currentTile, playerPosition, playerChasePathfindingDepth, out TileBehaviour foundTile))
                {
                    nextTile = foundTile;
                }
                else
                {
                    nextTile = GetRandomDestination(currentTile);
                }

                hunterPos.vec2Value = nextTile.position;
                LeanTween.move(gameObject, nextTile.position, hunterTileSwapTime);
            }

            yield return new WaitForSeconds(hunterBahviourUpdateTime);
        }
    }
}
