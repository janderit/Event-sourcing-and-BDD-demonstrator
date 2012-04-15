using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Keine_Abschreibung_nach_Sonderabschreibung : TestFramework.TestBase
    {
        public override void Configure()
        {
            var id = Guid.NewGuid();

            Given(Past<Events.InvestitionInAnlagevermoegenGetaetigt>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Lieferung, new DateTime(2012, 07, 01))
                      .With(_ => _.Wert, 480)
                      .With(_ => _.Nutzungsjahre, 3),

                  Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Geschaeftsjahr, 2012)
                      .With(_ => _.Abschreibung, 80),

                  Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Geschaeftsjahr, 2013)
                      .With(_ => _.Abschreibung, 400)
                      .With(_=>_.IstSonderabschreibung, true),

                  Past<Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)

                );

            When<Domain.AnlagevermoegenVerzeichnis>(_ => _.Jahresabschluss(2013), "Reguläre Abschreibung nach der Sonderabschreibung");

            Expect();
        }
    }
}
