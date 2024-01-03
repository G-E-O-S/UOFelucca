using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a battle turkey corpse")]
    public class BattleTurkey : BaseMount
    {
        
        [Constructable]
        public BattleTurkey()
            : this("Battle Turkey")
        {
        }

        [Constructable]
        public BattleTurkey(string name)
            : base(name, 1026, 1026, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4) // default 0x3E91,
        {
            //Hue = 1910;
            Title = "[Thanksgiving]";

            BaseSoundID = 0x66A;

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
            SetSkill(SkillName.MagicResist, 55.3, 100.0);
            SetSkill(SkillName.Tactics, 57.6, 100.0);
            SetSkill(SkillName.Wrestling, 40.5, 92.5);
            SetSkill(SkillName.Anatomy, 37.6, 100.0);
            SetSkill(SkillName.Meditation, 20.5, 92.5);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 80;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 95.1;
            SpellHue = 1909;

            Fame = 5000;  //Guessing here
            Karma = 5000;  //Guessing here           

            PackGold(101, 160);

            SetSpecialAbility(SpecialAbility.DragonBreath);
        }

        public BattleTurkey(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel
        {
            get { return 5; }
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
        public override int Feathers { get { return 25; } }

        public override bool CanAngerOnTame
        {
            get
            {
                return true;
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
            AddLoot(LootPack.AosFilthyRich, 5);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new TurkeyPlatter());                        
        }        

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Race != Race.Human && from == ControlMaster && from.IsPlayer())
            {
                Item pads = from.FindItemOnLayer(Layer.Shoes);

                if (pads is PadsOfTheCuSidhe)
                    from.SendLocalizedMessage(1071981); // Your boots allow you to mount the Cu Sidhe.
                else
                {
                    from.SendLocalizedMessage(1072203); // Only Elves may use 
                    return;
                }
            }

            base.OnDoubleClick(from);
        }

        public override int GetIdleSound()
        {
            return 0x66A;
        }

        public override int GetAttackSound()
        {
            return 0x66B;
        }

        public override int GetAngerSound()
        {
            return 0x66B;
        }

        public override int GetHurtSound()
        {
            return 0x66A;
        }

        public override int GetDeathSound()
        {
            return 0x66B;
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
