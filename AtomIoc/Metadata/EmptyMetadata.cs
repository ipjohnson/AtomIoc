using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;

namespace AtomIoc.Metadata
{
    public class EmptyMetadata : IMetadata
    {
        /// <summary>
        /// Empty metadata 
        /// </summary>
        public static readonly EmptyMetadata Instance = new EmptyMetadata();


        public bool Has(object key)
        {
            return false;
        }

        public T GetValueOrDefault<T>(object key, T defaultT = default(T))
        {
            return defaultT;
        }
    }
}
