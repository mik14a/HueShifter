#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to change the UI color in the Unity Editor.
/// </summary>
public class HueShifter : EditorWindow
{
    [MenuItem("Window/Change UI Schema")]
    /// <summary>
    /// Show the window for changing UI color.
    /// </summary>
    public static void ShowWindow()
    {
        GetWindow<HueShifter>("Change UI Schema");
    }

    /// <summary>
    /// OnEnable is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        var colorSchemaPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "ProjectSettings", "ColorSchema.json");
        if (File.Exists(colorSchemaPath)) {
            var json = File.ReadAllText(colorSchemaPath);
            _uiElementColors = JsonUtility.FromJson<UIElementColors>(json);
        } else {
            _uiElementColors = UIElementColors.Default;
            ApplyThemeColor();
        }

        _syncWithTheme = SessionState.GetBool("syncWithTheme", true);
        _foldoutButtonColor = SessionState.GetBool("foldoutButtonColor", true);
        _foldoutSliderColor = SessionState.GetBool("foldoutSliderColor", true);
        _foldoutToggleColor = SessionState.GetBool("foldoutToggleColor", true);
        _foldoutDropdownColor = SessionState.GetBool("foldoutDropdownColor", true);
        _foldoutInputFieldColor = SessionState.GetBool("foldoutInputFieldColor", true);
        _foldoutScrollbarColor = SessionState.GetBool("foldoutScrollbarColor", true);
        _foldoutScrollRectColor = SessionState.GetBool("foldoutScrollRectColor", true);
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// </summary>
    void OnGUI()
    {
        _syncWithTheme = EditorGUILayout.Toggle("Sync with Theme", _syncWithTheme);

        EditorGUI.BeginChangeCheck();
        _uiElementColors.ThemeColor = EditorGUILayout.ColorField("Theme Color", _uiElementColors.ThemeColor);
        _uiElementColors.BaseColor = EditorGUILayout.ColorField("Base Color", _uiElementColors.BaseColor);
        _uiElementColors.PressedBrightness = EditorGUILayout.Slider("Highlight Brightness", _uiElementColors.PressedBrightness, 0.0f, 1.0f);
        _uiElementColors.DisabledSaturation = EditorGUILayout.Slider("Disabled Saturation", _uiElementColors.DisabledSaturation, 0.0f, 1.0f);
        _uiElementColors.SelectedHueShift = EditorGUILayout.Slider("Selected Hue Shift", _uiElementColors.SelectedHueShift, -0.5f, 0.5f);
        if (EditorGUI.EndChangeCheck()) {
            if (_syncWithTheme) {
                ApplyThemeColor();
            }
        }

        _uiElementColors.TextColor = EditorGUILayout.ColorField("Text Color", _uiElementColors.TextColor);

        _foldoutButtonColor = EditorGUILayout.Foldout(_foldoutButtonColor, "Set the new Button elements");
        if (_foldoutButtonColor) {
            _uiElementColors.ButtonColor = EditorGUILayout.ColorField("Button Color", _uiElementColors.ButtonColor);
            _uiElementColors.ButtonHighlightColor = EditorGUILayout.ColorField("Button Highlight Color", _uiElementColors.ButtonHighlightColor);
            _uiElementColors.ButtonPressedColor = EditorGUILayout.ColorField("Button Pressed Color", _uiElementColors.ButtonPressedColor);
            _uiElementColors.ButtonSelectedColor = EditorGUILayout.ColorField("Button Selected Color", _uiElementColors.ButtonSelectedColor);
            _uiElementColors.ButtonDisabledColor = EditorGUILayout.ColorField("Button Disabled Color", _uiElementColors.ButtonDisabledColor);
        }

        _foldoutSliderColor = EditorGUILayout.Foldout(_foldoutSliderColor, "Set the new Slider elements");
        if (_foldoutSliderColor) {
            _uiElementColors.SliderColor = EditorGUILayout.ColorField("Slider Color", _uiElementColors.SliderColor);
            _uiElementColors.SliderHighlightColor = EditorGUILayout.ColorField("Slider Highlight Color", _uiElementColors.SliderHighlightColor);
            _uiElementColors.SliderPressedColor = EditorGUILayout.ColorField("Slider Pressed Color", _uiElementColors.SliderPressedColor);
            _uiElementColors.SliderSelectedColor = EditorGUILayout.ColorField("Slider Selected Color", _uiElementColors.SliderSelectedColor);
            _uiElementColors.SliderDisabledColor = EditorGUILayout.ColorField("Slider Disabled Color", _uiElementColors.SliderDisabledColor);
            _uiElementColors.SliderFillColor = EditorGUILayout.ColorField("Slider Fill Color", _uiElementColors.SliderFillColor);
        }

        _foldoutToggleColor = EditorGUILayout.Foldout(_foldoutToggleColor, "Set the new Toggle elements");
        if (_foldoutToggleColor) {
            _uiElementColors.ToggleColor = EditorGUILayout.ColorField("Toggle Color", _uiElementColors.ToggleColor);
            _uiElementColors.ToggleHighlightColor = EditorGUILayout.ColorField("Toggle Highlight Color", _uiElementColors.ToggleHighlightColor);
            _uiElementColors.TogglePressedColor = EditorGUILayout.ColorField("Toggle Pressed Color", _uiElementColors.TogglePressedColor);
            _uiElementColors.ToggleSelectedColor = EditorGUILayout.ColorField("Toggle Selected Color", _uiElementColors.ToggleSelectedColor);
            _uiElementColors.ToggleDisabledColor = EditorGUILayout.ColorField("Toggle Disabled Color", _uiElementColors.ToggleDisabledColor);
        }

        _foldoutDropdownColor = EditorGUILayout.Foldout(_foldoutDropdownColor, "Set the new Dropdown elements");
        if (_foldoutDropdownColor) {
            _uiElementColors.DropdownColor = EditorGUILayout.ColorField("Dropdown Color", _uiElementColors.DropdownColor);
            _uiElementColors.DropdownHighlightColor = EditorGUILayout.ColorField("Dropdown Highlight Color", _uiElementColors.DropdownHighlightColor);
            _uiElementColors.DropdownPressedColor = EditorGUILayout.ColorField("Dropdown Pressed Color", _uiElementColors.DropdownPressedColor);
            _uiElementColors.DropdownSelectedColor = EditorGUILayout.ColorField("Dropdown Selected Color", _uiElementColors.DropdownSelectedColor);
            _uiElementColors.DropdownDisabledColor = EditorGUILayout.ColorField("Dropdown Disabled Color", _uiElementColors.DropdownDisabledColor);
        }

        _foldoutInputFieldColor = EditorGUILayout.Foldout(_foldoutInputFieldColor, "Set the new Input Field elements");
        if (_foldoutInputFieldColor) {
            _uiElementColors.InputFieldColor = EditorGUILayout.ColorField("Input Field Color", _uiElementColors.InputFieldColor);
            _uiElementColors.InputFieldHighlightColor = EditorGUILayout.ColorField("Input Field Highlight Color", _uiElementColors.InputFieldHighlightColor);
            _uiElementColors.InputFieldPressedColor = EditorGUILayout.ColorField("Input Field Pressed Color", _uiElementColors.InputFieldPressedColor);
            _uiElementColors.InputFieldSelectedColor = EditorGUILayout.ColorField("Input Field Selected Color", _uiElementColors.InputFieldSelectedColor);
            _uiElementColors.InputFieldDisabledColor = EditorGUILayout.ColorField("Input Field Disabled Color", _uiElementColors.InputFieldDisabledColor);
        }

        _foldoutScrollbarColor = EditorGUILayout.Foldout(_foldoutScrollbarColor, "Set the new Scrollbar elements");
        if (_foldoutScrollbarColor) {
            _uiElementColors.ScrollbarColor = EditorGUILayout.ColorField("Scrollbar Color", _uiElementColors.ScrollbarColor);
            _uiElementColors.ScrollbarHighlightColor = EditorGUILayout.ColorField("Scrollbar Highlight Color", _uiElementColors.ScrollbarHighlightColor);
            _uiElementColors.ScrollbarPressedColor = EditorGUILayout.ColorField("Scrollbar Pressed Color", _uiElementColors.ScrollbarPressedColor);
            _uiElementColors.ScrollbarSelectedColor = EditorGUILayout.ColorField("Scrollbar Selected Color", _uiElementColors.ScrollbarSelectedColor);
            _uiElementColors.ScrollbarDisabledColor = EditorGUILayout.ColorField("Scrollbar Disabled Color", _uiElementColors.ScrollbarDisabledColor);
            _uiElementColors.ScrollbarImageColor = EditorGUILayout.ColorField("Scrollbar Image Color", _uiElementColors.ScrollbarImageColor);
        }

        _foldoutScrollRectColor = EditorGUILayout.Foldout(_foldoutScrollRectColor, "Set the new Scroll Rect elements");
        if (_foldoutScrollRectColor) {
            _uiElementColors.ScrollRectColor = EditorGUILayout.ColorField("Scroll Rect Color", _uiElementColors.ScrollRectColor);
        }

        if (GUILayout.Button("Apply Color")) {
            Undo.RecordObject(this, "Apply UI Color");
            ChangeColorInTextElements(_uiElementColors.TextColor);
            ChangeColorInButtonElements(_uiElementColors.ButtonColor, _uiElementColors.ButtonHighlightColor, _uiElementColors.ButtonPressedColor, _uiElementColors.ButtonDisabledColor, _uiElementColors.ButtonSelectedColor);
            ChangeColorInSliderElements(_uiElementColors.SliderColor, _uiElementColors.SliderHighlightColor, _uiElementColors.SliderPressedColor, _uiElementColors.SliderDisabledColor, _uiElementColors.SliderFillColor, _uiElementColors.SliderSelectedColor);
            ChangeColorInToggleElements(_uiElementColors.ToggleColor, _uiElementColors.ToggleHighlightColor, _uiElementColors.TogglePressedColor, _uiElementColors.ToggleDisabledColor, _uiElementColors.ToggleSelectedColor);
            ChangeColorInDropdownElements(_uiElementColors.DropdownColor, _uiElementColors.DropdownHighlightColor, _uiElementColors.DropdownPressedColor, _uiElementColors.DropdownDisabledColor, _uiElementColors.DropdownSelectedColor);
            ChangeColorInInputFieldElements(_uiElementColors.InputFieldColor, _uiElementColors.InputFieldHighlightColor, _uiElementColors.InputFieldPressedColor, _uiElementColors.InputFieldDisabledColor, _uiElementColors.InputFieldSelectedColor);
            ChangeColorInScrollbarElements(_uiElementColors.ScrollbarColor, _uiElementColors.ScrollbarHighlightColor, _uiElementColors.ScrollbarPressedColor, _uiElementColors.ScrollbarDisabledColor, _uiElementColors.ScrollbarImageColor, _uiElementColors.ScrollbarSelectedColor);
            ChangeColorInScrollRectElements(_uiElementColors.ScrollRectColor);
        }
    }

    /// <summary>
    /// OnDisable is called when the object becomes disabled and inactive.
    /// </summary>
    void OnDisable()
    {
        var colorSchemaPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "ProjectSettings", "ColorSchema.json");
        var json = JsonUtility.ToJson(_uiElementColors, true);
        File.WriteAllText(colorSchemaPath, json);

        SessionState.SetBool("syncWithTheme", _syncWithTheme);
        SessionState.SetBool("foldoutButtonColor", _foldoutButtonColor);
        SessionState.SetBool("foldoutSliderColor", _foldoutSliderColor);
        SessionState.SetBool("foldoutToggleColor", _foldoutToggleColor);
        SessionState.SetBool("foldoutDropdownColor", _foldoutDropdownColor);
        SessionState.SetBool("foldoutInputFieldColor", _foldoutInputFieldColor);
        SessionState.SetBool("foldoutScrollbarColor", _foldoutScrollbarColor);
        SessionState.SetBool("foldoutScrollRectColor", _foldoutScrollRectColor);
    }

    /// <summary>
    /// Apply the theme color to the UI elements.
    /// </summary>
    void ApplyThemeColor()
    {
        // Step 1: Set the base color for all UI elements.
        _uiElementColors.ButtonColor = _uiElementColors.DropdownColor = _uiElementColors.InputFieldColor = _uiElementColors.ScrollbarColor = _uiElementColors.SliderColor = _uiElementColors.ToggleColor = _uiElementColors.BaseColor;
        _uiElementColors.SliderFillColor = _uiElementColors.ScrollbarImageColor = _uiElementColors.ScrollRectColor = _uiElementColors.BaseColor;

        // Step 2: Compute the text color by inverting the lightness of the base color.
        var (h, s, l) = Rgb2Hsl(_uiElementColors.BaseColor);
        var textColor = Hsl2Rgb(h, s, l > 0.5f ? 0.2f : 0.8f);
        _uiElementColors.TextColor = textColor;

        // Step 3: Compute the colors for different states of the UI elements based on the theme color.
        (h, s, l) = Rgb2Hsl(_uiElementColors.ThemeColor);
        var highlightColor = Hsl2Rgb(h, s, l);
        var pressedColor = Hsl2Rgb(h, s, Math.Min(l + _uiElementColors.PressedBrightness, 1.0f));
        var disabledColor = Hsl2Rgb(h, s * _uiElementColors.DisabledSaturation, l);
        var selectedColor = Hsl2Rgb((h + _uiElementColors.SelectedHueShift) % 1.0f, s, l);
        _uiElementColors.ButtonHighlightColor = _uiElementColors.DropdownHighlightColor = _uiElementColors.InputFieldHighlightColor = _uiElementColors.ScrollbarHighlightColor = _uiElementColors.SliderHighlightColor = _uiElementColors.ToggleHighlightColor = highlightColor;
        _uiElementColors.ButtonPressedColor = _uiElementColors.SliderPressedColor = _uiElementColors.TogglePressedColor = _uiElementColors.DropdownPressedColor = _uiElementColors.InputFieldPressedColor = _uiElementColors.ScrollbarPressedColor = pressedColor;
        _uiElementColors.ButtonDisabledColor = _uiElementColors.DropdownDisabledColor = _uiElementColors.InputFieldDisabledColor = _uiElementColors.ScrollbarDisabledColor = _uiElementColors.SliderDisabledColor = _uiElementColors.ToggleDisabledColor = disabledColor;
        _uiElementColors.ButtonSelectedColor = _uiElementColors.DropdownSelectedColor = _uiElementColors.InputFieldSelectedColor = _uiElementColors.ScrollbarSelectedColor = _uiElementColors.SliderSelectedColor = _uiElementColors.ToggleSelectedColor = selectedColor;
    }

    /// <summary>
    /// Change the color in text elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    static void ChangeColorInTextElements(Color color)
    {
        foreach (var textElement in Resources.FindObjectsOfTypeAll(typeof(Text)).Cast<Text>()) {
            Undo.RecordObject(textElement, "Change Text Color");
            textElement.color = color;
        }
    }

    /// <summary>
    /// Change the color in image elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    static void ChangeColorInImageElements(Color color)
    {
        foreach (var imageElement in Resources.FindObjectsOfTypeAll(typeof(Image)).Cast<Image>()) {
            Undo.RecordObject(imageElement, "Change Image Color");
            imageElement.color = color;
        }
    }

    /// <summary>
    /// Change the color in button elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    /// <param name="highlightColor">The new highlight color.</param>
    /// <param name="pressedColor">The new pressed color.</param>
    /// <param name="disabledColor">The new disabled color.</param>
    /// <param name="selectedColor">The new selected color.</param>
    static void ChangeColorInButtonElements(Color color, Color highlightColor, Color pressedColor, Color disabledColor, Color selectedColor)
    {
        foreach (var buttonElement in Resources.FindObjectsOfTypeAll(typeof(Button)).Cast<Button>()) {
            Undo.RecordObject(buttonElement, "Change Button Color");
            var colors = buttonElement.colors;
            colors.normalColor = color;
            colors.highlightedColor = highlightColor;
            colors.pressedColor = pressedColor;
            colors.disabledColor = disabledColor;
            colors.selectedColor = selectedColor;
            buttonElement.colors = colors;
        }
    }

    /// <summary>
    /// Change the color in slider elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    /// <param name="highlightColor">The new highlight color.</param>
    /// <param name="pressedColor">The new pressed color.</param>
    /// <param name="disabledColor">The new disabled color.</param>
    /// <param name="fillColor">The new fill color.</param>
    /// <param name="selectedColor">The new selected color.</param>
    static void ChangeColorInSliderElements(Color color, Color highlightColor, Color pressedColor, Color disabledColor, Color fillColor, Color selectedColor)
    {
        foreach (var sliderElement in Resources.FindObjectsOfTypeAll(typeof(Slider)).Cast<Slider>()) {
            Undo.RecordObject(sliderElement, "Change Slider Color");
            var colors = sliderElement.colors;
            colors.normalColor = color;
            colors.highlightedColor = highlightColor;
            colors.pressedColor = pressedColor;
            colors.disabledColor = disabledColor;
            colors.selectedColor = selectedColor;
            sliderElement.colors = colors;
            if (sliderElement.fillRect != null) {
                Undo.RecordObject(sliderElement.fillRect.GetComponent<Image>(), "Change Slider Fill Color");
                sliderElement.fillRect.GetComponent<Image>().color = fillColor;
            }
        }
    }

    /// <summary>
    /// Change the color in toggle elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    /// <param name="highlightColor">The new highlight color.</param>
    /// <param name="pressedColor">The new pressed color.</param>
    /// <param name="disabledColor">The new disabled color.</param>
    /// <param name="selectedColor">The new selected color.</param>
    static void ChangeColorInToggleElements(Color color, Color highlightColor, Color pressedColor, Color disabledColor, Color selectedColor)
    {
        foreach (var toggleElement in Resources.FindObjectsOfTypeAll(typeof(Toggle)).Cast<Toggle>()) {
            Undo.RecordObject(toggleElement, "Change Toggle Color");
            var colors = toggleElement.colors;
            colors.normalColor = color;
            colors.highlightedColor = highlightColor;
            colors.pressedColor = pressedColor;
            colors.disabledColor = disabledColor;
            colors.selectedColor = selectedColor;
            toggleElement.colors = colors;
        }
    }

    /// <summary>
    /// Change the color in dropdown elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    /// <param name="highlightColor">The new highlight color.</param>
    /// <param name="pressedColor">The new pressed color.</param>
    /// <param name="disabledColor">The new disabled color.</param>
    /// <param name="selectedColor">The new selected color.</param>
    static void ChangeColorInDropdownElements(Color color, Color highlightColor, Color pressedColor, Color disabledColor, Color selectedColor)
    {
        foreach (var dropdownElement in Resources.FindObjectsOfTypeAll(typeof(Dropdown)).Cast<Dropdown>()) {
            Undo.RecordObject(dropdownElement, "Change Dropdown Color");
            var colors = dropdownElement.colors;
            colors.normalColor = color;
            colors.highlightedColor = highlightColor;
            colors.pressedColor = pressedColor;
            colors.disabledColor = disabledColor;
            colors.selectedColor = selectedColor;
            dropdownElement.colors = colors;
        }
    }

    /// <summary>
    /// Change the color in input field elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    /// <param name="highlightColor">The new highlight color.</param>
    /// <param name="pressedColor">The new pressed color.</param>
    /// <param name="disabledColor">The new disabled color.</param>
    /// <param name="selectedColor">The new selected color.</param>
    static void ChangeColorInInputFieldElements(Color color, Color highlightColor, Color pressedColor, Color disabledColor, Color selectedColor)
    {
        foreach (var inputFieldElement in Resources.FindObjectsOfTypeAll(typeof(InputField)).Cast<InputField>()) {
            Undo.RecordObject(inputFieldElement, "Change Input Field Color");
            var colors = inputFieldElement.colors;
            colors.normalColor = color;
            colors.highlightedColor = highlightColor;
            colors.pressedColor = pressedColor;
            colors.disabledColor = disabledColor;
            colors.selectedColor = selectedColor;
            inputFieldElement.colors = colors;
        }
    }

    /// <summary>
    /// Change the color in scrollbar elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    /// <param name="highlightColor">The new highlight color.</param>
    /// <param name="pressedColor">The new pressed color.</param>
    /// <param name="disabledColor">The new disabled color.</param>
    /// <param name="imageColor">The new image color.</param>
    /// <param name="selectedColor">The new selected color.</param>
    static void ChangeColorInScrollbarElements(Color color, Color highlightColor, Color pressedColor, Color disabledColor, Color imageColor, Color selectedColor)
    {
        foreach (var scrollbarElement in Resources.FindObjectsOfTypeAll(typeof(Scrollbar)).Cast<Scrollbar>()) {
            Undo.RecordObject(scrollbarElement, "Change Scrollbar Color");
            var colors = scrollbarElement.colors;
            colors.normalColor = color;
            colors.highlightedColor = highlightColor;
            colors.pressedColor = pressedColor;
            colors.disabledColor = disabledColor;
            colors.selectedColor = selectedColor;
            scrollbarElement.colors = colors;
            if (scrollbarElement.GetComponent<Image>() != null) {
                Undo.RecordObject(scrollbarElement.GetComponent<Image>(), "Change Scrollbar Image Color");
                scrollbarElement.GetComponent<Image>().color = imageColor;
            }
        }
    }

    /// <summary>
    /// Change the color in scroll rect elements.
    /// </summary>
    /// <param name="color">The new color.</param>
    static void ChangeColorInScrollRectElements(Color color)
    {
        foreach (var scrollViewElement in Resources.FindObjectsOfTypeAll(typeof(ScrollRect)).Cast<ScrollRect>()) {
            Undo.RecordObject(scrollViewElement, "Change Scroll Rect Color");
            if (scrollViewElement.GetComponent<Image>() != null) {
                Undo.RecordObject(scrollViewElement.GetComponent<Image>(), "Change Scroll Rect Image Color");
                scrollViewElement.GetComponent<Image>().color = color;
            }
        }
    }

    /// <summary>
    /// Helper function to convert from HSL color space to RGB color space.
    /// </summary>
    /// <param name="h">Hue (range 0.0 - 1.0)</param>
    /// <param name="s">Saturation (range 0.0 - 1.0)</param>
    /// <param name="l">Lightness (range 0.0 - 1.0)</param>
    /// <returns>RGB color (each component in the range 0.0 - 1.0)</returns>
    static Color Hsl2Rgb(float h, float s, float l)
    {
        float r, g, b;
        if (s == 0) {
            r = g = b = l; // achromatic
        } else {
            var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            var p = 2 * l - q;
            r = Hue2Rgb(p, q, h + 1 / 3f);
            g = Hue2Rgb(p, q, h);
            b = Hue2Rgb(p, q, h - 1 / 3f);
        }
        return new Color(r, g, b);

    }

    /// <summary>
    /// Helper function to convert from hue to RGB.
    /// </summary>
    /// <param name="p">Part of the hue</param>
    /// <param name="q">Part of the hue</param>
    /// <param name="t">Part of the hue</param>
    /// <returns>Part of the RGB color</returns>
    /// <remarks>
    /// This function is used to assist in the conversion from hue to RGB.
    /// </remarks>
    static float Hue2Rgb(float p, float q, float t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        return t < 1 / 6f ? p + (q - p) * 6 * t : t < 1 / 2f ? q : t < 2 / 3f ? p + (q - p) * (2 / 3f - t) * 6 : p;
    }

    /// <summary>
    /// Function to convert from RGB color space to HSL color space.
    /// </summary>
    /// <param name="rgb">RGB color (each component in the range 0.0 - 1.0)</param>
    /// <returns>Tuple of HSL color (each component in the range 0.0 - 1.0)</returns>
    static (float, float, float) Rgb2Hsl(Color rgb)
    {
        var r = rgb.r;
        var g = rgb.g;
        var b = rgb.b;
        var max = Mathf.Max(r, g, b);
        var min = Mathf.Min(r, g, b);
        float h, s, l;
        l = (max + min) / 2;
        if (max == min) {
            h = s = 0; // achromatic
        } else {
            var d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            h = max == r ? (g - b) / d + (g < b ? 6 : 0) : max == g ? (b - r) / d + 2 : (r - g) / d + 4;
            h /= 6;
        }
        return (h, s, l);
    }

    bool _syncWithTheme = true;
    bool _foldoutButtonColor = true;
    bool _foldoutSliderColor = true;
    bool _foldoutToggleColor = true;
    bool _foldoutDropdownColor = true;
    bool _foldoutInputFieldColor = true;
    bool _foldoutScrollbarColor = true;
    bool _foldoutScrollRectColor = true;
    UIElementColors _uiElementColors;

    [Serializable]
    public class UIElementColors
    {
        public static UIElementColors Default => new() {
            ThemeColor = new Color(255 / 255f, 96 / 255f, 192 / 255f, 1),
            BaseColor = new Color(64 / 255f, 64 / 255f, 64 / 255f, 1),
            PressedBrightness = 0.2f,
            DisabledSaturation = 0.3f,
            SelectedHueShift = 0.5f
        };

        public Color ThemeColor;
        public Color BaseColor;
        public float PressedBrightness;
        public float DisabledSaturation;
        public float SelectedHueShift;
        public Color TextColor;
        public Color ButtonColor;
        public Color ButtonHighlightColor;
        public Color ButtonPressedColor;
        public Color ButtonDisabledColor;
        public Color ButtonSelectedColor;
        public Color SliderColor;
        public Color SliderHighlightColor;
        public Color SliderPressedColor;
        public Color SliderDisabledColor;
        public Color SliderFillColor;
        public Color SliderSelectedColor;
        public Color ToggleColor;
        public Color ToggleHighlightColor;
        public Color TogglePressedColor;
        public Color ToggleDisabledColor;
        public Color ToggleSelectedColor;
        public Color DropdownColor;
        public Color DropdownHighlightColor;
        public Color DropdownPressedColor;
        public Color DropdownDisabledColor;
        public Color DropdownSelectedColor;
        public Color InputFieldColor;
        public Color InputFieldHighlightColor;
        public Color InputFieldPressedColor;
        public Color InputFieldDisabledColor;
        public Color InputFieldSelectedColor;
        public Color ScrollbarColor;
        public Color ScrollbarHighlightColor;
        public Color ScrollbarPressedColor;
        public Color ScrollbarDisabledColor;
        public Color ScrollbarSelectedColor;
        public Color ScrollbarImageColor;
        public Color ScrollRectColor;
    }
}
#endif
