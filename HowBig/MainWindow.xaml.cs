using System.Windows;
using System.Xml.Linq;

/*
* Title:    HowBig
* Author:   Kelvin Clements
* Date:     28-May-2024
* Purpose:  Display amount of code in a project
*
* LOG ------------------------------------------------------
* DD/MMM/YYYY   Comments ....................................
* ----------------------------------------------------------
* 28/May/2024 - Project started
*
*/

namespace HowBig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Global Vvariables
        private List<string> projects = new List<string>();

        private List<string> files = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            lblPrjName.Content = "";
            lblFiles.Content = 0;
            lblLines.Content = 0;
            lblBlanks.Content = 0;
            lblComments.Content = 0;
            lblCode.Content = 0;
        }

        private void btnPick1_Click(object sender, RoutedEventArgs e)
        {
            bool worked = General.GetFileName(out string SolutionName);

            if (worked)
            {
                tbProject.Text = SolutionName;
                lblPrjName.Content = System.IO.Path.GetFileNameWithoutExtension(tbProject.Text);
                tbFolder.Text = string.Empty;
            }
            else
            {
                tbProject.Text = string.Empty;
                lblPrjName.Content = "";
            }
        }

        private void btnPick2_Click(object sender, RoutedEventArgs e)
        {
            bool worked = General.GetFolderName(out string FolderName);

            if (worked)
            {
                tbFolder.Text = FolderName;
                lblPrjName.Content = "==========";
                tbProject.Text = string.Empty;
            }
            else
            {
                tbFolder.Text = string.Empty;
                lblPrjName.Content = "";
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            //txbResults.Text = string.Empty;
            List<FileTable> query = new List<FileTable>();

            projects.Clear();
            files.Clear();
            bool folder = false;

            if (tbProject.Text != string.Empty || tbFolder.Text != string.Empty)
            {
                if (tbProject.Text != string.Empty)
                {
                    // Find the Package names
                    projects = General.GetProjectNames(tbProject.Text);
                    
                    folder = false;

                    for (int i = 0; i < projects.Count; i++)
                    {
                        projects[i] = System.IO.Path.GetDirectoryName(tbProject.Text) + "\\" + projects[i];
                    }
                }


                if (tbFolder.Text != string.Empty)
                {
                    projects.Add( tbFolder.Text);
                    folder = true;
                }

                // H Code
                files.AddRange(General.BuildFileList(projects, "*.h", folder));
                // C Code
                files.AddRange(General.BuildFileList(projects, "*.c", folder));
                // C++ Code
                files.AddRange(General.BuildFileList(projects, "*.cpp", folder));
                // C Shape Code
                files.AddRange(General.BuildFileList(projects, "*.cs", folder));
                // F Shape Code
                files.AddRange(General.BuildFileList(projects, "*.fs", folder));
                // Java Code
                files.AddRange(General.BuildFileList(projects, "*.java", folder));
                // Java Script Code
                files.AddRange(General.BuildFileList(projects, "*.js", folder));
                // Python Code
                files.AddRange(General.BuildFileList(projects, "*.py", folder));
                // XAML Code
                files.AddRange(General.BuildFileList(projects, "*.xaml", folder));

                int totLines = 0;       // Total Lines  in Project
                int totBlanks = 0;      // Total Lines of Blnk lines in Project
                int totComments = 0;    // Total Lines of Comments lines in Project
                int totCode = 0;        // Total Lines of Code lines in Project

                foreach (var f in files)
                {
                    int lineCount = 0;  // Count of the number of lines of code
                    int blanks = 0;     // Count of the number of Blank lines
                    int comments = 0;   // Count of the number of lines of comments and multi-lines comments

                    switch (System.IO.Path.GetExtension(f))
                    {
                        case ".h":
                            (lineCount, blanks, comments) = General.CountLines(f, "//", "/*", "*/");
                            break;

                        case ".c":
                            (lineCount, blanks, comments) = General.CountLines(f, "//", "/*", "*/");
                            break;

                        case ".cpp":
                            (lineCount, blanks, comments) = General.CountLines(f, "//", "/*", "*/");
                            break;

                        case ".cs":
                            (lineCount, blanks, comments) = General.CountLines(f, "//", "/*", "*/");
                            break;

                        case ".fs":
                            (lineCount, blanks, comments) = General.CountLines(f, "//", "/*", "*/");
                            break;

                        case ".java":
                            (lineCount, blanks, comments) = General.CountLines(f, "//", "/*", "*/");
                            break;

                        case ".js":
                            (lineCount, blanks, comments) = General.CountLines(f, "//", "/*", "*/");
                            break;

                        case ".py":
                            (lineCount, blanks, comments) = General.CountLines(f, "#", "'''", "'''");
                            break;

                        case ".xaml":
                            (lineCount, blanks, comments) = General.CountLines(f, "--", "<!--", "-->");
                            break;

                        default:
                            break;
                    }

                    int code = lineCount - blanks - comments;

                    //string text = $"{System.IO.Path.GetFileName(f)} - Lines: {lineCount}\t Blanks: {blanks}\t Comments: {comments}\t Code: {code}";

                    totLines += lineCount;
                    totBlanks += blanks;
                    totComments += comments;
                    totCode += code;

                    //txbResults.Text = txbResults.Text + "\n" + text;
                    FileTable fdt = new FileTable();

                    fdt.BFileName = System.IO.Path.GetFileName(f);
                    fdt.BLines = lineCount.ToString();
                    fdt.BBlanks = blanks.ToString();
                    fdt.BComments = comments.ToString();
                    fdt.BCode = code.ToString();

                    query.Add(fdt);

                }

                lblFiles.Content = files.Count;
                lblLines.Content = totLines;
                lblBlanks.Content = totBlanks;
                lblComments.Content = totComments;
                lblCode.Content = totCode;

                dgProjectDetails.ItemsSource = query;
            }
        }
    }
}