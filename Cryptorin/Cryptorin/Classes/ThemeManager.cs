using System.Drawing;

namespace Cryptorin.Classes
{
    public class ThemeManager
    {
        Color colorLabelDark = Color.White;
        Color colorlabelLight = Color.Black;

        Color colorDarkButton = Color.FromArgb(255, 74, 80, 89);
        Color colorLightButton = Color.FromArgb(255, 124, 148, 166);
        //Color colorDarkessButton = Color.FromArgb(255, 48, 52, 59);

        Color colorMainBackgroundDark = Color.FromArgb(255, 44, 52, 64);
        Color colorMainBackgroundLight = Color.FromArgb(255, 244, 244, 244);




        Color colorEntryBackgroundDark = Color.FromArgb(255, 36, 38, 43);
        //Color colorEntryBackgroundLight = Color.FromArgb(255, 236, 236, 236);
        Color colorEntryBackgroundLight = Color.FromArgb(255, 205, 205, 203);

        Color colorEntryTextColorDark = Color.White;
        Color colorEntryTextColorLight = Color.Black;

        Color colorEntryPlaceHolderColorDark = Color.FromArgb(255, 184, 184, 184);
        Color colorEntryPlaceHolderColorLight = Color.FromArgb(255, 158, 142, 142);

        Color colorcolorMainAccentDark = Color.FromArgb(255, 88, 102, 122);
        //Color colorcolorMainAccentLight = Color.FromArgb(255, 167, 197, 199);
        Color colorcolorMainAccentLight = Color.FromArgb(255, 124, 148, 166);



        /// <summary>
        /// Set a global dark theme
        /// </summary>
        public void SetDark()
        {
            App.Current.Resources["colorLabel"] = colorLabelDark;
            App.Current.Resources["colorButtonBackground"] = colorDarkButton;
            App.Current.Resources["colorEntryTextColor"] = colorEntryTextColorDark;
            App.Current.Resources["colorEntryBackground"] = colorEntryBackgroundDark;
            App.Current.Resources["colorBackgroundMain"] = colorMainBackgroundDark;
            App.Current.Resources["colorEntryPlaceholder"] = colorEntryPlaceHolderColorDark;
            App.Current.Resources["colorMainAccent"] = colorcolorMainAccentDark;
        }

        /// <summary>
        /// Set a global light theme
        /// </summary>
        public void SetLight()
        {
            App.Current.Resources["colorLabel"] = colorlabelLight;
            App.Current.Resources["colorButtonBackground"] = colorLightButton;
            App.Current.Resources["colorEntryTextColor"] = colorEntryTextColorLight;
            App.Current.Resources["colorEntryBackground"] = colorEntryBackgroundLight;
            App.Current.Resources["colorBackgroundMain"] = colorMainBackgroundLight;
            App.Current.Resources["colorEntryPlaceholder"] = colorEntryPlaceHolderColorLight;
            App.Current.Resources["colorMainAccent"] = colorcolorMainAccentLight;
        }
    }
}
