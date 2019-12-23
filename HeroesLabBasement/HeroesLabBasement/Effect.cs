using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public enum EffectType
    {
        Natural,
        OnTurn,
        Periodic,
        OnBattle
    }
    public abstract class Effect
    {
        public abstract string Name { get; }
        public abstract EffectType Type { get; }
        public enum EffectDurationType
        {
            None,
            Turn,
            Round
        }
        public abstract EffectDurationType DurationType { get; }
        public int Duration { get; protected set; }
        public void Proceed()
        {
            Duration--;
        }
        public abstract void Remove();
        protected BattleUnitsStack master;
        protected abstract void Apply(BattleUnitsStack master);
        internal void ApplyEffect(BattleUnitsStack master)
        {
            this.master = master;
            Apply(master);
        }
    }
}
