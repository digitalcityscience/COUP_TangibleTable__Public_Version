using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NoiseAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject[] carsArray;
    [SerializeField]
    private GameObject[] prefabCars;



    [SerializeField]
    private int maxCarCount = 90;
    private int oldCount;

    public int randomCar;

    private int carSpeed;


    private void Start()
    {
        carsArray = new GameObject[maxCarCount];
        for (int i = 0; i < maxCarCount; i++)
        {
            randomCar = Random.Range(0, 3);
            GameObject car = Instantiate(prefabCars[randomCar], gameObject.transform.position, gameObject.transform.rotation);
            car.transform.SetParent(gameObject.transform);
            car.SetActive(false);
            carsArray[i] = car;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            carSpeed += 5;
            GlobalVariable.GlobalNoiseCarSpeed = carSpeed;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            carSpeed -= 5;
            GlobalVariable.GlobalNoiseCarSpeed = carSpeed;
        }
    }

    public void ToggleCarsActivity()
    {
        int count = GlobalVariable.GlobalNoiseCarVolume;
        if(count > oldCount)
        {
            for (int i = 0; i < count; i++)
            {
                if (!carsArray[i].activeSelf)
                {
                    carsArray[i].SetActive(true);
                }

                if (i+1 == count)
                {
                    oldCount = count;
                }
            }
        }
        else if(count == oldCount)
        {
            return;
        }
        else if (count < oldCount)
        {
            for (int i = count; i < oldCount; i++)
            {
                if (carsArray[i].activeSelf)
                {
                    carsArray[i].SetActive(false);
                }

                if(i+1 == oldCount)
                {
                    oldCount = count;
                }
            }
        }
        
    }



















    //[System.Serializable]
    //public class Pool
    //{
    //    public string tag;
    //    public GameObject prefab;
    //    public int size;
    //}

    //public PathCreator pathCreator;
    //public EndOfPathInstruction endOfPathInstruction;
    //public float randomStart;
    //[SerializeField]
    //private float carSpeed = 10;

    //public List<Pool> pools;
    //public Dictionary<string, Queue<GameObject>> poolDictionary;

    //private int oldCount = 0;


    //// Start is called before the first frame update
    //void Start()
    //{
    //    poolDictionary = new Dictionary<string, Queue<GameObject>>();

    //    foreach (Pool pool in pools)
    //    {
    //        Queue<GameObject> objectPool = new Queue<GameObject>();

    //        for (int i = 0; i < pool.size; i++)
    //        {
    //            GameObject obj = Instantiate(pool.prefab);
    //            obj.transform.SetParent(gameObject.transform);
    //            obj.SetActive(false);
    //            objectPool.Enqueue(obj);
    //        }

    //        poolDictionary.Add(pool.tag, objectPool);

    //    }

    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        SpawnFromPool("normal", 1);
    //    }

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        carSpeed += 5;
    //        GlobalVariable.GlobalCarSpeed = carSpeed;
    //    }
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        carSpeed -= 5;
    //        GlobalVariable.GlobalCarSpeed = carSpeed;
    //    }


    //}

    //public void SpawnFromPool (string tag, int count)
    //{
    //    GameObject objectToSpawn = poolDictionary[tag].Dequeue();
    //    if (!poolDictionary.ContainsKey(tag))
    //    {
    //        Debug.LogWarning("Pool with tag" + tag + " doesn´t exists.");
    //        return;
    //    }

    //    if(oldCount < count)
    //    {
    //        for (int i = 0; i <= count; i++)
    //        {
    //            randomStart = Random.Range(0, 500);

    //            objectToSpawn.SetActive(true);
    //            objectToSpawn.transform.position = pathCreator.path.GetPointAtDistance(randomStart, endOfPathInstruction);
    //            objectToSpawn.transform.rotation = pathCreator.path.GetRotationAtDistance(randomStart, endOfPathInstruction);

    //            poolDictionary[tag].Enqueue(objectToSpawn);
    //            print(i);
    //            if (i == count)
    //            {
    //                oldCount = count;
    //                print("OldCount:" +oldCount);
    //            }
    //        }

    //    }
    //    else if(oldCount > count)
    //    {
    //        for (int i = 0; i <= count; i++)
    //        {
    //            poolDictionary[tag].Enqueue(objectToSpawn);
    //            if (i == count)
    //            {
    //                oldCount = count;
    //                print("OldCount:" + oldCount);
    //            }
    //        }

    //    }



    //}

    //public void ReturnToPool(string tag)
    //{

    //}

}
