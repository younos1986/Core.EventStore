namespace Core.EventStore.UnitTest.Infrastructures
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
