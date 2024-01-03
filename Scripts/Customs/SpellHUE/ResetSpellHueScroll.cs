using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ResetSpellHueScroll : Item
    {


        [Constructable]
        public ResetSpellHueScroll() : base(0x14F0)
        {
            Name = "spell hue removal scroll";
            Hue = 0;
            LootType = LootType.Blessed;
        }

        public ResetSpellHueScroll(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            var player = from as PlayerMobile;

            if (player != null)
            {
                player.SpellHue = Hue;
                player.SendMessage("Your spell hue has been reset");
                Delete();
            }
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
