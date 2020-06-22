#pragma warning disable CS0649

using System;
using UnityEngine;

namespace Common
{

    [Serializable]
    public class Cooldown
    {

        public enum Mode
        {
            Manual, ResetAutomatically, StartAutomatically, Auto
        }

        public Mode mode = Mode.Auto;
        public bool ResetAutomatically => mode == Mode.Auto || mode == Mode.ResetAutomatically;
        public bool StartAutomatically => mode == Mode.Auto || mode == Mode.StartAutomatically;

        [Label] public float time;

        float? nextTrigger;
        public float? duration;

        [SerializeField] float m_duration;

        bool isStarted;

        public void Start()
        {
            isStarted = true;
        }

        public bool CanTrigger() =>
            Update();

        bool hasStarted;
        public bool Update()
        {

            if (!isStarted)
            {
                if (StartAutomatically)
                    Start();
                return false;
            }

            if (m_duration > 0)
                duration = m_duration;

            if (!duration.HasValue)
                return false;

            if (!nextTrigger.HasValue)
                Reset();

            if (nextTrigger > 0)
                nextTrigger = Mathf.Clamp(nextTrigger.Value - Time.deltaTime, 0, duration.Value);

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
            if (ResetAutomatically)
                Reset();
        }

        public void Reset() =>
            nextTrigger = duration ?? 1f;

    }

}
