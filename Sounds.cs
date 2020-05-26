using UnityEngine;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    AudioSource AS;
    public AudioClip moveSound1;
    public AudioClip PassWall;
    public AudioClip CrashSound;
    public AudioClip AllBlack;
    public Sprite MutedImage;
    public Sprite UnMutedImage;
    public static int Muted = 1;
    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
        Muted = PlayerPrefs.GetInt("Muted", 1);
        if (Muted > 0)
        {
            GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Image>().sprite = MutedImage;

        }
        else
        {
            GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Image>().sprite = UnMutedImage;
        }

    }
    public void MuteOrUnMute()
    {

        Muted = -Muted;
        PlayerPrefs.SetInt("Muted", Muted);
        PlayerPrefs.Save();

        if (Muted > 0)
        {
            GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Image>().sprite = MutedImage;

        }
        else
        {
            GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Image>().sprite = UnMutedImage;
        }
    }
    public void PlayMoveSound()
    {
        if (Muted > 0)
        {
            AS.PlayOneShot(moveSound1);
        }
    }
    public void PlayPassWallSound()
    {
        if (Muted > 0)
        {
            AS.PlayOneShot(PassWall);
        }
    }
    public void PlayCrashSound()
    {
        if (Muted > 0)
        {
            AS.PlayOneShot(CrashSound);
        }
    }
    public void PlayAllBlackSound()
    {
        if (Muted > 0)
        {
            AS.PlayOneShot(AllBlack);
        }

    }


}
