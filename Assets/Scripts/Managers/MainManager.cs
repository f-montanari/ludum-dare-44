using UnityEngine;

public class MainManager : MonoBehaviour
{
    public MouseManager mouseManager;
    public CameraManager cameraManager;
    public GameManager gameManager;
    public Lifebar lifebar;
    private GameObject playerInstance;
    

    public void StartGame()
    {
        mouseManager.player = playerInstance.GetComponent<NavEntityBehaviour>();
        cameraManager.target = playerInstance.transform;
        cameraManager.StartChasing();
    }    

    public void AssignPlayer(GameObject player)
    {
        playerInstance = player;
        lifebar.SetTarget(player.GetComponent<NavEntityBehaviour>());
        gameManager.SetupPlayer(player);
    }
}
