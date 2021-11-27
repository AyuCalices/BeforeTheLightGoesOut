using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Source: www.youtube.com/watch?v=zc8ac_qUXQY&t=643s

public class MainMenu : MonoBehaviour
{
  
List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

// If "Play" is pressed the game will start and we see our SampleScene

  public void Play()
  {

    /* scenesToLoad.Add(SceneManager.LoadSceneAsync("AnimationScene"));
     scenesToLoad.Add(SceneManager.LoadSceneAsync("MazeGenerationScene"));
     StartCoroutine(LoadingScreen());
 */
    scenesToLoad.Add(SceneManager.LoadSceneAsync("AnimationScene",LoadSceneMode.Additive));
    scenesToLoad.Add(SceneManager.LoadSceneAsync("MazeGenerationScene",LoadSceneMode.Additive));
    scenesToLoad.Add(SceneManager.LoadSceneAsync("Music",LoadSceneMode.Additive));
    scenesToLoad.Add(SceneManager.LoadSceneAsync("MapScene",LoadSceneMode.Additive));
    
  ; // hier muss noch die Scene rein oder die Scenen

    // try both

    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

  }
  

  // If "Quit" is pressed, the game will not start and we will see a message in the console that we quit the game
  // if build is used, the game will quit
  public void Quit()
  {
    Debug.Log("Game Quit!");
    Application.Quit();
  }
}
