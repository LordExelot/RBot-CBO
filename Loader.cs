using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RBot;
using RBot.Options;
using RBot.Plugins;

namespace CoreBotsOptions
{
	public class Loader : RPlugin
	{
		public override string Name => "CoreBots Options";
		public override string Author => "Lord Exelot";
		public override string Description => "Gives the ability to set the CoreBots Options with ease";

		private ToolStripItem menuItem;

		public override List<IOption> Options => new();

		public override void Load()
		{
			// This adds the plugin to the menu strip so it can be toggled on/off
			menuItem = Forms.Main.Plugins.DropDownItems.Add("CoreBots Options");
			menuItem.Click += MenuStripItem_Click;
		}

		public override void Unload()
		{
			// Unloading plugins is buggy in rbot so recommend not unloading but instead removing the plugin from the plugin folder
			// *this may be fixed in the future*
			menuItem.Click -= MenuStripItem_Click;
			Forms.Main.Plugins.DropDownItems.Remove(menuItem);
		}

		private void MenuStripItem_Click(object sender, EventArgs e)
		{
			// This will show/hide the example form when the menu strip button is click
			if (CoreBotsOptions.Instance.Visible)
			{
				CoreBotsOptions.Instance.Hide();
			}
			else
			{
				CoreBotsOptions.Instance.Show();
				CoreBotsOptions.Instance.BringToFront();
			}
		}
	}
}
