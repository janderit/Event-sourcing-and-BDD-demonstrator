using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Lineare_Abschreibung_waehrend_der_Nutzungsdauer : TestFramework.TestBase
    {
        public override void Configure()
        {
            var id = Guid.NewGuid();

            Given(Past<Events.InvestitionInAnlagevermoegenGetaetigt>()
                .With(_ => _.Vermoegensgegenstand, id)
                .With(_ => _.Lieferung, new DateTime(2012, 05, 08))
                .With(_ => _.Wert, 1230)
                .With(_ => _.Nutzungsjahre, 3),
                Past<Events.AnlagevermoegenWurdeAbgeschrieben>()
                    .With(_ => _.Vermoegensgegenstand, id)
                    .With(_ => _.Geschaeftsjahr, 2012)
                    .With(_ => _.Abschreibung, 273.33M)
                    .With(_ => _.Restwert, 956.67M)
                );

            When<Domain.AnlagevermoegenVerzeichnis>(_ => _.Jahresabschluss(2013), "Reguläre Abschreibung in 2013");

            Expect(
                Publication<Events.AnlagevermoegenWurdeAbgeschrieben>()
                    .With(_ => _.Vermoegensgegenstand, id)
                    .With(_ => _.Geschaeftsjahr, 2013)
                    .NotWith(_ => _.IstSonderabschreibung)
                    .With(_ => _.Abschreibung, 410M)
                    .With(_ => _.Restwert, 546.67M)
                );
        }
    }
}
