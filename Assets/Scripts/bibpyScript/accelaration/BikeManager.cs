using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeManager : MonoBehaviour
{
    private Player thePlayer;
    private Animator myAnimator;
    public bool collided;
    public GameObject driverPrefab;
    
    public float moveSpeed;
    public Rigidbody2D myRigidbody;
    private Collider2D myCollider;
    public GameObject driverStickman;
     public GameObject stickprefab;
     public GameObject stickmanpoint;
     public bool decelarate;
     public bool brake;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
         myAnimator.SetFloat("speed", myRigidbody.velocity.x);
      
       if (decelarate)
       {
           myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y);
       }
       else
       {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
       }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
         if (other.gameObject.tag == ("wall"))
        { 
            if (collided == false)
            {
                driverspawn();
                decelarate = true;
                thePlayer.standup = true;
                accSimulation.playerDead = true;
                collided = true;
                thePlayer.standup = true;
                driverPrefab.SetActive(false);
                driverStickman.SetActive(false);
            }  
        }
         if (other.gameObject.tag == ("braker"))
            {
                brake = true;
            }


    }
    
    public void driverspawn()
    {
        
        GameObject stick = Instantiate(stickprefab);
        stick.transform.position = stickmanpoint.transform.position;  
        stick.transform.rotation = stickmanpoint.transform.rotation;    
    }
    
    
}
