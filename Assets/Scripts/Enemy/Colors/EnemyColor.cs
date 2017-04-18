using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Enemy.Color
{
    public abstract class EnemyColor : ScriptableObject
    {

        public UnityEngine.Color color;
        

        public static bool operator ==(EnemyColor c, EnemyColor o)
        {
            if (c is CompositeColor)
            {
                return (CompositeColor)c == o;
            }
            if (c is PrimaryColor)
            {
                if (o is PrimaryColor)
                {
                    return c.color==o.color;
                }
            }
            return false;

        }
        public static bool operator !=(EnemyColor c, EnemyColor o)
        {
            return !(c == o);
        }
        public static EnemyColor operator -(EnemyColor c, EnemyColor o)
        {
            if(c is CompositeColor)
            {
                return (CompositeColor)c - o;
            }
            if (c == o)
            {
                Debug.Log("subtract to null");
                return null;
            }
            return c;
        }
    }

    [System.Serializable]
    public struct DecompositionPair
    {
        public PrimaryColor removed;
        public EnemyColor remaining;
    }

}
