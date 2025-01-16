using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameKontrol : MonoBehaviour
{

    GameObject ilkSecilenButon; // kartlar eþleþmesse, tekrardan default yapma, eþleþirse cardlarý yok etme
    GameObject butonunKendisi; // butona týklandýðýnda default spriteýn altýnda ki sprite gösterme
    public Sprite defaultSprite;
    public AudioSource[] oyunSesleri;
    public GameObject[] butonlarim; // bütün butonlarýn raycast target özelliðine eriþmek için hepsini aldýk.

    // teknik iþlemler
    int ilkSecimDegeri;
    // 18 olacak toplam 36 kart 18 doðru eþleþme olmalý
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


    // örnek uzun random kart sistemi
    /*
    public GameObject eklenecekObjem;
    public GameObject gridAlanim;
    */

    // kýsa random kart sistemi
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

            // süre bitti
            if (zamanSlider.maxValue == zamanSlider.value)
            {
                zamanlayici = false;
                //sureText.text = "00 : 00";
                Debug.Log("sure bitti");
                gameOver();
            }

            /*
            // mathfToInt : deðeri int'e çeviriyor
            dakika = Mathf.FloorToInt(toplamZaman / 60); // 119 / 60 = 1 dakika yý aldým
            saniye = Mathf.FloorToInt(toplamZaman % 60); // 119 % 60 = 1 ******* => 59 saniye yi aldým

            // bu bize 120 den geriye 1 azalarak çýktý verir. ancak biz dakika saniye istiyoruz.
            // sureText.text = Mathf.FloorToInt(toplamZaman).ToString();

            // bu format ile dakika ve saniye iþleyecek
            sureText.text = string.Format("{0:00} : {01:00}", dakika, saniye);
            */
        }

    }

    // gride random kart oluþtur
    // start fonksiyonunda çalýþacak
    IEnumerator kartOlustur()
    {
        /*
        * uzun yöntem
        * Örnek uygulama için not 
        * ancak çok uðraþtýrýcý ve uzun bir yöntem bütün kartlarý örneklemek gerekiyor vs.
        * 
       GameObject obje = Instantiate(eklenecekObjem);
       RectTransform rt = obje.GetComponent<RectTransform>();
       rt.localScale = new Vector3(1f,1f,1f);
       obje.transform.SetParent(gridAlanim.transform);
       */

        
        yield return new WaitForSeconds(.1f);

        while (olusturmaDurumu)
        {
            // kýsa yöntem - grid'e random kart ekleme.
            // ilk adým - havuz da ki kart sayýsý bulma
            //Debug.Log(kartHavuzum.transform.childCount);
            // ikinci adým - random kart belirleme
            int rastgeleSayi = Random.Range(0, kartHavuzum.transform.childCount - 1);

            if(kartHavuzum.transform.GetChild(rastgeleSayi).gameObject != null)
            {
                // Debug.Log(kartHavuzum.transform.GetChild(rastgeleSayi).name);
                // üçüncü adým - random belirlenen kartý gride atma
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

    // süre dolduysa
    void gameOver()
    {

        oyunSonuPaneller[0].SetActive(true);
    }

    // obje eþleþtirme istenilen þekilde bittiyse
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

    // butonlardan eriþtik
    public void objeVer(GameObject gelenObje)
    {
        butonunKendisi = gelenObje;
        // butonumuzun altýndaki objenin sprite ný ana objenin sprite na atadýk.
        // týklandýðýnda alttaki sprite gözükecek.
        butonunKendisi.GetComponent<Image>().sprite = butonunKendisi.GetComponentInChildren<SpriteRenderer>().sprite;

        // raycast target : resim týklanma özelliði
        // bir butona iki kere týklanmasýn diye, deðerler eþleþtiði için ayný karta iki kere bastýðýmýzda yok edebiliyoruz.
        // ancak butonlar eþleþmediðinde tekrardan týklanmayý true yapmalýyýz
        butonunKendisi.GetComponent<Image>().raycastTarget = false;

        // kutu açma sesi
        oyunSesleri[1].Play();
    }

    // butonlardan eriþtik
    public void butonTikladi(int deger)
    {
        kontrol(deger);
    }

    // buton týkladý fonksiyonundan eriþtik
    void kontrol(int gelenDeger)
    {
        // Debug.Log("tiklandi, gelen deðer: " + deger);

        if (ilkSecimDegeri == 0)
        {
            // butona týklandýðýnda, gelen deðeri ilk seçim deðerine atadýk.
            // sonradan gelen deðer ile karþýlaþtýrmak için
            ilkSecimDegeri = gelenDeger;

            // gelen buton objesini ilk seçilen butona atadýk, eþleþme varsa veya yoksa iþlem yapýlacak
            ilkSecilenButon = butonunKendisi;
        }
        else
        {
            StartCoroutine(kontrolZamanlayici(gelenDeger));
        }
    }

    // kontrol fonksiyonundan eriþtik.
    // butonlar eþleþtiðinde belli bir sürede yok olmasý
    // butonlar eþleþmediðinde belli bir sürede default sprite atanmasý
    IEnumerator kontrolZamanlayici(int gelenDeger)
    {
        // kontrol aþamasýnda týklanmayý kapat
        butonTiklanmaKontrol(false);

        yield return new WaitForSeconds(1f);

        // ilk deðer ile sonradan gelen deðer eþitse eþleþme var
        if (ilkSecimDegeri == gelenDeger)
        {
            Debug.Log("evet eþleþdi");
            // deðiþkenleri tekrardan kullanabilmek için içlerini boþalttýk.
            ilkSecimDegeri = 0;

            // grid sisteminde kartlarýn kaymamasý için bub þekilde yapýldý
            ilkSecilenButon.GetComponent<Image>().enabled = false;
            butonunKendisi.GetComponent<Image>().enabled = false;
            /*
             * // seçimler doðru olduðu için kartlarý yok ettik.
            Destroy(ilkSecilenButon.gameObject);
            Destroy(butonunKendisi.gameObject);
            */

            // kontrol bittiðinde týklanmayý aç
            butonTiklanmaKontrol(true);

            ilkSecilenButon = null;

            // her doðru eþleþmede 1 artýr;
            anlikBasari++;
            if (hedefBasari == anlikBasari)
            {
                win();
            }

        }
        // ilk deðer ile sonradan gelen deðer eþit deðilse eþleþme yok
        else
        {

            // yanlýþ bulma sesi
            oyunSesleri[2].Play();

            Debug.Log("hayýr eþleþmedi");
            ilkSecimDegeri = 0;

            // eþleþme olmadýðý için tekrardan default sprite'ý atadýk.
            ilkSecilenButon.GetComponent<Image>().sprite = defaultSprite;
            butonunKendisi.GetComponent<Image>().sprite = defaultSprite;

            /* gerek kalmadý buton týklama kontrol ile zaten true yapýyoruz.
            // eþleþme olmadýðý için tekrardan týklanmayý açtýk.
            ilkSecilenButon.GetComponent<Image>().raycastTarget = true;
            butonunKendisi.GetComponent<Image>().raycastTarget = true;
            */

            // kontrol bittiðinde týklanmayý aç
            butonTiklanmaKontrol(true);

            ilkSecilenButon = null;

        }
    }

    // kontrol zamanlayýcý fonksiyonundan eriþtik
    void butonTiklanmaKontrol(bool gelenDurum)
    {
        foreach (var item in butonlarim)
        {
            // item varsa eriþ
            if (item != null)
            {
                // gelen duruma göre resim týklanma kontorl 
                ilkSecilenButon.GetComponent<Image>().raycastTarget = gelenDurum;
            }
        }
    }
}
