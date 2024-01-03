
////////////////////////////////////////
//                                     //
//   Generated by CEO's YAAAG - Ver 2  //
// (Yet Another Arya Addon Generator)  //
//    Modified by Hammerhand for       //
//      SA & High Seas content         //
//                                     //
////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CaveEntranceEast_Addon : BaseAddon
    {
        private static int[,] m_AddOnSimpleComponents = new int[,] {
              {2272, 0, 2, 11}, {2275, 1, 2, 11}, {2281, 1, 1, 6}// 1	9	10	
			, {1354, 0, 0, 1}, {1356, 0, -1, 1}, {8550, 0, 1, 1}// 13	14	15	
			, {8551, 0, -2, 1}, {2272, -1, 2, 11}, {2280, -1, 1, 21}// 16	18	19	
			, {2280, -1, -2, 16}, {2280, -1, -1, 23}, {2280, -1, 0, 24}// 21	22	23	
					};



        public override BaseAddonDeed Deed
        {
            get
            {
                return new CaveEntranceEast_AddonDeed();
            }
        }

        [Constructable]
        public CaveEntranceEast_Addon()
        {

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent(new AddonComponent(m_AddOnSimpleComponents[i, 0]), m_AddOnSimpleComponents[i, 1], m_AddOnSimpleComponents[i, 2], m_AddOnSimpleComponents[i, 3]);


            AddComplexComponent((BaseAddon)this, 2274, 0, 2, 10, 0, -1, "Snow Pile", 1);// 2
            AddComplexComponent((BaseAddon)this, 2272, 0, -3, 7, 0, -1, "Snow Pile", 1);// 3
            AddComplexComponent((BaseAddon)this, 2278, 1, 1, 1, 0, -1, "Snow Pile", 1);// 4
            AddComplexComponent((BaseAddon)this, 2278, 0, 2, 1, 0, -1, "Snow Pile", 1);// 5
            AddComplexComponent((BaseAddon)this, 2278, 0, -3, 1, 0, -1, "Snow Pile", 1);// 6
            AddComplexComponent((BaseAddon)this, 2278, 1, -3, 1, 0, -1, "Snow Pile", 1);// 7
            AddComplexComponent((BaseAddon)this, 2278, 1, -2, 1, 0, -1, "Snow Pile", 1);// 8
            AddComplexComponent((BaseAddon)this, 2274, 0, 0, 1, 1, -1, "Snow Pile", 1);// 11
            AddComplexComponent((BaseAddon)this, 2274, 0, -1, 1, 1, -1, "Snow Pile", 1);// 12
            AddComplexComponent((BaseAddon)this, 2272, -1, 3, 1, 0, -1, "Snow Pile", 1);// 17
            AddComplexComponent((BaseAddon)this, 2274, -1, -3, 0, 0, -1, "Snow Pile", 1);// 20
            AddComplexComponent((BaseAddon)this, 2278, -1, 2, 1, 0, -1, "Snow Pile", 1);// 24
            AddComplexComponent((BaseAddon)this, 2274, -1, -1, 1, 1, -1, "Snow Pile", 1);// 25
            AddComplexComponent((BaseAddon)this, 2274, -1, 0, 1, 1, -1, "Snow Pile", 1);// 26

        }

        public CaveEntranceEast_Addon(Serial serial) : base(serial)
        {
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType)lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class CaveEntranceEast_AddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new CaveEntranceEast_Addon();
            }
        }

        [Constructable]
        public CaveEntranceEast_AddonDeed()
        {
            Name = "CaveEntranceEast_";
        }

        public CaveEntranceEast_AddonDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
