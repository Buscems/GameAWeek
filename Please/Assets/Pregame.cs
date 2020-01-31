using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pregame : MonoBehaviour
{

    public int timerAmount;

    public TextMeshProUGUI timerText;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = timerAmount.ToString();
    }

    IEnumerator StartGame()
    {
        while (timerAmount > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            timerAmount--;
        }
        yield return new WaitForSecondsRealtime(1);

        Time.timeScale = 1;
        timerText.enabled = false;
    }
}
