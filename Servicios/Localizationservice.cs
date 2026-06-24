using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Servicios.Localization
{
    // ─────────────────────────────────────────────
    //  Observer contract
    // ─────────────────────────────────────────────

    /// <summary>
    /// Any UI element (Form, UserControl, etc.) that wants to be
    /// notified when the application language changes must implement
    /// this interface.
    /// </summary>
    public interface ILocalizationObserver
    {
        /// <summary>Called by <see cref="LocalizationService"/> after the
        /// active locale has been replaced.</summary>
        void OnLanguageChanged();
    }

    // ─────────────────────────────────────────────
    //  Supported languages
    // ─────────────────────────────────────────────

    public enum AppLanguage
    {
        Espanol = 0,   // es.json  (default)
        English = 1,   // en.json
        Portugues = 2   // pt.json
    }

    // ─────────────────────────────────────────────
    //  Observable singleton service
    // ─────────────────────────────────────────────

    /// <summary>
    /// Singleton that loads JSON locale files and notifies every registered
    /// <see cref="ILocalizationObserver"/> when the language is switched.
    /// </summary>
    public sealed class LocalizationService
    {
        // ── Singleton ────────────────────────────
        private static readonly Lazy<LocalizationService> _lazy =
            new Lazy<LocalizationService>(() => new LocalizationService());

        public static LocalizationService Instance => _lazy.Value;

        // ── Observer list ─────────────────────────
        private readonly List<WeakReference<ILocalizationObserver>> _observers =
            new List<WeakReference<ILocalizationObserver>>();

        // ── State ─────────────────────────────────
        private AppLanguage _currentLanguage = AppLanguage.Espanol;
        private Dictionary<string, Dictionary<string, string>> _strings =
            new Dictionary<string, Dictionary<string, string>>();

        /// <summary>Folder that contains es.json / en.json / pt.json.
        /// Defaults to a "Localization" sub-folder next to the executable.</summary>
        public string LocalizationFolder { get; set; }

        public AppLanguage CurrentLanguage => _currentLanguage;

        // ── Constructor ───────────────────────────
        private LocalizationService()
        {
            LocalizationFolder = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
                "Localization");

            // Load the default language immediately
            LoadLanguage(AppLanguage.Espanol);
            ApplyThreadCulture(AppLanguage.Espanol);
        }

        // ─────────────────────────────────────────
        //  Observer management
        // ─────────────────────────────────────────

        /// <summary>Register an observer.  Uses a WeakReference so Forms
        /// that are closed/GC-collected never cause a memory leak.</summary>
        public void Subscribe(ILocalizationObserver observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");
            Purge();
            _observers.Add(new WeakReference<ILocalizationObserver>(observer));
        }

        /// <summary>Explicitly unregister an observer (optional – WeakReferences
        /// handle disposal automatically).</summary>
        public void Unsubscribe(ILocalizationObserver observer)
        {
            if (observer == null) return;
            Purge();
            _observers.RemoveAll(wr =>
            {
                ILocalizationObserver target;
                return wr.TryGetTarget(out target) && ReferenceEquals(target, observer);
            });
        }

        /// <summary>Remove dead weak-references.</summary>
        private void Purge()
        {
            _observers.RemoveAll(wr =>
            {
                ILocalizationObserver t;
                return !wr.TryGetTarget(out t);
            });
        }

        private void NotifyAll()
        {
            Purge();
            foreach (var wr in _observers)
            {
                ILocalizationObserver observer;
                if (wr.TryGetTarget(out observer))
                {
                    try { observer.OnLanguageChanged(); }
                    catch { /* never let an observer crash the service */ }
                }
            }
        }

        // ─────────────────────────────────────────
        //  Language switching
        // ─────────────────────────────────────────

        /// <summary>Switch the active language and notify all observers.</summary>
        public void SetLanguage(AppLanguage language)
        {
            if (language == _currentLanguage) return;
            LoadLanguage(language);
            _currentLanguage = language;
            ApplyThreadCulture(language);
            NotifyAll();
        }

        /// <summary>
        /// Sets the calling (UI) thread's culture so that stock .NET dialogs —
        /// most importantly MessageBox's Yes/No/OK/Cancel button captions —
        /// render in the selected app language instead of whatever the OS
        /// install language happens to be.
        /// </summary>
        private static void ApplyThreadCulture(AppLanguage lang)
        {
            CultureInfo culture;
            switch (lang)
            {
                case AppLanguage.English: culture = new CultureInfo("en-US"); break;
                case AppLanguage.Portugues: culture = new CultureInfo("pt-BR"); break;
                default: culture = new CultureInfo("es-ES"); break;
            }

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        /// <summary>Convenience overload that maps the combo-box index
        /// (0 = Español, 1 = English, 2 = Português) to a language.</summary>
        public void SetLanguageByIndex(int index)
        {
            if (Enum.IsDefined(typeof(AppLanguage), index))
                SetLanguage((AppLanguage)index);
        }

        // ─────────────────────────────────────────
        //  String lookup
        // ─────────────────────────────────────────

        /// <summary>
        /// Returns the localized string for <paramref name="key"/> inside
        /// <paramref name="section"/>.
        /// Falls back to "section.key" if the entry is missing.
        /// </summary>
        public string Get(string section, string key)
        {
            Dictionary<string, string> sec;
            if (_strings.TryGetValue(section, out sec))
            {
                string value;
                if (sec.TryGetValue(key, out value))
                    return value;
            }
            return string.Format("[{0}.{1}]", section, key);   // fallback
        }

        /// <summary>Shorthand: <c>T["FormLogin","Title"]</c></summary>
        public string this[string section, string key] => Get(section, key);

        // ─────────────────────────────────────────
        //  JSON loading  (no external dependencies)
        // ─────────────────────────────────────────

        private void LoadLanguage(AppLanguage lang)
        {
            string fileName = LanguageToFileName(lang);
            string fullPath = Path.Combine(LocalizationFolder, fileName);

            string json;
            if (File.Exists(fullPath))
            {
                json = File.ReadAllText(fullPath, Encoding.UTF8);
            }
            else
            {
                // Try to load from embedded resources as fallback
                json = TryLoadEmbedded(fileName);
                if (json == null)
                    throw new FileNotFoundException(
                        "Locale file not found: " + fullPath);
            }

            _strings = ParseJson(json);
        }

        private static string LanguageToFileName(AppLanguage lang)
        {
            switch (lang)
            {
                case AppLanguage.English: return "en.json";
                case AppLanguage.Portugues: return "pt.json";
                default: return "es.json";
            }
        }

        /// <summary>Try to load a locale file that was embedded as a resource
        /// (namespace Servicios.Localization.&lt;filename&gt;).</summary>
        private static string TryLoadEmbedded(string fileName)
        {
            var asm = Assembly.GetExecutingAssembly();
            string resourceName = "Servicios.Localization." + fileName;
            using (Stream s = asm.GetManifestResourceStream(resourceName))
            {
                if (s == null) return null;
                using (var reader = new StreamReader(s, Encoding.UTF8))
                    return reader.ReadToEnd();
            }
        }

        // ── Minimal hand-rolled JSON parser ───────
        // Handles the exact two-level { "section": { "key": "value" } }
        // structure used by the locale files without requiring Json.NET.

        private static Dictionary<string, Dictionary<string, string>> ParseJson(string json)
        {
            var result = new Dictionary<string, Dictionary<string, string>>(
                StringComparer.OrdinalIgnoreCase);

            int pos = 0;
            SkipWhitespace(json, ref pos);
            Expect(json, ref pos, '{');

            while (pos < json.Length)
            {
                SkipWhitespace(json, ref pos);
                if (json[pos] == '}') break;

                string sectionName = ReadString(json, ref pos);
                SkipWhitespace(json, ref pos);
                Expect(json, ref pos, ':');
                SkipWhitespace(json, ref pos);
                Expect(json, ref pos, '{');

                var section = new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

                while (pos < json.Length)
                {
                    SkipWhitespace(json, ref pos);
                    if (json[pos] == '}') { pos++; break; }

                    string key = ReadString(json, ref pos);
                    SkipWhitespace(json, ref pos);
                    Expect(json, ref pos, ':');
                    SkipWhitespace(json, ref pos);
                    string value = ReadString(json, ref pos);
                    section[key] = value;

                    SkipWhitespace(json, ref pos);
                    if (pos < json.Length && json[pos] == ',') pos++;
                }

                result[sectionName] = section;

                SkipWhitespace(json, ref pos);
                if (pos < json.Length && json[pos] == ',') pos++;
            }

            return result;
        }

        private static void SkipWhitespace(string s, ref int pos)
        {
            while (pos < s.Length && char.IsWhiteSpace(s[pos])) pos++;
        }

        private static void Expect(string s, ref int pos, char c)
        {
            if (pos >= s.Length || s[pos] != c)
                throw new FormatException(
                    string.Format("Expected '{0}' at position {1}", c, pos));
            pos++;
        }

        private static string ReadString(string s, ref int pos)
        {
            Expect(s, ref pos, '"');
            var sb = new StringBuilder();
            while (pos < s.Length)
            {
                char c = s[pos++];
                if (c == '"') break;
                if (c == '\\' && pos < s.Length)
                {
                    char esc = s[pos++];
                    switch (esc)
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        default: sb.Append(esc); break;
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}