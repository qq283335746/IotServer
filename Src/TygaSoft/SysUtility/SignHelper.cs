using System;
using Newtonsoft.Json;
using TygaSoft.Model;

namespace TygaSoft.SysUtility
{
    public class SignHelper
    {
        public static string EncodeToken(TokenInfo tokenInfo, string salt = "Tu9vwcJ9Co/nLtOAJ+B87g==")
        {
            try
            {
                return EncryptHelper.EncodePassword(JsonConvert.SerializeObject(tokenInfo), PasswordFormatOptions.Aes, salt);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static TokenInfo UnEncodeToken(string token, string salt = "Tu9vwcJ9Co/nLtOAJ+B87g==")
        {
            try
            {
                return EncryptHelper.UnEncodePassword(token, PasswordFormatOptions.Aes, salt).ToModel<TokenInfo>();
            }
            catch
            {
                return null;
            }
        }


        public static DateTime ConvertLongToDateTime(long ticks, int secondsOrMilliseconds = 1)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0);
            return startTime.Add(new TimeSpan(ticks * TimeSpan.TicksPerMillisecond));
            // return startTime.AddMilliseconds(ticks).ToLocalTime();
            //long beginTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime().Ticks;
            //return new DateTime((ticks*10000+beginTicks)); 
            //return new DateTime(beginTicks + ticks * 10000, DateTimeKind.Utc).ToUniversalTime();
            // DateTime datetime = DateTime.MinValue;
            // DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            // if (secondsOrMilliseconds == 1)
            //     datetime = startTime.AddSeconds(ticks);
            // else
            //     datetime = startTime.AddMilliseconds(ticks);
            // return datetime;

            // DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            // TimeSpan toNow = new TimeSpan(ticks * 10000);
            // DateTime dtResult = dtStart.Add(toNow);
            // return dtResult;
        }

        public static long GetTimestamp(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks) / 10000;
        }
    }
}