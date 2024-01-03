using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Collections;
using Server.Gumps;
using System.Text;
using Server.Targeting;
using Server.Commands;
using Server.Commands.Generic;
using Server.Engines.XmlSpawner2;

//////////////////////////
// SocketDeedPlusOne
// SocketDeedPlusTwo
// SocketDeedPlusThree
// SocketDeedPlusFour
// SocketDeedPlusFive
//////////////////////////

namespace Server.Items
{
	public class SocketDeedPlusOne : BaseSocketDeed
	{
		public override int Sockets { get { return 1; } }
		
		[Constructable]
		public SocketDeedPlusOne() : base() {}
		
		public SocketDeedPlusOne( Serial serial ) : base( serial ) {}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class SocketDeedPlusTwo : BaseSocketDeed
	{
		public override int Sockets { get { return 2; } }
		
		[Constructable]
		public SocketDeedPlusTwo() : base() {}
		
		public SocketDeedPlusTwo( Serial serial ) : base( serial ) {}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class SocketDeedPlusThree : BaseSocketDeed
	{
		public override int Sockets { get { return 3; } }
		
		[Constructable]
		public SocketDeedPlusThree() : base() {}
		
		public SocketDeedPlusThree( Serial serial ) : base( serial ) {}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class SocketDeedPlusFour : BaseSocketDeed
	{
		public override int Sockets { get { return 4; } }
		
		[Constructable]
		public SocketDeedPlusFour() : base() {}
		
		public SocketDeedPlusFour( Serial serial ) : base( serial ) {}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class SocketDeedPlusFive : BaseSocketDeed
	{
		public override int Sockets { get { return 5; } }
		
		[Constructable]
		public SocketDeedPlusFive() : base() {}
		
		public SocketDeedPlusFive( Serial serial ) : base( serial ) {}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				 from.SendLocalizedMessage( 1042001 );
			}
			else
			{
				from.SendMessage( "Select the item to add sockets" ); 
				from.Target = new SocketDeed_AddSocketToTarget( this, Sockets );
			 }
		}	
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public abstract class BaseSocketDeed : Item
	{
		public virtual int Sockets { get { return 0; } }
		
		public BaseSocketDeed(): base()
        {
			ItemID = 0x2831; 
			Weight = 1.0;
			Hue = 1161;
			Name = String.Format("Pet Socket Deed [+{0}]", Sockets  );
			LootType = LootType.Regular;
        }
		
		public BaseSocketDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override bool DisplayLootType{ get{ return true; } }
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				 from.SendLocalizedMessage( 1042001 );
			}
			else
			{
				from.SendMessage( "Select the item to add sockets" ); 
				from.Target = new SocketDeed_AddSocketToTarget( this, Sockets );
			 }
		}	

		public class SocketDeed_AddSocketToTarget : Target
		{
			private BaseSocketDeed _deed;
			private int m_extrasockets;

			public SocketDeed_AddSocketToTarget( BaseSocketDeed deed, int extrasockets ) :  base ( 30, false, TargetFlags.None )
			{
				_deed = deed;
				m_extrasockets = extrasockets;
			}
			protected override void OnTarget( Mobile from, object targeted )
			{
				if(from == null || targeted == null) return;

				// BaseShield uses BaseArmor
				if( targeted is BaseArmor || targeted is BaseWeapon || targeted is BaseJewel || targeted is BaseCreature )
				{
					if( targeted is BaseCreature )
					{
						BaseCreature bc = (BaseCreature)targeted;
						
						if( bc.ControlMaster != from )
						{
							from.SendMessage("That is not your pet!");
							return;
						}
					
					}

					int maxSockets = XmlSockets.DefaultMaxSockets;
					double difficulty = 0.00; //DefaultSocketDifficulty;
					SkillName skillname = SkillName.Meditation; // DefaultSocketSkill;
					double difficulty2 = 0.00; //DefaultSocketDifficulty;
					SkillName skillname2 = SkillName.Meditation; //DefaultSocketSkill;
					Type resource = null; //DefaultSocketResource;
					int quantity = 0; //DefaultSocketResourceQuantity;
					
					int nSockets = 0;
					
					XmlSockets s = XmlAttach.FindAttachment(targeted,typeof(XmlSockets)) as XmlSockets;
					
					if(s != null)
					{
						// find out how many sockets it has
						nSockets = s.NSockets;
					}

					// already full, no more sockets allowed
					if( (nSockets + m_extrasockets - 1) >= maxSockets && maxSockets >= 0)
					{
						from.SendMessage("You cannot add that many sockets to the item! It already has {0} of max {1} sockets.", nSockets, maxSockets );
						return;
					}
					
					if(nSockets >= maxSockets && maxSockets >= 0)
					{ 
						from.SendMessage("This cannot be socketed any further.");
						return;
					}

					from.PlaySound( 0x2A ); // play anvil sound

					from.SendMessage("You have successfully added a socket the target!");
					
					if(s != null)
						{
							// add an empty socket
							for( int i = 0; i <= ( m_extrasockets - 1 ); i++)
								s.SocketOccupants.Add(null);

						} 
					else
						{
							// add an xmlsockets attachment with the new socket
							s = new XmlSockets(m_extrasockets);
							XmlAttach.AttachTo(targeted, s);
						}

					if(s != null) 
					{
							s.InvalidateParentProperties();
					}
						from.SendGump(new XmlSockets.SocketsGump(from, s));

					_deed.Delete();
				}
				else
				{
					from.SendMessage("This cannot be socketed.");
				}
			}
		}
		
	}
}
