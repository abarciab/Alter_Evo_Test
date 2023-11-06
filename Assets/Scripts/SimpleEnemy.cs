using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed, visionDistance, deathDist = 12, rotSmoothness = 0.05f;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    void Update()
    {

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < deathDist) Destroy(gameObject);

        bool inRange = dist < visionDistance;
        if (!inRange) {
            Rotate(player.position);
            return;
        }

        var dir = transform.position - player.position;
        dir.y = 0;
        var moveDelta = dir.normalized * Time.deltaTime * speed;
        Rotate(transform.position + moveDelta);
        
        transform.position += moveDelta;
    }

    void Rotate(Vector3 targetPos)
    {
        targetPos.y = transform.position.y;
        var currentRot = transform.rotation;
        transform.LookAt(targetPos);
        transform.rotation = Quaternion.Lerp(currentRot, transform.rotation, rotSmoothness);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, visionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, deathDist);
    }
}
