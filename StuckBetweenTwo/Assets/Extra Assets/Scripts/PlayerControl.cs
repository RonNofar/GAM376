﻿using UnityEngine;
using System.Collections;

namespace R
{
    public class PlayerControl : MonoBehaviour
    {
        [Header("Player Variables")]
        [SerializeField]
        private float playerHealth = 100f;

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
        private Rigidbody myRigidbody;
        private Vector3 myPosition;

        [Header("Color Variables")]
        public ColorManager.eColors color = ColorManager.eColors.Blue;
        [SerializeField]
        private OrbUIHandler orbUI;

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

        private ColorManager.eColors[] orbColors =
        { // 0 -> current, 1 -> next, last -> prev
            ColorManager.eColors.Blue,
            ColorManager.eColors.Red,
            ColorManager.eColors.Black
        };

        void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("groundCheck");
            anim = GetComponent<Animator>();
            try
            {
                myTransform = GetComponent<Transform>();
            } catch {
                Debug.Log("Error: unable to get Transform as component");
            }
        }


        void Update()
        {
            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

            // If the jump button is pressed and the player is grounded then the player should jump.
            if (Input.GetButtonDown("Jump") && grounded)
                jump = true;
            if (Input.GetButton("Fire2")) Suck();
            if (Input.GetButton("Fire1"))
            {
                if (!isSlash)
                {
                    isSlash = true;
                    Attack();
                    currTime = Time.time;
                }
                if (isSlash && Time.time > currTime + slashDelay)
                {
                    isSlash = false;
                }
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

        void Suck()
        {

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
            newSlash.GetComponent<SlashHandler>().SetColor(orbColors[0]);
            StartCoroutine(DestroyAfterSecs(newSlash, slashLife));
        }
        IEnumerator DestroyAfterSecs (GameObject toDestroy, float secs)
        {
            yield return new WaitForSeconds(secs);
            Destroy(toDestroy);
        }

        public void DamagePlayer(float damage)
        {
            if (playerHealth-damage<0)
            {
                playerHealth = 0f;
            } 
            else
            {
                playerHealth -= damage;
            }
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
        }
        
    }
    
}
