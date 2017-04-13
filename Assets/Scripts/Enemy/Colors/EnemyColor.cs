using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Enemy.Color
{
    public abstract class EnemyColor : ScriptableObject
    {

        public UnityEngine.Color color;
        
    }

    [System.Serializable]
    public struct DecompositionPair
    {
        public PrimaryColor removed;
        public EnemyColor remaining;
    }

}
