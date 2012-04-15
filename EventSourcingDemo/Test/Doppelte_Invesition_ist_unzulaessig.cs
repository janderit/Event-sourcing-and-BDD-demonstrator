using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Test
{
    class Doppelte_Invesition_ist_unzulaessig : TestFramework.TestBase
    {
        public override void Configure()
        {
            var id = Guid.NewGuid();

            Given(Past<Events.InvestitionInAnlagevermoegenGetaetigt>()
                .With(_=>_.Vermoegensgegenstand, id));

            When<Domain.AnlagevermoegenVerzeichnis>(_=>_.ZugangDurchInvestition(id, DateTime.Now, 2, 1000), "Investition mit gleicher ID");

            Expect<InvalidOperationException>();
        }
    }
}
