using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WFS.business.SessionSettings
{
    public class PasswordRules : IDisposable
    {
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool Disposed { get; private set; }
        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        } 
        #endregion

        public enum PasswordStrength
        {
            Unacceptable,
            Weak,
            Normal,
            Strong,
            Secure
        }
        public int GetLowerScore(string password)
        {
            int rawScore = password.Length - Regex.Replace(password, "[a-z]", "").Length;
            return Math.Min(2, rawScore) * 5;
        }
        public int GetUpperScore(string password)
        {
            int rawScore = password.Length - Regex.Replace(password, "[A-Z]", "").Length;
            return Math.Min(2, rawScore) * 5;
        }
        public int GetDigitScore(string password)
        {
            int rawScore = password.Length - Regex.Replace(password, "[0-9]", "").Length;
            return Math.Min(2, rawScore) * 5;
        }
        public int GetSymbolScore(string password)
        {
            int rawScore = Regex.Replace(password, "[a-zA-Z0-9]", "").Length;
            return Math.Min(2, rawScore) * 5;
        }
        private int GetLengthScore(string password)
        {
            return Math.Min(10, password.Length) * 6;
        }
        public int GeneratePasswordScore(string password)
        {
            if (password == null)
            {
                return 0;
            }

            int lengthScore = GetLengthScore(password);
            int lowerScore = GetLowerScore(password);
            int upperScore = GetUpperScore(password);
            int digitScore = GetDigitScore(password);
            int symbolScore = GetSymbolScore(password);

            return lengthScore + lowerScore + upperScore + digitScore + symbolScore;
        }
        public PasswordStrength GetPasswordStrength(string password)
        {
            int score = GeneratePasswordScore(password);

            if (score < 50)
                return PasswordStrength.Unacceptable;
            else if (score < 60)
                return PasswordStrength.Weak;
            else if (score < 80)
                return PasswordStrength.Normal;
            else if (score < 90)
                return PasswordStrength.Strong;
            else
                return PasswordStrength.Secure;
        }
    }
}