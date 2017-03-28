using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.UI
{
    public interface ColoredObject
    {
        ColorManager.eColors Color { get; set; }
    }
}
