
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CompareFolders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> lstFileType = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            radAllType.IsChecked = true;
            txtother.Visibility = Visibility.Collapsed;
            tblMessage.Visibility = Visibility.Collapsed;
        }
        public void Compare()
        {
            string strpathA = txtFolder1.Text;  
            string strpathB = txtFolder2.Text;
            
            if (strpathA.Length > 0 && strpathB.Length > 0)
            {
                CompareDirs cd = new CompareDirs(strpathA, strpathB);
                LoadFileTypeFilter();
                cd.Compare(lstFileType);
                lstDiffResults1.ItemsSource = cd.lstDiffResult1;
                lstSameResults.ItemsSource = cd.lstSameResult1;
                lstDiffResults2.ItemsSource = cd.lstDiffResult2;

                txtHeader1.Text = "Only in Source ( " + cd.lstDiffResult1.Count + " ) ";
                txtHeader.Text = "Common ( " + cd.lstSameResult1.Count + " ) ";
                txtHeader2.Text = "Only in Destination ( " + cd.lstDiffResult2.Count + " ) ";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Select folder path");
                txtFolder1.Focus();
            }
        }
        private void LoadFileTypeFilter()
        {
            lstFileType.Clear();
            if (radFilterType.IsChecked.Value)
            {
                BuildFileTypeList(lstFileType);
                if (chkother.IsChecked.Value)
                {
                    var lsttxtother = txtother.Text.Split(';');
                    foreach (var s in lsttxtother)
                    {
                        lstFileType.Add("." + s.ToUpper());
                    }
                }
            }
        }
        private void BuildFileTypeList(List<string> lstFileType)
        {
            if (chkDll.IsChecked.Value) lstFileType.Add(".DLL");
            if (chkexe.IsChecked.Value) lstFileType.Add(".EXE");
            if (chktxt.IsChecked.Value) lstFileType.Add(".TXT");
            if (chkttf.IsChecked.Value) lstFileType.Add(".TTF");
            if (chkocx.IsChecked.Value) lstFileType.Add(".OCX");
            if (chkZip.IsChecked.Value) lstFileType.Add(".ZIP");
            if (chkxslt.IsChecked.Value) lstFileType.Add(".XSLT");
            if (chkxml.IsChecked.Value) lstFileType.Add(".XML");
            if (chkdoc.IsChecked.Value) lstFileType.Add(".DOC");
        }
        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            lstSameResults.ItemsSource = null;
            lstDiffResults1.ItemsSource = null;
            lstDiffResults2.ItemsSource = null;
            Compare();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtFolder1.Text = "";
            txtFolder2.Text = "";
            lstSameResults.ItemsSource = null;
            lstDiffResults1.ItemsSource = null;
            lstDiffResults2.ItemsSource = null;
            txtHeader1.Text = "Only in Source";
            txtHeader.Text = "Common";
            txtHeader2.Text = "Only in Destination";
            radAllType.IsChecked = true;
        }

        private void btnSelect1_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = @"C:\" ;
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtFolder1.Text=dialog.SelectedPath.ToString();
            }
        }

        private void btnSelect2_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = @"C:\";
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtFolder2.Text = dialog.SelectedPath.ToString();
            }
        }

        private void radAllType_Checked(object sender, RoutedEventArgs e)
        {
            stkChk.Visibility = Visibility.Collapsed;
            stkChkther.Visibility = Visibility.Collapsed;
            lstFileType.Clear();
        }

        private void radFilterType_Checked(object sender, RoutedEventArgs e)
        {
            stkChk.Visibility = Visibility.Visible;
            stkChkther.Visibility = Visibility.Visible;
        }

        private void chkother_Click(object sender, RoutedEventArgs e)
        {
            if (chkother.IsChecked.Value)
            {
                txtother.Visibility = Visibility.Visible;
                tblMessage.Visibility = Visibility.Visible;
            }
            else
            {
                txtother.Visibility = Visibility.Collapsed;
                tblMessage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
