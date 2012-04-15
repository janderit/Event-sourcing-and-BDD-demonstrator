using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Restwert_Abschreibung_im_letzten_Jahr : TestFramework.TestBase
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
                      .With(_ => _.Abschreibung, 160),

                  Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Geschaeftsjahr, 2014)
                      .With(_ => _.Abschreibung, 160)

                );

            When<Domain.AnlagevermoegenVerzeichnis>(_ => _.Jahresabschluss(2015), "Reguläre Abschreibung in 2015");

            Expect(
                Publication<Events.AnlagevermoegenWurdeAbgeschrieben>()
                    .With(_ => _.Vermoegensgegenstand, id)
                    .With(_ => _.Geschaeftsjahr, 2015)
                    .NotWith(_ => _.IstSonderabschreibung)
                    .With(_ => _.Abschreibung, 80)
                    .With(_ => _.Restwert, 0M),

                    Publication<Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben>()
                    .With(_ => _.Vermoegensgegenstand, id)
                );
        }
    }
}
