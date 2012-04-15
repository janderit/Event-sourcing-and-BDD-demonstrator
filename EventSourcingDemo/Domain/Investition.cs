using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.Domain
{
    class Investition
    {
        private readonly Action<Event> _publisher;
        private readonly InvestitionState _state;

        public Investition(Events.InvestitionInAnlagevermoegenGetaetigt e, Action<Event> publisher)
        {
            _publisher = publisher;
            _state = new InvestitionState(e, publisher);
        }

        public void Dispatch(Event e)
        {
            _state.Apply((dynamic) e);
        }

        public void VorzeitigeAbschreibung(int geschaeftsjahr)
        {
            if (geschaeftsjahr < _state.BeschaffungsJahr) throw new Fehler.AbschreibungVorInvestition();
            if (_state.AbgeschriebeneZeitraeume.Contains(geschaeftsjahr)) throw new InvalidOperationException("Im Jahr "+geschaeftsjahr+" wurde bereits eine Abschreibung vorgenommen.");

            _publisher(new Events.AnlagevermoegenWurdeAbgeschrieben
            {
                Geschaeftsjahr = geschaeftsjahr,
                Abschreibung = _state.Restwert,
                IstSonderabschreibung = true,
                Restwert = 0,
                Vermoegensgegenstand = _state.Id
            });

            _publisher(new Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben()
            {
                Vermoegensgegenstand = _state.Id
            });

        }

        public void RegulaereAbschreibungZumJahresabschluss(int geschaeftsjahr)
        {
            if (_state.AbgeschriebeneZeitraeume.Contains(geschaeftsjahr)) return;

            var afa = AbsetzungFuerAbnutzungBerechnungsDienst.Plan(_state.Beschaffungswert, _state.Nutzungsdauer, _state.Lieferdatum, geschaeftsjahr);

            var restwert = _state.Restwert - afa;

            if (restwert<10)
            {
                afa = _state.Restwert;
                restwert = 0;
            }

            _publisher(new Events.AnlagevermoegenWurdeAbgeschrieben
                           {
                               Geschaeftsjahr = geschaeftsjahr,
                               Abschreibung = afa,
                               IstSonderabschreibung = false,
                               Restwert = restwert,
                               Vermoegensgegenstand = _state.Id
                           });

            if (_state.Restwert == 0) _publisher(new Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben { Vermoegensgegenstand = _state.Id });
        }
    }
}
