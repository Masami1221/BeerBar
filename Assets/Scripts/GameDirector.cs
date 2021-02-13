using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // Start is called before the first frame update
    public static int score = 0;    
    Text scoreText; 
    Text timeText;
    float timeLimit = 60.0f;

    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();　//ScoreのUIを代入する
        timeText = GameObject.Find("Time").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit -= Time.deltaTime;
        timeText.text = "Time : " + Mathf.FloorToInt(timeLimit).ToString();
        if (timeLimit <= 0)
        {
            SceneManager.LoadScene("ResultScene");
        }
    }

    public void ScoreUp(int successLevel)
    {
        score += successLevel;
        scoreText.text = "Score : " + score.ToString(); //得点を表示させる
    }

    public void ScoreDown()
    {
        score --;
        scoreText.text = "Score : " + score.ToString();
    }

    public static int getscore()
    {
        return score;
    }
}
