using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LineAndMapGenerator : MonoBehaviour
{
    [SerializeField]
    Movement movement;

    [SerializeField]
    GameObject coinPrefab;
    [SerializeField]
    GameObject linePrefab;

    [SerializeField]
    float amplitude = Screen.width/2.5f;
    [SerializeField]
    float pointsYoffset = 3f;
    [SerializeField]
    int generateCount = 10;

    SpriteShapeController splineObject;
    Vector2 lastPointPosition = new Vector2(0f, -60f);
    Vector2 refreshPoint = new Vector2();

    void Start()
    {
        GeneratePoints(10, true);
    }
    public void GeneratePoints(int count = 10, bool first = false)
    {
        Vector2 currentPointPos = lastPointPosition;

        GameObject NewLineObject = Instantiate(linePrefab);
        splineObject = NewLineObject.GetComponent<SpriteShapeController>();

        Spline tempSpline = new Spline();
        tempSpline.isOpenEnded = true;

        List<Vector2> positions = new List<Vector2>();

        for (int i = 0; i < count; i++)
        {
            if (Random.Range(0, 1f) < .2f)
            {
                GameObject pointObject = Instantiate(coinPrefab);
                pointObject.transform.position = currentPointPos;
            }

            positions.Add(currentPointPos);
            Vector3 currPointPosV3 = new Vector3(currentPointPos.x, currentPointPos.y);
            tempSpline.InsertPointAt(i, currPointPosV3);
            tempSpline.SetTangentMode(i, ShapeTangentMode.Continuous);
            tempSpline.SetLeftTangent(i, new Vector3(0f, -4f));
            tempSpline.SetRightTangent(i, new Vector3(0f, 4f));

            lastPointPosition = currentPointPos;
            currentPointPos = new Vector2(Random.Range(-amplitude, amplitude), currentPointPos.y + pointsYoffset);
        }

        splineObject.spline = tempSpline;
        refreshPoint = positions[positions.Count / 2];

        if (first)
        {
            NewLineObject.SetActive(false);
            movement.UpdateLines(NewLineObject);
            GeneratePoints();
            GeneratePoints();
        }
        else
        {
            movement.UpdateLines(NewLineObject);
        }

    }
}
