using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KRaB.Split.Enemy
{
    public class AIMaster : MonoBehaviour
    {
        public Util.FloatRange JumpHeight;
        public Util.FloatRange JumpWidth;

        [SerializeField]
        private GameObject SpawnerPrefab;

        private List<Enemy> active = new List<Enemy>();

        [SerializeField]
        private int maxSpawns;

        public bool spawn { get { return maxSpawns > (active.Count); } }


        // Use this for initialization
        void Start()
        {
            SlimeSpawner[] children = GetComponentsInChildren<SlimeSpawner>();
            foreach(SlimeSpawner c in children)
            {
                c.Parent = this;
            }
            Transform[] child = GetComponentsInChildren<Transform>();
            Debug.Log(child.Length);
            
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
