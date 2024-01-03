using System;

namespace Server.Items
{
    public class ChristmasTreeLarge2023 : BaseAddon
    {
        [Constructable]
        public ChristmasTreeLarge2023()
        {
            AddComponent(new LocalizedAddonComponent(0xA6A9, 1007150), 1, 1, 0);            
        }

        public ChristmasTreeLarge2023(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed { get { return new ChristmasTreeLargeDeed2023(); } }

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

    public class ChristmasTreeLargeDeed2023 : BaseAddonDeed
    {
        public override int LabelNumber { get { return 1007150; } } // Holiday Tree

        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public ChristmasTreeLargeDeed2023()
        {
            LootType = LootType.Blessed;
            Hue = 1266;            
        }

        public ChristmasTreeLargeDeed2023(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new ChristmasTreeLarge2023(); } }

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
