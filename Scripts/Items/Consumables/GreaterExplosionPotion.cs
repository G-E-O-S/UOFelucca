using System;

namespace Server.Items
{
    public class GreaterExplosionPotion : BaseExplosionPotion
    {
        [Constructable]
        public GreaterExplosionPotion()
            : base(PotionEffect.ExplosionGreater)
        {
            Stackable = false;
        }

        public GreaterExplosionPotion(Serial serial)
            : base(serial)
        {
        }

        public override int MinDamage
        {
            get
            {
                return 13;
            }
        }
        public override int MaxDamage
        {
            get
            {
                return 15;
            }
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
