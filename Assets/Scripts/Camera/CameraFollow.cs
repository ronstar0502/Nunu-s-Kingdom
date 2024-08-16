using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]private Transform player;
    public Vector3 offset;

    private void Start()
    {
        if(offset == Vector3.zero)
        {
            offset = transform.position-player.position;
        }
    }

    private void Update()
    {
        transform.position = player.position + offset;
    }
}
