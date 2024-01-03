using System;

using Server.Mobiles;

namespace Server
{
    public class PetLeveling
    {
        public static void DoDeathCheck(BaseCreature from)
        {
            Mobile cm = from.ControlMaster;

            if (cm != null && from.Controlled == true && from.Tamable == true)
            {
                if (from.IsBonded == true)
                {
                    if (Utility.Random(100) < 5)
                    {
                        int strloss = from.Str / 20;
                        int dexloss = from.Dex / 20;
                        int intloss = from.Int / 20;
                        int hitsloss = from.Hits / 20;
                        int stamloss = from.Stam / 20;
                        int manaloss = from.Mana / 20;
                        int physloss = from.PhysicalResistance / 20;
                        int fireloss = from.FireResistance / 20;
                        int coldloss = from.ColdResistance / 20;
                        int energyloss = from.EnergyResistance / 20;
                        int poisonloss = from.PoisonResistance / 20;
                        int dminloss = from.DamageMin / 20;
                        int dmaxloss = from.DamageMax / 20;

                        if (from.Str > strloss)
                            from.Str -= strloss;

                        if (from.Str > dexloss)
                            from.Dex -= dexloss;

                        if (from.Str > intloss)
                            from.Int -= intloss;

                        if (from.HitsMaxSeed > hitsloss)
                            from.HitsMaxSeed -= hitsloss;
                        if (from.StamMaxSeed > stamloss)
                            from.StamMaxSeed -= stamloss;
                        if (from.ManaMaxSeed > manaloss)
                            from.ManaMaxSeed -= manaloss;

                        if (from.PhysicalResistanceSeed > physloss)
                            from.PhysicalResistanceSeed -= physloss;
                        if (from.FireResistSeed > fireloss)
                            from.FireResistSeed -= fireloss;
                        if (from.ColdResistSeed > coldloss)
                            from.ColdResistSeed -= coldloss;
                        if (from.EnergyResistSeed > energyloss)
                            from.EnergyResistSeed -= energyloss;
                        if (from.PoisonResistSeed > poisonloss)
                            from.PoisonResistSeed -= poisonloss;

                        if (from.DamageMin > dminloss)
                            from.DamageMin -= dminloss;

                        if (from.DamageMax > dmaxloss)
                            from.DamageMax -= dmaxloss;

                        cm.SendMessage(38, "Your pet has suffered a 5% stat lose due to its untimely death.");
                    }

                    cm.SendMessage(64, "Your pet has been killed!");
                }
                else
                {
                    cm.SendMessage(64, "Your pet has been killed!");
                }
            }
        }

        public static void DoBioDeath(BaseCreature from)
        {
            Mobile cm = from.ControlMaster;

            if (cm != null && from.Controlled == true && from.Tamable == true)
            {
                if (from.IsBonded == true)
                {
                    if (Utility.Random(100) < 25)
                    {
                        int strloss = from.Str / 20;
                        int dexloss = from.Dex / 20;
                        int intloss = from.Int / 20;
                        int hitsloss = from.Hits / 20;
                        int stamloss = from.Stam / 20;
                        int manaloss = from.Mana / 20;
                        int physloss = from.PhysicalResistance / 20;
                        int fireloss = from.FireResistance / 20;
                        int coldloss = from.ColdResistance / 20;
                        int energyloss = from.EnergyResistance / 20;
                        int poisonloss = from.PoisonResistance / 20;
                        int dminloss = from.DamageMin / 20;
                        int dmaxloss = from.DamageMax / 20;

                        if (from.Str > strloss)
                            from.Str -= strloss;

                        if (from.Str > dexloss)
                            from.Dex -= dexloss;

                        if (from.Str > intloss)
                            from.Int -= intloss;

                        if (from.HitsMaxSeed > hitsloss)
                            from.HitsMaxSeed -= hitsloss;
                        if (from.StamMaxSeed > stamloss)
                            from.StamMaxSeed -= stamloss;
                        if (from.ManaMaxSeed > manaloss)
                            from.ManaMaxSeed -= manaloss;

                        if (from.PhysicalResistanceSeed > physloss)
                            from.PhysicalResistanceSeed -= physloss;
                        if (from.FireResistSeed > fireloss)
                            from.FireResistSeed -= fireloss;
                        if (from.ColdResistSeed > coldloss)
                            from.ColdResistSeed -= coldloss;
                        if (from.EnergyResistSeed > energyloss)
                            from.EnergyResistSeed -= energyloss;
                        if (from.PoisonResistSeed > poisonloss)
                            from.PoisonResistSeed -= poisonloss;

                        if (from.DamageMin > dminloss)
                            from.DamageMin -= dminloss;

                        if (from.DamageMax > dmaxloss)
                            from.DamageMax -= dmaxloss;

                        cm.SendMessage(38, "Your pet has suffered a 5% stat lose due to its untimely death.");
                    }

                    cm.SendMessage(64, "Your pet has been killed!");
                }
                else
                {
                    cm.SendMessage(64, "Your pet has been killed!");
                }
            }
        }

        public static void DoEvoCheck(BaseCreature attacker)
        {
            if (attacker.Str >= 20)
                attacker.Str += attacker.Str / 20;
            else
                attacker.Str += 1;

            if (attacker.Dex >= 20)
                attacker.Dex += attacker.Dex / 20;
            else
                attacker.Dex += 1;

            if (attacker.Int >= 20)
                attacker.Int += attacker.Int / 20;
            else
                attacker.Int += 1;
        }

        /*
        // Removing bonuses given on level up
        public static void DoLevelBonus(BaseCreature attacker)
        {
            int chance = Utility.Random(100);

            if (chance < 35)
            {
                attacker.Str += Utility.RandomMinMax(1, 3);
                attacker.Dex += Utility.RandomMinMax(1, 3);
                attacker.Int += Utility.RandomMinMax(1, 3);
            }
        }
        */

        public static void CheckLevel(Mobile defender, BaseCreature attacker, int count)
        {
            bool nolevel = false;
            Type typ = attacker.GetType();
            string nam = attacker.Name;

            foreach (string check in FSATS.NoLevelCreatures)
            {
                if (check == nam)
                    nolevel = true;
            }

            if (nolevel != false)
                return;

            int expgainmin, expgainmax;

            if (defender is BaseCreature)
            {
                if (attacker.Controlled == true && attacker.ControlMaster != null && attacker.Summoned == false)
                {
                    BaseCreature bc = (BaseCreature)defender;

                    expgainmin = bc.HitsMax * 1;
                    expgainmax = bc.HitsMax * 5;

                    int xpgain = (bc.HitsMax / 4); //Utility.RandomMinMax(expgainmin, expgainmax);

                    if (xpgain <= 4)
                        xpgain = 5;

                    if (count > 1)
                        xpgain = xpgain / count;

                    if (attacker.Level <= attacker.MaxLevel - 1)
                    {
                        attacker.Exp += xpgain;
                        attacker.ControlMaster.SendMessage("Your pet has gained {0} experience points.", xpgain);

                        if (attacker.IsEvoPet && attacker.EvoStage < attacker.EvoMaxStage)
                            attacker.EvoPoints += xpgain;
                    }

                    if (attacker.IsEvoPet)
                        attacker.AddPoints(defender, attacker, count);

                    if (attacker.Exp >= attacker.NextLevel && attacker.Level <= attacker.MaxLevel - 1)
                    {
                        Mobile cm = attacker.ControlMaster;
                        attacker.Level++;
                        attacker.Exp -= attacker.NextLevel; // do this instead of resetting exp to zero when leveling, allows "carry over"
                        attacker.NextLevel = (attacker.Level * 1000);
                        attacker.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                        attacker.PlaySound(503);
                        cm.SendMessage(1161, "Your pets level has increased to {0}.", attacker.Level);

                        // IF Level is higher than 10, give 2 per level
                        // ELSE give 1 per level
                        int gain = attacker.Level > 10 ? 2 : 1; //Utility.RandomMinMax(10, 50);

                        if (attacker.ControlMaster != null)
                        {
                            attacker.AbilityPoints += gain;
                            attacker.ControlMaster.SendMessage(1153, "Your pet has gained {0} ability points.", gain);
                        }


                    }
                }
            }
        }
    }
}
