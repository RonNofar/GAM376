using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KRaB.Split.UI;
using KRaB.Split.Util;

namespace KRaB.Split.Player
{
    public class PlayerControl : External.PlayerControl
    {
        static public PlayerControl Instance {  get { return _instance; } }
        static public PlayerControl _instance;

        [Header("Player Variables")]
        [SerializeField]
        private float maxHealth = 100f;
        [SerializeField]
        private float currentHealth = 100f;
        [SerializeField]
        private float minimumHeight = -100f;
        [SerializeField]
        private float maxVelocity = 20f;
        
        private Transform myTransform;
        private Rigidbody2D myRigidbody;
        private Vector3 myPosition;

        [Header("Color Variables")]
        //public ColorManager.eColors color = ColorManager.eColors.Blue;
        [SerializeField]
        private OrbUIHandler orbUI;
        [SerializeField]
        private KRaB.Enemy.Colors.PrimaryColor[] orbColors;

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
        private float flashTime = 0.1f;
        [SerializeField]
        private Color damageColor = Color.red;
        [SerializeField]
        private Color healColor = Color.green;
        [SerializeField]
        private float flashDampner = 100f;
        [SerializeField]
        private float HealthChangeTime = 2f;
        [SerializeField]
        private GameObject HealthChangeCanvas;
        [SerializeField]
        private float HealthChangeMovementScaler = 2f;
        [SerializeField]
        private float HealthChangeFadeTime = 0.5f;
        [SerializeField]
        private Transform HealthChangeStartTransform;

        [Header("Death")]
        [SerializeField]
        private GameObject gravestone; // gravestone object on character to interact with
        [SerializeField]
        private Vector2 gravestoneStart; // an vec3 referencing where the gravestone will start
        [SerializeField]
        private float gravestoneTime; // total time it takes for gravestone to fall
        [SerializeField]
        private float deathDelay; // amount of time before end of death delay

        private Vector2 gravestoneFinal; // original position of the gravestone

        private bool isStartFlashing = false;

        private new AudioSource audio;

        protected override void Awake()
        {
            base.Awake();
            // Setting up references.

            _instance = this;
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
            gravestoneFinal = gravestone.transform.localPosition;
            gravestone.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
            gravestone.transform.localPosition = gravestoneStart;
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
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("P");
                anim.SetBool("Invisible", true);
            }
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

            if (myRigidbody.velocity.magnitude > maxVelocity)
            {
                myRigidbody.velocity = myRigidbody.velocity.normalized * maxVelocity;
            }
        }

        void CheckForDeath()
        { // to be called in update
            if (myTransform.position.y <= minimumHeight)
            {
                ZeroVelocity();
                Death();
            }
        }
        
        public void Death()
        { // is called in method called in update
            if (!isDead)
            {
                isDead = true;
                currentHealth = 0f;
                
                StartCoroutine(DeathAnimation(
                    deathDelay, 
                    () => {
                        Debug.Log("isRevive = true");
                        isRevive = true;
                    })
                );

                /*
                StartCoroutine(
                    RTool.WaitAndRunAction(
                        reviveDelay,
                        () => { isRevive = true; }
                    )
                );
                */
            }
        }

        private IEnumerator DeathAnimation(float delay, UnityEngine.Events.UnityAction action)
        {
            Debug.Log("In DeathAnimation");
            gravestone.transform.localPosition = gravestoneStart;
            float startTime = Time.time;
            float timeRatio = 0f;

            gravestone.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            while (timeRatio <= 1)
            {
                timeRatio = (Time.time - startTime) / gravestoneTime;
                if (timeRatio > 1) timeRatio = 1;

                float temp = (1 / timeRatio) - 1;
                gravestone.transform.localPosition = Vector2.Lerp(gravestoneStart, gravestoneFinal, timeRatio);

                if (timeRatio == 1)
                {
                    Debug.Log("timeRatio = 1");
                    anim.SetBool("Invisible",true);
                    gravestone.transform.localPosition = gravestoneFinal;
                    if (Time.time > startTime + delay)
                    {
                        Debug.Log("Time delat met");
                        gravestone.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                        gravestone.transform.localPosition = gravestoneStart;
                        action.Invoke();
                        break;
                    }
                }

                yield return null;
            }
        }

        public void Revive()
        {
            Debug.Log("In Revive | isDead="+isDead );
            if (!isReviveSequence)
            {
                anim.SetBool("Invisible", false);
                currentHealth = maxHealth;
                isDead = false;
                myTransform.position = spawnPosition;
                ZeroVelocity();
                reviveStartTime = Time.time;
                /*RTool.WaitAndRunAction(1f,
                    () =>
                    {*/
                        isReviveSequence = true;

                        currentHealth = maxHealth;

                        GetComponent<Collider2D>().enabled = false;

                        leftWing.SetActive(true);
                        rightWing.SetActive(true);
                        halo.SetActive(true);/*
                    }
                );*/
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
                    isDead = false;
                    isRevive = false;
                    isReviveSequence = false;

                    GetComponent<Collider2D>().enabled = true;

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
            anim.SetTrigger("Shovel");
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
            if ((Time.time - currTime) > shovelDelay) isShovelCurve = false;
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

        public KRaB.Enemy.Colors.PrimaryColor[] GetOrbArray()
        {
            return orbColors;
        }

        public void RotateOrbColors(int direction)
        { // direction > 0 = next, direction < 0 = prev. USE 1 and -1
            int l = orbColors.Length;
            if (direction > 0)
            { // next
                KRaB.Enemy.Colors.PrimaryColor temp = orbColors[0]; // first
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
                KRaB.Enemy.Colors.PrimaryColor temp = orbColors[l - 1]; // last
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

        public void UpdateBucketColor(KRaB.Enemy.Colors.PrimaryColor color)
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

        public void Damage(float damage)
        {
            anim.SetTrigger("Damaged");
            if (currentHealth - damage <= 0)
            {
                StartCoroutine(HealthChange(0 - currentHealth));
                currentHealth = 0f;
                Death();
            }
            else
            {
                currentHealth -= damage;
                audio.Play();
            }
        }

        public void Heal(float amount)
        {
            anim.SetTrigger("Healed");
            float temp = currentHealth + amount;
            if (temp >= maxHealth)
            {
                //StartCoroutine(HealthChange(maxHealth - currentHealth));
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = temp;
                //StartCoroutine(HealthChange(amount));
            }
        }

        public void SetSpawn(Vector3 position)
        {
            spawnPosition = position;
        }

        private IEnumerator HealthChange(float amount)
        {
            isStartFlashing = false;
            float startTime = Time.time;
            float timeRatio = 0f;
            bool coroutineStarted = false;

            GameObject canvas = (GameObject)Instantiate(HealthChangeCanvas, HealthChangeStartTransform.position, HealthChangeStartTransform.rotation);
            Transform text = canvas.GetComponent<Transform>().GetChild(0).GetComponent<Transform>();
            text.GetComponent<Text>().text = (amount >= 0) ? "+ "+amount.ToString() : "- "+(-amount).ToString();
            text.GetComponent<Text>().color = (amount >= 0) ? Color.green : Color.red;

            while (timeRatio < 1)
            {
                if (isStartFlashing) break;
                timeRatio = (Time.time - startTime) / HealthChangeTime;
                if (Time.time >= startTime + HealthChangeFadeTime && !coroutineStarted)
                {
                    StartCoroutine(fadeOut(text.GetComponent<CanvasRenderer>()));
                    coroutineStarted = true;
                }
                if (timeRatio >= 1) timeRatio = 1;
                text.localPosition = new Vector3(
                    text.localPosition.x,
                    HealthChangeMovementScaler * timeRatio,
                    text.localPosition.z
                );
                yield return null;
            }
            Destroy(canvas);
        }
        
        IEnumerator fadeOut(CanvasRenderer cr)
        {
            float f_startTime = Time.time;
            float f_timeRatio = 0f;

            while (f_timeRatio < 1)
            {
                f_timeRatio = (Time.time - f_startTime) / HealthChangeFadeTime;
                if (f_timeRatio >= 1) f_timeRatio = 1;

                //Debug.Log(f_timeRatio);
                cr.GetComponent<CanvasRenderer>().SetAlpha(1-f_timeRatio);
                if (f_timeRatio == 1)
                {
                    break;
                }

                yield return null;
            }
        }
    }

}
