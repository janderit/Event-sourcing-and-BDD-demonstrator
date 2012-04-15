using System;
using System.Linq.Expressions;

namespace EventSourcingDemo.TestFramework
{
    public class PropertyExpectation<T, TValue> : PropertyExpectationBase<T>
    {
        private Expression<Func<T, TValue>> Property { get; set; }
        private TValue Value { get; set; }

        public PropertyExpectation(Expression<Func<T, TValue>> property, TValue value)
        {
            Property = property;
            Value = value;
        }

        public override bool Test(T e)
        {
            return Get(e).Equals(Value);
        }

        private TValue Get(T e)
        {
            return Property.Compile()(e);
        }

        /*public override void Set(T e)
        {
            var member = (MemberExpression)Property.Body;
            typeof(T).GetProperty(member.Member.Name).SetValue(e, Value, null);
        }*/

        public override string PrintOk(T e)
        {
            var member = (MemberExpression)Property.Body;
            return "mit " + member.Member.Name + ": " + Value;
        }

        public override string PrintFailed(T e)
        {
            var member = (MemberExpression)Property.Body;
            if (Test(e)) return member.Member.Name + " (ok)";
            return "mit " + member.Member.Name + ": " + Value + " (jedoch: " + Get(e) + ")";
        }

        public override string PrintExpected()
        {
            var member = (MemberExpression)Property.Body;
            return "mit " + member.Member.Name + ": " + Value;
        }
    }
}