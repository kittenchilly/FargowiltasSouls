using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Projectiles.MutantBoss
{
    public class MutantGuardian : ModProjectile
    {
        public override string Texture => "FargowiltasSouls/NPCs/Resprites/NPC_127";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeletron Prime");
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 70;
            projectile.height = 70;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = -1;
            cooldownSlot = 1;

            projectile.timeLeft = 300;
            projectile.hide = true;
            projectile.light = 0.5f;

            projectile.GetGlobalProjectile<FargoGlobalProjectile>().ImmuneToGuttedHeart = true;
        }

        public override bool CanHitPlayer(Player target)
        {
            return target.hurtCooldowns[1] == 0;
        }

        public override void AI()
        {
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;
                projectile.rotation = Main.rand.NextFloat(0, 2 * (float)Math.PI);
                projectile.hide = false;

                for (int i = 0; i < 30; i++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, 0, 0, 100, default(Color), 2f);
                    Main.dust[dust].noGravity = true;
                }
            }

            projectile.frame = 2;
            projectile.direction = projectile.velocity.X < 0 ? -1 : 1;
            projectile.rotation += projectile.direction * .3f;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (FargoSoulsWorld.MasochistMode)
            {
                target.GetModPlayer<FargoPlayer>().MaxLifeReduction += 100;
                target.AddBuff(mod.BuffType("OceanicMaul"), 5400);
                target.AddBuff(mod.BuffType("GodEater"), 420);
                target.AddBuff(mod.BuffType("FlamesoftheUniverse"), 420);
                target.AddBuff(mod.BuffType("MarkedforDeath"), 420);
                target.AddBuff(mod.BuffType("MutantFang"), 180);
            }
            target.AddBuff(mod.BuffType("Defenseless"), 480);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, 0, 0, 100, default(Color), 2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            int num156 = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type]; //ypos of lower right corner of sprite to draw
            int y3 = num156 * projectile.frame; //ypos of upper left corner of sprite to draw
            Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
            Vector2 origin2 = rectangle.Size() / 2f;
            Main.spriteBatch.Draw(texture2D13, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), projectile.GetAlpha(lightColor), projectile.rotation, origin2, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}

