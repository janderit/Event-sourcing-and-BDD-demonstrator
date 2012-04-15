using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.TestFramework
{
    public abstract class HistorischesEventWrapper
    {
        public abstract Event Create();
        public abstract string Text();
    }

    public class HistorischesEventWrapper<T> : HistorischesEventWrapper where T : Event, new()
    {
        private readonly List<HistorischesEventProperty<T>> _properties = new List<HistorischesEventProperty<T>>();

        public override Event Create()
        {
            var result = new T();
            foreach (var p in _properties) p.Set(result);
            return result;
        }

        public HistorischesEventWrapper<T> With<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            _properties.Add(new HistorischesEventProperty<T, TValue>(property, value));
            return this;
        }
        

        public override string Text()
        {
            return _properties.Aggregate(Create().ToString(), (current, pe) => current + ", " + (pe.Print()));
        }

    }

    internal class HistorischesEventProperty<T, TValue> : HistorischesEventProperty<T>
    {
        private Expression<Func<T, TValue>> Property { get; set; }
        private TValue Value { get; set; }

        public HistorischesEventProperty(Expression<Func<T, TValue>> property, TValue value)
        {
            Property = property;
            Value = value;
        }

        public override void Set(T e)
        {
            var member = (MemberExpression)Property.Body;
            typeof(T).GetProperty(member.Member.Name).SetValue(e, Value, null);
        }

        public override string Print()
        {
            var member = (MemberExpression)Property.Body;
            return "mit " + member.Member.Name + ": " + Value;
        }
    }
}
