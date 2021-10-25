using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new BoolVariable", menuName = "Utils/Variables/BoolVariable")]
    public class BoolVariable : ScriptableObject
    {
        public bool boolValue;

        [SerializeField] private bool startValue;

        public void SetValue(bool value)
        {
            boolValue = value;
        }

        public bool GetVariableValue()
        {
            return boolValue;
        }
        
        public void ResetVariable()
        {
            boolValue = startValue;
        }

        public void SetTrue()
        {
            boolValue = true;
        }

        public void SetFalse()
        {
            boolValue = false;
        }
    }
}