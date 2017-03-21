using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.UI
{
    public class ColorManager : MonoBehaviour
    {
        public enum eColors
        {
            Black = 0, // null
            Blue = 0x001,
            Red = 0x002,
            Yellow = 0x004,
            Purple = Red | Blue,
            Green = Blue | Yellow,
            Orange = Red | Yellow,
            Brown = Red | Yellow | Blue
        }
        public static Dictionary<eColors, Color> dColors = new Dictionary<eColors, Color>() {
            { eColors.Black, Color.black },
            { eColors.Blue,  Color.blue  },
            { eColors.Red,   Color.red   }
        };

        public static Color GetColor(eColors color)
        {
            return dColors[color];
        }
    }
}
