using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Sonderabschreibung_fuehrt_zu_vollstaendiger_Abschreibung : TestFramework.TestBase
    {
        public override void Configure()
        {
            var id = Guid.NewGuid();

            Given(
                Past<Events.InvestitionInAnlagevermoegenGetaetigt>()
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

            When<Domain.AnlagevermoegenVerzeichnis>(_ => _.VorzeitigeAbschreibung(id, 2013), "Vorzeitige Abschreibung in 2013");

            Expect(
                Publication<Events.AnlagevermoegenWurdeAbgeschrieben>()
                    .With(_ => _.Vermoegensgegenstand, id)
                    .With(_ => _.Geschaeftsjahr, 2013)
                    .With(_ => _.IstSonderabschreibung)
                    .With(_ => _.Abschreibung, 956.67M)
                    .With(_ => _.Restwert, 0),

                    Publication<Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben>()
                    .With(_=>_.Vermoegensgegenstand, id)
                );
        }
    }
}
