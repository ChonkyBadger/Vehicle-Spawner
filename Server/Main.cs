using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CitizenFX.Core.Native.API;


namespace Server
{
    public class Main : BaseScript
    {
		struct VehicleMenu
        {
            public string Title { get; }
            public string Subtitle { get; }
            public int MenuKey { get; }
            public bool RightAligned { get; }
            public List<Category> Categories { get; }

            [JsonConstructor]
            public VehicleMenu(string title, string subtitle, int menuKey, bool rightAligned, List<Category> categories)
            {
                Title = title;
                Subtitle = subtitle;
                MenuKey = menuKey;
                RightAligned = rightAligned;
                Categories = categories;
            }
        }

		struct Category
        {
            public string Name { get; }
            public string MenuTitle { get; }
            public string Ace { get; }
            public List<VehicleS> Vehicles { get; }

            [JsonConstructor]
            public Category(string name, string menuTitle, string ace, List<VehicleS> vehicles)
            {
                Name = name;
                MenuTitle = menuTitle;
                Ace = ace;
                Vehicles = vehicles;
            }
        }

		struct VehicleS
        {
            public string Name { get; }
            public string Model { get; }

            [JsonConstructor]
            public VehicleS(string name, string model)
            {
                Name = name;
                Model = model;
            }
        }

		private static readonly JObject cfg = JObject.Parse(LoadResourceFile(GetCurrentResourceName(), "config/config.json"));
		private static readonly VehicleMenu menu = JsonConvert.DeserializeObject<VehicleMenu>(cfg.SelectToken("menu").ToString());

		[EventHandler("VehicleMenu:RequestList")]
        public void IsAllowed([FromSource] Player player)
        {
			var perms = new List<bool>();
			bool anyPerms = false;

			foreach (Category c in menu.Categories)
			{
				if (IsPlayerAceAllowed(player.Handle, c.Ace))
				{
					perms.Add(true);
					anyPerms = true;
				}
				else
				{
					perms.Add(false);
				}
			}

			// Only send to the client if it has any categories.
			if (anyPerms)
            {
                player.TriggerEvent("VehicleMenu:SendListToClient", perms);
			}
        }
    }
}
