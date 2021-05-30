using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LineAndMapGenerator : MonoBehaviour
{
    //[SerializeField]
    List<Vector2> positions = new List<Vector2>();
    [SerializeField]
    GameObject pointPrefab;
    [SerializeField]
    SpriteShapeController splineObject;
    [SerializeField]
    float amplitude = 3f;
    [SerializeField]
    float pointsXoffset = 3f;

    Vector2 currentPointPos = new Vector2(0f, -6f);
    int LastPointX;
    Spline spl = new Spline();

    void Start()
    {
        spl.isOpenEnded = true;
        positions.Add(currentPointPos);
        GeneratePoints(10);
        //spl = new Spline();
        foreach (Vector2 pointPos in positions){
            GameObject pointObject = Instantiate(pointPrefab);
            pointObject.transform.position = pointPos;
        }
        splineObject.spline = spl;
        //GenerateLine();
    }
    public void GeneratePoints(int count, bool removeOldPoints = false)
    {
        if (removeOldPoints) positions.RemoveRange(0, count);
        for (int i = 0; i < count; i++)
        {
            currentPointPos = new Vector2(Random.Range(-amplitude, amplitude), currentPointPos.y + pointsXoffset);
            positions.Add(currentPointPos);
            Vector3 currPointPosV3 = new Vector3(currentPointPos.x, currentPointPos.y);
            spl.InsertPointAt(i, currPointPosV3);
            spl.SetTangentMode(i, ShapeTangentMode.Continuous);
            spl.SetLeftTangent(i, new Vector3(0f,-4f));
            spl.SetRightTangent(i, new Vector3(0f, 4f));
        }
    }

    /*void GenerateLine()
    {
        LineRenderer lineRenderer = LineObj.GetComponent<LineRenderer>();

        lineRenderer.positionCount = positions.Count;

        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(positions[i].x, positions[i].y, 0f));
        }
    }*/
}
