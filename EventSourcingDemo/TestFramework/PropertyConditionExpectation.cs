using System;
using System.Linq.Expressions;

namespace EventSourcingDemo.TestFramework
{
    public class PropertyConditionExpectation<T, TValue> : PropertyExpectationBase<T>
    {
        private readonly string _info;
        private Expression<Func<T, TValue>> Property { get; set; }
        private Func<TValue, bool> Condition { get; set; }

        public PropertyConditionExpectation(Expression<Func<T, TValue>> property, Func<TValue, bool> condition, string info)
        {
            _info = info;
            Property = property;
            Condition = condition;
        }

        public override bool Test(T e)
        {
            return Condition(Get(e));
        }

        private TValue Get(T e)
        {
            return Property.Compile()(e);
        }

        public override string PrintOk(T e)
        {
            var member = (MemberExpression)Property.Body;
            return "mit " + member.Member.Name + ": " + _info;
        }

        public override string PrintFailed(T e)
        {
            var member = (MemberExpression)Property.Body;
            if (Test(e)) return member.Member.Name + " (ok)";
            return "mit " + member.Member.Name + ": " + _info;
        }

        public override string PrintExpected()
        {
            var member = (MemberExpression)Property.Body;
            return "mit " + member.Member.Name + ": " + _info;
        }
    }
}