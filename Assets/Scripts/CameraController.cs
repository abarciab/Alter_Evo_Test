using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset, targetOffset;
    [SerializeField] float moveSmoothness = 0.05f, rotSmoothness = 0.1f;
    Vector3 lookPos;

    private void Start()
    {
        lookPos = player.TransformPoint(targetOffset);
    }

    void LateUpdate()
    {
        var targetPos = player.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);

        var targetTargetPos = player.TransformPoint(targetOffset);
        lookPos = Vector3.Lerp(lookPos, targetTargetPos, rotSmoothness * Time.deltaTime);
        transform.LookAt(lookPos);
    }
}
