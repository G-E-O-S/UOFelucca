using System;

namespace Server.Items
{
    [Flipable(0x232A, 0x232B)]
    public class WinterGiftPackage2023 : GiftBox
    {
        [Constructable]
        public WinterGiftPackage2023()
        {
            this.Name = "Merry Christmas!";
            this.Hue = GiftBoxHues.RandomNeonBoxHue;
            this.DropItem(new SnowyMan());
            this.DropItem(new WreathDeed());
            this.DropItem(new BlueSnowflake());
            this.DropItem(new RedPoinsettia());
            this.DropItem(new ChristmasAngel());
            this.DropItem(new ChristmasCard());
            this.DropItem(new ChristmasSpiritCrystal());
            this.DropItem(new FancyWreath2023());
            this.DropItem(new Wreath2023());
            this.DropItem(new ReindeerStatues());
            this.DropItem(new Nutcracker2023());
            this.DropItem(new GreenStocking2023());
            this.DropItem(new RedStocking2023());
        }

        public WinterGiftPackage2023(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
