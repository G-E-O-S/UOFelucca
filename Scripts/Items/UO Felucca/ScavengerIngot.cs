using System;

namespace Server.Items
{
    [FlipableAttribute(0x1BF2, 0x1BEF)]

    public class ScavengerIngot : Item
    {
        [Constructable]
        public ScavengerIngot()
            : base(0x1BF2)
        {
            Hue = 1174;
            Name = "Scavenger";
            Stackable = true;
            LootType = LootType.Blessed;
        }       

        public ScavengerIngot(Serial serial)
            : base(serial)
        {
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
