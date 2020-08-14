using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUIExtension;

public class Demo : MonoBehaviour
{
    GUIEx.DropdownState dropdownState_ = new GUIEx.DropdownState();

    void Start()
    {
        
    }
    private void OnGUI()
    {
        dropdownState_ = GUIEx.Dropdown(new Rect(10, 10, 300, 30), new[] { "A", "B", "C" }, dropdownState_);
    }
}
