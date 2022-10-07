using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    public float Mass = 1f;

    /// <summary>
    /// ��Ϊ�б�
    /// </summary>
    public Steering[] Steerings;

    /// <summary>
    /// ����ٶ�
    /// </summary>
    [Range(5f,20f)]public float MaxSpeed;

    /// <summary>
    /// �����
    /// </summary>
    [Range(20f,100f)]public float MaxForce;

    /// <summary>
    /// ���������(��ά����)
    /// </summary>
    private Vector3 steeringForce;

    /// <summary>
    /// �ٶ�(��ά����)
    /// </summary>
    public Vector3 Velocity;

    /// <summary>
    /// ���ٶ�(��ά����)
    /// </summary>
    public Vector3 acceleration;

    private float timer;

    /// <summary>
    /// ���²�������ʱ����������һ֡��ʱ��ǳ��̣����ǲ�����Ҫ����ô�̵�ʱ����ȥʵʱ�������ǵĲ�������ͬʱ�����С�ĸ��µ��µĸ�������
    /// </summary>
    public float ComputeInterval = 0.2f;

    /// <summary>
    /// ��������ٶȵ�ƽ��,���ⷴ������ƽ���Ŀ���
    /// </summary>
    protected float sqrMaxspeed;

    /// <summary>
    /// �Ƿ�ֻ�ڵ�����
    /// </summary>
    public bool IsPlaner;

    /// <summary>
    /// ����ת����ٶ�
    /// </summary>
    public float damping=0.9f;
    protected virtual void Start()
    {
        steeringForce = Vector3.zero;
        Steerings = GetComponents<Steering>();
        timer = 0;
        sqrMaxspeed = MaxSpeed * MaxSpeed;
    }
    public void Update()
    {
        timer += Time.deltaTime;
        steeringForce = Vector3.zero;

        if(timer>ComputeInterval)
        {
            foreach(var _steering in Steerings)
            {
                if(_steering.enabled)
                steeringForce += _steering.Force() * _steering.Weight;
            }
            //���ⳬ������������ƴ�С��
            steeringForce = Vector3.ClampMagnitude(steeringForce, MaxForce);
            //�������
            acceleration = steeringForce / Mass;
            timer = 0;
        }
    }

}
