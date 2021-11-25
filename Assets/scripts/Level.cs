using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    private enum YpositionEnum
    {
        Top,
        Bottom
    }

    public enum StateEnum
    {
        Playing,
        BirdDead,
        WaitingToStart
    }


    private List<Transform> pipeList = new List<Transform>();
    private const float PipeDestroyXPosition = -100f;
    private const float PipeSpawnXPosition = +90f;
    private const float BirdXPosition = 0f;
    private float pipeSpawnTimer = 3f;
    private float pipeSpawnTimerMax = 2f;
    private float pipeSpeed = 25f;
    private static Level instance;
    private int pipePassedCount = 0;
    private StateEnum state;

    public static Level GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        state = StateEnum.WaitingToStart;
    }

    void Start()
    {
        Bird.GetInstance().Ondied += BirdOndied;
        Bird.GetInstance().OnStartedPlaying += BirdOnStartedPlaying;
    }

    private void BirdOndied(object sender, System.EventArgs e)
    {
        state = StateEnum.BirdDead;
        SoundManager.PlaySound(SoundManager.SoundEnum.BirdDie);
        StartCoroutine(Restart());
    }

    private void BirdOnStartedPlaying(object sender, System.EventArgs e)
    {
        state = StateEnum.Playing;
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    void Update()
    {
        if (state == StateEnum.Playing)
        {
            if (GetPipesPassed() > 1)
            {
                pipeSpeed = pipeSpeed + (GetPipesPassed() / 1000f);
            }

            if (GetPipesPassed() >= 10)
            {

                pipeSpawnTimerMax = 2.1f - 0.01f * GetPipesPassed();
            }

            HandlePipeMovement(pipeSpeed);
            HandlePipeSpawning();
        }
    }

    private void CreatePipesObstacle(float gapSize, float xPosition)
    {
        CreatePipe(YpositionEnum.Top, xPosition, gapSize);
        CreatePipe(YpositionEnum.Bottom, xPosition, gapSize);
    }

    private void CreatePipe(YpositionEnum yPosition, float xPosition, float gapSize)
    {
        var height = -50f;
        var rotateAngle = 0f;
        switch (yPosition)
        {
            case YpositionEnum.Top:
                height = 50f;
                rotateAngle = 180f;
                break;
            case YpositionEnum.Bottom:
                height = -50f;
                break;
        }

        Transform pipe = Instantiate(GameAssets.GetInstance().pipePrefab);
        pipe.position = new Vector3(xPosition, height, 0f);
        pipe.localScale = new Vector3(1f, gapSize, 1f);
        pipe.Rotate(0f, 0f, rotateAngle);
        SpriteRenderer pipeSpriteRenderer = pipe.GetComponent<SpriteRenderer>();
        pipeSpriteRenderer.sprite = GameAssets.GetInstance().pipeSprite;
        pipeList.Add(pipe);
    }

    private void HandlePipeMovement(float speed)
    {
        foreach (Transform pipe in pipeList)
        {
            bool isToTheRightOfBird = pipe.position.x > BirdXPosition;
            pipe.position += Vector3.left * speed * Time.deltaTime;
            if (isToTheRightOfBird && pipe.position.x < BirdXPosition)
            {
                pipePassedCount++;
                SoundManager.PlaySound(SoundManager.SoundEnum.BirdScore);
                if (pipePassedCount / 2 == 5 || pipePassedCount / 2 == 10 || pipePassedCount / 2 == 15 || pipePassedCount / 2 == 20 )
                {
                    Bird.GetInstance().UpgradeBirdSprite();
                }
            }

            if (pipe.position.x < PipeDestroyXPosition)
            {
                pipeList.Remove(pipe);
                Destroy(pipe.gameObject);
            }
        }
    }

    private void HandlePipeSpawning()
    {
        pipeSpawnTimer += Time.deltaTime;
        if (pipeSpawnTimer > pipeSpawnTimerMax)
        {
            pipeSpawnTimer = 0f;
            float startRange = 1.3f;
            float endRange = 2f;
            CreatePipesObstacle(Random.Range(startRange, endRange), PipeSpawnXPosition);
        }
    }

    public int GetPipesPassed()
    {
        return pipePassedCount / 2;
    }

    public StateEnum GetState()
    {
        return state;
    }
}
