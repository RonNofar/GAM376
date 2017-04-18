using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KRaB.Split.Enemy
{
    [CreateAssetMenu]
    public class AIMaster : ScriptableObject
    {
        [Header("Jumping")]
        public Util.FloatRange JumpHeight;
        public Util.FloatRange JumpWidth;
        [SerializeField]
        private float searchDistance;
        public float SearchDistance { get { return searchDistance; } }


        [Header("Damage")]
        [SerializeField]
        private int damage;
        [SerializeField]
        private Util.RTool.FloatRange damageDelay;

        public int Damage { get { return damage; } }
        public float DamageDelay { get { return damageDelay.RandomInRange; } }

        [Header("Spawning")]
        [SerializeField]
        private GameObject SpawnerPrefab;

        private List<Enemy> active = new List<Enemy>();

        [SerializeField]
        private int maxSpawns;

        public bool spawn { get { return maxSpawns > (active.Count); } }


        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Vector2 clampJump(Vector2 jump)
        {
            return new Vector2(
                JumpWidth.clamp(jump.x),
                JumpHeight.clamp(jump.y));
        }

        public void register(Enemy s)
        {
            active.Add(s);
        }
        public void deregister(Enemy s)
        {
            active.Remove(s);
        }
    }
}
