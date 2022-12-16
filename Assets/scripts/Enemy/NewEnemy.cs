using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public bool detected = false;

    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    [SerializeField] private Transform self;
    [SerializeField] private int triggerDistance;
    [SerializeField] private int triggerDistanceoff;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("squarePlayer").GetComponent<Transform>();
        self = GameObject.FindGameObjectWithTag("simpleEnemy").GetComponent<Transform>();
    }

    void Update()
    {
        var distance = Vector2.Distance(target.position, self.position);

        if (distance < triggerDistance && distance > 4)
        {
            detected = true;
        }

        if (detected)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

        if (distance > triggerDistanceoff || distance < 4)
        {
            detected = false;
        }
        
    }
}
