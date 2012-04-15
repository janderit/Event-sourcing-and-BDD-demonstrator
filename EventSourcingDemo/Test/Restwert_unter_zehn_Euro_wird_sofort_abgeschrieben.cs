using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Restwert_unter_zehn_Euro_wird_sofort_abgeschrieben : TestFramework.TestBase
    {
        public override void Configure()
        {
            var id = Guid.NewGuid();

            Given(Past<Events.InvestitionInAnlagevermoegenGetaetigt>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Lieferung, new DateTime(2012, 02, 01))
                      .With(_ => _.Wert, 480)
                      .With(_ => _.Nutzungsjahre, 5),

                  Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Geschaeftsjahr, 2012)
                      .With(_ => _.Abschreibung, 88),

                  Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Geschaeftsjahr, 2013)
                      .With(_ => _.Abschreibung, 96M),

                  Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Geschaeftsjahr, 2014)
                      .With(_ => _.Abschreibung, 96M),

                  Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                      .With(_ => _.Vermoegensgegenstand, id)
                      .With(_ => _.Geschaeftsjahr, 2015)
                      .With(_ => _.Abschreibung, 96M)
                );

            When<Domain.AnlagevermoegenVerzeichnis>(_ => _.Jahresabschluss(2016), "Reguläre Abschreibung in 2016");

            Expect(
                Publication<Events.AnlagevermoegenWurdeAbgeschrieben>()
                    .With(_ => _.Vermoegensgegenstand, id)
                    .With(_ => _.Geschaeftsjahr, 2016)
                    .NotWith(_ => _.IstSonderabschreibung)
                    .With(_ => _.Abschreibung, 104)
                    .With(_ => _.Restwert, 0M),

                    Publication<Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben>()
                    .With(_=>_.Vermoegensgegenstand, id)
                );
        }
    }
}
