using System;

namespace Utils.Variables_Namespace
{
    [Serializable]
    public class StringReference
    {
        public bool useConstant = true;
        public string constantValue;
        public StringVariable variable;

        public StringReference()
        {
        }

        public StringReference(string value)
        {
            useConstant = true;
            constantValue = value;
        }

        public string value => useConstant ? constantValue : variable.stringValue;

        public static implicit operator string (StringReference reference)
        {
            return reference.value;
        }
    }
}
