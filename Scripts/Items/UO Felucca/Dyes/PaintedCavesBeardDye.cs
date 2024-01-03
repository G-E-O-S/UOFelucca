using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
    public class PaintedCavesBeardDye : Item
    {
        [Constructable]
        public PaintedCavesBeardDye()
            : base(0x0E27)
        {
            Hue = 1163;
            LootType = LootType.Regular;
            Name = "Abyssal Beard Dye";
        }

        public PaintedCavesBeardDye(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (IsChildOf(m.Backpack))
            {
                ApplyDyeToFacialHair(m);
            }
            else
            {
                m.SendLocalizedMessage(1042010); // You must have the object in your backpack to use it.
            }
        }

        private void ApplyDyeToFacialHair(Mobile m)
        {
            PlayerMobile player = m as PlayerMobile;
            if (player != null)
            {
                // Check if the player has facial hair (beard or mustache)
                if (player.FacialHairItemID > 0)
                {
                    player.FacialHairHue = Hue;
                    player.SendMessage("You have dyed your beard.");
                    this.Delete();
                }
                else
                {
                    m.SendMessage("You don't have a beard.");
                }
            }
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
