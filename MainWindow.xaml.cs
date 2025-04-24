using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;


namespace DES_File_Protector {
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }
        // اختيار ملف
        private void openADialog(object sender, RoutedEventArgs e) {
            // creating a file dialog to choose a file
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result.HasValue && result == true) {
                // Open document 
                string filename = dlg.FileName;
                FilesListBox.Items.Add(filename);
            }

        }
        // اختيار مكان التحزين
        private void openFileButton_Click(object sender, RoutedEventArgs e) {
            // creating a file dialog to choose a file
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog()) {
                System.Windows.Forms.DialogResult res = dlg.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK) {
                    exportLocationField.Text = dlg.SelectedPath;
                }
            }

        }
        // random password button logic
        private void randomPassButton_Click(object sender, RoutedEventArgs e) {
            passwordField.Password = DESalgorithm.GenerateRandomKey(12);
        }
        // encrypt button logic
        private void EncryptButton_Click(object sender, RoutedEventArgs e) {

            try {
                // اذا الفايل ماموجود
                if (!Directory.Exists(exportLocationField.Text)) {
                    MessageBox.Show("error: export directory not found");
                }
                // اذا الزر مقعل يخزنة بداخل فولد اسمه انكربت
                else if((bool)saveDir.IsChecked){
                    if (!Directory.Exists(exportLocationField.Text + "\\Encrypted")) {
                        Directory.CreateDirectory(exportLocationField.Text + "\\Encrypted");
                    }
                    // يفتر على الملفات واحد واحد
                    foreach (string location in FilesListBox.Items) {
                        string fileName = new FileInfo(location).Name;
                        // خوازمية التشفير هنا
                        DESalgorithm.EncryptFile(location, exportLocationField.Text + "\\Encrypted\\" + fileName + ".enc", passwordField.Password);
                    }
                    MessageBox.Show("Encryption Completed Successfully", "Operation Completed");
                }
                // اذا مامفعل يخزنة بدون مايسوي فولد جديد يعني بنفس الماكن الي انت حددته
                else {
                    foreach (string location in FilesListBox.Items) {
                        string fileName = new FileInfo(location).Name;
                        DESalgorithm.EncryptFile(location, exportLocationField.Text + "\\" + fileName + ".enc", passwordField.Password);
                    }
                    MessageBox.Show("Encryption Completed Successfully", "Operation Completed");
                }
            }catch(Exception ex) {
                MessageBox.Show("Error during encryption: " + ex.Message);
            }
        }
        // decrypt button logic
        private void DecryptButton_Click(object sender, RoutedEventArgs e) {
            try { 
            // نفس الفكرة هنا بس لفك التشفير
            if (!Directory.Exists(exportLocationField.Text)) {
                MessageBox.Show("error: export directory not found");
            }
            else if ((bool)saveDir.IsChecked) {
                if (!Directory.Exists(exportLocationField.Text + "\\Decrypted")) {
                    Directory.CreateDirectory(exportLocationField.Text + "\\Decrypted");
                }
                foreach (string location in FilesListBox.Items) {
                    string fileName = new FileInfo(location).Name;
                    // حذف كلمة .enc 
                    fileName = fileName.Substring(0, fileName.Length - 4);
                    // خوازمية فك التشفير هنا
                    DESalgorithm.DecryptFile(location, exportLocationField.Text + "\\Decrypted\\" + fileName, passwordField.Password);
                    //DESalgorithm.Decrypt(location, exportLocationField.Text + "\\Decrypt\\" + fileName, passwordField.Password, iv_bytes);
                }
                MessageBox.Show("Decryption Completed Successfully","Operation Completed");
            }
            else {
                foreach (string location in FilesListBox.Items) {
                    string fileName = new FileInfo(location).Name;
                    fileName = fileName.Substring(0, fileName.Length - 4);
                    DESalgorithm.DecryptFile(location, exportLocationField.Text + fileName, passwordField.Password);
                }
                MessageBox.Show("Decryption Completed Successfully", "Operation Completed");
            }
            }
            catch (Exception ex) {
                MessageBox.Show("Error during decryption: " + ex.Message);
            }
        }

        // clearing the items
        private void clearButton_Click(object sender, RoutedEventArgs e) {
            FilesListBox.Items.Clear();
        }
    }
}
