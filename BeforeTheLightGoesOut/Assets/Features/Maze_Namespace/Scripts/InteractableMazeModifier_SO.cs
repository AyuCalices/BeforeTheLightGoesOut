using System.Collections.Generic;
using DataStructures.Variables;
using Features.Interactable_Namespace.Scripts;
using Features.Maze_Namespace;
using Features.Maze_Namespace.Scripts;
using UnityEngine;
using Utils.Variables;

[CreateAssetMenu]
public class InteractableMazeModifier_SO : MazeModifier_SO
{
    [SerializeField] private IntVariable height;
    [SerializeField] private IntVariable width;
    [SerializeField] private InteractableBehaviour interactablePrefab;
    [SerializeField] private float spawnPercentage;
    

    public override void AddInteractableModifier(MazeGeneratorBehaviour mazeGenerator, List<TileBehaviour> tilesWithoutInteractable)
    {
        //calculate percentage of interactable placement
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
