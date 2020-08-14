using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GUIExtension
{
    public static class GUIEx
    {
        static Texture2D buttonTexture_ = null;

        public static void setup()
        {
            if (buttonTexture_ == null)
            {
                buttonTexture_ = Resources.Load<Texture2D>("dropdown_button");
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
            public string Caption = "";
            public Status CurrentStatus = Status.Closed;
        }

        public static DropdownState Dropdown(Rect position, string[] options, DropdownState state)
        {
            setup();

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

            return state;
        }

        static DropdownState closedDropdown(Rect position, string[] options, DropdownState state)
        {
            if (drawCaption(position, options, state))
            {
                state.CurrentStatus = DropdownState.Status.Opening;
            }

            return state;
        }

        static bool drawCaption(Rect position, string[] options, DropdownState state)
        {
            if (0 <= state.Select && state.Select < options.Length)
            {
                state.Caption = options[state.Select];
            }
            bool pushed = GUI.Button(position, state.Caption);
            var texRect = new Rect(position.xMax - 16, position.y, 16, position.height);
            GUI.DrawTexture(texRect, buttonTexture_);
            return pushed;
        }

        static DropdownState openingDropdown(Rect position, string[] options, DropdownState state)
        {
            drawCaption(position, options, state);
            state.CurrentStatus = DropdownState.Status.Opened;
            return state;
        }
        static DropdownState openedDropdown(Rect position, string[] options, DropdownState state)
        {
            drawCaption(position, options, state);
            int newSelect = drawDropdownList(position, options, state);
            if (newSelect >= 0)
            {
                state.Select = newSelect;
                state.CurrentStatus = DropdownState.Status.Closing;
            }
            return state;
        }

        static int drawDropdownList(Rect position, string[] options, DropdownState state)
        {
            int newSelect = -1;
            for (int i = 0; i < options.Length; i++)
            {
                var optionPosition = position;
                optionPosition.y += position.height * (1 + i);
                string text = string.Format("{0}\t{1}", i == state.Select ? "✓" : " ", options[i]);
                if (GUI.Button(optionPosition, text))
                {
                    newSelect = i;
                }
            }
            return newSelect;
        }

        static DropdownState closingDropdown(Rect position, string[] options, DropdownState state)
        {
            drawCaption(position, options, state);
            state.CurrentStatus = DropdownState.Status.Closed;
            return state;
        }
    }
}