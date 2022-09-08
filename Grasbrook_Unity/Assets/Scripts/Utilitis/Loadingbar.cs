using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loadingbar : MonoBehaviour
{
    [SerializeField]
    Image loadingImage;
    
    public void ChangeLoadingBar(int loadingCount, int taskTotal)
    {
        print("Change Loadingbar!!");
        float fillValue = taskTotal * loadingCount;
        // the 1 is the max Value of fillAmount
        loadingImage.fillAmount = (1 / fillValue);

        switch (loadingCount)
        {
            case 0:
                loadingImage.fillAmount = 0;
                break;

            case 1:
                loadingImage.fillAmount = 0.14f;
                break;

            case 2:
                loadingImage.fillAmount = 0.28f;
                break;

            case 3:
                loadingImage.fillAmount = 0.42f;
                break;

            case 4:
                loadingImage.fillAmount = 0.56f;
                break;

            case 5:
                loadingImage.fillAmount = 0.7f;
                break;

            case 6:
                loadingImage.fillAmount = 0.85f;
                break;

            case 7:
                loadingImage.fillAmount = 1f;
                break;

            default:
                break;
        }
    }

    public void ToggleActive(bool isActive)
    {
        if (!isActive)
        {
            gameObject.SetActive(false);
        }
        else if (isActive)
        {
            gameObject.SetActive(true);
        }
    }
}
