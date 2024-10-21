using UnityEngine;

namespace SpeedMod.Checkpoints{

    /// <summary>
    /// Allows the mod to save the player's position and rotation.
    /// </summary>
    public class Checkpoint
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool checkpointSet;

        /// <summary>
        /// Constructor function.
        /// </summary>
        /// <param name="pos">Vector3 Position of the checkpoint.</param>
        /// <param name="rot">Quaternion rotation of the checkpoint.</param>
        public Checkpoint(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
            checkpointSet = false;
        }
    }
}