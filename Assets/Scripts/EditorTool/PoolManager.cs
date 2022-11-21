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
        public Dictionary<string, Queue<GameObject> > poolObjectDictionary;
        public Dictionary<string, Queue<GameObject>> outOfPoolObjDic;
        public Queue<GameObject> poolObjects;

        protected void Awake()
        {
            Instance = this;
            itemDictionary = new Dictionary<string, PoolItem>();
            poolObjectDictionary = new Dictionary<string, Queue<GameObject>>();
            poolMaxCountDic = new Dictionary<string, int>();
            outOfPoolObjDic = new Dictionary<string, Queue<GameObject>>();
            for(int i=0;i<itemsToPool.Count;i++)
            {
                GameObject parentNode = new GameObject();
                parentNode.name = itemsToPool[i].tag;
                parentNode.transform.SetParent(this.transform);
                ObjPoolItemToPool(i,parentNode.transform);
                itemDictionary.Add(itemsToPool[i].tag, itemsToPool[i]);
                poolMaxCountDic.Add(itemsToPool[i].tag, itemsToPool[i].countToPool);
                outOfPoolObjDic.Add(itemsToPool[i].tag, new Queue<GameObject>());
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
                outOfPoolObjDic.Add(tag, new Queue<GameObject>());
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
                    outOfPoolObjDic[tag].Enqueue(go);
                    go.SetActive(value:true);
                    return go;
                }
                else
                {
                    ExpandPool(tag);
                    var go = poolObjectDictionary[tag].Dequeue();
                    outOfPoolObjDic[tag].Enqueue(go);
                    go.SetActive(value: true);
                    return go;
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("No Pool Of This Name");
#endif
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
                    outOfPoolObjDic[tag].Enqueue(go);
                    go.SetActive(value: true);
                    res.Add(go);
                }
                return res;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("No Pool Of This Name");
#endif
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
                    outOfPoolObjDic[tag].Enqueue(go);
                }
                return res;
            }      
            else
            {
#if UNITY_EDITOR
                Debug.Log("No Pool Of This Name");
#endif
                return null;
            }
        }
        /// <summary>
        /// ���ն���������
        /// </summary>
        /// <param name="tag">����</param>
        /// <param name="ReGO">����</param>
        public void RecycleObjectToPool(string tag,GameObject poolObj)
        {
            if(poolObjectDictionary.ContainsKey(tag))
            {
                outOfPoolObjDic[tag].Dequeue();
                poolObjectDictionary[tag].Enqueue(poolObj);
                poolObj.transform.SetParent(this.transform.Find(tag));
                poolObj.SetActive(value: false);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("No Pool Of This Name,Recycling failed!");
#endif
            }
        }
        public void RecycleObjectsToPool(string tag)
        {
            if (poolObjectDictionary.ContainsKey(tag))
            {
                if (outOfPoolObjDic[tag].Count > 0)
                {
                    var poolObj = outOfPoolObjDic[tag].Dequeue();
                    poolObjectDictionary[tag].Enqueue(poolObj);
                    poolObj.transform.SetParent(this.transform.Find(tag));
                    poolObj.SetActive(value: false);
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("No Pool Of This Name,Recycling failed!");
#endif
            }
        }

        public void ClearAnyPool(string tag)
        {
            if (poolObjectDictionary.ContainsKey(tag))
            {
                while (outOfPoolObjDic[tag].Count > 0)
                {
                    var go = outOfPoolObjDic[tag].Dequeue();
                    Destroy(go);
                }
                outOfPoolObjDic.Remove(tag);

                for (int i = 0; i < itemsToPool.Count; i++)
                {
                    if (itemsToPool[i].tag == tag)
                    {
                        itemsToPool.RemoveAt(i);
                    }
                }
                poolObjectDictionary.Remove(tag);
                itemDictionary.Remove(tag);
                poolMaxCountDic.Remove(tag);
                Destroy(transform.Find(tag).gameObject);
            }
        }
        public void ClearAllPool()
        {
            for(int i=0;i<itemsToPool.Count;i++)
            {
                var tag = itemsToPool[i].tag;
                if(outOfPoolObjDic[tag]!=null)
                {
                    while(outOfPoolObjDic[tag].Count>0)
                    {
                        var go = outOfPoolObjDic[tag].Dequeue();
                        Destroy(go);
                    }
                }
                poolObjectDictionary.Remove(tag);
                outOfPoolObjDic.Remove(tag);
                Destroy(transform.Find(tag).gameObject);
            }
            poolObjectDictionary.Clear();
            outOfPoolObjDic.Clear();
            itemDictionary.Clear();
            poolMaxCountDic.Clear();
            itemsToPool.Clear();
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
