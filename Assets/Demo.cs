using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUIExtension;

public class Demo : MonoBehaviour
{
    GUIEx.DropdownState dropdownState_ = new GUIEx.DropdownState();
    GUIEx.DropdownState dropdownState2_ = new GUIEx.DropdownState();

    void Start()
    {
        
    }
    private void OnGUI()
    {
        dropdownState_ = GUIEx.Dropdown(new Rect(10, 10, 300, 30), new[] { "A", "B", "C" }, dropdownState_);

        dropdownState2_ = GUIEx.Dropdown(new Rect(10, Screen.height - 80, Screen.width - 20, 70), 
            new[] {
                "640x480",
                "1080x920",
                "2040x2040"
            }, dropdownState2_);
    }
}
