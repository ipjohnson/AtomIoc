using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Data
{
    public class KeyedType
    {
        private readonly int _hashcode;

        public KeyedType(Type type, object key)
        {
            Type = type;
            Key = key;
            _hashcode = (527 + type.GetHashCode()) * 31 + key.GetHashCode();
        }

        public Type Type { get; }

        public object Key { get; }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (obj is KeyedType compareKey)
            {
                return compareKey.Type == Type && compareKey.Key == Key;
            }

            return false;
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _hashcode;
        }
    }
}
