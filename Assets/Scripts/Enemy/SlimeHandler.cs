using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Enemy
{
    public class SlimeHandler : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField]
        private UI.ColorManager.eColors color = UI.ColorManager.eColors.Black;

        public UI.ColorManager.eColors Color
        {
            get { return color; }
            set
            {
                color = value;
                mySpriteRenderer.color = UI.ColorManager.GetColor(color);
            }
        }

        [Header("Movement")]
        [SerializeField]
        private Util.RTool.FloatRange jumpDelay;
        [SerializeField]
        private Util.RTool.FloatRange jumpForce;
        [SerializeField]
        private float minimumVerticleJumpForce = 5f;
        [SerializeField]
        private float minimumHeight = -100f;

        private Transform myTransform;
        [SerializeField]
        private SpriteRenderer mySpriteRenderer;
        private Rigidbody2D myRigidBody;

        private Transform playerTransform;
        private GameObject catcher; // catcher
        private Util.GrabController cController; // catcher controller
        private Transform cTransform; // catcher Transform

        private float delayTime = 0f;
        private bool isBounce = false;

        private bool isCurve = false;
        private bool isStart = false;
        private Vector3 originalPosition = new Vector3(0f, 0f, 0f);
        private Vector3 distance = new Vector3(0f, 10f, 0f);
        private float startTime = 0f;
        private float totalTime = 1f;
        private float curveDampner = 4f;

        private void Awake()
        {
            SetInitialReferences();
            Color = color;
        }

        private void SetInitialReferences()
        {
            myTransform = GetComponent<Transform>();
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            myRigidBody = GetComponent<Rigidbody2D>();

            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
            catcher = GameObject.FindWithTag("Bucket");
            cController = catcher.GetComponent<Util.GrabController>();
            cTransform = catcher.GetComponent<Transform>();
        }

        private void Update()
        {
            if (myTransform.position.y < minimumHeight)
            {
                Destroy(gameObject);
            }

            if (isStart)
            {
                ApplyShovelCurve(totalTime);
            }
            else if (!isStart && isCurve)
            {
                isCurve = false;
            }
        }

        private void FixedUpdate()
        {
            if(!isBounce)
            {
                Bounce();
                delayTime = Time.time + jumpDelay.RandomInRange;
            }
            else if (isBounce)
            {
                if (Time.time > delayTime)
                {
                    isBounce = false;
                }
            }
            
        }

        private void Bounce()
        {
            myRigidBody.AddForce(new Vector2(
                    (playerTransform.position.x - myTransform.position.x) * jumpForce.RandomInRange,
                    (playerTransform.position.y - myTransform.position.y) * jumpForce.RandomInRange + minimumVerticleJumpForce
                ),
                ForceMode2D.Impulse
            );
            //Debug.Log("Bounce");
            isBounce = true;
        }

        private void ApplyShovelCurve(float totalTime)
        {
            if (!isCurve)
            {
                originalPosition = myTransform.position;
                distance = new Vector3(0f, (Vector3.Distance(originalPosition, cTransform.position)) / curveDampner, 0f);
                startTime = Time.time;
                isCurve = true;
            }

            float timeRatio = (Time.time - startTime) / totalTime;
            timeRatio = (timeRatio > 1) ? 1 : timeRatio;
            myTransform.position = CurveThrow.CalculateBezierPoint(
                timeRatio,
                myTransform.position,
                myTransform.position + distance,
                cTransform.position,
                cTransform.position + distance
            );

            if (timeRatio == 1)
            {
                isStart = false;
                cController.AddToObjList(gameObject);
            }
        }

        public void SetCatcherTransform(ref Transform t)
        {
            cTransform = t;
        }

        public void SetTotalTime(float t)
        {
            totalTime = t;
        }

        public void SetIsStart(bool b)
        {
            isStart = b;
        }

        public void SetCurveDampner(float f)
        {
            curveDampner = f;
        }
    }
}
