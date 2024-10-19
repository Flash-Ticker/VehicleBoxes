using Oxide.Core;
using Oxide.Core.Libraries.Covalence;
using System.Collections.Generic;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("VehicleBoxes", "RustFlash", "1.0.7")]
    [Description("Allows players to add and remove boxes to/from vehicles")]
    class VehicleBoxes : CovalencePlugin
    {
        private const string PermissionAddBox = "vehicleboxes.addbox";
        private const string PermissionRemoveBox = "vehicleboxes.removebox";

        private Configuration config;
        private Dictionary<NetworkableId, NetworkableId> attachedBoxes = new Dictionary<NetworkableId, NetworkableId>();

        private class Configuration
        {
            public string BoxPrefab = "assets/prefabs/deployable/woodenbox/woodbox_deployed.prefab";
            public Dictionary<string, BoxPosition> VehiclePositions = new Dictionary<string, BoxPosition>
            {
                ["assets/content/vehicles/minicopter/minicopter.entity.prefab"] = new BoxPosition { Position = new Vector3(0, 0.31f, -0.57f), Rotation = new Vector3(0, 90, 0) },
                ["assets/content/vehicles/scrap heli carrier/scraptransporthelicopter.prefab"] = new BoxPosition { Position = new Vector3(-0.5f, 0.80f, 1.75f), Rotation = Vector3.zero }
            };
        }

        private class BoxPosition
        {
            public Vector3 Position;
            public Vector3 Rotation;
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            config = Config.ReadObject<Configuration>();
            SaveConfig();
        }

        protected override void LoadDefaultConfig() => config = new Configuration();

        protected override void SaveConfig() => Config.WriteObject(config);

        protected override void LoadDefaultMessages()
        {
            // English (default)
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "You don't have permission to use this command.",
                ["VehicleNotFound"] = "No vehicle found. Make sure you're looking at a vehicle.",
                ["BoxAdded"] = "Box added to the vehicle.",
                ["BoxRemoved"] = "Box removed from the vehicle.",
                ["BoxAlreadyAdded"] = "This vehicle already has a box added.",
                ["NoBoxAdded"] = "This vehicle doesn't have a box added.",
                ["VehicleNotSupported"] = "This type of vehicle is not supported."
            }, this, "en");

            // German
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "Du hast keine Berechtigung, diesen Befehl zu verwenden.",
                ["VehicleNotFound"] = "Kein Fahrzeug gefunden. Stelle sicher, dass du auf ein Fahrzeug schaust.",
                ["BoxAdded"] = "Box zum Fahrzeug hinzugefügt.",
                ["BoxRemoved"] = "Box vom Fahrzeug entfernt.",
                ["BoxAlreadyAdded"] = "Dieses Fahrzeug hat bereits eine Box.",
                ["NoBoxAdded"] = "Dieses Fahrzeug hat keine Box.",
                ["VehicleNotSupported"] = "Dieser Fahrzeugtyp wird nicht unterstützt."
            }, this, "de");

            // French
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "Vous n'avez pas la permission d'utiliser cette commande.",
                ["VehicleNotFound"] = "Aucun véhicule trouvé. Assurez-vous de regarder un véhicule.",
                ["BoxAdded"] = "Boîte ajoutée au véhicule.",
                ["BoxRemoved"] = "Boîte retirée du véhicule.",
                ["BoxAlreadyAdded"] = "Ce véhicule a déjà une boîte ajoutée.",
                ["NoBoxAdded"] = "Ce véhicule n'a pas de boîte ajoutée.",
                ["VehicleNotSupported"] = "Ce type de véhicule n'est pas pris en charge."
            }, this, "fr");

            // Spanish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "No tienes permiso para usar este comando.",
                ["VehicleNotFound"] = "No se encontró ningún vehículo. Asegúrate de estar mirando a un vehículo.",
                ["BoxAdded"] = "Caja añadida al vehículo.",
                ["BoxRemoved"] = "Caja retirada del vehículo.",
                ["BoxAlreadyAdded"] = "Este vehículo ya tiene una caja añadida.",
                ["NoBoxAdded"] = "Este vehículo no tiene una caja añadida.",
                ["VehicleNotSupported"] = "Este tipo de vehículo no está soportado."
            }, this, "es");

            // Italian
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "Non hai il permesso di usare questo comando.",
                ["VehicleNotFound"] = "Nessun veicolo trovato. Assicurati di guardare un veicolo.",
                ["BoxAdded"] = "Scatola aggiunta al veicolo.",
                ["BoxRemoved"] = "Scatola rimossa dal veicolo.",
                ["BoxAlreadyAdded"] = "Questo veicolo ha già una scatola aggiunta.",
                ["NoBoxAdded"] = "Questo veicolo non ha una scatola aggiunta.",
                ["VehicleNotSupported"] = "Questo tipo di veicolo non è supportato."
            }, this, "it");

            // Turkish
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "Bu komutu kullanma izniniz yok.",
                ["VehicleNotFound"] = "Araç bulunamadı. Bir araca baktığınızdan emin olun.",
                ["BoxAdded"] = "Kutu araca eklendi.",
                ["BoxRemoved"] = "Kutu araçtan kaldırıldı.",
                ["BoxAlreadyAdded"] = "Bu araca zaten bir kutu eklenmiş.",
                ["NoBoxAdded"] = "Bu araca eklenmiş bir kutu yok.",
                ["VehicleNotSupported"] = "Bu araç tipi desteklenmiyor."
            }, this, "tr");

            // Russian
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "У вас нет разрешения на использование этой команды.",
                ["VehicleNotFound"] = "Транспортное средство не найдено. Убедитесь, что вы смотрите на транспортное средство.",
                ["BoxAdded"] = "Ящик добавлен к транспортному средству.",
                ["BoxRemoved"] = "Ящик удален с транспортного средства.",
                ["BoxAlreadyAdded"] = "К этому транспортному средству уже добавлен ящик.",
                ["NoBoxAdded"] = "К этому транспортному средству не добавлен ящик.",
                ["VehicleNotSupported"] = "Этот тип транспортного средства не поддерживается."
            }, this, "ru");

            // Ukrainian
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["NoPermission"] = "У вас немає дозволу на використання цієї команди.",
                ["VehicleNotFound"] = "Транспортний засіб не знайдено. Переконайтеся, що ви дивитесь на транспортний засіб.",
                ["BoxAdded"] = "Ящик додано до транспортного засобу.",
                ["BoxRemoved"] = "Ящик видалено з транспортного засобу.",
                ["BoxAlreadyAdded"] = "До цього транспортного засобу вже додано ящик.",
                ["NoBoxAdded"] = "До цього транспортного засобу не додано ящик.",
                ["VehicleNotSupported"] = "Цей тип транспортного засобу не підтримується."
            }, this, "uk");
        }

        private void Init()
        {
            permission.RegisterPermission(PermissionAddBox, this);
            permission.RegisterPermission(PermissionRemoveBox, this);
        }

        [Command("addbox")]
        private void AddBoxCommand(IPlayer player, string command, string[] args)
        {
            if (!player.HasPermission(PermissionAddBox))
            {
                player.Reply(GetMessage("NoPermission", player.Id));
                return;
            }

            BaseVehicle vehicle = GetLookingAtVehicle(player);
            if (vehicle == null)
            {
                player.Reply(GetMessage("VehicleNotFound", player.Id));
                return;
            }

            if (HasAttachedBox(vehicle))
            {
                player.Reply(GetMessage("BoxAlreadyAdded", player.Id));
                return;
            }

            if (AddBox(vehicle))
            {
                player.Reply(GetMessage("BoxAdded", player.Id));
            }
            else
            {
                player.Reply(GetMessage("VehicleNotSupported", player.Id));
            }
        }

        [Command("removebox")]
        private void RemoveBoxCommand(IPlayer player, string command, string[] args)
        {
            if (!player.HasPermission(PermissionRemoveBox))
            {
                player.Reply(GetMessage("NoPermission", player.Id));
                return;
            }

            BaseVehicle vehicle = GetLookingAtVehicle(player);
            if (vehicle == null)
            {
                player.Reply(GetMessage("VehicleNotFound", player.Id));
                return;
            }

            if (RemoveBox(vehicle))
            {
                player.Reply(GetMessage("BoxRemoved", player.Id));
            }
            else
            {
                player.Reply(GetMessage("NoBoxAdded", player.Id));
            }
        }

        private BaseVehicle GetLookingAtVehicle(IPlayer player)
        {
            BasePlayer basePlayer = player.Object as BasePlayer;
            RaycastHit hit;
            if (Physics.Raycast(basePlayer.eyes.HeadRay(), out hit, 5f))
            {
                return hit.GetEntity()?.GetComponent<BaseVehicle>();
            }
            return null;
        }

        private bool AddBox(BaseVehicle vehicle)
        {
            if (!config.VehiclePositions.TryGetValue(vehicle.PrefabName, out BoxPosition boxPosition))
            {
                return false;
            }

            BaseEntity box = GameManager.server.CreateEntity(config.BoxPrefab, vehicle.transform.position);
            if (box == null) return false;

            box.SetParent(vehicle, true);
            box.transform.localPosition = boxPosition.Position;
            box.transform.localEulerAngles = boxPosition.Rotation;
            box.Spawn();

            attachedBoxes[vehicle.net.ID] = box.net.ID;

            return true;
        }

        private bool RemoveBox(BaseVehicle vehicle)
        {
            if (!attachedBoxes.TryGetValue(vehicle.net.ID, out NetworkableId boxNetId))
                return false;

            BaseNetworkable boxNetworkable = BaseNetworkable.serverEntities.Find(boxNetId);
            StorageContainer attachedBox = boxNetworkable as StorageContainer;

            if (attachedBox == null) return false;

            attachedBox.SetParent(null);
            attachedBox.transform.position = vehicle.transform.position + (Vector3.up * 1f);
            attachedBoxes.Remove(vehicle.net.ID);
            attachedBox.SendNetworkUpdate();

            return true;
        }

        private bool HasAttachedBox(BaseVehicle vehicle)
        {
            return attachedBoxes.ContainsKey(vehicle.net.ID);
        }

        private string GetMessage(string key, string playerId = null) => lang.GetMessage(key, this, playerId);
    }
}