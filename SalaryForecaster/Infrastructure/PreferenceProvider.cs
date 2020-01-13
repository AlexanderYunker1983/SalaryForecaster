using System.Collections.Generic;
using Android.Content;
using Android.Preferences;

namespace SalaryForecaster.Infrastructure
{
    public class PreferenceProvider
    {
        private readonly ISharedPreferences _sharedPreferences;
        private readonly ISharedPreferencesEditor _sharedPreferencesEditor;

        public PreferenceProvider(Context context)
        {
            _sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
            _sharedPreferencesEditor = _sharedPreferences.Edit();
        }

        public void SavePreference(string key, string value)
        {
            _sharedPreferencesEditor.PutString(key, value);
            _sharedPreferencesEditor.Commit();
        }
        public void SavePreference(string key, bool value)
        {
            _sharedPreferencesEditor.PutBoolean(key, value);
            _sharedPreferencesEditor.Commit();
        }
        public void SavePreference(string key, float value)
        {
            _sharedPreferencesEditor.PutFloat(key, value);
            _sharedPreferencesEditor.Commit();
        }
        public void SavePreference(string key, double value)
        {
            _sharedPreferencesEditor.PutLong(key, Java.Lang.Double.DoubleToLongBits(value));
            _sharedPreferencesEditor.Commit();
        }
        public void SavePreference(string key, int value)
        {
            _sharedPreferencesEditor.PutInt(key, value);
            _sharedPreferencesEditor.Commit();
        }
        public void SavePreference(string key, decimal value)
        {
            _sharedPreferencesEditor.PutFloat(key, (float) value);
            _sharedPreferencesEditor.Commit();
        }
        public void SavePreference(string key, long value)
        {
            _sharedPreferencesEditor.PutLong(key, value);
            _sharedPreferencesEditor.Commit();
        }
        public void SavePreference(string key, List<string> value)
        {
            _sharedPreferencesEditor.PutStringSet(key, value);
            _sharedPreferencesEditor.Commit();
        }

        public string GetStringPreference(string key, string defaultValue = default(string))
        {
            return _sharedPreferences.GetString(key, defaultValue);
        }
        public bool GetBoolPreference(string key, bool defaultValue = default(bool))
        {
            return _sharedPreferences.GetBoolean(key, defaultValue);
        }
        public float GetFloatPreference(string key, float defValue = default(float))
        {
            return _sharedPreferences.GetFloat(key, defValue);
        }
        public double GetDoublePreference(string key)
        {
            var longPref = _sharedPreferences.GetLong(key, default(long));
            return Java.Lang.Double.LongBitsToDouble(longPref);
        }
        public int GetIntPreference(string key, int defValue = default(int))
        {
            return _sharedPreferences.GetInt(key, defValue);
        }
        public long GetLongPreference(string key)
        {
            return _sharedPreferences.GetLong(key, default(long));
        }
        public ICollection<string> GetStringSetPreference(string key)
        {
            var set = new List<string>(_sharedPreferences.GetStringSet(key, new List<string>()));
            return set;
        }
    }
}