using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Prompts;

namespace Server.Gumps
{
	public class ButlerRevokeGump : Gump
    {
		private readonly Butler m_Butler;
		private readonly Mobile m_From;
		
		private int m_Page;
		
		public ButlerRevokeGump(Mobile from, Butler butler, int page)
			: base(50, 40)
		{
			m_Butler = butler;
			m_From = from;
			m_Page = page;
			
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			
			AddPage(0);
			AddBackground(0, 0, 300, 440, 9200);
			AddLabel(110, 30, 0, @"Revoke Access");
			
			LoadPage();
		}
		
		public override void OnResponse(NetState sender, RelayInfo info)
        {
			if( info.ButtonID == 0 
				|| m_Butler == null 
				|| m_Butler.Deleted
				|| m_Butler.m_ButlerDeed == null 
				|| m_Butler.m_ButlerDeed.Deleted ) 
			{
				return;
			}
			
			if( info.ButtonID == 1 )
			{
				m_From.SendGump(new ButlerRevokeGump(m_From, m_Butler, m_Page - 1));
			}
			else if( info.ButtonID == 2 )
			{
				m_From.SendGump(new ButlerRevokeGump(m_From, m_Butler, m_Page + 1));
			}
			else if( info.ButtonID > 2 ) 
			{
				m_Butler.Friends.Remove(m_Butler.Friends[info.ButtonID - 3]);
				
				if (m_Butler.Friends.Count > 0)
				{
					m_From.SendGump(new ButlerRevokeGump(m_From, m_Butler, 1));
				}
			}
		}
		
		private void LoadPage()
		{
			int startIndex = 0;			
			if(m_Page > 1)
			{
				startIndex = (10 * (m_Page - 1));
			}
			
			bool hasNextPage = false;
			int maxIndex = startIndex + 10;
			if(maxIndex >= m_Butler.Friends.Count)
			{
				maxIndex = m_Butler.Friends.Count;
			}
			else
			{
				hasNextPage = true;
			}
			
			int xIndex = 0;
			for(int index = startIndex; index < maxIndex; index++)
			{
				AddButton(35, 75 + (xIndex * 30), 4017, 4018, index + 3, GumpButtonType.Reply, 0);
				AddLabel(80, 75 + (xIndex * 30), 0, ((Mobile)m_Butler.Friends[index]).Name);
				xIndex++;
			}
			
			if(m_Page > 1)
			{
				AddButton(35, 390, 4014, 4015, 1, GumpButtonType.Reply, 0);
			}
			
			if(hasNextPage)
			{
				AddButton(230, 390, 4005, 4006, 2, GumpButtonType.Reply, 0);
			}
		}
	}
}