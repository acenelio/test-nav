using UnityEngine;
using UnityEngine.SceneManagement;

namespace NavGame.Managers
{
    public class NavigationManager : MonoBehaviour
    {
        public static NavigationManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void ReloadCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            LoadScene(currentScene.name);
        }
    }
}