using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Doppelte_Abschreibung_in_einem_Jahr_ist_unzulaessig : TestFramework.TestBase
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
                      .With(_ => _.Abschreibung, 200)
                );

            When<Domain.AnlagevermoegenVerzeichnis>(_ => _.VorzeitigeAbschreibung(id, 2013), "Weitere Abschreibung im Jahr 2013");

            Expect<InvalidOperationException>();
        }
    }
}
