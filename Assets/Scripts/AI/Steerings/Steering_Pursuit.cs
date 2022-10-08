using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_Pursuit : Steering
{
    public GameObject target;
    private Vector3 desireVelocity;
    private Vehicle vehicle;
    private float maxSpeed;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
    }

    public override Vector3 Force()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        //����׷����������֮��ļн�
        float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);
        //����нǴ���0����׷���߻���������ӱ���(���-1Ϊ��ȫ���)����ôֱ�����ӱ��߷���ǰ����
        if(Vector3.Dot(toTarget,transform.forward)>0&&relativeDirection<-0.95f)
        {
            desireVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
            return desireVelocity - vehicle.Velocity;
        }
        //����Ԥ��ʱ�䣬������׷�������ӱ��ߵľ��룬������׷�������ӱ��ߵ��ٶȺ�
        float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().Velocity.magnitude);

        desireVelocity = (target.transform.position + target.GetComponent<Vehicle>().Velocity * lookaheadTime - transform.position).normalized * maxSpeed;
        return desireVelocity - vehicle.Velocity;
    }
}
