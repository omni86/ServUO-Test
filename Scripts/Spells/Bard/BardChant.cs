#region Header
// **********
// ServUO - BardSpell.cs
// **********
#endregion

#region References
using System;
using System.Collections.Generic;
using Server.Network;
#endregion

namespace Server.Spells.Bard
{
    public abstract class BardChant : Spell
    {
        public BardChant(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        { }

        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
        public abstract int MantraNumber { get; }
        public override SkillName CastSkill { get { return SkillName.Musicianship; } }
        public override SkillName DamageSkill { get { return SkillName.Musicianship; } }
        public override bool ClearHandsOnCast { get { return false; } }
        //public override int CastDelayBase{ get{ return 1; } }
        public override int CastRecoveryBase { get { return 8; } }

        public virtual int UpkeepCost { get { return 5; } }

        public static int ComputePowerValue(Mobile from, int div)
        {
            if (from == null)
            {
                return 0;
            }

            int v = (int)Math.Sqrt(from.Karma + 20000 + (from.Skills.Chivalry.Fixed * 10));

            return v / div;
        }

        public override bool CheckCast()
        {
            int mana = ScaleMana(RequiredMana);

            if (!base.CheckCast())
            {
                return false;
            }

            if (Caster.Mana < mana)
            {
                Caster.SendLocalizedMessage(1060174, mana.ToString());
                // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            return true;
        }

        public override bool CheckFizzle()
        {
            return true;
        }

        public override void SayMantra()
        {
            Caster.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, Info.Mantra, false);
        }

        public override void DoFizzle()
        {
            Caster.PlaySound(0x1D6);
            Caster.NextSpellTime = Core.TickCount;
        }

        public override void DoHurtFizzle()
        {
            Caster.PlaySound(0x1D6);
        }

        public override void OnDisturb(DisturbType type, bool message)
        {
            base.OnDisturb(type, message);

            if (message)
            {
                Caster.PlaySound(0x1D6);
            }
        }

        public override void OnBeginCast()
        {
            base.OnBeginCast();

            SendCastEffect();
        }

        public virtual void SendCastEffect()
        {
            Caster.FixedEffect(0x37C4, 10, (int)(GetCastDelay().TotalSeconds * 28), 4, 3);
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = RequiredSkill + 50.0;
        }

        public override int GetMana()
        {
            return 0;
        }

        public int ComputePowerValue(int div)
        {
            return ComputePowerValue(Caster, div);
        }

        protected BuffInfo m_BuffInfo = null;
    }
    public class BardTimer : Timer
    {

        protected readonly int m_UpkeepCost = 0;
        protected readonly Mobile m_Caster;
        protected readonly BuffInfo m_Buff;
        public BardTimer(int upkeep, Mobile caster, BuffInfo buffInfo)
            : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(2))
        {
            this.m_Caster = caster;
            this.m_UpkeepCost = upkeep;
            this.m_Buff = buffInfo;
            this.Priority = TimerPriority.OneSecond;
            this.Start();
            BuffInfo.AddBuff(m_Caster, m_Buff);
        }

        protected override void OnTick()
        {
            int upkeepModifier = 0;

            Console.WriteLine("BardTimer: OnTick()");
            foreach (Mobile m in this.m_Caster.GetMobilesInRange(8))
            {
                Console.WriteLine("BardTimer: Checking if {0} is valid target.", m.Name);
                if (IsTarget(m))
                {
                    Console.WriteLine("BardTimer: {0} is valid target.", m.Name);
                    upkeepModifier += 1;
                    this.Effect(m);
                }
            }

            Console.WriteLine("BardTimer: Checking mana.");
            if (this.m_Caster.Mana > m_UpkeepCost + (upkeepModifier / 5))
            {
                this.m_Caster.Mana -= m_UpkeepCost + (upkeepModifier / 5);
            }
            else
            {
                BuffInfo.RemoveBuff(m_Caster, m_Buff);
                this.Stop();
            }
                
        }

        protected virtual bool IsTarget(Mobile m)
        {
            return false;
        }

        protected virtual void Effect(Mobile target)
        {

        }
    }
}