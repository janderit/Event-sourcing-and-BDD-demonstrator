using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.Domain
{

    class AnlagevermoegenVerzeichnis : EventSourcedAggregate
    {

        private readonly AnlagevermoegenVerzeichnisState _state;

        public AnlagevermoegenVerzeichnis()
        {
            _state = new AnlagevermoegenVerzeichnisState(Publish);
        }

        protected override void Dispatch(Event e)
        {
            _state.Apply((dynamic) e);
        }

        public void ZugangDurchInvestition(Guid id, DateTime lieferzeitpunkt, int abschreibungsdauerInJahren, decimal anfangswert)
        {
            if (_state.IstBekannt(id)) throw new InvalidOperationException("Doppelte Investition in einen Vermögensgegenstand.");
        }

        public void Jahresabschluss(int geschaeftsjahr)
        {
            foreach (var investition in _state.AktiveInvestitionen) investition.RegulaereAbschreibungZumJahresabschluss(geschaeftsjahr);
        }

        public void VorzeitigeAbschreibung(Guid id, int geschaeftsjahr)
        {
            if (!_state.IstBekannt(id)) throw new Fehler.AnlagevermoegenNichtGefunden();
            if (!_state.IstAktiv(id)) throw new Fehler.InvestitionBereitsVollstaendigAbgeschrieben();
            _state.Investition(id).VorzeitigeAbschreibung(geschaeftsjahr);
        }
    }
}
