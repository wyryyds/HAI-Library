using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_ForSeek : Steering
{
    /// <summary>
    /// Ŀ������
    /// </summary>
    public GameObject target;

    //Ԥ���ٶ�
    private Vector3 desiredVelocity;

    private Vehicle vehicle;
    private float maxSpeed;
    private bool isPlaner;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        isPlaner = vehicle.IsPlaner;

    }
    public override Vector3 Force()
    {
        desiredVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
        if (isPlaner) desiredVelocity.y = 0;

        return desiredVelocity - vehicle.Velocity;
        
    }
}
