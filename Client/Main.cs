using CitizenFX.Core;
using CitizenFX.Core.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using MenuAPI;
using System.Linq;

namespace Client
{
	class Main : BaseScript
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

		public Main()
		{
			TriggerServerEvent("VehicleMenu:RequestList");
		}

		private Menu menu;

		[EventHandler("VehicleMenu:SendListToClient")]
		public void StartMenu(List<dynamic> perms)
		{
			var cfg = JObject.Parse(LoadResourceFile(GetCurrentResourceName(), "config/config.json"));
			var menuData = JsonConvert.DeserializeObject<VehicleMenu>(cfg.SelectToken("menu").ToString());

			menu = new Menu(menuData.Title, menuData.Subtitle);

			MenuController.AddMenu(menu);
			MenuController.MainMenu = menu;
			MenuController.DisableMenuButtons = false;
			MenuController.DontOpenAnyMenu = false;
			MenuController.MenuToggleKey = (Control)menuData.MenuKey;

			if (menuData.RightAligned)
				MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
			else MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;

			// Create Categories
			for (int ci = 0; ci < menuData.Categories.Count; ci++)
			{
				var c = menuData.Categories[ci];

				if (perms[ci])
				{
					Menu submenu = new Menu(c.MenuTitle, "Badger#5830");
					MenuItem submenuItem = new MenuItem(c.Name)
					{
						Label = "→→→"
					};

					menu.AddMenuItem(submenuItem);
					MenuController.BindMenuItem(menu, submenu, submenuItem);

					for (int vi = 0; vi < c.Vehicles.Count; vi++)
					{
						var v = c.Vehicles[vi];

						MenuItem vehicleItem = new MenuItem(v.Name);
						submenu.AddMenuItem(vehicleItem);

						submenu.OnItemSelect += (menu, item, index) =>
						{
							if (item == vehicleItem)
							{
								SpawnVehicle(v.Model);
							}
						};
					}
				}
			}
		}

		private async Task SpawnVehicle(string model)
		{
			var ped = Game.PlayerPed;
			var hash = (uint)GetHashKey(model);

			// Invalid vehicle.
			if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
			{
				TriggerEvent("chat:addMessage", new
				{
					color = new[] { 255, 255, 255 },
					multiline = true,
					args = new[] { $"Vehicle model is invalid." }
				});
			}

			// Valid vehicle, continue.
			else
			{
				Vector3 spawnLoc = ped.Position;

				// Delete current vehicle.
				if (ped.IsInVehicle())
				{
					Vehicle curVeh = ped.CurrentVehicle;
					if (curVeh.GetPedOnSeat(VehicleSeat.Driver) == ped || curVeh.Occupants.Count() == 1)
					{
						spawnLoc = curVeh.Position;
						curVeh.Delete();
					}
					else if (curVeh.Occupants.Count() > 1)
					{
						spawnLoc = GetOffsetFromEntityInWorldCoords(ped.Handle, 0, 8f, 0.1f) + new Vector3(0f, 0f, 1f);
					}
				}
				// Delete previous vehicle.
				else
				{
					var lastVeh = GetVehiclePedIsIn(ped.Handle, true);
					if (DoesEntityExist(lastVeh))
					{
						Vehicle lVeh = ped.LastVehicle;
						
						if (lVeh.Occupants.Count() == 0)
						{
							lVeh.Delete();
						}
					}
				}

				// Load model
				bool loaded = await LoadModel(hash);
				if (loaded)
				{
					var veh = await World.CreateVehicle(model, spawnLoc, Game.PlayerPed.Heading);
					Game.PlayerPed.SetIntoVehicle(veh, VehicleSeat.Driver);
					SetVehicleEngineOn(ped.CurrentVehicle.Handle, true, true, false);
				}
			}
		}

		private async Task<bool> LoadModel(uint modelHash)
		{
			if (IsModelInCdimage(modelHash))
			{
				RequestModel(modelHash);

				while (!HasModelLoaded(modelHash))
				{
					await Delay(0);
				}
				return true;
			}
			else return false;
		}
	}
}