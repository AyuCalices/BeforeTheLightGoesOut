using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.CanvasNavigator;

namespace Features.GameStates.Scripts
{
    [CreateAssetMenu(fileName = "MenuState", menuName = "GameStates/Menu")]
    public class MenuState_SO : State_SO
    {
        [SerializeField] protected GameStateController_SO gameStateController;
        public float fadeTime = 1f;
        
        private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
        private MenuType_SO endScreenType;
        
        public void SetEndScreen(MenuType_SO endScreenType)
        {
            this.endScreenType = endScreenType;
        }
        
        public override void Enter()
        {
            if (scenesToLoad.Count == 0) return;
            
            ShowFadeMenu(() =>
            {
                gameStateController.CanvasManager.SwitchCanvas(endScreenType);
      
                SceneManager.UnloadSceneAsync("Music");
                SceneManager.UnloadSceneAsync("AnimationScene");
                SceneManager.UnloadSceneAsync("HatchScene");
                SceneManager.UnloadSceneAsync("MapScene");
                SceneManager.UnloadSceneAsync("HunterScene");
                SceneManager.UnloadSceneAsync("MazeGenerationScene");

                scenesToLoad[scenesToLoad.Count - 1].completed += _ =>
                {
                    HideFadeMenu(() =>
                    {
                        scenesToLoad.Clear();
                        gameStateController.MusicBehaviour.Enable();
                    });
                };
            });
        }

        public override void Exit()
        {
            gameStateController.MusicBehaviour.Disable(fadeTime);
            ShowFadeMenu(() =>
            {
                gameStateController.CanvasManager.CloseCanvas();
      
                scenesToLoad.Add(SceneManager.LoadSceneAsync("Music",LoadSceneMode.Additive));
                scenesToLoad.Add(SceneManager.LoadSceneAsync("AnimationScene",LoadSceneMode.Additive));
                scenesToLoad.Add(SceneManager.LoadSceneAsync("HatchScene",LoadSceneMode.Additive));
                scenesToLoad.Add(SceneManager.LoadSceneAsync("MapScene",LoadSceneMode.Additive));
                scenesToLoad.Add(SceneManager.LoadSceneAsync("HunterScene",LoadSceneMode.Additive));
                scenesToLoad.Add(SceneManager.LoadSceneAsync("MazeGenerationScene",LoadSceneMode.Additive));

                scenesToLoad[scenesToLoad.Count - 1].completed +=  _ =>
                {
                    HideFadeMenu();
                };
            });
        }

        private void ShowFadeMenu(Action onCompleteAction = null)
        {
            gameStateController.FadeMenu.gameObject.SetActive(true);
            LeanTween.alphaCanvas(gameStateController.FadeMenu, 1f, fadeTime).setOnComplete(onCompleteAction);
        }
        
        private void HideFadeMenu(Action onfadeCompleteAction = null)
        {
            LeanTween.alphaCanvas(gameStateController.FadeMenu, 0f, fadeTime).setOnComplete(() =>
            {
                gameStateController.FadeMenu.gameObject.SetActive(false);
                onfadeCompleteAction?.Invoke();
            });
        }
    }
}
