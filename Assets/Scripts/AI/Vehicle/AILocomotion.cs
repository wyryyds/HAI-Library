using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILocomotion : Vehicle
{
    private Rigidbody _rigidbody;
    private CharacterController controller;
    /// <summary>
    /// �ƶ��ľ���
    /// </summary>
    private Vector3 moveDistance;

     protected override void Start()
    {
        controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();

        moveDistance = Vector3.zero;
        base.Start();
    }

    private void FixedUpdate()
    {
        Velocity += acceleration * Time.fixedDeltaTime;

        //��������ٶ�
        Velocity = Velocity.sqrMagnitude > sqrMaxspeed ? Velocity.normalized * MaxSpeed : Velocity;
        if (acceleration.sqrMagnitude == 0) Velocity *= 0.9f;
        moveDistance = Velocity * Time.fixedDeltaTime;
        //�Ƿ�ֻ��ƽ�����˶����ǾͰ�y����0��
        if(IsPlaner)
        {
            Velocity.y = 0;
            moveDistance.y = 0;
        }
        //����н�ɫ����������ȡ��ɫ���������ƶ�
        if(controller)
        {
            controller.SimpleMove(Velocity);
        }
        //û�н�ɫ��������û�и�������Ǿ�̬����
        else if(_rigidbody==null||_rigidbody.isKinematic)
        {
            transform.position += moveDistance;
        }
        //���嶯��ѧ��ʽ�˶�
        else
        {
            _rigidbody.MovePosition(_rigidbody.position + moveDistance);
        }

        //���³���
        if(Velocity.sqrMagnitude>1e-5)
        {
            Vector3 newForward = Vector3.Slerp(transform.forward, Velocity, damping);
            if (IsPlaner) newForward.y = 0;

            transform.forward = newForward;
        }
        //TODO �����ȵ�.....
    }
}
