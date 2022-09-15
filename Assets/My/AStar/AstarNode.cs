using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstarNode : MonoBehaviour
{
    public int x;
    public int y;
    public float f;//g+h
    public float g;//����㵽�õ�Ĺ��ƴ���
    public float h;//�Ӹõ㵽�յ�Ĺ��ƴ���

    public int clickCount = 0; 

    public bool isObstacle = false;
    public bool isStart = false;
    public bool isEnd = false;

    public AstarNode parent;

    public SpriteRenderer sprite;
    public Text text;

    public List<AstarNode> Neighbor = new List<AstarNode>();

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        text = transform.Find("Canvas/Text")?.GetComponent<Text>();
        parent = null;
    }

    public void Update()
    {
        text.text = $"x:{x}      y:{y}\nf:{f}\ng:{g}\nh;{h}";
        SetNode(sprite);
    }
    /// <summary>
    /// ���ýڵ�״̬
    /// </summary>
    /// <param name="sprite"></param>
    private void SetNode(SpriteRenderer sprite)
    {
        switch(clickCount%6)
        {
            case 0:
                isObstacle = false;
                isStart = false;
                isEnd = false;
                sprite.color = Color.white;
                break;
            case 1:
                isObstacle = true;
                isStart = false;
                isEnd = false;
                sprite.color = Color.black;
                break;
            case 2:
                isObstacle = false;
                isStart = true;
                isEnd = false;
                sprite.color = Color.red;
                break;
            case 3:
                isObstacle = false;
                isStart = false;
                isEnd = true;
                sprite.color = Color.blue;
                break;
            case 4:
                sprite.color = Color.green;
                break;
            case 5:
                sprite.color = Color.yellow;
                break;
        }
        
    }
    /// <summary>
    /// �����ڵ�Ĵ���
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public static float EvaluationCost(AstarNode currentNode, AstarNode targetNode)
    {
        if (currentNode == null) return 0;
       return  Vector2.Distance(new Vector2(currentNode.x, currentNode.y),
        new Vector2(targetNode.x, targetNode.y));
    }
    /// <summary>
    /// ��ȡ�ܴ���
    /// </summary>
    public void GetCost(AstarNode sourceNode,AstarNode targetNode)
    {
        if (sourceNode != null)
        {
            g = sourceNode.g + EvaluationCost(sourceNode, this);
        }
        if (targetNode != null)
        {
            h = EvaluationCost(this, targetNode);
        }
        f = g + h;
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) clickCount++;
    }
}
