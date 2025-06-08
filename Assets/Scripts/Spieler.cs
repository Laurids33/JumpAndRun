using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class Spieler : MonoBehaviour
{
    readonly float inputFactor = 10;
    public GameObject gewinn;

    int anzahlPunkte = 0;
    public TextMeshProUGUI punkteAnzeige;

    int anzahlLeben = 3;
    public TextMeshProUGUI lebenAnzeige;

    float zeitStart;
    public float? zeit;
    bool spielGestartet = false;
    public bool spielBeendet = false;
    public Highscore highscoreClass;
    public TextMeshProUGUI zeitAnzeige;

    public TextMeshProUGUI zeitAltAnzeige;

    public TextMeshProUGUI infoAnzeige;

    void Start()
    {
        float zeitAlt = 0;
        if (PlayerPrefs.HasKey("zeitAlt")) zeitAlt = PlayerPrefs.GetFloat("zeitAlt");
        zeitAltAnzeige.text = string.Format("Alt: {0,6:0.0} sec.", zeitAlt);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); ;
        float verticalInput = Input.GetAxis("Vertical"); ;


        if (highscoreClass.inputName.text != "")
        {

            if (verticalInput < 0)
            {
                return;
            }

            float horizontalNew = transform.position.x + horizontalInput * inputFactor * Time.deltaTime;
            if (horizontalNew > 8.3f)
            {
                horizontalNew = 8.3f;
            }
            if (horizontalNew < -8.3f)
            {
                horizontalNew = -8.3f;
            }

            float verticalNew = transform.position.y + verticalInput * inputFactor * Time.deltaTime;

            transform.position = new Vector3(horizontalNew, verticalNew, 0);
        }

        if (!spielGestartet && (horizontalInput != 0 || verticalInput != 0))
        {
            if (highscoreClass.inputName.text != "")
            {
                spielGestartet = true;
                zeitStart = Time.time;
                infoAnzeige.text = "";
            }
            else
            {
                infoAnzeige.text = "Gebe Sie erst Ihren Namen ein";
            }
        }
        if (spielGestartet)
        {
            zeitAnzeige.text = string.Format("Zeit: {0,6:0.0} sec.", Time.time - zeitStart);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Gewinn"))
        {
            anzahlPunkte++;
            gewinn.SetActive(false);

            if (anzahlPunkte < 6)
            {

                punkteAnzeige.text = $"Punkte: {anzahlPunkte}";
                if (anzahlPunkte == 1)
                {
                    infoAnzeige.text = "Du hast bereits 1 Punkt!";
                }
                else
                {
                    infoAnzeige.text = $"Du hast bereits {anzahlPunkte} Punkte!";
                }
                Invoke(nameof(NaechsterGewinn), 2);
            }
            else
            {
                infoAnzeige.text = "Du hast gewonnen!";
                spielGestartet = false;
                spielBeendet = true;
                gameObject.SetActive(false);
                gewinn.SetActive(false);
                punkteAnzeige.text = "Gewonnen";
                zeit = Time.time - zeitStart;
                PlayerPrefs.SetFloat("zeitAlt", zeit ?? 0f);
                PlayerPrefs.Save();
            }
            float xNeu = Random.Range(-8.0f, 8.0f);

            float yNeu;
            if (anzahlPunkte < 2) yNeu = -2.7f;
            else if (anzahlPunkte < 4) yNeu = 0.15f;
            else yNeu = 3;

            gewinn.transform.position = new Vector3(xNeu, yNeu, 0);
        }
        else if (coll.gameObject.CompareTag("Gefahr"))
        {
            anzahlLeben--;
            lebenAnzeige.text = $"Leben: {anzahlLeben}";

            gameObject.SetActive(false);
            if (anzahlLeben > 0)
            {
                infoAnzeige.text = $"Du hast nur noch {anzahlLeben} Leben!";
                Invoke(nameof(NaechstesLeben), 2);
            }
            else
            {
                infoAnzeige.text = "Du hast verloren!";
                spielGestartet = false;
                gewinn.SetActive(false);
                lebenAnzeige.text = "Verloren";
            }
        }
    }

    void NaechsterGewinn()
    {
        float xNew = Random.Range(-8.0f, 8.0f);

        float yNew;
        if (anzahlPunkte < 2) yNew = -2.7f;
        else if (anzahlPunkte < 4) yNew = 0.15f;
        else yNew = 3;

        gewinn.transform.position = new Vector3(xNew, yNew, 0);
        gewinn.SetActive(true);
        infoAnzeige.text = "";
    }

    void NaechstesLeben()
    {
        transform.position = new Vector3(0, -4.4f, 0);
        gameObject.SetActive(true);
        infoAnzeige.text = "";
    }

    public void SpielNeuButton_Click()
    {
        if (spielGestartet)
            return;

        spielBeendet = false;
        zeit = null;
        anzahlPunkte = 0;
        anzahlLeben = 3;
        float zeitAlt = 0;
        if (PlayerPrefs.HasKey("zeitAlt")) zeitAlt = PlayerPrefs.GetFloat("zeitAlt");
        zeitAltAnzeige.text = string.Format("Alt: {0,6:0.0} sec.", zeitAlt);

        punkteAnzeige.text = "Punkte: 0";
        lebenAnzeige.text = "Leben: 3";
        zeitAnzeige.text = "Zeit: 0.0 sec.";
        zeitAltAnzeige.text = string.Format("Alt: {0,6:0.0} sec.", zeitAlt);
        infoAnzeige.text = @"Bei Betätigen einer beliebigen Taste beginnt das Spiel. 
Lenke den Affen mit den Pfeiltasten. 
Sammle die Bananen, und vermeide die Tiger.";

        transform.position = new Vector3(0, -4.4f, 0);
        gameObject.SetActive(true);

        gewinn.transform.position = new Vector3(4, -2.7f, 0);
        gewinn.SetActive(true);

        highscoreClass.highscoreSafedYet = false;
    }

    public void AnwendungBeendenButton_Click()
    {
        if (!spielGestartet) Application.Quit();
    }
}
