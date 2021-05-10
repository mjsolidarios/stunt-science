using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VelocityMediumManager : MonoBehaviour
{
    StageManager sm = new StageManager();
    [SerializeField] GameObject boulder, directorsBubble, floor, afterStuntMessage;
    [SerializeField] LineRenderer EndOfAnnotation;
    Annotation distanceMeassure;
    Player myPlayer;
    [SerializeField] TMP_Text question, timer, level, errorTxtBox, directorsSpeech, messageTxt;
    [SerializeField] TMP_InputField answerField;
    [SerializeField] Button answerButton, nextButton, retryButton;
    [SerializeField] float playerVelocity, boulderVelocity, stuntTime, distance, jumpDistance;
    public int stage;
    Rigidbody2D boulderRB;
    float playerPos, playerAnswer, elapsed, currentPlayerPos, jumpTime;
    string playerName, playerGender, pronoun, pPronoun;
    bool isStartOfStunt, directorIsCalling, isAnswered, isAnswerCorrect, isEndOfStunt;
    // Start is called before the first frame update
    void Start()
    {
        myPlayer = FindObjectOfType<Player>();
        distanceMeassure = FindObjectOfType<Annotation>();
        boulderRB = boulder.GetComponent<Rigidbody2D>();
        playerName = PlayerPrefs.GetString("Name");
        playerGender = PlayerPrefs.GetString("Gender");
        sm.SetGameLevel(1);
        playerPos = myPlayer.gameObject.transform.position.x;
        if (playerGender == "Male")
        {
            pronoun = "he";
            pPronoun = "his";
        }
        else
        {
            pronoun = "she";
            pPronoun = "her";
        }
        level.text = sm.GetGameLevel();
        VeloMediumSetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (directorIsCalling)
            StartCoroutine(DirectorsCall());
        currentPlayerPos = myPlayer.transform.position.x;
        if (isAnswered)
        {
            if (playerAnswer == stuntTime)
            {
                nextButton.gameObject.SetActive(true);
                messageTxt.text = "<b><color=green>Stunt Successful!</color></b>\n\n\n" + playerName + " has jumped over the boulder <color=green>safely</color>!";
            }
            else
            {
                retryButton.gameObject.SetActive(true);
                if (playerAnswer < stuntTime)
                {

                    messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + playerName + " jumped too soon and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
                }

                else
                    messageTxt.text = "<b><color=red>Stunt Failed!</color></b>\n\n\n" + playerName + " jumped too late and hit the boulder.\nThe correct answer is <color=red>" + stuntTime + " seconds</color>.";
            }
            timer.text = elapsed.ToString("f2");
            elapsed += Time.deltaTime;
            myPlayer.moveSpeed = playerVelocity;
            boulderRB.velocity = new Vector2(-boulderVelocity, 0);
            if (playerAnswer < elapsed)
            {
                StartCoroutine(Jump());
            }
        }
        if (isEndOfStunt)
        {
            directorIsCalling = true;
            StartCoroutine(StuntResult());
            isEndOfStunt = false;
        }
    }
    void VeloMediumSetUp()
    {
        nextButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        timer.text = "0.00s";

        boulderVelocity = 0;

        switch (stage)
        {
            case 1:
                float Va = Random.Range(8f, 9f),
                    Vb = Random.Range(4f, 6f),
                    d = Random.Range(27f, 30f),
                    Da, Db, Dj, t, tm;

                playerVelocity = (float)System.Math.Round(Va, 2);
                boulderVelocity = (float)System.Math.Round(Vb, 2);
                distance = (float)System.Math.Round(d, 2);
                t = (2 * distance) / (3 * (playerVelocity + boulderVelocity));
                tm = distance / (playerVelocity + boulderVelocity);
                stuntTime = (float)System.Math.Round(t, 2);
                Da = playerVelocity * stuntTime;
                Db = boulderVelocity * stuntTime;
                Dj = distance - Da - Db;
                jumpTime = tm - t;
                myPlayer.jumpforce = 0.77f / jumpTime;

                jumpDistance = (float)System.Math.Round(Dj, 2);
                distanceMeassure.distance = distance;

                EndOfAnnotation.SetPosition(0, new Vector2(distance - 15, 0));
                EndOfAnnotation.SetPosition(1, new Vector2(distance - 15, 1.5f));

                myPlayer.moveSpeed = 0;
                myPlayer.transform.position = new Vector2(playerPos, 0);


                boulder.transform.position = new Vector2(distance - 15, boulder.transform.position.y);
                boulderRB.velocity = new Vector2(0, 0);

                question.text = playerName + " is instructed to run until the end of the scene while jumping over the rolling boulder. If " + pronoun + " is running at a velocity of <color=purple>" + playerVelocity + " meters per second</color> while an incoming boulder <color=red>" + distance + " meters</color> away is rolling at the velocity of <color=purple>" + boulderVelocity + "meters per second</color>, at how many <color=#006400>seconds</color> after will " + playerName + " jump if " + pronoun + " has to jump at exactly <color=red>" + jumpDistance + " meters</color> away from the boulder in order to jump over it safely?";
                break;
        }



    }
    public void PlayButton()
    {
        if (answerField.text == "")
        {
            errorTxtBox.text = "Please enter your answer!";
        }
        else
        {
            playerAnswer = float.Parse(answerField.text);
            answerButton.interactable = false;
            if (stage == 1)
            {
                isStartOfStunt = true;
                directorIsCalling = true;
                answerField.text = playerAnswer.ToString() + "s";
            }
            else if (stage == 2)
            {
                isStartOfStunt = true;
                directorIsCalling = true;
                answerField.text = playerAnswer.ToString() + "s";
            }
            // else
            // {
            //     stage3Flag = true;
            //     answerField.text = playerAnswer.ToString() + "m";
            // }
        }
    }
    public void RetryButton()
    {
        isAnswered = false;
        answerField.text = "";
        answerButton.interactable = true;
        myPlayer.gameObject.SetActive(true);
        afterStuntMessage.SetActive(false);
        VeloMediumSetUp();
    }
    public void NextButton()
    {
        myPlayer.SetEmotion("");
        //ragdollSpawn.SetActive(false);
        //StartCoroutine(resetPrefab());
        if (stage == 1)
        {
            stage = 2;
            //VelocityEasyStage1.gameObject.SetActive(false);
            //theManager2.gameObject.SetActive(true);
            VeloMediumSetUp();
        }
        else if (stage == 2)
        {
            stage = 3;
            VeloMediumSetUp();
            //theManager2.gameObject.SetActive(false);
        }
        else
        {
            sm.SetDifficulty(2);
        }
        //StartCoroutine(ExitStage());
    }
    public IEnumerator DirectorsCall()
    {
        directorIsCalling = false;
        if (isStartOfStunt)
        {
            isStartOfStunt = false;
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Lights!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Camera!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "Action!";
            yield return new WaitForSeconds(0.75f);
            directorsSpeech.text = "";
            directorsBubble.SetActive(false);
            //boulder.transform.position = new Vector2(boulder.transform.position.x - 0.5f, boulder.transform.position.y);
            isAnswered = true;
        }
        else
        {
            RumblingManager.shakeON = false;
            yield return new WaitForSeconds((35 / playerVelocity) - stuntTime);
            directorsBubble.SetActive(true);
            directorsSpeech.text = "Cut!";
            yield return new WaitForSeconds(1f);
            directorsBubble.SetActive(false);
            directorsSpeech.text = "";
            if (isAnswerCorrect)
            {
                if (stage == 3)
                    myPlayer.slide = true;
                else
                    myPlayer.happy = true;
            }
            else
            {
                //myPlayer.standup = true;
            }
            boulderVelocity = 0;
        }
    }
    IEnumerator StuntResult()
    {
        yield return new WaitForSeconds(1);
        directorIsCalling = true;
        isStartOfStunt = false;
        yield return new WaitForSeconds(3f);
        afterStuntMessage.SetActive(true);
    }
    void OnTriggerEnter(Collider other)
    {

    }
    IEnumerator Jump()
    {
        myPlayer.jump();
        yield return new WaitForSeconds(jumpTime);
        isAnswered = false;
        isEndOfStunt = true;
    }
}
