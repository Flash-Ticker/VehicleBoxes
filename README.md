## VehicleBoxes

**Project details**

**Project:** *VehicleBoxes*

**dev language:** *c# oxide*

**Plugin language:** *en*
**Author:** [@RustFlash](https://github.com/Flash-Ticker)
[![RustFlash - Your Favourite Trio Server](https://github.com/Flash-Ticker/VehicleBoxes/blob/main/VehicleBoxes_Thumb.png)]()

## Description

 **VehicleBoxes** is a plugin that expands the vehicle storage system in the game. It allows players to attach and remove storage boxes from vehicles to create additional storage space and improve the overall utility of the vehicle. The plugin aims to improve the game experience in Rust by providing more flexibility in vehicle usage. It adds an extra layer of customization and practicality to vehicles, making them even more valuable objects in the game. 
 
## Features:

- **Add Boxes**: Players can add storage boxes to supported vehicles.
- **Remove Boxes**: Players can remove added boxes from vehicles.
- **Vehicle Support**: Currently supports Minicopter and Scrap Transport Helicopter.
- **Permission System**: Ensures that only authorized players can add or remove boxes.
- **Configurable**: Customizable box prefab and positioning for different vehicle types.

## Commands:

1. `/addbox` - Adds a storage box to the vehicle the player is looking at.
2. `/removebox` - Removes the storage box from the vehicle the player is looking at.

## Permissions:

1. `vehicleboxes.addbox` - Allows players to add boxes to vehicles.
2. `vehicleboxes.removebox` - Allows players to remove boxes from vehicles.

## Configuration:

The plugin settings can be adjusted in the `VehicleBoxes.json` file in the `config` folder. Here you can change properties like the box prefab and the position/rotation for different vehicle types.

```json
{
  "BoxPrefab": "assets/prefabs/deployable/woodenbox/woodbox_deployed.prefab",
  "VehiclePositions": {
    "assets/content/vehicles/minicopter/minicopter.entity.prefab": {
      "Position": {
        "x": 0,
        "y": 0.31,
        "z": -0.57
      },
      "Rotation": {
        "x": 0,
        "y": 90,
        "z": 0
      }
    },
    "assets/content/vehicles/scrap heli carrier/scraptransporthelicopter.prefab": {
      "Position": {
        "x": -0.5,
        "y": 0.80,
        "z": 1.75
      },
      "Rotation": {
        "x": 0,
        "y": 0,
        "z": 0
      }
    }
  }
}
```

## Multilingual Support:

The plugin supports multiple languages. Language files are located in the lang folder and can be edited or expanded as needed. Currently supported languages:

default: EN

**DE | FR | ES | IT | TR | RU | UK**

Please note that the language files were translated using the DeepL Language Tool 

---

**load, run, enjoy** 💝

