﻿namespace MapEditorReborn.Commands.Position.SubCommands
{
    using System;
    using API;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using UnityEngine;

    /// <summary>
    /// Modifies object's position by setting it to the sender's current position.
    /// </summary>
    public class Bring : ICommand
    {
        /// <inheritdoc/>
        public string Command => "bring";

        /// <inheritdoc/>
        public string[] Aliases => Array.Empty<string>();

        /// <inheritdoc/>
        public string Description => string.Empty;

        /// <inheritdoc/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("mpr.position"))
            {
                response = $"You don't have permission to execute this command. Required permission: mpr.position";
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

            if (mapObject is RoomLightComponent)
            {
                response = "You can't modify this object's position!";
                return false;
            }

            Vector3 newPosition = player.Position;

            if (mapObject.name.Contains("Door"))
                newPosition += Vector3.down * 1.33f;

            mapObject.transform.position = newPosition;

            mapObject.UpdateObject();
            mapObject.UpdateIndicator();
            player.ShowGameObjectHint(mapObject);

            response = newPosition.ToString("F3");
            return true;
        }
    }
}
