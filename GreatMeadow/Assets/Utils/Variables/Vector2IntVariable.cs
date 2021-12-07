using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewVector3Variable", menuName = "Utils/Variables/Vector3Variable")]
    public class Vector2IntVariable : AbstractVariable<Vector2Int>
    {
        public void Add(Vector2Int value)
        {
            runtimeValue += value;
            onValueChanged.Raise();
        }

        public void Add(Vector2IntVariable value)
        {
            runtimeValue += value.runtimeValue;
            onValueChanged.Raise();
        }
    }
}

