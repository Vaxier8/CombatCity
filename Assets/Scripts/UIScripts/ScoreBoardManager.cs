using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreBoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int score = 0;
    public TextMeshProUGUI scoreBoard;
    void Start()
    {
        updateScoreDisplay();
    }

    // Update is called once per frame
    void Update()
    {
       updateScoreDisplay(); 
    }
    public void addScore(int points)
    {
        score += points;
    }
    private void updateScoreDisplay()
    {
        scoreBoard.text = "Score: " + score.ToString();
    }
}
