using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new FloatVariable", menuName = "Utils/Variables/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        public float floatValue;

        [SerializeField] private float startValue;

        public void SetValue(float value)
        {
            floatValue = value;
        }

        public float GetVariableValue()
        {
            return floatValue;
        }
        
        public void ResetVariable()
        {
            floatValue = startValue;
        }

        public void SetValue(FloatVariable value)
        {
            this.floatValue = value.floatValue;
        }

        public void ApplyChange(float amount)
        {
            floatValue += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            floatValue += amount.floatValue;
        }
    }
}
