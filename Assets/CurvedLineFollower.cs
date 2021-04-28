using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurvedLineFollower : MonoBehaviour
{
    [SerializeField] TMP_Text angleDegree;
    [SerializeField] LineRenderer line, straightLine;
    public Rigidbody2D gear2;
    float angle;
    public static float arc, stage;
    // Start is called before the first frame update
    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 51;
        line.useWorldSpace = false;

        straightLine.positionCount = 3;
        straightLine.useWorldSpace = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(arc);
        if (Level5EasyManager.gear2Speed == 0)
            gear2.rotation = 0;
        gear2.angularVelocity = Level5EasyManager.gear2Speed;
        if (arc != 0)
        {
            angleDegree.text = "<color=#A15D04>" + System.Math.Round(arc, 2) + "</color>";
            if (stage == 1)
            {
                angleDegree.transform.position = new Vector3(line.GetPosition(25).x + 4, line.GetPosition(25).y + 1, 0);
            }else{
                angleDegree.transform.position = new Vector3(line.GetPosition(25).x -3.8f, line.GetPosition(25).y + 1f, 0);
            }

            straightLine.SetPosition(1, new Vector3(0, 0, 0));
            CreatePoints();
        }
        else
        {
            if (stage == 1)
                angleDegree.text = "<color=#9A0000>210°</color>";
            else
            {
                angleDegree.rectTransform.position = new Vector2(-6.7f, 2.3f);
                angleDegree.text = "<color=#9A0000>118°</color>";
            }
            line.enabled = false;
            straightLine.enabled = false;
        }
    }
    void CreatePoints()
    {
        line.enabled = true;
        straightLine.enabled = true;
        float x, y, xSL, ySL;
        switch (stage)
        {
            case 1:
                angle = -120;
                straightLine.SetPosition(0, new Vector3(-5.196152f, -3, 0));
                break;
            case 2:
                angle = -118;
                straightLine.SetPosition(0, new Vector3(-5.297686f, -2.816829f, 0));
                break;
        }
        for (int i = 0; i <= 50; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * 3f;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * 3f;
            xSL = Mathf.Sin(Mathf.Deg2Rad * angle) * 6f;
            ySL = Mathf.Cos(Mathf.Deg2Rad * angle) * 6f;

            line.SetPosition(i, new Vector3(x, y, 0));
            straightLine.SetPosition(2, new Vector3(xSL, ySL, 0));

            angle += ((float)System.Math.Round(arc, 2) / 50);
        }
    }
}
