using System;

namespace Server.Items
{
    public class ChristmasTreeHuge2023 : BaseAddon
    {
        [Constructable]
        public ChristmasTreeHuge2023()
        {
            AddComponent(new LocalizedAddonComponent(0x9DBB, 1007150), 1, 1, 0);
        }

        public ChristmasTreeHuge2023(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed { get { return new ChristmasTreeHugeDeed2023(); } }

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

    public class ChristmasTreeHugeDeed2023 : BaseAddonDeed
    {
        public override int LabelNumber { get { return 1007150; } } // Holiday Tree

        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public ChristmasTreeHugeDeed2023()
        {
            LootType = LootType.Blessed;
            Hue = 1266;
        }

        public ChristmasTreeHugeDeed2023(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new ChristmasTreeHuge2023(); } }

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
