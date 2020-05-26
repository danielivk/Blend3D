using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float cubeSize = 0.5f;
    public int cubesInRow = 2;
    public Material Material_;

    float cubesPivotDistance;
    Vector3 cubesPivot;
    private GameObject cam;
    private RotateCam RC;
    public float explosionForce = 50f;
    public float explosionRadius = 5f;
    public float explosionUpward = 0.4f;

    // Use this for initialization
    void Start()
    {

        cam = GameObject.FindGameObjectWithTag("MainCamera");
        RC = cam.GetComponent<RotateCam>();
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Explode();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Explode();
            other.gameObject.transform.SetParent(null);
            other.gameObject.AddComponent<Explosion>();
            Material mat = other.gameObject.GetComponent<Renderer>().material;
            Explosion ex = other.gameObject.GetComponent<Explosion>();
            ex.Material_ = mat;
            ex.Explode();
            GetComponent<Collider>().isTrigger = false;
            
        }

    }

    public void Explode()
    {
        //make object disappear
        gameObject.SetActive(false);
        if(this.CompareTag("Player"))
        {
            RC.Rotate();
        }
        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        //get explosion position
        Vector3 explosionPos = transform.position;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position + new Vector3(0f,0f,1f), explosionRadius, explosionUpward);
            }
        }

    }

    void createPiece(int x, int y, int z)
    {

        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.GetComponent<Renderer>().material = Material_;

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
    }

}