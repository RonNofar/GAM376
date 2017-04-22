using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using KRaB.Split.UI;

namespace KRaB.Split.Enemy
{
    public class enemyScript : MonoBehaviour
    {
        [SerializeField]
        private ColorManager.eColors colorType;
        [SerializeField]
        private float damage = 10f;
        [SerializeField]
        private float yMin = -100f;

        public Player.PlayerControl player;
        public Util.GrabController bucket;

        private Color color;

        private Transform myTransform;
        private Transform cTransform;

        private bool isStart = false;

        private bool isCurve = false;
        private Vector3 distance = new Vector3(0,10f,0);
        private float startTime = 0f;
        private float totalTime = 5f;
        private float curveDampner = 4f;
        private Vector3 originalPosition;

        private SpriteRenderer enemySR;
        private ColorManager.eColors lastColor = ColorManager.eColors.Black;

        private void Awake()
        {
            InitializeEnemy();
        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.transform.position.y < yMin)
            {
                Destroy(gameObject);
            }
            if(isStart)
            {
                ApplyShovelCurve(totalTime);
            }
            else if (!isStart && isCurve)
            {
                isCurve = false;
            }
            if (lastColor != colorType)
            {
                lastColor = colorType;
                UpdateSpriteRendererColor(colorType);
            }
        }

        private void OnCollisionEnter2D(Collision2D col) // trigger might not work with this
        {
            //Debug.Log("Collision: "+col.transform.name+" | Tag: "+ col.transform.tag);
            Transform colTransform = col.gameObject.GetComponent<Transform>();
            if (colTransform.gameObject.tag == "slash")
            {
                // put thing here
                Debug.Log("In there fam");
                Destroy(gameObject);
            }
            else if (colTransform.gameObject.tag == "Player")
            {
                player.Damage(damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Transform colTransform = col.gameObject.GetComponent<Transform>();
            /*
            Player.SlashHandler colObj = colTransform.GetComponent<Player.SlashHandler>();
            if (colObj.tag != null)
            {
                if (colorType == colObj.Color)
                {
                    // put thing here
                    Destroy(gameObject);
                }
            }
            */
        }

        private void InitializeEnemy()
        {
            enemySR = GetComponent<SpriteRenderer>();
            //UpdateSpriteRendererColor(colorType);
            player = GameObject.FindWithTag("Player").GetComponent<Player.PlayerControl>();
            myTransform = GetComponent<Transform>();
            bucket = GameObject.FindWithTag("Bucket").GetComponent<Util.GrabController>();
        }

        public void ApplyShovelCurve(float totalTime)
        {
            if (!isCurve)
            {
                originalPosition = myTransform.position;
                distance = new Vector3(0f, (Vector3.Distance(originalPosition, cTransform.position))/curveDampner, 0f);
                //Debug.Log(Vector3.Distance(originalPosition, cTransform.position));
                startTime = Time.time;
                isCurve = true;
            }
            //Debug.Log(myTransform.position + distance);
            float timeRatio = (Time.time - startTime) / totalTime;
            timeRatio = (timeRatio > 1) ? 1 : timeRatio;
            myTransform.position = CurveThrow.CalculateBezierPoint(
                timeRatio,
                myTransform.position,
                myTransform.position + distance,
                cTransform.position,
                cTransform.position + distance
            );

            if(timeRatio == 1)
            {
                isStart = false;
                bucket.AddToObjList(gameObject);
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


        public ColorManager.eColors GetEnumColor()
        {
            return colorType;
        }

        public void UpdateSpriteRendererColor(ColorManager.eColors color)
        {
            //Debug.Log("Update to: " + color + " | Time: " + Time.time);
            colorType = color;
            enemySR.color = ColorManager.GetColor(color);
        }
    }
}
