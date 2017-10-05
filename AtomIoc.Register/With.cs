using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Register
{
    public static class With
    {
        #region CtorParam



        #endregion

        #region Metadata

        public static KeyValuePair<object, object> Metadata(object key, object value) 
            => new KeyValuePair<object, object>(key, value);

        #endregion

        #region Condition

        public static Conditions Condition => new Conditions();

        #endregion

    }
}
