using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[] statsValues = new[] {10, 10, 10, 10, 10, 10};
        ObservableCollection<int> compValues = new ObservableCollection<int>() /*new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}*/;
        bool[] masteryValues = new[] {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false};

    public MainWindow()
        {
            InitializeComponent();
            for (int i = 0;i <18;i++)
                compValues.Add(0);
            CompetenceList.ItemsSource = compValues;
            SetSheetImageFromSource(@"C:\Users\PLOE3004\Documents\Rider\WpfApp1\WpfApp1\Sheet.png");
        }

        void SetSheetImageFromSource(string source)
        {
            BitmapImage image = new BitmapImage(new Uri(source));
            BackgroundSheet.Source = image;
        }

        void RollClicked(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                List<int> _rolledValues = new List<int>();
                int _lowest = 7;
                for (int j = 0; j < 4; j++)
                {
                    int _new = rnd.Next(1, 7);
                    _rolledValues.Add(_new);
                    if (_new < _lowest)
                        _lowest = _new;
                }

                _rolledValues.Remove(_lowest);
                statsValues[i] = _rolledValues[0] + _rolledValues[1] + _rolledValues[2];
            }

            SetStatsValues();
        }

        void SetStatsValues()
        {
            SetStatAndModifier(Strength, StrengthMod, statsValues[0]);
            SetStatAndModifier(Dexterity, DexterityMod, statsValues[1]);
            AC.Text = (10 +(statsValues[1] >= 10 ? MathF.Truncate((statsValues[1] - 10) / 2.0f) : MathF.Truncate((statsValues[1] - 11) / 2.0f))).ToString();
            SetStatAndModifier(Constitution, ConstitutionMod, statsValues[2]);
            SetStatAndModifier(Intelligence, IntelligenceMod, statsValues[3]);
            SetStatAndModifier(Wisdom, WisdomMod, statsValues[4]);
            SetStatAndModifier(Charisma, CharismaMod, statsValues[5]);

            SetCompValues();
        }

        void SetStatAndModifier(TextBlock _statBlock, TextBlock _modBlock, int _value)
        {
            _statBlock.Text = _value.ToString();
            _modBlock.Text = _value >= 10 ? "+" + MathF.Truncate((_value - 10) / 2.0f) : MathF.Truncate((_value - 11) / 2.0f).ToString();
        }

        void SetCompValues()
        {
            compValues[3] = statsValues[0] >= 10 ? (int)MathF.Truncate((statsValues[0] - 10) / 2.0f) : (int)MathF.Truncate((statsValues[0] - 11) / 2.0f);
            compValues[0] = compValues[15] = compValues[16] = statsValues[1] >= 10 ? (int)MathF.Truncate((statsValues[1] - 10) / 2.0f) : (int)MathF.Truncate((statsValues[1] - 11) / 2.0f);
            compValues[2] = compValues[5] = compValues[8] = compValues[10] = compValues[14] = statsValues[3] >= 10 ? (int)MathF.Truncate((statsValues[3] - 10) / 2.0f) : (int)MathF.Truncate((statsValues[3] - 11) / 2.0f);
            compValues[1] = compValues[6] = compValues[9] = compValues[11] = compValues[17] = statsValues[4] >= 10 ? (int)MathF.Truncate((statsValues[4] - 10) / 2.0f) : (int)MathF.Truncate((statsValues[4] - 11) / 2.0f);
            compValues[4] = compValues[7] = compValues[12] = compValues[13] = statsValues[5] >= 10 ? (int)MathF.Truncate((statsValues[5] - 10) / 2.0f) : (int)MathF.Truncate((statsValues[5] - 11) / 2.0f);
           // compValues[0] +=MasteryList.RowDefinitions[0] ? 1 : 0;

           for (int i = 0; i < 18; i++)
           {
               compValues[i] += masteryValues[i] ? 2 : 0;
           }

           //CompetenceList.ItemsSource = compValues;
        }

        void SaveButton_OnClick(object _sender, RoutedEventArgs _e)
        {
            Rect rect = new Rect(this.RenderSize);
            rect.Width -= 130;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
                (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(this);
            //encode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            System.IO.File.WriteAllBytes(@"C:\Users\PLOE3004\Pictures\" + NameText.Text.ToString() +"_Sheet.png", ms.ToArray());
            Console.WriteLine("Saved");
        }

        void MasteryClick(object _sender, RoutedEventArgs _e)
        {
            CheckBox _box = (CheckBox) _sender;
            string _nameForInt =  _box.Name.Remove(0,7);
            int _num = Convert.ToInt32(_nameForInt);
            compValues[_num] += _box.IsChecked.Value ? 2 : -2;
            masteryValues[_num] = _box.IsChecked.Value;
            
            //CompetenceList.ItemsSource = compValues;
        }
        
    }
}