using BepInEx.Logging;
using UnityEngine;
using ECM.Components;
using System.Linq;
using System;
using System.Collections.Generic;
using BepInEx.Configuration;

namespace SpeedMod.Checkpoints{

    public class CheckpointManager
    {
        public event Action OnCheckpointUpdated;

        internal static ManualLogSource Logger;
        private Dictionary<int, Checkpoint> checkpoints;

        private static ConfigEntry<KeyboardShortcut> setCheckpoint1;
        private static ConfigEntry<KeyboardShortcut> setCheckpoint2;
        private static ConfigEntry<KeyboardShortcut> setCheckpoint3;

        private static ConfigEntry<KeyboardShortcut> loadCheckpoint1;
        private static ConfigEntry<KeyboardShortcut> loadCheckpoint2;
        private static ConfigEntry<KeyboardShortcut> loadCheckpoint3;
        private static ConfigEntry<KeyboardShortcut> resetCheckpoints;
        private static List<bool> lastCheckpointState;
        private static List<bool> currentCheckpointState;

        public CheckpointManager(ManualLogSource logger,
                                 ConfigEntry<KeyboardShortcut> sCheckpoint1,
                                 ConfigEntry<KeyboardShortcut> sCheckpoint2,
                                 ConfigEntry<KeyboardShortcut> sCheckpoint3,
                                 ConfigEntry<KeyboardShortcut> lCheckpoint1,
                                 ConfigEntry<KeyboardShortcut> lCheckpoint2,
                                 ConfigEntry<KeyboardShortcut> lCheckpoint3,
                                 ConfigEntry<KeyboardShortcut> rCheckpoints)
        {
            Logger = logger;
            Logger.LogDebug("Instanciating CheckpointManager");

            this.checkpoints = new Dictionary<int, Checkpoint>();

            this.checkpoints.Add(1, new Checkpoint(new Vector3(0, 0, 0), Quaternion.identity));
            this.checkpoints.Add(2, new Checkpoint(new Vector3(0, 0, 0), Quaternion.identity));
            this.checkpoints.Add(3, new Checkpoint(new Vector3(0, 0, 0), Quaternion.identity));

            setCheckpoint1 = sCheckpoint1;
            setCheckpoint2 = sCheckpoint2;
            setCheckpoint3 = sCheckpoint3;

            loadCheckpoint1 = lCheckpoint1;
            loadCheckpoint2 = lCheckpoint2;
            loadCheckpoint3 = lCheckpoint3;

            resetCheckpoints = rCheckpoints;


            currentCheckpointState = [
                false,
                false,
                false
            ];
            lastCheckpointState = new List<bool>(currentCheckpointState);
        }

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

        void SetCheckpoint(int checkpointNumber, Transform climberTransform){
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
        
        void LoadCheckpoint(int checkpointNumber, Transform climberTransform, CharacterMovement characterMovement){
            Logger.LogInfo("Loading checkpoint");

            climberTransform.position = checkpoints[checkpointNumber].position;
            climberTransform.rotation = checkpoints[checkpointNumber].rotation;

            characterMovement.velocity = new Vector3(0, 0, 0);
        }
        public bool IsCheckpointSet(int checkpointNumber) {
            bool set = this.checkpoints[checkpointNumber].checkpointSet;
            return set;
        }
        private void ResetCheckpoints()
        {
            Logger.LogInfo("Resetting all checkpoints");

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

        public void HandleInput(Transform climberTransform, CharacterMovement characterMovement)
        {
            // Set checkpoint when the player presses "F1"
            if (Input.GetKeyDown(setCheckpoint1.Value.MainKey))
            {
                SetCheckpoint(1, climberTransform);
            }
            if (Input.GetKeyDown(setCheckpoint2.Value.MainKey))
            {
                SetCheckpoint(2, climberTransform);
            }
            if (Input.GetKeyDown(setCheckpoint3.Value.MainKey))
            {
                SetCheckpoint(3, climberTransform);
            }


            if (Input.GetKeyDown(loadCheckpoint1.Value.MainKey))
            {
                LoadCheckpoint(1, climberTransform, characterMovement);
            }

            if (Input.GetKeyDown(loadCheckpoint2.Value.MainKey))
            {
                LoadCheckpoint(2, climberTransform, characterMovement);
            }

            if (Input.GetKeyDown(loadCheckpoint3.Value.MainKey))
            {
                LoadCheckpoint(3, climberTransform, characterMovement);
            }

            if (Input.GetKeyDown(resetCheckpoints.Value.MainKey))
            {
                ResetCheckpoints();
            }
        }
    }
}