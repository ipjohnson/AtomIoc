using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Tests.Classes
{
    public interface IDependsOnBasicService
    {
        IBasicService BasicService { get; }
    }

    public class DependsOnBasicService : IDependsOnBasicService
    {
        public DependsOnBasicService(IBasicService basicService)
        {
            BasicService = basicService;
        }

        public IBasicService BasicService { get; }
    }
}
