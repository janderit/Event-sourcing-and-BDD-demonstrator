using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Domain;
using EventSourcingDemo.TestFramework;

namespace EventSourcingDemo.Test
{
    class Keine_Abschreibung_vor_der_Anschaffung : TestBase
    {
        private readonly DateTime _datum;

        public Keine_Abschreibung_vor_der_Anschaffung()
        {
            _datum = DateTime.Now;
        }

        public override void Configure()
        {
            var id = Guid.NewGuid();

            Given(Past<Events.InvestitionInAnlagevermoegenGetaetigt>()
                .With(_=>_.Vermoegensgegenstand, id)
                .With(_=>_.Lieferung, _datum)
                );

            When<AnlagevermoegenVerzeichnis>(_=>_.VorzeitigeAbschreibung(id, _datum.Year-1), "Abschreibung dieser Investition für das Jahr "+(_datum.Year-1));

            Expect<Fehler.AbschreibungVorInvestition>();
        }
    }
}
