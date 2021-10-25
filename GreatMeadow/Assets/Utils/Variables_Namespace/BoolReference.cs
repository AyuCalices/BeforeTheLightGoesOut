using System;

namespace Utils.Variables_Namespace
{
    [Serializable]
    public class BoolReference
    {
        public bool useConstant = true;
        public bool constantValue;
        public BoolVariable variable;

        public BoolReference()
        {
        }

        public BoolReference(bool value)
        {
            useConstant = true;
            constantValue = value;
        }

        public bool value => useConstant ? constantValue : variable.boolValue;

        public static implicit operator bool(BoolReference reference)
        {
            return reference.value;
        }
    }
}
