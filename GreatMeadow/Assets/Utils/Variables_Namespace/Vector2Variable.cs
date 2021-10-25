using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new Vector2Variable", menuName = "Utils/Variables/Vector2Variable")]
    public class Vector2Variable : ScriptableObject
    {
        #if unityeditor
        [SerializeField][Multiline] private string developerMessage;
        #endif
        
        public Vector2 vec2Value;

        public void SetValue(Vector2 value)
        {
            vec2Value = value;
        }

        public Vector3 GetVariableValue()
        {
            return vec2Value;
        }

        public void SetValue(Vector2Variable value)
        {
            vec2Value = value.vec2Value;
        }

        public void ApplyChange(Vector2 amount)
        {
            vec2Value += amount;
        }

        public void ApplyChange(Vector2Variable amount)
        {
            vec2Value += amount.vec2Value;
        }
    }
}

