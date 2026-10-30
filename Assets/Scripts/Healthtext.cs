using UnityEngine;
using TMPro;
public class Healthtext : MonoBehaviour
{
    [SerializeField] Base baseScript;
    TMP_Text healthText;
    [SerializeField] GameObject loseText;

    void Awake()
    {
        healthText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        healthText.text = "Health: " + baseScript.Health;
        if (baseScript.Health <= 0)
        {
            Time.timeScale = 0;
            loseText.SetActive(true);
        }
    }

    public void OnClick()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
