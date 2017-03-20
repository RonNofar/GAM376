using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R
{
    public class SlashHandler : MonoBehaviour {

        [Header("Color")]
        [SerializeField]
        private ColorManager.eColors color = ColorManager.eColors.Blue;
        [SerializeField]
        private SpriteRenderer sprite;

        void Start() {
            UpdateColor();
        }

        void Update() {

        }

        public void SetColor(ColorManager.eColors c)
        {
            color = c;
            UpdateColor();
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
