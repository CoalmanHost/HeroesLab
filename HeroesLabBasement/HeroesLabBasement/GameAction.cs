using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public class GameAction
    {
        public GameActionType Type { get; set; }
        public BattleUnitsStack Subject { get; }
        public BattleUnitsStack Object { get; }

        public GameAction(BattleUnitsStack subject, BattleUnitsStack _object)
        {
            Subject = subject;
            Object = _object;
        }

        public delegate void AttackHandler();
        public event AttackHandler Attacked;
        public event AttackHandler Revenged;
        public void Fight()
        {
            ApplyModifiers();
            BattleUnitsStack attacker = Subject;
            BattleUnitsStack target = Object;
            attacker.AttackStack(target);
            Attacked?.Invoke();
            if (target.canRetaliate)
            {
                target.AttackStack(attacker);
            }
            target.canRetaliate = false;
            Revenged?.Invoke();
        }

        public delegate void SkillCastHandler();
        public event SkillCastHandler Casted;
        public void Cast(Skill skill)
        {
            ApplyModifiers();
            skill.Use(Subject, Object);
            Casted?.Invoke();
        }
        void ApplyModifiers()
        {
            foreach (var item in Subject.UnitClass.Modifiers)
            {
                item.Apply(this);
            }
            foreach (var item in Object.UnitClass.Modifiers)
            {
                item.Apply(this);
            }
        }
    }
}
