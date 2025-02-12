﻿namespace MapEditorReborn.API
{
    using UnityEngine;

    /// <summary>
    /// Handles rotating a dummy indicator.
    /// </summary>
    public class DummySpiningComponent : MapEditorObject
    {
        /// <summary>
        /// Initializes the <see cref="DummySpiningComponent"/>.
        /// </summary>
        /// <param name="referenceHub">The <see cref="ReferenceHub"/> of the dummy.</param>
        /// <param name="speed">The rotation speed.</param>
        /// <returns>Instance of this compoment.</returns>
        public DummySpiningComponent Init(ReferenceHub referenceHub, float speed = 3f)
        {
            hub = referenceHub;
            Speed = speed;

            return this;
        }

        /// <summary>
        /// The spinning speed.
        /// </summary>
        public float Speed = 3f;

        private void Update()
        {
            hub.playerMovementSync.RotationSync = new Vector2(0, i);

            i += Speed;
            if (i > 360)
                i = 0;
        }

        private ReferenceHub hub;

        private float i;
    }
}
