using System;

namespace Server.Items
{
    public class LordBlackthornsExemplar : BaseShield
	{
		public override bool IsArtifact { get { return true; } }
        [Constructable]
        public LordBlackthornsExemplar()
            : base(0x1BC3)
        {
            Hue = 0x501;
        }

        public LordBlackthornsExemplar(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1079793;
            }
        }// Lord Blackthorn's Exemplar
        public override int ArtifactRarity
        {
            get
            {
                return 11;
            }
        }
        public override int BasePhysicalResistance
        {
            get
            {
                return 6;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 10;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 10;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 15;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 10;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 150;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 150;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
            {
                NegativeAttributes.NoRepair = 0;
            }
        }
    }
}
