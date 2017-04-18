using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace KRaB.Split.UI
{
    public class OrbUIHandler : MonoBehaviour
    {

        [Header("Orb Images")]
        [SerializeField]
        private Image mainOrb;
        [SerializeField]
        private Image prevOrb;
        [SerializeField]
        private Image nextOrb;

        [Header("Player Controller")]
        [SerializeField]
        private Player.PlayerControl player;

        void Start()
        {
            UpdateOrbs(player);
        }

        public void UpdateOrbs(Player.PlayerControl player)
        {
            KRaB.Enemy.Color.PrimaryColor [] orbs = player.GetOrbArray();
            int l = orbs.Length;
            mainOrb.color = orbs[0].color;
            prevOrb.color = orbs[l - 1].color;
            nextOrb.color = orbs[1].color;
        }
    }
}
