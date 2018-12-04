using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ender : MonoBehaviour {

    public SpriteRenderer rend;
    public List<Sprite> sprites = new List<Sprite>();
    public Animator animator;

    public float fps;

	// Use this for initialization
	void Start () {
        rend = GetComponent<SpriteRenderer>();
        fps = 1 / fps;
	}

    public void End()
    {
        StartCoroutine(Movie());
    }

    public IEnumerator Movie()
    {
        int i = 0;
        foreach (Sprite sprite in sprites)
        {
            i++;
            rend.sprite = sprite;
            if (i == 19) animator.SetBool("fadeWhite", true);
            yield return new WaitForSeconds(fps);
        }

        yield return new WaitForSeconds(16);

        SceneManager.LoadScene("Menu");
    }
}
