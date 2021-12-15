using UnityEngine;

namespace Features.GameStates.Scripts
{
    [CreateAssetMenu(fileName = "PlayState", menuName = "GameStates/Play")]
    public class PlayState_SO : State_SO
    {
        public override void Enter()
        {
            Debug.Log("enter playstate");
        }
    }
}
