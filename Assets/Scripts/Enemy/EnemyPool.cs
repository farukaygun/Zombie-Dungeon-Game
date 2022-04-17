using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
	public static EnemyPool instance;
	
	public  List<GameObject> pooledObjects;
	public  GameObject 		 objectToPool;
	private int 			 amountToPool;

	private void Awake()
	{
		instance = this;
	}

	private void Start() 
	{
		amountToPool = 50;
		CreateObjectToPool();
	}

	public void CreateObjectToPool()
	{
		pooledObjects = new	List<GameObject>();
		GameObject tmp;

		for (int i = 0; i < amountToPool; i++)
		{
			tmp = Instantiate(objectToPool);
			tmp.SetActive(false);
			pooledObjects.Add(tmp);
		}
	}

	public GameObject GetPooledObject()
	{
		for (int i = 0; i < amountToPool; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}
		return AddObjectToPool(1);
	}

	private GameObject AddObjectToPool(int amount)
	{
		GameObject tmp;
		tmp = Instantiate(objectToPool);
		pooledObjects.Add(tmp);

		return tmp;
	}
}