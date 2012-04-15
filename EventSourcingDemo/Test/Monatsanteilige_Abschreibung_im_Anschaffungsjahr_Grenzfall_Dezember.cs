using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Monatsanteilige_Abschreibung_im_Anschaffungsjahr_Grenzfall_Dezember : TestFramework.TestBase
    {
        public override void Configure()
        {
            var id = Guid.NewGuid();

            Given(Past<Events.InvestitionInAnlagevermoegenGetaetigt>()
                .With(_ => _.Vermoegensgegenstand, id)
                .With(_ => _.Lieferung, new DateTime(2012, 12, 31))
                .With(_ => _.Wert, 1230)
                .With(_ => _.Nutzungsjahre, 3)
                );

            When<Domain.AnlagevermoegenVerzeichnis>(_ => _.Jahresabschluss(2012), "Reguläre Abschreibung im Anschaffungsjahr");

            Expect(
                Publication<Events.AnlagevermoegenWurdeAbgeschrieben>()
                    .With(_ => _.Vermoegensgegenstand, id)
                    .With(_ => _.Geschaeftsjahr, 2012)
                    .NotWith(_ => _.IstSonderabschreibung)
                    .With(_ => _.Abschreibung, 34.17M)
                    .With(_ => _.Restwert, 1195.83M)
                );
        }
    }
}
