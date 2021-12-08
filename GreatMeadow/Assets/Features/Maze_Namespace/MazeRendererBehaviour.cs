using System.Collections;
using System.Collections.Generic;
using DataStructures.Variables;
using Features.Maze_Namespace;
using Features.Maze_Namespace.Tiles;
using UnityEngine;

[RequireComponent(typeof(MazeGenerator_Behaviour))]
public class MazeRendererBehaviour : MonoBehaviour
{
    [Header("Tiles")]
    [Tooltip("List of tiles to be spawned.")]
    [SerializeField] private TileList_SO tiles;
    [Tooltip("Determine the number of neighbor tiles to be rendered.")]
    [SerializeField] private int renderSize = 1;
    [Tooltip("Grants access to the neighbor tiles' information.")]
    [SerializeField] private PositionController posControl;

    [SerializeField] private List<Vector2IntVariable> renderPositions;
    
    private bool isInitialized;
    private List<RenderEntity> renderEntities;

    public void InitializeMazeRenderer()
    {
        foreach (var tilesY in tiles.GetAllTiles())
        {
            foreach (var tile in tilesY)
            {
                tile.PrepareRenderer();
            }
        }

        renderEntities = new List<RenderEntity>();
        foreach (var renderPosition in renderPositions)
        {
            renderEntities.Add(new RenderEntity(renderPosition.Get(), renderPosition));
        }

        foreach (RenderEntity renderEntity in renderEntities)
        {
            Render(renderEntity);
        }

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        for (int index = renderEntities.Count - 1; index >= 0; index--)
        {
            var renderEntity = renderEntities[index];
            if (renderEntity.lastTilePosition != renderEntity.renderPosition.Get())
            {
                Render(renderEntity);
            }
        }
    }

    private void Render(RenderEntity renderEntity)
    {
        List<RenderEntity> otherEntities = new List<RenderEntity>(renderEntities);
        otherEntities.Remove(renderEntity);
        OptimizeRender(renderEntity, otherEntities);
        
        renderEntity.lastTilePosition = renderEntity.renderPosition.Get();
    }

    private bool RenderEntityContains(List<RenderEntity> renderEntities, TileBehaviour tile)
    {
        foreach (var renderEntity in renderEntities)
        {
            if (renderEntity.oldRenderTiles.Contains(tile))
            {
                return true;
            }
        }

        return false;
    }
    
    private void OptimizeRender(RenderEntity focusedRenderEntity, List<RenderEntity> otherRenderEntities)
    {
        List<TileBehaviour> tilesToRender = new List<TileBehaviour>();
        tilesToRender = posControl.GetPathsByDepth(tiles.GetTileAt(focusedRenderEntity.renderPosition.Get()), renderSize);

        List<TileBehaviour> newRenderTiles = new List<TileBehaviour>();
        foreach(TileBehaviour renderedTile in tilesToRender)
        {
            // render the tile at the given position
            if (!RenderEntityContains(otherRenderEntities, renderedTile) && !focusedRenderEntity.oldRenderTiles.Contains(renderedTile))
            {
                renderedTile.Enable();
            } 
            
            if (focusedRenderEntity.oldRenderTiles.Contains(renderedTile))
            {
                focusedRenderEntity.oldRenderTiles.Remove(renderedTile);
            }

            newRenderTiles.Add(renderedTile);
        }
            
        foreach (var renderedTile in focusedRenderEntity.oldRenderTiles)
        {
            if (!RenderEntityContains(otherRenderEntities, renderedTile))
            {
                renderedTile.Disable();
            }
        }

        focusedRenderEntity.oldRenderTiles = newRenderTiles;
    }

    private class RenderEntity
    {
        public Vector2Int lastTilePosition;
        public Vector2IntVariable renderPosition;
        public List<TileBehaviour> oldRenderTiles;

        public RenderEntity(Vector2Int lastTilePosition, Vector2IntVariable renderPosition)
        {
            this.lastTilePosition = lastTilePosition;
            this.renderPosition = renderPosition;
            oldRenderTiles = new List<TileBehaviour>();
        }
    }
}
