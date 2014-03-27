using System;
using System.Collections.Generic;
using Server.Engines.PartySystem;
using Server.Engines.Quests.Ninja;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Bard
{
    public class PerseveranceChant : BardChant
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Perseverance", "Uus Jux Sanct",
            -1,
            9002);
        public PerseveranceChant(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
            
        }

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds(1.75);
            }
        }
        public override double RequiredSkill
        {
            get
            {
                return 90.0;
            }
        }
        public override int RequiredMana
        {
            get
            {
                return 18;
            }
        }

        public override int UpkeepCost
        {
            get { return 5; }
        }

        public override bool BlocksMovement
        {
            get
            {
                return false;
            }
        }
        public override bool DelayedDamage
        {
            get
            {
                return false;
            }
        }
        public override void OnCast()
        {
            new PerseveranceUpkeepTimer(this.UpkeepCost, this.Caster).Start();
            this.FinishSequence();
        }

    }

    public class PerseveranceUpkeepTimer : BardTimer
    {

        private int m_Heal;

        public PerseveranceUpkeepTimer(int upkeep, Mobile caster)
            : base(caster, true, BardEffect.Perseverance)
        {
            m_Heal = 0;
        }

        protected override bool IsTarget(Mobile m)
        {
            if (this.m_Caster != m || (this.m_Caster.Party != null && ((Party)this.m_Caster.Party).Contains(m))) // Target self or party members
                return true;

            return false;
        }

        protected override void Effect(Mobile m)
        {
            //Sanity Check
            if (!(m is PlayerMobile)) return;

            PlayerMobile target = m as PlayerMobile;

            if (target.BardEffects.ContainsKey(BardEffect.Perseverance))
            {
                target.AddStatMod(new StatMod(StatType.All, "Perseverance", 4, TimeSpan.FromHours(1.0)));
                if (m_Heal == 2)
                {
                    m_Heal = 0;
                    target.Hits += 16;
                }
                else m_Heal++;
            }
            else
            {
                target.BardEffects.Add(BardEffect.Perseverance, new Dictionary<AosAttribute, int>()
                {
                    { AosAttribute.DefendChance, 24 } 
                });
                //TODO: Setup SSAAttribute.CastingFocus
            }
        }

        protected override void EndEffect(Mobile m)
        {
            if (!(m is PlayerMobile)) return;

            PlayerMobile target = m as PlayerMobile;

            if (target.BardEffects.ContainsKey(BardEffect.Perseverance))
            {
                target.BardEffects.Remove(BardEffect.Perseverance);
                target.RemoveStatMod("Perseverance");
            }
            else
                return;
        }
    }
}