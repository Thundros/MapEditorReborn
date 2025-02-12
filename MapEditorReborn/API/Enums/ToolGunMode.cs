﻿namespace MapEditorReborn.API
{
    /// <summary>
    /// The modes used by the ToolGun.
    /// </summary>
    public enum ToolGunMode
    {
        /// <summary>
        /// The mode that will spawn a Light Containment Zone door.
        /// </summary>
        LczDoor = 0,

        /// <summary>
        /// The mode that will spawn a Heavy Containment Zone door.
        /// </summary>
        HczDoor = 1,

        /// <summary>
        /// The mode that will spawn a Entrance Zone door.
        /// </summary>
        EzDoor = 2,

        /// <summary>
        /// The mode that will spawn a WorkStation.
        /// </summary>
        WorkStation = 3,

        /// <summary>
        /// The mode that will spawn a ItemSpawnPoint.
        /// </summary>
        ItemSpawnPoint = 4,

        /// <summary>
        /// The mode that will spawn a PlayerSpawnPoint.
        /// </summary>
        PlayerSpawnPoint = 5,

        /// <summary>
        /// The mode that will spawn a RagdollSpawnPoint.
        /// </summary>
        RagdollSpawnPoint = 6,

        DummySpawnPoint = 7,

        /// <summary>
        /// The mode that will spawn a SportShootingTarget.
        /// </summary>
        SportShootingTarget = 8,

        /// <summary>
        /// The mode that will spawn a DboyShootingTarget.
        /// </summary>
        DboyShootingTarget = 9,

        /// <summary>
        /// The mode that will spawn a BinaryShootingTarget.
        /// </summary>
        BinaryShootingTarget = 10,

        Primitive = 11,

        LightSource = 12,

        /// <summary>
        /// The mode that will spawn a LightController.
        /// </summary>
        RoomLight = 13,

        /// <summary>
        /// The mode that will spawn a Teleporter.
        /// </summary>
        Teleporter = 14,
    }
}
