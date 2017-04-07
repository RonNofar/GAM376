﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split.Tutorial
{
    public class MixTutorial : Tutorial
    {
        [Header("To Unlock")]
        [SerializeField]
        private GameObject unlock;

        protected override void Awake()
        {
            base.Awake();
            unlock.SetActive(false);
        }

        protected override void OnTriggerStay2D (Collider2D collision)
        {
            base.OnTriggerStay2D(collision);
            if (GameObject.Find("slime(purple)") == null)
            {
                Unlock();
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("Unlock");
            base.OnTriggerExit2D(collision);
        }

        private void Unlock()
        {
            unlock.SetActive(true);
        }
    }
}