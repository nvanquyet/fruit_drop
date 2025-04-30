using DesignPatterns;
using UnityEngine;

namespace _VnW.Scripts.Extension
{
    public class TimerUtility : SingletonDontDestroy<TimerUtility>
    {
        public void CallWithDelay(System.Action action, float delay)
        {
            StartCoroutine(DelayedCallRoutine(action, delay));
        }
    
        private System.Collections.IEnumerator DelayedCallRoutine(System.Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}