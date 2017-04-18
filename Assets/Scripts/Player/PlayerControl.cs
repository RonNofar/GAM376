using UnityEngine;
using System.Collections;
using KRaB.Split.UI;
using KRaB.Split.Util;
using KRaB.Split.Player;

namespace KRaB.Split.Player
{
    public class PlayerControl : External.PlayerControl
    {
        [Header("Player Variables")]
        [SerializeField]
        private float maxHealth = 100f;
        [SerializeField]
        private float currentHealth = 100f;
        [SerializeField]
        private float minimumHeight = -100f;
        [HideInInspector]
        public bool isDead = false;
        
        private Transform myTransform;
        private Rigidbody2D myRigidbody;
        private Vector3 myPosition;

        [Header("Color Variables")]
        //public ColorManager.eColors color = ColorManager.eColors.Blue;
        [SerializeField]
        private OrbUIHandler orbUI;

        [Header("Bucket Variables")]
        [SerializeField]
        private GameObject bucket;

        private GrabController bucketScript;
        private SpriteRenderer bucketSR;

        [Header("Shovel Variables")]
        [SerializeField]
        private GameObject shovelPivot;
        [SerializeField]
        private float shovelLife = 1f;
        [SerializeField]
        private float shovelDelay = 0.5f;
        [SerializeField]
        private float halfFlapAngle = 20f;

        private Transform shovelPivotTransform;
        private float shovelAngle;
        private bool isShovel = false;

        [Header("Shovel Curve Variables")]
        [SerializeField]
        private float curveTime = 1f;
        [SerializeField]
        private float curveDampner = 4f; // 4 is recommended (maybe 3)

        private Transform bucketTransform;
        private Transform toShovelTransform;
        private float curveStartTime = 0f;
        private float distance = 5f;
        private bool isShovelCurve = false;

        [Header("Slash Variable")]
        [SerializeField]
        private GameObject slashPrefab;
        [SerializeField]
        private float slashLife = 1f;
        [SerializeField]
        private float slashDelay = 0.1f;
        [SerializeField]
        private RTool.FloatRange slashForce;
        [SerializeField]
        private RTool.FloatRange slashDistance;
        [SerializeField]
        private RTool.FloatRange slashScale;
        [SerializeField]
        private RTool.FloatRange slashAngle;

        private bool isSlash = false;
        private float currTime = 0f;

        [Header("Revive")]
        [SerializeField]
        private float reviveDelay = 3f;
        [SerializeField]
        private float reviveLength = 3f;
        [Tooltip("Make sure that reviveDampner is greater than 1")]
        [SerializeField]
        private float reviveDampner = 4f;
        [SerializeField]
        private float maximumUpwardForce = 10f;
        [SerializeField]
        private GameObject rightWing;
        [SerializeField]
        private GameObject leftWing;
        [SerializeField]
        private GameObject halo;
        [SerializeField]
        private float wingFlaps = 5f;
        [SerializeField]
        private float wingMaxAngle = 27.5f;

        private Transform rightWingTransform;
        private Transform leftWingTransform;
        private bool isRevive = false;
        private bool isReviveSequence = false;
        private float reviveStartTime = 0f;
        private float reviveTimeRatio = 0f;
        private Vector3 spawnPosition;

        [Header("Reject")]
        public float xForce = 12f;
        public float yForce = 25f;

        [Header("Damage")]
        [SerializeField]
        private SpriteRenderer[] sprites;
        [SerializeField]
        private float flashTime = 0.1f;
        [SerializeField]
        private Color flashColor = Color.red;

        private new AudioSource audio;

        [SerializeField]
        private KRaB.Enemy.Color.PrimaryColor[] orbColors;

        protected override void Awake()
        {
            base.Awake();
            // Setting up references.

            SetInitialReferences();
            UpdateBucketColor(orbColors[0]);
        }

        #region Initializers
        void SetInitialReferences()
        {
            try
            {
                myTransform = GetComponent<Transform>();
            }
            catch
            {
                Debug.Log("Error: unable to get Transform as component");
            }
            try
            {
                myRigidbody = GetComponent<Rigidbody2D>();
            }
            catch
            {
                Debug.Log("Error: unable to get Rigidbody2D as component");
            }
            if (shovelPivot != null)
            {
                try
                {
                    shovelPivotTransform = shovelPivot.GetComponent<Transform>();
                    shovelAngle = shovelPivotTransform.eulerAngles.z;
                }
                catch
                {
                    Debug.Log("Error: could not get Transform component from shovel object");
                }
            }
            else
            {
                Debug.Log("Error: reference to shovelPivot object is null");
            }
            if (bucket != null)
            {
                try
                {
                    bucketTransform = bucket.GetComponent<Transform>();
                    bucketSR = bucket.GetComponent<SpriteRenderer>();
                    bucketScript = bucket.GetComponent<GrabController>();
                }
                catch
                {
                    Debug.Log("Error: could not get Transform or SpriteRenderer or script component from bucket object");
                }
            }
            else
            {
                Debug.Log("Error: reference to bucket object is null");
            }
            if (rightWing != null)
            {
                try
                {
                    rightWingTransform = rightWing.GetComponent<Transform>();
                    rightWing.SetActive(false);
                }
                catch
                {
                    Debug.Log("Error: unable to get rightWingTransform as component");
                }
            }
            if (leftWing != null)
            {
                try
                {
                    leftWingTransform = leftWing.GetComponent<Transform>();
                    leftWing.SetActive(false);
                }
                catch
                {
                    Debug.Log("Error: unable to get leftWingTransform as component");
                }
            }
            halo.SetActive(false);
            spawnPosition = myTransform.position;
            audio = GetComponent<AudioSource>();
        }
        #endregion

        protected override void Update()
        {
            base.Update();
            //if (Input.GetButton("Fire2")) Suck();
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Joystick1Button2))//Input.GetButton("Fire1"))
            {
                if (!isShovel)
                {
                    isShovel = true;
                    //Attack();
                    Shovel();
                    currTime = Time.time;
                }
            }
            if (isShovel && Time.time > currTime + shovelDelay)
            {
                //Debug.Log("isShovel false");
                isShovel = false;
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button5))
            { // next orb (>>)
                if (orbColors[1] != null)
                {
                    RotateOrbColors(1);
                }
            }
            //Debug.Log(Input.GetAxis("ShiftLeft"));
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button4))
            { // previous orb (<<)
                if (orbColors[orbColors.Length - 1] != null)
                {
                    RotateOrbColors(-1);
                }
            }
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button6))
            {
                isRevive = true;
            }
            CheckForDeath();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isShovel)
            {
                ShovelAnimation();
            }
            else
            {
                ResetShovel();
            }
            if (isRevive)
            {
                Revive();
            }
        }

        void CheckForDeath()
        { // to be called in update
            if (myTransform.position.y <= minimumHeight) Death();
        }
        
        public void Death()
        { // is called in method called in update
            if (!isDead)
            {
                currentHealth = 0f;
                isDead = true;
                // death animation here
                StartCoroutine(
                    RTool.WaitAndRunAction(
                        reviveDelay,
                        () => { isRevive = true; }
                    )
                );
            }
        }

        public void Revive()
        {
            if (!isReviveSequence)
            {
                isDead = false;
                myTransform.position = spawnPosition;
                ZeroVelocity();
                reviveStartTime = Time.time;
                isReviveSequence = true;

                currentHealth = maxHealth;

                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;

                leftWing.SetActive(true);
                rightWing.SetActive(true);
                halo.SetActive(true);
            }
            else
            {
                reviveTimeRatio = (Time.time - reviveStartTime) / reviveLength;
                ZeroVelocity();
                myTransform.Translate(
                    new Vector2(
                        0f,
                        Mathf.Pow(
                            1 / reviveDampner,
                            reviveTimeRatio) * maximumUpwardForce// + -(Physics2D.gravity.y))
                        )
                    );
                leftWingTransform.localEulerAngles = new Vector3(
                    leftWingTransform.localEulerAngles.x,
                    leftWingTransform.localEulerAngles.y,
                    leftWingTransform.localEulerAngles.z + Mathf.Sin((wingFlaps) * 2 * Mathf.PI * reviveTimeRatio) * halfFlapAngle
                );
                rightWingTransform.localEulerAngles = new Vector3(
                    rightWingTransform.localEulerAngles.x,
                    rightWingTransform.localEulerAngles.y,
                    rightWingTransform.localEulerAngles.z + -(Mathf.Sin((wingFlaps) * 2 * Mathf.PI * reviveTimeRatio) * halfFlapAngle)
                );
                if (reviveTimeRatio >= 1)
                {
                    isRevive = false;
                    isReviveSequence = false;

                    GetComponent<BoxCollider2D>().enabled = true;
                    GetComponent<CircleCollider2D>().enabled = true;

                    leftWing.SetActive(false);
                    rightWing.SetActive(false);
                    halo.SetActive(false);
                }
            }
        }

        public void ZeroVelocity()
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
        }

        void Shovel()
        {
            isShovel = true;
            isShovelCurve = true;
            // call shovel handler

        }

        void ShovelAnimation()
        {
            float addAngle = (Mathf.Sin((Time.time - currTime) * (2 * Mathf.PI) / shovelDelay)) * halfFlapAngle; // sin from 0 to 1, time is x axis, 2pi (or full cicle) is delayTime
            //Debug.Log("Add: " + addAngle);
            //Debug.Log("Z: " + bucketTransform.localEulerAngles.z + " + " + addAngle + " | Total: " + (bucketTransform.localEulerAngles.z + addAngle));
            //addAngle = facingRight ? addAngle : -addAngle;
            if ((Time.time - currTime) > shovelDelay / 2) isShovelCurve = false;
            shovelPivotTransform.localEulerAngles = new Vector3(
                shovelPivotTransform.localEulerAngles.x,
                shovelPivotTransform.localEulerAngles.y,
                shovelPivotTransform.localEulerAngles.z + addAngle
            );
            //Debug.Log("Actual Z: " + bucketTransform.localEulerAngles.z);

        }

        void ResetShovel()
        {
            shovelPivotTransform.localEulerAngles = new Vector3(
                shovelPivotTransform.eulerAngles.x,
                shovelPivotTransform.eulerAngles.y,
                shovelAngle
            );
        }

        public void ApplyShovelCurve(GameObject toShovel)
        {
            Enemy.Slime es = toShovel.GetComponent<Enemy.Slime>();
            es.launch(ref bucketTransform, curveTime, curveDampner);
            //toShovelTransform = toShovel.GetComponent<Transform>();

            // x = ((1-t)^3)*P0X + 3*((1-t)^2)*t*P1X + 3(1-t)*(t^2)*P2X + (t^3)*P3X
            // float timeRatio = (Time.time - currTime) / shovelCurveTime
            //Debug.Log("Shoveled");
            //CurveThrow thrw = new CurveThrow(ref bucketTrans, ref toShovelTrans, 5f);
            //toShovel.GetComponent<Rigidbody2D>().AddForce(new Vector2(facingRight ? 12f : -12f, 25), ForceMode2D.Impulse);
        }

        IEnumerator DestroyAfterSecs(GameObject toDestroy, float secs)
        {
            yield return new WaitForSeconds(secs);
            Destroy(toDestroy);
        }

        public KRaB.Enemy.Color.PrimaryColor[] GetOrbArray()
        {
            return orbColors;
        }

        public void RotateOrbColors(int direction)
        { // direction > 0 = next, direction < 0 = prev. USE 1 and -1
            int l = orbColors.Length;
            if (direction > 0)
            { // next
                KRaB.Enemy.Color.PrimaryColor temp = orbColors[0]; // first
                for (int i = 0; i < l; ++i)
                {
                    if (i <= l - 2)
                    { // if before last in array
                        orbColors[i] = orbColors[i + 1];
                    }
                    else if (i == l - 1)
                    { // if last in array
                        orbColors[i] = temp;
                    }

                }
            }
            else if (direction < 0)
            { // previous
                KRaB.Enemy.Color.PrimaryColor temp = orbColors[l - 1]; // last
                for (int i = l - 1; i >= 0; --i)
                {
                    if (i > 0)
                    { // if before first in array
                        orbColors[i] = orbColors[i - 1];
                    }
                    else if (i == 0)
                    { // if first in array
                        orbColors[i] = temp;
                    }
                }
            }
            else
            {
                Debug.Log("Error: the method RotateOrbsColor received an unknown direction");
            }
            //Debug.Log(orbColors[0] + " " + orbColors[1] + " " + orbColors[2]);
            orbUI.UpdateOrbs(this);
            UpdateBucketColor(orbColors[0]);
        }

        public void UpdateBucketColor(KRaB.Enemy.Color.PrimaryColor color)
        {
            bucketSR.color = color.color;
            bucketScript.SetColor(color);
        }

        public bool GetIsShovel()
        {
            return isShovel && isShovelCurve;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public float GetHealthPercentage()
        {
            return currentHealth / maxHealth;
        }

        public void DamagePlayer(float damage)
        {
            if (currentHealth - damage < 0)
            {
                currentHealth = 0f;
                Death();
            }
            else
            {
                currentHealth -= damage;
                audio.Play();
                Color[] orgColor = new Color[sprites.Length];
                for (int i = 0; i < sprites.Length; ++i)
                {
                    orgColor[i] = sprites[i].color;
                    sprites[i].color = flashColor;
                }
                RTool.WaitAndRunAction(flashTime, () => {
                    for (int i = 0; i < sprites.Length; ++i)
                    {
                        sprites[i].color = orgColor[i];
                    }
                });
            }
        }

        public void SetSpawn(Vector3 position)
        {
            spawnPosition = position;
        }
    }

}
