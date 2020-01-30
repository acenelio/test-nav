using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavGame.Core;

public class KamikazeContact : MonoBehaviour
{
    public OnReachBossAreaEvent OnReachBossArea;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BossArea")
        {
            if (OnReachBossArea != null)
            {
                OnReachBossArea();
            }
        }
    }
}
