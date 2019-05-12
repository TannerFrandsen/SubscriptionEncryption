using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SubscriptionEncryption
{
    public class Security
    {
        public static string Encode(string clearText)
        {
            StringBuilder encoded = new StringBuilder();
            for(int i = 0; i < clearText.Length && i + 1 < clearText.Length; i += 2)
            {
                // First convert each letter to an integer.
                int letter1 = Convert.ToInt32(clearText[i]);
                int letter2 = Convert.ToInt32(clearText[i + 1]);

                // Hide the first letter in each pair using XOR.
                // Hide the second letter in each pair using a modified linear shift.
                char char1 = Convert.ToChar(letter1 ^ letter2);
                char char2 = Convert.ToChar(letter2 + 7);

                encoded.Append(char1);
                encoded.Append(char2);
            }

            // Dont forget the last character if needed.
            if (encoded.Length != clearText.Length)
                encoded.Append(clearText[clearText.Length - 1]);

            return encoded.ToString();
        }

        public static string Decode(string encodedText)
        {
            StringBuilder decoded = new StringBuilder();

            for (int i = 0; i < encodedText.Length && i + 1 < encodedText.Length; i += 2)
            {
                // First convert each letter to an integer.
                // Dont forget to calculate the real key value by reversing the modified linear shift.
                int letter1 = Convert.ToInt32(encodedText[i]);
                int letter2 = Convert.ToInt32(encodedText[i + 1]) -  7;

                // Calculate the original values.
                char char1 = Convert.ToChar(letter1 ^ letter2);
                char char2 = Convert.ToChar(letter2);

                decoded.Append(char1);
                decoded.Append(char2);
            }

            // Dont forget the last character if needed
            if (decoded.Length != encodedText.Length)
                decoded.Append(encodedText[encodedText.Length - 1]);

            return decoded.ToString();
        }

        public static void WriteToFile(string filePath, string data)
        {
            try
            {
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(filePath))
                {
                    outputFile.WriteLine(data);
                    outputFile.Close();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Error Writing data to the file '" + filePath + "'");
                throw e;
            }
        }

        public static string ReadFromFile(string filePath)
        {
            string data = "";
            try
            {
                using (System.IO.StreamReader inputFile = new System.IO.StreamReader(filePath))
                {
                    data = inputFile.ReadToEnd();
                    data = data.Remove(data.Length - 2, 2);
                    inputFile.Close();
                    return data;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Error Reading data from file '" + filePath + "'");
                throw e;
            }
        }
    }
}
