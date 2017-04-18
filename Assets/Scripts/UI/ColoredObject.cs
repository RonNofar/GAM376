using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.UI
{
    public interface ColoredObject
    {
        KRaB.Enemy.Color.EnemyColor ColorData { get; set; }
    }
}
