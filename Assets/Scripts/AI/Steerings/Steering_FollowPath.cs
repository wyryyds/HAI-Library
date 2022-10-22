using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_FollowPath : Steering
{
    /// <summary>
    /// �ڵ�����ʾ·��
    /// </summary>
    public GameObject[] wayPoints;
    /// <summary>
    /// Ŀ���
    /// </summary>
    private Transform target;
    /// <summary>
    /// ��ǰ·��
    /// </summary>
    private int currentNode;
    /// <summary>
    /// �ִ�뾶
    /// </summary>
    public float arriveDistance;
    private float sqrArriveDistance;

    /// <summary>
    /// ·������
    /// </summary>
    private int numberOfNodes;

    private Vehicle vehicle;
    private Vector3 desiredVelocity;
    private Vector3 force;
    private float maxSpeed;
    private bool isPlaner;
    private float stopDistance;
    private void Start()
    {
        numberOfNodes = wayPoints.Length;
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        isPlaner = vehicle.IsPlaner;
        currentNode = 0;
        target = wayPoints[currentNode].transform;
        arriveDistance = 1.0f;
        sqrArriveDistance = arriveDistance * arriveDistance;
    }
    public override Vector3 Force()
    {
        force = new Vector3(0f, 0f, 0f);
        Vector3 dist = target.position - transform.position;
        if (isPlaner) dist.y = 0;
        if(currentNode==numberOfNodes-1)
        {
            if(dist.magnitude>stopDistance)
            {
                desiredVelocity = dist.normalized * maxSpeed;
                force = desiredVelocity - vehicle.Velocity;
            }
            else
            {
                desiredVelocity = dist - vehicle.Velocity;
                force = desiredVelocity - vehicle.Velocity;
            }
        }
        else
        {
            if(dist.sqrMagnitude<arriveDistance)
            {
                currentNode++;
                target = wayPoints[currentNode].transform;
            }
            desiredVelocity = dist.normalized * maxSpeed;
            force = desiredVelocity - vehicle.Velocity;
        }
        return force;
    }
}
