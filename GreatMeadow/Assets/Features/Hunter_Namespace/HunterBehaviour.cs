using System.Collections;
using System.Collections.Generic;
using Features.Maze_Namespace.Tiles;
using UnityEngine;
using Utils.Variables_Namespace;

public class HunterBehaviour : MonoBehaviour
{
    [SerializeField] private Vector2Variable hunterPos;
    [SerializeField] private TileList_SO tiles;
    [SerializeField] private PositionController posControl;
    [SerializeField] private float tileSwapTime;

    private TileBehaviour lastTile;
    
    public void Initialize()
    {
        transform.position = hunterPos.vec2Value;
        lastTile = tiles.GetTileAt(Mathf.RoundToInt(hunterPos.vec2Value.x), Mathf.RoundToInt(hunterPos.vec2Value.y));

        StartCoroutine(Move());
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
            TileBehaviour newTile = GetRandomDestination(currentTile);
            
            hunterPos.vec2Value = newTile.position;
            LeanTween.move(gameObject, newTile.position, tileSwapTime);
            
            yield return new WaitForSeconds(tileSwapTime);
        }
    }
}
