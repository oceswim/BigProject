using UnityEngine;

public class SpawnModel : MonoBehaviour
{
    public GameObject[] models;
    public GameObject[] spawner;
    private GameObject toSpawn;
    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        for(int i =0;i<models.Length; i++) 
        {
            if(GameManager.models.Contains(models[i].name))
            {
               
                toSpawn = models[i];
                GameObject temp = Instantiate(toSpawn,spawner[index].transform);
                Debug.Log("Spawning " + temp.name + " in position " + temp.transform.position + " parent is " + spawner[index].name);
                index++;
            }
        }

    }

}
