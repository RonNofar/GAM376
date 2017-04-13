using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Enemy.Color {
    [CreateAssetMenu(order =12)]
    public class CompositeColor : EnemyColor
    {
        [SerializeField]
        List<EnemyColor> components;
        [SerializeField]
        List<DecompositionPair> decomposition;

        public static bool operator ==(CompositeColor c, EnemyColor o)
        {
            if (c == o)
            {
                return true;
            }
            if(o is PrimaryColor)
            {
                return c.components.Contains((PrimaryColor)o);
            }
            return false;

        }
        public static bool operator !=(CompositeColor c, EnemyColor o)
        {
            return !(c == o);
        }
        public static EnemyColor operator-(CompositeColor c,EnemyColor o)
        {
            if (c == o)
                return null;
            if(o is PrimaryColor)
            {
                foreach(DecompositionPair pair in c.decomposition)
                {
                    if(pair.removed == o)
                    {
                        return pair.remaining;
                    }
                }
            }
            return c;
        }
    }
}
