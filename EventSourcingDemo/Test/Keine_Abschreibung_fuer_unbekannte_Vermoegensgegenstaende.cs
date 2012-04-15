using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Keine_Abschreibung_fuer_unbekannte_Vermoegensgegenstaende : TestFramework.TestBase
    {
        public override void Configure()
        {
            Given();

            When<Domain.AnlagevermoegenVerzeichnis>(_=>_.VorzeitigeAbschreibung(Guid.NewGuid(), 2012), "Abschreibung eines unbekannten Vermögensgegenstandes");

            Expect<Fehler.AnlagevermoegenNichtGefunden>();
        }
    }
}
