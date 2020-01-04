using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NavGame.Core
{
    [RequireComponent(typeof(Character))]
    public class HealthUIController : MonoBehaviour
    {
        public GameObject HealthUIPrefab;
        public Transform HealthPosition;

        GameObject HealthUI;
        Image HealthSlider;
        Transform Cam;

        Character character;

        void Start()
        {
            Canvas canvas = FindWorldSpaceCamera();
            if (canvas == null)
            {
                throw new Exception("A WorldSpace Canvas is needed to use HealthUIController");
            }
            Cam = Camera.main.transform;
            HealthUI = Instantiate(HealthUIPrefab, canvas.transform);
            HealthSlider = HealthUI.transform.GetChild(0).GetComponent<Image>();
            //HealthUI.gameObject.SetActive(true);

            character = GetComponent<Character>();
            character.OnHealthChanged += UpdateHealthUI;
            character.OnDied += DestroyUI;
        }

        void LateUpdate()
        {
            if (HealthUI != null)
            {
                HealthUI.transform.position = HealthPosition.position;
                HealthUI.transform.forward = -Cam.forward;
            }
        }

        void UpdateHealthUI(int maxHealth, int currentHealth)
        {
            if (HealthUI != null)
            {
                //HealthUI.gameObject.SetActive(true); // ?

                float healthPercent = (float)character.CurrentHealth / maxHealth;
                HealthSlider.fillAmount = healthPercent;
            }
        }

        void DestroyUI()
        {
            if (HealthUI != null) 
            {
                Destroy(HealthUI);
            }
        }

        Canvas FindWorldSpaceCamera()
        {
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    return c;
                }
            }
            return null;
        }
    }
}
