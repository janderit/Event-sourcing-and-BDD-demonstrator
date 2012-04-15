using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.TestFramework
{
    public abstract class ErwartetesEventWrapper 
    {
        public abstract Func<IEnumerable<Event>, bool> Condition { get; }
        public abstract Func<IEnumerable<Event>, string> Text { get; }
        public abstract string AssertCondition(IEnumerable<Event> ev);
    }

    public class ErwartetesEventWrapper<T> : ErwartetesEventWrapper where T : new()
    {
        private readonly List<PropertyExpectationBase<T>> _expectations = new List<PropertyExpectationBase<T>>();

        public override Func<IEnumerable<Event>, bool> Condition
        {
            get { return evs => evs.OfType<T>().Count(MatchesExactly) == 1; }
        }

        private bool MatchesExactly(T arg)
        {
            return _expectations.All(pe => pe.Test(arg));
        }

        public override Func<IEnumerable<Event>, string> Text
        {
            get
            {
                return evs =>
                {
                    var kand = evs.OfType<T>().Where(MatchesExactly);
                    if (kand.Count() > 0)
                    {
                        return PrintOk(kand);
                    }
                    kand = evs.OfType<T>();
                    if (kand.Count() > 0)
                    {
                        return PrintFail(kand);
                    }
                    return PrintExpected();
                };
            }
        }



        public override string AssertCondition(IEnumerable<Event> ev)
        {
            var kand = ev.OfType<T>().Where(MatchesExactly);
            if (kand.Count() == 1)
            {
                return "";
            }
            if (kand.Count() > 1)
            {
                return("Zu viele Ereignisse " + typeof(T).Name + " gefunden (erwartet: 1, gefunden: " + (kand.Count()) + ").");
            }
            kand = ev.OfType<T>();
            if (kand.Count() > 0)
            {
                return("Kein passendes Ereignis " + typeof(T).Name + " aufgetreten");
            }
            return("Kein Ereignis " + typeof(T).Name + " aufgetreten");
        }

        private string PrintOk(IEnumerable<T> kand)
        {
            var lines = new Dictionary<string, int>();
            foreach (var l in from k in kand
                              let l = _expectations.Aggregate(k + " [{0}]", (current, pe) => current + (", " + pe.PrintOk(k)))
                              select l)
            {
                if (lines.ContainsKey(l)) lines[l]++; else lines.Add(l, 1);
            }

            return lines.Aggregate("", (current, l) => current + String.Format(l.Key, l.Value) + Environment.NewLine).Trim();
        }

        private string PrintFail(IEnumerable<T> kand)
        {
            var lines = new Dictionary<string, int>();
            foreach (var l in from k in kand
                              let l = _expectations.Aggregate("", (current, pe) => current + (", " + pe.PrintFailed(k)))
                              select l + k)
            {
                if (lines.ContainsKey(l)) lines[l]++; else lines.Add(l, 1);
            }

            return lines.Aggregate(PrintExpected() + Environment.NewLine, (current, l) => current + (typeof(T).Name + " [" + l.Value + "] " + l.Key) + Environment.NewLine).Trim();
        }

        private string PrintExpected()
        {
            return _expectations.Aggregate(new T().ToString(), (current, pe) => current + ", " + (pe.PrintExpected()));
        }

        public ErwartetesEventWrapper<T> With<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            _expectations.Add(new PropertyExpectation<T, TValue>(property, value));
            return this;
        }

        public ErwartetesEventWrapper<T> With<TValue>(Expression<Func<T, TValue>> property, Func<TValue, bool> condition, string info)
        {
            _expectations.Add(new PropertyConditionExpectation<T, TValue>(property, condition, info));
            return this;
        }

        public ErwartetesEventWrapper<T> With(Expression<Func<T, bool>> property)
        {
            return With(property, true);
        }

        public ErwartetesEventWrapper<T> NotWith(Expression<Func<T, bool>> property)
        {
            return With(property, false);
        }

    }
}
