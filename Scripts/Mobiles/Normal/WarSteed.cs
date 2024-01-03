using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a warsteed corpse")]
    public class WarSteed : BaseMount
    {
        [Constructable]
        public WarSteed() : this("a warsteed")
        {
        }

        [Constructable]
        public WarSteed(string name) : base(name, 0x74, 0x3EA7, AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xA8;
            Hue = Utility.RandomList ( 0, 138, 1109, 148, 1175, 1194, 33, 1161 );

            SetStr(100, 250);
            SetDex(86, 150);
            SetInt(86, 150);

            SetHits(325, 425);

            SetDamage(16, 22);

            //SetDamageType(ResistanceType.Physical, 40);
            //SetDamageType(ResistanceType.Fire, 40);
            //SetDamageType(ResistanceType.Energy, 20);

            //SetResistance(ResistanceType.Physical, 55, 65);
            //SetResistance(ResistanceType.Fire, 30, 40);
            //SetResistance(ResistanceType.Cold, 30, 40);
            //SetResistance(ResistanceType.Poison, 30, 40);
            //SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 10.4, 50.0);
            SetSkill(SkillName.Magery, 10.4, 50.0);
            SetSkill(SkillName.MagicResist, 85.3, 100.0);
            SetSkill(SkillName.Tactics, 97.6, 95.0);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);
            SetSkill(SkillName.Anatomy, 80.5, 92.5);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 80;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 25.5;
                   
            int totalstats = this.Str + this.Dex + this.Int + this.HitsMax + this.StamMax + this.ManaMax + this.PhysicalResistance + this.FireResistance + this.ColdResistance + this.EnergyResistance + this.PoisonResistance + this.DamageMin + this.DamageMax + this.VirtualArmor;
            int nextlevel = totalstats * 10;

            switch (Utility.Random(3))
            {
                case 0:
                    {
                        BodyValue = 284;
                        ItemID = 16018;
                        break;
                    }
                case 1:
                    {
                        BodyValue = 284;
                        ItemID = 16018;
                        break;
                    }
                case 2:
                    {
                        BodyValue = 284;
                        ItemID = 16018;
                        break;
                    }
            }

            PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            //AddLoot( LootPack.LowScrolls );
            //AddLoot( LootPack.Potions );
        }

        public override int GetAngerSound()
        {
            if (!Controlled)
                return 0x16A;

            return base.GetAngerSound();
        }

        public override int Meat { get { return 5; } }
        public override int Hides { get { return 10; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public WarSteed(Serial serial) : base(serial)
        {
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

            if (BaseSoundID == 0x16A)
                BaseSoundID = 0xA8;
        }
    }

}
