namespace Craftsman.Helpers
{
    using System.Diagnostics;

    public class NugetPackageInstaller
    {
        public static void InstallMediatr(string directory)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"add webapi package mediatr.extensions.microsoft.dependencyinjection",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WorkingDirectory = directory
                }
            };

            process.Start();
            process.WaitForExit();

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"add application package mediatr.extensions.microsoft.dependencyinjection",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WorkingDirectory = directory
                }
            };

            process.Start();
            process.WaitForExit();

            process.Start();
            process.WaitForExit();

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"add application package mediatr",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WorkingDirectory = directory
                }
            };

            process.Start();
            process.WaitForExit();

            process.Start();
            process.WaitForExit();

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = @$"add application package mediatr",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WorkingDirectory = directory
                }
            };

            process.Start();
            process.WaitForExit();
        }
    }
}
