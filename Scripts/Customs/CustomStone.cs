using Server;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    public class CustomStone : Item
    {
        [Constructable]
        public CustomStone() : base(0xED4)
        {
            Movable = true;
            Stackable = false;
            Hue = 0; // You can set the desired hue here
        }

        public CustomStone(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile)
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
            else
            {
                from.SendLocalizedMessage(501025); // You can't use that.
            }

            // Open the URL when the stone is double-clicked
            from.LaunchBrowser("http://www.uofelucca.com");
        }

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
