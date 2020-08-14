using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUIExtension;

public class Demo : MonoBehaviour
{
    GUIEx.DropdownState state1_ = new GUIEx.DropdownState();
    GUIEx.DropdownState state2_ = new GUIEx.DropdownState();
    GUIEx.DropdownState state3_ = new GUIEx.DropdownState();

    private void OnGUI()
    {
        state1_ = GUIEx.Dropdown(new Rect(30, 30, 100, 30), new[] { "A", "B", "C" }, state1_);

        var styles = new GUIEx.DropdownStyles("button", "box", Color.white, 24, 8);
        state2_ = GUIEx.Dropdown(new Rect(150, 30, 150, 30), new[] { "X", "Y", "Z", "W" }, state2_, styles);

        state3_ = GUIEx.Dropdown(
            new Rect(10, Screen.height - 50, Screen.width - 20, 40), 
            new[] {
                "1920 x 1080 (Recommend)",
                "1600 x 1024",
                "1280 x 960",
                "1280 x 800", }, 
            state3_);
    }
}
