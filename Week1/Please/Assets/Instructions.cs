using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Instructions : MonoBehaviour
{

    public Image textBackground;
    public TextMeshProUGUI instructionText;

    float flashTimer;
    public float flashTimerMax;

    float opacity;

    // Start is called before the first frame update
    void Start()
    {
        textBackground.color = new Vector4(textBackground.color.r, textBackground.color.g, textBackground.color.b, 0);
        textBackground.enabled = false;
        instructionText.color = new Vector4(instructionText.color.r, instructionText.color.g, instructionText.color.b, 0);
        instructionText.enabled = false;

        StartCoroutine(Move());

    }

    // Update is called once per frame
    void Update()
    {

        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            opacity += Time.deltaTime * 2;
            textBackground.color = new Vector4(textBackground.color.r, textBackground.color.g, textBackground.color.b, Mathf.Sin(opacity));
            instructionText.color = new Vector4(instructionText.color.r, instructionText.color.g, instructionText.color.b, Mathf.Sin(opacity));
        }
        else
        {
            textBackground.color = new Vector4(textBackground.color.r, textBackground.color.g, textBackground.color.b, 0);
            instructionText.color = new Vector4(instructionText.color.r, instructionText.color.g, instructionText.color.b, 0);
            textBackground.enabled = false;
            instructionText.enabled = false;
        }

    }

    IEnumerator Shoot()
    {
        instructionText.text = "Shoot with the right trigger";
        textBackground.enabled = true;
        instructionText.enabled = true;
        flashTimer = flashTimerMax;
        yield return new WaitForSeconds(flashTimerMax);
        StartCoroutine(Aim());
    }

    IEnumerator Move()
    {
        instructionText.text = "Move with the Left Joystick";
        textBackground.enabled = true;
        instructionText.enabled = true;
        flashTimer = flashTimerMax;
        yield return new WaitForSeconds(flashTimerMax);
        StartCoroutine(Shoot());
    }

    IEnumerator Aim()
    {
        instructionText.text = "Aim with the Right Joystick";
        textBackground.enabled = true;
        instructionText.enabled = true;
        flashTimer = flashTimerMax;
        yield return new WaitForSeconds(flashTimerMax);
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        instructionText.text = "Dash with the Left Trigger";
        textBackground.enabled = true;
        instructionText.enabled = true;
        flashTimer = flashTimerMax;
        yield return new WaitForSeconds(flashTimerMax);
        StartCoroutine(Boss());
    }

    IEnumerator Boss()
    {
        instructionText.text = "Every Ten Seconds the Boss will Power Up";
        textBackground.enabled = true;
        instructionText.enabled = true;
        flashTimer = flashTimerMax;
        yield return new WaitForSeconds(flashTimerMax);
        yield return new WaitForSeconds(3);
        StartCoroutine(Move());
    }

}
