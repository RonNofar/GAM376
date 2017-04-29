using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KRaB.Split.UI
{
    public class HeartHandler : MonoBehaviour
    {
        [Header("Heart Reference")]
        [SerializeField]
        private Image heart;

        [Header("Heart Images")]
        [SerializeField]
        private Sprite[] lives;

        private Player.PlayerControl player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player.PlayerControl>();
            if (heart == null) heart = GetComponent<Image>();
        }

        private void FixedUpdate()
        {
            float hp = player.GetHealthPercentage();
            int temp = (hp == 0) ? 0 : (int)Mathf.Ceil(player.GetHealthPercentage() * 10);
            //Debug.Log(temp);

            heart.sprite = lives[temp];
        }
    }
}
