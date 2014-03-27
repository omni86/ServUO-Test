using System;
using System.Collections.Generic;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Bard
{
    public class InspireChant : BardChant
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Inspire", "Uus Por",
            -1,
            9002);
        public InspireChant(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
            
        }

        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(1.75); }
        }

        public override int RequiredMana
        {
            get { return 16; }
        }

        public override int UpkeepCost
        {
            get { return 4; }
        }

        public override bool BlocksMovement
        {
            get { return false; }
        }

        public override void OnCast()
        {
            new InspireUpkeepTimer(this.UpkeepCost, this.Caster).Start();
            this.FinishSequence();
        }

    }

    public class InspireUpkeepTimer : BardTimer
    {
        public InspireUpkeepTimer(int upkeep, Mobile caster) 
            : base(caster, true, BardEffect.Inspire)
        {
            
        }

        protected override bool IsTarget(Mobile m)
        {
            if (m_Caster == m || !m.Criminal)
                return true;

            return false;
        }

        protected override void StartEffect(Mobile m)
        {
            //Sanity Check
            if (!(m is PlayerMobile)) return;

            PlayerMobile target = m as PlayerMobile;

            int HCI_SDI = BardHelper.Scaler(m_Caster, 4, 165, 2);

            target.BardEffects.Add(BardEffect.Inspire, new Dictionary<AosAttribute, int>()
            {
                {AosAttribute.AttackChance, HCI_SDI},
                {AosAttribute.SpellDamage, HCI_SDI},
                {AosAttribute.WeaponDamage, BardHelper.Scaler(m_Caster, 20, 40, 18)}
            });

            base.StartEffect(m);
        }

        protected override void EndEffect(Mobile m)
        {
            if (!(m is PlayerMobile)) return;

            PlayerMobile target = m as PlayerMobile;

            target.BardEffects.Remove(BardEffect.Inspire);

            base.EndEffect(m);
        }
    }
}