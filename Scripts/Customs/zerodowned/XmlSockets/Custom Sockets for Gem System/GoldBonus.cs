using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GoldBonus : BaseSocketAugmentation, IMythicAugment
    {

        [Constructable]
        public GoldBonus() : base(0x1F13)
        {
            Name = "Gold Bonus x2";
            Hue = 53;
        }

        public GoldBonus( Serial serial ) : base( serial )
		{
		}

        public override int SocketsRequired {get { return 1; } }

        public override int Icon {get { return 0x9a8; } } // gump ID

        public override bool UseGumpArt {get { return true; } }
        
        public override int IconXOffset { get { return 15;} }

        public override int IconYOffset { get { return 15;} }


        public override string OnIdentify(Mobile from)
        {
            return "Gold Bonus Loot x2";
        }

        public override bool OnAugment(Mobile from, object target)
        {
            if(target is BaseCreature)
            {
                ((BaseCreature)target).BonusGold += 2.0;
            }
            else
                return false;

            return true;
        }

        public override bool CanAugment(Mobile from, object target)
        {
            return (target is BaseCreature);
        }
        
        public override bool OnRecover(Mobile from, object target, int version)
        {
            if(target is BaseCreature)
            {
                ((BaseCreature)target).BonusGold -= 2.0;
            }
            else
                return false;

            return true;
        }

        public override bool CanRecover(Mobile from, object target, int version)
        {
            return true;
        }
        

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
    }
}
