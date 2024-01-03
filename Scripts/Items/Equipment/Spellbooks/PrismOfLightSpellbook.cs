using System;

namespace Server.Items
{
    public class PrismOfLightSpellbook : Spellbook
    {
        public override bool IsArtifact { get { return false; } }
        [Constructable]
        public PrismOfLightSpellbook()
            : base()
        {
            Hue = 2748;
            Name = "Prism of Light";
            //Attributes.SpellChanneling = 1;
            //Attributes.RegenMana = 3;
            //Attributes.Luck = 80;
            //Attributes.LowerRegCost = 15;
        }

        public PrismOfLightSpellbook(Serial serial)
            : base(serial)
        {
        }

        //public override int LabelNumber
        //{
        //    get
        //    {
        //        return 1073262;
        //    }
        //}// Clainin's Spellbook - Museum of Vesper Replica
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
