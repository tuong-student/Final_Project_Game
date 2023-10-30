using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NOOD
{
    public class CoroutineScript : MonoBehaviour
    {
        private Action action;
        private Action onCompleteLoop;
        private bool isComplete;
        private float pauseTimePerLoop;

        public void StartCoroutineLoop(Action action, float pauseTimePerLoop)
        {
            this.action = action;
            this.pauseTimePerLoop = pauseTimePerLoop;
            StartCoroutine(LoopFunctionCR());
        }

        public void Complete()
        {
            StopAllCoroutines();
            onCompleteLoop?.Invoke();
            Destroy(this.gameObject, 0.2f);
        }
        
        IEnumerator LoopFunctionCR()
        {
            while(isComplete == false)
            {
                yield return pauseTimePerLoop;
                action?.Invoke();
            }
        }
    }
}
