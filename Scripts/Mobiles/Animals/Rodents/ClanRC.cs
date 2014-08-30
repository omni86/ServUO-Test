using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a clan ribbon courtier corpse")]
    public class ClanRC : BaseCreature
    {
        //public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }
        [Constructable]
        public ClanRC()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Clan Ribbon Courtier";
            this.Body = 42;
            this.Hue = 2207;
            this.BaseSoundID = 437;

            this.SetStr(231);
            this.SetDex(252);
            this.SetInt(125);

            this.SetHits(2054, 2100);

            this.SetDamage(7, 14);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 40);
            this.SetResistance(ResistanceType.Fire, 10, 12);
            this.SetResistance(ResistanceType.Cold, 15, 20);
            this.SetResistance(ResistanceType.Poison, 10, 12);
            this.SetResistance(ResistanceType.Energy, 10, 12);

            this.SetSkill(SkillName.MagicResist, 113.5, 115.0);
            this.SetSkill(SkillName.Tactics, 65.1, 70.0);
            this.SetSkill(SkillName.Wrestling, 50.5, 55.0);

            this.Fame = 1500;
            this.Karma = -1500;

            this.VirtualArmor = 48;
        }

        public ClanRC(Serial serial)
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
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Average);
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
        }
    }
}