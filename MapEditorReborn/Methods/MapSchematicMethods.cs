﻿namespace MapEditorReborn
{
    using System.IO;
    using API;
    using Exiled.API.Features;
    using Exiled.Loader;
    using MEC;

    public static partial class Methods
    {
        /// <summary>
        /// Loads the <see cref="MapSchematic"/> map.
        /// It also may be used for reloading the map.
        /// </summary>
        /// <param name="map"><see cref="MapSchematic"/> to load.</param>
        public static void LoadMap(MapSchematic map)
        {
            Log.Debug("Trying to load the map...", Config.Debug);

            foreach (MapEditorObject mapEditorObject in SpawnedObjects)
            {
                mapEditorObject?.Destroy();
            }

            SpawnedObjects.Clear();

            Log.Debug("Destroyed all map's GameObjects and indicators.", Config.Debug);

            // This is to bring vanilla spawnpoints to their previous state.
            PlayerSpawnPointComponent.VanillaSpawnPointsDisabled = false;

            // This is to remove selected object hint.
            foreach (Player player in Player.List)
            {
                SelectObject(player, null);
            }

            if (map == null)
            {
                Log.Debug("Map is null. Returning...", Config.Debug);
                return;
            }

            foreach (DoorObject door in map.Doors)
            {
                Log.Debug($"Trying to spawn door at {door.RoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnDoor(door));
            }

            if (map.Doors.Count > 0)
                Log.Debug("All doors have been successfully spawned!", Config.Debug);

            foreach (WorkStationObject workstation in map.WorkStations)
            {
                Log.Debug($"Spawning workstation at {workstation.RoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnWorkStation(workstation));
            }

            if (map.WorkStations.Count > 0)
                Log.Debug("All workstations have been successfully spawned!", Config.Debug);

            foreach (ItemSpawnPointObject itemSpawnPoint in map.ItemSpawnPoints)
            {
                Log.Debug($"Trying to spawn a item spawn point at {itemSpawnPoint.RoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnItemSpawnPoint(itemSpawnPoint));
            }

            if (map.ItemSpawnPoints.Count > 0)
                Log.Debug("All item spawn points have been spawned!", Config.Debug);

            foreach (PlayerSpawnPointObject playerSpawnPoint in map.PlayerSpawnPoints)
            {
                Log.Debug($"Trying to spawn a player spawn point at {playerSpawnPoint.RoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnPlayerSpawnPoint(playerSpawnPoint));
            }

            if (map.PlayerSpawnPoints.Count > 0)
                Log.Debug("All player spawn points have been spawned!", Config.Debug);

            PlayerSpawnPointComponent.VanillaSpawnPointsDisabled = map.RemoveDefaultSpawnPoints;

            foreach (RagdollSpawnPointObject ragdollSpawnPoint in map.RagdollSpawnPoints)
            {
                Log.Debug($"Trying to spawn a ragdoll spawn point at {ragdollSpawnPoint.RoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnRagdollSpawnPoint(ragdollSpawnPoint));
            }

            if (map.RagdollSpawnPoints.Count > 0)
                Log.Debug("All ragdoll spawn points have been spawned!", Config.Debug);

            foreach (ShootingTargetObject shootingTargetObject in map.ShootingTargetObjects)
            {
                Log.Debug($"Trying to spawn a shooting target at {shootingTargetObject.RoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnShootingTarget(shootingTargetObject));
            }

            if (map.ShootingTargetObjects.Count > 0)
                Log.Debug("All shooting targets have been spawned!", Config.Debug);

            foreach (PrimitiveObject primitiveObject in map.PrimitiveObjects)
            {
                SpawnedObjects.Add(SpawnPrimitive(primitiveObject));
            }

            foreach (RoomLightObject lightControllerObject in map.RoomLightObjects)
            {
                Log.Debug($"Trying to spawn a light controller at {lightControllerObject.RoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnLightController(lightControllerObject));
            }

            if (map.RoomLightObjects.Count > 0)
                Log.Debug("All light controllers have been spawned!", Config.Debug);

            foreach (LightSourceObject lightSourceObject in map.LightSourceObjects)
            {
                SpawnedObjects.Add(SpawnLightSource(lightSourceObject));
            }

            foreach (TeleportObject teleportObject in map.TeleportObjects)
            {
                Log.Debug($"Trying to spawn a teleporter at {teleportObject.EntranceTeleporterRoomType}...", Config.Debug);
                SpawnedObjects.Add(SpawnTeleport(teleportObject));
            }

            if (map.TeleportObjects.Count > 0)
                Log.Debug("All teleporters have been spawned!", Config.Debug);

            foreach (SchematicObject schematicObject in map.SchematicObjects)
            {
                Log.Debug($"Trying to spawn a schematic named \"{schematicObject.SchematicName}\" at {schematicObject.RoomType}...", Config.Debug);

                MapEditorObject schematic = SpawnSchematic(schematicObject);

                if (schematic == null)
                {
                    Log.Warn($"The schematic with \"{schematicObject.SchematicName}\" name does not exist or has an invalid name. Skipping...");
                    continue;
                }

                SpawnedObjects.Add(schematic);
            }

            if (map.SchematicObjects.Count > 0)
                Log.Debug("All schematics have been spawned!", Config.Debug);

            Log.Debug("All GameObject have been spawned and the MapSchematic has been fully loaded!", Config.Debug);
        }

        /// <summary>
        /// Saves the map to a file.
        /// </summary>
        /// <param name="name">The name of the map.</param>
        public static void SaveMap(string name)
        {
            Log.Debug("Trying to save the map...", Config.Debug);

            MapSchematic map = GetMapByName(name);

            if (map == null)
            {
                map = new MapSchematic(name);
            }
            else
            {
                map.CleanupAll();
            }

            Log.Debug($"Map name set to \"{map.Name}\"", Config.Debug);

            foreach (MapEditorObject spawnedObject in SpawnedObjects)
            {
                try
                {
                    if (spawnedObject is IndicatorObjectComponent)
                        continue;

                    Log.Debug($"Trying to save GameObject at {spawnedObject.transform.position}...", Config.Debug);

                    switch (spawnedObject)
                    {
                        case DoorObjectComponent door:
                            {
                                door.Base.Position = door.RelativePosition;
                                door.Base.Rotation = door.RelativeRotation;
                                door.Base.Scale = door.Scale;
                                door.Base.RoomType = door.RoomType;

                                map.Doors.Add(door.Base);

                                break;
                            }

                        case WorkStationObjectComponent workStation:
                            {
                                workStation.Base.Position = workStation.RelativePosition;
                                workStation.Base.Rotation = workStation.RelativeRotation;
                                workStation.Base.Scale = workStation.Scale;
                                workStation.Base.RoomType = workStation.RoomType;

                                map.WorkStations.Add(workStation.Base);

                                break;
                            }

                        case PlayerSpawnPointComponent playerspawnPoint:
                            {
                                playerspawnPoint.Base.Position = playerspawnPoint.RelativePosition;
                                playerspawnPoint.Base.RoomType = playerspawnPoint.RoomType;

                                map.PlayerSpawnPoints.Add(playerspawnPoint.Base);

                                break;
                            }

                        case ItemSpawnPointComponent itemSpawnPoint:
                            {
                                itemSpawnPoint.Base.Position = itemSpawnPoint.RelativePosition;
                                itemSpawnPoint.Base.Rotation = itemSpawnPoint.RelativeRotation;
                                itemSpawnPoint.Base.Scale = itemSpawnPoint.Scale;
                                itemSpawnPoint.Base.RoomType = itemSpawnPoint.RoomType;

                                map.ItemSpawnPoints.Add(itemSpawnPoint.Base);

                                break;
                            }

                        case RagdollSpawnPointComponent ragdollSpawnPoint:
                            {
                                ragdollSpawnPoint.Base.Position = ragdollSpawnPoint.RelativePosition;
                                ragdollSpawnPoint.Base.Rotation = ragdollSpawnPoint.RelativeRotation;
                                ragdollSpawnPoint.Base.RoomType = ragdollSpawnPoint.RoomType;

                                map.RagdollSpawnPoints.Add(ragdollSpawnPoint.Base);

                                break;
                            }

                        case ShootingTargetComponent shootingTarget:
                            {
                                shootingTarget.Base.Position = shootingTarget.RelativePosition;
                                shootingTarget.Base.Rotation = shootingTarget.RelativeRotation;
                                shootingTarget.Base.Scale = shootingTarget.Scale;
                                shootingTarget.Base.RoomType = shootingTarget.RoomType;

                                map.ShootingTargetObjects.Add(shootingTarget.Base);

                                break;
                            }

                        case PrimitiveObjectComponent primitive:
                            {
                                primitive.Base.Position = primitive.RelativePosition;
                                primitive.Base.Rotation = primitive.RelativeRotation;
                                primitive.Base.RoomType = primitive.RoomType;
                                primitive.Base.Scale = primitive.Scale;

                                map.PrimitiveObjects.Add(primitive.Base);

                                break;
                            }

                        case RoomLightComponent lightController:
                            {
                                map.RoomLightObjects.Add(lightController.Base);

                                break;
                            }

                        case LightSourceComponent lightSource:
                            {
                                lightSource.Base.Position = lightSource.RelativePosition;
                                lightSource.Base.RoomType = lightSource.RoomType;

                                map.LightSourceObjects.Add(lightSource.Base);

                                break;
                            }

                        case TeleportControllerComponent teleportController:
                            {
                                teleportController.Base.EntranceTeleporterPosition = teleportController.EntranceTeleport.RelativePosition;
                                teleportController.Base.EntranceTeleporterScale = teleportController.EntranceTeleport.Scale;
                                teleportController.Base.EntranceTeleporterRoomType = teleportController.EntranceTeleport.RoomType;

                                teleportController.Base.ExitTeleporters.Clear();
                                foreach (var exitTeleport in teleportController.ExitTeleports)
                                {
                                    teleportController.Base.ExitTeleporters.Add(new ExitTeleporter(exitTeleport.RelativePosition, exitTeleport.Scale, exitTeleport.RoomType));
                                }

                                map.TeleportObjects.Add(teleportController.Base);

                                break;
                            }

                        case SchematicObjectComponent schematicObject:
                            {
                                schematicObject.Base.Position = schematicObject.OriginalPosition;
                                schematicObject.Base.Rotation = schematicObject.OriginalRotation;
                                schematicObject.Base.Scale = schematicObject.Scale;
                                schematicObject.Base.RoomType = schematicObject.RoomType;

                                map.SchematicObjects.Add(schematicObject.Base);

                                break;
                            }
                    }
                }
                catch (System.Exception)
                {
                    continue;
                }
            }

            string path = Path.Combine(MapEditorReborn.MapsDir, $"{map.Name}.yml");

            Log.Debug($"Path to file set to: {path}", Config.Debug);

            bool prevValue = Config.EnableFileSystemWatcher;
            if (prevValue)
                Config.EnableFileSystemWatcher = false;

            Log.Debug("Trying to serialize the MapSchematic...", Config.Debug);

            File.WriteAllText(path, Loader.Serializer.Serialize(map));

            Log.Debug("MapSchematic has been successfully saved to a file!", Config.Debug);

            Timing.CallDelayed(1f, () => Config.EnableFileSystemWatcher = prevValue);
        }

        /// <summary>
        /// Gets the <see cref="MapSchematic"/> by it's name.
        /// </summary>
        /// <param name="mapName">The name of the map.</param>
        /// <returns><see cref="MapSchematic"/> if the file with the map was found, otherwise <see langword="null"/>.</returns>
        public static MapSchematic GetMapByName(string mapName)
        {
            if (mapName == CurrentLoadedMap?.Name)
                return CurrentLoadedMap;

            string path = Path.Combine(MapEditorReborn.MapsDir, $"{mapName}.yml");

            if (!File.Exists(path))
                return null;

            MapSchematic map = Loader.Deserializer.Deserialize<MapSchematic>(File.ReadAllText(path));
            map.Name = mapName;

            return map;
        }

        /// <summary>
        /// Gets the <see cref="SaveDataObjectList"/> by it's name.
        /// </summary>
        /// <param name="schematicName">The name of the map.</param>
        /// <returns><see cref="SaveDataObjectList"/> if the file with the schematic was found, otherwise <see langword="null"/>.</returns>
        public static SaveDataObjectList GetSchematicDataByName(string schematicName)
        {
            string path = Path.Combine(MapEditorReborn.SchematicsDir, $"{schematicName}.json");

            if (!File.Exists(path))
                return null;

            return Utf8Json.JsonSerializer.Deserialize<SaveDataObjectList>(File.ReadAllText(path));
        }
    }
}
