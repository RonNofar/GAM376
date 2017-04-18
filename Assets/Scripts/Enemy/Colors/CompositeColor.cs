using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Enemy.Color
{
    [CreateAssetMenu(order = 12)]
    public class CompositeColor : EnemyColor
    {
        [SerializeField]
        protected List<PrimaryColor> components;
        [SerializeField]
        List<DecompositionPair> decomposition;

        public static bool operator ==(CompositeColor c, EnemyColor o)
        {
            if (o is CompositeColor)
            {
                foreach (PrimaryColor comp in ((CompositeColor)o).components)
                {
                    if (c.components.Contains(comp))
                        continue;
                    return false;
                }
                return true;
            }
            if (o is PrimaryColor)
            {
                return c.components.Contains((PrimaryColor)o);
            }
            return false;
        }

        public static bool operator !=(CompositeColor c, EnemyColor o)
        {
            return !(c == o);
        }

        public static EnemyColor operator -(CompositeColor c, EnemyColor o)
        {
            if (o is PrimaryColor)
            {
                for(int i = 0;i<c.decomposition.Count;i++)
                {
                    if (c.decomposition[i].removed == o)
                    {
                        return c.decomposition[i].remaining;
                    }
                }
            }
            return c;
        }
    }
}
