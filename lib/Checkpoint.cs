using UnityEngine;

namespace SpeedMod.Checkpoints{
    public class Checkpoint
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool checkpointSet;

        public Checkpoint(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
            checkpointSet = false;
        }
    }
}