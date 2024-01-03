using System;

namespace Server.Items
{
    public class GoldenCandelabra2 : BaseAddon
    {
        [Constructable]
        public GoldenCandelabra2()
        {
            AddComponent(new LocalizedAddonComponent(0xA6E1, 1007150), 1, 1, 0);
            Name = "Golden Candelabra";
        }

        public GoldenCandelabra2(Serial serial)
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

    public class GoldenCandelabra2Deed : BaseAddonDeed
    {
        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public GoldenCandelabra2Deed()
        {
            LootType = LootType.Blessed;
            Hue = 1266;
            Name = "Golden Candelabra";
        }

        public GoldenCandelabra2Deed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new GoldenCandelabra2(); } }

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
