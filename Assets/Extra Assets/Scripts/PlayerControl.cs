using UnityEngine;
using System.Collections;
using KRaB.Split.UI;
using KRaB.Split.Util;
using KRaB.Split.Player;

namespace KRaB.Split.Player
{
    public class PlayerControl : MonoBehaviour
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

        [HideInInspector]
        public bool facingRight = true;         // For determining which way the player is currently facing.
        [HideInInspector]
        public bool jump = false;               // Condition for whether the player should jump.


        public float moveForce = 365f;          // Amount of force added to move the player left and right.
        public float maxSpeed = 5f;             // The fastest the player can travel in the x axis.
        public AudioClip[] jumpClips;           // Array of clips for when the player jumps.
        public float jumpForce = 1000f;         // Amount of force added when the player jumps.
        public AudioClip[] taunts;              // Array of clips for when the player taunts.
        public float tauntProbability = 50f;    // Chance of a taunt happening.
        public float tauntDelay = 1f;           // Delay for when the taunt should happen.


        private int tauntIndex;                 // The index of the taunts array indicating the most recent taunt.
        private Transform groundCheck;          // A position marking where to check if the player is grounded.
        private bool grounded = false;          // Whether or not the player is grounded.
        private Animator anim;                  // Reference to the player's animator component.

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

        private ColorManager.eColors[] orbColors =
        { // 0 -> current, 1 -> next, last -> prev
            ColorManager.eColors.Blue,
            ColorManager.eColors.Red,
            ColorManager.eColors.Black
        };

        void Awake()
        {
            // Setting up references.
            
            SetInitialReferences();
            UpdateBucketColor(orbColors[0]);
        }

        #region Initializers
        void SetInitialReferences()
        {
            groundCheck = transform.Find("groundCheck");
            anim = GetComponent<Animator>();
            try
            {
                myTransform = GetComponent<Transform>();
            } catch {
                Debug.Log("Error: unable to get Transform as component");
            }
            try
            {
                myRigidbody = GetComponent<Rigidbody2D>();
            } catch {
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
                } catch {
                    Debug.Log("Error: unable to get rightWingTransform as component");
                }
            }
            if (leftWing != null)
            {
                try
                {
                    leftWingTransform = leftWing.GetComponent<Transform>();
                    leftWing.SetActive(false); 
                } catch {
                    Debug.Log("Error: unable to get leftWingTransform as component");
                }
            }
            halo.SetActive(false);
            spawnPosition = myTransform.position;
        }
        #endregion

        void Update()
        {
            //Debug.Log("Time: " + Time.time);
            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

            // If the jump button is pressed and the player is grounded then the player should jump.
            if (Input.GetButtonDown("Jump") && grounded)
                jump = true;
            //if (Input.GetButton("Fire2")) Suck();
            if (Input.GetButton("Fire1"))
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
            if (Input.GetKeyDown(KeyCode.E))
            { // next orb
                if (orbColors[1] != ColorManager.eColors.Black)
                {
                    RotateOrbColors(1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            { // previous orb
                if (orbColors[orbColors.Length - 1] != ColorManager.eColors.Black)
                {
                    RotateOrbColors(-1);
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                isRevive = true;
            }
            CheckForDeath();
        }

        void FixedUpdate()
        {
            // Cache the horizontal input.
            float h = Input.GetAxis("Horizontal");

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            anim.SetFloat("Speed", Mathf.Abs(h));

            // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
            if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
                // ... add a force to the player.
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

            // If the player's horizontal velocity is greater than the maxSpeed...
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
                // ... set the player's velocity to the maxSpeed in the x axis.
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (h > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (h < 0 && facingRight)
                // ... flip the player.
                Flip();

            // If the player should jump...
            if (jump)
            {
                // Set the Jump animator trigger parameter.
                anim.SetTrigger("Jump");

                // Play a random jump audio clip.
                int i = Random.Range(0, jumpClips.Length);
                AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

                // Add a vertical force to the player.
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

                // Make sure the player can't jump again until the jump conditions from Update are satisfied.
                jump = false;
            }
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

        void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public IEnumerator Taunt()
        {
            // Check the random chance of taunting.
            float tauntChance = Random.Range(0f, 100f);
            if (tauntChance > tauntProbability)
            {
                // Wait for tauntDelay number of seconds.
                yield return new WaitForSeconds(tauntDelay);

                // If there is no clip currently playing.
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    // Choose a random, but different taunt.
                    tauntIndex = TauntRandom();

                    // Play the new taunt.
                    GetComponent<AudioSource>().clip = taunts[tauntIndex];
                    GetComponent<AudioSource>().Play();
                }
            }
        }

        int TauntRandom()
        {
            // Choose a random index of the taunts array.
            int i = Random.Range(0, taunts.Length);

            // If it's the same as the previous taunt...
            if (i == tauntIndex)
                // ... try another random taunt.
                return TauntRandom();
            else
                // Otherwise return this index.
                return i;
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
            if (!isReviveSequence) {
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
            float addAngle = (Mathf.Sin((Time.time - currTime)*(2*Mathf.PI)/shovelDelay))*halfFlapAngle; // sin from 0 to 1, time is x axis, 2pi (or full cicle) is delayTime
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
            Enemy.SlimeHandler es = toShovel.GetComponent<Enemy.SlimeHandler>();
            es.SetCatcherTransform(ref bucketTransform);
            es.SetTotalTime(curveTime);
            es.SetCurveDampner(curveDampner);
            es.SetIsStart(true);
            //toShovelTransform = toShovel.GetComponent<Transform>();

            // x = ((1-t)^3)*P0X + 3*((1-t)^2)*t*P1X + 3(1-t)*(t^2)*P2X + (t^3)*P3X
            // float timeRatio = (Time.time - currTime) / shovelCurveTime
            //Debug.Log("Shoveled");
            //CurveThrow thrw = new CurveThrow(ref bucketTrans, ref toShovelTrans, 5f);
            //toShovel.GetComponent<Rigidbody2D>().AddForce(new Vector2(facingRight ? 12f : -12f, 25), ForceMode2D.Impulse);
        }

        void Attack()
        {
            // spawn sprite of slash with collider on side of player using facingRight
            // Figure out color coordination, probably better to hardcode everything
            isSlash = true;
            int isRight = (facingRight ? 1 : -1);
            myPosition = myTransform.position;
            GameObject newSlash = Instantiate(slashPrefab);
            Transform slashTransform = newSlash.GetComponent<Transform>();
            Rigidbody2D slashRigidbody = newSlash.GetComponent<Rigidbody2D>();
            slashTransform.localPosition = myPosition + new Vector3(slashDistance.RandomInRange,0f) * isRight;
            slashTransform.localScale = new Vector3(
                slashTransform.localScale.x * isRight,
                slashTransform.localScale.y
            );
            slashTransform.localRotation = new Quaternion(
                slashTransform.rotation.x,
                slashTransform.rotation.y,
                slashTransform.rotation.z + slashAngle.RandomInRange,
                slashTransform.rotation.w
            );
            slashRigidbody.AddForce(
                new Vector2(
                    slashForce.RandomInRange * isRight,
                    0f), 
                ForceMode2D.Impulse);
            newSlash.GetComponent<SlashHandler>().Color = (orbColors[0]);
            StartCoroutine(DestroyAfterSecs(newSlash, slashLife));
        }

        IEnumerator DestroyAfterSecs (GameObject toDestroy, float secs)
        {
            yield return new WaitForSeconds(secs);
            Destroy(toDestroy);
        }

        public ColorManager.eColors[] GetOrbArray()
        {
            return orbColors;
        }

        public void RotateOrbColors(int direction)
        { // direction > 0 = next, direction < 0 = prev. USE 1 and -1
            int l = orbColors.Length;
            if (direction > 0)
            { // next
                ColorManager.eColors temp = orbColors[0]; // first
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
                ColorManager.eColors temp = orbColors[l-1]; // last
                for (int i = l-1; i >= 0; --i)
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
            Debug.Log(orbColors[0]+" "+orbColors[1] + " " +orbColors[2]);
            orbUI.UpdateOrbs(this);
            UpdateBucketColor(orbColors[0]);
        }

        public void UpdateBucketColor(ColorManager.eColors color)
        {
            bucketSR.color = ColorManager.GetColor(color);
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
            return currentHealth/maxHealth;
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
            }
        }
    }
    
}
