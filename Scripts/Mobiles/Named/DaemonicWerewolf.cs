using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a daemonic corpse")]
    public class DaemonicWerewolf : BaseMount
    {
        public override double HealChance { get { return 1.00; } }

        [Constructable]
        public DaemonicWerewolf()
            : this("a daemonic werewolf")
        {
        }

        [Constructable]
        public DaemonicWerewolf(string name)
            : base(name, 1410, 1410, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4) // default 0x3E91,
        {
            Hue = 1910;

            SetStr(725, 750);
            SetDex(125, 150);
            SetInt(125, 150);

            SetHits(1100, 1250);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 25, 45);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 70, 85);

            SetSkill(SkillName.Wrestling, 90.1, 96.8);
            SetSkill(SkillName.Tactics, 90.3, 99.3);
            SetSkill(SkillName.MagicResist, 75.3, 90.0);
            SetSkill(SkillName.Anatomy, 22.2, 44);
            SetSkill(SkillName.Healing, 22.2, 44);

            Fame = 5000;  //Guessing here
            Karma = 5000;  //Guessing here

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 95.1;
            VirtualArmor = 80;

            PackGold(1000, 1600);

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public DaemonicWerewolf(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel
        {
            get { return 5; }
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
                return true;
            }
        }
        //public override bool StatLossAfterTame
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Canine;
            }
        }

        public override int Hides
        {
            get
            {
                return 10;
            }
        }
        public override int Meat
        {
            get
            {
                return 3;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosFilthyRich, 5);
        }

        //public override void OnAfterTame(Mobile tamer)
        //{
        //    if (Owners.Count == 0 && PetTrainingHelper.Enabled)
        //    {
        //        if (RawStr > 0)
        //            RawStr = (int)Math.Max(1, RawStr * 0.5);

        //        if (RawDex > 0)
        //            RawDex = (int)Math.Max(1, RawDex * 0.5);

        //        if (HitsMaxSeed > 0)
        //            HitsMaxSeed = (int)Math.Max(1, HitsMaxSeed * 0.5);

        //        Hits = Math.Min(HitsMaxSeed, Hits);
        //        Stam = Math.Min(RawDex, Stam);
        //    }
        //    else
        //    {
        //        base.OnAfterTame(tamer);
        //    }
        //}

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
            return 0x577;
        }

        public override int GetAttackSound()
        {
            return 0x576;
        }

        public override int GetAngerSound()
        {
            return 0x578;
        }

        public override int GetHurtSound()
        {
            return 0x576;
        }

        public override int GetDeathSound()
        {
            return 0x579;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)3); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 3 && Controlled && RawStr >= 1200 && ControlSlots == ControlSlotsMin)
            {
                Server.SkillHandlers.AnimalTaming.ScaleStats(this, 0.5);
            }

            if (version < 1 && Name == "a werewolf")
                Name = "a werewolf";

            if (version == 1)
            {
                SetWeaponAbility(WeaponAbility.BleedAttack);
            }
        }
    }
}
