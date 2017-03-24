using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KRaB.Split.UI
{
    public class HealthBarHandler : MonoBehaviour
    {
        [Header("Slider")]
        [SerializeField]
        private Slider slider;

        [Header("Text")]
        [SerializeField]
        private Text hpText;

        private Player.PlayerControl player;

        void Awake()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player.PlayerControl>();
        }

        void Update()
        {

        }

        private void FixedUpdate()
        {
            slider.value = player.GetHealthPercentage();
            hpText.text = "HP: " + player.GetCurrentHealth().ToString() + "/" + player.GetMaxHealth().ToString();
        }
    }
}
