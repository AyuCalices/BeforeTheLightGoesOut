using DataStructures.Variables;
using UnityEngine;

namespace Utils.Variables
{
    [CreateAssetMenu(fileName = "NewBoolVariable", menuName = "Utils/Variables/BoolVariable")]
    public class BoolVariable : AbstractVariable<bool>
    {
        public void Toggle()
        {
            Set(!runtimeValue);
        }
    }
}