using Server.ContextMenus;
using Server.Engines.XmlSpawner2;
using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using Server.Engines.Craft;

namespace Server.Items
{
    [FlipableAttribute(0x1439, 0x1438)]
    public class Mjollnir : BaseMeleeWeapon, Server.Engines.Craft.IRepairable
    {

        public CraftSystem RepairSystem { get { return DefBlacksmithy.CraftSystem; } }

        [Constructable]
        public Mjollnir()
            : base(0x1439)
        {
            this.Weight = 10.0;
            this.LootType = LootType.Newbied;
            this.Hue = Utility.RandomList(1175, 1910, 1161, 1194, 1109, 1150, 2721, 2068, 2742, 1261, 2739, 2731, 1287);
            Name = "Mjöllnir";
            this.DurabilityLevel = Utility.RandomList(WeaponDurabilityLevel.Massive, WeaponDurabilityLevel.Fortified, WeaponDurabilityLevel.Indestructible);
            this.DamageLevel = Utility.RandomList(WeaponDamageLevel.Force, WeaponDamageLevel.Power, WeaponDamageLevel.Vanq);
            this.AccuracyLevel = Utility.RandomList(WeaponAccuracyLevel.Eminently, WeaponAccuracyLevel.Exceedingly, WeaponAccuracyLevel.Supremely);
            this.Slayer = SlayerName.None;
            this.Skill = SkillName.Macing;
            this.Layer = Layer.TwoHanded;
            MaxHitPoints = 999;
            HitPoints = 999;

            if (Utility.RandomDouble() < 0.50)
            {
                this.Slayer = Utility.RandomList(SlayerName.Silver, SlayerName.DragonSlaying, SlayerName.DaemonDismissal, SlayerName.BalronDamnation, SlayerName.TrollSlaughter,
                    SlayerName.OgreTrashing, SlayerName.GargoylesFoe, SlayerName.LizardmanSlaughter, SlayerName.BloodDrinking, SlayerName.Fey, SlayerName.OrcSlaying, SlayerName.SnakesBane,
                    SlayerName.SpidersDeath, SlayerName.ScorpionsBane, SlayerName.FlameDousing, SlayerName.WaterDissipation, SlayerName.Vacuum, SlayerName.EarthShatter, SlayerName.BloodDrinking,
                    SlayerName.SummerWind, SlayerName.ElementalHealth, SlayerName.Exorcism, SlayerName.Repond, SlayerName.ReptilianDeath, SlayerName.ArachnidDoom, SlayerName.ElementalBan, SlayerName.Terathan,
                    SlayerName.Ophidian);
            }

            switch (Utility.Random(0))
            {
                case 0:
                    // add a specific list of custom attacks like this
                    XmlAttach.AttachTo(this,
                        new XmlCustomAttacks(
                            new XmlCustomAttacks.SpecialAttacks[]
                            {
                            XmlCustomAttacks.SpecialAttacks.LightningStrike
                            }
                        )
                    );
                    break;
            }
        }

        public Mjollnir(Serial serial) : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new InfoEntry(player));
            }
        }

        private class InfoEntry : ContextMenuEntry
        {
            private PlayerMobile m_Player;

            public InfoEntry(PlayerMobile player)
                : base(1079449, 4)
            {
                m_Player = player;
            }

            public override void OnClick()
            {
                m_Player.SendGump(new MjollnirGump());
            }

            private class MjollnirGump : Gump
            {
                public MjollnirGump() : base(0, 360)
                {
                    this.Closable = true;
                    this.Disposable = true;
                    this.Dragable = true;
                    this.Resizable = false;

                    this.AddImage(0, 0, 39925);
                    this.AddImage(36, 0, 39926);
                    this.AddImage(72, 0, 39926);
                    this.AddImage(108, 0, 39926);
                    this.AddImage(144, 0, 39926);
                    this.AddImage(180, 0, 39926);
                    this.AddImage(216, 0, 39926);
                    this.AddImage(252, 0, 39926);
                    this.AddImage(288, 0, 39926);
                    this.AddImage(324, 0, 39926);
                    this.AddImage(360, 0, 39927);
                    this.AddImage(0, 36, 39928);
                    this.AddImage(36, 36, 39929);
                    this.AddImage(72, 36, 39929);
                    this.AddImage(108, 36, 39929);
                    this.AddImage(144, 36, 39929);
                    this.AddImage(180, 36, 39929);
                    this.AddImage(216, 36, 39929);
                    this.AddImage(252, 36, 39929);
                    this.AddImage(288, 36, 39929);
                    this.AddImage(324, 36, 39929);
                    this.AddImage(360, 36, 39930);
                    this.AddImage(0, 72, 39928);
                    this.AddImage(36, 72, 39929);
                    this.AddImage(72, 72, 39929);
                    this.AddImage(108, 72, 39929);
                    this.AddImage(144, 72, 39929);
                    this.AddImage(180, 72, 39929);
                    this.AddImage(216, 72, 39929);
                    this.AddImage(252, 72, 39929);
                    this.AddImage(288, 72, 39929);
                    this.AddImage(324, 72, 39929);
                    this.AddImage(360, 72, 39930);
                    this.AddImage(0, 108, 39928);
                    this.AddImage(36, 108, 39929);
                    this.AddImage(72, 108, 39929);
                    this.AddImage(108, 108, 39929);
                    this.AddImage(144, 108, 39929);
                    this.AddImage(180, 108, 39929);
                    this.AddImage(216, 108, 39929);
                    this.AddImage(252, 108, 39929);
                    this.AddImage(288, 108, 39929);
                    this.AddImage(324, 108, 39929);
                    this.AddImage(360, 108, 39930);
                    this.AddImage(0, 144, 39928);
                    this.AddImage(36, 144, 39929);
                    this.AddImage(72, 144, 39929);
                    this.AddImage(108, 144, 39929);
                    this.AddImage(144, 144, 39929);
                    this.AddImage(180, 144, 39929);
                    this.AddImage(216, 144, 39929);
                    this.AddImage(252, 144, 39929);
                    this.AddImage(288, 144, 39929);
                    this.AddImage(324, 144, 39929);
                    this.AddImage(360, 144, 39930);
                    this.AddImage(0, 180, 39928);
                    this.AddImage(36, 180, 39929);
                    this.AddImage(72, 180, 39929);
                    this.AddImage(108, 180, 39929);
                    this.AddImage(144, 180, 39929);
                    this.AddImage(180, 180, 39929);
                    this.AddImage(216, 180, 39929);
                    this.AddImage(252, 180, 39929);
                    this.AddImage(288, 180, 39929);
                    this.AddImage(324, 180, 39929);
                    this.AddImage(360, 180, 39930);
                    this.AddImage(0, 216, 39928);
                    this.AddImage(36, 216, 39929);
                    this.AddImage(72, 216, 39929);
                    this.AddImage(108, 216, 39929);
                    this.AddImage(144, 216, 39929);
                    this.AddImage(180, 216, 39929);
                    this.AddImage(216, 216, 39929);
                    this.AddImage(252, 216, 39929);
                    this.AddImage(288, 216, 39929);
                    this.AddImage(324, 216, 39929);
                    this.AddImage(360, 216, 39930);
                    this.AddImage(0, 252, 39928);
                    this.AddImage(36, 252, 39929);
                    this.AddImage(72, 252, 39929);
                    this.AddImage(108, 252, 39929);
                    this.AddImage(144, 252, 39929);
                    this.AddImage(180, 252, 39929);
                    this.AddImage(216, 252, 39929);
                    this.AddImage(252, 252, 39929);
                    this.AddImage(288, 252, 39929);
                    this.AddImage(324, 252, 39929);
                    this.AddImage(360, 252, 39930);
                    this.AddImage(0, 288, 39928);
                    this.AddImage(36, 288, 39929);
                    this.AddImage(72, 288, 39929);
                    this.AddImage(108, 288, 39929);
                    this.AddImage(144, 288, 39929);
                    this.AddImage(180, 288, 39929);
                    this.AddImage(216, 288, 39929);
                    this.AddImage(252, 288, 39929);
                    this.AddImage(288, 288, 39929);
                    this.AddImage(324, 288, 39929);
                    this.AddImage(360, 288, 39930);
                    this.AddImage(0, 324, 39928);
                    this.AddImage(36, 324, 39929);
                    this.AddImage(72, 324, 39929);
                    this.AddImage(108, 324, 39929);
                    this.AddImage(144, 324, 39929);
                    this.AddImage(180, 324, 39929);
                    this.AddImage(216, 324, 39929);
                    this.AddImage(252, 324, 39929);
                    this.AddImage(288, 324, 39929);
                    this.AddImage(324, 324, 39929);
                    this.AddImage(360, 324, 39930);
                    this.AddImage(0, 360, 39931);
                    this.AddImage(36, 360, 39932);
                    this.AddImage(72, 360, 39932);
                    this.AddImage(108, 360, 39932);
                    this.AddImage(144, 360, 39932);
                    this.AddImage(180, 360, 39932);
                    this.AddImage(216, 360, 39932);
                    this.AddImage(252, 360, 39932);
                    this.AddImage(288, 360, 39932);
                    this.AddImage(324, 360, 39932);
                    this.AddImage(360, 360, 39933);

                    this.AddLabel(153, 65, 1161, "Exodus Weapon");
                    this.AddLabel(20, 90, 1153, "Mjöllnir (Mace Fighting) - This ancient weapon was crafted");
                    this.AddLabel(20, 110, 1153, "by a god who is said to have ruled over lightning. It is");
                    this.AddLabel(20, 130, 1153, "unknown as to if he was good or evil or indeed even a 'he'.");
                    this.AddLabel(20, 150, 1153, "It has the ability to hit with the force of thunder!");

                    this.AddLabel(20, 190, 1153, "On Use - Lightning Strike");
                    this.AddLabel(20, 210, 1153, "Damage Bonus - 100%");
                    this.AddLabel(20, 230, 1153, "Requirements - GM Mace Fighting");
                    this.AddLabel(20, 250, 1153, "Use Cost - Reward Scroll");

                    this.AddImage(165, 15, 494, 1161);

                    this.AddLabel(120, 330, 33, "*Does not work in PvP*");

                    this.AddButton(165, 360, 247, 248, 249, GumpButtonType.Reply, 0);
                }
            }
        }

        public override int OldStrengthReq
        {
            get
            {
                return 10;
            }
        }

        public override int OldMinDamage
        {
            get
            {
                return 8;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 36;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 31;
            }
        }

        public override WeaponAnimation DefAnimation
        {
            get
            {
                return WeaponAnimation.Bash2H;
            }
        }

        public override int DefHitSound
        {
            get
            {
                return 0x23B;
            }
        }

        public override int DefMissSound
        {
            get
            {
                return 0x23A;
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
