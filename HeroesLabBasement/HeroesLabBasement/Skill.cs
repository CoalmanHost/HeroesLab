using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public enum SkillType
    {
        Self,
        Allies,
        AllAllies,
        Enemies,
        AllEnemies,
        All
    }
    public abstract class Skill : IEquatable<Skill>
    {
        public bool Used { get; set; }
        public abstract SkillType Type { get; }
        public abstract string Name { get; }
        protected abstract void Cast(BattleUnitsStack subject, BattleUnitsStack target);
        internal void Use(BattleUnitsStack subject, BattleUnitsStack target)
        {
            Cast(subject, target);
            Used = true;
        }
        public bool Equals(Skill other)
        {
            return Name == other.Name;
        }
    }
}
