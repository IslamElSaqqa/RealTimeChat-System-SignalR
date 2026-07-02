using Desktop.Forms;
using Desktop.Helpers;

namespace Desktop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Check if user is already logged in
            if (TokenStorage.IsTokenAvailable())
            {
                Application.Run(new ChatForm());
            }
            else
            {
                Application.Run(new LoginForm());
            }
        }
    }
}
