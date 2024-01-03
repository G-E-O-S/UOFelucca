using System;

namespace Server.Items
{
    [Flipable(0xA6E7, 0xA6E8)]
    public class Gnome : Item
    {
        [Constructable]
        public Gnome()
            : base(0xA6E7)
        {
            Name = "Gnome";
            Weight = 1;
        }

        public Gnome(Serial serial)
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
