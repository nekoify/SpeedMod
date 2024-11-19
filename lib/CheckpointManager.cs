using UnityEngine;
using ECM.Components;
using SpeedMod.Timer;

namespace SpeedMod.Checkpoints{

    public class CheckpointManager
    {
        // Event to trigger canvas redraw
      //  public event Action OnCheckpointUpdated;

        // Checkpoint dictionary structure
        private Dictionary<int, Checkpoint> checkpoints;


    

        private TimerManager timerManager;

        private int checkpointIndex = 0;

        public CheckpointManager()
        {
            // Melon<SpeedMod>.Logger.Msg("Instanciating CheckpointManager");

            this.checkpoints = new Dictionary<int, Checkpoint>();

            this.timerManager = new TimerManager();
        }
        /// <summary>
        /// Sets a checkpoint's position, rotation and time data.
        /// </summary>
        /// <param name="climberTransform">Transform of the player, contains rotation & position</param>

        public void SetCheckpoint(Transform climberTransform){
            checkpointIndex++;
            this.checkpoints.Add(checkpointIndex, new Checkpoint(climberTransform.position, climberTransform.rotation, Time.time, timerManager.GetStartTime()));
        }
        
        /// <summary>
        /// Load an existing checkpoint.
        /// </summary>
        /// <param name="climberTransform">Transform of the player, contains rotation & position.</param>
        /// <param name="characterMovement">Movement of the climber.</param>
        public void LoadCheckpoint(Transform climberTransform, CharacterMovement characterMovement){
            // Melon<SpeedMod>.Logger.Msg($"Loading checkpoint {checkpointNumber}");
                 if (checkpointIndex > 0) {
                climberTransform.position = checkpoints[checkpointIndex].position;
                climberTransform.rotation = checkpoints[checkpointIndex].rotation;

                characterMovement.velocity = new Vector3(0, 0, 0);

                timerManager.UpdateTimer(checkpoints[checkpointIndex].time, checkpoints[checkpointIndex].startTime);
                checkpoints[checkpointIndex].time = Time.time;
                checkpoints[checkpointIndex].startTime = timerManager.GetStartTime();
                 }
        }
         /// <summary>
        /// Delete the current checkpoint
        /// </summary>
         public void DeleteCheckpoint(){
        
            if (checkpointIndex > 0)
            {
                checkpoints.Remove(checkpointIndex);
                checkpointIndex--;
            }
        }
        
        /// <summary>
        /// Wipe checkpoints
        /// </summary>
        public void ResetCheckpoints()
        {
            checkpoints.Clear();
            // Melon<SpeedMod>.Logger.Msg("All checkpoints have been reset.");
        }
    }
}
