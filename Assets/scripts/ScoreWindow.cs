using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Level.GetInstance().GetState() == Level.StateEnum.Playing)
        {
            scoreText.text = Level.GetInstance().GetPipesPassed().ToString();
        }

        if (Level.GetInstance().GetState() == Level.StateEnum.BirdDead)
        {
            scoreText.text = "GAME OVER - " + Level.GetInstance().GetPipesPassed().ToString();
        }

    }
}
