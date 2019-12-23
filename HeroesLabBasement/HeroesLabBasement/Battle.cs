using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesLab
{
    public enum GameActionType
    {
        Fight,
        Cast,
        Wait,
        SelfDefend
    }
    public class Battle
    {
        Army left, right;
        public BattleArmy leftArmy { get; private set; } public BattleArmy rightArmy { get; private set; }
        public List<BattleUnitsStack> InitiativeQueueList { get { return iqueue.Queue; } }
        public BattleUnitsStack Actor { get { return iqueue.ActiveStack; } }
        public bool EndOfRound { get { return iqueue.IsEmpty; } }
        public bool End { get { return !leftArmy.IsAlive || !rightArmy.IsAlive; } }
        public void Initialize(Army armyA, Army armyB)
        {
            left = armyA;
            right = armyB;
            leftArmy = new BattleArmy(armyA);
            rightArmy = new BattleArmy(armyB);
            foreach (var item in leftArmy.BattleUnitsStacks)
            {
                item.Side = leftArmy;
            }
            foreach (var item in rightArmy.BattleUnitsStacks)
            {
                item.Side = rightArmy;
            }
            iqueue = new InitiativeQueue(this);
        }

        void StackAct()
        {
            List<Effect> toRemove = new List<Effect>();
            foreach (var item in iqueue.ActiveStack.ActiveEffects)
            {
                if (item.Type == EffectType.OnTurn)
                {
                    item.Remove();
                    toRemove.Add(item);
                }
            }
            foreach (var item in toRemove)
            {
                iqueue.ActiveStack.RemoveEffect(item);
            }
            iqueue.Act();
            UpdatePeriodicEffects();
        }

        public void StackWait()
        {
            iqueue.Wait();
            UpdatePeriodicEffects();
        }

        public void StartNewRound()
        {
            iqueue = new InitiativeQueue(this);
            foreach (var item in leftArmy.BattleUnitsStacks)
            {
                item.canRetaliate = true;
            }
            foreach (var item in rightArmy.BattleUnitsStacks)
            {
                item.canRetaliate = true;
            }
        }

        public void StackFight(BattleUnitsStack target)
        {
            GameAction gameAction = new GameAction(Actor, target);
            gameAction.Fight();
            StackAct();
        }

        public void StackDefence()
        {
            Actor.AddEffect(new HoldPositionEffect());
            StackAct();
        }

        public void StackCast(Skill skill, BattleUnitsStack target)
        {
            GameAction gameAction = new GameAction(Actor, target);
            gameAction.Cast(skill);
            StackAct();
        }

        public void Stop()
        {
            left.UpdateStacks(leftArmy);
            right.UpdateStacks(rightArmy);
        }

        public void UpdatePeriodicEffects()
        {
            List<BattleUnitsStack> all = new List<BattleUnitsStack>();
            all.AddRange(leftArmy.BattleUnitsStacks);
            all.AddRange(rightArmy.BattleUnitsStacks);
            List<Effect> toRemove = new List<Effect>();
            foreach (var stack in all)
            {
                stack.ApplyActiveEffects();
            }
            foreach (var stack in all)
            {
                foreach (var effect in stack.ActiveEffects)
                {
                    if (effect.Type == EffectType.Periodic && effect.DurationType == Effect.EffectDurationType.Turn)
                    {
                        effect.Proceed();
                        if (effect.Duration <= 0)
                        {
                            toRemove.Add(effect);
                        }
                    }
                }
                foreach (var effect in toRemove)
                {
                    stack.RemoveEffect(effect);
                }
            }
            toRemove.Clear();
            if (iqueue.IsEmpty)
            {
                foreach (var stack in all)
                {
                    foreach (var effect in stack.ActiveEffects)
                    {
                        if (effect.Type == EffectType.Periodic && effect.DurationType == Effect.EffectDurationType.Round)
                        {
                            effect.Proceed();
                            if (effect.Duration <= 0)
                            {
                                toRemove.Add(effect);
                            }
                        }
                    }
                    foreach (var effect in toRemove)
                    {
                        stack.RemoveEffect(effect);
                    }
                }
            }
        }

        private class InitiativeQueue
        {
            public BattleUnitsStack ActiveStack
            {
                get { return queue[0]; }
            }
            public bool IsEmpty { get { return queue.Count == 0; } }
            public List<BattleUnitsStack> Queue { get { return queue.Copy(); } }
            List<BattleUnitsStack> queue;
            int waitCount;
            public InitiativeQueue(Battle battle)
            {
                queue = new List<BattleUnitsStack>();
                waitCount = 0;
                foreach (var item in battle.leftArmy.BattleUnitsStacks)
                {
                    Add(item);
                }
                foreach (var item in battle.rightArmy.BattleUnitsStacks)
                {
                    Add(item);
                }
            }
            public void Add(BattleUnitsStack stack)
            {
                int i = queue.Count - waitCount;
                queue.Insert(i, stack);
                while (i > 0 && queue[i - 1].Initiative < queue[i].Initiative)
                {
                    BattleUnitsStack temp = queue[i - 1];
                    queue[i - 1] = queue[i];
                    queue[i] = temp;
                    i--;
                }
            }
            class InitiativeComparer : IComparer<BattleUnitsStack>
            {
                public int Compare(BattleUnitsStack x, BattleUnitsStack y)
                {
                    return Math.Sign(x.Initiative - y.Initiative);
                }
            }
            class InitiativeComparerReversed : IComparer<BattleUnitsStack>
            {
                public int Compare(BattleUnitsStack x, BattleUnitsStack y)
                {
                    return Math.Sign(y.Initiative - x.Initiative);
                }
            }
            void Update()
            {
                List<BattleUnitsStack> toRemove = new List<BattleUnitsStack>();
                for (int i = 0; i < queue.Count; i++)
                {
                    if (!queue[i].isAlive)
                    {
                        toRemove.Add(queue[i]);
                        if (i >= queue.Count - (waitCount % queue.Count))
                        {
                            waitCount--;
                        }
                    }
                }
                foreach (var item in toRemove)
                {
                    queue.Remove(item);
                }
                if (queue.Count <= 0)
                {
                    return;
                }
                queue.Sort(0, queue.Count - (waitCount % queue.Count), new InitiativeComparerReversed());
                queue.Sort(queue.Count - (waitCount % queue.Count), queue.Count - (queue.Count - (waitCount % queue.Count)), new InitiativeComparer());
            }
            public void Wait()
            {
                BattleUnitsStack waiting = ActiveStack;
                queue.Remove(waiting);
                queue.Insert(queue.Count - (waitCount % queue.Count), waiting);
                waitCount++;
                Update();
            }
            public void Act()
            {
                queue.Remove(ActiveStack);
                if (queue.Count > 0)
                {
                    Update();
                }
            }
        }
        InitiativeQueue iqueue;
    }
}
