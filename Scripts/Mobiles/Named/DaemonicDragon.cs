using System;

namespace Server.Mobiles
{
    [CorpseName("a daemonic corpse")]
    public class DaemonicDragon : BaseCreature
    {
        [Constructable]
        public DaemonicDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a daemonic dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = 1910;

            SetStr(825, 850);
            SetDex(100, 150);
            SetInt(450, 500);

            SetHits(500, 510);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.EvalInt, 30.1, 40.0);
            SetSkill(SkillName.Magery, 30.1, 40.0);
            SetSkill(SkillName.MagicResist, 99.1, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 92.5);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 80;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;
            SpellHue = 1909;

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public DaemonicDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement
        {
            get
            {
                return !Controlled;
            }
        }
        public override bool AutoDispel
        {
            get
            {
                return !Controlled;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 4;
            }
        }
        public override int Meat
        {
            get
            {
                return 19;
            }
        }
        public override int DragonBlood
        {
            get
            {
                return 2;
            }
        }
        public override int Hides
        {
            get
            {
                return 20;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Barbed;
            }
        }
        public override int Scales
        {
            get
            {
                return 7;
            }
        }
        public override ScaleType ScaleType
        {
            get
            {
                return (Body == 12 ? ScaleType.Yellow : ScaleType.Red);
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override bool CanAngerOnTame
        {
            get
            {
                return false;
            }
        }
        public override bool CanFly
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 8);
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
