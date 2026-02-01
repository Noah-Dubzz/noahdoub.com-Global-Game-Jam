using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public GameObject optionsPanel;

    // Add these to play the select SFX
    public AudioClip selectSfx;
    public AudioSource audioSource; // optional, will use PlayClipAtPoint if null


    public void StartGame()
    {

        StartCoroutine(PlaySfxThen(() => SceneManager.LoadScene("Game")));
    }

    public void OpenOptions()
    {
        // Send player to youtube video as placeholder for options menu
        StartCoroutine(PlaySfxThen(() => Application.OpenURL("https://youtu.be/4QBBtebTOdE")));
    }

    public void CloseOptions()
    {
        StartCoroutine(PlaySfxThen(() =>
        {
            if (optionsPanel != null) optionsPanel.SetActive(false);
        }));
    }

    public void QuitGame()
    {
        StartCoroutine(PlaySfxThen(QuitNow));
    }

    private void QuitNow()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private System.Collections.IEnumerator PlaySfxThen(Action callback)
    {
        float waitTime = 0f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuSelect();
            waitTime = AudioManager.Instance.MenuSelectDuration;
        }
        else if (selectSfx != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(selectSfx);
            }
            else
            {
                var pos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
                AudioSource.PlayClipAtPoint(selectSfx, pos);
            }
            waitTime = selectSfx.length;
        }

        if (waitTime > 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }
        else
        {
            // slight frame delay so UI feels responsive even without a clip
            yield return null;
        }

        callback?.Invoke();
    }
}
