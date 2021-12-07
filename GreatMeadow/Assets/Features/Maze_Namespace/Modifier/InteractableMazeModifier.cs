using DataStructures.Variables;
using Features.Maze_Namespace;
using Features.Maze_Namespace.Tiles;
using UnityEngine;

[CreateAssetMenu]
public class InteractableMazeModifier : MazeModifier
{
    [SerializeField] private IntVariable height;
    [SerializeField] private IntVariable width;
    [SerializeField] private Vector2IntVariable playerPosition;
    [SerializeField] private TileList_SO tilesList;
    [SerializeField] private InteractableTorch interactablePrefab;
    [SerializeField] private float spawnPercentage;
    

    public override void AddInteractableModifier(MazeGenerator_Behaviour mazeGenerator)
    {
        //calculate percentage of torch placement
        int amountOfInteractable = Mathf.RoundToInt((height.Get() * width.Get()) * spawnPercentage);
        Vector2[] interactablesToPlace = new Vector2[amountOfInteractable];

        for (int index = 0; index < interactablesToPlace.Length; index++)
        {
            Vector2Int interactablePosition = new Vector2Int(Mathf.RoundToInt(Random.Range(0f, width.Get() - 1)), Mathf.RoundToInt(Random.Range(0f, height.Get() - 1)));
                
            if (!tilesList.GetTileAt(interactablePosition).ContainsInteractable() && interactablePosition != playerPosition.Get())
            {
                interactablesToPlace[index] = interactablePosition;
                    
                InteractableBehaviour interactable = Instantiate(interactablePrefab, mazeGenerator.GetInteractableInstantiationParent());
                interactable.transform.position = (Vector2)interactablePosition;
                    
                tilesList.GetTileAt(interactablePosition).RegisterInteractable(interactable);
            }
        }
    }
}
