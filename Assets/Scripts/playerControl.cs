using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour
{
    Rigidbody2D rb;
    public int speed = 4;
    public int jump = 5;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] int lives = 3;

    [SerializeField] GameObject shot;
    [SerializeField] int items = 0;
    [SerializeField] float time = 180;

    public static bool right = true;

    [SerializeField] TMP_Text TxtLives, TxtItems, TxtTime;

    [SerializeField] GameObject TxtWin, TxtLose;

    bool endGame = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.invulnerable = false;
        rb = GetComponent<Rigidbody2D>();
        TxtLives.text = "Lives: " + lives;

        TxtItems.text = "Items: " + items;

        TxtTime.text = time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!endGame)
        {
            float inputX = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);

            if (inputX > 0)
            { //Derecha
                sprite.flipX = false;
                right = true;
            }
            else if (inputX < 0)
            {  //Izquierda
                sprite.flipX = true;
                right = false;
            }

            //Animaciones
            if (Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }

            if (grounded() == false)
            {
                anim.SetBool("isJumping", true);
            }
            else
            {
                anim.SetBool("isJumping", false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded())
            {
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            }

            //Disparo
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(shot, new Vector3(transform.position.x, transform.position.y + 1.7f, 0), Quaternion.identity);
                anim.SetBool("isShooting", true);
            }
            time = time - Time.deltaTime;
            if (time < 0)
            {
                time = 0;
                endGame = true;
                TxtLose.SetActive(true);
                Invoke("goTomenu", 3);
            }

            float min, sec;
            min = Mathf.Floor(time / 60);
            sec = Mathf.Floor(time % 60);

            TxtTime.text = min.ToString("00") + ":" + sec.ToString("00");
        }
        else {
            rb.linearVelocity = Vector2.zero;
            sprite.gameObject.SetActive(false);
            }
    }

    bool grounded()
    {
        RaycastHit2D touch = Physics2D.Raycast(transform.position,
                                              Vector2.down,
                                              0.2f);
        if (touch.collider == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            sprite.color = Color.blue;
            GameManager.invulnerable = true;
            Invoke("becomeVulnerable", 5);
        }

        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            items++;
            TxtItems.text = "Items: " + items;
            if (items == 4)
            {
                endGame = true;
                TxtWin.SetActive(true);
                Invoke("goToCredits", 3);
            }
        }
    }

    void becomeVulnerable()
    {
        sprite.color = Color.white;
        GameManager.invulnerable = false;
    }


    public void damage()
    {
        if (!endGame){
            lives--;
        }

        sprite.color = Color.red;
        GameManager.invulnerable = true;
        Invoke("becomeVulnerable", 1);
        if (lives < 0)
        {
            lives = 0;
            endGame = true;
            TxtLose.SetActive(true);
            Invoke("goToMenu", 3);
        }
        TxtLives.text = "Lives: " + lives;
    }

    void goToMenu()
    {
        SceneManager.LoadScene("Mainmenu");
    }

    void goToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
