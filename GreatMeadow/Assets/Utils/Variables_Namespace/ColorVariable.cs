using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new ColorVariable", menuName = "Utils/Variables/ColorVariable")]
    public class ColorVariable : ScriptableObject
    {
        public Color colorValue;

        [SerializeField] private Color startColor;

        public void SetValue(Color value)
        {
            colorValue = value;
        }

        public Color GetVariableValue()
        {
            return colorValue;
        }
        
        public void ResetVariable()
        {
            colorValue = startColor;
        }

        public Color GetValue()
        {
            return colorValue;
        }
    }
}
