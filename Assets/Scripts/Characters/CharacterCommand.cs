namespace Tactical.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MEC;
    public abstract class CharacterCommand : MonoBehaviour
    {
        protected Character character;

        protected bool ConsumeATB = false;

        protected bool PreAbilityIsDone = false;
        protected bool AbilityIsDone = false;

        //Abstracts
        public abstract IEnumerator _MainAbility();
        //Virtuals
        public virtual void Execute()
        {
            if (AbilityCondition())
            {
                StartCoroutine(_ExecuteAbility());
            }
            else
            {
                OnConditionFailure();
            }
        }


        public virtual IEnumerator _ExecuteAbility()
        {

            yield return StartCoroutine(_PreAbility());
            yield return StartCoroutine(_MainAbility());
            yield return StartCoroutine(_PostAbility());
        }

        public virtual IEnumerator _PreAbility()
        {
            yield return Timing.WaitForOneFrame;
        }

        public virtual IEnumerator _PostAbility()
        {

            yield return Timing.WaitForOneFrame;
        }

        public virtual bool AbilityCondition()
        {
            return true;
        }

        public virtual void OnConditionFailure()
        {

        }


    }
}
