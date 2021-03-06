﻿namespace Cawotte.Tactical.Level
{
    
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;
    using MEC;

    [Serializable]
    public class ATBGauge
    {

        #region Private Members
        [SerializeField] private float reloadDuration = 1f;

        private const float maxValue = 1f;
        private float currentValue = 1f;
        #endregion

        //Called actions when the ATB value change.
        public Action<float> OnValueChange = null;


        public float CurrentValue
        {
            get
            {
                return currentValue;
            }

            private set
            {
                value = Mathf.Clamp(value, 0, maxValue);
                if (value != CurrentValue)
                {
                    currentValue = value;
                    OnValueChange?.Invoke(value);
                }
            }
        }

        public float ReloadDuration { get => reloadDuration; set => reloadDuration = value; }

        public ATBGauge (float reloadDuration)
        {
            this.currentValue = maxValue;
            this.reloadDuration = reloadDuration;
        }

        #region Public Methods
        public void Consume(int percentConsumed)
        {
            percentConsumed = Mathf.Clamp(percentConsumed, 0, 100);

            float loss = Mathf.Lerp(0, maxValue, percentConsumed / 100f);
            CurrentValue -= loss;
        }

        public void Add(int percentAdded)
        {
            percentAdded = Mathf.Clamp(percentAdded, 0, 100);

            float gain = Mathf.Lerp(0, maxValue, percentAdded / 100f);
            CurrentValue += gain;
        }

        public void ResetValue()
        {
            CurrentValue = maxValue;
        }

        public void StartReloading()
        {
            Timing.RunCoroutine(_reloadATB());
        }

        public bool IsFull()
        {
            return CurrentValue == maxValue;
        }

        public bool IsEmpty()
        {
            return CurrentValue == 0;
        }
        #endregion

        #region Coroutines
        private IEnumerator<float> _reloadATB()
        {
            float t = 0f;

            //We scale t with the remaining ATB fill
            t = Mathf.Lerp(0, reloadDuration, CurrentValue / maxValue);

            while (t < reloadDuration)
            {
                t += Time.deltaTime;
                CurrentValue = Mathf.Lerp(0, maxValue, t / reloadDuration);
                yield return Timing.WaitForOneFrame;
            }

            CurrentValue = maxValue;
        }
        #endregion


    }
}
