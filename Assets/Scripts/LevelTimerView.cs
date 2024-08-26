using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimerView : MonoBehaviour
{
    [SerializeField] private Text _counter;
    [SerializeField] private Text _winCounter;

    private float count;

    private void OnEnable()
    {
        count = 0;
        SetTextTimer(0);
        StartCoroutine(DoTimer());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    //public void StartLevelTimeCounter()
    //{
    //    StartCoroutine(DoTimer());
    //}

    public void StopLevelTimeCounter()
    {
        StopAllCoroutines();
    }

    private IEnumerator DoTimer(float countTime = 1f)
    {
        while (true)
        {
            yield return new WaitForSeconds(countTime);
            count += 1;
            SetTextTimer(count);
        }
    }

    private void SetTextTimer(float value)
    {
        int minutes = Mathf.FloorToInt(value / 60);
        int seconds = Mathf.FloorToInt(value % 60);
        _counter.text = $"{minutes:00}:{seconds:00}";
        _winCounter.text = $"{minutes:00}:{seconds:00}";
    }

}

