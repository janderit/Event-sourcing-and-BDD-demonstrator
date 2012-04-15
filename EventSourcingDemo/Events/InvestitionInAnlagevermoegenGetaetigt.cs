using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.Events
{
    public class InvestitionInAnlagevermoegenGetaetigt : Event
    {
        public Guid Vermoegensgegenstand { get; set; }
        public Decimal Wert { get; set; }
        public DateTime Lieferung { get; set; }
        public int Nutzungsjahre { get; set; }

        public override string ToString()
        {
            return "Eine Investition in das Anlagevermögen wurde vorgenommen";
        }
    }
}
