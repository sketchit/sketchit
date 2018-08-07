using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SketchIt.Api.Internal;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SketchIt.Utilities
{
    public class Settings
    {
        public static Settings User { get; private set; } = new Settings(true);
        public static Settings Global { get; private set; } = new Settings(false);

        public static string GetUserFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SketchIt";
        }

        private bool _user = false;
        private JObject _jsonData = null;
        private FileSystemWatcher _settingsFileWatcher;
        private bool _saving = false;

        private Settings(bool user)
        {
            _user = user;
        }

        private string GetFilePath()
        {
            string folder = GetUserFolder();

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (_settingsFileWatcher == null)
            {
                //string folder = GetUserFolder();
                _settingsFileWatcher = new FileSystemWatcher(folder, "settings.json");
                _settingsFileWatcher.Changed += SettingsFileChanged;
                _settingsFileWatcher.EnableRaisingEvents = true;
            }

            return folder + "\\settings.json";
        }

        public void EnableFileWatcher()
        {
        }

        private void SettingsFileChanged(object sender, FileSystemEventArgs e)
        {
            if (_saving)
            {
                return;
            }

            _settingsFileWatcher.EnableRaisingEvents = false;
            _jsonData = null;

            AppearanceSettings.Update();

            using (ThreadLocker locker = ThreadLocker.Lock(Application.OpenForms))
            {
                if (locker.IsLocked)
                {
                    int i = 0;

                    while (i < Application.OpenForms.Count)
                    {
                        if (Application.OpenForms[i] is BaseForm form)
                        {
                            MethodInvoker mi = new MethodInvoker(((BaseForm)form).UpdateAppearance);
                            form.Invoke(mi);
                        }

                        i++;
                    }
                }
            }

            _settingsFileWatcher.EnableRaisingEvents = true;
        }

        private void Save()
        {
            try
            {
                _saving = true;

                using (TextWriter writer = new StreamWriter(GetFilePath()))
                {
                    JsonTextWriter jsonWriter = new JsonTextWriter(writer);

                    jsonWriter.Formatting = Formatting.Indented;
                    _jsonData.WriteTo(jsonWriter, new JsonConverter[] { });
                    jsonWriter.Close();
                    writer.Close();
                }
            }
            finally
            {
                _saving = false;
            }
        }

        private void CheckJson()
        {
            if (_jsonData == null)
            {
                string path = GetFilePath();

                if (File.Exists(path))
                {
                    try
                    {
                        using (TextReader reader = new StreamReader(path))
                        {
                            string jsonString = reader.ReadToEnd();
                            _jsonData = (JsonConvert.DeserializeObject(jsonString) ?? new JObject()) as JObject;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(10);
                        CheckJson();
                    }
                }
                else
                {
                    _jsonData = new JObject();
                }
            }
        }

        private object GetSetting(string name, object defaultValue)
        {
            CheckJson();

            object result = defaultValue;
            JToken value = _jsonData.GetValue(name, StringComparison.OrdinalIgnoreCase);

            if (value != null)
            {
                result = ((JValue)value).Value;
            }

            return result;
        }

        public string GetString(string name, string defaultValue)
        {
            return GetSetting(name, defaultValue ?? "").ToString();
        }

        public int GetInt(string name, int defaultValue)
        {
            return Convert.ToInt32(GetSetting(name, defaultValue));
        }

        public bool GetBool(string name, bool defaultValue)
        {
            return Convert.ToBoolean(GetSetting(name, defaultValue));
        }

        public Color GetColor(string name, Color defaultValue)
        {
            object value = GetSetting(name, defaultValue.ToArgb()) ?? "";
            int valueInt;

            if (value is int)
            {
                return Color.FromArgb((int)value);
            }

            if (int.TryParse(value.ToString(), out valueInt))
            {
                return Color.FromArgb(valueInt);
            }

            if (value.ToString().StartsWith("0x"))
            {
                string hex = value.ToString().Substring(2);

                if (hex.Length == 6)
                {
                    hex = "FF" + hex;
                }

                if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out valueInt))
                {
                    Color color = Color.FromArgb(valueInt);
                    return color;
                }
            }

            return Color.Empty;
        }

        public object this[string name]
        {
            get
            {
                return GetSetting(name, null);
            }

            set
            {
                CheckJson();
                _jsonData[name] = new JValue(value);
                Save();
            }
        }
    }
}
