using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.Domain
{
    class InvestitionState
    {
        private readonly Action<Event> _publisher;

        public int BeschaffungsJahr { get; private set; }
        public int Nutzungsdauer { get; private set; }
        public DateTime Lieferdatum { get; private set; }
        public decimal Beschaffungswert { get; private set; }
        public decimal Restwert { get; private set; }
        public Guid Id { get; private set; }
        public IEnumerable<int> AbgeschriebeneZeitraeume { get { return _abgeschriebeneZeitraeume.ToList(); } }

        private List<int> _abgeschriebeneZeitraeume = new List<int>();

        internal InvestitionState(Events.InvestitionInAnlagevermoegenGetaetigt e, Action<Event> publisher)
        {
            _publisher = publisher;
            Apply(e);
        }

        internal void Apply(Events.InvestitionInAnlagevermoegenGetaetigt e)
        {
            Id = e.Vermoegensgegenstand;
            Beschaffungswert = e.Wert;
            BeschaffungsJahr = e.Lieferung.Year;
            Restwert = e.Wert;
            Lieferdatum = e.Lieferung;
            Nutzungsdauer = e.Nutzungsjahre;
        }

        internal void Apply(Events.AnlagevermoegenWurdeAbgeschrieben e)
        {
            Restwert -= e.Abschreibung;
            _abgeschriebeneZeitraeume.Add(e.Geschaeftsjahr);
        }

        internal void Apply(Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben e){}
    }
}
