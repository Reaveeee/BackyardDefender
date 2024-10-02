using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TryAgainButton : MonoBehaviour
{
    Vector2 size;
    float alpha;
    Image bgImage;
    GameObject popUpImage;
    private void Start()
    {
        popUpImage = GameObject.Find("GameOverPopUpBG");
        bgImage = GameObject.Find("Background").GetComponent<Image>();
        size = popUpImage.transform.localScale;
        alpha = 0.5f;

        popUpImage.transform.localScale = Vector2.zero;
        bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0);

        LeanTween.scale(popUpImage, size, 1).setEaseOutCubic();
        LeanTween.alpha(bgImage.gameObject.GetComponent<RectTransform>(), alpha, .5f).setEaseInCubic();
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("InGame");
    }
}
