using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new DoubleVariable", menuName = "Utils/Variables/DoubleVariable")]
    public class DoubleVariable : ScriptableObject
    {
        public double doubleValue;

        [SerializeField] private double startValue;

        public void SetValue(double value)
        {
            doubleValue = value;
        }

        public double GetVariableValue()
        {
            return doubleValue;
        }

        public void ResetVariable()
        {
            doubleValue = startValue;
        }

        public void SetValue(DoubleVariable value)
        {
            doubleValue = value.doubleValue;
        }

        public void ApplySubtractionChange(DoubleVariable amount)
        {
            doubleValue -= amount.doubleValue;
        }

        public void ApplyAdditionChange(DoubleVariable amount)
        {
            doubleValue += amount.doubleValue;
        }
    }
}
