using System;
using System.Collections.Generic;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Bard
{
    public class ResilianceChant : BardChant
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Resiliance", "Kal Mani Tym",
            -1,
            9002);
        public ResilianceChant(Mobile caster, Item scroll)
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
                return 16;
            }
        }

        public override int UpkeepCost
        {
            get { return 4; }
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
            new ResilianceUpkeepTimer(this.UpkeepCost, this.Caster).Start();
            this.FinishSequence();
        }

    }

    public class ResilianceUpkeepTimer : BardTimer
    {
        public ResilianceUpkeepTimer(int upkeep, Mobile caster)
            : base(caster, true, BardEffect.Resilience)
        {

        }

        protected override bool IsTarget(Mobile m)
        {
            if (this.m_Caster != m || (this.m_Caster.Party != null && ((Party)this.m_Caster.Party).Contains(m))) // party members
                return true;

            return false;
        }

        protected override void Effect(Mobile m)
        {
            //Sanity Check
            if (!(m is PlayerMobile)) return;

            PlayerMobile target = m as PlayerMobile;

            if (target.BardEffects.ContainsKey(BardEffect.Resilience))
                return;
            else
            {
                target.BardEffects.Add(BardEffect.Resilience, new Dictionary<AosAttribute, int>()
                {
                    { AosAttribute.RegenHits, 16 },
                    { AosAttribute.RegenMana, 16 },
                    { AosAttribute.RegenStam, 16 }
                });
            }


        }

        protected override void EndEffect(Mobile m)
        {
            if (!(m is PlayerMobile)) return;

            PlayerMobile target = m as PlayerMobile;

            if (target.BardEffects.ContainsKey(BardEffect.Resilience))
                target.BardEffects.Remove(BardEffect.Resilience);
            else
                return;
        }
    }
}