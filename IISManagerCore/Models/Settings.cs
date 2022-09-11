using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IISManagerCore.Models
{
    public class Settings
    {
        // Settings doesn't use inheritence for its values (e.g. a plugin would define its own classes derived from Setting)
        // as it makes serialization a pain (every plugin would have to have its own over-ridden serialize method).
        // It's also a lot easier to use like this, and more error tolerant.
        private List<SettingValue> _values;

        /// <summary>
        /// The id of the plugin that the setting belongs to, used primarily for readability in the JSON.
        /// 设置所属插件的 ID，主要用于 JSON 中的可读性。
        /// </summary>
        public string PluginId { get; set; }

        /// <summary>
        /// The version of the plugin that the setting belongs to, used primarily for readability in the JSON.
        /// 设置所属的插件版本，主要用于 JSON 中的可读性。
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Whether the plugin is enabled or not.
        /// 插件是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets all the setting values.
        /// 获取所有设置值。
        /// </summary>
        public IEnumerable<SettingValue> Values
        {
            get
            {
                return _values;
            }
        }

        /// <summary>
        /// 初始化类的新实例
        /// </summary>
        /// <param name="pluginId">插件id，仅供参考</param>
        /// <param name="version">设置的插件版本，仅供参考</param>
        public Settings(string pluginId, string version)
        {
            PluginId = pluginId;
            Version = version;
            _values = new List<SettingValue>();
        }

        /// <summary>
        /// 设定Setting值
        /// </summary>
        /// <param name="name">设置的名称</param>
        /// <param name="value">设置的值</param>
        /// <param name="formType">应用于表示值的 UI 类型（当前未实现）</param>
        public void SetValue(string name, string value, SettingFormType formType = SettingFormType.Textbox)
        {
            SettingValue settingValue = _values.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) &&
                                                                    x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (settingValue == null)
            {
                settingValue = new SettingValue();
                settingValue.Name = name;
                _values.Add(settingValue);
            }

            settingValue.Value = value;
            settingValue.FormType = formType;
        }

        /// <summary>
        /// Retrieves the setting value from this instance's current values.
        /// 从此实例的当前值中检索设置值
        /// </summary>
        /// <param name="name">设置的名称</param>
        /// <returns>设置的值，如果未找到设置名称，则为空字符串</returns>
        public string GetValue(string name)
        {
            SettingValue settingValue = _values.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (settingValue != null)
                return settingValue.Value;

            return "";
        }

        /// <summary>
        /// 序列化此实例并将其作为 JSON 字符串返回
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// 从提供的 JSON 创建一个新实例
        /// </summary>
        /// <param name="json">json</param>
        /// <returns>一个新实例，或者如果 JSON 无法反序列化，一个具有虚拟（随机）插件 ID 和版本的实例，以及记录到日志中的错误。</returns>
        public static Settings LoadFromJson(string json)
        {
            //SiteSettings.LoadFromJson 的精确副本
            if (string.IsNullOrEmpty(json))
            {
                //Log.Warn("PluginSettings.LoadFromJson - json 字符串为空（返回默认设置对象）");
                return new Settings("error - dummy id: " + Guid.NewGuid(), "1.0");
            }

            try
            {
                return JsonConvert.DeserializeObject<Settings>(json);
            }
            catch (JsonReaderException ex)
            {
                //Log.Error(ex, "Settings.LoadFromJson - 反序列化 JSON 时发生异常");
                return new Settings("error - dummy id:" + Guid.NewGuid(), "1.0");
            }
        }

        #region 新逻辑

        private static string _rootFolder;
        private static string _libFolder;
        private static string _webPath;
        private static string _packagesFolder;

        public static string ROOT_FOLDER
        {
            get
            {
                if (string.IsNullOrEmpty(_rootFolder))
                {
                    string relativePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..");

                    _rootFolder = new DirectoryInfo(relativePath).FullName;
                    Console.WriteLine("Using '{0}' for tests ROOT_FOLDER", ROOT_FOLDER);
                }
                return _rootFolder;
            }
        }

        public static string LIB_FOLDER
        {
            get
            {
                if (string.IsNullOrEmpty(_libFolder))
                {
                    _libFolder = Path.Combine(ROOT_FOLDER, "lib");
                }

                return _libFolder;
            }
        }

        public static string PACKAGES_FOLDER
        {
            get
            {
                if (string.IsNullOrEmpty(_packagesFolder))
                {
                    _packagesFolder = Path.Combine(ROOT_FOLDER, "Packages");
                }

                return _packagesFolder;
            }
        }

        public static string WEB_PATH
        {
            get
            {
                if (string.IsNullOrEmpty(_webPath))
                {
                    _webPath = Path.Combine(Settings.ROOT_FOLDER, "src", "Roadkill.Web");
                    _webPath = new DirectoryInfo(_webPath).FullName;
                }

                return _webPath;
            }
        }

        public static readonly string ADMIN_EMAIL = "admin@localhost";
        public static readonly string ADMIN_PASSWORD = "password";
        public static readonly string EDITOR_EMAIL = "editor@localhost";
        public static readonly string EDITOR_PASSWORD = "password";
        public static readonly Guid ADMIN_ID = new Guid("aabd5468-1c0e-4277-ae10-a0ce00d2fefc");
        #endregion
    }
}