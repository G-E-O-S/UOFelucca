using System;

namespace Server.Items
{
    public class ChristmasCloak : Cloak
    {
        [Constructable]
        public ChristmasCloak()
        {
            Name = "Merry Christmas!";
            Hue = Utility.RandomList(1150, 1266, 1153, 2763, 2764, 2765, 2766, 2767, 2770, 2771, 2772, 2773, 2774, 2776, 2777, 2778, 2780, 2781, 2782, 2783, 2784, 2786, 2788, 2789,
                2790, 2791, 2792, 2793, 2794, 2795, 2796, 2797, 2798, 2799, 2800, 2801, 2802, 2803, 2804, 2805, 2806, 2807, 2808, 2809, 2810, 2813, 2814, 2815, 2816,
                2817, 2818, 2819, 2820, 2822, 2823);
        }

        public ChristmasCloak(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }
}
