using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a daemonic corpse")]
    public class DaemonicBeetle : BaseCreature
    {
        private static readonly Hashtable m_Table = new Hashtable();
        [Constructable]
        public DaemonicBeetle()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a daemonic beetle";
            Body = 244;

            SetStr(825, 850);
            SetDex(125, 150);
            SetInt(450, 500);

            SetHits(700, 750);

            SetDamage(15, 22);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 10);
            SetDamageType(ResistanceType.Energy, 70);

            SetResistance(ResistanceType.Physical, 40, 65);
            SetResistance(ResistanceType.Fire, 35, 50);
            SetResistance(ResistanceType.Cold, 35, 50);
            SetResistance(ResistanceType.Poison, 75, 95);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.EvalInt, 100.1, 125.0);
            SetSkill(SkillName.Magery, 100.1, 110.0);
            SetSkill(SkillName.Poisoning, 120.1, 140.0);
            SetSkill(SkillName.MagicResist, 95.1, 110.0);
            SetSkill(SkillName.Tactics, 78.1, 93.0);
            SetSkill(SkillName.Wrestling, 70.1, 77.5);

            VirtualArmor = 80;
            Hue = 1910;
            Fame = 15000;
            Karma = -15000;
            SpellHue = 1909;

            if (Utility.RandomDouble() < .25)
                PackItem(Engines.Plants.Seed.RandomBonsaiSeed());

            PackBodyPartOrBones();

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            SetSpecialAbility(SpecialAbility.RuneCorruption);
            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public DaemonicBeetle(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Greater;
            }
        }
        public override Poison HitPoison
        {
            get
            {
                return Poison.Greater;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
            }
        }
        public override bool CanAngerOnTame
        {
            get
            {
                return false;
            }
        }

        public override int GetAngerSound()
        {
            return 0x4E8;
        }

        public override int GetIdleSound()
        {
            return 0x4E7;
        }

        public override int GetAttackSound()
        {
            return 0x4E6;
        }

        public override int GetHurtSound()
        {
            return 0x4E9;
        }

        public override int GetDeathSound()
        {
            return 0x4E5;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 1);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)3);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version < 1)
            {
                for (int i = 0; i < Skills.Length; ++i)
                {
                    Skills[i].Cap = Math.Max(100.0, Skills[i].Cap * 0.9);

                    if (Skills[i].Base > Skills[i].Cap)
                    {
                        Skills[i].Base = Skills[i].Cap;
                    }
                }
            }

            if (version < 3)
            {
                if (AbilityProfile == null || AbilityProfile.MagicalAbility == MagicalAbility.None)
                {
                    SetMagicalAbility(MagicalAbility.Poisoning);
                }

                if (version == 1)
                {
                    SetSpecialAbility(SpecialAbility.RuneCorruption);
                    SetWeaponAbility(WeaponAbility.BleedAttack);
                }
            }
        }
    }
}
