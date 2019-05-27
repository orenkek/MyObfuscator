using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Obfuscate;

namespace Obfuscate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Win32.OpenFileDialog dlg;
        string progInfo;
        string obInfo;
        bool? ifFilesSelected;

        public MainWindow()
        {
            InitializeComponent();
            dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".cs";
            dlg.Filter = "File with code (.cs)|*.cs";

            StreamReader sr = new StreamReader("ProgInfo.txt");
            obInfo = sr.ReadToEnd();
            sr.Close();
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("Images/imag.png", UriKind.Relative));
            TextBlock tbl = new TextBlock();
            img.Height = 25;
            img.Width = 25;
            tbl.Text = "Открыть файл";
            img.Margin = new Thickness(5);
            tbl.VerticalAlignment = VerticalAlignment.Center;
            sp.Children.Add(img);
            sp.Children.Add(tbl);
            btnBrowse.Content = sp;
            btnBrowse.Width = 120;
            btnBrowse.Margin = new Thickness(5);
            Image start = new Image();
            start.Source = new BitmapImage(new Uri("Images/start.png", UriKind.Relative));
            TextBlock tblText = new TextBlock();
            tblText.Text = "Обфусцировать";
            StackPanel butt = new StackPanel();
            butt.Orientation = Orientation.Horizontal;
            start.Height = 25;
            start.Width = 25;
            tblText.VerticalAlignment = VerticalAlignment.Center;
            start.Margin = new Thickness(5);
            butt.Children.Add(start);
            butt.Children.Add(tblText);
            btnObfuscate.Content = butt;
        }

        private void BtnObfuscate_Click(object sender, RoutedEventArgs e)
        {
            StreamReader sr;
            StringBuilder code = new StringBuilder();
        }
        public static void ShowExceptionMessageBox()
        {
            MessageBox.Show("Ошибка, неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
        }

        private void BtnBrowse_Click_1(object sender, RoutedEventArgs e)
        {
            ifFilesSelected = dlg.ShowDialog();
            if (ifFilesSelected == true)
            {
                tbxProcesses.Text = "Selected file\n";
                string WorkPath = dlg.FileName;
                WorkPath += '\n';
                tbxProcesses.Text += WorkPath;
                StreamReader sr = new StreamReader(dlg.FileName);
                tbxCode.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void BtnObfuscate_Click_1(object sender, RoutedEventArgs e)
        {
            StreamReader SR;
            StringBuilder Code = new StringBuilder();
            if ((ifFilesSelected == false) || (ifFilesSelected == null))
            {
                MessageBox.Show("Файл не был выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            if (ifFilesSelected == true)//если файлы выбраны
            {
                for (int i = 0; i < dlg.FileNames.Length; i++)
                {
                    SR = new StreamReader(dlg.FileNames[i]);
                    Code.Append(SR.ReadToEnd().ToString());
                }
            }

            Obfuscate.obfuscator_lib OC = new Obfuscate.obfuscator_lib(Code);
            StringBuilder sb = new StringBuilder();
            sb.Append(OC.GetObfuscatedCode());
            tbxObfuscatedCode.Text = sb.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.DefaultExt = ".cs";
            sfd.Filter = "File with code (.cs)|*.cs";
            sfd.InitialDirectory = @"c:\";
            if (sfd.ShowDialog() == true)
                File.WriteAllText(sfd.FileName, tbxObfuscatedCode.Text);

        }
    }
}
