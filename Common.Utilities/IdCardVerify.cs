using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public class IdCardVerify
    {
        /**
         * 校验给定的身份证号是否有效。会从证件号位数，是否含无效字符，出生日期码及校验码几方面加以验证。
         * 
         * @param idNumber
         *            受检身份证号
         * @return bool
         *         如果给定的身份证号是15位或18位长，出生日期码，及校验位（只对18位证号检查）均正确时返回True，否则返回False。
         */
        public static bool Verify(string idNumber)
        {
          return Verify(idNumber, false);
        }
        public static bool Verify(string idNumber, bool unCheckDigitAndDate)
        {
            if (idNumber == null)
                return false;
            if (idNumber.Length != 15 && idNumber.Length != 18)
                return false;
            for (int i = 0; i < idNumber.Length; i++)
            {
                char c = idNumber[i];
                if (i < 17 && !char.IsDigit(c))
                    return false;
            }
            if (idNumber.Length == 18)
            {
                if (idNumber.Substring(17).Equals("x"))
                {
                    idNumber = idNumber.Substring(0, 17) + "X";
                }
            }

            if (idNumber.Length == 18 && idNumber[17] != 'X' && !char.IsDigit(idNumber[17]))
                return false;

            string dateStr = idNumber.Length == 18 ? idNumber.Substring(6, 8) : idNumber.Substring(6, 6);

            return unCheckDigitAndDate || isValidDate(dateStr) && VerifyCheckDigit(idNumber);
        }
        /**
         * 校验给定的身份证号的出生日期码所代表的日期与给定的出生日期是否相同。
         * 
         * @param idNumber
         *            受检身份证号
         * @param birthDate
         *            出生日期
         * @return bool
         *         如果birthDate为Null，或者身份证号的出生日期码所代表的日期与给定的出生日期相同时返回True，否则返回False。
         */
        public static bool Verify(string idNumber, DateTime birthDate)
        {
            if (idNumber == null)
                return false;
            if (birthDate == null)
                return true;
            DateTime cal = birthDate;
            int year = cal.Year;
            int month = cal.Month;
            int date = cal.Day;
            if (idNumber.Length == 18)
            {
                string yearStr = idNumber.Substring(6, 4);
                string monthStr = idNumber.Substring(10, 2);
                string dateStr = idNumber.Substring(12, 2);
                try
                {
                    return (int.Parse(dateStr) == date) && (int.Parse(monthStr) == month) && (int.Parse(yearStr) == year);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            else if (idNumber.Length == 15)
            {
                string yearStr =string.Format("19{0}",idNumber.Substring(6, 2));

                string monthStr = idNumber.Substring(8, 2);
                string dateStr = idNumber.Substring(10, 2);
                try
                {
                    return (int.Parse(dateStr) == date) && (int.Parse(monthStr) == month) && (int.Parse(yearStr) == (year % 100));
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        /**
         * 校验给定的身份证号的顺序码所揭示的性别是否与给定的性别相同。如果顺序码为奇数应为男性，为偶数应为女性。
         * 
         * @param idNumber
         *            受检身份证号
         * @param gender
         *            性别
         * @return bool 如果gender为Null，或者身份证号的顺序码所揭示的性别与给定的性别相同时返回True，否则返回False。
         */
        public static bool Verify(string idNumber, Gender gender)
        {
            if (idNumber == null)
                return false;
            if (gender == null)
                return true;
            if (idNumber.Length != 18 && idNumber.Length != 15)
                return false;
            int g = idNumber.Length == 18 ? idNumber[16] : idNumber[14];
            return (Gender.FEMALE.Equals(gender) && ((g % 2) == 0)) || (Gender.MALE.Equals(gender) && ((g % 2) == 1));
        }

        /**
         * 检验给定身份证号的地址码是否是给定的行政区划代码。
         * 
         * @param idNumber
         *            受检身份证号
         * @param regionalCode
         *            6位行政区划代码
         * @return 
         *         如果regionalCode不是6位长，返回True；如果idNumber前6位与regionalCode相同，返回True；否则返回False
         *         。
         */
        public static bool Verify(string idNumber, string regionalCode)
        {
            if (idNumber == null)
                return false;
            if (regionalCode == null || regionalCode.Length != 6)
                return true;
            return idNumber.StartsWith(regionalCode);
        }

        /**
         * 检验给定身份证号的校验码是否正确。
         * 
         * @param idNumber
         *            受检身份证号
         * @return 如果是15位证号，或者是校验码正确的18位证号返回True；否则返回False。
         */
        public static bool VerifyCheckDigit(string idNumber)
        {
            if (idNumber == null)
                return false;
            if (idNumber.Length == 15)
                return true;

            if (idNumber.Length == 18)
            {
                char checkDigit = idNumber[17];
                return checkDigit == CreateCode(idNumber.ToCharArray());
            }

            return false;
        }

        /**
         * 将15位身份证号转换为18位证号。将15位证号的日期码由6位改为8位，即由XXMMDD改为为19XXMMDD。
         * 
         * @param personIDCode
         *            15位身份证号
         * @return 如果personIDCode是15位证号，则返回对应的18位身份证号；否则返回personIDCode。
         */
        public static string transferTo18Bit(string personIDCode)
        {
            if (personIDCode == null || personIDCode.Trim().Length != 15)
            {
                return personIDCode;
            }
            char[] id15 = personIDCode.Trim().ToCharArray();
            char[] id18 = new char[18];
            for (int i = 0; i < 6; i++)
                id18[i] = id15[i];
            for (int i = 6; i < 15; i++)
                id18[i + 2] = id15[i];
            id18[6] = '1';
            id18[7] = '9';
            id18[17] = CreateCode(id18);
            return new string(id18);
        }

        /**
         * 将18位身份证号转换为15位证号。将18位证号的日期码由8位改为6位，即由19XXMMDD改为为XXMMDD。
         * 
         * @param personIDCode
         *            15位身份证号
         * @return 如果personIDCode是19XX年出生的18位证号，则返回对应的15位身份证号；否则返回personIDCode。
         */
        public static string transferTo15Bit(string personIDCode)
        {
            if (personIDCode == null || personIDCode.Trim().Length != 18)
            {
                return personIDCode;
            }
            char[] id18 = personIDCode.Trim().ToCharArray();
            if (id18[6] == '1' && id18[7] == '9')
            {
                char[] id15 = new char[15];
                for (int i = 0; i < 6; i++)
                    id15[i] = id18[i];
                for (int i = 8; i < 17; i++)
                    id15[i - 2] = id18[i];
                return new string(id15);
            }
            else
                return personIDCode;

        }

        protected static char CreateCode(char[] id18)
        {
            char[] code = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' }; // 11个

            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum = sum + int.Parse(id18[i].ToString()) * wi(i);
            }
            int index = sum % 11;

            return code[index];
        }

        protected static int wi(int index)
        {
            if (index > 17 || index < 0)
                throw new Exception("index out of bound");
            int n = 17 - index;
            return (1 << n) % 11;
        }
        public static bool VerifyDate(string idNumber)
        {
            string dateStr = idNumber.Length == 18 ? idNumber.Substring(6, 8) : idNumber.Substring(6, 6);
            
            return isValidDate(dateStr);
        }
        private static bool isValidDate(string datestring)
        {
            if (datestring == null)
                return false;
            if (datestring.Length == 6)
                datestring = string.Format("19{0}", datestring);

            datestring = datestring.Insert(6, "-").Insert(4, "-");
            DateTime dt;
            return DateTime.TryParse(datestring, out dt);
        }

        /**
         * 
         * @param idNumber
         * @return Owner's Actual Birthdate
         */
        public static DateTime idNumber2BirthDate(string idNumber)
        {
            try
            {
                
                DateTime cal = new DateTime();
            
                if (idNumber.Length == 18)
                {
                    int yearStr = int.Parse(idNumber.Substring(6, 10));
                    int monthStr = int.Parse(idNumber.Substring(10, 12)) - 1;
                    int dateStr = int.Parse(idNumber.Substring(12, 14));
                    cal = new DateTime(yearStr, monthStr, dateStr);
                }
                else if (idNumber.Length == 15)
                {
                    int year = int.Parse(idNumber.Substring(6, 8));
                    int yearStr = year + 1900;
                    int monthStr = int.Parse(idNumber.Substring(8, 10)) - 1;
                    int dateStr = int.Parse(idNumber.Substring(10, 12));
                    cal = new DateTime(yearStr, monthStr, dateStr);
                }
                return cal;
            }
            catch { return DateTime.MinValue; }
        }
        /**
        * 
        * @param idNumber
        * @return Owner's Actual Birthdate
        */
        public static DateTime idNumberBirthDate(string idNumber)
        {

            return idNumberBirthDate(idNumber, DateTime.MinValue);
        }
        public static DateTime idNumberBirthDate(string idNumber,DateTime defaultBirth)
        {
            DateTime result = DateTime.MinValue;
            if (idNumber.Length == 18)
            {
                if (DateTime.TryParse(idNumber.Substring(6, 4) + "-" + idNumber.Substring(10, 2) + "-" + idNumber.Substring(12, 2), out result))
                    return result;
            }
            else if (idNumber.Length == 15)
            {
                if (DateTime.TryParse("19" + idNumber.Substring(6, 2) + '-' + idNumber.Substring(8, 2) + '-' + idNumber.Substring(10, 2), out result))
                    return result;
            }
            return defaultBirth;
        }

        /**
         * 
         * @param idNumber
         * @return Owner's Actual Gender
         */
        public static Gender getSexFromCard(string idNumber)
        {
            try
            {
                string inputStr = idNumber.ToString();
                int sex;
                if (inputStr.Length == 18)
                {
                    sex = inputStr[16];
                    if (sex % 2 == 0)
                    {
                        return Gender.FEMALE;
                    }
                    else
                    {
                        return Gender.MALE;
                    }
                }
                else if (inputStr.Length == 15)
                {
                    sex = inputStr[14];
                    if (sex % 2 == 0)
                    {
                        return Gender.FEMALE;
                    }
                    else
                    {
                        return Gender.MALE;
                    }
                }
            }
            catch {  }
            return Gender.MALE;
        }

        public enum Gender
        {  
            MALE=1,
            FEMALE=2
        }
    }
}
