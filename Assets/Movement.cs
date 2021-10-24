using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float movementSpeed = 1;
    [Range(0.0f, 10.0f)]
    public float cooldownTime = 3f;

    [SerializeField]
    Slider cooldownSlider;
    [SerializeField]
    LineAndMapGenerator mapGen;
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    GameObject point;
    [SerializeField]
    GameObject swordObject;

    public List<GameObject> generatorCollider = new List<GameObject>();
    EdgeCollider2D currLineCollider;

    Animator anim;

    Vector2[] points = new Vector2[0];
    Vector2 prevPos = new Vector2(0,0);

    int currLineId = 1;
    int currPointId = 0;
    float cooldownValue = 0;
    int failedTries = 0;
    //bool freeSlide = false;

    private void Start()
    {
        anim = swordObject.transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "coin") Destroy(collision.gameObject);
    }

    void LateUpdate()
    {
        //Debug.Log(currLineCollider.points.Length);

        float angle = Vector2.Angle(new Vector2(0f, -1f), prevPos - new Vector2(transform.position.x, transform.position.y));
        int direction = 0;

        if (prevPos.x - transform.position.x > 0) direction = 1;
        else direction = -1;

        swordObject.transform.rotation = Quaternion.Lerp(swordObject.transform.rotation, Quaternion.Euler(0f, 0f, direction * angle), Time.deltaTime * 12f);
        prevPos = transform.position;


        //Debug.Log("lateupdate: " + currLineCollider.pointCount + " id: " + currLineId + " count: " + generatorCollider.Count);
        //if (currLineCollider && points.Length != currLineCollider.pointCount)
        if (currLineCollider && currLineCollider.pointCount > 10 && points.Length < 10)
        {
            Debug.Log("dd");
            points = currLineCollider.points;
            transform.position = currLineCollider.points[0];
        } else if (points.Length < 10 && currLineCollider && failedTries < 5) failedTries++;
        else if (points.Length < 10 && currLineCollider && failedTries > 5)
        {
            failedTries = 0;
            generatorCollider.RemoveAt(generatorCollider.Count - 1);
            mapGen.GeneratePoints(true);
        }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                gameObject.transform.position = Vector3.Lerp(transform.position, points[currPointId], Time.deltaTime / (Vector2.Distance(transform.position, points[currPointId]) * movementSpeed));

                if (Vector2.Distance(gameObject.transform.position, points[currPointId]) < 0.1f)
                {
                    if (currPointId < points.Length - 1)
                    {
                        currPointId++;
                    }
                    /*else if(generatorCollider.Count < 3) //koniec pierwszej linii
                    {
                        Debug.Log("1st end");
                        currPointId = 0;
                        currLineId = 1;
                        points = currLineCollider.points;
                        //transform.position = currLineCollider.points[0];
                        mapGen.GeneratePoints();
                    }*/
                    else //koniec kazdej linii
                    {
                        Debug.Log("end");
                        points = new Vector2[0];
                        mapGen.GeneratePoints();
                        currPointId = 0;
                        points = currLineCollider.points;
                        //transform.position = currLineCollider.points[0];
                    }
                }

                SwordHandle(true);
                //return;
            }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Debug.Log("value: " + cooldownValue);

            if (cooldownValue >= .98f)
            {
                anim.Play("Hit");
                cooldownValue = 0;
            }
        }

        SwordHandle(false);
    }

    public void UpdateLines(GameObject newLine)
    {
        //Debug.Log("updateLines: " + newLine.GetComponent<EdgeCollider2D>().pointCount);
        //StartCoroutine(delay2());
        Debug.Log("updateLines: " + newLine.GetComponent<EdgeCollider2D>().pointCount);
        if (generatorCollider.Count > 2)
        {
            Destroy(generatorCollider[0]);
            generatorCollider.RemoveAt(0);
        }
        generatorCollider.Add(newLine);
        if(generatorCollider.Count > 1) currLineCollider = generatorCollider[currLineId].GetComponent<EdgeCollider2D>();

        //Debug.Log(generatorCollider[currLineId].name + " | " + currLineCollider.pointCount);
    }

    IEnumerator delay2()
    {
        Debug.Log("pause");
        yield return new WaitForSeconds(1);
    }

    public void SwordHandle(bool raise)
    {
        if (raise)
        {
            cooldownValue += Time.deltaTime / cooldownTime;
            cooldownValue = Mathf.Clamp(cooldownValue, 0, 1f);
            cooldownSlider.value = cooldownValue;
        } else
        {
            cooldownValue -= Time.deltaTime / (cooldownTime * 3);
            cooldownValue = Mathf.Clamp(cooldownValue, 0, 1f);
            cooldownSlider.value = cooldownValue;
        }
    }
}
