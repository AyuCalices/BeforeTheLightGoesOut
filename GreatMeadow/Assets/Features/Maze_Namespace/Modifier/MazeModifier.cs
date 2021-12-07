using System.Collections;
using System.Collections.Generic;
using Features.Maze_Namespace;
using UnityEngine;

public abstract class MazeModifier : ScriptableObject
{
    public abstract void AddInteractableModifier(MazeGenerator_Behaviour mazeGenerator);
}
