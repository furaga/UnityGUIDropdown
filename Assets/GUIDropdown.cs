using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GUIExtension
{
    public static class GUIEx
    {
        static Texture2D buttonTexture_ = null;
        static GUISkin skin_ = null;

        public static void setup()
        {
            if (buttonTexture_ == null)
            {
                buttonTexture_ = Resources.Load<Texture2D>("dropdown_button");
            }
            if (skin_ == null)
            {
                skin_ = Resources.Load<GUISkin>("Dropdown");
            }
        }

        public class DropdownState
        {
            public enum Status
            {
                Closed,
                Opening,
                Opened,
                Closing,
            }
            public int Select = 0;
            public int NextSelect = 0;
            public string Caption = "";
            private Status currentStatus_ = Status.Closed;
            public Status CurrentStatus
            {
                get
                {
                    return currentStatus_;
                }
                set
                {
                    currentStatus_ = value;
                    CurrentStatusStartTime = Time.time;
                }
            }
            public float CurrentStatusStartTime { get; private set; }
        }

        public static DropdownState Dropdown(Rect position, string[] options, DropdownState state)
        {
            setup();

            var orgSkin = GUI.skin;

            GUI.skin = skin_;

            switch (state.CurrentStatus)
            {
                case DropdownState.Status.Closed:
                    return closedDropdown(position, options, state);
                case DropdownState.Status.Opening:
                    return openingDropdown(position, options, state);
                case DropdownState.Status.Opened:
                    return openedDropdown(position, options, state);
                case DropdownState.Status.Closing:
                    return closingDropdown(position, options, state);
            }

            GUI.skin = orgSkin;
            return state;
        }

        static bool drawCaption(Rect position, string[] options, DropdownState state)
        {
            if (0 <= state.Select && state.Select < options.Length)
            {
                state.Caption = options[state.Select];
            }
            bool pushed = GUI.Button(position, state.Caption);
            var texRect = new Rect(position.xMax - 24, position.center.y - 8, 16, 16);

            var prevColor = GUI.color;
            GUI.color = new Color(0.5f, 0.5f, 0.5f);
            GUI.DrawTexture(texRect, buttonTexture_);
            GUI.color = prevColor;
            return pushed;
        }

        static int drawDropdownList(Rect position, string[] options, DropdownState state)
        {
            var optionStyle = skin_.FindStyle("dropdown_option");
            int newSelect = -1;

            float offsetY = position.yMax;
            float totalOptionHeight = position.height * options.Length;
            if (offsetY + totalOptionHeight > Screen.height)
            {
                offsetY = position.yMin - totalOptionHeight;
            }

            for (int i = 0; i < options.Length; i++)
            {
                var optionPosition = position;
                optionPosition.y = offsetY + position.height * i;
                string text = string.Format("{0}{1}", i == state.Select ? " ✓ " : " 　 ", options[i]);
                if (GUI.Button(optionPosition, text, optionStyle))
                {
                    newSelect = i;
                }
            }
            return newSelect;
        }

        static DropdownState closedDropdown(Rect position, string[] options, DropdownState state)
        {
            if (drawCaption(position, options, state))
            {
                state.CurrentStatus = DropdownState.Status.Opening;
            }

            return state;
        }

        static DropdownState openingDropdown(Rect position, string[] options, DropdownState state)
        {
            const float fadeTime = 0.1f;
            float dt = Time.time - state.CurrentStatusStartTime;
            float a = dt / fadeTime;

            drawCaption(position, options, state);

            var prevColor = GUI.color;
            GUI.color = new Color(1, 1, 1, a);
            drawDropdownList(position, options, state);
            GUI.color = prevColor;

            if (dt >= fadeTime)
            {
                state.CurrentStatus = DropdownState.Status.Opened;
            }
            return state;
        }

        static DropdownState openedDropdown(Rect position, string[] options, DropdownState state)
        {
            if (drawCaption(position, options, state))
            {
                state.CurrentStatus = DropdownState.Status.Closing;
            }
            int newSelect = drawDropdownList(position, options, state);
            if (newSelect >= 0)
            {
                state.NextSelect = newSelect;
                state.CurrentStatus = DropdownState.Status.Closing;
            }
            return state;
        }

        static DropdownState closingDropdown(Rect position, string[] options, DropdownState state)
        {
            const float fadeTime = 0.1f;
            float dt = Time.time - state.CurrentStatusStartTime;
            float a = 1 - dt / fadeTime;

            drawCaption(position, options, state);

            var prevColor = GUI.color;
            GUI.color = new Color(1, 1, 1, a);
            drawDropdownList(position, options, state);
            GUI.color = prevColor;

            if (dt >= fadeTime)
            {
                state.Select = state.NextSelect;
                state.CurrentStatus = DropdownState.Status.Closed;
            }
            return state;
        }
    }
}