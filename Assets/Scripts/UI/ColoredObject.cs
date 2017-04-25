using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.UI
{
    public interface ColoredObject
    {
        KRaB.Enemy.Colors.EnemyColor ColorData { get; set; }
    }
}
