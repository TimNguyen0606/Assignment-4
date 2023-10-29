using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Ghost[] ghosts;
    [SerializeField]
    private Pacman pacman;
    [SerializeField]
    private Transform pellets;
    [SerializeField]
    private GameObject pacmanDeath;
    [SerializeField]
    private GameObject pacmanWin;

    [SerializeField]
    private GameObject ghostRetreatBlinky;
    [SerializeField]
    private GameObject ghostRetreatInky;
    [SerializeField]
    private GameObject ghostRetreatPinky;
    [SerializeField]
    private GameObject ghostRetreatClyde;

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject ready;
    [SerializeField]
    private Text gameOver;
    [SerializeField]
    private float startDelayTime;

    [SerializeField]
    private Text fruitText;

    [SerializeField]
    private Text ghostText1;
    [SerializeField]
    private Text ghostText2;
    [SerializeField]
    private Text ghostText3;
    [SerializeField]
    private Text ghostText4;

    private RectTransform ghost1TextTransform;
    private RectTransform ghost2TextTransform;
    private RectTransform ghost3TextTransform;
    private RectTransform ghost4TextTransform;

    private RectTransform canvasRectT;

    [SerializeField]
    private Image[] lifeIcons;
    [SerializeField]
    private Image[] fruitIcons;

    [Header("Fruit Icons")]
    [SerializeField]
    private Sprite empty;
    [SerializeField]
    private Sprite cherry;
    [SerializeField]
    private Sprite strawberry;
    [SerializeField]
    private Sprite orange;
    [SerializeField]
    private Sprite apple;
    [SerializeField]
    private Sprite melon;
    [SerializeField]
    private Sprite galaxianStarship;
    [SerializeField]
    private Sprite bell;
    [SerializeField]
    private Sprite key;

    [Header("Fruit Pickups")]
    [SerializeField]
    private GameObject cherryPickup;
    [SerializeField]
    private GameObject strawberryPickup;
    [SerializeField]
    private GameObject orangePickup;
    [SerializeField]
    private GameObject applePickup;
    [SerializeField]
    private GameObject melonPickup;
    [SerializeField]
    private GameObject galaxianStarshipPickup;
    [SerializeField]
    private GameObject bellPickup;
    [SerializeField]
    private GameObject keyPickup;

    [Space(10)]
    [SerializeField]
    private Transform fruitSpawnLocation;

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }
    public int level { get; private set; }

    [SerializeField]
    private int bonusLifeScore = 10000;
    private bool bonusLifeAwarded = false;

    [SerializeField]
    private int pelletsForFruit = 70;
    private bool fruitSpawned = false;

    private bool pacmanDead = false;

    [SerializeField]
    private Tilemap wallsTilemap;
    [SerializeField]
    private Color tilemapDim;
    [SerializeField]
    private Color tilemapBright;

    [Header("Chomp SFX")]
    [SerializeField]
    private AudioSource chompSource;
    [SerializeField]
    private AudioClip[] chompClips;

    [Header("Power Pellet SFX")]
    [SerializeField]
    private AudioSource powerSource;
    [SerializeField]
    private AudioClip powerPelletSound;

    [Header("General SFX")]
    [SerializeField]
    private AudioSource generalSource;
    [SerializeField]
    private AudioClip fruitSound;
    [SerializeField]
    private AudioClip extraLifeSound;
    [SerializeField]
    private AudioClip startGameSound;
    [SerializeField]
    private AudioClip pacmanDeathSound;
    [SerializeField]
    private AudioClip ghostDeathSound;

    [Header("Siren SFX")]
    [SerializeField]
    private AudioSource sirenSource;
    [SerializeField]
    private AudioClip siren1Sound;
    [SerializeField]
    private AudioClip siren2Sound;
    [SerializeField]
    private AudioClip siren3Sound;
    [SerializeField]
    private AudioClip siren4Sound;
    [SerializeField]
    private AudioClip siren5Sound;

    private bool skipRoundPause = true;

    [Space(10)]
    [SerializeField]
    private float returnToMenuTime = 2f;

    private float timer = 0;

    private bool muteMusic;

    private void Start()
    {
        gameOver.enabled = false;
        fruitText.enabled = false;
        ghostText1.enabled = false;
        ghostText2.enabled = false;
        ghostText3.enabled = false;
        ghostText4.enabled = false;

        ghost1TextTransform = ghostText1.rectTransform;
        ghost2TextTransform = ghostText2.rectTransform;
        ghost3TextTransform = ghostText3.rectTransform;
        ghost4TextTransform = ghostText4.rectTransform;

        canvasRectT = FindObjectOfType<Canvas>().GetComponent<RectTransform>();

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

        DelayedStart();

        // NewGame();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }
    }

    private void DelayedStart()
    {
        NewGame();
        HidePacmanAndGhosts();
        SetLives(3);

        Time.timeScale = 0;

        PlayStartGameSound();
        StartCoroutine(WaitForRealTime(startDelayTime, 2f));
    }

    public IEnumerator WaitForRealTime(float delay) // Credit - https://answers.unity.com/questions/787180/make-a-coroutine-run-when-timetimescale-0.html
    {
        ready.SetActive(true);
        SubtractLivesIcon();

        while (true)
        {
            float pauseEndTime = Time.realtimeSinceStartup + delay;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }

        Time.timeScale = 1;
        ready.SetActive(false);
        sirenSource.volume = 1f;
        sirenSource.Play();
    }

    public IEnumerator WaitForRealTime(float delay, float secondDelay) // Credit - https://answers.unity.com/questions/787180/make-a-coroutine-run-when-timetimescale-0.html
    {
        ready.SetActive(true);

        while (true) // At the end of this loop, pacman + ghosts placed on map
        {
            float pauseEndTime = Time.realtimeSinceStartup + delay - secondDelay;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }

        NewGame();
        SubtractLivesIcon();
        skipRoundPause = false;

        while (true) // At end of this loop, the game timescale is set to normal and the game begins
        {
            float pauseEndTime = Time.realtimeSinceStartup + secondDelay;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }

        Time.timeScale = 1;
        ready.SetActive(false);
        sirenSource.Play();
    }

    private void NewGame()
    {
        SetScore(0);
        SetLevel(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound() // Set pacman, ghosts, and pellets to be active
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        SetLevel(level + 1);
        fruitSpawned = false;

        ResetState();
    }

    private void ResetState() // resets ghosts + pacman to true
    {
        ResetGhostMultiplier();

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        ghostRetreatBlinky.SetActive(false);
        ghostRetreatBlinky.GetComponent<GhostRetreat>().enabled = false;
        ghostRetreatInky.SetActive(false);
        ghostRetreatInky.GetComponent<GhostRetreat>().enabled = false;
        ghostRetreatPinky.SetActive(false);
        ghostRetreatPinky.GetComponent<GhostRetreat>().enabled = false;
        ghostRetreatClyde.SetActive(false);
        ghostRetreatClyde.GetComponent<GhostRetreat>().enabled = false;

        pacman.ResetState();
        pacmanDead = false;
        pacmanWin.SetActive(false);

        if (!skipRoundPause) // unless its start of game, make sure to pause at the start of each round
        {
            Time.timeScale = 0;
            StartCoroutine(WaitForRealTime(2f));
        }
    }

    private void GameOver() // sets ghosts + pacman to false
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);

        gameOver.enabled = true;

        StartCoroutine(ReturnToMenu());
    }

    private IEnumerator ReturnToMenu() // returns player to menu after a delay
    {
        yield return new WaitForSeconds(returnToMenuTime);

        SceneManager.LoadScene("MenuScene");
    }

    private void SetScore(int score)
    {
        this.score = score;

        if (!bonusLifeAwarded && score >= bonusLifeScore) // If player reaches 10000, they get a bonus life
        {
            bonusLifeAwarded = true;
            SetLives(lives + 1);
            SubtractLivesIcon();

            PlayExtraLifeSound();
        }

        if (score > PlayerPrefs.GetInt("PacmanHighScore"))
        {
            PlayerPrefs.SetInt("PacmanHighScore", score);
            SetHighScoreText();
        }

        scoreText.text = score.ToString();
    }

    private void SetLevel(int level)
    {
        this.level = level;

        if (level > PlayerPrefs.GetInt("PacmanHighLevel"))
        {
            PlayerPrefs.SetInt("PacmanHighLevel", level);
            SetHighScoreText();
        }

        levelText.text = level.ToString();

        UpdateFruits();

        timer = 0;
    }

    private void UpdateFruits()
    {
        switch (level)
        {
            case 0:
                EmptyFruits();
                break;
            case 1:
                PlaceFruit(0, cherry);
                break;
            case 2:
                PlaceFruit(1, strawberry);
                break;
            case 3:
                PlaceFruit(2, orange);
                break;
            case 4:
                PlaceFruit(3, orange);
                break;
            case 5:
                PlaceFruit(4, apple);
                break;
            case 6:
                PlaceFruit(5, apple);
                break;
            case 7:
                PlaceFruit(6, melon);
                break;
            case 8:
                PushFruit(melon);
                break;
            case 9:
            case 10:
                PushFruit(galaxianStarship);
                break;
            case 11:
            case 12:
                PushFruit(bell);
                break;
            default:
                PushFruit(key);
                break;
        }
    }

    private void PlaceFruit(int index, Sprite fruit) // if no overflow, simply place the fruit, otherwise PushFruit() method is used
    {
        fruitIcons[index].sprite = fruit;
    }

    private void PushFruit(Sprite fruit) // goes through and shifts all of the fruits by one, then pushes in the new fruit being added
    {
        for (int i = 0; i < fruitIcons.Length; i++)
        {
            if (i == (fruitIcons.Length - 1))
            {
                fruitIcons[i].sprite = fruit;
            }
            else
            {
                fruitIcons[i].sprite = fruitIcons[i+1].sprite;
            }
        }
    }

    private void EmptyFruits() // set all fruit icons to be empty
    {
        foreach (Image fruit in fruitIcons)
        {
            fruit.sprite = empty;
        }
    }

    private void SetLives(int lives)
    {
        this.lives = lives;

        DisableAllIcons();

        int displayedLives = lives;

        for (int i = 0; i < displayedLives; i++)
        {
            lifeIcons[i].enabled = true;
        }
    }

    private void SubtractLivesIcon()
    {
        DisableAllIcons();

        int displayedLives = lives - 1;

        for (int i = 0; i < displayedLives; i++)
        {
            lifeIcons[i].enabled = true;
        }
    }

    private void DisableAllIcons()
    {
        foreach (Image lifeIcon in lifeIcons)
        {
            lifeIcon.enabled = false;
        }
    }

    private void HidePacmanAndGhosts()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    public void CallEatGhostSequence(Ghost ghost, GameObject ghostRetreat)
    {
        StartCoroutine(EatGhostSequence(ghost, ghostRetreat));
    }

    public IEnumerator EatGhostSequence(Ghost ghost, GameObject ghostRetreat)
    {
        Time.timeScale = 0;

        ghost.gameObject.SetActive(false);
        pacman.gameObject.SetActive(false);

        Text ghostText = GhostEaten(ghost);

        ghostRetreat.transform.position = ghost.gameObject.transform.position;

        float delay = 1f;

        while (true)
        {
            float pauseEndTime = Time.realtimeSinceStartup + delay;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }

        Time.timeScale = 1;

        ghostText.enabled = false;

        pacman.gameObject.SetActive(true);
        ghostRetreat.SetActive(true);
        ghostRetreat.GetComponent<GhostRetreat>().enabled = true;
    }

    public IEnumerator EatGhostSequence(float delay) 
    {
        ready.SetActive(true);
        SubtractLivesIcon();

        while (true)
        {
            float pauseEndTime = Time.realtimeSinceStartup + delay;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            break;
        }

        Time.timeScale = 1;
        ready.SetActive(false);
        sirenSource.Play();
    }

    public Text GhostEaten(Ghost ghost) // adds points for killing ghost
    {
        int ghostScore = ghost.GetPoints() * (int)(Mathf.Pow(2, (ghostMultiplier - 1)));
        SetScore(score + ghostScore);
        ghostMultiplier++;

        PlayGhostDeathSound();

        if (!ghostText1.isActiveAndEnabled)
        {
            //StartCoroutine(TempGhostText(ghostScore, ghost, ghostText1, ghost1TextTransform));
            SetGhostText(ghostScore, ghost, ghostText1, ghost1TextTransform);
            return ghostText1;
        }
        else if (!ghostText2.isActiveAndEnabled)
        {
            //StartCoroutine(TempGhostText(ghostScore, ghost, ghostText2, ghost2TextTransform));
            SetGhostText(ghostScore, ghost, ghostText2, ghost2TextTransform);
            return ghostText2;
        }
        else if (!ghostText3.isActiveAndEnabled)
        {
            //StartCoroutine(TempGhostText(ghostScore, ghost, ghostText3, ghost3TextTransform));
            SetGhostText(ghostScore, ghost, ghostText3, ghost3TextTransform);
            return ghostText3;
        }
        else
        {
            //StartCoroutine(TempGhostText(ghostScore, ghost, ghostText4, ghost4TextTransform));
            SetGhostText(ghostScore, ghost, ghostText4, ghost4TextTransform);
            return ghostText4;
        }
    }

    private IEnumerator TempGhostText(int ghostPoints, Ghost ghost, Text ghostText, RectTransform ghostTextRectTransform)
    {
        //ghostText.text = ghostPoints.ToString();

        //Vector2 ghostPos = ghost.gameObject.transform.position;
        //Vector2 screenPoint = Camera.main.WorldToViewportPoint(ghostPos);
        ////Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, ghostPos);
        ////ghostTextRectTransform.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
        //Vector2 WorldObject_ScreenPosition = new Vector2(((screenPoint.x * canvasRectT.sizeDelta.x) - (canvasRectT.sizeDelta.x * 0.5f)),((screenPoint.y * canvasRectT.sizeDelta.y) - (canvasRectT.sizeDelta.y * 0.5f)));
        //ghostTextRectTransform.anchoredPosition = WorldObject_ScreenPosition; // https://gist.github.com/unitycoder/54f4be0324cccb649eff

        SetGhostText(ghostPoints, ghost, ghostText, ghostTextRectTransform);

        ghostText.enabled = true;

        yield return new WaitForSeconds(1f);

        ghostText.enabled = false;
    }

    private void SetGhostText(int ghostPoints, Ghost ghost, Text ghostText, RectTransform ghostTextRectTransform)
    {
        ghostText.text = ghostPoints.ToString();

        Vector2 ghostPos = ghost.gameObject.transform.position;
        Vector2 screenPoint = Camera.main.WorldToViewportPoint(ghostPos);
        //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, ghostPos);
        //ghostTextRectTransform.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
        Vector2 WorldObject_ScreenPosition = new Vector2(((screenPoint.x * canvasRectT.sizeDelta.x) - (canvasRectT.sizeDelta.x * 0.5f)), ((screenPoint.y * canvasRectT.sizeDelta.y) - (canvasRectT.sizeDelta.y * 0.5f)));
        ghostTextRectTransform.anchoredPosition = WorldObject_ScreenPosition; // https://gist.github.com/unitycoder/54f4be0324cccb649eff

        ghostText.enabled = true;
    }

    public void PacmanEaten()
    {
        sirenSource.Stop();
        sirenSource.volume = 0f;
        StopPowerPelletSound();

        pacman.gameObject.SetActive(false);

        if (!pacmanDead)
        {
            pacmanDead = true;
            pacmanDeath.transform.position = pacman.transform.position;
            pacmanDeath.SetActive(true);

            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].gameObject.SetActive(false);
            }

            PlayPacmanDeathSound();
        }

        SetLives(lives - 1); // decrement lives

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false); // hide pellet from map

        SetScore(score + pellet.points); // increment score

        if (!HasRemainingPellets())
        {
            sirenSource.Stop();
            sirenSource.pitch = 1f;

            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].gameObject.SetActive(false);
            }

            pacmanWin.transform.position = pacman.gameObject.transform.position;
            pacmanWin.SetActive(true);
            pacman.gameObject.SetActive(false);

            CancelInvoke();
            StopPowerPelletSound();

            StartCoroutine(FlashMap());
            Invoke(nameof(NewRound), 3.0f);
        }
        else if (HalfPelletsEaten() && !fruitSpawned) // Checks to see if we need to spawn a fruit
        {
            fruitSpawned = true;
            SpawnFruit();
        }
    }

    private void SpawnFruit() // spawns a fruit based on the current level
    {
        switch (level)
        {
            case 0:
                Debug.LogError("Error: GameManger-->SpawnFruit-->Case 0");
                break;
            case 1:
                InstantiateFruit(cherryPickup);
                break;
            case 2:
                InstantiateFruit(strawberryPickup);
                break;
            case 3:
            case 4:
                InstantiateFruit(orangePickup);
                break;
            case 5:
            case 6:
                InstantiateFruit(applePickup);
                break;
            case 7:
            case 8:
                InstantiateFruit(melonPickup);
                break;
            case 9:
            case 10:
                InstantiateFruit(galaxianStarshipPickup);
                break;
            case 11:
            case 12:
                InstantiateFruit(bellPickup);
                break;
            default:
                InstantiateFruit(keyPickup);
                break;
        }
    }

    private void InstantiateFruit(GameObject fruit) // spawns fruit at fruitSpawnLocation
    {
        Instantiate(fruit, fruitSpawnLocation.position, Quaternion.identity);
    }

    public void PowerPelletEaten(PowerPellet pellet) // set all ghosts to frightened state 
    {
        if (level < 21)
        {
            for (int i = 0; i < ghosts.Length; i++) // loop through & set each ghost to be frightened
            {
                ghosts[i].frightened.Enable(pellet.duration);
                Vector2 priorDirection = ghosts[i].movement.direction;
                ghosts[i].movement.SetDirection(-priorDirection); // make ghost turn around
            }
        }

        PelletEaten(pellet);

        if (level >= 21)
        {
            return;
        }
        sirenSource.Stop();
        PlayPowerPelletSound();

        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration); // start power state countdown
        Invoke(nameof(StopPowerPelletSound), pellet.duration); // start power state countdown
    }

    public void FruitEaten(Fruit fruit)
    {
        fruit.gameObject.SetActive(false);

        SetScore(score + fruit.points); // increment score

        fruitText.text = fruit.points.ToString();
        StartCoroutine(TempFruitText());
    }

    private IEnumerator TempFruitText() // fruit points text appears on screen for a period of time then disappears
    {
        fruitText.enabled = true;

        yield return new WaitForSeconds(2f);

        fruitText.enabled = false;
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private bool HalfPelletsEaten()
    {
        return (PelletsEaten() >= pelletsForFruit); // check if required pellets for fruit spawn are eaten
    }

    public int PelletsEaten()
    {
        int pelletsEaten = 0;

        foreach (Transform pellet in pellets)
        {
            if (!pellet.gameObject.activeSelf)
            {
                pelletsEaten++;
            }
        }

        UpdateSirenSound(pelletsEaten);

        return pelletsEaten;
    }

    private void UpdateSirenSound(int pelletsEaten)
    {
        if (pelletsEaten >= 230)
        {
            sirenSource.pitch = 1.8f;
        }
        else if (pelletsEaten >= 220)
        {
            sirenSource.pitch = 1.6f;
        }
        else if (pelletsEaten >= 190)
        {
            sirenSource.pitch = 1.4f;
        }
        else if (pelletsEaten >= 170)
        {
            sirenSource.pitch = 1.2f;
        }
        else
        {
            sirenSource.pitch = 1f;
        }
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
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

    private void SetHighScoreText()
    {
        highScoreText.text = PlayerPrefs.GetInt("PacmanHighScore").ToString() + " L" + PlayerPrefs.GetInt("PacmanHighLevel").ToString();
    }

    private void PlayStartGameSound()
    {
        generalSource.PlayOneShot(startGameSound);
    }

    private void PlayPacmanDeathSound()
    {
        generalSource.PlayOneShot(pacmanDeathSound);
    }

    private void PlayGhostDeathSound()
    {
        generalSource.PlayOneShot(ghostDeathSound);
    }

    public void PlayFruitSound()
    {
        generalSource.PlayOneShot(fruitSound);
    }

    private void PlayExtraLifeSound()
    {
        generalSource.PlayOneShot(extraLifeSound);
    }

    private void PlayPowerPelletSound()
    {
        powerSource.Play();
    }

    private void StopPowerPelletSound()
    {
        powerSource.Pause();
        if (HasRemainingPellets())
        {
            sirenSource.Play();
        }
    }

    public void PlayChompSound()
    {
        if (!chompSource.isPlaying)
        {
            StartCoroutine(PlayChompSequence());
        }
    }

    IEnumerator PlayChompSequence()
    {
        //yield return null;

        //1.Loop through each AudioClip
        for (int i = 0; i < chompClips.Length; i++)
        {
            //2.Assign current AudioClip to audiosource
            chompSource.clip = chompClips[i];

            //3.Play Audio
            chompSource.Play();

            //4.Wait for it to finish playing
            while (chompSource.isPlaying)
            {
                yield return null;
            }

            //5. Go back to #2 and play the next audio in the adClips array
        }
    }

    IEnumerator FlashMap() // flashes map whenever round is complete
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {
            wallsTilemap.color = tilemapDim;
            yield return new WaitForSeconds(.2f);
            wallsTilemap.color = tilemapBright;
            yield return new WaitForSeconds(.2f);
        }
    }

    public float CalculateScatterTime() // returns scatter time based on level and phase
    {
        switch (level)
        {
            case 0:
                //Debug.LogError("Invalid Case: GameManager-->CalculateScatterTime");
                break;
            case 1:
                if (timer <= 40) // Phase 1 + 2
                {
                    return 7;
                }
                else if (timer <= 90)// Phase 3 + 4
                {
                    return 5;
                }
                else // Phase 5+
                {
                    return float.MinValue;
                }

            case 2:
            case 3:
            case 4:
                if (timer <= 40) // Phase 1 + 2
                {
                    return 7;
                }
                else if (timer <= 60)// Phase 3
                {
                    return 5;
                }
                else // Phase 4+
                {
                    return float.MinValue;
                }

            default:
                if (timer <= 60) // Phase 1 to 3
                {
                    return 5;
                }
                else // Phase 4+
                {
                    return float.MinValue;
                }
        }

        return 0.0f;
    }

    public float CalculateChaseTime() // returns chase time based on level and phase
    {
        switch (level)
        {
            case 0:
                //Debug.LogError("Invalid Case: GameManager-->CalculateChaseTime");
                break;
            case 1:
                if (timer <= 82) // Phase 1 to 3
                {
                    return 20;
                }
                else // Phase 4+
                {
                    return float.MaxValue;
                }

            case 2:
            case 3:
            case 4:
                if (timer <= 55) // Phase 1 to 2
                {
                    return 20;
                }
                else // Phase 3+
                {
                    return float.MaxValue;
                }

            default:
                if (timer <= 52) // Phase 1 to 2
                {
                    return 20;
                }
                else // Phase 3+
                {
                    return float.MaxValue;
                }
        }

        return 0.0f;
    }
}
