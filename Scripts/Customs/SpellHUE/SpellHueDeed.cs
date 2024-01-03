using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Gumps;

namespace Server.Items
{
    public class SpellHueDeed : Item
    {
        [Constructable]
        public SpellHueDeed() : base(0x46AE)
        {
            Name = "Spell HUE Deed";
            LootType = LootType.Newbied;
            //Hue = 1173;
        }

        public SpellHueDeed(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.IsChildOf(from.Backpack))
            {
                from.CloseGump(typeof(SpellHueGump));
                from.SendGump(new SpellHueGump(this));
                from.SendMessage("Choose a new hue for your spells.");
            }
            else
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
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
