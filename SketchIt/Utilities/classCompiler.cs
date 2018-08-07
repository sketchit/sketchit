using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using SketchIt.Api.Interfaces;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchIt.Utilities
{
    public class Compiler
    {
        private class SourceCode
        {
            public string Using { get; private set; }
            public string Code { get; private set; }
            public int CodeLineCount { get; private set; }
            public int UsingLineCount { get; private set; }

            public SourceCode(string input)
            {
                StringBuilder usingText = new StringBuilder();
                StringBuilder sourceText = new StringBuilder();
                bool sourceEmpty = true;
                int temp = 0;

                foreach (string line in input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    if (line.Trim().Length == 0 && sourceEmpty)
                    {
                        temp++;
                        continue;
                    }

                    if ((line.StartsWith("using ") && sourceEmpty))
                    {
                        for (int i = 0; i < temp; i++)
                        {
                            usingText.AppendLine("");
                        }

                        usingText.AppendLine(line);
                        UsingLineCount += temp + 1;
                    }
                    else
                    {
                        if (line.Trim().Length > 0 && sourceEmpty)
                        {
                            sourceEmpty = false;

                            for (int i = 0; i < temp; i++)
                            {
                                sourceText.AppendLine("");
                            }
                        }

                        sourceText.AppendLine(line);
                        CodeLineCount += temp + 1;
                    }

                    temp = 0;
                }

                Using = usingText.ToString();
                Code = sourceText.ToString();
            }
        }

        public Compiler()
            : this(false)
        {
        }

        public Compiler(bool isBackgroundCompiler)
        {
            IsBackgroundCompiler = isBackgroundCompiler;
        }

        public Exception Exception
        {
            get;
            private set;
        }

        public bool IsBackgroundCompiler
        {
            get;
            private set;
        }

        public Assembly Output
        {
            get;
            private set;
        }

        public CompilerErrorCollection CompilerErrors
        {
            get;
            private set;
        }

        public Process LaunchOutput()
        {
            ProcessStartInfo psi = new ProcessStartInfo(Output.CodeBase);
            return Process.Start(psi);
        }

        private SourceCode[] GetCodeFiles()
        {
            List<SourceCode> sourceCode = new List<SourceCode>();

            foreach (BaseForm form in Application.OpenForms)
            {
                if (form == null) continue;
                if (form.Type != WindowType.SourceFile) continue;

                sourceCode.Add(new SourceCode(((EditorForm)form).EditorText ?? ""));
            }

            return sourceCode.ToArray();
        }

        private string[] GetAssemblyList(bool fullPath = false)
        {
            Uri appUri = new Uri(Application.StartupPath + "\\");
            List<string> list = new List<string>(new string[]
            {
                (fullPath ? Application.StartupPath + "\\" : "") + "SketchIt.Api.dll",
                (fullPath ? Application.StartupPath + "\\" : "") + "SketchIt.Windows.dll",
                (fullPath ? Application.StartupPath + "\\" : "") + "SharpGL.dll"
            });

            foreach (ILibrary library in Program.GetLibraries())
            {
                Assembly assembly = library.GetType().Assembly;

                if (!fullPath)
                {
                    Uri assemblyUri = new Uri(assembly.Location);
                    list.Add(Uri.UnescapeDataString(appUri.MakeRelativeUri(assemblyUri).ToString().Replace("/", "\\")));
                }
                else if (library.Embeddable)
                {
                    FileInfo fileInfo = new FileInfo(assembly.Location);

                    list.Add(assembly.Location);

                    foreach (string dependancy in library.EmbeddableDependancies)
                    {
                        list.Add(fileInfo.DirectoryName + "\\" + dependancy);
                    }
                }
            }

            return list.ToArray();
        }

        public bool Compile(string startup = null)
        {
            Status status = IsBackgroundCompiler ? null : Status.Set("Compiling...");

            CompilerErrors = null;
            Output = null;
            Exception = null;

            try
            {
                if (string.IsNullOrEmpty(startup))
                {
                    startup = Properties.Resources.AppStartup;
                }

                string outputFolder = Settings.GetUserFolder() + "\\temp";
                SourceCode[] sourceCode = GetCodeFiles();
                List<string> sourceFiles = new List<string>();
                bool isStatic = Program.Parser.IsStatic();

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                sourceFiles.Add(startup);

                int usingOffset = Properties.Resources.AppTemplate.IndexOf("//#using-place-holder#");
                int codeOffset = Properties.Resources.AppTemplate.IndexOf("//#code-place-holder#");

                usingOffset = Properties.Resources.AppTemplate.Substring(0, usingOffset).Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length - 1;
                codeOffset = Properties.Resources.AppTemplate.Substring(0, codeOffset).Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Length - 1;

                foreach (SourceCode sc in sourceCode)
                {
                    sourceFiles.Add(Properties.Resources.AppTemplate
                        .Replace("//#using-place-holder#", sc.Using)
                        .Replace("//#code-place-holder#", (isStatic ? "void RunStatic() {" : "") + sc.Code + (isStatic ? "\r\n}" : ""))
                        );
                }

                List<string> assemblyList = new List<string>(new string[] {
                    "System.dll",
                    "System.Windows.Forms.dll",
                    "System.Drawing.dll"
                });

                assemblyList.AddRange(GetAssemblyList());

                Dictionary<string, string> options = new Dictionary<string, string>();
                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters(assemblyList.ToArray())
                {
                    GenerateInMemory = IsBackgroundCompiler,
                    GenerateExecutable = !IsBackgroundCompiler,
                    OutputAssembly = outputFolder + (IsBackgroundCompiler ? "\\sketch.tmp" : "\\sketch.exe"),
                    CompilerOptions = "/target:winexe /optimize /win32icon:\"" + Application.StartupPath + "\\SketchIt.ico\"",
                };

                int retries = 0;
                while (File.Exists(parameters.OutputAssembly) && retries < 10)
                {
                    try
                    {
                        File.Delete(parameters.OutputAssembly);
                    }
                    catch
                    {
                        retries++;
                    }
                }

                if (!IsBackgroundCompiler)
                {
                    if (!File.Exists(Application.StartupPath + "\\SketchIt.ico"))
                    {
                        using (FileStream stream = new FileStream(Application.StartupPath + "\\SketchIt.ico", FileMode.Create))
                            Program.MainForm.Icon.Save(stream);
                    }

                    parameters.EmbeddedResources.Add(Application.StartupPath + "\\SketchIt.ico");

                    foreach (string filename in GetAssemblyList(true))
                    {
                        parameters.EmbeddedResources.Add(filename);
                    }

                    foreach (ILibrary library in Program.Libraries)
                    {
                        FileInfo libraryFile = new FileInfo(library.GetType().Assembly.Location);
                        List<string> dependancies = new List<string>(library.AdditionalDependancies);

                        if (!library.Embeddable)
                        {
                            dependancies.Add(libraryFile.Name);
                        }

                        foreach (string dependancy in dependancies)
                        {
                            FileInfo destinationFile = new FileInfo(outputFolder + "\\" + dependancy);
                            string sourceFile = libraryFile.DirectoryName + "\\" + dependancy;

                            if (!Directory.Exists(destinationFile.DirectoryName))
                            {
                                Directory.CreateDirectory(destinationFile.DirectoryName);
                            }

                            if (File.Exists(destinationFile.FullName))
                            {
                                try
                                {
                                    File.Delete(destinationFile.FullName);
                                }
                                catch
                                {
                                }
                            }

                            if (!File.Exists(destinationFile.FullName))
                            {
                                File.Copy(sourceFile, destinationFile.FullName);
                            }
                        }
                    }
                }

                CompilerResults results = provider.CompileAssemblyFromSource(parameters, sourceFiles.ToArray());

                if (results.Errors.Count == 0)
                {
                    Output = results.CompiledAssembly;
                }
                else
                {
                    foreach (CompilerError error in results.Errors)
                    {
                        try
                        {
                            FileInfo fileInfo = string.IsNullOrEmpty(error.FileName) ? null : new FileInfo(Path.GetFileNameWithoutExtension(error.FileName));
                            int fileIndex = fileInfo != null ? int.Parse(fileInfo.Extension.Substring(1)) - 1 : -1;

                            if (fileIndex < 0)
                            {
                            }
                            else if (error.Line < codeOffset + sourceCode[fileIndex].UsingLineCount)
                            {
                                error.Line -= usingOffset;
                            }
                            else if (error.Line - codeOffset > sourceCode[fileIndex].CodeLineCount)
                            {
                                error.Line = sourceCode[fileIndex].CodeLineCount;
                            }
                            else
                            {
                                error.Line -= codeOffset;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    CompilerErrors = results.Errors;
                }
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            finally
            {
                if (status != null)
                {
                    status.Dispose();
                }
            }

            return Output != null;
        }
    }
}
