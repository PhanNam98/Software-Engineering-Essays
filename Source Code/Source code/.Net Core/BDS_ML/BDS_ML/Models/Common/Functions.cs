using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.Common
{
    public static class Functions
    {
        public static string getTime(DateTime? dtNull)
        {
            DateTime dt = dtNull ?? DateTime.Now;

            string result = (DateTime.Now.Subtract(dt).Days / 365).ToString();

            if (result != "0")
            {
                result += " năm trước";
            }
            else
            {
                result = (DateTime.Now.Subtract(dt).Days / 30).ToString();

                if (result != "0")
                {
                    result += " tháng trước";
                }
                else
                {
                    result = (DateTime.Now.Subtract(dt).Days / 7).ToString();

                    if (result != "0")
                    {
                        result += " tuần trước";
                    }
                    else
                    {
                        result = DateTime.Now.Subtract(dt).Days.ToString();

                        if (result != "0")
                        {
                            result += " ngày trước";
                        }
                        else
                        {
                            result = DateTime.Now.Subtract(dt).Hours.ToString();

                            if (result != "0")
                            {
                                result += " giờ trước";
                            }
                            else
                            {
                                result = DateTime.Now.Subtract(dt).Minutes.ToString();

                                if (result != "0")
                                {
                                    result += " phút trước";
                                }
                                else
                                {
                                    result = DateTime.Now.Subtract(dt).Seconds.ToString();

                                    result += " giây trước";
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
