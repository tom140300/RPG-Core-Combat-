using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experiences experiences;

        private void Awake()
        {
            experiences = GameObject.FindWithTag("Player").GetComponent<Experiences>();
           
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}", experiences.GetPoint());
        }
    }
}