using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        // object to hold all members
        GameObject poolHolder = new GameObject(prefab.name + " pool");
        poolHolder.transform.parent = transform;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObject);

                newObject.SetParent(poolHolder.transform);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(position, rotation);
        }
    }

    public void ReuseObjectFollow(GameObject prefab, Vector3 position, Quaternion rotation, GameObject tempParent)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);
             
            objectToReuse.ReuseFollow(position, rotation, tempParent);
        }
    }

    public class ObjectInstance
    {
        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            //gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();

            }
        }

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            if (hasPoolObjectComponent)
            {
                poolObjectScript.OnObjectReuse();
            }

            gameObject.SetActive(true);           

            transform.position = position;
            transform.rotation = rotation;

            // делаем объект обратно непрозрачным
            MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                rend.enabled = true;
                Color meshColor = rend.material.color;
                rend.material.color = new Color(meshColor.r, meshColor.g, meshColor.b, 1);
            }

        }

        public void ReuseFollow(Vector3 position, Quaternion rotation, GameObject tempParent)
        {
            if (hasPoolObjectComponent)
            {
                poolObjectScript.OnObjectReuse();
            }

            gameObject.SetActive(true);

            Quaternion initRot = Quaternion.identity;

            gameObject.transform.parent = tempParent.transform;

            transform.position = tempParent.transform.position;
            transform.rotation = initRot;

            // делаем объект обратно непрозрачным
            MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                rend.enabled = true;
                Color meshColor = rend.material.color;
                rend.material.color = new Color(meshColor.r, meshColor.g, meshColor.b, 1);
            }
            
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }

    
}
