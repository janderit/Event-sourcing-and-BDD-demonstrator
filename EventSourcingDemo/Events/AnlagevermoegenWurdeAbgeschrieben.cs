using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.Events
{
    public class AnlagevermoegenWurdeAbgeschrieben : Event
    {
        public Guid Vermoegensgegenstand { get; set; }
        public int Geschaeftsjahr { get; set; }
        public Decimal Abschreibung { get; set; }
        public Decimal Restwert { get; set; }
        public bool IstSonderabschreibung { get; set; }

        public override string ToString()
        {
            return "Eine Absetzung für Abnutzung wurde vorgenommen";
        }

    }
}
