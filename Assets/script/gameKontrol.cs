using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameKontrol : MonoBehaviour
{

    GameObject ilkSecilenButon; // kartlar e�le�messe, tekrardan default yapma, e�le�irse cardlar� yok etme
    GameObject butonunKendisi; // butona t�kland���nda default sprite�n alt�nda ki sprite g�sterme
    public Sprite defaultSprite;
    public AudioSource[] oyunSesleri;
    public GameObject[] butonlarim; // b�t�n butonlar�n raycast target �zelli�ine eri�mek i�in hepsini ald�k.

    // teknik i�lemler
    int ilkSecimDegeri;
    // 18 olacak toplam 36 kart 18 do�ru e�le�me olmal�
    // 24 hak verilebilir.
    public int hedefBasari;
    int anlikBasari;


    // sayac
    public Slider zamanSlider;
    public TextMeshProUGUI sureText;
    public float toplamZaman;
    float dakika;
    float saniye;
    bool zamanlayici;
    float gecenZaman;

    // paneller
    public GameObject[] oyunSonuPaneller;


    // �rnek uzun random kart sistemi
    /*
    public GameObject eklenecekObjem;
    public GameObject gridAlanim;
    */

    // k�sa random kart sistemi
    public GameObject grid;
    public GameObject kartHavuzum;
    bool olusturmaDurumu;
    int olusturmaSayisi;
    int toplamElemanSayisi;


    void Start()
    {
        ilkSecimDegeri = 0;
        //---------
        zamanlayici = true;
        gecenZaman = 0;
        zamanSlider.maxValue = toplamZaman;
        zamanSlider.value = gecenZaman;
        //-----------
        olusturmaDurumu = true;
        olusturmaSayisi = 0;
        toplamElemanSayisi = kartHavuzum.transform.childCount;

        StartCoroutine(kartOlustur());
    }

    private void Update()
    {

        if (zamanlayici && toplamZaman > 0 && gecenZaman != toplamZaman)
        {
            gecenZaman += Time.deltaTime;
            zamanSlider.value = gecenZaman;

            // s�re bitti
            if (zamanSlider.maxValue == zamanSlider.value)
            {
                zamanlayici = false;
                //sureText.text = "00 : 00";
                Debug.Log("sure bitti");
                gameOver();
            }

            /*
            // mathfToInt : de�eri int'e �eviriyor
            dakika = Mathf.FloorToInt(toplamZaman / 60); // 119 / 60 = 1 dakika y� ald�m
            saniye = Mathf.FloorToInt(toplamZaman % 60); // 119 % 60 = 1 ******* => 59 saniye yi ald�m

            // bu bize 120 den geriye 1 azalarak ��kt� verir. ancak biz dakika saniye istiyoruz.
            // sureText.text = Mathf.FloorToInt(toplamZaman).ToString();

            // bu format ile dakika ve saniye i�leyecek
            sureText.text = string.Format("{0:00} : {01:00}", dakika, saniye);
            */
        }

    }

    // gride random kart olu�tur
    // start fonksiyonunda �al��acak
    IEnumerator kartOlustur()
    {
        /*
        * uzun y�ntem
        * �rnek uygulama i�in not 
        * ancak �ok u�ra�t�r�c� ve uzun bir y�ntem b�t�n kartlar� �rneklemek gerekiyor vs.
        * 
       GameObject obje = Instantiate(eklenecekObjem);
       RectTransform rt = obje.GetComponent<RectTransform>();
       rt.localScale = new Vector3(1f,1f,1f);
       obje.transform.SetParent(gridAlanim.transform);
       */

        
        yield return new WaitForSeconds(.1f);

        while (olusturmaDurumu)
        {
            // k�sa y�ntem - grid'e random kart ekleme.
            // ilk ad�m - havuz da ki kart say�s� bulma
            //Debug.Log(kartHavuzum.transform.childCount);
            // ikinci ad�m - random kart belirleme
            int rastgeleSayi = Random.Range(0, kartHavuzum.transform.childCount - 1);

            if(kartHavuzum.transform.GetChild(rastgeleSayi).gameObject != null)
            {
                // Debug.Log(kartHavuzum.transform.GetChild(rastgeleSayi).name);
                // ���nc� ad�m - random belirlenen kart� gride atma
                Transform randomKart = kartHavuzum.transform.GetChild(rastgeleSayi);
                randomKart.transform.SetParent(grid.transform);

                olusturmaSayisi++;

                if (olusturmaSayisi == toplamElemanSayisi)
                {
                    olusturmaDurumu = false;
                    Destroy(kartHavuzum.gameObject);
                }
            }

            
        }
    }

    // durdurma butonundan
    public void oyunuDurdur()
    {
        Time.timeScale = 0;
        oyunSonuPaneller[2].SetActive(true);
    }

    // pause panel play butonundan
    public void oyunaDevamEt()
    {
        Time.timeScale = 1;
        oyunSonuPaneller[2].SetActive(false);
    }

    // s�re dolduysa
    void gameOver()
    {

        oyunSonuPaneller[0].SetActive(true);
    }

    // obje e�le�tirme istenilen �ekilde bittiyse
    void win()
    {
        oyunSonuPaneller[1].SetActive(true);
    }

    // panellerdeki butonlardan
    public void anaSayfa()
    {
        SceneManager.LoadScene(0);
    }
    // panellerdeki butonlardan
    public void tekrarBasla()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // butonlardan eri�tik
    public void objeVer(GameObject gelenObje)
    {
        butonunKendisi = gelenObje;
        // butonumuzun alt�ndaki objenin sprite n� ana objenin sprite na atad�k.
        // t�kland���nda alttaki sprite g�z�kecek.
        butonunKendisi.GetComponent<Image>().sprite = butonunKendisi.GetComponentInChildren<SpriteRenderer>().sprite;

        // raycast target : resim t�klanma �zelli�i
        // bir butona iki kere t�klanmas�n diye, de�erler e�le�ti�i i�in ayn� karta iki kere bast���m�zda yok edebiliyoruz.
        // ancak butonlar e�le�medi�inde tekrardan t�klanmay� true yapmal�y�z
        butonunKendisi.GetComponent<Image>().raycastTarget = false;

        // kutu a�ma sesi
        oyunSesleri[1].Play();
    }

    // butonlardan eri�tik
    public void butonTikladi(int deger)
    {
        kontrol(deger);
    }

    // buton t�klad� fonksiyonundan eri�tik
    void kontrol(int gelenDeger)
    {
        // Debug.Log("tiklandi, gelen de�er: " + deger);

        if (ilkSecimDegeri == 0)
        {
            // butona t�kland���nda, gelen de�eri ilk se�im de�erine atad�k.
            // sonradan gelen de�er ile kar��la�t�rmak i�in
            ilkSecimDegeri = gelenDeger;

            // gelen buton objesini ilk se�ilen butona atad�k, e�le�me varsa veya yoksa i�lem yap�lacak
            ilkSecilenButon = butonunKendisi;
        }
        else
        {
            StartCoroutine(kontrolZamanlayici(gelenDeger));
        }
    }

    // kontrol fonksiyonundan eri�tik.
    // butonlar e�le�ti�inde belli bir s�rede yok olmas�
    // butonlar e�le�medi�inde belli bir s�rede default sprite atanmas�
    IEnumerator kontrolZamanlayici(int gelenDeger)
    {
        // kontrol a�amas�nda t�klanmay� kapat
        butonTiklanmaKontrol(false);

        yield return new WaitForSeconds(1f);

        // ilk de�er ile sonradan gelen de�er e�itse e�le�me var
        if (ilkSecimDegeri == gelenDeger)
        {
            Debug.Log("evet e�le�di");
            // de�i�kenleri tekrardan kullanabilmek i�in i�lerini bo�altt�k.
            ilkSecimDegeri = 0;

            // grid sisteminde kartlar�n kaymamas� i�in bub �ekilde yap�ld�
            ilkSecilenButon.GetComponent<Image>().enabled = false;
            butonunKendisi.GetComponent<Image>().enabled = false;
            /*
             * // se�imler do�ru oldu�u i�in kartlar� yok ettik.
            Destroy(ilkSecilenButon.gameObject);
            Destroy(butonunKendisi.gameObject);
            */

            // kontrol bitti�inde t�klanmay� a�
            butonTiklanmaKontrol(true);

            ilkSecilenButon = null;

            // her do�ru e�le�mede 1 art�r;
            anlikBasari++;
            if (hedefBasari == anlikBasari)
            {
                win();
            }

        }
        // ilk de�er ile sonradan gelen de�er e�it de�ilse e�le�me yok
        else
        {

            // yanl�� bulma sesi
            oyunSesleri[2].Play();

            Debug.Log("hay�r e�le�medi");
            ilkSecimDegeri = 0;

            // e�le�me olmad��� i�in tekrardan default sprite'� atad�k.
            ilkSecilenButon.GetComponent<Image>().sprite = defaultSprite;
            butonunKendisi.GetComponent<Image>().sprite = defaultSprite;

            /* gerek kalmad� buton t�klama kontrol ile zaten true yap�yoruz.
            // e�le�me olmad��� i�in tekrardan t�klanmay� a�t�k.
            ilkSecilenButon.GetComponent<Image>().raycastTarget = true;
            butonunKendisi.GetComponent<Image>().raycastTarget = true;
            */

            // kontrol bitti�inde t�klanmay� a�
            butonTiklanmaKontrol(true);

            ilkSecilenButon = null;

        }
    }

    // kontrol zamanlay�c� fonksiyonundan eri�tik
    void butonTiklanmaKontrol(bool gelenDurum)
    {
        foreach (var item in butonlarim)
        {
            // item varsa eri�
            if (item != null)
            {
                // gelen duruma g�re resim t�klanma kontorl 
                ilkSecilenButon.GetComponent<Image>().raycastTarget = gelenDurum;
            }
        }
    }
}
