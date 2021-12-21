using System.Collections.Generic;
using UnityEngine;

namespace Features.Maze_Namespace.Scripts
{
    public abstract class MazeModifier_SO : ScriptableObject
    {
        public abstract void AddInteractableModifier(MazeGeneratorBehaviour mazeGenerator, List<TileBehaviour> tilesWithoutInteractable);
    }
}
