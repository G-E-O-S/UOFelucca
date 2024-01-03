using Server;
using Server.Items;

namespace YourNamespace
{
    public class NewbiedDagger : Dagger
    {
        [Constructable]
        public NewbiedDagger()
            : base()
        {
            LootType = LootType.Newbied; // Set the loot type to Newbied
            UsesRemaining = 5000;
        }

        public NewbiedDagger(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
