using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_Wander : Steering
{
    /// <summary>
    /// �ǻ��뾶
    /// </summary>
    public float wanderRadius;
    /// <summary>
    /// �ǻ�����
    /// </summary>
    public float wanderDistance;
    /// <summary>
    /// ÿ��ӵ�Ŀ����漴λ�Ƶ����ֵ��
    /// </summary>
    public float wanderJitter;

    public bool isPlaner;

    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;
    private Vector3 circleTarget;
    private Vector3 wanderTarget;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        isPlaner = vehicle.IsPlaner;
        circleTarget = new Vector3(wanderRadius * 0.707f, 0, wanderRadius * 0.707f);
    }

    public override Vector3 Force()
    {
        Vector3 randomDisplacement = new Vector3((Random.value - 0.5f) * 2 * wanderJitter,
                                                 (Random.value - 0.5f) * 2 * wanderJitter,
                                                 (Random.value - 0.5f) * 2 * wanderJitter);
        if (isPlaner) randomDisplacement.y = 0;
        //�����λ�Ƽӵ���ʼ���ϣ��õ��µı�����
        circleTarget += randomDisplacement;
        //��λ�ÿ��ܲ���Բ���ϣ�����ͶӰ��
        circleTarget = wanderDistance * circleTarget.normalized;
        //�����ֵ�������AI��ǰ��������Ҫת��Ϊ��������
        wanderTarget = vehicle.Velocity.normalized * wanderDistance + circleTarget + transform.position;

        desiredVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
        return (desiredVelocity - vehicle.Velocity);
    }
   
}
