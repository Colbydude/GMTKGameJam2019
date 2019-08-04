using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartOnDeath : MonoBehaviour
{
    private void OnDestroy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
