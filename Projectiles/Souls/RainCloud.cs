﻿using FargowiltasSouls.Projectiles.Souls;
using FargowiltasSouls.Toggler;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.Souls
{
    public class RainCloud : ModProjectile
    {
        private int shrinkTimer = 0;
        public override string Texture => "Terraria/Projectile_238";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain Cloud");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.RainCloudRaining);
            aiType = ProjectileID.RainCloudRaining;
            projectile.GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;

            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();

            projectile.timeLeft++;

            //destroy duplicates if they somehow spawn
            if (player.ownedProjectileCounts[projectile.type] > 1
                || (projectile.owner == Main.myPlayer && (!player.GetToggleValue("Rain") || !modPlayer.RainEnchant)))
            {
                projectile.Kill();
            }

            //follow player
            float dist = Vector2.Distance(projectile.Center, player.Center);

            if (dist > 200)
            {
                Vector2 velocity = Vector2.Normalize(player.Center - projectile.Center) * player.velocity.Length();
                projectile.position += velocity;
            }
            else
            {
                projectile.velocity.Y = 0;
            }

            //always max size
            if (modPlayer.WizardEnchant || modPlayer.NatureForce)
            {
                projectile.scale = 3f;
            }

            //absorb projectiles
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.active && proj.friendly && !proj.hostile && proj.owner == player.whoAmI && proj.damage > 0 && !proj.minion
                    && proj.type != projectile.type && proj.type != ProjectileID.RainFriendly && proj.whoAmI != Main.player[proj.owner].heldProj
                    && Array.IndexOf(FargoGlobalProjectile.noSplit, projectile.type) <= -1 && proj.type != ModContent.ProjectileType<Chlorofuck>() && proj.Hitbox.Intersects(projectile.Hitbox))
                {
                    if (projectile.scale < 3f)
                    {
                        projectile.scale *= 1.1f;
                    }
                    else
                    {
                        Vector2 rotationVector2 = (proj.Center + proj.velocity * 25) - projectile.Center;
                        rotationVector2.Normalize();

                        Vector2 vector2_3 = rotationVector2 * 8f;
                        float ai_1 = Main.rand.Next(80);
                        Projectile.NewProjectile(projectile.Center.X + vector2_3.X * 5, projectile.Center.Y + vector2_3.Y * 5, vector2_3.X, vector2_3.Y,
                            mod.ProjectileType("LightningArc"), proj.damage * 3, projectile.knockBack, projectile.owner,
                            rotationVector2.ToRotation(), ai_1);

                    }

                    proj.active = false;
                    shrinkTimer = 120;

                    break;
                }
            }

            //shrink over time if no projectiles absorbed
            if (shrinkTimer > 0 && projectile.scale > 1f)
            {
                shrinkTimer--;

                if (shrinkTimer == 0)
                {
                    projectile.scale *= 0.9f;
                    shrinkTimer = 10;
                }
            }

            //cancel normal rain
            projectile.ai[0] = 0;

            projectile.localAI[1]++;

            //bigger = more rain
            if (projectile.scale > 3f)
            {
                projectile.localAI[1] += 4;
            }
            else if (projectile.scale > 2f)
            {
                projectile.localAI[1] += 3;
            }
            else if (projectile.scale > 1.5f)
            {
                projectile.localAI[1] += 2;
            }
            else
            {
                projectile.localAI[1]++;
            }

            //do the rain
            if (projectile.localAI[1] >= 8)
            {
                projectile.localAI[1] = 0;

                int num414 = (int)(projectile.Center.X + (float)Main.rand.Next((int)(-20 * projectile.scale), (int)(20 * projectile.scale)));
                int num415 = (int)(projectile.position.Y + (float)projectile.height + 4f);
                int p = Projectile.NewProjectile((float)num414, (float)num415, 0f, 5f, ProjectileID.RainFriendly, projectile.damage / 4, 0f, projectile.owner, 0f, 0f);
                Main.projectile[p].penetrate = 1;
            }
        }
    }
}