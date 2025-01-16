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

        // duraklatma panelinden ana sayfaya ge�ildi�inde timescale 0 olarak kal�yor du d�zelmi� oldu
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    // ��k�� butonundan
    public void cikisYap()
    {
        cikisPanel.SetActive(true);
        Time.timeScale = 0;
    }

    // cikis panelde ki cevab� al�caz
    public void cevap(string deger)
    {
        if(deger == "evet")
        {
            Application.Quit();
            Debug.Log("��k�� yap�ld�");
        }
        else
        {
            cikisPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // hemen ba�la butonundan
    public void oyunaBasla()
    {
        SceneManager.LoadScene(1);
        oyunSesi.mute = true;
    }
}
