using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using GameConfig;
using UnityEngine.UI;

public class QuestionControllerVThree : MonoBehaviour
{
    float playerAnswer;
    private Transform baseComponent, problemBox, extraComponent, levelBadge;
    public bool answerIsCorrect = false, isModalOpen = true, isSimulating, nextStage, retried;
    public Color correctAnswerColor, givenColor, wrongAnswerColor;
    public Difficulty levelDifficulty;
    public int levelNumber, stage;
    public string levelName, modalTitle, question, timer;
    public TextColorMode colorMode;
    public UnitOf unitOf;
    string answerUnit, difficulty;
    int passedLevel;
    [SerializeField] bool timerOn = false, loaded = false;
    [SerializeField] TMP_InputField answerFieldHorizontal;
    [SerializeField] Transform difficultyName, stageName;
    [SerializeField] string modalText, errorText;
    [SerializeField] bool popupVisible, extraOn;

    [SerializeField]
    GameObject modalComponentHorizontal, popupComponentHorizontal, playButtonHorizontal, timerComponentHorizontal, problemBoxHorizontal,
            problemTextHorizontal, modalTextHorizontal, modalTitleHorizontal, wrongIconHorizontal, correctIconHorizontal;
    [SerializeField] TMP_Text popupTextHorizontal;
    [SerializeField] Button actionBtn;
    StageManager level = new StageManager();
    HeartManager life;

    // Start is called before the first frame update
    void Start()
    {
        baseComponent = transform.Find("Base");
        extraComponent = transform.Find("Base").Find("Extra");
        levelBadge = baseComponent.Find("LevelBadge");

        Transform[] components = { baseComponent, modalComponentHorizontal.transform, extraComponent };
        problemBox = baseComponent.Find("ProblemBox");
        stageName = problemBox.Find("StageBar2").Find("StageName");
        difficultyName = problemBox.Find("StageBar3").Find("DifficultyName");

        // givenColor = new Color32(0x73, 0x2b, 0xc2, 0xff);
        // correctAnswerColor = new Color32(150, 217, 72, 255);
        // wrongAnswerColor = new Color32(237, 66, 66, 255);

        levelName = level.GetGameLevel();
        switch (levelDifficulty)
        {
            case Difficulty.Easy:
                difficulty = level.GetDifficulty();
                break;
            case Difficulty.Medium:
                difficulty = level.GetDifficulty();
                break;
            case Difficulty.Hard:
                difficulty = level.GetDifficulty();
                break;
        }
        difficultyName.GetComponent<TMP_Text>().text = difficulty;
        levelNumber = level.GetLevelNum(levelName);

        life = FindObjectOfType<HeartManager>();
    }
    public void ActionBtn()
    {
        answerFieldHorizontal.text = "";
        if (answerIsCorrect)
            Next();
        else
            StartCoroutine(Retry());
        isModalOpen = false;
        timerOn = false;
    }
    public void ActivateResult(string message, bool isCorrect)
    {
        answerIsCorrect = isCorrect;
        isModalOpen = true;
        if (isCorrect)
        {
            actionBtn.gameObject.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Next";
            modalTitle = "Stunt Success!";
            modalText = message;
            SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Correct);
        }
        else
        {
            actionBtn.gameObject.transform.Find("BtnName").GetComponent<TMP_Text>().text = "Retry";
            modalTitle = "Stunt Failed!";
            modalText = message;
            SetColor(modalTitleHorizontal.GetComponent<TMP_Text>(), TextColorMode.Wrong);
        }
        actionBtn.interactable = true;
    }
    public void SetQuestion(string qstn)
    {
        question = qstn;
    }

    public float GetPlayerAnswer()
    {
        return this.playerAnswer;
    }
    public void SetAnswer()
    {
        if (answerFieldHorizontal.text == "")
        {
            StartCoroutine(IsEmpty());
        }
        else
        {
            timerOn = true;
            playerAnswer = float.Parse(answerFieldHorizontal.text);
            answerFieldHorizontal.text = playerAnswer + answerUnit;
            isSimulating = true;
        }
        extraOn = true;
    }
    public void Next()
    {
        if (stage == 1)
        {
            stage = 2;
            nextStage = true;
        }
        else if (stage == 2)
        {
            stage = 3;
            nextStage = true;
        }
        else
        {
            string difficulty = level.GetDifficulty();
            if (difficulty == "easy")
            {
                stage = 1;
                level.SetDifficulty(2);
            }
            else if (difficulty == "medium")
            {
                stage = 1;
                level.SetDifficulty(3);
            }
            else
            {
                passedLevel++;
                level.SetDifficulty(1);
                stage = 1;
                switch (level.GetGameLevel())
                {
                    case "Velocity":
                        level.SetGameLevel(2);
                        break;
                    case "Acceleration":
                        level.SetGameLevel(3);
                        break;
                    case "Free Fall":
                        level.SetGameLevel(4);
                        break;
                    case "Projectile Motion":
                        level.SetGameLevel(5);
                        break;
                    case "Circular Motion":
                        level.SetGameLevel(6);
                        break;
                    case "Forces":
                        level.SetGameLevel(7);
                        break;
                    case "Work":
                        level.SetGameLevel(8);
                        break;
                    case "Energy":
                        level.SetGameLevel(9);
                        break;
                    case "Power":
                        level.SetGameLevel(10);
                        break;
                    case "Momentum":
                        //Done
                        break;
                }
            }
            SceneManager.LoadScene("LevelSelection");
        }
    }
    IEnumerator Retry()
    {
        extraOn = false;
        answerFieldHorizontal.text = "";
        retried = true;
        if (stage == 1)
            stage = 1;
        else if (stage == 2)
            stage = 2;
        else
            stage = 3;
        yield return new WaitForEndOfFrame();
        isModalOpen = false;
    }

    IEnumerator IsEmpty()
    {
        popupVisible = true;
        errorText = "Please enter your answer!";
        yield return new WaitForSeconds(3);
        popupVisible = false;
        errorText = "";
    }

    public string Unit(UnitOf unitOf)
    {
        switch (unitOf)
        {
            case UnitOf.distance:
                answerUnit = "m";
                break;
            case UnitOf.time:
                answerUnit = "s";
                break;
            case UnitOf.velocity:
                answerUnit = "m/s";
                break;
            case UnitOf.acceleration:
                answerUnit = "m/s²";
                break;
            case UnitOf.angle:
                answerUnit = "°";
                break;
            case UnitOf.angularVelocity:
                answerUnit = "°/s";
                break;
            case UnitOf.force:
                answerUnit = "N";
                break;
            case UnitOf.mass:
                answerUnit = "kg";
                break;
            case UnitOf.work:
                answerUnit = "J";
                break;
            case UnitOf.energy:
                answerUnit = "kW";
                break;
            case UnitOf.power:
                answerUnit = "kWh";
                break;
            case UnitOf.momuntum:
                answerUnit = "kg•m/s";
                break;
        }
        return answerUnit;
    }
    public Color getHexColor(TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                return wrongAnswerColor;
            case TextColorMode.Correct:
                return correctAnswerColor;
            default:
                return givenColor;
        }
    }
    public void SetColor(TMP_Text target, TextColorMode mode)
    {
        switch (mode)
        {
            case TextColorMode.Wrong:
                target.color = wrongAnswerColor;
                break;
            case TextColorMode.Correct:
                target.color = correctAnswerColor;
                break;
            default:
                target.color = givenColor;
                break;
        }
    }
    void Update()
    {
        //levelNumber = level.GetLevelNum(levelName);
        correctIconHorizontal.SetActive(answerIsCorrect);
        wrongIconHorizontal.SetActive(!answerIsCorrect);
        popupComponentHorizontal.SetActive(popupVisible);
        popupTextHorizontal.SetText(errorText);
        modalComponentHorizontal.gameObject.SetActive(isModalOpen);
        modalTitleHorizontal.GetComponent<TMP_Text>().SetText(modalTitle);
        problemTextHorizontal.GetComponent<TMP_Text>().SetText(question);
        modalTextHorizontal.GetComponent<TMP_Text>().SetText(modalText);
        problemTextHorizontal.SetActive(!isModalOpen);
        // playButtonHorizontal.SetActive(!isModalOpen);
        answerFieldHorizontal.gameObject.SetActive(!isModalOpen);

        extraComponent.gameObject.SetActive(timerOn || popupVisible);
        playButtonHorizontal.SetActive(!timerOn);
        // timerComponentHorizontal.gameObject.SetActive(timerOn);
        timerComponentHorizontal.GetComponent<TMP_Text>().SetText(timer);

        problemBox.Find("StageBar1").Find("LevelName").GetComponent<TMP_Text>().SetText($"{levelName}");
        levelBadge.Find("LevelNumber").GetComponent<TMP_Text>().SetText($"{levelNumber}");
        stageName.GetComponent<TMP_Text>().SetText($"Stage {stage}");
        difficultyName.GetComponent<TMP_Text>().SetText($"{levelDifficulty}");
    }
}