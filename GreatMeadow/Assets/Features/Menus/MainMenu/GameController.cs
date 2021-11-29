using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Source: www.youtube.com/watch?v=zc8ac_qUXQY&t=643s

public class GameController : MonoBehaviour
{
  
List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

// If "Quit" is pressed, the game will not start and we will see a message in the console that we quit the game
  // if build is used, the game will quit
  public void Quit()
  {
    Debug.Log("Game Quit!");
    Application.Quit();
  }
  public void LoadGameScenes()
  {
    scenesToLoad.Add(SceneManager.LoadSceneAsync("AnimationScene",LoadSceneMode.Additive));
    scenesToLoad.Add(SceneManager.LoadSceneAsync("MazeGenerationScene",LoadSceneMode.Additive));
    scenesToLoad.Add(SceneManager.LoadSceneAsync("Music",LoadSceneMode.Additive));
    scenesToLoad.Add(SceneManager.LoadSceneAsync("MapScene",LoadSceneMode.Additive));
  }
  public void UnloadGameScenes()
  {
    scenesToLoad.Remove(SceneManager.UnloadSceneAsync("AnimationScene"));
    scenesToLoad.Remove(SceneManager.UnloadSceneAsync("MazeGenerationScene"));
    scenesToLoad.Remove(SceneManager.UnloadSceneAsync("Music"));
    scenesToLoad.Remove(SceneManager.UnloadSceneAsync("MapScene"));
  }
}
