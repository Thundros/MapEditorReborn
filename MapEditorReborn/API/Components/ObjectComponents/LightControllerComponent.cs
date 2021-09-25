﻿namespace MapEditorReborn.API
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using UnityEngine;

    /// <summary>
    /// Component added to spawned LightControllerObject. Is is used for easier idendification of the object and it's variables.
    /// </summary>
    public class LightControllerComponent : MapEditorObject
    {
        /// <summary>
        /// Instantiates <see cref="LightControllerObject"/>.
        /// </summary>
        /// <param name="lightControllerObject">The <see cref="LightControllerObject"/> used for instantiating the object.</param>
        /// <returns>Instance of this compoment.</returns>
        public LightControllerComponent Init(LightControllerObject lightControllerObject)
        {
            Base = lightControllerObject;

            UpdateObject();

            return this;
        }

        public LightControllerObject Base;

        /// <inheritdoc cref="MapEditorObject.UpdateObject()"/>
        public override void UpdateObject()
        {
            OnDestroy();
            lightControllers.Clear();

            Color color = new Color(Base.Red, Base.Green, Base.Blue, Base.Alpha);

            foreach (Room room in Map.Rooms.Where(x => x.Type == Base.RoomType))
            {
                FlickerableLightController lightController = null;

                if (Base.RoomType != RoomType.Surface)
                {
                    lightController = room.GetComponentInChildren<FlickerableLightController>();
                }
                else
                {
                    lightController = FindObjectsOfType<FlickerableLightController>().First(x => Map.FindParentRoom(x.gameObject).Type == RoomType.Surface);
                }

                if (lightController != null)
                {
                    lightControllers.Add(lightController);

                    lightController.Network_warheadLightColor = color;
                    lightController.Network_warheadLightOverride = !Base.OnlyWarheadLight;
                }
            }

            currentColor = color;
        }

        private void Update()
        {
            if (Base.ShiftSpeed == 0f)
                return;

            currentColor = ShiftHueBy(currentColor, Base.ShiftSpeed * Time.deltaTime);

            foreach (FlickerableLightController lightController in lightControllers)
            {
                lightController.Network_warheadLightColor = currentColor;
            }
        }

        private void OnDestroy()
        {
            foreach (FlickerableLightController lightController in lightControllers)
            {
                lightController.Network_warheadLightColor = FlickerableLightController.DefaultWarheadColor;
                lightController.Network_warheadLightOverride = false;
            }
        }

        // Credits to Killers0992
        private Color ShiftHueBy(Color color, float amount)
        {
            // convert from RGB to HSV
            Color.RGBToHSV(color, out float hue, out float saturation, out float value);

            // shift hue by amount
            hue += amount;

            // convert back to RGB and return the color
            return Color.HSVToRGB(hue, saturation, value);
        }

        private Color currentColor;

        private List<FlickerableLightController> lightControllers = new List<FlickerableLightController>();
    }
}
