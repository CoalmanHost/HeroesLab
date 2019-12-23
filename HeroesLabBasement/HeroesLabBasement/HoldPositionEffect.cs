using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public class HoldPositionEffect : Effect
    {
        public override string Name { get; } = "Holding position";
        public override EffectType Type { get; } = EffectType.Periodic;
        public override EffectDurationType DurationType { get; } = EffectDurationType.Round;
        private float baseDefence;
        
        public void HoldPosition(ref float defence)
        {
            defence += baseDefence * 0.3f;
        }
        public override void Remove()
        {
            master.DefenceChange -= HoldPosition;
        }
        protected override void Apply(BattleUnitsStack master)
        {
            BattleUnitsStack Master = this.master;
            Duration = 1;
            baseDefence = Master.Defence;
            Master.DefenceChange += HoldPosition;
        }
    }
}
