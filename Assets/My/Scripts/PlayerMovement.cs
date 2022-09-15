using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�ƶ��ٶ�")]
    public float speed=12f;

    [Header("�������ٶ�")]
    public float gravity = -9.81f;

    [Header("������")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask GroundMask;

    [Header("��Ծ�߶�")]
    public float jumpHeight = 3f;

    private CharacterController controller;

    private Vector3 velocity;
    private bool isGround;
   
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        movement();
    }
    void movement()
    {
        //x��z���ƶ���
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //���ݽ�ɫ�ĳ�����л���x����z����ƶ�   

        //Vector3 move =new  Vector3(x, 0f, z); //����ĸ�ֵ��
        /*���︳ֵ�ĵ� vector3 �����������������ϵ�ģ����۽�ɫ�泯�κη��򣬶�ֻ������������ϵ��x���z�����ƶ���*/

        controller.Move(move * speed * Time.deltaTime);


        //����������y���ƶ���
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime );

        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, GroundMask);

        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //��Ծ
        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(-2 * jumpHeight * gravity); //v=sqrt��2gh����
        }
    }
}
