# HueShifter

HueShifter is a little Unity editor extension that lets you easily change the colors of UI elements in your project. Kinda cool, isn't it?

## Features

- Sync with theme color: Change the theme color and watch as all your UI elements automatically update. Magic!
- Customize each UI element: You can individually set the colors for buttons, sliders, toggles, dropdowns, input fields, scrollbars, and scroll rects. Total control!
- Save and load color schemes: Save your color scheme as a JSON file and load it later. Because who wants to do the same work twice?

## Installation

To install HueShifter, follow these steps:

1. Open your Unity project and go to the Unity Package Manager (Window > Package Manager).
1. Click on the '+' button in the top left corner of the Package Manager window.
1. Select 'Add package from git URL...'.
1. Enter the following URL and press 'Add': `https://github.com/mik14a/HueShifter.git?path=Assets/Plugins/HueShifter`
1. Unity will now download and install the HueShifter package into your project.

Make sure that your Unity version is compatible with the package. HueShifter requires Unity 2019.4 or later.

## How to use

1. Go to Window > Change UI Schema in the Unity editor menu bar.
1. The HueShifter window will pop up. This is where you set the colors for your UI elements.
1. Check the Sync with Theme box and your UI elements will automatically update when you change the theme color.
1. Adjust the following sliders to fine-tune your colors:
   - Highlight Brightness: Adjust the brightness when highlighted.
   - Disabled Saturation: Adjust the saturation when disabled.
   - Selected Hue Shift: Adjust the hue shift when selected.
1. Once you're happy with your settings, hit the "Apply Color" button to apply the changes. This button applies your color settings to all UI elements. Voila! Your UI is now color-coordinated!

## License

This project is open source under the MIT license. Check out the LICENSE file for more details.
