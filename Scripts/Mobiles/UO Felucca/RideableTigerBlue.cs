using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a tiger corpse")]
    public class RideableTigerBlue : BaseMount
    {
        [Constructable]
        public RideableTigerBlue()
            : this("a tiger")
        {
        }

        [Constructable]
        public RideableTigerBlue(string name)
            : base(name, 0x4E6, 0x3EC7, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Hue = 2122;

            SetStr(476, 500);
            SetDex(191, 220);
            SetInt(491, 600);

            SetHits(476, 500);

            SetDamage(13, 22);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 50, 55);
            SetResistance(ResistanceType.Fire, 35, 55);
            SetResistance(ResistanceType.Cold, 70, 70);
            SetResistance(ResistanceType.Poison, 50, 55);
            SetResistance(ResistanceType.Energy, 50, 55);

            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);

            Fame = 24000;
            Karma = -24000;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 108.0;


        }

        public override void GenerateLoot()
        {
            //AddLoot(LootPack.LootItem<SulfurousAsh>(151, 300, true));

        }

        public RideableTigerBlue(Serial serial)
            : base(serial)
        {
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
