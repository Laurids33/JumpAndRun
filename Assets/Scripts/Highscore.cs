using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
    public GameObject entry;
    public GameObject content;
    public int numbreEntries = 0;
    public Spieler SpielerKlasse;
    public TMP_InputField inputName;
    public bool highscoreSafedYet = false;

    [System.Serializable]
    public class SaveData
    {
        public string name;
        public float recordTime;
    }

    [System.Serializable]
    public class SaveDataList
    {
        public SaveData[] saves;
    }

    void Start()
    {
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = 0;
        rectTransform.sizeDelta = size;

        LoadData();
    }

    void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "saveData.json");
        SaveDataList saveData;

        if (File.Exists(path) && !string.IsNullOrWhiteSpace(File.ReadAllText(path)))
        {
            saveData = JsonUtility.FromJson<SaveDataList>(File.ReadAllText(path));
        }
        else
        {
            saveData = new SaveDataList { saves = new SaveData[0] };
        }

        // Nur die beste Zeit pro Name behalten
        var bestTimes = new Dictionary<string, SaveData>();
        foreach (var entry in saveData.saves)
        {
            if (!bestTimes.ContainsKey(entry.name) || entry.recordTime < bestTimes[entry.name].recordTime)
            {
                bestTimes[entry.name] = entry;
            }
        }

        // Nach Zeit sortieren
        var sorted = new List<SaveData>(bestTimes.Values);
        sorted.Sort((a, b) => a.recordTime.CompareTo(b.recordTime));

        foreach (var entry in sorted)
        {
            newEntry(entry.name, entry.recordTime);
            numbreEntries++;
        }
    }

    void newEntry(string name, float time)
    {
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y += 27;
        rectTransform.sizeDelta = size;

        GameObject newEntry = Instantiate(entry, content.transform);
        newEntry.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -numbreEntries * 26);
        newEntry.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = $"{numbreEntries + 1}. {name}";
        newEntry.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = string.Format("{0,3:0.0}", time);
    }

    void deleteEntries()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        numbreEntries = 0;
        RectTransform rectTransform = content.GetComponent<RectTransform>();
        Vector2 size = rectTransform.sizeDelta;
        size.y = 0;
        rectTransform.sizeDelta = size;
    }

    public void saveCurrent()
    {
        if (SpielerKlasse.spielBeendet && inputName.text != "" && SpielerKlasse.zeit != null)
        {
            if (inputName.text.Length < 14)
            {
                string path = Path.Combine(Application.persistentDataPath, "saveData.json");
                SaveDataList saveData;

                if (File.Exists(path) && !string.IsNullOrWhiteSpace(File.ReadAllText(path)))
                {
                    saveData = JsonUtility.FromJson<SaveDataList>(File.ReadAllText(path));
                }
                else
                {
                    saveData = new SaveDataList { saves = new SaveData[0] };
                }

                var list = new List<SaveData>(saveData.saves);
                list.Add(new SaveData { name = inputName.text, recordTime = SpielerKlasse.zeit ?? 0f });
                saveData.saves = list.ToArray();

                string newJson = JsonUtility.ToJson(saveData, true);
                File.WriteAllText(path, newJson);

                Debug.Log("Highscore gespeichert: " + path);
                SpielerKlasse.infoAnzeige.text = "Highscore gespeichert!";
                deleteEntries();
                LoadData();
                highscoreSafedYet = true;
            }
            else
            {
                SpielerKlasse.infoAnzeige.text = "Name ist zu lang!";
            }

        }
    }

    void Update()
    {
        if (!highscoreSafedYet)
        {
            saveCurrent();
        }
    }
}
