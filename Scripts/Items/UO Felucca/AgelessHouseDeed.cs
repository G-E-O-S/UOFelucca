using System;
using System.Drawing;

using Server.Targeting;
using Server.Multis;

namespace Server.Items
{
    public class AgelessHouseTarget : Target
    {
        private AgelessHouseDeed m_Deed;

        public AgelessHouseTarget(AgelessHouseDeed deed)
            : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Deed.Deleted || m_Deed.RootParent != from)
                return;

            if (target is HouseSign sign && sign.Owner?.IsOwner(from) == true)
            {
                if (sign.RestrictDecay)
                {
                    from.SendMessage("This house is already ageless");
                }
                else
                {
                    sign.RestrictDecay = true;

                    from.SendMessage("The house is now ageless.");

                    m_Deed.Delete(); // Delete the ageless house deed
                }
            }
            else
            {
                from.SendMessage("You must target the sign of the house this character owns!");
            }
        }
    }

    public class AgelessHouseDeed : Item
    {
        [Constructable]
        public AgelessHouseDeed()
            : base(0x2258)
        {
            Weight = 1.0;
            Hue = 1161;
            LootType = LootType.Blessed;
            Name = "Ageless House Deed";
        }

        public AgelessHouseDeed(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("Use: [Target Your House Sign] Restricts Decay of 1 House Forever".WrapUOHtmlColor(Color.SkyBlue));
        }

        public override void OnDoubleClick(Mobile from) // Override double click of the deed to call our target
        {
            if (!IsChildOf(from.Backpack)) // Make sure its in their pack
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else
            {
                from.SendMessage("Which house would you like to make ageless? Target the sign.");
                from.Target = new AgelessHouseTarget(this); // Call our target
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

            LootType = LootType.Blessed;

            _ = reader.ReadInt();
        }
    }
}
