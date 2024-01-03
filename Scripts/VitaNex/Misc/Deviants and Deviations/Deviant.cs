#region Header
//               _,-'/-'/
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2023  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #                                       #
#endregion

namespace Server.Mobiles
{
	public class Deviant : BaseDeviant
	{
		[Constructable]
		public Deviant()
			: base(AIType.AI_Melee, FightMode.Closest, 16, 1, 0.1, 0.2)
		{
			Name = "Voxpire"; // default Incognito
        }

		public Deviant(Serial serial)
			: base(serial)
		{ }

		protected override int InitBody()
		{
			return Race.Body(this);
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
