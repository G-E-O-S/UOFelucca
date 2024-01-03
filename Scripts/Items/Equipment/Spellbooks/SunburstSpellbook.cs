using System;

namespace Server.Items
{
    public class SunburstSpellbook : Spellbook
    {
        public override bool IsArtifact { get { return false; } }
        [Constructable]
        public SunburstSpellbook()
            : base()
        {
            Hue = 2736;
            Name = "The Exodus";
            //Attributes.SpellChanneling = 1;
            //Attributes.RegenMana = 3;
            //Attributes.Luck = 80;
            //Attributes.LowerRegCost = 15;
        }

        public SunburstSpellbook(Serial serial)
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
