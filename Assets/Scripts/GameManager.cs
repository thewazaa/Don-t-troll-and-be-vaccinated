using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerController trollController;
    public int total;
    public static int totalVaccinated = 0;
    public static int totalTrolled = 0;

    private static readonly List<PlayerController> playerControllers = new List<PlayerController>();

    private void Start()
    {
        trollController.gameObject.SetActive(false);

        for (int i = 0; i < total; i++)
        {
            PlayerController tmp = GameObject.Instantiate(playerController);
            tmp.gameObject.SetActive(false);
            tmp.name = $"{i}";
            playerControllers.Add(tmp);
        }

        for (int i = 0; i < total / 6; i++)
        {
            PlayerController tmp = playerControllers[0];
            playerControllers.RemoveAt(0);
            PlayerController.Init(tmp);
        }

        StartCoroutine(AddPlayer());
    }

    public IEnumerator AddPlayer()
    {
        while (1 == 1)
        {
            while (playerControllers.Count > 0)
            {
                yield return new WaitForSeconds(9);

                PlayerController tmp = playerControllers[0];
                playerControllers.RemoveAt(0);
                PlayerController.Init(tmp);
            }
            yield return new WaitForSeconds(10);
        }
    }

    public static void Queue(PlayerController playerController) => playerControllers.Add(playerController);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && trollController.gameObject.activeSelf == false)
            PlayerController.Init(trollController);
    }
}