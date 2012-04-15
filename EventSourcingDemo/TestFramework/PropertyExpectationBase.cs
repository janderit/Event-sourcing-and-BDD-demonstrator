namespace EventSourcingDemo.TestFramework
{
    public abstract class PropertyExpectationBase<T>
    {
        public abstract bool Test(T e);
        public abstract string PrintOk(T e);
        public abstract string PrintFailed(T e);
        public abstract string PrintExpected();
    }
}