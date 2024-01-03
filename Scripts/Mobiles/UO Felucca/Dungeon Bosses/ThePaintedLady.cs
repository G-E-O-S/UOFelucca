using System;
using System.Linq;
using System.Collections;

using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a Lady's corpse")]
    public class ThePaintedLady : BaseCreature
    {
        [Constructable]
        public ThePaintedLady()
            : base(AIType.AI_Mystic, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "the Painted Lady";
            NameHue = 33;
            Body = 728;
            Hue = 1163;
            BaseSoundID = 1554;
            SpellHue = 1162;

            SetStr(488, 520);
            SetDex(121, 150);
            SetInt(498, 501);

            SetHits(7249, 7499);

            SetDamage(26, 34);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 99.1, 100.0);
            SetSkill(SkillName.Meditation, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 80.1, 90.0);
            SetSkill(SkillName.Mysticism, 49.1, 61.0);

            Fame = 99000;
            Karma = -99000;

            VirtualArmor = 80;

            //SetSpecialAbility(SpecialAbility.LifeDrain);
        }

        public ThePaintedLady(Serial serial)
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

            //if (Paragon.ChestChance > Utility.RandomDouble())
            c.DropItem(new Gold(1099, 1999));
            c.DropItem(new ParagonChest(Name, 3));
            c.DropItem(new ParagonChest(Name, 4));
            if (Utility.RandomDouble() < 0.50)
                c.DropItem(new ParagonChest(Name, 4));
            //c.DropItem(new ParagonChest(Name, 5));
            c.DropItem(new IDWand());
            c.DropItem(new LightningWand());
            c.DropItem(new GreaterHealWand());
            c.DropItem(new RewardScrollDeed());
            c.DropItem(new PaintedCavesCloth());
            PowerScroll ps = PowerScroll.CreateRandom(5, 5);
            ps.Value = Utility.RandomList(105, 110, 115, 120);
            if (Utility.RandomDouble() < 0.50)
                c.DropItem(ps);
            if (Utility.RandomDouble() < 0.40)
                c.DropItem(new SocketDeedPlusOne());
            if (Utility.RandomDouble() < 0.30)
                c.DropItem(new PaintedCavesBackpackDye());
            if (Utility.RandomDouble() < 0.20)
                c.DropItem(new PaintedCavesBeardDye());
            if (Utility.RandomDouble() < 0.10)
                c.DropItem(new PaintedCavesCandle());
            if (Utility.RandomDouble() < 0.08)
                c.DropItem(new PaintedCavesLantern());
            if (Utility.RandomDouble() < 0.06)
                c.DropItem(new PaintedCavesTorch());
            if (Utility.RandomDouble() < 0.04)
                c.DropItem(new PaintedCavesSpellbook());
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
