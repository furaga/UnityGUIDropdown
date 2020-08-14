# UnityGUIDropdown

<img src=Docs/screenshot1.png >

<img src=Docs/screenshot2.png >


## What is this

An implementation of dropdown for Unity script without GameObject.  
There are GUI.Button(), GUI.Slider(), ... however **there is NOT GUI.Dropdown()**

Therefore, we implemented **GUIEx.Dropdown()**.


## How to use

```
GUIEx.DropdownState state_ = new GUIEx.DropdownState();

void OnGUI()
{
    state_ = GUIEx.Dropdown(
        new Rect(30, 30, 100, 30),
        new[] { "A", "B", "C" },
        state_);

    Debug.LogFormat("Selecting {0} ({1})", state_.Caption, state_.Select);
}
```

<img src=Docs/screenshot3.png >

You can know which option is selected from **state_.Select** and **state_.Caption**.

