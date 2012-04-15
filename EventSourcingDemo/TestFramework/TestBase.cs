using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.TestFramework
{
    abstract class TestBase
    {

        private List<HistorischesEventWrapper> _given = new List<HistorischesEventWrapper>();
        private List<ErwartetesEventWrapper> _expected;
        private Type _expectedExceptionType;
        private Action _invocation;

        private Exception _caught;
        private IEnumerable<Event> _published;

        public abstract void Configure();

        protected void Given(params HistorischesEventWrapper[] evs)
        {
            _given = evs.ToList();
        }

        protected void When<TAggregate>(Action<TAggregate> invocation, string when) 
            where TAggregate: EventSourcedAggregate, new()
        {
            _when = when;
            _invocation = () =>
                              {
                                  var sut = new TAggregate();
                                  sut.LoadFromHistory(_given.Select(_=>_.Create()));
                                  try
                                  {
                                      invocation(sut);
                                      _published = sut.UncommittedEvents.ToList();
                                      sut.Commit();
                                  } 
                                  catch (Exception ex)
                                  {
                                      _caught = ex;
                                  }
                              };
        }

        protected void Expect(params ErwartetesEventWrapper[] evs)
        {
            _expected = evs.ToList();
        }

        protected void Expect<TException>() where TException : Exception
        {
            _expectedExceptionType = typeof(TException);
        }

        protected static HistorischesEventWrapper<T> Past<T>() where T : Event, new()
        {
            return new HistorischesEventWrapper<T>();
        }

        protected static ErwartetesEventWrapper<T> Publication<T>() where T : Event, new()
        {
            return new ErwartetesEventWrapper<T>();
        }

        public bool RunAndPrint()
        {
            Configure();

            if (_invocation == null) return Fail("Testsubjekt nicht konfiguriert");
            if (_expected == null && _expectedExceptionType == null) return Fail("Testerwartungen nicht konfiguriert");

            _invocation();

            LogGiven();
            Log("");
            LogWhen();
            Log("");

            if (_caught != null || _expectedExceptionType != null)
            {
                if (_caught != null && _caught.GetType() != _expectedExceptionType)
                {
                    Log(_caught.ToString());
                    if (_expectedExceptionType!=null) Log("  (erwartet wurde: "+_expectedExceptionType.Name+")");
                    return Fail("Unerwarteter Fehler: " + _caught.GetType().Name);
                }

                if (_caught == null)
                {
                    Log("  (erwartet wurde: " + _expectedExceptionType.Name + ")");
                    return Fail("Erwarteter Fehler trat nicht auf.");
                }

                Log("[OK] Erwarteter Fehler: "+_caught.Message+" ("+_caught.GetType().Name+")");
                return Print(false, "");

            }
            else
            {
                var fail = false;
                Log("Then:");
                var count = _expected.Count();

                if (count==0) Log("  Es werden keine Ereignisse erwartet.");

                foreach (var ex in _expected)
                {
                    var r = ex.Condition(_published);
                    if (!r) fail = true;
                    Log(" " + (r ? "[OK] " : "[FAIL] ") + ex.Text(_published));
                    var x = ex.AssertCondition(_published);
                    if (x!="") Log("  "+x);
                }

                if (_published.Count() != count)
                {
                    return Fail("Es sind " + _published.Count() + " Ereignisse aufgetreten, jedoch waren " + count + " erwartet.");
                }

                return Print(fail, "");
            }

        }

        private void LogGiven()
        {
            Log("Given:");
            if (_given.Count==0) Log("  <keine Historie>");
            foreach (var g in _given) Log("  " + g.Text());
        }

        private void LogWhen()
        {
            Log("When:");
            Log("  "+_when);
        }

        private List<string> _log = new List<string>();
        private string _when;

        private void Log(string log)
        {
            _log.Add(log);
        }

        private bool Fail(string reason)
        {
            return Print(true, reason);
        }
        
        private bool Print(bool failed, string reason)
        {

            if (!failed)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[PASSED] {0}", GetType().Name.Replace('_', ' '));
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            } else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[FAILED] {0}", GetType().Name.Replace('_', ' '));
                Console.WriteLine("  " + reason);
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }

            foreach(var line in _log) Console.WriteLine("  "+line);

            Console.WriteLine();

            return !failed;

        }



    }
}
