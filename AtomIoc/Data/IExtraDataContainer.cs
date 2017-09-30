using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Data
{
    public interface IExtraDataContainer
    {
        /// <summary>
        /// Enumeration of all the key value pairs
        /// </summary>
        IEnumerable<KeyValuePair<object, object>> KeyValuePairs { get; }

        /// <summary>
        /// Extra data associated with the injection request. 
        /// </summary>
        /// <param name="key">key of the data object to get</param>
        /// <returns>data value</returns>
        object GetExtraData(object key);

        /// <summary>
        /// Sets extra data on the injection context
        /// </summary>
        /// <param name="key">object name</param>
        /// <param name="newValue">new object value</param>
        /// <param name="replaceIfExists">replace value if key exists</param>
        /// <returns>the final value of key</returns>
        object SetExtraData(object key, object newValue, bool replaceIfExists = true);
    }
}
