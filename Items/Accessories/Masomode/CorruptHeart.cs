using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Masomode
{
    public class CorruptHeart : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrupt Heart");
            Tooltip.SetDefault(@"'Flies refuse to approach it'
Grants immunity to Rotting
10% increased movement speed
You spawn mini eaters to seek out enemies every few attacks");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 3;
            item.value = Item.sellPrice(0, 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            player.buffImmune[mod.BuffType("Rotting")] = true;
            player.moveSpeed += 0.1f;
            modPlayer.CorruptHeart = true;
            if (modPlayer.CorruptHeartCD > 0)
                modPlayer.CorruptHeartCD--;
        }
    }
}