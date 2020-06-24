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

        [HideInInspector] public bool useFixedDeltaTime;

        public Mode mode = Mode.Auto;
        public bool ResetAutomatically => mode == Mode.Auto || mode == Mode.ResetAutomatically;
        public bool StartAutomatically => mode == Mode.Auto || mode == Mode.StartAutomatically;

        [Label] public float time;

        float? nextTrigger;
        public float duration;

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

            if (duration <= 0)
            {
                isTriggered = true;
                return;
            }

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

            if (!nextTrigger.HasValue)
                Reset();

            if (nextTrigger > 0)
                nextTrigger = Mathf.Clamp(nextTrigger.Value - (useFixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime), 0, duration);

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
                if (StartAutomatically || mode == Mode.Manual)
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
        public void Reset()
        {
            Start();
            hasStarted = true;
            nextTrigger = duration;
        }

    }

}
