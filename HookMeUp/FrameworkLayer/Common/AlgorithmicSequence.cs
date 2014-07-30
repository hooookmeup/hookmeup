using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SS.Framework.Common
{
    public class AlgorithmicSequence
    {

        private static string encryptionKey = "1jh3o";

        const int numseq = 6;
        const int numsig = 4;
        const int nBase = 26;

        public List<string> GenerateNumbers( int numofnumbers)
        {
            return GenerateNumbers(0, numofnumbers);
        }

        public List<string> GenerateNumbers(int startNumber, int numofnumbers)
        {
            var sha1 = SHA1.Create();
            List<string> rez = new List<string>();
            for (int number = startNumber; number < (startNumber+numofnumbers); number++)
            {
                string data = number.ToString("0000000000");
                byte[] chec = new byte[8];
                for (int i = 0; i < 4; i++)
                    chec[3 - i] = (byte)(number >> (i * 8));

                byte[] checksumData = System.Text.Encoding.UTF8.GetBytes(data + encryptionKey);
                byte[] hash = sha1.ComputeHash(checksumData);

                int hashnum = 0;
                for (int i = 0; i < 2; i++)
                    hashnum = hashnum * 256 + hash[i];
                string hashstr = Num25BaseString(hashnum, numsig, numsig);
                string rezstr = MixHash(Num25BaseString(number, numseq, numseq), hashstr);
                rez.Add(FormatString(rezstr, 5));
            }
            return rez;
        }


        // restoer bytes of signature with seq to original order.

        private void UnmixHash(string value, out string seq, out string hash)
        {
            hash = "";
            seq = "";
            for (int i = 0; i < numsig * 2; i++)
            {
                seq = seq + value[i];
                i++;
                hash = hash + value[i];
            }
            seq = seq + (value.Substring(numsig * 2));
        }

        // mix bytes of signature with seq to obscure appirance.

        private string MixHash(string seq, string hash)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < numsig; i++)
            {
                sb.Append(seq[i]);
                sb.Append(hash[i]);

            }
            sb.Append(seq.Substring(numsig));
            return sb.ToString();
        }


        public bool testSign(string parString, out int seqNumber)
        {
            var sha1 = SHA1.Create();
            bool rez = false;

            try
            {

                string NumberString = parString.ToUpper();
                NumberString = NumberString.Replace("-", "");
                string hashstr;
                string numstr;
                UnmixHash(NumberString, out numstr, out hashstr);
                seqNumber = BaseStr2Number(numstr);

                string data = seqNumber.ToString("0000000000");
                byte[] checksumData = System.Text.Encoding.UTF8.GetBytes(data + encryptionKey);
                byte[] hash = sha1.ComputeHash(checksumData);

                int hashnum = 0;
                for (int i = 0; i < 2; i++)
                    hashnum = hashnum * 256 + hash[i];
                rez = hashstr == Num25BaseString(hashnum, numsig, numsig);

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                rez = false;
                seqNumber = -1;
            }

            return rez;
        }

        /*
                private string ByteArray2String(byte[] value)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in value)
                    {
                        if (sb.Length > 0)
                            sb.Append("-");
                        sb.Append(b.ToString("X2"));

                    }
                    return sb.ToString();
                }


                private byte[] String2ByteArray(string value)
                {
                    string HexString = value.Replace("-", "");
                    int NumberChars = HexString.Length;
                    byte[] bytes = new byte[NumberChars / 2];
                    for (int i = 0; i < NumberChars; i += 2)
                    {
                        bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
                    }
                    return bytes;
                }


                private string ByteArray2_25BaseString(byte[] value)
                {
                    StringBuilder sb = new StringBuilder();
                    ulong a = 0;
                    for (int i = 0; i < value.Length; i++)
                    {
                        a = 256 * a + value[i];


                    }
                    ulong rest = a;
                    ulong cuur;
                    do
                    {
                        cuur = rest % 25;
                        rest = rest / 25;
                        sb.Append((char)(cuur + 65));

                    } while (cuur > 0);



                    return sb.ToString();
                }

        */

        public string Num25BaseString(int number, int minchar, int maxChar)
        {
            StringBuilder sb = new StringBuilder();

            int rest = number;
            int cuur;
            do
            {
                cuur = rest % nBase;
                rest = rest / nBase;
                sb.Insert(0, (char)(cuur + 65));

            } while (rest > 0 && sb.Length < maxChar);

            while (sb.Length < minchar)
                sb.Insert(0, 'A');
            return sb.ToString();
        }

        public int BaseStr2Number(string value)
        {
            int rez = 0;
            char[] val = value.ToCharArray();
            for (int i = 0; i < val.Length; i++)
            {
                rez = nBase * rez + (byte)val[i] - 65;
            }
            return rez;
        }




        private string FormatString(string str, int grSize)
        {
            StringBuilder sb = new StringBuilder();
            int num = 0;
            foreach (var ch in str)
            {
                if (num == grSize)
                {
                    num = 0;
                    sb.Append("-");
                }
                sb.Append(ch);
                num++;

            }
            return sb.ToString();

        }
    }
}








