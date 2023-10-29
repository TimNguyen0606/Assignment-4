using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Text highScoreText;

    [Header("Blinky")]
    [SerializeField]
    private GameObject blinkyIcon;
    [SerializeField]
    private Text blinkyName;
    [SerializeField]
    private Text blinkyNickname;

    [Header("Pinky")]
    [SerializeField]
    private GameObject pinkyIcon;
    [SerializeField]
    private Text pinkyName;
    [SerializeField]
    private Text pinkyNickname;

    [Header("Inky")]
    [SerializeField]
    private GameObject inkyIcon;
    [SerializeField]
    private Text inkyName;
    [SerializeField]
    private Text inkyNickname;

    [Header("Clyde")]
    [SerializeField]
    private GameObject clydeIcon;
    [SerializeField]
    private Text clydeName;
    [SerializeField]
    private Text clydeNickname;

    [Header("Pellets")]
    [SerializeField]
    private GameObject tenPts;
    [SerializeField]
    private GameObject fiftyPts;

    [Header("Chase Sequence")]
    [SerializeField]
    private GameObject powerPellet;
    [SerializeField]
    private GameObject pacman;
    [SerializeField]
    private GameObject blinky;
    [SerializeField]
    private GameObject pinky;
    [SerializeField]
    private GameObject inky;
    [SerializeField]
    private GameObject clyde;

    [SerializeField]
    private RectTransform startPos;
    [SerializeField]
    private RectTransform endPos;
    [SerializeField]
    private RectTransform blinkyStartPos;
    [SerializeField]
    private RectTransform blinkyEndPos;
    [SerializeField]
    private RectTransform pinkyStartPos;
    [SerializeField]
    private RectTransform pinkyEndPos;
    [SerializeField]
    private RectTransform inkyStartPos;
    [SerializeField]
    private RectTransform inkyEndPos;
    [SerializeField]
    private RectTransform clydeStartPos;
    [SerializeField]
    private RectTransform clydeEndPos;

    [SerializeField]
    private GameObject blinkyEyes;
    [SerializeField]
    private GameObject pinkyEyes;
    [SerializeField]
    private GameObject inkyEyes;
    [SerializeField]
    private GameObject clydeEyes;

    [SerializeField]
    private GameObject blinkyBlue;
    [SerializeField]
    private GameObject pinkyBlue;
    [SerializeField]
    private GameObject inkyBlue;
    [SerializeField]
    private GameObject clydeBlue;

    [Header("Timing Variables")]
    [SerializeField]
    private float ghostIconDelay;
    [SerializeField]
    private float ghostNameDelay;

    [Space(10)]
    [SerializeField]
    private Text anyKeyText;

    private bool muteMusic;

    private void Start()
    {
        SetupStats();

        if (PlayerPrefs.GetInt("PacmanMute") == 1)
        {
            AudioListener.volume = 0;
            AudioListener.pause = true;
            muteMusic = true;
        }
        else
        {
            AudioListener.volume = 1;
            AudioListener.pause = false;
            muteMusic = false;
        }

        ClearUI();
        StartCoroutine(LoadUI());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }

        if (Input.anyKeyDown)
        {
            StartGame();
        }
    }

    private IEnumerator LoadUI() // loads UI in piece by piece with delays like in original
    {
        yield return new WaitForSeconds(1f); // initial delay



        yield return new WaitForSeconds(ghostNameDelay);

        blinkyIcon.SetActive(true);

        yield return new WaitForSeconds(ghostIconDelay);

        blinkyName.enabled = true;

        yield return new WaitForSeconds(ghostNameDelay);

        blinkyNickname.enabled = true;

        yield return new WaitForSeconds(ghostNameDelay);



        pinkyIcon.SetActive(true);

        yield return new WaitForSeconds(ghostIconDelay);

        pinkyName.enabled = true;

        yield return new WaitForSeconds(ghostNameDelay);

        pinkyNickname.enabled = true;

        yield return new WaitForSeconds(ghostNameDelay);



        inkyIcon.SetActive(true);

        yield return new WaitForSeconds(ghostIconDelay);

        inkyName.enabled = true;

        yield return new WaitForSeconds(ghostNameDelay);

        inkyNickname.enabled = true;

        yield return new WaitForSeconds(ghostNameDelay);



        clydeIcon.SetActive(true);

        yield return new WaitForSeconds(ghostIconDelay);

        clydeName.enabled = true;

        yield return new WaitForSeconds(ghostNameDelay);

        clydeNickname.enabled = true;

        yield return new WaitForSeconds(ghostIconDelay);

        tenPts.SetActive(true);
        fiftyPts.SetActive(true);

        StartCoroutine(PlayChaseSequence());
    }

    private IEnumerator PlayChaseSequence()
    {
        yield return new WaitForSeconds(ghostNameDelay); // initial delay

        powerPellet.SetActive(true);

        yield return new WaitForSeconds(ghostNameDelay);

        StartCoroutine(LerpObject(4f, pacman, startPos.anchoredPosition, endPos.anchoredPosition));
        pacman.SetActive(true);

        StartCoroutine(LerpObject(4f, blinky, blinkyStartPos.anchoredPosition, blinkyEndPos.anchoredPosition));
        blinky.SetActive(true);

        StartCoroutine(LerpObject(4f, pinky, pinkyStartPos.anchoredPosition, pinkyEndPos.anchoredPosition));
        pinky.SetActive(true);

        StartCoroutine(LerpObject(4f, inky, inkyStartPos.anchoredPosition, inkyEndPos.anchoredPosition));
        inky.SetActive(true);

        StartCoroutine(LerpObject(4f, clyde, clydeStartPos.anchoredPosition, clydeEndPos.anchoredPosition));
        clyde.SetActive(true);

        yield return new WaitForSeconds(4f);

        powerPellet.SetActive(false);

        pacman.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward); // turn pacman back around
        StartCoroutine(LerpObject(4f, pacman, endPos.anchoredPosition, startPos.anchoredPosition));

        blinkyEyes.SetActive(false);
        blinky.GetComponent<Image>().enabled = false;
        blinkyBlue.SetActive(true);
        StartCoroutine(LerpObject(4f, blinky, blinkyEndPos.anchoredPosition, blinkyStartPos.anchoredPosition));

        pinkyEyes.SetActive(false);
        pinky.GetComponent<Image>().enabled = false;
        pinkyBlue.SetActive(true);
        StartCoroutine(LerpObject(4f, pinky, pinkyEndPos.anchoredPosition, pinkyStartPos.anchoredPosition));

        inkyEyes.SetActive(false);
        inky.GetComponent<Image>().enabled = false;
        inkyBlue.SetActive(true);
        StartCoroutine(LerpObject(4f, inky, inkyEndPos.anchoredPosition, inkyStartPos.anchoredPosition));

        clydeEyes.SetActive(false);
        clyde.GetComponent<Image>().enabled = false;
        clydeBlue.SetActive(true);
        StartCoroutine(LerpObject(4f, clyde, clydeEndPos.anchoredPosition, clydeStartPos.anchoredPosition));

        yield return new WaitForSeconds(4f);

        pacman.SetActive(false);
        blinky.SetActive(false);
        pinky.SetActive(false);
        inky.SetActive(false);
        clyde.SetActive(false);

        StartCoroutine(FadeTextToPartialAlpha(1.5f, anyKeyText));
    }

    private IEnumerator LerpObject(float timeOfTravel, GameObject gameObject, Vector3 startPosition, Vector3 endPosition) // https://answers.unity.com/questions/1240045/how-to-smoothly-move-object-with-recttransform.html
    {
        float currentTime = 0;
        float normalizedValue;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, normalizedValue);
            yield return null;
        }
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i) // https://forum.unity.com/threads/fading-in-out-gui-text-with-c-solved.380822/
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0.1f);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
        StartCoroutine(FadeTextToPartialAlpha(1.5f, anyKeyText));
    }

    public IEnumerator FadeTextToPartialAlpha(float t, Text i) // https://forum.unity.com/threads/fading-in-out-gui-text-with-c-solved.380822/
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.1f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha(1.5f, anyKeyText));
    }

    private void ClearUI() // hides all of the UI that is going to be revealed
    {
        blinkyIcon.SetActive(false);
        blinkyName.enabled = false;
        blinkyNickname.enabled = false;

        pinkyIcon.SetActive(false);
        pinkyName.enabled = false;
        pinkyNickname.enabled = false;

        inkyIcon.SetActive(false);
        inkyName.enabled = false;
        inkyNickname.enabled = false;

        clydeIcon.SetActive(false);
        clydeName.enabled = false;
        clydeNickname.enabled = false;

        tenPts.SetActive(false);
        fiftyPts.SetActive(false);

        powerPellet.SetActive(false);
        pacman.SetActive(false);
        blinky.SetActive(false);
        pinky.SetActive(false);
        inky.SetActive(false);
        clyde.SetActive(false);
    }

    private void StartGame() // loads game scene
    {
        SceneManager.LoadScene("GameScene");
    }

    private void SetupStats()
    {
        if (!PlayerPrefs.HasKey("PacmanHighScore"))
        {
            PlayerPrefs.SetInt("PacmanHighScore", 0);
        }
        if (!PlayerPrefs.HasKey("PacmanHighLevel"))
        {
            PlayerPrefs.SetInt("PacmanHighLevel", 0);
        }
        if (!PlayerPrefs.HasKey("PacmanMute"))
        {
            PlayerPrefs.SetInt("PacmanMute", 0);
        }

        SetHighScoreText();
    }

    private void SetHighScoreText()
    {
        highScoreText.text = PlayerPrefs.GetInt("PacmanHighScore").ToString() + " L" + PlayerPrefs.GetInt("PacmanHighLevel").ToString();
    }

    private void ToggleMute()
    {
        if (!muteMusic)
        {
            muteMusic = true;
            AudioListener.volume = 0;
            AudioListener.pause = true;
            PlayerPrefs.SetInt("PacmanMute", 1);
        }
        else
        {
            muteMusic = false;
            AudioListener.volume = 1;
            AudioListener.pause = false;
            PlayerPrefs.SetInt("PacmanMute", 0);
        }
    }
}
