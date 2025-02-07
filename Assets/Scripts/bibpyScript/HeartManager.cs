using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    //public GameObject[] hearts;
    public AudioSource bgm;
    public AudioSource Gameoversfx;
    public int life;
    public GameObject heartItem;
    public GameObject gameOverBG, startBG;
    public bool losslife;

    // Start is called before the first frame update
    void Start()
    {
        startbgentrance();
    }

    public void DestroyHearts()
    {
        foreach (Transform item in transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount < life)
        {
            for (int i = 0; i < 1; i++)
            {
                var heart = Instantiate(heartItem, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                heart.transform.parent = transform;
                heart.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
        else
        {
            int diff = transform.childCount - life;
            for (int i = 0; i < diff; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }

            if (life == 0)
            {
                Time.timeScale = 0.4f;

                StartCoroutine(actionreset());
                StartCoroutine(gameover());
            }
        }

    }
    IEnumerator actionreset()
    {
        yield return new WaitForSeconds(3);
        // TODO: Get data from playerprefs
        life = 3;
    }
    IEnumerator gameover()
    {
        bgm.Stop();
        Gameoversfx.Play();
        StartCoroutine(endBGgone());
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("LevelOne");
        Time.timeScale = 1f;
    }
    public IEnumerator endBGgone()
    {
        gameOverBG.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverBG.SetActive(false);
    }


    public IEnumerator startBGgone()
    {
        startBG.SetActive(true);
        yield return new WaitForSeconds(1);
        startBG.SetActive(false);

    }
    public void startbgentrance()
    {
        StartCoroutine(startBGgone());
    }
    public void ReduceLife()
    {
        if (losslife == false)
        {
            life -= 1;
            losslife = true;
        }
    }
}
