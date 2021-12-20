using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace El.Helpers
{
    /// <summary>
    /// Extention methods
    /// </summary>
    public static class Extensions
    {
        //The reg ex
        private static readonly Regex reUserName = new Regex("[^@]+@", RegexOptions.Compiled | RegexOptions.Singleline);

        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = false,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// JSON Serialize object
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="toSerialize">The object</param>
        /// <returns>Serialized object</returns>
        public static string SerializeObject<T>(this T toSerialize)
        {
            //Serialize data
            return JsonSerializer.Serialize(toSerialize, JsonOptions);
        }


        /// <summary>
        /// JSON De-serialize
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="jsonData">JSON string</param>
        /// <returns>The object</returns>
        public static T DeserializeObject<T>(this string jsonData)
        {
            //De-Serialize data
            var res = JsonSerializer.Deserialize<T>(jsonData, JsonOptions);
            if (res != null) {
                return res;
            } else {
                throw new JsonException("Null object deserialized!");
            }
        }

        /// <summary>
        /// Deep clone public properties of the object
        /// </summary>
        /// <param name="source">The source</param>
        public static T? Clone<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) {
                return default(T);
            }

            //return
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source, JsonOptions), JsonOptions);
        }

        /// <summary>
        /// Calc hash
        /// </summary>
        /// <param name="source">The source</param>
        public static string Hash<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) {
                return string.Empty;
            }

            // calc and return
            using (var sha = SHA256.Create()) {
                return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(SerializeObject(source))));
            }
        }

        ///// <summary>
        ///// A method used to save error information from desktop
        ///// </summary>
        ///// <param name="Log">The log object</param>
        ///// <param name="title">Some title text</param>
        ///// <param name="ex">The exception</param>
        //public static void SaveError(this ILog Log, string title, Exception ex)
        //{
        //    try {
        //        //Locals
        //        StringBuilder sb = new StringBuilder();
        //        Exception hlp = ex;

        //        //Log error
        //        while (hlp.InnerException != null && !string.IsNullOrWhiteSpace(hlp.InnerException.Message)) {
        //            hlp = hlp.InnerException;
        //        }
        //        Log.LogError(title, $"ErrorText:{hlp.Message}\r\nStackTrace:{ex.StackTrace}");
        //    } catch { }
        //}

        ///// <summary>
        ///// A method used to save error information from desktop
        ///// </summary>
        ///// <param name="title">Some title text</param>
        ///// <param name="ex">The exception</param>
        //public static void SaveError(this ILog Log, string title, string ex)
        //{
        //    try {
        //        //Log error
        //        Log.LogError(title, ex);
        //    } catch { }
        //}

        ///// <summary>
        ///// A method used to save info information
        ///// </summary>
        ///// <param name="title">Some title text</param>
        ///// <param name="infoMess">The info message</param>
        //public static void SaveInfo(this ILog Log, string title, string infoMess)
        //{
        //    try {
        //        //Log error
        //        Log.LogInfo(title, infoMess);
        //    } catch { }
        //}

        ///// <summary>
        ///// A method used to save warning information
        ///// </summary>
        ///// <param name="title">Some title text</param>
        ///// <param name="infoMess">The warning message</param>
        //public static void SaveWarn(this ILog Log, string title, string warnMess)
        //{
        //    try {
        //        //Log error
        //        Log.LogWarn(title, warnMess);
        //    } catch { }
        //}

        /// <summary>
        /// Strip domain name from user
        /// </summary>
        /// <param name="usrName">Original name</param>
        /// <returns>Stripped name</returns>
        public static string NormalizeUserName(this string usrName)
        {
            //Normalize name
            Match mRe = reUserName.Match(usrName);
            if (mRe.Success) {
                return mRe.Value.Substring(0, mRe.Value.Length - 1);
            } else {
                return usrName;
            }
        }

        /// <summary>
        /// A method that creates a common culture
        /// </summary>
        /// <param name="lang">The base culture for the common it</param>
        /// <returns>The create common/returns>
        public static CultureInfo GetCommonCultureInfo(this string lang)
        {
            DateTimeFormatInfo dtfi;
            CultureInfo _pci;

            //If
            if (string.IsNullOrWhiteSpace(lang)) {
                lang = Nomens.Const.Globalization.DefaultLang;
            }

            dtfi = CultureInfo.CreateSpecificCulture(Nomens.Const.Globalization.DefaultLang).DateTimeFormat;
            dtfi.ShortDatePattern = Nomens.Const.Globalization.ShortDatePattern;
            _pci = CultureInfo.CreateSpecificCulture(lang);
            _pci.DateTimeFormat = dtfi;
            _pci.DateTimeFormat.ShortDatePattern = Nomens.Const.Globalization.ShortDatePattern;
            _pci.NumberFormat.CurrencyDecimalSeparator = ".";
            _pci.NumberFormat.CurrencyGroupSeparator = " ";
            _pci.NumberFormat.NumberDecimalSeparator = ".";
            _pci.NumberFormat.NumberGroupSeparator = " ";

            return _pci;
        }

        ///// <summary>
        ///// Store activity
        ///// </summary>
        ///// <typeparam name="T">Object to store.If no object can sent message</typeparam>
        ///// <param name="userID">User ID</param>
        ///// <param name="uAction">Action</param>
        ///// <param name="message">message text to store if no obkect</param>
        ///// <param name="pReference">Object DB reference</param>
        ///// <param name="obj">Object to store as JSON</param>
        ///// <returns></returns>
        //public static RUsersActivity CreateRec<T>(int userID, UserActions uAction, T? obj = default(T))
        //{
        //    //Create
        //    var act = new RUsersActivity
        //    {
        //        //Set
        //        EventTime = DateTime.Now,
        //        IdActivity = (short)uAction,
        //        IdUser = (short)userID
        //    };

        //    //write notes
        //    if (obj != null) {
        //        if (obj is not string) {
        //            act.Notes = obj.SerializeObject();
        //        } else {
        //            act.Notes = obj as string;
        //        }
        //    }

        //    return act;
        //}

        //public static async Task<decimal> DoInsertActivity(DskMcbContext Db, RUsersActivity sua, decimal? pReference = null)
        //{
        //    // Store It
        //    sua.Reference = pReference;
        //    Db.Entry(sua).State = EntityState.Added;

        //    // Do Store it
        //    await Db.SaveChangesAsync().ConfigureAwait(false);

        //    // return
        //    return sua.IdUa;
        //}
    }
}
