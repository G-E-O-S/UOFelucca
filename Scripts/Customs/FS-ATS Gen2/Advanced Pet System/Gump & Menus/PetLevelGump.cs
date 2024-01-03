using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Gumps
{
    public class PetLevelGump : Gump
    {
        private readonly Mobile m_Pet;

        public PetLevelGump(Mobile pet) : base(0, 0)
        {
            m_Pet = pet;

            BaseCreature bc = (BaseCreature)m_Pet;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            // adjust the gump height/length based on the number of entries per level
            int height = 250;

            if(bc.Level == 0 )
                height = 175;
            else if(bc.Level > 0 && bc.Level < 4)
                height = 285;
            else if(bc.Level >= 4 && bc.Level < 7)
                height = 310;
            else if(bc.Level >= 8 && bc.Level < 10)
                height = 360;
            else if(bc.Level >= 10)
                height = 435;

            //AddBackground(12, 9, 394, height, 2620); //1755 /* 2620 */);

            //// in case of names that are clilocs or there's a compatbility issues
            //AddLabel(22, 20, 1149, $"Name: {bc.Name}");
            //AddLabel(22, 40, 1149, $"Level: {bc.Level.ToString()} / {bc.MaxLevel.ToString()}" );

            //int nextLevel = bc.Level * 1000;
            //AddLabel(22, 60, 1149, $"Exp: {bc.Exp.ToString("#,0")} / {nextLevel.ToString("#,0")}" );
            //AddLabel(22, 80, 1149, $"Experience Points: {bc.AbilityPoints.ToString()}" );            
            //AddImage(336, 20, 5549);

            if (bc.IsEvoPet)
            {
                AddBackground(12, 9, 394, height, 2620); //1755 /* 2620 */);

                AddLabel(22, 20, 1149, $"Name: {bc.Name}");

                AddLabel(22, 40, 1149, $"Evolution Stage: {bc.EvoStage.ToString()} / {bc.EvoMaxStage.ToString()}");               
                AddLabel(22, 60, 1149, $"Evolution Points: {bc.EvoPoints.ToString()}");
                AddLabel(22, 80, 1149, $"Evolution Points Needed: {bc.EvoPointsToEvolve.ToString()}");

                AddLabel(198, 40, 1149, $"Level: {bc.Level.ToString()} / {bc.MaxLevel.ToString()}");
                AddLabel(198, 60, 1149, $"Experience Points: {bc.AbilityPoints.ToString()}");

                AddImage(336, 20, 5547);
            }
            else
            {
                AddBackground(12, 9, 394, height, 2620); //1755 /* 2620 */);

                // in case of names that are clilocs or there's a compatbility issues
                AddLabel(22, 20, 1149, $"Name: {bc.Name}");
                AddLabel(22, 40, 1149, $"Level: {bc.Level.ToString()} / {bc.MaxLevel.ToString()}");

                int nextLevel = bc.Level * 1000;
                AddLabel(22, 60, 1149, $"Exp: {bc.Exp.ToString("#,0")} / {nextLevel.ToString("#,0")}");
                AddLabel(22, 80, 1149, $"Experience Points: {bc.AbilityPoints.ToString()}");

                AddImage(336, 20, 5549);
            }

            XmlPoison xmlPoison = (XmlPoison)XmlAttach.FindAttachment(bc, typeof(XmlPoison));
            XmlAreaDamage xmlAreaDamage = (XmlAreaDamage)XmlAttach.FindAttachment(bc, typeof(XmlAreaDamage));

            if(bc.BonusGold > 0)
            {
                AddImage(22, 110, 2361); // size 11 x 11 // default - AddImage(22, 102, 2361); 
                AddLabel(35, 108, 1149, $"Gold Bonus Activated"); // to add buff info - [{bc.BonusGold.ToString()}] // default - AddLabel(35, 100, 1149, $"Gold Bonus Activated");
            }
            else
            {
                AddImage(22, 110, 2360); // size 11 x 11 // default - AddImage(22, 102, 2360);
                AddLabel(35, 108, 1149, @"Gold Bonus Inactive"); // default - AddLabel(35, 100, 1149, @"Gold Bonus Inactive");
            }
                        
            if (xmlPoison != null && xmlPoison.ImmuneLevel > 0)
            {
                AddImage(22, 130, 2361); // size 11 x 11 // default - AddImage(22, 122, 2361);
                AddLabel(35, 128, 1149, $"Poison Immune Activated"); // to add buff info - [{bc.BonusGold.ToString()}] // default - AddLabel(35, 120, 1149, $"Poison Immune Activated +{xmlPoison.ImmuneLevel.ToString()}"); 
            }
            else 
            {
                AddImage(22, 130, 2360); // size 11 x 11 // default - AddImage(22, 122, 2360);
                AddLabel(35, 128, 1149, @"Poison Immune Inactive"); // default AddLabel(35, 120, 1149, @"Poison Immune Inactive");
            }
            if (xmlAreaDamage != null && xmlAreaDamage.ChanceToTrigger > 0)
            {
                AddImage(22, 150, 2361); // size 11 x 11 // default - AddImage(22, 142, 2361);
                AddLabel(35, 148, 1149, $"Area Damage Activated"); // to add buff info - [{bc.BonusGold.ToString()}] // default - AddLabel(35, 140, 1149, $"Area Damage Activated {xmlAreaDamage.ChanceToTrigger.ToString()}%");
            }
            else
            {
                AddImage(22, 150, 2360); // size 11 x 11 // default - AddImage(22, 142, 2360);
                AddLabel(35, 148, 1149, @"Area Damage Inactive"); // default - AddLabel(35, 140, 1149, @"Area Damage Inactive"); 
            }

            // green circle 2361
            // red circle 2360

            AddPage(1);

            const int yBump = 35;

            if (bc.Level > 0)
            {
                AddLabel(22, 140 + yBump, 1149, @"Property Name");
                AddLabel(330, 140 + yBump, 1149, @"Amount");

                AddLabel(60, 175 + yBump, 1149, @"Hit Points");
                AddLabel(60, 200 + yBump, 1149, @"Stamina");
                AddLabel(60, 225 + yBump, 1149, @"Mana");

                AddLabel(330, 175 + yBump, 1149, bc.HitsMax.ToString() + "/" + FSATS.NormalHITS.ToString());
                AddLabel(330, 200 + yBump, 1149, bc.StamMax.ToString() + "/" + FSATS.NormalSTAM.ToString());
                AddLabel(330, 225 + yBump, 1149, bc.ManaMax.ToString() + "/" + FSATS.NormalMANA.ToString());

                AddButton(24, 175 + yBump, 4005, 4006, 1, GumpButtonType.Reply, 0);
                AddButton(24, 200 + yBump, 4005, 4006, 2, GumpButtonType.Reply, 0);
                AddButton(24, 225 + yBump, 4005, 4006, 3, GumpButtonType.Reply, 0);
            }

            if (bc.Level >= 4)
            {
                AddLabel(60, 250 + yBump, 1149, @"Armor Rating");
                AddLabel(330, 250 + yBump, 1149, bc.VirtualArmor.ToString() + "/" + FSATS.NormalVArmor.ToString());
                AddButton(24, 250 + yBump, 4005, 4006, 11, GumpButtonType.Reply, 0);
            }

            if (bc.Level >= 8)
            {
                AddLabel(60, 275 + yBump, 1149, @"Min Damage");
                AddLabel(60, 300 + yBump, 1149, @"Max Damage");

                AddLabel(330, 275 + yBump, 1149, bc.DamageMin.ToString() + "/" + FSATS.NormalMinDam.ToString());
                AddLabel(330, 300 + yBump, 1149, bc.DamageMax.ToString() + "/" + FSATS.NormalMaxDam.ToString());

                AddButton(24, 275 + yBump, 4005, 4006, 9, GumpButtonType.Reply, 0);
                AddButton(24, 300 + yBump, 4005, 4006, 10, GumpButtonType.Reply, 0);
            }

            if(bc.Level >= 10)
            {
                AddLabel(60, 325 + yBump, 1149, @"Strength");
                AddLabel(60, 350 + yBump, 1149, @"Dexterity");
                AddLabel(60, 375 + yBump, 1149, @"Intelligence");

                AddLabel(330, 325 + yBump, 1149, bc.RawStr.ToString() + "/" + FSATS.NormalSTR.ToString());
                AddLabel(330, 350 + yBump, 1149, bc.RawDex.ToString() + "/" + FSATS.NormalDEX.ToString());
                AddLabel(330, 375 + yBump, 1149, bc.RawInt.ToString() + "/" + FSATS.NormalINT.ToString());

                AddButton(24, 325 + yBump, 4005, 4006, 12, GumpButtonType.Reply, 0);
                AddButton(24, 350 + yBump, 4005, 4006, 13, GumpButtonType.Reply, 0);
                AddButton(24, 375 + yBump, 4005, 4006, 14, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            BaseCreature bc = (BaseCreature)m_Pet;

            if (from == null)
                return;

            if (info.ButtonID == 1)
            {
                if (bc.HitsMax >= FSATS.NormalHITS)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;

                    if (bc.HitsMaxSeed != -1)
                        bc.HitsMaxSeed += 1;
                    else
                        bc.HitsMaxSeed = bc.HitsMax + 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 2)
            {
                if (bc.StamMax >= FSATS.NormalSTAM)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;

                    if (bc.StamMaxSeed != -1)
                        bc.StamMaxSeed += 1;
                    else
                        bc.StamMaxSeed = bc.StamMax + 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 3)
            {
                if (bc.ManaMax >= FSATS.NormalMANA)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;

                    if (bc.ManaMaxSeed != -1)
                        bc.ManaMaxSeed += 1;
                    else
                        bc.ManaMaxSeed = bc.ManaMax + 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            /* if (info.ButtonID == 4)
            {
                if (bc.PhysicalResistance >= FSATS.NormalPhys)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.PhysicalResistanceSeed += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            } */

            /* if (info.ButtonID == 5)
            {
                if (bc.FireResistance >= FSATS.NormalFire)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.FireResistSeed += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 6)
            {
                if (bc.ColdResistance >= FSATS.NormalCold)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.ColdResistSeed += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 7)
            {
                if (bc.EnergyResistance >= FSATS.NormalEnergy)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.EnergyResistSeed += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 8)
            {
                if (bc.PoisonResistance >= FSATS.NormalPoison)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.PoisonResistSeed += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            } */

            if (info.ButtonID == 9)
            {
                if ( !(bc.Level >= 8) )
                {
                    from.SendMessage("Stop trying to cheat!");
                    return;
                }    

                if (bc.DamageMin >= FSATS.NormalMinDam)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.DamageMin += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 10)
            {
                if ( !(bc.Level >= 8) )
                {
                    from.SendMessage("Stop trying to cheat!");
                    return;
                }    

                if (bc.DamageMax >= FSATS.NormalMaxDam)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.DamageMax += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 11)
            {
                if ( !(bc.Level >= 4) )
                {
                    from.SendMessage("Stop trying to cheat!");
                    return;
                }    

                if (bc.VirtualArmor >= FSATS.NormalVArmor)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.VirtualArmor += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 12)
            {
                if ( !(bc.Level >= 10) )
                {
                    from.SendMessage("Stop trying to cheat!");
                    return;
                }    

                if (bc.RawStr >= FSATS.NormalSTR)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.Str += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 13)
            {
                if ( !(bc.Level >= 10) )
                {
                    from.SendMessage("Stop trying to cheat!");
                    return;
                }    

                if (bc.RawDex >= FSATS.NormalDEX)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.Dex += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 14)
            {
                if ( !(bc.Level >= 10) )
                {
                    from.SendMessage("Stop trying to cheat!");
                    return;
                }    

                if (bc.RawInt >= FSATS.NormalINT)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.Int += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            /* if (info.ButtonID == 15)
            {
                if (bc.RoarAttack >= 100)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.RoarAttack += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 16)
            {
                if (bc.PetPoisonAttack >= 100)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.PetPoisonAttack += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            }

            if (info.ButtonID == 17)
            {
                if (bc.FireBreathAttack >= 100)
                {
                    from.SendMessage("This cannot gain any farther.");

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else if (bc.AbilityPoints != 0)
                {
                    bc.AbilityPoints -= 1;
                    bc.FireBreathAttack += 1;

                    if (bc.AbilityPoints != 0)
                        from.SendGump(new PetLevelGump(bc));
                }
                else
                {
                    from.SendMessage("Your pet lacks the ability points to do that.");
                }
            } */
        }
    }
}
