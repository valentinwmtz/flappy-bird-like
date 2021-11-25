using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private const float JumpAmount = 90f;
    private Rigidbody2D birdRigidbody2D;
    private SpriteRenderer birdSpriterender;
    public event EventHandler Ondied;
    public event EventHandler OnStartedPlaying;
    private static Bird instance;
    private int birdSprite = 0;
    private enum stateEnum
    {
        WaitingToStart,
        Playing,
        Dead
    }

    private stateEnum state;

    public static Bird GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
            case stateEnum.WaitingToStart:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    state = stateEnum.Playing;
                    birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (OnStartedPlaying != null)
                    {
                        OnStartedPlaying(this, EventArgs.Empty);
                    }
                }

                break;
            case stateEnum.Playing:
                if (transform.position.y < -50f || transform.position.y > +50f)
                {
                    state = stateEnum.Dead;
                    if (Ondied != null)
                    {
                        Ondied(this, EventArgs.Empty);
                    }
                }

                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
                transform.eulerAngles = new Vector3(0, 0, birdRigidbody2D.velocity.y * 0.2f);
                break;
            case stateEnum.Dead:
                break;
        }
    }

    private void Jump()
    {
        birdRigidbody2D.velocity = Vector2.up * JumpAmount;
        SoundManager.PlaySound(SoundManager.SoundEnum.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = stateEnum.Dead;
        if (Ondied != null)
        {
            Ondied(this, EventArgs.Empty);
        }
    }

    public void UpgradeBirdSprite()
    {
        birdSpriterender = GetComponent<SpriteRenderer>();
        if (birdSprite <  GameAssets.GetInstance().birdSprites.Length - 1)
        {
            birdSprite++;
            Debug.Log(birdSprite.ToString());
            birdSpriterender.sprite = GameAssets.GetInstance().birdSprites[birdSprite];
        }

    }
}
