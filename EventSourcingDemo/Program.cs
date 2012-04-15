using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.TestFramework;

namespace EventSourcingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new TestBase[]
                           {
                               new Test.Keine_Abschreibung_vor_der_Anschaffung(),
                               new Test.Monatsanteilige_Abschreibung_im_Anschaffungsjahr(),
                               new Test.Monatsanteilige_Abschreibung_im_Anschaffungsjahr_Grenzfall_Januar(),
                               new Test.Monatsanteilige_Abschreibung_im_Anschaffungsjahr_Grenzfall_Dezember(),
                               new Test.Lineare_Abschreibung_waehrend_der_Nutzungsdauer(),
                               new Test.Restwert_unter_zehn_Euro_wird_sofort_abgeschrieben(),
                               new Test.Restwert_Abschreibung_im_letzten_Jahr(),
                               new Test.Sonderabschreibung_fuehrt_zu_vollstaendiger_Abschreibung(),
                               new Test.Keine_Abschreibung_fuer_unbekannte_Vermoegensgegenstaende(),
                               new Test.Keine_Abschreibung_nach_Sonderabschreibung(),
                               new Test.Keine_Sonderabschreibung_nach_vollstaendiger_Abschreibung(),
                               new Test.Doppelte_Abschreibung_in_einem_Jahr_ist_unzulaessig(),
                               new Test.Doppelte_Invesition_ist_unzulaessig()
                           };

            var failed = test.Any(_ => !_.RunAndPrint());

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = failed ? ConsoleColor.Red : ConsoleColor.DarkGreen;
            Console.WriteLine(failed ? "FAILED" : (test.Count())+" tests PASSED");
            Console.ReadLine();
        }
    }
}
