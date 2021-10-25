using System;

namespace Utils.Variables_Namespace
{
    [Serializable]
    public class FloatReference
    {
        public bool useConstant = true;
        public float constantValue;
        public FloatVariable variable;

        public FloatReference()
        {
        }

        public FloatReference(float value)
        {
            useConstant = true;
            constantValue = value;
        }

        public float Value => useConstant ? constantValue : variable.floatValue;

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}
