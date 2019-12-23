using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public class Unit : ICloneable
    {
        public List<Modifier> Modifiers { get; }
        public List<Skill> Skills { get; }
        public bool CanCast { get; }
        public string Name { get; }
        public float HitPoints { get; }
        public float Attack { get; }
        public float Defence { get; }
        Tuple<float, float> damageRange;
        public float Damage
        {
            get
            {
                return (damageRange.Item1 + (float)(new Random()).NextDouble() * (damageRange.Item2 - damageRange.Item1));
            }
        }
        public string DamageScatter
        {
            get
            {
                return $"{damageRange.Item1}-{damageRange.Item2}";
            }
        }
        public int Initiative { get; }
        public Unit(string name, float hitPoints, float attack, float defence, Tuple<float, float> damageRange, int initiative, List<Modifier> modifiers, List<Skill> skills)
        {
            this.Name = name;
            if (hitPoints <= 0)
            {
                hitPoints = 1;
            }
            if (attack < 0)
            {
                attack = 0;
            }
            if (defence < 0)
            {
                defence = 0;
            }
            if (damageRange.Item1 < 0 || damageRange.Item2 < 0)
            {
                damageRange = new Tuple<float, float>(0, 0);
            }
            this.HitPoints = hitPoints;
            this.Attack = attack;
            this.Defence = defence;
            this.damageRange = damageRange;
            this.Initiative = initiative;
            this.Modifiers = modifiers;
            this.Skills = skills;
            CanCast = true;
        }
        public object Clone()
        {
            return new Unit(Name, HitPoints, Attack, Defence, damageRange, Initiative, Modifiers, Skills);
        }
        public virtual void ApplyFeatures(BattleUnitsStack originStack) { }
    }
}
