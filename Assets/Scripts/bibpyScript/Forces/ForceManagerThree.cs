using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForceManagerThree : MonoBehaviour
{
    public Player thePlayer;
    public BombScript theBombScript;
    private ForceSimulation theSimulate;
    private ColliderManager theCollider;
    private BombManager theBomb;
    float generateAccelaration, accelaration, playerAccelaration, generateForce, force, generateCorrectAnswer, currentPos;
    public float correctAnswer, playerAnswer,increaseMass;
    public GameObject glassHolder, stickPrefab, stickmanpoint, afterStuntMessage, retry, next, glassDebri;
    public GameObject[] glassDebriLoc;
    public bool tooWeak, tooStrong, ragdollReady, startAddingMass, crowdExit;
    public bool throwBomb, addingWeight, startRunning, goExit;
    public TMP_Text stuntMessageTxt, playerMass;




    // Start is called before the first frame update
    void Start()
    {
        //thePlayer = FindObjectOfType<Player>();
        theCollider = FindObjectOfType<ColliderManager>();
        theSimulate = FindObjectOfType<ForceSimulation>();
        theBomb = FindObjectOfType<BombManager>();
        GenerateProblem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = thePlayer.transform.position.x;
        playerAnswer = ForceSimulation.playerAnswer;
        generateCorrectAnswer = force / accelaration - 70;
        correctAnswer = (float)System.Math.Round(generateCorrectAnswer, 2);
        playerMass.text = increaseMass.ToString("F2") + ("kg");
        if (addingWeight)
        {
            StartCoroutine(addingMass());
            addingWeight = false;
        }

        if (ForceSimulation.simulate == true)
        {
            thePlayer.moveSpeed = 3;
        }
        if (startAddingMass)
        {
            increaseMass += 50 * Time.fixedDeltaTime;
            if(increaseMass >= playerAnswer + 70)
            {
                startAddingMass = false;
                increaseMass = playerAnswer + 70;
            }
        }
        if (goExit)
        {
            if(currentPos >= 22.6f)
            {
                thePlayer.moveSpeed = 0;
                thePlayer.godown = true;
            }
        }


        if (startRunning)
        {
            ForceSimulation.simulate = false;
            thePlayer.moveSpeed += accelaration * Time.fixedDeltaTime;
            if (theCollider.collide == true)
            {
                if (playerAnswer == correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=green>Your Answer is Correct!!!</b>\n\n" + PlayerPrefs.GetString("Name") + " has broken the glass</color>";
                    glassHolder.SetActive(false);

                    if (currentPos >= 22)
                    {
                        startRunning = false;
                        StartCoroutine(braking());
                        thePlayer.moveSpeed = 0;
                        thePlayer.transform.position = new Vector2(22, 2.5f);
                        StartCoroutine(collision());


                    }
                }
                if (playerAnswer < correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + " the glass was too tough for </color>" + PlayerPrefs.GetString("Name") + ", and unable to break the glass. The correct answer is " + correctAnswer.ToString("F1") + "Newtons.";
                    tooWeak = true;
                    thePlayer.gameObject.SetActive(false);
                    if (ragdollReady)
                    {
                        ragdollSpawn();
                        thePlayer.standup = true;
                    }
                    thePlayer.moveSpeed = 0;
                    retry.SetActive(true);
                    StartCoroutine(collision());
                    StartCoroutine(StuntResult());
                    theSimulate.playerDead = true;
                }
                if (playerAnswer > correctAnswer)
                {
                    stuntMessageTxt.text = "<b><color=red>Stunt Failed!!!</b>\n\n" + " the glass was too weak for </color>" + PlayerPrefs.GetString("Name") + ", able to break the glass but also went through it. The correct answer is " + correctAnswer.ToString("F1") + "Newtons.";
                    tooStrong = true;
                    thePlayer.gameObject.SetActive(false);
                    glassHolder.SetActive(false);
                    throwBomb = true;
                    if (ragdollReady)
                    {
                        ragdollSpawn();
                    }
                    StartCoroutine(collision());
                    retry.SetActive(true);
                    StartCoroutine(StuntResult());
                    theSimulate.playerDead = true;
                }
            }
        }

    }
    public void GenerateProblem()
    {
        generateAccelaration = Random.Range(3f, 3.5f);
        accelaration = (float)System.Math.Round(generateAccelaration, 2);
        generateForce = Random.Range(250, 300);
        force = (float)System.Math.Round(generateForce, 2);
        theBomb.glassHolder[0].SetActive(true);
        theBomb.otherGlassHolder[0].SetActive(true);
        ragdollReady = true;
        theBomb.bomb.SetActive(true);
        theBomb.bomb.transform.position = thePlayer.transform.position;
        theBomb.followRagdoll = false;
        thePlayer.transform.position = new Vector2(0, 3.2f);
        theBombScript.gameObject.transform.position = new Vector2(7.8f, 1.5f);
        glassRespawn();
        ForceSimulation.question = ((PlayerPrefs.GetString("Name") + ("</b> is instructed to break the glass wall by running into it using his own body force. If  <b>") + PlayerPrefs.GetString("Name") + ("</b> has a force of  <b>") + force.ToString("F2") + ("</b> kg and runs with an accelaration of <b>") + accelaration.ToString("F2") + ("</b> m/s², what should impact force breaking point of the glass wall? If the glass is too tough , it will not break. If the glass is too weak, ") + PlayerPrefs.GetString("Name") + (" will overshoot beyond the glass after breaking.")));




    }
    public void ragdollSpawn()
    {
        GameObject stick = Instantiate(stickPrefab);
        stick.transform.position = stickmanpoint.transform.position;
        ragdollReady = false;
    }
    IEnumerator braking()
    {
        thePlayer.brake = true;
        thePlayer.thisway = true;
        crowdExit = true;
        yield return new WaitForSeconds(1);
        //throwBomb = true;
        thePlayer.brake = false;
        theBombScript.inPlayer = false;
        yield return new WaitForSeconds(5);
        goExit = true;
        thePlayer.thisway = false;
        thePlayer.moveSpeed = 5;
        yield return new WaitForSeconds(1);
        goExit = false;
        thePlayer.godown = false;
        yield return new WaitForSeconds(1);
        thePlayer.moveSpeed = 5;
    }
    IEnumerator collision()
    {
        yield return new WaitForEndOfFrame();
        theCollider.collide = false;

    }
    IEnumerator StuntResult()
    {
        ForceSimulation.simulate = false;
        StartCoroutine(theSimulate.DirectorsCall());
        yield return new WaitForSeconds(5);
        afterStuntMessage.SetActive(true);

    }
    public void glassRespawn()
    {
        GameObject glass1 = Instantiate(glassDebri);
        glass1.transform.position = glassDebriLoc[0].transform.position;
       



    }
    IEnumerator addingMass()
    {
        yield return new WaitForSeconds(1.5f);
        startAddingMass = true;
        yield return new WaitForSeconds(2f);
        thePlayer.addweights = false;
        yield return new WaitForSeconds(2);
        playerMass.gameObject.SetActive(false);
        startRunning = true;
    }

}
