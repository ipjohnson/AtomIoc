using System;
using System.Reflection;

namespace AtomIoc
{
    public class ContainerConfiguration
    {
        public bool SingletonCreatesNewContext { get; set; } = true;

        public bool ScopedCreatesNewContext { get; set; } = true;

        public Func<Type, bool> KeyFunction { get; set; } =
            type => type.GetTypeInfo().IsValueType || type == typeof(string);

        //public bool ProcessMembersForImportAttributes { get; set; } = true;

        public bool TrackTransientsForDisposal { get; set; } = true;

    }
}
