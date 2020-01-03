using UnityEngine;
using NavGame.Managers;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public GameObject GetPlayer() {
        return GameObject.FindGameObjectWithTag("Player");
    }    

    public void KillPlayer() {
        NavigationManager.instance.ReloadCurrentScene();
    }
}
