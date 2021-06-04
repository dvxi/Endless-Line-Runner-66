using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Movement : MonoBehaviour
{
    [SerializeField]
    EdgeCollider2D generatorCollider;
    [Range(0.0f, 1.0f)]
    public float movementSpeed = 1;
    [SerializeField]
    GameObject point;

    Vector2[] points = new Vector2[0];
    int currPointId = 0;

    void LateUpdate()
    {
        if (points.Length != generatorCollider.points.Length)
        {
            points = generatorCollider.points;
            Debug.Log(points.Length);
            //GenPoints();
        }

        gameObject.transform.position = Vector3.Lerp(transform.position, points[currPointId], Time.deltaTime / (Vector2.Distance(transform.position, points[currPointId]) * movementSpeed));

        if(Vector2.Distance(gameObject.transform.position, points[currPointId]) < 0.2f)
        {
            if (currPointId < points.Length - 1) currPointId++;
        }
    }

    /*void GenPoints()
    {
        for (int i = 0; i < points.Length; i++)
        {
            GameObject objTemp = Instantiate(point);
            objTemp.transform.position = points[i];
        }
    }*/
}
