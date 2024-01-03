using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
    public class SpellHueGump2 : Gump
    {
        private SpellHueDeed2 m_Deed;

        private static List<int> m_HuesList = new List<int>()
        {
            88, 43, 5, 133,
            2955, 1266, 253, 2741,
			//1152, 2741, 2671, 2720, 1914, 1272, 
		};

        public SpellHueGump2(SpellHueDeed2 deed)
            : base(0, 0)
        {
            m_Deed = deed;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(137, 49, 754, 580, 39925);
            this.AddHtml(444, 73, 150, 30, $"<big><basefont color=#FFD700>Choose a spell color", false, false);

            int shift_X = 0;
            int shift_Y = 0;

            for (int i = 0; i < m_HuesList.Count; i++)
            {
                if (i > 0 && i % 4 == 0) // new line of spells each 7 draws
                {
                    if (i < 13) // shifts for first 14 elements
                    {
                        shift_X = 0;
                        shift_Y += 156;
                    }
                    else // shifts for drawing last line
                    {
                        shift_X = 60;
                        shift_Y += 156;
                    }
                }

                this.AddItem(208 + shift_X, 121 + shift_Y, 14049, m_HuesList[i]);
                this.AddImage(196 + shift_X, 151 + shift_Y, 7063, m_HuesList[i] - 1);
                this.AddButton(199 + shift_X, 231 + shift_Y, 238, 240, i + 1, GumpButtonType.Reply, 0);

                shift_X += 94;
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            var m = sender.Mobile;

            if (m_Deed.Deleted)
                return;
            else if (!m_Deed.IsChildOf(m.Backpack))
            {
                m.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
                return;
            }

            if (info.ButtonID != 0)
            {
                int hue = m_HuesList[info.ButtonID - 1];
                m.SpellHue = hue - 1;  // -1 to correct showing the animation spell hue.
                m.SendMessage($"You have taken {hue} hue for your spells.");
                m_Deed.Delete();
            }
        }

    }
}
