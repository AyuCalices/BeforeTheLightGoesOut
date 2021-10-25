using System;

namespace Utils.Variables_Namespace
{
    [Serializable]
    public class DoubleReference
    {
        private bool useConstant = true;
        private double constantValue;
        public DoubleVariable variable;

        public DoubleReference()
        {
        }

        public DoubleReference(double value)
        {
            useConstant = true;
            constantValue = value;
        }

        public double Value => useConstant ? constantValue : variable.doubleValue;

        public static implicit operator double(DoubleReference reference)
        {
            return reference.Value;
        }
    }
}
