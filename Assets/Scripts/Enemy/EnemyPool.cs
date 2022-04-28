using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
	[SerializeField] private List<GameObject> pooledObjects;
	[SerializeField] private GameObject objectToPool;
	[SerializeField] private int amountToPool;

	private void Start() 
	{
		CreateObjectToPool();
	}

	public void CreateObjectToPool()
	{
		pooledObjects = new	List<GameObject>();
		GameObject tmp;

		for (int i = 0; i < amountToPool; i++)
		{
			tmp = Instantiate(objectToPool);
			tmp.name = "zombie " + i.ToString();
			pooledObjects.Add(tmp);
		}
	}

	public GameObject GetPooledObject()
	{
		for (int i = 0; i < amountToPool; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
				print("activated: " + pooledObjects[i].name);
				return pooledObjects[i];
			}
		}
		return AddObjectToPool();
	}

	private GameObject AddObjectToPool()
	{
		GameObject tmp;
		tmp = Instantiate(objectToPool);
		tmp.name = "zombie " + pooledObjects.Count.ToString();
		pooledObjects.Add(tmp);

		return tmp;
	}
}