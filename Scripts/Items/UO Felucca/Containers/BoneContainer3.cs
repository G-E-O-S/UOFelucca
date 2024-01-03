using System;

namespace Server.Items
{
    [Furniture]
    [Flipable(0x0ECA)]
    public class BoneContainer3 : BaseContainer
    {

        [Constructable]
        public BoneContainer3()
            : this(0x0ECA)
        {
        }

        [Constructable]
        public BoneContainer3(int hue)
            : base(0x0ECA)
        {
            this.Weight = 2.0;
            this.Hue = Utility.RandomList(1150, 1161, 1194, 1175, 0, 1266);
            this.Name = "Bones";
            this.ItemID = 3790;
            this.GumpID = 9;
        }

        public BoneContainer3(Serial serial)
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
