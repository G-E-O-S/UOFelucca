using System;

namespace Server.Items
{
    public class SpinningGlobeOfSosariaAddon : BaseAddon
    {
        [Constructable]
        public SpinningGlobeOfSosariaAddon()
        {
            AddComponent(new LocalizedAddonComponent(0x3660, 1076681), 1, 1, 0);
            //AddComponent(new LocalizedAddonComponent(0x3661, 1076681), 0, 1, 0);
            //AddComponent(new LocalizedAddonComponent(0x3662, 1076681), 1, 0, 0);
            //AddComponent(new LocalizedAddonComponent(0x3663, 1076681), 1, 1, 0);
        }

        public SpinningGlobeOfSosariaAddon(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed { get { return new SpinningGlobeOfSosariaDeed(); } }

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

    public class SpinningGlobeOfSosariaDeed : BaseAddonDeed
    {
        public override int LabelNumber { get { return 1076681; } } // Globe of Sosaria

        public override bool ExcludeDeedHue { get { return true; } }

        [Constructable]
        public SpinningGlobeOfSosariaDeed()
        {
            LootType = LootType.Blessed;
            Hue = 1266;
        }

        public SpinningGlobeOfSosariaDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon { get { return new SpinningGlobeOfSosariaAddon(); } }

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
