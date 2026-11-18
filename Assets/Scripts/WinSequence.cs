using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSequence : MonoBehaviour
{
    [SerializeField] private GameObject winParticles;

    private bool isWinning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null && !isWinning)
        {
            isWinning = true;

            if (winParticles != null)
            {
                Instantiate(winParticles, transform.position, Quaternion.identity);
            }

            StartCoroutine(WinRoutine());
        }
    }

    private IEnumerator WinRoutine()
  {
    yield return new WaitForSeconds(2f);

    LevelController levelController = FindAnyObjectByType<LevelController>();
    if (levelController != null)
    {
        levelController.Win();   
    }
    else
    {
        Debug.LogWarning("WinSequence: LevelController not found");
    }
  }
}
