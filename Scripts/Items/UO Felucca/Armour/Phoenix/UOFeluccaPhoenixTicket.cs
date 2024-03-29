using System;
using Server.Network;

namespace Server.Items
{

    public class UOFeluccaPhoenixTicket : Item
    {
        [Constructable]
        public UOFeluccaPhoenixTicket() : base(0x14F0)
        {
            LootType = LootType.Blessed;
            Hue = 1359;
        }

        public UOFeluccaPhoenixTicket(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041234;
            }
        }// Ticket for a piece of phoenix armor

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
            }
            else
            {
                switch (Utility.Random(6))
                {
                    case 0: from.AddToBackpack(new UOFeluccaPhoenixTunic()); break;
                    case 1: from.AddToBackpack(new UOFeluccaPhoenixSleeves()); break;
                    case 2: from.AddToBackpack(new UOFeluccaPhoenixGorget()); break;
                    case 3: from.AddToBackpack(new UOFeluccaPhoenixGloves()); break;
                    case 4: from.AddToBackpack(new UOFeluccaPhoenixLegs()); break;
                    case 5: from.AddToBackpack(new UOFeluccaPhoenixHelm()); break;
                }
                this.Delete();
                from.SendLocalizedMessage(502064); // A piece of phoenix armor has been placed in your backpack.
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
