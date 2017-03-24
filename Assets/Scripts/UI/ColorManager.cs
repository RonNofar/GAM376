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
            { eColors.Red,   Color.red   },
            { eColors.Yellow, Color.yellow },
            { eColors.Purple, (Color.red/2+Color.blue/2) },
            { eColors.Green, Color.green },
            {eColors.Orange,(Color.red/2 + Color.yellow/2) },
            {eColors.Brown, (Color.red/3+Color.yellow/3+Color.blue/3) }
        };

        public static Color GetColor(eColors color)
        {
            /*
            Color o = new Color(1,1,1);
            Color g = new Color(0.25f, 0.25f, 0.2f);
            Color c0 = new Color(0.1f, 0.1f, 0.8f);
            Color c1 = new Color(1.0f, 0.5f, 0.1f);
            Color c2 = new Color(0.75f,0.5f,0.1f);
            if ((color & eColors.Blue) != 0)
                o *= c0+g;
            if ((color & eColors.Red) != 0)
                o *= c1+g;
            if ((color & eColors.Yellow) != 0)
                o *= c2+g;
            return o;
            /*/
            return dColors[color];
            //*/
        }
    }
}
