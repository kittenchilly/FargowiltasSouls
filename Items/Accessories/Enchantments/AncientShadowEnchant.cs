﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class AncientShadowEnchant : SoulsItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shadow Enchantment");
            Tooltip.SetDefault(
@"Your attacks may inflict Darkness on enemies
Darkened enemies occasionally fire shadowflame tentacles at other enemies
Three Shadow Orbs will orbit around you
'Archaic, yet functional'");
        }

        public override void SafeModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(94, 85, 220);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = ItemRarityID.Pink;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.AncientShadowEffect();
            modPlayer.ShadowEffect(hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.AncientShadowHelmet);
            recipe.AddIngredient(ItemID.AncientShadowScalemail);
            recipe.AddIngredient(ItemID.AncientShadowGreaves);
            //recipe.AddIngredient(ItemID.AncientNecroHelmet);
            //recipe.AddIngredient(ItemID.AncientGoldHelmet);
            recipe.AddIngredient(null, "ShadowEnchant");
            recipe.AddIngredient(ItemID.ShadowFlameKnife);
            recipe.AddIngredient(ItemID.ShadowFlameHexDoll);
            //dart rifle
            //toxicarp

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}