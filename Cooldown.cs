#pragma warning disable CS0649

using System;
using UnityEngine;

namespace Common
{

    /// <summary>A class that manages cooldowns.</summary>
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

        bool canStart;
        bool hasStarted;

        int lastFrameCount;
        bool isTriggered;

        /// <summary>Starts this cooldown.</summary>
        public void Start() =>
            canStart = true;

        /// <summary>Updates and returns whatever this cooldown is triggered or not.</summary>
        public bool IsTriggered()
        {
            Update();
            return isTriggered;
        }

        /// <summary>Updates this cooldown, this or IsTriggered must be called every frame.</summary>
        public void Update()
        {

            if (lastFrameCount == Time.frameCount)
                return;
            lastFrameCount = Time.frameCount;

            if (!canStart)
            {
                if (StartAutomatically)
                    Start();
                isTriggered = false;
                return;
            }

            if (m_duration > 0)
                duration = m_duration;

            if (!duration.HasValue)
            {
                isTriggered = false;
                return;
            }

            if (!nextTrigger.HasValue)
                Reset();

            if (nextTrigger > 0)
                nextTrigger = Mathf.Clamp(nextTrigger.Value - Time.deltaTime, 0, duration.Value);

            if (nextTrigger > 0)
            {
                hasStarted = true;
                time = nextTrigger.Value;
                isTriggered = false;
                return;
            }
            else if (hasStarted)
            {
                ResetAuto();
                time = nextTrigger.Value;
                hasStarted = false;
                isTriggered = true;
                return;
            }

            isTriggered = false;

        }

        void ResetAuto()
        {
            if (ResetAutomatically)
                Reset();
        }

        /// <summary>Resets this cooldown.</summary>
        public void Reset() =>
            nextTrigger = duration ?? 1f;

    }

}
