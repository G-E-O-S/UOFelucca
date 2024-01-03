using Server;
using System;

namespace Server.Items
{
    public class GargishEpauletteBearingTheCrestOfBlackthorn1 : Cloak
    {
        public override bool IsArtifact { get { return true; } }
        public override Race RequiredRace { get { return Race.Human; } }
        public override bool CanBeWornByGargoyles { get { return true; } }

        public override int LabelNumber { get { return 1123326; } } // Gargish Epaulette

        [Constructable]
        public GargishEpauletteBearingTheCrestOfBlackthorn1()
        {
            ReforgedSuffix = ReforgedSuffix.Blackthorn;
            ItemID = 0x9986;
            Attributes.BonusHits = 3;
            Attributes.BonusInt = 5;
            Hue = 2075;
			
			Layer = Layer.OuterTorso;
        }

        public GargishEpauletteBearingTheCrestOfBlackthorn1(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
			
			if (version == 0)
            {
                MaxHitPoints = 0;
                HitPoints = 0;

                if (Layer != Layer.OuterTorso)
                {
                    if (Parent is Mobile)
                    {
                        ((Mobile)Parent).AddToBackpack(this);
                    }

                    Layer = Layer.OuterTorso;
                }
            }
        }
    }
}
