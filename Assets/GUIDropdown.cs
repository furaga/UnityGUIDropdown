using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GUIExtension
{
    public static class GUIEx
    {
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


        public class DropdownStyles
        {
            public GUIStyle Caption;
            public GUIStyle Option;
            public Color ArrowColor = new Color(0.5f, 0.5f, 0.5f);
            public int ArrowSize = 16;
            public int ArrowMargin = 8;

            public DropdownStyles(GUIStyle caption, GUIStyle option)
            {
                this.Caption = caption;
                this.Option = option;
            }

            public DropdownStyles(GUIStyle caption, GUIStyle option, Color arrowColor, int arrowSize = 16, int arrowMargin = 8)
            {
                this.Caption = caption;
                this.Option = option;
                this.ArrowColor = arrowColor;
                this.ArrowSize = arrowSize;
                this.ArrowMargin = arrowMargin;
            }
        }

        static Texture2D arrowTexture_ = null;
        static GUISkin skin_ = null;
        static DropdownStyles defaultDropdownStyles_ = null;

        static void setup()
        {
            if (arrowTexture_ == null)
            {
                arrowTexture_ = Resources.Load<Texture2D>("arrow");
            }
            if (skin_ == null)
            {
                skin_ = Resources.Load<GUISkin>("Dropdown");
            }
            if (defaultDropdownStyles_ == null)
            {
                defaultDropdownStyles_ = new DropdownStyles(skin_.button, skin_.FindStyle("dropdown_option"));
            }
        }

        public static DropdownState Dropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles = null)
        {
            setup();

            if (styles == null)
            {
                styles = defaultDropdownStyles_;
            }

            switch (state.CurrentStatus)
            {
                case DropdownState.Status.Closed:
                    return closedDropdown(position, options, state, styles);
                case DropdownState.Status.Opening:
                    return openingDropdown(position, options, state, styles);
                case DropdownState.Status.Opened:
                    return openedDropdown(position, options, state, styles);
                case DropdownState.Status.Closing:
                    return closingDropdown(position, options, state, styles);
            }

            return state;
        }

        static DropdownState closedDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
        {
            if (drawCaption(position, options, state, styles))
            {
                state.CurrentStatus = DropdownState.Status.Opening;
            }
            return state;
        }

        static DropdownState openingDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
        {
            const float fadeTime = 0.1f;

            float dt = Time.time - state.CurrentStatusStartTime;
            drawCaption(position, options, state, styles);

            var prevColor = GUI.color;
            GUI.color = new Color(1, 1, 1, dt / fadeTime);
            drawDropdownList(position, options, state, styles);
            GUI.color = prevColor;

            if (dt >= fadeTime)
            {
                state.CurrentStatus = DropdownState.Status.Opened;
            }
            return state;
        }

        static DropdownState openedDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
        {
            if (drawCaption(position, options, state, styles))
            {
                state.CurrentStatus = DropdownState.Status.Closing;
            }
            int newSelect = drawDropdownList(position, options, state, styles);
            if (newSelect >= 0)
            {
                state.NextSelect = newSelect;
                state.CurrentStatus = DropdownState.Status.Closing;
            }
            return state;
        }

        static DropdownState closingDropdown(Rect position, string[] options, DropdownState state, DropdownStyles styles)
        {
            const float fadeTime = 0.1f;

            float dt = Time.time - state.CurrentStatusStartTime;
            drawCaption(position, options, state, styles);

            var prevColor = GUI.color;
            GUI.color = new Color(1, 1, 1, 1 - dt / fadeTime);
            drawDropdownList(position, options, state, styles);
            GUI.color = prevColor;

            if (dt >= fadeTime)
            {
                state.Select = state.NextSelect;
                state.CurrentStatus = DropdownState.Status.Closed;
            }
            return state;
        }

        static bool drawCaption(Rect position, string[] options, DropdownState state, DropdownStyles styles)
        {
            if (0 <= state.Select && state.Select < options.Length)
            {
                state.Caption = options[state.Select];
            }

            // Caption
            bool pushed = GUI.Button(position, state.Caption, styles.Caption);

            // Arrow
            var arrowPosition = new Rect(
                position.xMax - styles.ArrowMargin - styles.ArrowSize,
                position.center.y - styles.ArrowSize / 2,
                styles.ArrowSize, styles.ArrowSize);
            var prevColor = GUI.color;
            GUI.color = styles.ArrowColor;
            GUI.DrawTexture(arrowPosition, arrowTexture_);
            GUI.color = prevColor;

            return pushed;
        }

        static int drawDropdownList(Rect position, string[] options, DropdownState state, DropdownStyles styles)
        {
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
                if (GUI.Button(optionPosition, text, styles.Option))
                {
                    newSelect = i;
                }
            }

            return newSelect;
        }

    }
}