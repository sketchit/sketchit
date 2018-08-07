using SketchIt.Api.Interfaces;
using SketchIt.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace SketchIt
{
    static class Program
    {
        public static bool IsRunning { get; private set; }
        public static MainForm MainForm { get; private set; }
        public static BusyDialog BusyDialog { get; set; }
        public static SplashScreenForm SplashScreen { get; set; }
        public static ILibrary[] Libraries { get; private set; }

        [STAThread]
        static void Main()
        {
            IsRunning = true;

            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs e)
              {
                  return GetAssembly(e.Name);
              };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BusyDialog.Initialize();

            MainForm = new MainForm();

            Application.Run(MainForm);
        }

        public static SimpleParser Parser
        {
            get
            {
                return MainForm.Parser;
            }
        }

        public static void SetStatusMessage(string message) { SetStatusMessage(new StatusAction(message, Cursors.Default, false)); }
        public static void SetStatusMessage(StatusAction action)
        {
            if (MainForm != null)
            {
                MainForm.SetStatusMessage(action);
            }
        }

        public static void RemoveStatusMessage()
        {
            if (MainForm != null)
            {
                MainForm.RemoveStatusMessage();
            }
        }

        public static void InvokeMethod(Control control, Delegate method)
        {
            try
            {
                if (control != null && control.InvokeRequired)
                    control.Invoke(method);
                else
                    method.DynamicInvoke();
            }
            catch
            {
            }
        }

        public static ILibrary[] GetLibraries(bool refresh = false)
        {
            if (Libraries == null || refresh)
            {
                List<ILibrary> libraries = new List<ILibrary>();

                foreach (string filename in Directory.GetFiles(Application.StartupPath + "\\libraries", "*.dll", SearchOption.AllDirectories))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(filename);

                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.GetInterface("SketchIt.Api.Interfaces.ILibrary") != null)
                            {
                                ILibrary library = Activator.CreateInstance(type) as ILibrary;

                                if (library != null)
                                {
                                    libraries.Add(library);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                Libraries = libraries.ToArray();
            }

            return Libraries;
        }

        public static Assembly GetAssembly(string assemblyName)
        {
            foreach (ILibrary libary in GetLibraries())
            {
                Type type = libary.GetType();

                if (type.Assembly.FullName == assemblyName)
                {
                    return type.Assembly;
                }
            }

            return null;
        }

        public static string IsUpdateAvailable()
        {
            //a basic implementation to check for a new version. a proper automatic updater
            //must still be implemented.
            try
            {
                WebClient web = new WebClient();
                byte[] data = web.DownloadData("http://www.sketchit.org/downloads/latest.version.txt");
                string[] latest = System.Text.Encoding.Default.GetString(data).Split(new char[] { '.' });
                string[] current = Application.ProductVersion.Split(new char[] { '.' });

                for (int i = 0; i < latest.Length; i++)
                {
                    if (Convert.ToInt32(latest[i]) > Convert.ToInt32(current[i]))
                    {
                        return string.Join(".", latest);
                    }
                    else if (Convert.ToInt32(latest[i]) < Convert.ToInt32(current[i]))
                    {
                        return null;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
