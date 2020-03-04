using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.EventStore.Test.Infrastructures
{
    public abstract class GivenWhenThen 
    {
        public GivenWhenThen()
        {
            Given();   
            When();
        }

        protected abstract void Given();

        protected abstract void When();

        

    }
}
