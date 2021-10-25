using System;
using UnityEngine;

namespace Utils.Variables_Namespace
{
    [CreateAssetMenu(fileName = "new IntVariable", menuName = "Utils/Variables/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        public int intValue;


        public void SetValue(int value)
        {
            intValue = value;
        }

        public int GetVariableValue()
        {
            return intValue;
        }

        public void SetValue(IntVariable value)
        {
            this.intValue = value.intValue;
        }

        public void ApplyChange(int amount)
        {
            intValue += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            intValue += amount.intValue;
        }
    }
}
