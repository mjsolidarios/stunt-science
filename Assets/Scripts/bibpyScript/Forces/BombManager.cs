using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    private ColliderManager theCollide;
    private ForceManagerOne theManagerOne;
    public GameObject bomb, explosionPrefab, glassHolder, otherGlassHolder;
    private Rigidbody2D bombRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        theCollide = FindObjectOfType<ColliderManager>();
        theManagerOne = FindObjectOfType<ForceManagerOne>();
        bombRigidbody = bomb.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(theCollide.collide == true)
        {
            StartCoroutine(explode());
        }
        if(theManagerOne.throwBomb == true)
        {
            if(theManagerOne.playerAnswer == theManagerOne.correctAnswer)
            {
                bombRigidbody.velocity = new Vector2(12, bombRigidbody.velocity.y);
            }
            if(theManagerOne.playerAnswer < theManagerOne.correctAnswer)
            {
                bombRigidbody.velocity = new Vector2(-5, 3);
            }
             if(theManagerOne.playerAnswer > theManagerOne.correctAnswer)
            {
                bombRigidbody.velocity = new Vector2(12, bombRigidbody.velocity.y);
            }
            
            //theManagerOne.throwBomb = false;
        }
    }
    IEnumerator explode()
    {
        yield return new WaitForSeconds(1.2f);
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = bomb.transform.position;
        bomb.SetActive(false);
        
        yield return new WaitForSeconds(.1f);
        if(theManagerOne.tooWeak == true)
        {
            glassHolder.SetActive(false);
            otherGlassHolder.SetActive(false);
        }
        yield return new WaitForSeconds(.4f);
        Destroy(explosion);

    }
}
