using UnityEngine;

namespace Features.GameStates.Scripts
{
    [CreateAssetMenu(fileName = "PauseState", menuName = "GameStates/Pause")]
    public class PauseState_SO : State_SO
    {
        public override void Enter()
        {
            Time.timeScale = 0f;
        }

        public override void Exit()
        {
            Time.timeScale = 1f;
        }
    }
}
