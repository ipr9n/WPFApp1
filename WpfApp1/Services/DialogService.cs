using System;
using Microsoft.Win32;

namespace WpfApp1.Services
{
    public class DialogService
    {
        public enum TypeIndex
        {
            Json = 1,
            XML,
            CSV
        }
        public string FilePath { get; set; }

        public bool SaveFileDialog(out TypeIndex ind)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON File (*.json)|*.json|XML File (*.xml)|*.xml|CSV File (*.csv)|*csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                ind = (TypeIndex)saveFileDialog.FilterIndex;
                return true;
            }

            ind = TypeIndex.Json;
            return false;
        }
    }
}
