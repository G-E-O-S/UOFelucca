using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public class TrammyBarstool : BaseHat, IRepairable
    {
        public CraftSystem RepairSystem { get { return DefTailoring.CraftSystem; } }

        public override int BasePhysicalResistance
        {
            get
            {
                return 2;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 6;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 8;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 1;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 7;
            }
        }

        public override int InitMinHits
        {
            get
            {
                return 20;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 30;
            }
        }
        [Constructable]
        public TrammyBarstool()
        : this(0)
        {
        }

        [Constructable]
        public TrammyBarstool(int hue)
            : base(0x1547, hue)
        {
            this.Name = "Trammy Barstool";
            this.Hue = Utility.RandomList(0, 1175, 1910, 1194, 1161, 1150, 33, 1266, 1153, 2068, 2721, 2758, 1258);
        }

        public override bool Dye(Mobile from, DyeTub sender)
        {
            from.SendLocalizedMessage(sender.FailMessage);
            return false;
        }

        public TrammyBarstool(Serial serial)
            : base(serial)
        {
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
