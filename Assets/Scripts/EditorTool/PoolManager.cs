using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAIPoolTool
{
    [AddComponentMenu("HAIPoolTool/PoolManager")]
    public class PoolManager : MonoBehaviour
    {
        /// <summary>
        /// �����ʵ��
        /// </summary>
        public static PoolManager Instance { get ; private set; }

        public List<PoolItem> itemsToPool;
        public Dictionary<string, PoolItem> itemDictionary;
        public Dictionary<string, int> poolMaxCountDic;
        public Dictionary<string, Queue<GameObject>> poolObjectDictionary;
        public Queue<GameObject> poolObjects;

        protected void Awake()
        {
            Instance = this;
            itemDictionary = new Dictionary<string, PoolItem>();
            poolObjectDictionary = new Dictionary<string, Queue<GameObject>>();
            poolMaxCountDic = new Dictionary<string, int>();
            for(int i=0;i<itemsToPool.Count;i++)
            {
                GameObject parentNode = new GameObject();
                parentNode.name = itemsToPool[i].tag;
                parentNode.transform.SetParent(this.transform);
                ObjPoolItemToPool(i,parentNode.transform);
                itemDictionary.Add(itemsToPool[i].tag, itemsToPool[i]);
                poolMaxCountDic.Add(itemsToPool[i].tag, itemsToPool[i].countToPool);
            }
        }
        /// <summary>
        /// ��̬������嵽�������
        /// </summary>
        /// <param name="tag">���������</param>
        /// <param name="poolItemObj">��Ӧ����Ϸ����</param>
        /// <param name="Count">��ʼ����</param>
        public void AddToPool(string tag,GameObject poolItemObj,int Count)
        {
            if(!poolObjectDictionary.ContainsKey(tag))
            {
                PoolItem item = new PoolItem(tag, poolItemObj, Count);
                itemsToPool.Add(item);
                GameObject parentNode = new GameObject();
                parentNode.name = tag;
                parentNode.transform.SetParent(this.transform);
                ObjPoolItemToPool(itemsToPool.Count - 1,parentNode.transform);
                itemDictionary.Add(tag, item);
                poolMaxCountDic.Add(tag, Count);
            }
        }
        /// <summary>
        /// �ӳ�����ȡ������Ϸ����
        /// </summary>
        /// <param name="tag">������</param>
        /// <returns></returns>
        public GameObject GetPoolObject(string tag)
        {
            if (poolObjectDictionary.ContainsKey(tag))
            {
                if (poolObjectDictionary[tag].Count > 0)
                {
                    var go = poolObjectDictionary[tag].Dequeue();
                    go.SetActive(value:true);
                    return go;
                }
                else
                {
                    ExpandPool(tag);
                    var go = poolObjectDictionary[tag].Dequeue();
                    go.SetActive(value: true);
                    return go;
                }
            }
            else
            {
                Debug.Log("No Pool Of This Name");
                return null;
            }
        }
        /// <summary>
        /// �ӳ�����ȡָ����������Ϸ����
        /// </summary>
        /// <param name="tag">������</param>
        /// <param name="Count">����</param>
        /// <returns></returns>
        public List<GameObject> GetPoolObjects(string tag,int Count)
        {
            List<GameObject> res=new List<GameObject>();
            if (poolObjectDictionary.ContainsKey(tag))
            {
                if (Count > poolObjectDictionary[tag].Count)
                {
                    ExpandPool(tag);
                }
                int tempCount = 0;
                while (tempCount++ < Count)
                {
                    var go = poolObjectDictionary[tag].Dequeue();
                    go.SetActive(value: true);
                    res.Add(go);
                }
                return res;
            }
            else
            {
                Debug.Log("No Pool Of This Name");
                return null;
            }
        }
        /// <summary>
        /// ȡ������ȫ������
        /// </summary>
        /// <param name="tag">������</param>
        /// <returns></returns>
        public List<GameObject> GetPoolAllObjects(string tag)
        {
            List<GameObject> res=new List<GameObject>();
            if (poolObjectDictionary.ContainsKey(tag))
            {
                while(poolObjectDictionary[tag].Count!=0)
                {
                    var go = poolObjectDictionary[tag].Dequeue();
                    go.SetActive(value: true);
                    res.Add(go);
                }
                return res;
            }      
            else
            {
                Debug.Log("No Pool Of This Name");
                return null;
            }
        }
        /// <summary>
        /// ���ն���������
        /// </summary>
        /// <param name="tag">����</param>
        /// <param name="ReGO">����</param>
        public void RecycleObjectToPool(string tag, GameObject ReGO)
        {
            if(poolObjectDictionary.ContainsKey(tag))
            {
                poolObjectDictionary[tag].Enqueue(ReGO);
                ReGO.SetActive(value: false);
            }
            else
            {
                Debug.Log("No Pool Of This Name,Recycling failed!");
            }
        }

        public void ClearAnyPool(string tag)
        {
            poolObjectDictionary[tag].Clear();
            itemDictionary.Remove(tag);
            poolMaxCountDic.Remove(tag);
        }
        public void ClearAllPool()
        {
            poolObjectDictionary.Clear();
            itemDictionary.Clear();
            poolMaxCountDic.Clear();
        }
        /// <summary>
        /// ����itemToPool list��ʼ�������
        /// </summary>
        /// <param name="index">itemToPool����</param>
        /// <param name="parentNode">hierarchy���ڵĸ��ڵ�</param>
        protected void ObjPoolItemToPool(int index,Transform parentNode)
        {
            var poolItem = itemsToPool[index];
            poolObjects = new Queue<GameObject>();
            for(int i=0;i<poolItem.countToPool;i++)
            {
                var go = Instantiate(poolItem.objToPool, parentNode, true);
                go.name = poolItem.tag;
                go.SetActive(value: false);
                poolObjects.Enqueue(go);
            }
            poolObjectDictionary.Add(poolItem.tag, poolObjects);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tag">�������</param>
        protected void ExpandPool(string tag)
        {
            var parentNode = transform.Find(tag).transform;
            int tempCount = 0;
            poolMaxCountDic[tag] <<= 1;
            while (tempCount++ < poolMaxCountDic[tag])
            {
                var _go = Instantiate(itemDictionary[tag].objToPool, parentNode, true);
                _go.name = tag;
                _go.SetActive(value: false);
                poolObjectDictionary[tag].Enqueue(_go);
            }
        }
    }
}
