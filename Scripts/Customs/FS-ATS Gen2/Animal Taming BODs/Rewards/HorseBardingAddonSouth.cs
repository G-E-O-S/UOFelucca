namespace Server.Items
{
    public class BardedHorseSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed => new BardedHorseSouthDeed();

        [Constructable]
        public BardedHorseSouthAddon()
        {
            AddComponent(new AddonComponent(0x1376), 0, 0, 0);
            AddComponent(new AddonComponent(0x1377), 1, 0, 0);
        }

        public BardedHorseSouthAddon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BardedHorseSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon => new BardedHorseSouthAddon();

        [Constructable]
        public BardedHorseSouthDeed()
        {
            Name = "horse barding addon deed [south]";
        }

        public BardedHorseSouthDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}