using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public Text scoreText;
    int score;
    public GameObject[] rank;
    void Start()
    {
        score = GameDirector.getscore();
        scoreText = GameObject.Find("Score").GetComponent<Text>();

        if (score >= 30)
        {
            GameObject result = Instantiate(rank[0]) as GameObject;
            result.transform.position = new Vector3(0, 3.7f, 0);
        }
        else if ((score >= 15) && (score < 30))
        {
            　GameObject result = Instantiate(rank[1]) as GameObject;
            result.transform.position = new Vector3(0, 3.7f, 0);
        }
        else if (score < 15)   
        {
            GameObject result = Instantiate(rank[2]) as GameObject;
            result.transform.position = new Vector3(0, 3.7f, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
