using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float speed = 10;
    public float Timer = 1f;
    bool Phase2 = false;
    private Rigidbody rb;
    private Transform CurrentShape;
    Animator animator;
    GameObject Player;
    GameObject Manager;
    LevelManager LM;
    public List<Vector2> Holes;
    Sounds sounds;
    bool PlaySound = true;
    bool Blackitup = true;
    Dictionary<int, GameObject> map = new Dictionary<int, GameObject>();

    private void Start()
    {
        Holes = new List<Vector2>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Manager = GameObject.FindGameObjectWithTag("GameController");
        LM = Manager.GetComponent<LevelManager>();
        sounds = Manager.GetComponent<Sounds>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Grow");
        rb.AddForce(new Vector3(0f, 0f, -speed), ForceMode.VelocityChange);
        CurrentShape = GameObject.FindGameObjectWithTag("Blocks").transform;
        foreach (Transform t in transform)
        {
            t.gameObject.tag = "Wall";
        }
        initMap();
    }
    void initMap()
    {
        int i, j;
        
        foreach (Transform ShapeCube in CurrentShape)
        {
            if (!ShapeCube.gameObject.GetComponent<MeshRenderer>().enabled)
            {
                continue;
            }
            i = Mathf.RoundToInt(ShapeCube.position.x);
            j = Mathf.RoundToInt(ShapeCube.position.y);
            Vector3 spawnPoint = new Vector3(i, j, transform.position.z);
            Collider[] hitColliders = Physics.OverlapBox(spawnPoint, new Vector3(0.3f, 0.3f, 0.3f));
            foreach(Collider col in hitColliders)
            {
                
                    Destroy(col.gameObject);                
            }
        }
        
        HolePosibilities();
    }
    
    public void HolePosibilities()
    {
        int x, y;


        bool ContinueSearch;
        int[] posibilities = new int[] { -1, 1, 0 };
        do
        {
            ContinueSearch = false;
            Transform RandomCubeTransform = CurrentShape.GetChild(Random.Range(0, CurrentShape.childCount));
            x = Mathf.RoundToInt(RandomCubeTransform.position.x);
            y = Mathf.RoundToInt(RandomCubeTransform.position.y);
            int VercialRandom = posibilities[Mathf.RoundToInt(Random.Range(0, 2))];
            int HorizontalRandom = posibilities[Mathf.RoundToInt(Random.Range(0, 2))];
            x += VercialRandom;
            y += HorizontalRandom;
            if (y < 0)
            {
                y = 0;
            }           
            Vector3 spawnPoint = new Vector3(x, y, 0f);
            Collider[] hitColliders = Physics.OverlapBox(spawnPoint, new Vector3(0.1f, 0.1f, 0.1f));
            if (hitColliders.Length > 0)
            {
                ContinueSearch = true;
            }
        } while (ContinueSearch);

        // x,y object is ready to be deleted.
        Vector3 spawnPoint2 = new Vector3(x, y, transform.position.z);
        Collider[] hitColliders2 = Physics.OverlapBox(spawnPoint2, new Vector3(0.1f, 0.1f, 0.1f));
        foreach(Collider col in hitColliders2)
        {            
            Destroy(col.gameObject);
        }
        
        Holes.Add(new Vector2(x, y));


    }
    


    public void SetSpeed(float amount)
    {
        speed = amount;
    }
    
    private void Update()
    {
        Timer -= Time.deltaTime;
        if(!Phase2)
        {
            if (Timer <= 0)
            {
                rb.AddForce(new Vector3(0f, 0f, -speed), ForceMode.VelocityChange);
                Timer = 1;
                Phase2 = true;
            }
        }       
        if(Phase2)
        {
            if (Timer <= 0)
            {
                rb.AddForce(new Vector3(0f, 0f, -0.5f*speed), ForceMode.VelocityChange);
                Timer = 30;
            }
        }
        if(Blackitup && transform.position.z < 15f)
        {
            Blackitup = false;
            Player.GetComponent<Movement>().AllBlack();
        }
        if(transform.position.z <=0 && PlaySound)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            this.tag = "OldWall";
            if (Player != null)
            {
                if (Player.activeSelf)
                {
                    LM.Passed();
                    sounds.PlayPassWallSound();
                    Player.GetComponent<Movement>().Shrink();
                    Player.GetComponent<Movement>().ResetColorLines();


                }
                else
                {
                    LM.Crashed();
                    sounds.PlayCrashSound();
                }
            }
            else
            {
                LM.Crashed();
                sounds.PlayCrashSound();
            }
           
            PlaySound = false;
        }
    }

}
