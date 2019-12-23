using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    static class Extensions
    {
        public static List<UnitsStack> Copy(this List<UnitsStack> list)
        {
            List<UnitsStack> newList = new List<UnitsStack>();
            foreach (var item in list)
            {
                newList.Add((UnitsStack)item.Clone());
            }
            return newList;
        }
        public static List<BattleUnitsStack> Copy(this List<BattleUnitsStack> list)
        {
            List<BattleUnitsStack> newList = new List<BattleUnitsStack>();
            foreach (var item in list)
            {
                newList.Add((BattleUnitsStack)item.Clone());
            }
            return newList;
        }
    }
}
