using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;
using Server.Engines.XmlSpawner2;

namespace Server.ContextMenus
{
	public class SocketInfoEntry : ContextMenuEntry
	{
		private Item m_Item;
		private Mobile m_From;
		
        //3000362 = Open
        //context menu auto adds 3000000 to this number
        public SocketInfoEntry( Mobile from, Item item ) : base( 362, 3 )  
		{
			m_From = from;
			m_Item = item;
		}

		public override void OnClick()
		{
            XmlSockets socket = (XmlSockets)XmlAttach.FindAttachment(m_Item, typeof(XmlSockets));


			Owner.From.CloseGump( typeof( XmlSockets.SocketsGump ) );
			Owner.From.SendGump( new XmlSockets.SocketsGump( m_From, socket ) ); 
		}
	}
}