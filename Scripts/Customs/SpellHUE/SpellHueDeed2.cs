using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Gumps;

namespace Server.Items
{
    public class SpellHueDeed2 : Item
    {
        [Constructable]
        public SpellHueDeed2() : base(0x46B2)
        {
            Name = "Spell HUE Deed";
            LootType = LootType.Newbied;
            //Hue = 1173;
        }

        public SpellHueDeed2(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.IsChildOf(from.Backpack))
            {
                from.CloseGump(typeof(SpellHueGump2));
                from.SendGump(new SpellHueGump2(this));
                from.SendMessage("Choose a new hue for your spells.");
            }
            else
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            {
                list.Add("Donation Store Spellhue Ticket: 8 Colors"); // recovered from a shipwreck                    
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
