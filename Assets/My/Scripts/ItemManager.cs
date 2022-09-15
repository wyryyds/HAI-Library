using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class Item
	{
		public uint itemId;  //uintΪ�޷������͡�
		public string itemName;
		public uint itemPrice;
	}
	public class ItemManager : ScriptableObject
	{

		public Item[] dataArray;
	}
}
