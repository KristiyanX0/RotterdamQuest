using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CallbackAfterTime : MonoBehaviour
{
    public UnityEvent callbackEvent; // Global UnityEvent variable

    public void TriggerCallbackAfterTime(float delay)
    {
        StartCoroutine(CallbackCoroutine(delay));
    }

    private IEnumerator CallbackCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        callbackEvent?.Invoke();
    }

    // ...existing code...
}