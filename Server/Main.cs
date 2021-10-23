using System;
using System.Collections.Generic;
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
        private class Veh
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

        private class SubMenu
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

        private List<SubMenu> subMenus;
        
        public Main()
        {
            Debug.WriteLine("server started");
            var rawJson = LoadResourceFile(GetCurrentResourceName(), "Config/config.json");
            var cfg = JObject.Parse(rawJson);

            subMenus = JsonConvert.DeserializeObject<List<SubMenu>>(cfg.SelectToken("menus").ToString());
        }

        
        [EventHandler("BadgerVehSpawner:GetFromServer")]
        private void SendToClient([FromSource] Player player)
		{
            var subMenusB = subMenus;

            for (int i = subMenusB.Count - 1; i >= 0; i--)
			{
                if (!IsPlayerAceAllowed(player.Handle, subMenusB[i].AceRequired))
                {
                    subMenus.RemoveAt(i);
                }
            }

            string jsonString = JsonConvert.SerializeObject(subMenusB);
            TriggerClientEvent(player, "BadgerVehSpawner:SendToClient", jsonString);
        }
    }
}
