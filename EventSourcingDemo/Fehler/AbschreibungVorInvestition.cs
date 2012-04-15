using System;

namespace EventSourcingDemo.Fehler
{
    class AbschreibungVorInvestition : Exception
    {
        public AbschreibungVorInvestition()
            : base("Anlagevermögen war noch nicht angeschafft.")
        {
        }
    }
}