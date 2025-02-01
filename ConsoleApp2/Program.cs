using System;
using System.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("错误：未提供路径参数。");
                return;
            }

            string folderPath = args[0];

            try
            {
                string currentSystemPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) ?? "";
                if (!currentSystemPath.Split(';').Contains(folderPath))
                {
                    Environment.SetEnvironmentVariable("Path", currentSystemPath + ";" + folderPath, EnvironmentVariableTarget.Machine);
                    Console.WriteLine("成功将路径添加到系统环境变量。");
                }
                else
                {
                    Console.WriteLine("该路径已在系统环境变量中。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("修改系统环境变量失败：" + ex.Message);
            }
        }
    }
}
