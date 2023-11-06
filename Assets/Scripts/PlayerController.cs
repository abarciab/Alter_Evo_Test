using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10, boostSpeed = 10, turnSpeed = 10, turnSmoothness = 0.5f, moveSmoothness = 0.05f;
    Vector3 moveDelta;
    float turnDelta;

    private void Update()
    {
        Vector3 inputDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) inputDir = transform.forward;
        if (Input.GetKey(KeyCode.S)) inputDir = -transform.forward;
        moveDelta = Vector3.Lerp(moveDelta, inputDir, moveSmoothness);
        float selectedSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : speed;
        transform.position += moveDelta * selectedSpeed * Time.deltaTime;

        float turnAmount = 0;
        if (Input.GetKey(KeyCode.D)) turnAmount = 1;
        if (Input.GetKey(KeyCode.A)) turnAmount = -1;
        turnDelta = Mathf.Lerp(turnDelta, turnAmount, turnSmoothness);
        transform.Rotate(transform.up, turnDelta * turnSpeed * Time.deltaTime) ;
    }

}
