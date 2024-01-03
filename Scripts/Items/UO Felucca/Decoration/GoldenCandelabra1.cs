using System;

namespace Server.Items
{
    public class GoldenCandelabra1 : BaseAddon
    {
        [Constructable]
        public GoldenCandelabra1()
        {
            AddComponent(new LocalizedAddonComponent(0xA6DC, 1007150), 1, 1, 0);
            Name = "Golden Candelabra";
        }

        public GoldenCandelabra1(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed { get { return new EggNogDeed(); } }

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

    public class GoldenCandelabra1Deed : BaseAddonDeed
    {
        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public GoldenCandelabra1Deed()
        {
            LootType = LootType.Blessed;
            Hue = 1266;
            Name = "Golden Candelabra";
        }

        public GoldenCandelabra1Deed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new GoldenCandelabra1(); } }

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
