
namespace Cawotte.Tactical.Level
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public abstract class ATBAction : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 100)]
        private int atbConsume;

        [SerializeField]
        private ConsumeTime consumeTime = ConsumeTime.Start;
        
        private ATBGauge atb;

        protected abstract void execute();

        // Start is called before the first frame update
        void Start()
        {
            atb = GetComponent<Character>().AtbGauge;
        }
        

        public enum ConsumeTime
        {
            Start, End
        }
    }
}
