using System;
using System.Text;

namespace Sharp_231.Networking
{
    internal class OtpPass
    {
        public void Run()
        {
            Console.WriteLine("OTP password generation");

            Console.Write("Enter password length: ");
            int length = Convert.ToInt32(Console.ReadLine());

            Console.Write("Select mode (1 - digits, 2 - letters, 3 - mixed): ");
            int mode = Convert.ToInt32(Console.ReadLine());

            try
            {
                String otp = Generate(length, mode);
                Console.WriteLine($"OTP: {otp}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OTP generation error: {ex.Message}");
            }
        }

        public String Generate(int length, int mode)
        {
            if (length <= 0)
            {
                throw new Exception("Invalid password length");
            }

            String alphabet;

            if (mode == 1)
            {
                alphabet = "0123456789";
            }
            else if (mode == 2)
            {
                alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            }
            else if (mode == 3)
            {
                alphabet = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            }
            else
            {
                throw new Exception("Invalid mode");
            }

            Random random = new();
            StringBuilder sb = new();

            for (int i = 0; i < length; i++)
            {
                sb.Append(alphabet[random.Next(alphabet.Length)]);
            }

            return sb.ToString();
        }
    }
}