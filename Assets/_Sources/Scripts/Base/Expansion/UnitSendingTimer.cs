using System;
using System.Collections;
using UnityEngine;

public class UnitSendingTimer : MonoBehaviour
{
    [SerializeField, Min(0)] private float _scanIntervalSeconds = 3f;

    private Coroutine _coroutine;
    
    public event Action Ticked;

    public void StartSending()
    {
        StopSending();
        _coroutine = StartCoroutine(Send());
    }

    public void StopSending()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator Send()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(_scanIntervalSeconds);

        while (enabled)
        {
            yield return wait;
            Ticked?.Invoke();
        }
    }
}