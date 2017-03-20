using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace R
{
    public class ColorManager : MonoBehaviour
    {
        public enum eColors
        {
            Black = 0, // null
            Blue,
            Red
        }
        public static Dictionary<string, Color> dColors = new Dictionary<string, Color>() {
            { "Black", Color.black },
            { "Blue",  Color.blue  },
            { "Red",   Color.red   }
        };

        public static Color GetColor<T>(T color)
        {
            if (color.GetType() == typeof(string))
            {
                return dColors[(string)(object)color];
            }
            else if (color.GetType() == typeof(eColors))
            {
                if ((eColors)(object)color == eColors.Blue)
                {
                    return dColors["Blue"];
                }
                else if ((eColors)(object)color == eColors.Red)
                {
                    return dColors["Red"];
                }
            }
            return dColors["Black"];
        }
    }
}
