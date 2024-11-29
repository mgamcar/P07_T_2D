using Unity.VisualScripting;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    Rigidbody2D rb;
    public int speed = 4;
    public int jump = 5;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);

        if (inputX > 0){ //Derecha
           sprite.flipX = false; 
        } else if (inputX < 0){  //Izquierda
           sprite.flipX = true; 
        }

        //Animaciones
        if (Input.GetKey(KeyCode.A) || 
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow)){
              anim.SetBool("isRunning",true);
        } else {
              anim.SetBool("isRunning",false);
        }
 
        if (grounded()==false){
            anim.SetBool("isJumping",true);
        } else {
            anim.SetBool("isJumping",false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded()){
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
    }

   

    bool grounded(){
        RaycastHit2D touch = Physics2D.Raycast(transform.position,
                                              Vector2.down,
                                              0.2f); 
        if (touch.collider ==  null){
            return false; 
        } else {
            return true;
        }                                      
    }

}
