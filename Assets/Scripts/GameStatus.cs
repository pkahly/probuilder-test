using UnityEngine;
using UnityEngine.UI;

public enum Status
{
    running = 0,
    win = 1,
    lose = 2,
}

public class GameStatus : MonoBehaviour
{
    public GameObject loseImage;

    private Status status = Status.running;

    void Start()
    {
        loseImage.SetActive(false);
    }

    public Status GetStatus()
    {
        return status;
    }

    public void SetStatus(Status status)
    {
        this.status = status;

        if (status == Status.lose)
        {
            loseImage.SetActive(true);
        }
    }
}