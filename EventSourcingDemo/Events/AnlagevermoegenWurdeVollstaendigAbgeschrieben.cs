using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.Events
{
    public class AnlagevermoegenWurdeVollstaendigAbgeschrieben : Event
    {
        public Guid Vermoegensgegenstand { get; set; }

        public override string ToString()
        {
            return "Anlagevermögen wurde vollständig abgeschrieben";
        }

    }
}
