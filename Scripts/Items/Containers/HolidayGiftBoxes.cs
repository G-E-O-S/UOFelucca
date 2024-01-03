using System;

namespace Server.Items
{
    public class GiftBoxHues
    {
        /* there's possibly a couple more, but this is what we could verify on OSI */
        private static readonly int[] m_NormalHues =
        {
            //0x672,
            //0x454,
            //0x507,
            //0x4ac,
            //0x504,
            //0x84b,
            //0x495,
            //0x97c,
            //0x493,
            //0x4a8,
            //0x494,
            //0x4aa,
            //0xb8b,
            //0x84f,
            //0x491,
            //0x851,
            //0x503,
            //0xb8c,
            //0x4ab,
            //0x84B,
            //0x438,
            //0x424,
            //0x433,
            //0x445,
            //0x42b,
            //0x448

                1150, 1266, 1153, 2763, 2764, 2765, 2766, 2767, 2770, 2771, 2772, 2773, 2774, 2776, 2777, 2778, 2780, 2781, 2782, 2783, 2784, 2786, 2788, 2789,
                2790, 2791, 2792, 2793, 2794, 2795, 2796, 2797, 2798, 2799, 2800, 2801, 2802, 2803, 2804, 2805, 2806, 2807, 2808, 2809, 2810, 2813, 2814, 2815, 2816,
                2817, 2818, 2819, 2820, 2822, 2823
        };
        private static readonly int[] m_NeonHues =
        {
            0x438,
            0x424,
            0x433,
            0x445,
            0x42b,
            0x448
        };
        public static int RandomGiftBoxHue
        {
            get
            {
                return m_NormalHues[Utility.Random(m_NormalHues.Length)];
            }
        }
        public static int RandomNeonBoxHue
        {
            get
            {
                return m_NeonHues[Utility.Random(m_NeonHues.Length)];
            }
        }
    }

    [FlipableAttribute(0x46A5, 0x46A6)]
    public class GiftBoxRectangle : BaseContainer
    {
        [Constructable]
        public GiftBoxRectangle()
            : base(Utility.RandomBool() ? 0x46A5 : 0x46A6)
        {
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        public GiftBoxRectangle(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultGumpID
        {
            get
            {
                return 0x11E;
            }
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

    public class GiftBoxCube : BaseContainer
    {
        [Constructable]
        public GiftBoxCube()
            : base(0x46A2)
        {
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        public GiftBoxCube(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultGumpID
        {
            get
            {
                return 0x11B;
            }
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

    public class GiftBoxCylinder : BaseContainer
    {
        [Constructable]
        public GiftBoxCylinder()
            : base(0x46A3)
        {
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        public GiftBoxCylinder(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultGumpID
        {
            get
            {
                return 0x11C;
            }
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

    public class GiftBoxOctogon : BaseContainer
    {
        [Constructable]
        public GiftBoxOctogon()
            : base(0x46A4)
        {
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        public GiftBoxOctogon(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultGumpID
        {
            get
            {
                return 0x11D;
            }
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

    public class GiftBoxAngel : BaseContainer
    {
        [Constructable]
        public GiftBoxAngel()
            : base(0x46A7)
        {
            this.Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        public GiftBoxAngel(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultGumpID
        {
            get
            {
                return 0x11F;
            }
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

    [Flipable(0x232A, 0x232B)]
    public class GiftBoxNeon : BaseContainer
    {
        [Constructable]
        public GiftBoxNeon()
            : base(Utility.RandomBool() ? 0x232A : 0x232B)
        {
            this.Hue = GiftBoxHues.RandomNeonBoxHue;
        }

        public GiftBoxNeon(Serial serial)
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
