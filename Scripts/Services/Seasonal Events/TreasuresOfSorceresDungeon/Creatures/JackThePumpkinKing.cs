using System;

using Server.Mobiles;
using Server.Items;

namespace Server.Engines.SorcerersDungeon
{
    [CorpseName("the corpse of jack the pumpkin king")]
    public class JackThePumpkinKing : BaseCreature
    {
        [Constructable]
        public JackThePumpkinKing()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "jack";
            Title = "the pumpkin king";
            Body = 0x190;
            Hue = Race.RandomSkinHue();

            SetStr(500);
            SetDex(200);
            SetInt(1200);

            SetHits(26000);

            SetDamage(22, 26);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25);
            SetSkill(SkillName.MagicResist, 120);
            SetSkill(SkillName.Tactics, 100, 110);
            SetSkill(SkillName.Wrestling, 130, 140);
            SetSkill(SkillName.Parry, 20, 30);

            SetSkill(SkillName.Magery, 120);
            SetSkill(SkillName.EvalInt, 120);
            SetSkill(SkillName.Necromancy, 120);
            SetSkill(SkillName.SpiritSpeak, 120);
            SetSkill(SkillName.Meditation, 120);
            SetSkill(SkillName.Focus, 70, 80);

            Fame = 12000;
            Karma = -12000;

            SetWearable(new ClothNinjaHood(), 1281);
            SetWearable(new BoneChest(), 1175);
            SetWearable(new BoneArms(), 1175);
            SetWearable(new BoneGloves(), 1175);
            SetWearable(new ThighBoots());
            SetWearable(new Scepter());
        }

        private bool m_InHere;

        // gleefully stolen from DemonKnight
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (from != null && from != this && !m_InHere)
            {
                m_InHere = true;
                AOS.Damage(from, this, Utility.RandomMinMax(8, 20), 100, 0, 0, 0, 0);

                MovingEffect(from, Utility.RandomMinMax(0xC6A, 0xC6C), 10, 0, false, false, 0, 0);
                PlaySound(0x491);

                if (0.05 > Utility.RandomDouble())
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(CreatePumpkin_Callback), from);

                m_InHere = false;
            }
        }

        public virtual void CreatePumpkin_Callback(object state)
        {
            Mobile from = (Mobile)state;
            Map map = from.Map;

            if (map == null)
                return;

            int count = Utility.RandomMinMax(1, 3);

            for (int i = 0; i < count; ++i)
            {
                int x = from.X + Utility.RandomMinMax(-1, 1);
                int y = from.Y + Utility.RandomMinMax(-1, 1);
                int z = from.Z;

                if (!map.CanFit(x, y, z, 16, false, true))
                {
                    z = map.GetAverageZ(x, y);

                    if (z == from.Z || !map.CanFit(x, y, z, 16, false, true))
                        continue;
                }

                UnholyPumpkin bone = new UnholyPumpkin();

                bone.Hue = 0;
                bone.Name = "unholy pumpkin";
                bone.ItemID = Utility.RandomMinMax(0xC6A, 0xC6C);

                bone.MoveToWorld(new Point3D(x, y, z), map);
            }
        }

        public JackThePumpkinKing(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
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

    public class UnholyPumpkin : Item, ICarvable
    {
        private SpawnTimer m_Timer;

        [Constructable]
        public UnholyPumpkin()
            : base(0xF7E)
        {
            Movable = false;

            m_Timer = new SpawnTimer(this);
            m_Timer.Start();
        }

        public UnholyPumpkin(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "unholy pumpkin";
            }
        }
        public bool Carve(Mobile from, Item item)
        {
            Effects.PlaySound(GetWorldLocation(), Map, 0x48F);
            Effects.SendLocationEffect(GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0);

            if (0.3 > Utility.RandomDouble())
            {
                from.SendMessage("You destroy the pumpkin.");
                Gold gold = new Gold(25, 100);

                gold.MoveToWorld(GetWorldLocation(), Map);

                Delete();
                m_Timer.Stop();
            }
            else
            {
                from.SendMessage("You damage the pumpkin.");
            }

            return true;
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

            m_Timer = new SpawnTimer(this);
            m_Timer.Start();
        }

        private class SpawnTimer : Timer
        {
            private readonly Item m_Item;
            public SpawnTimer(Item item)
                : base(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)))
            {
                Priority = TimerPriority.FiftyMS;

                m_Item = item;
            }

            protected override void OnTick()
            {
                if (m_Item.Deleted)
                    return;

                Mobile spawn;

                switch (Utility.Random(12))
                {
                    default:
                    case 0:
                        spawn = new Skeleton();
                        break;
                    case 1:
                        spawn = new Zombie();
                        break;
                    case 2:
                        spawn = new Wraith();
                        break;
                    case 3:
                        spawn = new Spectre();
                        break;
                    case 4:
                        spawn = new Ghoul();
                        break;
                    case 5:
                        spawn = new Mummy();
                        break;
                    case 6:
                        spawn = new Bogle();
                        break;
                    case 7:
                        spawn = new RottingCorpse();
                        break;
                    case 8:
                        spawn = new BoneKnight();
                        break;
                    case 9:
                        spawn = new SkeletalKnight();
                        break;
                    case 10:
                        spawn = new Lich();
                        break;
                    case 11:
                        spawn = new LichLord();
                        break;
                }

                spawn.MoveToWorld(m_Item.Location, m_Item.Map);

                m_Item.Delete();
            }
        }
    }
}
