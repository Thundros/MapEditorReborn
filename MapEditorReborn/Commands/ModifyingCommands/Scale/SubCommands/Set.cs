﻿namespace MapEditorReborn.Commands.Scale.SubCommands
{
    using System;
    using API;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    /// <summary>
    /// Modifies object's scale by setting it to a certain value.
    /// </summary>
    public class Set : ICommand
    {
        /// <inheritdoc/>
        public string Command => "set";

        /// <inheritdoc/>
        public string[] Aliases => Array.Empty<string>();

        /// <inheritdoc/>
        public string Description => string.Empty;

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("mpr.scale"))
            {
                response = $"You don't have permission to execute this command. Required permission: mpr.scale";
                return false;
            }

            Player player = Player.Get(sender);
            if (!player.TryGetSessionVariable(Methods.SelectedObjectSessionVarName, out MapEditorObject mapObject) || mapObject == null)
            {
                if (!Methods.TryGetMapObject(player, out mapObject))
                {
                    response = "You haven't selected any object!";
                    return false;
                }
                else
                {
                    Methods.SelectObject(player, mapObject);
                }
            }

            if (mapObject is RoomLightComponent || mapObject is PlayerSpawnPointComponent || mapObject is RagdollSpawnPointComponent)
            {
                response = "You can't modify this object's scale!";
                return false;
            }

            if (arguments.Count >= 3 && float.TryParse(arguments.At(0), out float x) && float.TryParse(arguments.At(1), out float y) && float.TryParse(arguments.At(2), out float z))
            {
                Vector3 newScale = new Vector3(x, y, z);

                mapObject.transform.localScale = newScale;
                player.ShowGameObjectHint(mapObject);

                mapObject.UpdateObject();
                mapObject.UpdateIndicator();

                response = newScale.ToString("F3");
                return true;
            }

            response = "Invalid values.";
            return false;
        }
    }
}
