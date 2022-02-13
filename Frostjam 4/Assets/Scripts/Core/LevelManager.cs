using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public static class LevelManager
    {
        public static int CurrentSceneIndex { get; private set; } = 0;

        public delegate void OnResetHandler();

        public static event OnResetHandler OnResetLevel;

        public static bool levelEnded;
        public static bool isGameFinished; // Is the game finished? (For "Thanks for playing" screen)

        static LevelManager()
        {
            OnResetLevel += () => levelEnded = false;
        }
        
        /// <summary>
        /// Load level by build index Additively
        /// </summary>
        /// <param name="sceneIndex">Build index of scene</param>
        public static void LoadLevel(int sceneIndex)
        {
            if (sceneIndex >= SceneManager.sceneCountInBuildSettings || sceneIndex <= 0) 
                throw new Exception($"{sceneIndex} Out of build range");

            if (SceneManager.sceneCount > 1)
            {
                Resources.UnloadUnusedAssets();
            }

            CurrentSceneIndex = sceneIndex;
            SceneManager.LoadScene(sceneIndex);
        }

        public static void ResetLevel()
        {
            SetSceneIndex();
            SceneManager.LoadScene(CurrentSceneIndex);
            OnResetLevel?.Invoke();
        }

        public static void SetSceneIndex()
        {
            CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
    }
}