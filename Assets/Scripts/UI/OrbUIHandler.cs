using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R;


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
        private PlayerControl player;

        void Start()
        {
            UpdateOrbs(player);
        }

        public void UpdateOrbs(PlayerControl player)
        {
            ColorManager.eColors[] orbs = player.GetOrbArray();
            int l = orbs.Length;
            mainOrb.color = ColorManager.GetColor(orbs[0]);
            prevOrb.color = ColorManager.GetColor(orbs[l - 1]);
            nextOrb.color = ColorManager.GetColor(orbs[1]);
        }
    }
}
