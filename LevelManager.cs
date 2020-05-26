using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public float speed = 10f;
    public float maxSpeed = 50f;
    public float spawnDelay = 2f;
    public int LevelLength = 5;
    public static int SceneCount = 5;
    public GameObject FullWall;
    public static int Score = 0;
    public static int HighestScore;
    private TextMeshProUGUI text;
    public GameObject[] Shapes;
    private GameObject CurrentShape;
    private Sounds sounds;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI HighestScoreText;
    public bool CanSpawn = true;
    GameObject bonus;
    Animator animator;
    GameObject TryAgainButton;
    GameObject wall;

    void OnDestroy()
    {
        PlayerPrefs.SetInt("highscore", HighestScore);
        PlayerPrefs.Save();
    }
    void Start()
    {
        Application.targetFrameRate = 120;
        HighestScore = PlayerPrefs.GetInt("highscore",0);
        if (CanSpawn)
        {
            bonus = GameObject.FindGameObjectWithTag("+5");
            TryAgainButton = GameObject.FindGameObjectWithTag("TryAgainButton");
            TryAgainButton.SetActive(false);
            animator = bonus.GetComponent<Animator>();
            StartCoroutine(Spawn());
            text = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
            text.text = Score.ToString();
        }
        
        sounds = GetComponent<Sounds>();
    }

    IEnumerator PassedWall()
    {
        Score ++;
        text.text = Score.ToString();
        yield return new WaitForSeconds(1f);
        if (CanSpawn)
        {
            StartCoroutine(Spawn());
        }
        if (speed < maxSpeed)
        {
            speed += 0.5f;
        }



    }
    public void ExitGame()
    {
        PlayerPrefs.SetInt("highscore", HighestScore);
        PlayerPrefs.Save();
        Application.Quit();
    }
    public void Passed()
    {
        StartCoroutine(PassedWall());
    }
    public void Crashed()
    {
        if(wall != null)
        {
            Destroy(wall);
        }
        CanSpawn = false;
        currentScoreText.SetText("Score: " + Score);
        if (HighestScore < Score)
        {
            HighestScore = Score;
            HighestScoreText.SetText("New Record: " + HighestScore + "!");

        }
        else
        {
            HighestScoreText.SetText("Record: " + HighestScore);
        }
        PlayerPrefs.SetInt("highscore", HighestScore);
        PlayerPrefs.Save();


        TryAgainButton.SetActive(true);

        Score = 0;
    }
    public void Bonus()
    {
        Score += 5;
        text.text = Score.ToString();
        sounds.PlayAllBlackSound();
        
        animator.SetTrigger("Bonus");
        GameObject wall = GameObject.FindGameObjectWithTag("TheWall");
        if(wall.transform.position.z >= 0)
        {
            Wall wall_ = wall.GetComponent<Wall>();
            for (int i=0; i<2;i++)
            {
                wall_.HolePosibilities();
            }

           
        }

        
    }
    IEnumerator Spawn()
    {
       
        int index;        
        index = Random.Range(0, Shapes.Length);
        if(CurrentShape != null)
        {
            CurrentShape.GetComponent<SpawnPlayer>().EndStage();
        }
        CurrentShape = Instantiate(Shapes[index], Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        wall = Instantiate(FullWall, transform.position, Quaternion.identity);
        Wall wall_ = wall.GetComponent<Wall>();
        wall_.SetSpeed(speed);
        Destroy(wall, 12f);
    }
    public void RandomizeScene()
    {
        int index = Random.Range(0, SceneCount);
        SceneManager.LoadScene(index);
    }
    public void RestartScene()
    {
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nextSceneIndex);

    }
    public void NextScene()
    {
       
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneCount)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    public void PreviousScene()
    {
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (nextSceneIndex >= 0)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            RestartScene();
        }
    }


}
