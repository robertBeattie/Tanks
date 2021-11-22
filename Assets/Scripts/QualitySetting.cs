using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualitySetting : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value, true);
    }
}
