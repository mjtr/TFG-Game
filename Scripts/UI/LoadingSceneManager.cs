using System.Collections;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public string sceneLoadName  ="MainGame";
    public TextMeshProUGUI textProgress;
    public Slider sliderProgress;
    public float currentPercent;
    private float loadingProgress = 0f;
    private AsyncOperation asyncLoad;
    private bool firstTime = true;

    private void Start() {
        StartCoroutine(loadScene(sceneLoadName));
    }

    public IEnumerator loadScene(string nameToLoad)
    {
     
        textProgress.text = "Cargando... 00%";
        yield return new WaitForSeconds(2);

        asyncLoad = SceneManager.LoadSceneAsync(nameToLoad);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            currentPercent = asyncLoad.progress * 100 / 0.9f;
            textProgress.text = "Cargando... " + currentPercent.ToString("00") + "%";
            sliderProgress.value = Mathf.MoveTowards(sliderProgress.value,currentPercent, 10 * Time.deltaTime);

            if(currentPercent >= 100 && firstTime){
                firstTime = false;
                asyncLoad.allowSceneActivation= true;
            }
            yield return null;
        }
    }

    
    private void Update()
    {
        sliderProgress.value = Mathf.MoveTowards(sliderProgress.value,currentPercent, 10 * Time.deltaTime);

       if(asyncLoad != null && currentPercent >= 100 && firstTime){
            firstTime = false;
            asyncLoad.allowSceneActivation= true;
       }
    }

   

}
