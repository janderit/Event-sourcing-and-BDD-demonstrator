using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingDemo.Domain
{
    class AbsetzungFuerAbnutzungBerechnungsDienst
    {

        /* implementiert lineare AfA gem. deutschem Steuerrecht 2011 */

        public static decimal Plan(decimal startwert, int nutzungsjahre, DateTime beginn, int jahr)
        {
            var normaleAbsetzung = Decimal.Round(startwert / (1.0M * nutzungsjahre), 2);

            var monateimerstenjahr = 12 - beginn.Month + 1;
            var ersteAbsetzung = Decimal.Round(normaleAbsetzung * (monateimerstenjahr / 12M), 2);
            var ueberhangImLetztenJahr = normaleAbsetzung - ersteAbsetzung;

            var betrachtetesJahr = jahr - beginn.Year;

            if (betrachtetesJahr == 0) return ersteAbsetzung;
            if (0 < betrachtetesJahr && betrachtetesJahr < nutzungsjahre) return normaleAbsetzung;
            if (betrachtetesJahr == nutzungsjahre) return ueberhangImLetztenJahr;

            return 0;
        }
    }
}
