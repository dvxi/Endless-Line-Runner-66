using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform Player;

    Vector2 targetPos;

    void Update()
    {
        targetPos = Vector2.Lerp(transform.position + new Vector3(0f, -14f), Player.position, Time.deltaTime / .5f);

        transform.position = new Vector3(0, targetPos.y + 14f, -10f);
    }
}
