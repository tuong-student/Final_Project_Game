using System;
using System.Collections;
using UnityEngine;

namespace NOOD
{
    public class CoroutineScript : MonoBehaviour
    {
        private Func<bool> func;
        private Action onCompleteLoop;
        private bool isComplete;
        private float pauseTimePerLoop;
        private int loopTime = -1;
        private int performTime;

        public void StartCoroutineLoop(Func<bool> func, float pauseTimePerLoop, int loopTime)
        {
            isComplete = false;
            this.func = func;
            this.pauseTimePerLoop = pauseTimePerLoop;
            this.loopTime = loopTime;
            this.performTime = 0;
            StartCoroutine(LoopFunctionCR());
        }

        public void Complete()
        {
            this.StopCoroutine(LoopFunctionCR());
            onCompleteLoop?.Invoke();
            Destroy(this.gameObject, 0.2f);
        }
        
        IEnumerator LoopFunctionCR()
        {
            while( isComplete == false)
            {
                yield return pauseTimePerLoop;
                performTime++;
                if(performTime == loopTime || func?.Invoke() == true)
                {
                    isComplete = true;
                    Complete();
                }
            }
        }
    }
}
