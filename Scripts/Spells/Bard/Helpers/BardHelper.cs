using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Mobiles;

namespace Server.Spells.Bard
{

    public enum BardEffect
    {
        Inspire = 0,
        Invigorate,
        Resilience,
        Perseverance,
        Tribulation,
        Despair
    }

    public static class BardHelper
    {

        public static BuffInfo GenerateBuffInfo(BardEffect effect, Mobile caster)
        {
            switch (effect)
            {

                case BardEffect.Inspire:
                    int HCI_SPI = Scaler(caster, 4, 16, 1);
                    int DI = Scaler(caster, 20, 40, 3);
                    int BDM = Scaler(caster, 1, 15, 0);
                    return new BuffInfo(BuffIcon.Inspire, 1115612, 1151951,
                        String.Format("{0}\t{1}\t{2}\t{3}", HCI_SPI, HCI_SPI, DI, BDM), false);
                        // Inspire +~1_HCI~% Hit Chance Increase.<br>+~2_SDI~% Spell Damage Increase.<br>+~3_DI~% Damage Increase.<br>Bonus Damage Modifier: + ~4_DM~%

                case BardEffect.Invigorate:
                    int statIncrease = Scaler(caster, 8, 8, 2);
                    int HPI = Scaler(caster, 20, 20, 2);
                    return new BuffInfo(BuffIcon.Invigorate, 1115613, 1115730,
                        String.Format("{0}\t{1}\t{2}\t{3}", HPI, statIncrease, statIncrease, statIncrease), false);
                        // Invigorate +~1_HPI~ Hit Point Increase.<br>+~2_STR~ Strength.<br>+~3_INT~ Intelligence.<br>+~4_DEX~ Dexterity.<br>
            }

            return new BuffInfo(BuffIcon.Bless, 500000);
        }

        public static BuffInfo[] BardBuffInfo = new BuffInfo[6]
        {
            new BuffInfo(BuffIcon.MagicReflection, 1115612, false), // Inspire
            new BuffInfo(BuffIcon.MagicReflection, 1115613, false), 
            new BuffInfo(BuffIcon.MagicReflection, 1115614, false), // Resilience
            new BuffInfo(BuffIcon.MagicReflection, 1115615, false), // Perseverance
            new BuffInfo(BuffIcon.MagicReflection, 1115616, false), // Tribulation
            new BuffInfo(BuffIcon.MagicReflection, 1115617, false)  // Despair
        };

        public static int[] BardUpkeepCosts = new int[6]
        {
            4, // Inspire
            5, // Invigorate
            4, // Resilience
            5, // Perseverance
            8, // Tribulation
            10 // Despair
        };

        public static int GetUpkeepCost(Mobile caster, BardEffect effect, int targets = 0)
        {
            int cost = BardUpkeepCosts[(int) effect];

            cost += targets / 5;

            if (caster.Skills.Peacemaking.Base > 100.0)
                cost -= 1;
            if (caster.Skills.Discordance.Base > 100.0)
                cost -= 1;
            if (caster.Skills.Provocation.Base > 100.0)
                cost -= 1;

            return cost;
        }

        public static int Scaler(Mobile mobile, int low, int high, int complementryModifier)
        {
            int complementryPoints = 0;

            if (mobile.Skills.Peacemaking.Base > 100)
                complementryPoints += (int) Math.Floor((mobile.Skills.Peacemaking.Base - 100.0)/10);

            if (mobile.Skills.Discordance.Base > 100)
                complementryPoints += (int)Math.Floor((mobile.Skills.Discordance.Base - 100.0) / 10);

            if (mobile.Skills.Provocation.Base > 100)
                complementryPoints += (int)Math.Floor((mobile.Skills.Provocation.Base - 100.0) / 10);

            if (mobile.Skills.Musicianship.Base > 100)
                complementryPoints += (int)Math.Floor((mobile.Skills.Musicianship.Base - 100.0) / 10);


            double scale = (high - low)/(120.0 - 90.0);

            int value = (int) (low + ((mobile.Skills.Musicianship.Base - 90)*scale));

            value += (complementryModifier*complementryPoints);
            
            return value;
        }

        public static BardTimer GetActiveSong(Mobile mobile, Type songType)
        {
            if (!(mobile is PlayerMobile)) return null;

            return ((PlayerMobile)mobile).ActiveSongs.FirstOrDefault(song => song.GetType() == songType);
        }
    }
}
