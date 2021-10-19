using UnityEngine;
using System;
using System.Collections;
public class Mover : MonoBehaviour
{
    float originalY;
    public float Strength = 1;
    void Start()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0 , 0 , Time.deltaTime * 50));
        transform.position = new Vector3(transform.position.x,
        originalY + ((float)Math.Sin(4 * Time.time) * Strength),
        transform.position.z);
    }
}