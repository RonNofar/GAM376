using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Util
{
    public class GrabController : MonoBehaviour
    {

        [SerializeField]
        private GameObject toFollowObject;

        public bool X = false;
        public bool Y = false;
        public bool Z = false;

        [Header("Main Object Values")]
        [SerializeField]
        [Tooltip("Keep minimum distance less than maximum speed")]
        private float minDistance = 0.1f;
        [SerializeField]
        private float maxDistance = 0.5f;
        [Tooltip("Keep minimum speed low")]
        [SerializeField]
        private float minSpeed = 0.01f;
        [SerializeField]
        private bool enableMaxSpeed = false;
        [SerializeField]
        private float maxSpeed = 2f;
        [SerializeField]
        private bool isDeltaTime = true;

        [Header("Grabbing Values")]
        [SerializeField]
        private float grabDelay = 1f;
        [SerializeField]
        [Tooltip("Keep minimum distance less than maximum speed")]
        private float grabMinDistance = 0.1f;
        [SerializeField]
        private float grabMaxDistance = 10f;
        [Tooltip("Keep minimum speed low")]
        [SerializeField]
        private float grabMinSpeed = 0.01f;
        [SerializeField]
        private float grabMaxSpeed = 2f;

        [Header("Other Stuff")]
        [SerializeField]
        private GameObject targetSpritePrefab;
        [SerializeField]
        private KRaB.Enemy.Colors.PrimaryColor color;

        private Transform myTransform;
        private Transform toFollow;

        private GameObject hitObj;
        private Transform hitTransform;

        private GameObject tempTargetSprite;

        private bool isGrabbing = false;
        private bool isDoneGrabbing = false;
        private bool isTargetAnimation = false;
        private bool isTargetAnimationStart = false;

        private Transform targetSpriteTransform;

        private List<GameObject> objs = new List<GameObject>();

        private AudioSource[] audio;

        private void Awake()
        {
            SetInitialReferences();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("MouseButtonDown(0)");
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    hitObj = hit.collider.gameObject;
                    hitTransform = hitObj.GetComponent<Transform>();
                    if (hitTransform.tag == "PickUp")
                    {
                        //isGrabbing = true;
                        isTargetAnimation = true;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (!isGrabbing)
            { // if on player
                ObjectFollow.FollowObject(
                    myTransform,
                    toFollow,
                    minDistance,
                    maxDistance,
                    minSpeed,
                    enableMaxSpeed,
                    maxSpeed,
                    isDeltaTime,
                    X,
                    Y,
                    Z
                );
            }
            else if (isGrabbing)
            {

            }
            /*else if (isGrabbing)
            { // if player indicates grab
                bool isMoving = ObjectFollow.FollowObject(
                    myTransform,
                    hitTransform,
                    grabMinDistance,
                    grabMaxDistance,
                    grabMinSpeed,
                    true,
                    grabMaxSpeed,
                    true,
                    true,
                    true,
                    false
                );
                if (!isMoving)
                { // and grabber not moving
                    StartCoroutine(GrabDelay(grabDelay));
                } // and isTargetAnimation triggerred (== true)
                if (isTargetAnimation)
                {
                    if (!isTargetAnimationStart)
                    {
                        tempTargetSprite = Instantiate(targetSpritePrefab, hitTransform);
                        targetSpriteTransform = targetSpritePrefab.GetComponent<Transform>();
                        isTargetAnimationStart = true;
                    }
                    if (!isDoneGrabbing)
                    {
                        float scaleValue = Mathf.Sin(Time.time) / 4 + 0.5f;
                        targetSpriteTransform.localScale = new Vector3(
                            scaleValue,
                            scaleValue,
                            0f
                        );
                    } else
                    {
                        Destroy(tempTargetSprite);
                        isDoneGrabbing = false;
                    }
                }
            }*/
        }

        private void SetInitialReferences()
        {
            if (toFollowObject == null)
            {
                Debug.Log("Error: toFollowObject is null");
            }
            else
            {
                try
                {
                    toFollow = toFollowObject.GetComponent<Transform>();
                    //toFollowPosition = toFollow.position;
                }
                catch
                {
                    Debug.Log("Error: Could not reference toFollowObject transform.");
                }
            }
            try
            {
                myTransform = GetComponent<Transform>();
                hitTransform = myTransform; // for null reference exceptions
            }
            catch { Debug.Log("Error: Could not reference gameObject's transform."); }
            try
            {
                targetSpriteTransform = targetSpritePrefab.GetComponent<Transform>();
            }
            catch
            {
                Debug.Log("Error: Could not reference targetSprite's transform");
            }
            audio = GetComponents<AudioSource>();
        }

        private IEnumerator GrabDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            isGrabbing = false;
            isDoneGrabbing = true;
        }

        public void SetColor(KRaB.Enemy.Colors.PrimaryColor c)
        {
            color = c;
        }

        public void SetIsGrabbing(bool b)
        {
            isGrabbing = b;
        }

        public void AddToObjList(GameObject obj)
        {
            //if (!objs.Contains(obj)) objs.Add(obj);
            Enemy.Slime s = obj.GetComponent<Enemy.Slime>();
            if ((s.ColorData == color))
            {
                audio[0].Play();
                KRaB.Enemy.Colors.EnemyColor c = s.ColorData - color;
                if (c == (MonoBehaviour)null)
                {
                    Destroy(obj);
                    return;
                }
                s.ColorData = c;
            }
            audio[1].Play();
            obj.GetComponent<Enemy.Slime>().ApplyRejectForce();
        }

        /*
        private IEnumerator AnalyseObject()
        {
            yield return new null;
        }*/
    }
}
