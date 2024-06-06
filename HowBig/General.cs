using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace HowBig
{
    public class General
    {
        // *********************************************************
        public static bool GetFileName(out string filename)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Solution (*.sln)|*.sln|All files (*.*)|*.*";

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                return true;
            }
            else
            {
                filename = "";
                return false;
            }
        }

        // *********************************************************
        public static bool GetFolderName(out string folderename)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();

            openFolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFolderDialog.ShowDialog() == true)
            {
                folderename = openFolderDialog.FolderName;
                return true;
            }
            else
            {
                folderename = "";
                return false;
            }
        }

        // *********************************************************
        public static List<String> GetProjectNames(string path)
        {
            List<String> output = new List<String>();

            String line;
            try
            {
                StreamReader sr = new StreamReader(path);
                line = sr.ReadLine();

                while (line != null)
                {
                    if (line.Length >= 7)
                    {
                        if (line.Substring(0, 7).ToUpper() == "PROJECT")
                        {
                            string[] atemp = line.Split(",");
                            int len = atemp[1].Length - 3;
                            string temp = atemp[1].Trim().Substring(1, len);
                            output.Add(temp);
                        }
                    }

                    line = sr.ReadLine();
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return output;
        }

        // *********************************************************
        public static String[] GetFileNames(string path, string type, bool folder)
        {
            String[] output;

            if (folder == true)
            {
                output = Directory.GetFiles(path, type, SearchOption.AllDirectories);
            }
            else
            {
                output = Directory.GetFiles(System.IO.Path.GetDirectoryName(path), type, SearchOption.AllDirectories);
            }

            return output;
        }

        // *********************************************************
        public static (int, int, int) CountLines(string fileName, string sigleComment, string multiStart, string multiEnd)
        {
            int lc = 0;
            int bks = 0;
            int Cmts = 0;

            List<string> lines = new List<string>();
            String line;
            try
            {
                StreamReader sr = new StreamReader(fileName);
                line = sr.ReadLine();
                lines.Add(line);

                while (line != null)
                {
                    lines.Add(line);

                    line = sr.ReadLine();
                }

                sr.Close();

                lc = lines.Count;
                bks = CountBlanks(lines);
                Cmts = CountComments(lines, sigleComment, multiStart, multiEnd);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return (lc - 1, bks, Cmts);
        }

        // *********************************************************
        private static int CountBlanks(List<string> lines)
        {
            int output = 0;

            foreach (string line in lines)
            {
                string l = line.Trim();

                if (l.Length == 0)
                {
                    output++;
                }
            }

            return output;
        }

        // *********************************************************
        public static int CountComments(List<string> lines, string sigleComment, string multiStart, string multiEnd)
        {
            int output = 0;
            bool ml = false;

            foreach (string line in lines)
            {
                string l = line.Trim();

                if (l.Length >= sigleComment.Length)
                {
                    if (l.Substring(0, sigleComment.Length) == sigleComment)
                    {
                        output++;
                    }
                }

                if (l.Length >= multiStart.Length && ml == false)
                {
                    if (l.Substring(0, multiStart.Length) == multiStart)
                    {
                        ml = true;
                    }
                }

                if (ml == true)
                {
                    output++;
                }

                if (l.Length >= multiEnd.Length && ml == true)
                {
                    if (l.IndexOf(multiEnd) > 0)
                    {
                        ml = false;
                    }
                }
            }

            return output;
        }

        // *********************************************************
        public static List<string> BuildFileList(List<string> projects, string codeType, bool folder)
        {
            List<string> allFiles = new List<string>();
            List<string> files = new List<string>();

            foreach (var p in projects)
            {
                // Get the files in the project folder
                allFiles = General.GetFileNames(p, codeType, folder).ToList();

                foreach (var f in allFiles)
                {
                    bool exclude = false;

                    if ((f.ToUpper().IndexOf("DEBUG") > 0))
                    {
                        exclude = true;
                    }

                    if ((f.ToUpper().IndexOf("ASSEMBLY") > 0))
                    {
                        exclude = true;
                    }

                    if (exclude == false)
                    {
                        files.Add(f);
                    }
                }
            }

            return files;
        }
    }
}