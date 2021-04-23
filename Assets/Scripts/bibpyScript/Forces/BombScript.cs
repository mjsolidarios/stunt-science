using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private float distanceToMoveX;
    private Player thePlayer;
    private float distanceToMoveY;
    private ragdollForces theRagdoll;
    private Vector2 ragDollLastPOs;
    private BombManager theBomb;
    public GameObject bombPos;
    public bool inPlayer;
   

    // Start is called before the first frame update
    void Start()
    {
        
        theBomb = FindObjectOfType<BombManager>();
        thePlayer = FindObjectOfType<Player>();
        ragDollLastPOs = new Vector2(22, -0.6f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inPlayer)
        {
            transform.position = bombPos.transform.position;
            transform.rotation = bombPos.transform.rotation;
        }
        theRagdoll = FindObjectOfType<ragdollForces>();
        if (theBomb.followRagdoll == true)
        {
            transform.position = theRagdoll.transform.position;
            /*distanceToMoveX = theRagdoll.transform.position.x - ragDollLastPOs.x;
            distanceToMoveY = theRagdoll.transform.position.y - ragDollLastPOs.y;
            transform.position = new Vector2(transform.position.x + distanceToMoveX, transform.position.y + distanceToMoveY);
            ragDollLastPOs = theRagdoll.transform.position;*/
        }
    }
}
