using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeManager : MonoBehaviour
{
    private Player thePlayer;
    bool collided;
    public GameObject driverPrefab;
    
    public float moveSpeed;
    public Rigidbody2D myRigidbody;
    private Collider2D myCollider;
    public GameObject driverStickman;
     public GameObject stickprefab;
     public GameObject stickmanpoint;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
         if (other.gameObject.tag == ("wall"))
        { 
            if (collided == false)
            {
                driverspawn();
                moveSpeed = myRigidbody.velocity.x;
                thePlayer.standup = true;
                accSimulation.playerDead = true;
                collided = true;
                thePlayer.standup = true;
                driverPrefab.SetActive(false);
                driverStickman.SetActive(false);
            }
            
           
        }

    }
    
    public void driverspawn()
    {
        
        GameObject stick = Instantiate(stickprefab);
        stick.transform.position = stickmanpoint.transform.position;      
    }
    
    
}
