﻿using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria.UI;
using Terraria;

namespace FargowiltasSouls.UI
{
	// From ExampleMod
	public class UIDragablePanel : UIPanel
	{
		// Stores the offset from the top left of the UIPanel while dragging.
		private Vector2 offset;
		public bool dragging;
		public UIElement[] ExtraChildren;

		public UIDragablePanel() { }
		public UIDragablePanel(params UIElement[] countMeAsChildren)
        {
			ExtraChildren = countMeAsChildren;
        }

		public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);

			bool upperMost = true;
			IEnumerable<UIElement> children = Elements.Concat(ExtraChildren);
			foreach (UIElement element in children)
            {
				if (element.ContainsPoint(evt.MousePosition) && element as UIPanel == null)
                {
					upperMost = false;
					break;
                }
            }

			if (upperMost)
				DragStart(evt);
		}

		public override void MouseUp(UIMouseEvent evt)
		{
			base.MouseUp(evt);
			if (dragging)
				DragEnd(evt);
		}

		private void DragStart(UIMouseEvent evt)
		{
			offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
			dragging = true;
		}

		private void DragEnd(UIMouseEvent evt)
		{
			Vector2 end = evt.MousePosition;
			string top = GetDimensions().Position().ToString();
			dragging = false;

			Left.Set(end.X - offset.X, 0f);
			Top.Set(end.Y - offset.Y, 0f);

			Recalculate();
			top = GetDimensions().Position().ToString();
			int brdeak = 0;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime); // don't remove.

			// Checking ContainsPoint and then setting mouseInterface to true is very common. This causes clicks on this UIElement to not cause the player to use current items. 
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}

			if (dragging)
			{
				Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same.
				Top.Set(Main.mouseY - offset.Y, 0f);
				Recalculate();
			}

			// Here we check if the DragableUIPanel is outside the Parent UIElement rectangle. 
			var parentSpace = Parent.GetDimensions().ToRectangle();
			if (!GetDimensions().ToRectangle().Intersects(parentSpace))
			{
				Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
				Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
				// Recalculate forces the UI system to do the positioning math again.
				Recalculate();
			}
		}
	}
}
