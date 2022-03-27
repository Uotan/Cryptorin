using System.Drawing;

namespace Cryptorin.Classes
{
    public class ThemeManager
    {
        public Color colorLabelDark = Color.White;
        public Color colorlabelLight = Color.Black;

        public Color colorDarkButton = Color.FromArgb(255, 74, 80, 89);
        public Color colorLightButton = Color.FromArgb(255, 124, 148, 166);

        public Color colorMainBackgroundDark = Color.FromArgb(255, 44, 52, 64);
        public Color colorMainBackgroundLight = Color.FromArgb(255, 246, 246, 246);




        public Color colorEntryBackgroundDark = Color.FromArgb(255, 36, 38, 43);
        public Color colorEntryBackgroundLight = Color.FromArgb(255, 237, 237, 237);

        public Color colorEntryTextColorDark = Color.White;
        public Color colorEntryTextColorLight = Color.Black;

        public Color colorEntryPlaceHolderColorDark = Color.FromArgb(255, 184, 184, 184);
        public Color colorEntryPlaceHolderColorLight = Color.FromArgb(255, 158, 142, 142);

        public Color colorcolorMainAccentDark = Color.FromArgb(255, 88, 102, 122);
        public Color colorcolorMainAccentLight = Color.FromArgb(255, 167, 197, 199);


        

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
