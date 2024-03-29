using Server.Gumps;
using Server.Network;

namespace Server.Engines.BulkOrders
{
    public class SmallMobileBODAcceptGump : Gump
    {
        private readonly SmallMobileBOD m_Deed;
        private readonly Mobile m_From;

        public SmallMobileBODAcceptGump(Mobile from, SmallMobileBOD deed) : base(50, 50)
        {
            m_From = from;
            m_Deed = deed;

            m_From.CloseGump(typeof(LargeBODAcceptGump));
            m_From.CloseGump(typeof(SmallMobileBODAcceptGump));

            AddPage(0);

            AddBackground(25, 10, 430, 264, 5054);

            AddImageTiled(33, 20, 413, 245, 2624);
            AddAlphaRegion(33, 20, 413, 245);

            AddImage(20, 5, 10460);
            AddImage(430, 5, 10460);
            AddImage(20, 249, 10460);
            AddImage(430, 249, 10460);

            AddHtmlLocalized(190, 25, 120, 20, 1045133, 0x7FFF, false, false); // A bulk order
            AddHtmlLocalized(40, 48, 350, 20, 1045135, 0x7FFF, false, false); // Ah!  Thanks for the goods!  Would you help me out?

            AddLabel(40, 72, 1152, @"Amount to tame:"); // Amount to make:
            AddLabel(250, 72, 1152, deed.AmountMax.ToString());

            AddHtmlLocalized(40, 96, 120, 20, 1045136, 0x7FFF, false, false); // Item requested:
            AddItem(385, 96, deed.Graphic);

            string s = deed.AnimalName;

            int capsbreak = s.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(), 1);

            if (capsbreak > -1)
            {
                string secondhalf = s.Substring(capsbreak);
                string firsthalf = s.Substring(0, capsbreak);

                string newname = firsthalf + " " + secondhalf;

                AddLabel(40, 120, 1152, newname.ToString());
            }
            else
            {
                AddLabel(40, 120, 1152, deed.AnimalName.ToString());
            }

            AddHtmlLocalized(40, 216, 350, 20, 1045139, 0x7FFF, false, false); // Do you want to accept this order?

            AddButton(100, 240, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(135, 240, 120, 20, 1006044, 0x7FFF, false, false); // Ok

            AddButton(275, 240, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(310, 240, 120, 20, 1011012, 0x7FFF, false, false); // CANCEL
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1) // Ok
            {
                if (m_From.PlaceInBackpack(m_Deed))
                {
                    m_From.SendLocalizedMessage(1045152); // The bulk order deed has been placed in your backpack.
                }
                else
                {
                    m_From.SendLocalizedMessage(1045150); // There is not enough room in your backpack for the deed.
                    m_Deed.Delete();
                }
            }
            else
            {
                m_Deed.Delete();
            }
        }

        public static int GetMaterialNumberFor(BulkMaterialType material)
        {
            if (material >= BulkMaterialType.DullCopper && material <= BulkMaterialType.Valorite)
                return 1045142 + (material - BulkMaterialType.DullCopper);
            else if (material >= BulkMaterialType.Spined && material <= BulkMaterialType.Barbed)
                return 1049348 + (material - BulkMaterialType.Spined);

            return 0;
        }
    }
}