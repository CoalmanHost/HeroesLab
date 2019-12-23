using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public class BattleUnitsStack : UnitsStack
    {
        public BattleArmy Side { get; set; }
        public int StartCount { get; private set; }
        public float LastUnitHitPoints { get; private set; }
        public bool canRetaliate { get; set; }
        public List<Effect> ActiveEffects { get; }
        public delegate void ActiveEffectsApplyHandler();
        public event ActiveEffectsApplyHandler ActiveEffectsApplying;
        public void ApplyActiveEffects()
        {
            ActiveEffectsApplying?.Invoke();
        }
        public delegate void AttackChangeHandler(ref float attack);
        public event AttackChangeHandler AttackChange;
        public float Attack
        {
            get
            {
                float a = UnitClass.Attack;
                AttackChange?.Invoke(ref a);
                return a;
            }
        }
        public float Damage
        {
            get
            {
                return UnitClass.Damage * Count;
            }
        }
        public delegate void DefenceChangeHandler(ref float defence);
        public event DefenceChangeHandler DefenceChange;
        public float Defence
        {
            get
            {
                float d = UnitClass.Defence;
                DefenceChange?.Invoke(ref d);
                return d;
            }
        }
        public float Initiative { get; set; }
        public BattleUnitsStack(Unit unit, int count) : base(unit, count)
        {
            StartCount = count;
            LastUnitHitPoints = unit.HitPoints;
            Initiative = UnitClass.Initiative;
            ActiveEffects = new List<Effect>();
            canRetaliate = true;
        }

        public delegate void AttackEffectHandler(BattleUnitsStack target);
        public event AttackEffectHandler ApplyAttackEffects;
        public void AttackStack(BattleUnitsStack target)
        {
            ApplyAttackEffects?.Invoke(target);
            float totalDamage = Damage * (1 + 0.05f * Math.Abs(Attack - target.Defence));
            target.GetDamage(totalDamage);
        }
        public delegate void GetDamageEffectHandler();
        public event GetDamageEffectHandler ApplyGetDamageEffects;
        public void GetDamage(float damage)
        {
            ApplyGetDamageEffects?.Invoke();
            LastUnitHitPoints -= Math.Min(damage, LastUnitHitPoints);
            if (LastUnitHitPoints == 0)
            {
                damage -= UnitClass.HitPoints;
                Count--;
                Count -= (int)(damage / UnitClass.HitPoints);
                LastUnitHitPoints = UnitClass.HitPoints;
                LastUnitHitPoints -= damage % UnitClass.HitPoints;
            }
            if (Count < 0)
            {
                Count = 0;
            }
        }
        public delegate void GetHealEffectHandler();
        public event GetHealEffectHandler ApplyGetHealEffects;
        public void GetHeal(float healing)
        {
            ApplyGetHealEffects?.Invoke();
            LastUnitHitPoints += Math.Min(healing, UnitClass.HitPoints);
            if (LastUnitHitPoints == UnitClass.HitPoints)
            {
                Count += (int)(healing / UnitClass.HitPoints);
                if (Count > StartCount)
                {
                    Count = StartCount;
                }
            }
        }
        public void AddEffect(Effect effect)
        {
            effect.ApplyEffect(this);
            ActiveEffects.Add(effect);
        }
        public void RemoveEffect(Effect effect)
        {
            effect.Remove();
            ActiveEffects.Remove(effect);
        }
        public override object Clone()
        {
            UnitsStack us = (UnitsStack)base.Clone();
            BattleUnitsStack newStack = new BattleUnitsStack(us.UnitClass, us.Count);
            newStack.canRetaliate = canRetaliate;
            newStack.LastUnitHitPoints = LastUnitHitPoints;
            newStack.StartCount = StartCount;
            newStack.Count = Count;
            newStack.Side = Side;
            return newStack;
        }
    }
}
