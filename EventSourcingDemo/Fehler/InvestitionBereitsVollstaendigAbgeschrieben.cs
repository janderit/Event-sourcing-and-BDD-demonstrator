using System;

namespace EventSourcingDemo.Fehler
{
    class InvestitionBereitsVollstaendigAbgeschrieben : Exception
    {
        public InvestitionBereitsVollstaendigAbgeschrieben()
            : base("Anlagevermögen war bereits abgeschrieben.")
        {
        }
    }
}