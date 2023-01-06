using System;
using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
           
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<TextMeshProUGUI>().text = "N/A";
                return;
            }

            Health health = fighter.GetTarget();
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(),health.GetMaxHealthPoints());
        }
    }
}