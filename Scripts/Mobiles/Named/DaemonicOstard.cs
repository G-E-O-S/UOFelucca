using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a daemonic corpse")]
    public class DaemonicOstard : BaseMount
    {
        [Constructable]
        public DaemonicOstard()
            : this("a daemonic ostard")
        {
        }

        [Constructable]
        public DaemonicOstard(string name)
            : base(name, 0xDA, 0x3EA4, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x275;
            Hue = 1910;
            Body = Utility.RandomList(210, 219);            

            SetStr(525, 550);
            SetDex(100, 150);
            SetInt(100, 150);

            SetHits(320, 330);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 10.4, 50.0);
            SetSkill(SkillName.Magery, 10.4, 50.0);
            SetSkill(SkillName.MagicResist, 85.3, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 80;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 95.1;
            SpellHue = 1909;

            //switch (Utility.Random(12))
            //{
            //    case 0: PackItem(new BloodOathScroll()); break;
            //    case 1: PackItem(new HorrificBeastScroll()); break;
            //    case 2: PackItem(new StrangleScroll()); break;
            //    case 3: PackItem(new VengefulSpiritScroll()); break;
            //}

            //switch (Utility.Random(4))
            //{
            //    case 0:
            //        {
            //            BodyValue = 116;
            //            ItemID = 16039;
            //            break;
            //        }
            //    case 1:
            //        {
            //            BodyValue = 177;
            //            ItemID = 16053;
            //            break;
            //        }
            //    case 2:
            //        {
            //            BodyValue = 178;
            //            ItemID = 16041;
            //            break;
            //        }
            //    case 3:
            //        {
            //            BodyValue = 179;
            //            ItemID = 16055;
            //            break;
            //        }
            //}

            //if (Utility.RandomDouble() < 0.05)
            //    Hue = 1910;

            PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));
            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public DaemonicOstard(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 5;
            }
        }
        public override int Hides
        {
            get
            {
                return 10;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Barbed;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.FruitsAndVegies;
            }
        }
        public override bool CanAngerOnTame
        {
            get
            {
                return false;
            }
        }

        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Ostard;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }

        public override int GetAngerSound()
        {
            if (!Controlled)
                return 0x16A;

            return base.GetAngerSound();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
