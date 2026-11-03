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

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Last scene in Build Settings: show the win screen if there is one
            WinScreen winScreen = FindAnyObjectByType<WinScreen>();

            if (winScreen != null)
            {
                winScreen.Show();
            }
            else
            {
                Debug.Log("Congratulations! You completed all levels!");
            }
        }
    }
}
