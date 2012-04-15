namespace EventSourcingDemo.TestFramework
{
    internal abstract class HistorischesEventProperty<T>
    {
        public abstract void Set(T e);
        public abstract string Print();
    }
}