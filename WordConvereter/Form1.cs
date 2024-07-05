using System;
using System.IO;
using System.Windows.Forms;
using Aspose.Words;
using Aspose.Words.Saving;

namespace WordToPdfConverter
{
    public partial class Form1 : Form
    {
        private string selectedFilePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Word files (*.docx)|*.docx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;
                    lblFileName.Text = "Selected file: " + Path.GetFileName(selectedFilePath);
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a Word file first.");
                return;
            }

            try
            {
                ConvertWordToSeparatePdfPages(selectedFilePath);
                MessageBox.Show("Conversion successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void ConvertWordToSeparatePdfPages(string filePath)
        {
            Document doc = new Document(filePath);
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            Directory.CreateDirectory(directoryPath);

            PdfSaveOptions saveOptions = new PdfSaveOptions();

            for (int pageIndex = 0; pageIndex < doc.PageCount; pageIndex++)
            {
                Document pageDoc = doc.ExtractPages(pageIndex, 1);
                string outputFilePath = Path.Combine(directoryPath, $"Page_{pageIndex + 1}.pdf");
                pageDoc.Save(outputFilePath, saveOptions);
            }
        }
    }
}
