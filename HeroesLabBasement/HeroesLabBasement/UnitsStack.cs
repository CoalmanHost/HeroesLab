using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public class UnitsStack : ICloneable
    {
        public Unit UnitClass { get; }
        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                if (Count > 999999 || value > 999999)
                {
                    throw new UnitStackOverflowException();
                }
                else count = value;
            }
        }
        public bool isAlive
        {
            get
            {
                return Count > 0;
            }
        }
        public UnitsStack(Unit unit, int count)
        {
            this.UnitClass = unit;
            if (count <= 0)
            {
                count = -count;
            }
            this.Count = count;
        }
        public virtual object Clone()
        {
            return new UnitsStack((Unit)UnitClass.Clone(), Count);
        }
    }
}
