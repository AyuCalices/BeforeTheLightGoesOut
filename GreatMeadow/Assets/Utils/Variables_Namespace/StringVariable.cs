using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new StringVariable", menuName = "Utils/Variables/StringVariable")]
    public class StringVariable : ScriptableObject    {
        
        public string stringValue;

        [SerializeField] private string startValue;
        
        public void SetValue(string value)
        {
            stringValue = value;
        }

        public string GetVariableValue()
        {
            return stringValue;
        }
        
        public void ResetVariable()
        {
            stringValue = startValue;
        }

        public void SetValue(StringVariable value)
        {
            this.stringValue = value.stringValue;
        }

        public void ApplyChange(string amount)
        {
            stringValue += amount;
        }

        public void ApplyChange(StringVariable amount)
        {
            stringValue += amount.stringValue;
        }
    }
}
