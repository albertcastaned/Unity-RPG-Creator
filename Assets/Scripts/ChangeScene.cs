using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public Animator animator;

    private string nextScene;
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    public void FadeToLevel(string name)
    {
        nextScene = name;
        animator.SetTrigger(FadeOut);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(nextScene);
    }
}
