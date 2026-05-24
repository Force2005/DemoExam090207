using DE.Forms.Infrastructure;

namespace DE.Forms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                var bootstrapper = new ApplicationBootstrapper();
                bootstrapper.InitializeAsync().GetAwaiter().GetResult();
                Application.Run(bootstrapper.CreateLoginForm());
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    $"Не удалось запустить приложение.{Environment.NewLine}{Environment.NewLine}{exception.Message}",
                    "Ошибка запуска",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
