using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KRaB.Split
{
    public class CurveThrow
    {

        private Transform m_ball;
        private Transform m_catcher;

        private float totalTime;

        private Vector3 bPosition;
        private Vector3 cPosition;

        private Vector3 bPosition_org; // original
        private float distance;

        private float startTime;



        private float timeRatio = 0f;

        private bool isFinished = false;

        public CurveThrow(ref Transform ball, ref Transform catcher, float totalTime)
        {
            Debug.Log("Instantiated");
            m_ball = ball;
            m_catcher = catcher;

            bPosition = m_ball.position;
            cPosition = m_catcher.position;

            //bPosition_org = bPosition;
            distance = Vector3.Distance(bPosition, cPosition);

            startTime = Time.time;
        }

        private void Update()
        {
            float timeRatio = (Time.time - startTime) / totalTime;
            timeRatio = (timeRatio > 1) ? 1 : timeRatio;

            Vector3 d = new Vector3(0, distance, 0);
            m_ball.position = CalculateBezierPoint(
                timeRatio,
                bPosition,
                bPosition + d,
                cPosition,
                cPosition + d
            );

            if (timeRatio == 1)
            {
                isFinished = true;
            }
        }

        public static Vector3 CalculateBezierPoint(float t,
            Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            //Debug.Log("Calculating");
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float ttt = tt * t;
            float uuu = uu * u;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

        public bool GetIsFinished()
        {
            return isFinished;
        }
    }
}
