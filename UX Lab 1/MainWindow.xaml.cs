using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Documents;
using System.Windows.Media;

namespace UX_Lab_1
{
    public partial class MainWindow : RibbonWindow
    {
        public bool isSaved = false;

        public MainWindow()
        {
            InitializeComponent();
            _fontSize.ItemsSource = FontSizes;
        }

        public double[] FontSizes
        {
            get
            {
                return new double[] { 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 40, 52, 64, 76, 100 };
            }
        }

        private void btnTextColor_Click(object sender, RoutedEventArgs e)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                doc1.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
            }
        }


        private void btnBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            var colorDialog = new System.Windows.Forms.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                doc1.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(System.Windows.Media.Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B)));
            }
        }


        private void btnFindText_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchTextBox.Text;
            TextRange textRange = new TextRange(doc1.Document.ContentStart, doc1.Document.ContentEnd);
            int index = textRange.Text.IndexOf(searchText);

            if (index != -1)
            {
                TextPointer start = doc1.Document.ContentStart.GetPositionAtOffset(index);
                TextPointer end = start.GetPositionAtOffset(searchText.Length);
                doc1.Selection.Select(start, end);
                doc1.Focus();
            }
            else
            {}
        }


        private void btnNumberedList_Click(object sender, RoutedEventArgs e)
        {
            Paragraph paragraph = new Paragraph();
            doc1.Document.Blocks.Add(paragraph);
        }


        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Document files (*.rtf)|*.rtf";
            var result = dlg.ShowDialog();
            if (result == true)
            {
                TextRange t = new TextRange(doc1.Document.ContentStart, doc1.Document.ContentEnd);
                FileStream file = new FileStream(dlg.FileName, FileMode.Open);
                t.Load(file, System.Windows.DataFormats.Rtf);
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.SaveFileDialog savefile = new Microsoft.Win32.SaveFileDialog();
            savefile.Filter = "Document files (*.rtf)|*.rtf";

            if (savefile.ShowDialog() == true)
            {
                TextRange t = new TextRange(doc1.Document.ContentStart, doc1.Document.ContentEnd);
                FileStream file = new FileStream(savefile.FileName, FileMode.Create);
                t.Save(file, System.Windows.DataFormats.Rtf);
                file.Close();
            }

            isSaved = true;
        }


        private void FontFamil1_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                System.Windows.Media.FontFamily editValue = (System.Windows.Media.FontFamily)e.AddedItems[0];
                ApplyPropertyValueToSelectedText(TextElement.FontFamilyProperty, editValue);
            }
            catch { }
        }


        private void FontSize_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ApplyPropertyValueToSelectedText(TextElement.FontSizeProperty, e.AddedItems[0]);
            }
            catch { }
        }


        private void ApplyPropertyValueToSelectedText(DependencyProperty formattingProperty, object value)
        {
            if (value != null)
            {
                doc1.Selection.ApplyPropertyValue(formattingProperty, value);
            }
        }


        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            doc1.Document.Blocks.Clear();
        }


        private void btnIncreaseIndent_Click(object sender, RoutedEventArgs e)
        {
            TextSelection selection = doc1.Selection;

            if (!selection.IsEmpty)
            {
                var currentRightIndent = selection.GetPropertyValue(Paragraph.MarginProperty);

                Thickness margin;

                if (currentRightIndent != DependencyProperty.UnsetValue)
                {
                    margin = (Thickness)currentRightIndent;
                }
                else
                {
                    margin = new Thickness(0);
                }

                margin.Right += 20;

                selection.ApplyPropertyValue(Paragraph.MarginProperty, margin);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                IDocumentPaginatorSource document = doc1.Document;
                printDialog.PrintDocument(document.DocumentPaginator, "Printing Document");
            }
        }
    }
}
