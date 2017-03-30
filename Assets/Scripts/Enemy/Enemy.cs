using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Enemy
{
    public abstract class Enemy : Entity
    {

        [Header("Runtime Linking", order = 101)]
        [SerializeField]
        protected AIMaster parent;
        public AIMaster Parent
        {
            get { return parent; }
            set
            {
                if (parent == null)
                {
                    parent = value;
                    parent.register(this);
                }
            }
        }

        public SlimeZone Zone
        {
            get;
            set;
        }

        protected Player.PlayerControl player;
        protected Transform playerTransform;

        private Util.GrabController cController; // catcher controller
        private Transform cTransform; // catcher Transform

        protected bool tossed { get; private set; }

        protected virtual void OnDestroy()
        {
            parent.deregister(this);
        }


        // Use this for initialization
        protected override void Start()
        {
            base.Start();

            tossed = false;

            //temp
            GameObject playerObject = GameObject.FindWithTag("Player");
            playerTransform = playerObject.GetComponent<Transform>();
            player = playerObject.GetComponent<Player.PlayerControl>();

            GameObject catcher = GameObject.FindWithTag("Bucket");
            cController = catcher.GetComponent<Util.GrabController>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

        }

        public void ApplyRejectForce()
        {
            myRigidbody.AddForce(new Vector2(player.facingRight ? -player.xForce : player.xForce, player.yForce), ForceMode2D.Impulse);
        }

        public void launch(ref Transform destination, float totalTime, float curveDamp)
        {
            cTransform = destination;
            StartCoroutine(ApplyShovelCurve(totalTime, curveDamp));
        }

        private IEnumerator ApplyShovelCurve(float totalTime, float curveDamp)
        {
            Vector3 originalPosition = transform.position;
            Vector3 distance = new Vector3(
                0f,
                (Vector3.Distance(originalPosition, cTransform.position)) / curveDamp,
                0f);

            tossed = true;
            float startTime = Time.time;
            float timeRatio = 0;

            while (timeRatio < 1)
            {
                timeRatio = (Time.time - startTime) / totalTime;
                timeRatio = (timeRatio > 1) ? 1 : timeRatio;
                transform.position = CurveThrow.CalculateBezierPoint(
                    timeRatio,
                    transform.position,
                    transform.position + distance,
                    cTransform.position,
                    cTransform.position + distance
                );
                yield return new WaitForFixedUpdate();
            }
            cController.AddToObjList(gameObject);
        }
    }
}
