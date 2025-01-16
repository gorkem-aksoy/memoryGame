using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class anaSayfaKontrol : MonoBehaviour
{
    AudioSource oyunSesi;
    public GameObject cikisPanel;

    private void Start()
    {
        oyunSesi = GetComponent<AudioSource>();

        // duraklatma panelinden ana sayfaya geçildiðinde timescale 0 olarak kalýyor du düzelmiþ oldu
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    // çýkýþ butonundan
    public void cikisYap()
    {
        cikisPanel.SetActive(true);
        Time.timeScale = 0;
    }

    // cikis panelde ki cevabý alýcaz
    public void cevap(string deger)
    {
        if(deger == "evet")
        {
            Application.Quit();
            Debug.Log("çýkýþ yapýldý");
        }
        else
        {
            cikisPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // hemen baþla butonundan
    public void oyunaBasla()
    {
        SceneManager.LoadScene(1);
        oyunSesi.mute = true;
    }
}
