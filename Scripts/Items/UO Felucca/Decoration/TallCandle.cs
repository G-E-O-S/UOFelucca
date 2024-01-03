using System;

namespace Server.Items
{
    public class TallCandle : BaseAddon
    {
        [Constructable]
        public TallCandle()
        {
            AddComponent(new LocalizedAddonComponent(0xA6D7, 1007150), 1, 1, 0);
            Name = "Tall Candle";
        }

        public TallCandle(Serial serial)
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

    public class TallCandleDeed : BaseAddonDeed
    {     
        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public TallCandleDeed()
        {
            LootType = LootType.Blessed;
            Hue = 1266;
            Name = "Tall Candle";
        }

        public TallCandleDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new TallCandle(); } }

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
