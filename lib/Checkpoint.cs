using UnityEngine;

namespace SpeedMod.Checkpoints{

    /// <summary>
    /// Allows the mod to save the player's position and rotation.
    /// </summary>
    public class Checkpoint
    {
        public Vector3 position;
        public Quaternion rotation;
        public float time;
        public float startTime;

        /// <summary>
        /// Constructor function.
        /// </summary>
        /// <param name="pos">Vector3 Position of the checkpoint.</param>
        /// <param name="rot">Quaternion rotation of the checkpoint.</param>
        /// <param name="timeNow">Current time when the checkpoint was made.</param>
        /// <param name="st">Start time of the game when the checkpoint was made.</param>
        public Checkpoint(Vector3 pos, Quaternion rot, float timeNow, float st)
        {
            position = pos;
            rotation = rot;
            time = timeNow;
            startTime = st;
        }
    }
}