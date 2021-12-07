using System.Collections.Generic;
using DataStructures.Variables;
using Features.Maze_Namespace;
using Features.Maze_Namespace.Tiles;
using UnityEngine;

[CreateAssetMenu]
public class InteractableMazeModifier : MazeModifier
{
    [SerializeField] private IntVariable height;
    [SerializeField] private IntVariable width;
    [SerializeField] private InteractableTorch interactablePrefab;
    [SerializeField] private float spawnPercentage;
    

    public override void AddInteractableModifier(MazeGenerator_Behaviour mazeGenerator, List<TileBehaviour> tilesWithoutInteractable)
    {
        //calculate percentage of torch placement
        int amountOfInteractable = Mathf.RoundToInt((height.Get() * width.Get()) * spawnPercentage);

        for (int index = 0; index < amountOfInteractable; index++)
        {
            if (tilesWithoutInteractable.Count == 0)
            {
                break;
            }
            int interactablePosition = Random.Range(0, tilesWithoutInteractable.Count);

            TileBehaviour tileToBeFilled = tilesWithoutInteractable[interactablePosition];
            tilesWithoutInteractable.Remove(tileToBeFilled);
            
            InteractableBehaviour interactable = Instantiate(interactablePrefab, mazeGenerator.GetInteractableInstantiationParent());
            interactable.transform.position = (Vector2)tileToBeFilled.position;
            tileToBeFilled.RegisterInteractable(interactable);
        }
    }
}
