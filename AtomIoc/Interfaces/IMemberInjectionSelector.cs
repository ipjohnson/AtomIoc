using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface IMemberInjectionInfo
    {
        MemberInfo Member { get; }

        Action<InjectionContext> MemberInjectionAction();
    }

    public interface IMemberInjectionSelector
    {
        IEnumerable<IMemberInjectionInfo> GetMembersToInject(IEnumerable<MemberInfo> members);
    }
}
