using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clan scratch scrounger corpse")]
    public class ClanSS : BaseCreature
    {
        //public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }
        [Constructable]
        public ClanSS()
            : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Clan Scratch Scrounger";
            this.Body = 0x8E;
            this.BaseSoundID = 437;

            this.SetStr(97, 100);
            this.SetDex(98, 100);
            this.SetInt(45, 50);

            this.SetHits(135);

            this.SetDamage(4, 5);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 30);
            this.SetResistance(ResistanceType.Fire, 20, 25);
            this.SetResistance(ResistanceType.Cold, 49, 55);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 10, 20);

            this.SetSkill(SkillName.Anatomy, 51.5, 55.5);
            this.SetSkill(SkillName.MagicResist, 65.1, 90.0);
            this.SetSkill(SkillName.Tactics, 59.1, 65.0);
            this.SetSkill(SkillName.Wrestling, 72.5, 75.0);

            this.Fame = 6500;
            this.Karma = -6500;

            this.VirtualArmor = 56;

            this.AddItem(new Bow());
            this.PackItem(new Arrow(Utility.RandomMinMax(50, 70)));
        }

        public ClanSS(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override int Hides
        {
            get
            {
                return 8;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Spined;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich, 2);
        }

        public override void OnDeath(Container c)
        {

            base.OnDeath(c);
            Region reg = Region.Find(c.GetWorldLocation(), c.Map);
            if (1.0 > Utility.RandomDouble() && reg.Name == "Cavern of the Discarded")
            {
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new AbyssalCloth());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new PowderedIron());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new CrystallineBlackrock());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new EssenceBalance());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new CrystalShards());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new ArcanicRuneStone());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new DelicateScales());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new SeedRenewal());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new CrushedGlass());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new ElvenFletchings());
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (this.Body == 42)
            {
                this.Body = 0x8E;
                this.Hue = 0;
            }
        }
    }
}