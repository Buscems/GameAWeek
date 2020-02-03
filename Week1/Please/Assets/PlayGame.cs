using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{

    public Image gameBox;
    bool start;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameBox.fillAmount >= 1 && !start)
        {
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        start = true;
        yield return new WaitForSeconds(.5f);
        if (this.gameObject.name == "MainMenu")
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (this.gameObject.name == "PlayGame" || this.gameObject.name == "Restart")
        {
            SceneManager.LoadScene("GameScene");
        }
        if (this.gameObject.name == "Instruction")
        {
            SceneManager.LoadScene("Instructions");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameBox.fillAmount += Time.deltaTime;
        }
    }

}
