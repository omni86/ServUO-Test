using System;
using System.Collections.Generic;
using Server.Engines.PartySystem;
using Server.Items;

namespace Server.Spells.Bard
{
    public class InspireSpell : BardChant
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Inspire", "Uus Por",
            -1,
            9002);
        public InspireSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
            m_BuffInfo = new BuffInfo(BuffIcon.MagicReflection, 1115612, false);
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

        public override int MantraNumber
        {
            get
            {
                return 1060724;
            }
        }// Augus Luminos
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
            new InspireUpkeepTimer(this.UpkeepCost, this.Caster, this.m_BuffInfo).Start();
            this.FinishSequence();
        }

    }

    public class InspireUpkeepTimer : BardTimer
    {
        public InspireUpkeepTimer(int upkeep, Mobile caster, BuffInfo buffInfo) 
            : base(upkeep, caster, buffInfo)
        {
            
        }

        protected override bool IsTarget(Mobile m)
        {
            if (this.m_Caster.Party != null && ((Party)this.m_Caster.Party).Contains(m)) // Target Party Members
                return true;

            return false;
        }

        protected override void Effect(Mobile target)
        {
            target.Heal(100); //Test 
        }
    }
}