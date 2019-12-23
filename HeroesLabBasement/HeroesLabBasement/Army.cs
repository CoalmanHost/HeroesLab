using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public class Army
    {
        public string Name { get; set; }
        private List<UnitsStack> unitsStacks;
        public List<UnitsStack> UnitsStacks
        {
            get
            {
                return unitsStacks.Copy();
            }
            private set
            {
                unitsStacks = value;
            }
        }
        public int StacksCount
        {
            get
            {
                return UnitsStacks.Count;
            }
        }
        public void AddStack(UnitsStack unitsStack)
        {
            unitsStacks.Add(unitsStack);
        }
        public void RemoveStack(UnitsStack unitsStack)
        {
            unitsStacks.Remove(unitsStack);
        }
        public void RemoveStack(int id)
        {
            unitsStacks.RemoveAt(id);
        }
        public void UpdateStacks(BattleArmy bArmy)
        {
            bArmy.ClearSummoned();
            unitsStacks.Clear();
            foreach (var item in bArmy.BattleUnitsStacks)
            {
                unitsStacks.Add(item);
            }
        }
        public Army(string name)
        {
            Name = name;
            unitsStacks = new List<UnitsStack>(6);
        }
        public Army(string name, List<UnitsStack> listOfStacks)
        {
            Name = name;
            unitsStacks = listOfStacks.Copy();
        }
    }
}
