using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Fehler
{
    class AnlagevermoegenNichtGefunden : Exception
    {
        public AnlagevermoegenNichtGefunden() : base("Anlagevermögen nicht gefunden.")
        {
        }
    }
}
