using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class GenericTextPopup : MonoBehaviour
{

    private TextMeshPro textMesh;

    public static GenericTextPopup Create(Transform textPrefab, Vector3 position, string text)
    {
        Transform textPopupTransform = Instantiate(textPrefab, position, Quaternion.identity);
        GenericTextPopup textPopup = textPopupTransform.GetComponent<GenericTextPopup>();
        textPopup.Setup(text);
        //StartCoroutine(GameMaster.deleteGameObjectAfterWaiting(popupDeleteTimer, textPopupTransform.gameObject));
        return textPopup;
        
    }


    public void setTextFaceColor(Color c)
    {
        TextMeshPro tmpObj = textMesh.GetComponent<TextMeshPro>();
        tmpObj.faceColor = c;
    }
    

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }



    public void Setup(string text)
    {
        textMesh.SetText(text);
    }

}
