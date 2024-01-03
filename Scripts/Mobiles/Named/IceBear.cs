using System; 
using Server; 
using Server.Items; 

namespace Server.Mobiles 
{ 
   [CorpseName( "a ice bear corpse" )] 
   public class IceBear : BaseMount 
   { 
      [Constructable] 
      public IceBear() : this( "a ice bear" ) 
      { 
      } 

      [Constructable] 
      public IceBear ( string name ) : base( name, 0xD5, 0x3EC5, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
      { 
         Body = 213; 
         BaseSoundID = 0xA3;
	 Hue = 1152;

         SetStr( 455, 555 ); 
         SetDex( 300, 305 ); 
         SetInt( 200, 295 ); 

         SetHits( 450, 554 ); 
         SetMana( 50, 60 ); 

         SetDamage( 15, 25 ); 

         SetDamageType( ResistanceType.Physical, 100 ); 

         SetResistance( ResistanceType.Physical, 55, 55 ); 
         SetResistance( ResistanceType.Cold, 60, 80 ); 
         SetResistance( ResistanceType.Poison, 45, 50 ); 
         SetResistance( ResistanceType.Energy, 40, 50 ); 

         SetSkill( SkillName.MagicResist, 100.1, 120.0 ); 
         SetSkill( SkillName.Tactics, 99.1, 110.0 ); 
         SetSkill( SkillName.Wrestling, 112.1, 125.0 ); 

         Fame = 1500; 
         Karma = 0; 

         VirtualArmor = 18; 

         Tamable = false; 
         ControlSlots = 2; 
         MinTameSkill = 98.1;
      } 

      public override int Meat{ get{ return 2; } } 
      public override int Hides{ get{ return 16; } } 
      public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } } 
      public override PackInstinct PackInstinct{ get{ return PackInstinct.Bear; } } 

      public IceBear( Serial serial ) : base( serial ) 
      { 
      } 

      public override void Serialize( GenericWriter writer ) 
      { 
         base.Serialize( writer ); 

         writer.Write( (int) 0 ); 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
         base.Deserialize( reader ); 

         int version = reader.ReadInt(); 
      } 
   } 
}
