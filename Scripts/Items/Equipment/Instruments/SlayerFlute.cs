using Server.Mobiles;
using Server.Network;
using System;

namespace Server.Items
{
    public class SlayerFlute : BaseInstrument
    {
               
        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
            
            this.LabelTo(from, UsesRemaining.ToString() + " Uses Remaining");            
        }

        [Constructable]
        public SlayerFlute()
            : base(0x2805, 0x504, 0x503)
        {             
            this.Name = "Slayer Flute";
            this.Weight = 1.0;
            this.Slayer = Utility.RandomList(SlayerName.Silver, SlayerName.DragonSlaying, SlayerName.DaemonDismissal, SlayerName.BalronDamnation, SlayerName.TrollSlaughter,
                    SlayerName.OgreTrashing, SlayerName.GargoylesFoe, SlayerName.LizardmanSlaughter, SlayerName.BloodDrinking, SlayerName.Fey, SlayerName.OrcSlaying, SlayerName.SnakesBane,
                    SlayerName.SpidersDeath, SlayerName.ScorpionsBane, SlayerName.FlameDousing, SlayerName.WaterDissipation, SlayerName.Vacuum, SlayerName.EarthShatter, SlayerName.BloodDrinking,
                    SlayerName.SummerWind, SlayerName.ElementalHealth, SlayerName.Exorcism, SlayerName.Repond, SlayerName.ReptilianDeath, SlayerName.ArachnidDoom, SlayerName.ElementalBan, SlayerName.Terathan,
                    SlayerName.Ophidian);
        }

        public SlayerFlute(Serial serial)
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

            if (this.Weight == 3.0)
                this.Weight = 1.0;
        }
    }
}
