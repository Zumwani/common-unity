using System;
using UnityEngine;

namespace Common
{

    [Serializable]
    public class Cooldown
    {

        public bool resetAutomatically = true;
        public bool startAutomatically = true;
        [ReadOnly] public float time;

        float? nextTrigger;
        public float? duration;

        [SerializeField] float m_duration;

        public bool CanTrigger() =>
            Update();

        bool hasStarted;
        public bool Update()
        {

            if (m_duration > 0)
                duration = m_duration;

            if (!duration.HasValue)
                return false;

            if (!nextTrigger.HasValue && startAutomatically)
                Reset();

            if (nextTrigger > 0)
                nextTrigger -= Time.deltaTime;

            if (nextTrigger > 0)
            {
                hasStarted = true;
                time = nextTrigger.Value;
                return false;
            }
            else if (hasStarted)
            {
                ResetAuto();
                time = nextTrigger.Value;
                hasStarted = false;
                return true;
            }

            return false;

        }

        void ResetAuto()
        {
            if (resetAutomatically)
                Reset();
        }

        public void Reset() =>
            nextTrigger = duration ?? 1f;

    }

}
