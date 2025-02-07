using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VelocityEasyStage3 : MonoBehaviour
{
    public ScoreManager Scorer;
    public Player myPlayer;
    public CeillingGenerator theCeiling;
    public HeartManager HeartManager;
    public TMP_Text playerNameText, messageText, timer;
    public float distance, gameTime, Speed, elapsed, currentPos;
    string pronoun, playerName, playerGender;
    public GameObject slidePlatform, lowerGround, AfterStuntMessage, safeZone, rubblesStopper, dimensionLine, ragdollSpawn, manholeCover;
    bool director;
    float answer;

    StageManager sm = new StageManager();

    // Start is called before the first frame update

    public void Start()
    {
        theCeiling = FindObjectOfType<CeillingGenerator>();
        myPlayer = FindObjectOfType<Player>();
        HeartManager = FindObjectOfType<HeartManager>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        Scorer = FindObjectOfType<ScoreManager>();
        Stage3SetUp();
    }
    void FixedUpdate()
    {
        answer = SimulationManager.playerAnswer;
        if (SimulationManager.stage3Flag)
        {
            dimensionLine.SetActive(true);
            float totalDistance = 40f;
            float initialDistance = (float)System.Math.Round((totalDistance - answer), 2);
            DimensionManager.startLength = initialDistance;
            DimensionManager.dimensionLength = answer;
        }
        if (SimulationManager.isSimulating)
        {
            dimensionLine.SetActive(false);
            myPlayer.moveSpeed = Speed;
            timer.text = elapsed.ToString("f2") + "s";
            elapsed += Time.fixedDeltaTime;
            StartCoroutine(RagdollCollider());
            if (elapsed >= gameTime)
            {
                StartCoroutine(StuntResult());
                SimulationManager.isSimulating = false;
                RumblingManager.isCrumbling = true;
                rubblesStopper.SetActive(false);
                myPlayer.moveSpeed = 0;
                timer.text = gameTime.ToString("f2") + "s";
                StartCoroutine(ManholeCover());
                if ((answer == distance))
                {
                    myPlayer.slide = true;
                    messageText.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " is finally <b><color=green>safe</color></b>.";
                    SimulationManager.isAnswerCorrect = true;
                    myPlayer.transform.position = new Vector2(40, myPlayer.transform.position.y);
                }
                else
                {
                    HeartManager.ReduceLife();
                    if (SimulationManager.isRagdollActive)
                    {
                        myPlayer.lost = false;
                        SimulationManager.isRagdollActive = false;
                    }
                    else
                    {
                        myPlayer.lost = true;
                    }
                    SimulationManager.isAnswerCorrect = false;
                    currentPos = (40 - answer) + (Speed*gameTime);
                    if (answer < distance)
                    {
                        myPlayer.transform.position = new Vector2(currentPos + 0.4f, myPlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " ran too near from the manhole and " + pronoun + " stopped after the safe spot.\nThe correct answer is <color=red>" + distance + "m</color>.";
                    }
                    else
                    {
                        myPlayer.transform.position = new Vector2(currentPos - 0.4f, myPlayer.transform.position.y);
                        messageText.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + PlayerPrefs.GetString("Name") + " ran too far from the manhole and " + pronoun + " stopped before the safe spot.\nThe correct answer is <color=red>" + distance + "m</color>.";
                    }
                }
            }
        }
    }
    public void Stage3SetUp()
    {
        distance = 0;
        dimensionLine.SetActive(false);
        rubblesStopper.SetActive(true);
        AfterStuntMessage.SetActive(false);
        rubblesStopper.SetActive(true);
        slidePlatform.SetActive(true);
        lowerGround.SetActive(false);
        manholeCover.SetActive(true);
        if (playerGender == "Male")
        {
            pronoun = "he";
        }
        else
        {
            pronoun = "she";
        }
        float v = Random.Range(9f, 10f);
        Speed = (float)System.Math.Round(v, 2);
        float t = Random.Range(3f, 3.5f);
        gameTime = (float)System.Math.Round(t, 2);
        distance = (float)System.Math.Round((Speed * gameTime), 2);
        HeartManager.losslife = false;
        myPlayer.lost = false;
        myPlayer.standup = false;
        RumblingManager.shakeON = true;
        theCeiling.createQuadtilemap();
        safeZone.transform.position = new Vector2(40, -3);
        safeZone.GetComponent<BoxCollider2D>().enabled = false;
        ragdollSpawn.SetActive(true);
        ragdollSpawn.transform.position = new Vector2(43.65f, -3);
        timer.text = "0.00s";
        myPlayer.transform.position = new Vector2(0f, -3);
        elapsed = 0;
        SimulationManager.isSimulating = false;
        SimulationManager.question = "The entire ceiling is now crumbling and the only safe way out is for " + playerName + " to jump and slide down the manhole. If " + pronoun + " runs at constant velocity of <color=purple>" + Speed.ToString() + " meters per second</color> for exactly <color=#006400>" + gameTime.ToString() + " seconds</color>, how  <color=red>far</color> from the center of the manhole should " + playerName + " start running so that " + pronoun + " will fall down in it when " + pronoun + " stops?";
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1f);
        SimulationManager.directorIsCalling = true;
        SimulationManager.isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        AfterStuntMessage.SetActive(true);
        if ((answer == distance))
        {
            yield return new WaitForSeconds(3);
            Scorer.finalstar();
            AfterStuntMessage.SetActive(false);
        }
    }
    IEnumerator RagdollCollider()
    {
        yield return new WaitUntil(() => SimulationManager.isRagdollActive);
        ragdollSpawn.SetActive(false);
    }
    IEnumerator ManholeCover()
    {
        manholeCover.SetActive(false);
        yield return new WaitForSeconds(1f);
        manholeCover.SetActive(true);
    }
}

