using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay.Tutorials
{
    public static class TutorialService
    {
        private static Dictionary<string, GameObject> _spawnedTutorials = new();
        
        public static void ShowTutorial(string tutorialId, bool ignoreMemory = false)
        {
            if(PlayerPrefs.GetInt($"tutorial_played_{tutorialId}") == 1 && !ignoreMemory)
                return;
            
            var config = Resources.Load<TutorialConfig>($"Gameplay/Tutorials/{tutorialId}");
            if (config == null) return;
            if(config.TutorialPrefab == null) return;

            if (_spawnedTutorials.ContainsKey(tutorialId))
            {
                if (!_spawnedTutorials[tutorialId])
                    _spawnedTutorials.Remove(tutorialId);
                else
                    return;                    
            }
            
            var obj = GameObject.Instantiate(config.TutorialPrefab);
            _spawnedTutorials.Add(tutorialId, obj);

            PlayerPrefs.SetInt($"tutorial_played_{tutorialId}", 1);
        }
    }
}
