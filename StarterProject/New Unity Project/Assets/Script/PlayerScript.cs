using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text win;
    public Text annocement;
    public Text countdownText;
    private int scoreValue = 0;
    AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    public AudioClip pickupSound;
    private bool facingRight = true;
    private bool vertUp = true;
    private float hozSpeed; 
    float currentTime = 0f;
    float startingTime = 10f;
    private bool keepTimer;
    private bool gameOver;
    

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        audioSource= GetComponent<AudioSource>();

        score.text = "Score: " + scoreValue.ToString();
        annocement.text = "Collect 4 demon fire in 10 seconds to win.";
        {
            Destroy(annocement, 2);
        }

        speed = 0;

        StartCoroutine(Delay());
        currentTime = 3f; 
        countdownText.text = "Timer: " + currentTime.ToString("0");
        PlaySound (backgroundMusic); 
        keepTimer = false;
        gameOver = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds (2f);
        currentTime = startingTime; 
        countdownText.text = "Timer: " + currentTime.ToString("0");
        speed = 10;
        
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = "Timer: " + currentTime.ToString("0");

        if(currentTime <= 5)
        {
            countdownText.color = Color.red;
        }

        if(currentTime <= 0)
        {
            currentTime = 0;
            win.text = "Sorry, you lost! Press R to try again.";
            speed = 0f;
            gameOver = true;
            PlaySound (loseMusic);
        }

        if(keepTimer==true)
        {
            currentTime += 1 * Time.deltaTime;
            countdownText.text = " ";
        } 
        
        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        
        if (hozSpeed > 0 && vertUp == false)
        {
            if (facingRight == false)
            {
                Flip();
            }
        }
        if (hozSpeed < 0 && vertUp == false)
        {
            if (facingRight == true)
            {
                Flip();
            }
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Pickup")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            PlaySound (pickupSound); 
            
            if (scoreValue == 4)
            {
                win.text = "Congratulations, you won! Press R to play again.";
                keepTimer = true;
                gameOver = true;
                PlaySound (winMusic);
            }
        
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            vertUp = false;

            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0,3),ForceMode2D.Impulse);
                vertUp = true;
            }
        }
    }
}
