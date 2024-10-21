using BepInEx.Logging;
using UnityEngine;
using ECM.Components;
using System.Linq;
using System;
using System.Collections.Generic;

namespace SpeedMod.Checkpoints{

    public class CheckpointManager
    {
        // Event to trigger canvas redraw
        public event Action OnCheckpointUpdated;

        //Logging
        internal static ManualLogSource Logger;

        // Checkpoint dictionary structure
        private Dictionary<int, Checkpoint> checkpoints;

        // Checkpoint states to detect changes
        private static List<bool> lastCheckpointState;
        private static List<bool> currentCheckpointState;

        public CheckpointManager(ManualLogSource logger)
        {
            Logger = logger;
            Logger.LogDebug("Instanciating CheckpointManager");

            this.checkpoints = new Dictionary<int, Checkpoint>();

            this.checkpoints.Add(1, new Checkpoint(new Vector3(0, 0, 0), Quaternion.identity));
            this.checkpoints.Add(2, new Checkpoint(new Vector3(0, 0, 0), Quaternion.identity));
            this.checkpoints.Add(3, new Checkpoint(new Vector3(0, 0, 0), Quaternion.identity));

            currentCheckpointState = [false, false, false];

            lastCheckpointState = new List<bool>(currentCheckpointState);
        }

        /// <summary>
        /// Checks whether or not checkpoints have been updated since last state.
        /// </summary>
        public bool CheckpointsUpdated()
        {
            if (currentCheckpointState.SequenceEqual(lastCheckpointState))
            {
                return false;
            }
            else
            {
                lastCheckpointState = new List<bool>(currentCheckpointState);
                return true;
            }
        }

        /// <summary>
        /// Sets a checkpoint's position & rotation.
        /// </summary>
        /// <param name="checkpointNumber">Index of the checkpoint.</param>
        /// <param name="climberTransform">Transform of the player, contains rotation & position</param>
        public void SetCheckpoint(int checkpointNumber, Transform climberTransform){
            Logger.LogInfo($"Setting checkpoint {checkpointNumber}");

            Checkpoint checkpoint    = this.checkpoints[checkpointNumber];

            checkpoint.position      = climberTransform.position;
            checkpoint.rotation      = climberTransform.rotation;
            checkpoint.checkpointSet = true;

            currentCheckpointState[checkpointNumber - 1] = true;

            if (CheckpointsUpdated() && OnCheckpointUpdated != null)
            {
                OnCheckpointUpdated.Invoke();  // Trigger event if checkpoints are updated
            }
        }
        
        /// <summary>
        /// Load an existing checkpoint.
        /// </summary>
        /// <param name="checkpointNumber">Index of the checkpoint.</param>
        /// <param name="climberTransform">Transform of the player, contains rotation & position.</param>
        /// <param name="characterMovement">Movement of the climber.</param>
        public void LoadCheckpoint(int checkpointNumber, Transform climberTransform, CharacterMovement characterMovement){
            Logger.LogInfo($"Loading checkpoint {checkpointNumber}");

            if (IsCheckpointSet(checkpointNumber))
            {
                climberTransform.position = checkpoints[checkpointNumber].position;
                climberTransform.rotation = checkpoints[checkpointNumber].rotation;

                characterMovement.velocity = new Vector3(0, 0, 0);
            }
        }
        
        /// <summary>
        /// Checks if the checkpoint index contains a set checkpoint.
        /// </summary>
        /// <param name="checkpointNumber">Index of the checkpoint.</param>
        public bool IsCheckpointSet(int checkpointNumber) {
            bool set = this.checkpoints[checkpointNumber].checkpointSet;
            return set;
        }

        /// <summary>
        /// Sets all checkpoints to zero, and unsets them.
        /// </summary>
        public void ResetCheckpoints()
        {
            // Reset each checkpoint's position, rotation, and state
            foreach (var checkpoint in checkpoints)
            {
                checkpoint.Value.position = Vector3.zero;          // Reset position to (0, 0, 0)
                checkpoint.Value.rotation = Quaternion.identity;   // Reset rotation to identity
                checkpoint.Value.checkpointSet = false;            // Mark the checkpoint as not set
            }

            // Reset the checkpoint states
            for (int i = 0; i < currentCheckpointState.Count; i++)
            {
                currentCheckpointState[i] = false;  // Mark all checkpoints as not set
            }

            // Check if the state was updated and invoke the event if necessary
            if (CheckpointsUpdated() && OnCheckpointUpdated != null)
            {
                OnCheckpointUpdated.Invoke();  // Notify listeners that checkpoints were reset
            }

            Logger.LogInfo("All checkpoints have been reset.");
        }
    }
}