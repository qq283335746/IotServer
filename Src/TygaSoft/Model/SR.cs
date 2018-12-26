namespace TygaSoft.Model
{
    public class SR
    {
        public static string GetString(string strString)
        {
            return strString;
        }
        public static string GetString(string format, string param1)
        {
            return string.Format(format, param1);
        }
        public static string GetString(string format, string param1, string param2)
        {
            return string.Format(format, param1, param2);
        }

        public static string GetString(string format, params object[] parms)
        {
            return string.Format(format, parms);
        }

        public const string Response_Ok = "调用成功！";
        public const string Response_Error = "调用失败！";

        public const string M_DataEmpty = "暂无数据！";
        public const string M_InvalidError = "";
        public const string M_CanNotDecodeHashedPassword = "无法对该加密类型进行解密";
    }
}