using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Blitzy.UnityRadialController;
public class RadialMenu : MonoBehaviour
{

    List<RadialMenuEntry> entries;

    public GameObject EntryPrefab;
    public float radius;
    public int selectedEntry;

    private void Start()
    {
        entries = new List<RadialMenuEntry>();

    }

    public void AddEntry(string labelString, Texture texture, int pos, float execDelay, MenuSystem.Screen link)
    {
        GameObject entry = Instantiate(EntryPrefab, transform);

        RadialMenuEntry rme = entry.GetComponent<RadialMenuEntry>();
        rme.SetLabel(labelString);
        rme.SetIcon(texture);
        rme.SetLink(link);

        entries.Add(rme);

        float x = Mathf.Sin(pos * Mathf.Deg2Rad) * radius;
        float y = Mathf.Cos(pos * Mathf.Deg2Rad) * radius;

        RectTransform rect = entry.GetComponent<RectTransform>();

        rect.localScale = Vector3.zero;

        rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad).SetDelay(.05f * execDelay);
        rect.transform.localPosition = new Vector3(x, y, 0);
        //rect.DOAnchorPos(new Vector3(x, y, 0), .3f).SetEase(Ease.OutQuad).SetDelay(.05f * execDelay);
    }


    public RadialMenuEntry Press()
    {
        if (entries.Count == 0)
        {
            Debug.LogError("ERROR: No Entries in RadialMenu");
        }
            return entries[selectedEntry];
      
    }
    public void OnRotation(float numDegs)
    {
        selectedEntry = selectedEntry + (int)Mathf.Sign(numDegs);


        if(selectedEntry < 0)
        {
            selectedEntry = entries.Count - 1;
        }

        if(selectedEntry >= entries.Count)
        {
            selectedEntry = 0;
        }
        SelectEntry(entries[selectedEntry]);
    }


    void SelectEntry(RadialMenuEntry highlightEntry)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            RectTransform rectTrafo = entries[i].GetComponent<RectTransform>();
            rectTrafo.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
        }

        RectTransform rect = highlightEntry.GetComponent<RectTransform>();
        rect.DOScale(new Vector3(1.5f, 1.5f, 1.5f), .3f).SetEase(Ease.OutQuad);
    }




    public void Close()
    {
        for(int i = 0; i < entries.Count; i++)
        {
            RectTransform rect = entries[i].GetComponent<RectTransform>();
            GameObject entry = entries[i].gameObject;

            rect.transform.localPosition = new Vector3(0,0,0);
            //rect.DOAnchorPos3D(Vector3.zero, .3f).SetEase(Ease.OutQuad).onComplete =
            Destroy(entry);
           
        }
        entries.Clear();
        selectedEntry = 0;
    }
}
