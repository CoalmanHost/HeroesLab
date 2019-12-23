using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public abstract class Modifier
    {
        public abstract string Name { get; }
        public abstract void Apply(GameAction gameAction);
    }
}
