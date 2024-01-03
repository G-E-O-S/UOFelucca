#region Header
//   Vorspire    _,-'/-'/  KillResetToken.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Drawing;

using Server;
#endregion

namespace VitaNex.Items
{
    public class KillResetToken : Item
    {
        public override bool DisplayLootType { get { return false; } }

        [Constructable]
        public KillResetToken()
            : base(0x2AAA)
        {
            Name = "Kill Reset Token";
            Weight = 1.0;
            Hue = 34;
            Stackable = true;
            LootType = LootType.Blessed;
        }

        public KillResetToken(Serial serial)
            : base(serial)
        { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("Use: Resets your recorded kills, returning your innocent status".WrapUOHtmlColor(Color.LawnGreen));
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (!this.CheckDoubleClick(m, true, false, 2, true, false, false))
            {
                return;
            }

            if (m.Kills <= 0)
            {
                m.SendMessage("You don't have any recorded kills.");
                return;
            }

            m.Kills = 0;
            m.SendMessage("Your recorded kills have been wiped, you are now innocent.");

            Consume();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }
}
