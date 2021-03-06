using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace BadgerVehSpawner
{
    public class Main : BaseScript
    {
        public class Veh
		{
            public string Name;
            public string SpawnCode;

            [JsonConstructor]
            public Veh(string name, string spawncode)
			{
                Name = name;
                SpawnCode = spawncode;
			}
        }
        
        public class SubMenu
        {
            public string Name;
            public string AceRequired;
            public Veh[] Vehicles;

            [JsonConstructor]
            public SubMenu(string name, string aceRequired, Veh[] vehicles)
			{
                Name = name;
                AceRequired = aceRequired;
                Vehicles = vehicles;
			}
        }

        [Command("cars")]
        private void TempCommandCars(int source, List<object> args, string raw)
		{
            mainMenu.OpenMenu();
		}

        //private SubMenu[] subMenus;
        private SubMenu[] subMenus;
        private bool rightAlligned;
        private Control menuKey;
        private bool enabledInEmergencyVehicles; 

        [EventHandler("onClientMapStart")]
        private void Start()
		{
            var rawJson = LoadResourceFile(GetCurrentResourceName(), "Config/config.json");
            var cfg = JObject.Parse(rawJson);

            menuKey = (Control)(int)cfg.SelectToken("menuKey");
            rightAlligned = (bool)cfg.SelectToken("rightAlligned");
            enabledInEmergencyVehicles = (bool)cfg.SelectToken("enabledInEmergencyVehicles");

            TriggerServerEvent("BadgerVehSpawner:GetFromServer");         
        }

        [EventHandler("BadgerVehSpawner:SendToClient")]
        private void ReceiveFromServer(string json)
		{
            subMenus = JsonConvert.DeserializeObject<SubMenu[]>(json);
            StartMenu();
        }

        private readonly Menu mainMenu = new Menu("Vehicle Spawner", "Vehicle Categories");

        private void StartMenu()
		{
            if (rightAlligned)
            {
                MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
            }
            else MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;

            MenuController.AddMenu(mainMenu);
            MenuController.MenuToggleKey = menuKey;
			MenuController.EnableMenuToggleKeyOnController = false;

            foreach (dynamic subMenu in subMenus)
			{
                Menu newSubMenu = new Menu(subMenu.Name, "Badger#5830");
                MenuItem newMainItem = new MenuItem(subMenu.Name)
                {
                    Label = "→→→"
                };
                mainMenu.AddMenuItem(newMainItem);
                MenuController.BindMenuItem(mainMenu, newSubMenu, newMainItem);

                foreach (Veh v in subMenu.Vehicles)
                {
                    MenuItem newSubItem = new MenuItem(v.Name);
                    newSubMenu.AddMenuItem(newSubItem);

                    newSubMenu.OnItemSelect += (menu, item, index) =>
                    {
                        if (item == newSubItem)
                        {
                            SpawnVehicle(v.SpawnCode);
                        }
                    };
                }
            }
        }

        private async void SpawnVehicle(string spawncode)
		{
            int ped = PlayerPedId();

            var model = spawncode;
            var hash = (uint)GetHashKey(model);

            if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
            {
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    multiline = true,
                    args = new[] { $"^1[BadgerVehSpawner] ^3Vehicle model is invalid, please contact the server's developers about this issue." }
                });
            }
            else
            {
                Vector3 spawnLoc = Game.PlayerPed.Position;

                if (IsPedInAnyVehicle(ped, false))
                {
                    var curVehicle = GetVehiclePedIsIn(ped, false);
                    if (GetPedInVehicleSeat(curVehicle, -1) == ped)
                    {
                        spawnLoc = GetEntityCoords(curVehicle, false);
                        DeleteEntity(ref curVehicle);
                    }
                }   
                else
				{
                    var lastVehicle = GetVehiclePedIsIn(ped, true);

                    if (IsAnyVehicleSeatEmpty(lastVehicle))
					{
                        DeleteEntity(ref lastVehicle);
					}
				}

                // Load model before spawning vehicle.
                bool successful = await LoadModel(hash);
                if (successful)
                {
                    var vehicle = await World.CreateVehicle(model, spawnLoc, Game.PlayerPed.Heading);
                    Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);
                    SetVehicleEngineOn(GetVehiclePedIsIn(ped, false), true, true, false);
                }
            }
        }

        private async Task<bool> LoadModel(uint modelHash)
        {
            // Check if the model exists in the game.
            if (IsModelInCdimage(modelHash))
            {
                // Load the model.
                RequestModel(modelHash);
                // Wait until it's loaded.
                while (!HasModelLoaded(modelHash))
                {
                    await Delay(0);
                }
                // Model is loaded, return true.
                return true;
            }
            // Model is not valid or is not loaded correctly.
            else
            {
                // Return false.
                return false;
            }
        }

        [Tick]
        private async Task OnTick()
		{
            if (!enabledInEmergencyVehicles)
            {
                var ped = PlayerPedId();

                if (IsPedInAnyVehicle(ped, true))
                {
                    var veh = GetVehiclePedIsIn(ped, false);

                    if (GetVehicleClass(veh) == 18 && GetPedInVehicleSeat(veh, -1) == ped)
                    {
                        MenuController.DontOpenAnyMenu = true;
                    }
                    else MenuController.DontOpenAnyMenu = false;
                }
                else MenuController.DontOpenAnyMenu = false;

                await Delay(250);
            }

            else await Delay(6000);
		}
    }
}
