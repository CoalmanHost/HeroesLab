using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public class BattleArmy
    {
        public string Name { get; }
        private List<BattleUnitsStack> battleUnitsStacks;
        public List<BattleUnitsStack> BattleUnitsStacks
        {
            get
            {
                return battleUnitsStacks;
            }
            private set
            {
                battleUnitsStacks = value;
            }
        }
        public List<BattleUnitsStack> BattleUnitsStacksCopy
        {
            get
            {
                return battleUnitsStacks.Copy();
            }
            private set
            {
                battleUnitsStacks = value;
            }
        }
        private List<BattleUnitsStack> deadStacks;
        public List<BattleUnitsStack> DeadStacks
        {
            get
            {
                return deadStacks.Copy();
            }
            private set
            {
                deadStacks = value;
            }
        }
        public bool IsAlive
        {
            get
            {
                return battleUnitsStacks.Count > 0;
            }
        }
        List<BattleUnitsStack> refSummoned;
        public BattleArmy(Army army)
        {
            Name = army.Name;
            battleUnitsStacks = new List<BattleUnitsStack>(9);
            refSummoned = new List<BattleUnitsStack>(3);
            deadStacks = new List<BattleUnitsStack>();
            foreach (var item in army.UnitsStacks)
            {
                BattleUnitsStack stack = new BattleUnitsStack(item.UnitClass, item.Count);
                battleUnitsStacks.Add(stack);
            }
        }
        public void AddSummonedStack(BattleUnitsStack summoned)
        {
            BattleUnitsStack stack = (BattleUnitsStack)summoned.Clone();
            battleUnitsStacks.Add(stack);
            refSummoned.Add(stack);
        }
        public void ClearSummoned()
        {
            foreach (var item in refSummoned)
            {
                battleUnitsStacks.Remove(item);
            }
            refSummoned.Clear();
        }
        public void ResurrectStack(BattleUnitsStack target)
        {
            battleUnitsStacks.Add(target);
            deadStacks.Remove(target);
        }
        public void KillStack(BattleUnitsStack target)
        {
            battleUnitsStacks.Remove(target);
            deadStacks.Add(target);
        }
    }
}
