using UnityEngine;

namespace Features.GameStates_Namespace.Scripts.States
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
