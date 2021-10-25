using System;
using UnityEngine;

namespace Utils.Variables_Namespace
{
    [Serializable]
    public class Vector3Reference
    {
        public bool useConstant = true;
        public Vector3 constantValue;
        public Vector3Variable variable;

        public Vector3Reference()
        {
        }

        public Vector3Reference(Vector3 value)
        {
            useConstant = true;
            constantValue = value;
        }

        public Vector3 value => useConstant ? constantValue : variable.vec3Value;

        public static implicit operator Vector3(Vector3Reference reference)
        {
            return reference.value;
        }
    }
}
