using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Movement : MonoBehaviour
{
    List<GameObject> generatorCollider = new List<GameObject>();
    [Range(0.0f, 1.0f)]
    public float movementSpeed = 1;
    [SerializeField]
    GameObject point;
    [SerializeField]
    LineAndMapGenerator mapGen;

    Vector2[] points = new Vector2[0];
    int currLineId = 0;
    int currPointId = 0;
    EdgeCollider2D currLineCollider;

    void LateUpdate()
    {
        Debug.Log(currLineId);
        if (currLineCollider && points.Length != currLineCollider.points.Length)
        {
            points = currLineCollider.points;
            transform.position = currLineCollider.points[0];
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
                else if(generatorCollider.Count < 3) //koniec pierwszej linii
                {
                    currPointId = 0;
                    currLineId = 1;
                    points = currLineCollider.points;
                    //transform.position = currLineCollider.points[0];
                    mapGen.GeneratePoints(10, new Vector2(0f, 0f));
                }
                else //koniec kazdej kolejnej linii
                {
                    mapGen.GeneratePoints(10, new Vector2(0f,0f));
                    currPointId = 0;
                    points = currLineCollider.points;
                    //transform.position = currLineCollider.points[0];
                }
            }
        }
    }

    public void UpdateLines(GameObject newLine)
    {
        if (generatorCollider.Count > 2)
        {
            Destroy(generatorCollider[0]);
            generatorCollider.RemoveAt(0);
        }
        generatorCollider.Add(newLine);
        currLineCollider = generatorCollider[currLineId].GetComponent<EdgeCollider2D>();
    }
}
