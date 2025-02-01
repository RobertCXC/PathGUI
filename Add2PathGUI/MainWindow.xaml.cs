using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Add2PathGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtFolderPath_PreviewDragOver(object sender, DragEventArgs e)
        {
            // 允许拖拽
            e.Handled = true;
            e.Effects = DragDropEffects.Copy;
        }

        private void txtFolderPath_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && Directory.Exists(files[0])) // 只处理文件夹
                {
                    FolderPathTextBox.Text = files[0]; // 显示文件夹路径
                }
            }
        }


        private void AddPathButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = FolderPathTextBox.Text;
            if (Directory.Exists(folderPath))
            {
                if (UserEnvironmentCheckBox.IsChecked == true)
                {
                    // 修改用户环境变量（普通权限可用）
                    string currentUserPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? "";
                    if (!currentUserPath.Split(';').Contains(folderPath))
                    {
                        Environment.SetEnvironmentVariable("Path", currentUserPath + ";" + folderPath, EnvironmentVariableTarget.User);
                        MessageBox.Show("已将路径添加到用户环境变量。");
                    }
                    else
                    {
                        MessageBox.Show("该路径已在用户环境变量中。");
                    }
                }

                if (SystemEnvironmentCheckBox.IsChecked == true)
                {
                    // 调用管理员子进程来修改系统环境变量
                    RunAsAdmin(folderPath);
                }
            }
            else
            {
                MessageBox.Show("指定的文件夹不存在！");
            }
        }
        private void RunAsAdmin(string folderPath)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ConsoleApp2.exe", // 你的子进程程序
                    Arguments = $"\"{folderPath}\"", // 传递参数
                    Verb = "runas", // 以管理员身份运行
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改系统环境变量失败：" + ex.Message);
            }
        }
    }
}
