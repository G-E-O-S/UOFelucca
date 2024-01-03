using System;
using System.Linq;
using System.Collections;

using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a horrific corpse")]
    public class TheHorrorOfOrcCave : BaseCreature
    {
        [Constructable]
        public TheHorrorOfOrcCave()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "the Horror of Orc Cave";
            NameHue = 33;
            Body = 189;
            Hue = 1258;
            BaseSoundID = 352;
            SpellHue = 1257;

            SetStr(488, 520);
            SetDex(121, 150);
            SetInt(498, 501);

            SetHits(5001, 5499);

            SetDamage(18, 22);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 22.2, 44.4);
            SetSkill(SkillName.Magery, 99.1, 100.0);
            SetSkill(SkillName.Meditation, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 80.1, 90.0);
            SetSkill(SkillName.Mysticism, 49.1, 61.0);

            Fame = 9000;
            Karma = -9000;

            VirtualArmor = 80;

            //SetSpecialAbility(SpecialAbility.LifeDrain);
        }

        public TheHorrorOfOrcCave(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 5;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.OldSuperBoss, 2);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new Gold(999, 1999));
            c.DropItem(new ParagonChest(Name, 3));
            c.DropItem(new ParagonChest(Name, 4));
            c.DropItem(new IDWand());
            c.DropItem(new LightningWand());
            c.DropItem(new GreaterHealWand());
            c.DropItem(new RewardScrollDeed());
            c.DropItem(new OrcCaveCloth());    
            PowerScroll ps = PowerScroll.CreateRandom(5, 5);
            ps.Value = Utility.RandomList(105, 110, 115, 120);
            if (Utility.RandomDouble() < 0.50)
                c.DropItem(ps);

            if (Utility.RandomDouble() < 0.5)
            {
                switch (Utility.Random(5)) // Since you have 5 items now
                {
                    case 0:
                        c.DropItem(new BoneContainer1());
                        break;
                    case 1:
                        c.DropItem(new BoneContainer2());
                        break;
                    case 2:
                        c.DropItem(new BoneContainer3());
                        break;
                    case 3:
                        c.DropItem(new BoneContainer4());
                        break;
                    case 4:
                        c.DropItem(new BoneContainer5());
                        break;
                }
            }

            if (Utility.RandomDouble() < 0.40)
                c.DropItem(new SocketDeedPlusOne());
            if (Utility.RandomDouble() < 0.30)
                c.DropItem(new OrcCaveBackpackDye());
            if (Utility.RandomDouble() < 0.20)
                c.DropItem(new OrcCaveBeardDye());
            if (Utility.RandomDouble() < 0.10)
                c.DropItem(new OrcCaveCandle());
            if (Utility.RandomDouble() < 0.08)
                c.DropItem(new OrcCaveLantern());
            if (Utility.RandomDouble() < 0.06)
                c.DropItem(new OrcCaveTorch());
            if (Utility.RandomDouble() < 0.04)
                c.DropItem(new OrcCaveSpellbook());
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
