using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject WarningPanel;
    Text WarningText;

    protected virtual void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        WarningText = WarningPanel.GetComponentInChildren<Text>();
    }

    public void ShowWarning(string message, float delay) 
    {
        WarningPanel.SetActive(true);
        WarningText.text = message;
        StartCoroutine(HideWarningPanel(delay));
    }

    IEnumerator HideWarningPanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        WarningPanel.SetActive(false);
    }
}
