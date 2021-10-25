using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new Vector3Variable", menuName = "Utils/Variables/Vector3Variable")]
    public class Vector3Variable : ScriptableObject
    {
        public Vector3 vec3Value;

        public void SetValue(Vector3 value)
        {
            vec3Value = value;
        }

        public Vector3 GetVariableValue()
        {
            return vec3Value;
        }

        public void SetValue(Vector3Variable value)
        {
            vec3Value = value.vec3Value;
        }

        public void ApplyChange(Vector3 amount)
        {
            vec3Value += amount;
        }

        public void ApplyChange(Vector3Variable amount)
        {
            vec3Value += amount.vec3Value;
        }
    }
}

