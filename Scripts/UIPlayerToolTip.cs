using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIPlayerToolTip : MonoBehaviour
{
    private TextMeshProUGUI textMesh;


    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshProUGUI>();
    }



    public void Setup(string text)
    {
        textMesh.SetText(text);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
