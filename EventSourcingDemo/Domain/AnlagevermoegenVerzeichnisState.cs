using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventSourcingDemo.Base;

namespace EventSourcingDemo.Domain
{
    class AnlagevermoegenVerzeichnisState
    {
        private readonly Action<Event> _publisher;
        
        private readonly Dictionary<Guid, Investition> _investitionen = new Dictionary<Guid, Investition>();
        private readonly List<Guid> _ehemalige = new List<Guid>();

        public IEnumerable<Investition> AktiveInvestitionen
        {
            get { return _investitionen.Select(_ => _.Value).ToList(); }
        }

        public bool IstBekannt(Guid id)
        {
            return _investitionen.ContainsKey(id) || _ehemalige.Contains(id);
        }

        public bool IstAktiv(Guid id)
        {
            return _investitionen.ContainsKey(id);
        }

        public Investition Investition(Guid id)
        {
            if (!_investitionen.ContainsKey(id)) throw new Fehler.AnlagevermoegenNichtGefunden();
            return _investitionen[id];
        }

        internal AnlagevermoegenVerzeichnisState(Action<Event> publisher)
        {
            _publisher = publisher;
        }

        internal void Apply(Events.InvestitionInAnlagevermoegenGetaetigt e)
        {
            _investitionen.Add(e.Vermoegensgegenstand, new Investition(e, _publisher));
        }

        internal void Apply(Events.AnlagevermoegenWurdeAbgeschrieben e)
        {
            _investitionen[e.Vermoegensgegenstand].Dispatch(e);
        }

        internal void Apply(Events.AnlagevermoegenWurdeVollstaendigAbgeschrieben e)
        {
            _investitionen.Remove(e.Vermoegensgegenstand);
            _ehemalige.Add(e.Vermoegensgegenstand);
        }
    }
}
