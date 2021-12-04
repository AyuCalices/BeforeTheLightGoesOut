using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Event_Namespace;

// Source: www.youtube.com/watch?v=zc8ac_qUXQY&t=643s

public class GameLoader : MonoBehaviour
{
  [SerializeField] private bool loadScenes;
  
  [Header("Events")]
  [SerializeField] private GameEvent onFadeComplete;
  [SerializeField] private GameEvent onLoadGameScenesComplete;
  
  [Header("Canvas")]
  [SerializeField] private CanvasManager canvasManager;
  [SerializeField] private CanvasGroup fadeMenu;
  [SerializeField] private float fadeTime = 1f;
  
  private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

  private void HideFadeMenu(Action onfadeCompleteAction = null)
  {
    LeanTween.alphaCanvas(fadeMenu, 0f, fadeTime).setOnComplete(() =>
    {
      fadeMenu.gameObject.SetActive(false);
      onfadeCompleteAction?.Invoke();
    });
  }
  
  private void ShowFadeMenu(Action onCompleteAction = null)
  {
    fadeMenu.gameObject.SetActive(true);
    LeanTween.alphaCanvas(fadeMenu, 1f, fadeTime).setOnComplete(onCompleteAction);
  }

  public void Quit()
  {
    Debug.Log("Game Quit!");
    Application.Quit();
  }
  
  public void LoadGameScenes()
  {
    if (!loadScenes)
    {
      canvasManager.CloseCanvas();
      return;
    }
    
    ShowFadeMenu(() =>
    {
      canvasManager.CloseCanvas();
      
      scenesToLoad.Add(SceneManager.LoadSceneAsync("Music",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("AnimationScene",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("HatchScene",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("MazeGenerationScene",LoadSceneMode.Additive));
      scenesToLoad.Add(SceneManager.LoadSceneAsync("MapScene",LoadSceneMode.Additive));

      scenesToLoad[scenesToLoad.Count - 1].completed +=  _ =>
      {
        onLoadGameScenesComplete.Raise();
        HideFadeMenu(() => onFadeComplete.Raise());
      };
    });
  }

  public void UnloadGameScenes(MenuType_SO menuToBeOpened)
  {
    if (!loadScenes)
    {
      return;
    }
    
    ShowFadeMenu(() =>
    {
      canvasManager.SwitchCanvas(menuToBeOpened);
      SceneManager.UnloadSceneAsync("Music");
      SceneManager.UnloadSceneAsync("AnimationScene");
      SceneManager.UnloadSceneAsync("HatchScene");
      SceneManager.UnloadSceneAsync("MazeGenerationScene");
      SceneManager.UnloadSceneAsync("MapScene");
      
      scenesToLoad[scenesToLoad.Count - 1].completed += _ =>
      {
        HideFadeMenu((() => scenesToLoad.Clear()));
      };
    });
  }
}
