using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Vector3 PlayerPosition;
    Vector3 BlocksRandomPosition;
    GameObject player;
    
    public int NumberOfInvisibleBlocks = 0;
    // Start is called before the first frame update
    void Start()
    {
        BlocksRandomPosition = new Vector3(Random.Range(-3, 3), 0f, 0f);
        transform.position = BlocksRandomPosition;
        player = Instantiate(PlayerPrefab, PlayerPosition + BlocksRandomPosition, Quaternion.identity);
        
    }
    public void Shrink()
    {
        foreach(Transform t in transform)
        {
            t.gameObject.GetComponent<Block>().Shrink();
        }
    }
    public void AllBlack(Material mat)
    {
        foreach(Transform t in transform)
        {
            t.gameObject.GetComponent<Block>().ChangeColor(mat);
        }
    }
   
    public void EndStage()
    {
        Destroy(player);
        Destroy(gameObject);
    }
    
}
