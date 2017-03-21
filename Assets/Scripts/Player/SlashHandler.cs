using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KRaB.Split.UI;

namespace KRaB.Split.Player
{
    public class SlashHandler : MonoBehaviour
    {

        [Header("Color")]
        [SerializeField]
        private ColorManager.eColors color = ColorManager.eColors.Blue;
        [SerializeField]
        private SpriteRenderer sprite;

        public ColorManager.eColors Color
        {
            get { return color; }
            set
            {
                color = value;
                UpdateColor();
            }
        }
        void Start()
        {
            UpdateColor();
        }

        void Update()
        {

        }
        
        public ColorManager.eColors GetColor()
        {
            return color;
        }

        public void UpdateColor()
        {
            sprite.color = ColorManager.GetColor(color);
        }
    }
}
