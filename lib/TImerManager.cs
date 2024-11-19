using UnityEngine;


namespace SpeedMod.Timer {
    public class TimerManager : MonoBehaviour {
        private LevelStats levelStatsInstance;
        public float GetStartTime() {    
            LevelStats[] levelStatsInstances = FindObjectsOfType<LevelStats>();
            levelStatsInstance = levelStatsInstances[0];
            return levelStatsInstance.startTime;
        }

        public void UpdateTimer(float time, float startTime) {
            LevelStats[] levelStatsInstances = FindObjectsOfType<LevelStats>();
            levelStatsInstance = levelStatsInstances[0];
            levelStatsInstance.startTime = Time.time - (time - startTime);
        }
    }
}
