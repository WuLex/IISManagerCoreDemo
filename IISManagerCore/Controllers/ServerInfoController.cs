using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace IISManagerCore.Controllers
{
    public class ServerInfoController : Controller
    {
        public IActionResult Index()
        {
            var serverInfo = new
            {
                MachineName = Environment.MachineName,
                OSVersion = Environment.OSVersion.ToString(),
                ProcessorCount = Environment.ProcessorCount,
                CurrentDirectory = Environment.CurrentDirectory,
                SystemPageSize = Environment.SystemPageSize,
                UserName = Environment.UserName,
                WorkingSet = Environment.WorkingSet,
                DotNetVersion = Environment.Version.ToString(),
                ProcessUptime = GetProcessUptime(),
                IISVersion = Request.Headers["Server"].ToString(),
                IPAddress = HttpContext.Connection.LocalIpAddress?.ToString(),
                Port = HttpContext.Connection.LocalPort,
                AspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            };

            return View(serverInfo);
        }

        private string GetProcessUptime()
        {
            using var process = Process.GetCurrentProcess();
            var uptime = DateTime.Now - process.StartTime;
            return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Environment.CurrentDirectory, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, uploadedFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }

                return Json(new { message = "File uploaded successfully!", filePath });
            }

            return BadRequest("No file uploaded or invalid file.");
        }

        [HttpPost]
        public IActionResult ExecuteCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return BadRequest("Command cannot be empty.");
            }

            // Example: Only allow safe commands like "echo" or "dir"
            var allowedCommands = new[] { "echo", "dir", "type nul >", "date /t", "time /t", "whoami" };
            //var allowedCommands = new[]
            //{
            //    "dir", "echo", "type nul >", "date /t", "time /t", "whoami", "hostname", "systeminfo", "ipconfig", "ver"
            //};

            if (!allowedCommands.Any(cmd => command.StartsWith(cmd, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Command not allowed.");
            }

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C {command}",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return Json(new { command, output });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error executing command: {ex.Message}");
            }
        }
    }
}
