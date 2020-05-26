using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    public Vector3 RotateAmount;  // degrees per second to rotate in each axis. Set in inspector.

    private GameObject Center;
    private GameObject UP;
    private GameObject DOWN;
    private GameObject RIGHT;
    private GameObject LEFT;
    private GameObject RIGHTUP;
    private GameObject LEFTUP;
    private int NumColored = 0;
    public GameObject block;
    public GameObject block2;
    public GameObject blocks;
    public Material BlackMaterial;
    public Material GreyMaterial;
    public Material LineMaterial;
    public Material NormalLineMaterial;
    public int step = 9;
    private float speed = 0.02f;
    bool input = true;
    private Rigidbody rb;
    public Vector2 Destination;
    private Vector2 Destination2;
    private Vector2 Position;
    Sounds sounds;
    GameObject Manager;
    public int blackBlocks = 0;
    SpawnPlayer SP;
    Animator animator;
    GameObject CurrentLine;
    GameObject Lines;


    public enum Direction{ Up , Down , Vertical};
    public Direction LeftDirection = Direction.Vertical;
    public Direction RightDirection = Direction.Vertical;

   

    private void Start()
    {
        Destination = new Vector2(100, 100);
        blocks = GameObject.FindGameObjectWithTag("Blocks");
        Center = GameObject.FindGameObjectWithTag("Center");
        RIGHT = GameObject.FindGameObjectWithTag("RIGHT");
        LEFT = GameObject.FindGameObjectWithTag("LEFT");
        UP = GameObject.FindGameObjectWithTag("UP");
        DOWN = GameObject.FindGameObjectWithTag("DOWN");
        RIGHTUP = GameObject.FindGameObjectWithTag("RIGHTUP");
        LEFTUP = GameObject.FindGameObjectWithTag("LEFTUP");
        ClosestBlock();
        Center.transform.position = transform.position;
        rb = GetComponent<Rigidbody>();
        Manager = GameObject.FindGameObjectWithTag("GameController");
        sounds = Manager.GetComponent<Sounds>();
        Position = transform.position;
        animator = GetComponent<Animator>();
        Lines = GameObject.FindGameObjectWithTag("Lines");
    }
    public void Shrink()
    {
        animator.SetTrigger("Shrink");
        blocks.GetComponent<SpawnPlayer>().Shrink();
    }
    
    public bool Arrived()
    {
        GameObject wall = GameObject.FindGameObjectWithTag("TheWall");
        Wall wall_ = wall.GetComponent<Wall>();
        foreach(Vector2 pos in wall_.Holes)
        {
            if(Position == pos)
            {
                return true;
            }
        }

        return false;
    }
    public void Right()
    {
        if (input)
        {
            ResetColorLines();
            sounds.PlayMoveSound();
            if (RightDirection == Direction.Down)
            {
                StartCoroutine(MoveRightDOWN());
                Position.y--;

            }
            else if (RightDirection == Direction.Up)
            {
                StartCoroutine(MoveRightUP());
                Position.y++;

            }
            else
            {
                StartCoroutine(MoveRight());
                Position.x++;
                if (transform.position.x < -8.5f || transform.position.x > 8.5f)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    GetComponent<Explosion>().Explode();
                    sounds.PlayCrashSound();
                    Manager.GetComponent<LevelManager>().Crashed();
                }
            }
            input = false;
            DetermineCurrentLine();
            ColorLines();


        }
    }
    private void OnDestroy()
    {
        ResetColorLines();
    }
    public void DetermineCurrentLine()
    {
        foreach (Transform t in Lines.transform)
        {
            if(Position.x == t.position.x)
            {
                CurrentLine = t.gameObject;
                return;
            }
        }
    }
    public void ColorLines()
    {
        CurrentLine.GetComponent<Renderer>().material = LineMaterial;
    }
    public void ResetColorLines()
    {
        if(CurrentLine != null)
        {
            CurrentLine.GetComponent<Renderer>().material = NormalLineMaterial;
        }
    }
    public void Left()
    {
        if (input)
        {
            ResetColorLines();
            sounds.PlayMoveSound();
            if (LeftDirection == Direction.Down)
                {
                    StartCoroutine(MoveLeftDOWN());
                    Position.y--;
                }
                else if (LeftDirection == Direction.Up)
                {
                    StartCoroutine(MoveLeftUp());
                    Position.y++;
                }
                else
                {
                    StartCoroutine(MoveLeft());
                    Position.x--;
                if (transform.position.x < -8.5f || transform.position.x > 8.5f)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    GetComponent<Explosion>().Explode();
                    sounds.PlayCrashSound();
                    Manager.GetComponent<LevelManager>().Crashed();
                }
            }
                input = false;
                DetermineCurrentLine();
                ColorLines();

        }
    }
    public void AllBlack()
    {
        if (Arrived())
        {
            GetComponent<Renderer>().material = BlackMaterial;
        }
        SP = blocks.GetComponent<SpawnPlayer>();
        SP.AllBlack(BlackMaterial);
    }
    IEnumerator MoveUp()
    {
        for(int i=0; i<90/step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(UP.transform.position, Vector3.right, step);
        }
        input = true;
        Center.transform.position = transform.position;

    }
    IEnumerator MoveDown()
    {
        for (int i = 0; i < 90 / step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(DOWN.transform.position, Vector3.left, step);
        }
        input = true;
        Center.transform.position = transform.position;
        ClosestBlock();


    }
    IEnumerator MoveLeft()
    {
        for (int i = 0; i < 90 / step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(LEFT.transform.position, Vector3.forward, step);
        }
        input = true;
        Center.transform.position = transform.position;
        ClosestBlock();


    }
    IEnumerator MoveRight()
    {
        for (int i = 0; i < 90 / step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(RIGHT.transform.position, Vector3.back, step);
        }
        input = true;
        Center.transform.position = transform.position;
        ClosestBlock();


    }
    IEnumerator MoveRightUP()
    {
        for (int i = 0; i < 90 / step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(RIGHTUP.transform.position, Vector3.back, step);
        }
        input = true;
        Center.transform.position = transform.position;
        ClosestBlock();


    }
    IEnumerator MoveLeftUp()
    {
        for (int i = 0; i < 90 / step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(LEFTUP.transform.position, Vector3.forward, step);
        }
        input = true;
        Center.transform.position = transform.position;
        ClosestBlock();


    }

    IEnumerator MoveRightDOWN()
    {
        for (int i = 0; i < 90 / step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(LEFT.transform.position, Vector3.back, step);
        }
        input = true;
        Center.transform.position = transform.position;
        ClosestBlock();


    }
    IEnumerator MoveLeftDOWN()
    {
        for (int i = 0; i < 90 / step; i++)
        {
            yield return new WaitForSeconds(speed);
            transform.RotateAround(RIGHT.transform.position, Vector3.forward, step);
        }
        input = true;
        Center.transform.position = transform.position;
        ClosestBlock();


    }
    void HandleDirections()
    {
        SP = blocks.GetComponent<SpawnPlayer>();
        if (block == null)
            return;
        if ((transform.position - block.transform.position).magnitude > 1.5f)
        {
            LeftDirection = Direction.Vertical;
            RightDirection = Direction.Vertical;
            return;
        }
        int PlayerX = Mathf.RoundToInt(transform.position.x);
        int PlayerY = Mathf.RoundToInt(transform.position.y);
        int blockX = Mathf.RoundToInt(block.transform.position.x);
        int blockY = Mathf.RoundToInt(block.transform.position.y);
        if (block2 == null)
        {
            
            if (PlayerY == blockY)
            {
                if (PlayerX < blockX)
                {
                    LeftDirection = Direction.Down;
                    RightDirection = Direction.Up;
                    if(PlayerY == 0)
                    {
                        LeftDirection = Direction.Vertical;
                    }
                }
                else if (PlayerX > blockX)
                {
                    LeftDirection = Direction.Up;
                    RightDirection = Direction.Down;
                    if (PlayerY == 0)
                    {
                        RightDirection = Direction.Vertical;
                    }
                }
            }
            else if (PlayerY > blockY)
            {
                if (PlayerX > blockX)
                {
                    LeftDirection = Direction.Vertical;
                    RightDirection = Direction.Down;
                }
                else if (PlayerX < blockX)
                {
                    LeftDirection = Direction.Down;
                    RightDirection = Direction.Vertical;
                }

                if (PlayerX == blockX)
                {
                    LeftDirection = Direction.Vertical;
                    RightDirection = Direction.Vertical;
                }

            }
            Block block_ = block.GetComponent<Block>();
            if (!block_.isBlack && block_.GetComponent<Renderer>().material != GreyMaterial)
            {
                block_.ChangeColor(GreyMaterial);
                NumColored++;
                if(NumColored == SP.transform.childCount - SP.NumberOfInvisibleBlocks)
                {
                    Manager.GetComponent<LevelManager>().Bonus();
                }
            }
        }
        else //block2 exists
        {
            int block2X = Mathf.RoundToInt(block2.transform.position.x);
            int block2Y = Mathf.RoundToInt(block2.transform.position.y);
            if(block2Y > blockY)
            {
                if(block2Y == PlayerY)
                {
                    if(block2X < PlayerX)
                    {
                        LeftDirection = Direction.Up;
                        RightDirection = Direction.Vertical;
                    }
                    if (PlayerX < block2X)
                    {
                        LeftDirection = Direction.Vertical;
                        RightDirection = Direction.Up;
                    }
                }
                
            }
            if (block2Y < blockY)
            {
                if (blockY == PlayerY)
                {
                    if (blockX < PlayerX)
                    {
                        LeftDirection = Direction.Up;
                        RightDirection = Direction.Vertical;
                    }
                    if (PlayerX < blockX)
                    {
                        LeftDirection = Direction.Vertical;
                        RightDirection = Direction.Up;
                    }
                }
            }
            Block block_ = block.GetComponent<Block>();
            if (!block_.isBlack && block_.GetComponent<Renderer>().material != GreyMaterial)
            {
                block_.ChangeColor(GreyMaterial);
                NumColored++;
                if (NumColored == SP.transform.childCount - SP.NumberOfInvisibleBlocks)
                {
                    Manager.GetComponent<LevelManager>().Bonus();
                }
            }
            Block block_2 = block2.GetComponent<Block>();
            if (!block_2.isBlack && block_2.GetComponent<Renderer>().material != GreyMaterial)
            {
                block_2.ChangeColor(GreyMaterial);
                NumColored++;
                if (NumColored == SP.transform.childCount - SP.NumberOfInvisibleBlocks)
                {
                    Manager.GetComponent<LevelManager>().Bonus();
                }
            }


        }


    }
    void ClosestBlock()
    {
        
        block2 = null;
        Vector3 PlayerPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        float minDist = Mathf.Infinity;
        float currentDist;
        foreach(Transform obj in blocks.transform)
        {
            currentDist = (obj.position - PlayerPos).magnitude;
            if(currentDist < minDist)
            {
                block = obj.gameObject;
                minDist = currentDist;
            }           
        }
        
        foreach (Transform obj in blocks.transform)
        {
            if(obj.gameObject.Equals(block))
            {
                continue;
            }
            currentDist = (obj.position - PlayerPos).magnitude;
            if (Mathf.Abs((currentDist - (block.transform.position - PlayerPos).magnitude)) <= 0.05f)
            {
                block2 = obj.gameObject;
            };

        }
        
        HandleDirections();


    }
    


    void Update()
    {
        if (input)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.position.x > Screen.width/2)
                {
                    Right();
                }
                if (touch.position.x < Screen.width / 2)
                {
                    Left();
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {

                Left();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Right();
            }
        }
    }
}