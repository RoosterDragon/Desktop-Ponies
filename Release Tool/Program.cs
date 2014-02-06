namespace ReleaseTool
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using DesktopSprites.Core;

    public static class Program
    {
        public static void Main(string[] args)
        {
            string dpVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(DesktopPonies.Direction)).Location).FileVersion;
            Console.WriteLine("Current version of Desktop Ponies is " + dpVersion);
            
            string currentDirectory = Environment.CurrentDirectory;
            string solutionDirectory = currentDirectory.Remove(currentDirectory.IndexOf("Release Tool"));
            string contentDirectory = Path.Combine(solutionDirectory, "Content");
            string releaseDirectory = Path.Combine(solutionDirectory, "Desktop Ponies", "bin", "Release");

            DesktopPonies.EvilGlobals.InstallLocation = contentDirectory;

            bool contentChanged = false;
            if (ConsoleReadYesNoQuit("Run image optimizers?"))
            {
                CropImages(contentDirectory);
                contentChanged = CompressGifs(contentDirectory) > 0 || contentChanged;
                CompressPngs(contentDirectory);
                Console.WriteLine("Optimizing finished.");
                Console.WriteLine();
            }
            if (contentChanged)
            {
                Console.WriteLine("Content has changed and you should ensure it is copied to output. (Rebuilding will work).");
            }
            else if (ConsoleReadYesNoQuit("Package output?"))
            {
                CleanPdbFiles(releaseDirectory, solutionDirectory);
                PackageReleaseFiles(releaseDirectory, solutionDirectory, new Version(dpVersion).ToDisplayString());
            }

            Console.WriteLine("Finished. Press any key to exit...");
            Console.Read();
        }

        private static void CropImages(string contentDirectory)
        {
            Console.WriteLine("Loading bases pending cropping of transparent borders.");
            var ponies = new DesktopPonies.PonyCollection(true);
            string poniesDirectory = Path.Combine(contentDirectory, DesktopPonies.PonyBase.RootDirectory);
            int currentLine = -1;
            foreach (var ponyBase in ponies.Bases)
            {
                if (currentLine == Console.CursorTop)
                    ConsoleReplacePreviousLine("Checking images for " + ponyBase.Directory);
                else
                    Console.WriteLine("Checking images for " + ponyBase.Directory);
                currentLine = Console.CursorTop;
                var ponyDirectory = Path.Combine(poniesDirectory, ponyBase.Directory);
                var imageFilePaths = Directory.GetFiles(ponyDirectory, "*.gif").Concat(Directory.GetFiles(ponyDirectory, "*.png"));
                // Ignore any images involved in effects, as the transparent borders may be used for alignment.
                var imagesToIgnore = ponyBase.Effects.Select(e =>
                {
                    var behavior = ponyBase.Behaviors.Single(b => b.Name == e.BehaviorName);
                    return new string[] { e.LeftImage.Path, e.RightImage.Path, behavior.LeftImage.Path, behavior.RightImage.Path };
                }).SelectMany(images => images).ToArray();
                var imageFilePathsToUse = new HashSet<string>(imageFilePaths, DesktopPonies.PathEquality.Comparer);
                imageFilePathsToUse.ExceptWith(imagesToIgnore);
                var imageCropInfo = new Dictionary<string, Point>(DesktopPonies.PathEquality.Comparer);
                foreach (var imageFilePath in imageFilePathsToUse)
                {
                    Rectangle? maybeCropped = GetCroppedRegion(contentDirectory, imageFilePath);
                    if (maybeCropped.HasValue)
                    {
                        if (Path.GetExtension(imageFilePath) == ".gif")
                        {
                            if (maybeCropped.Value == Rectangle.Empty)
                                CropGifImage(contentDirectory, imageFilePath, new Rectangle(0, 0, 1, 1));
                            else
                                CropGifImage(contentDirectory, imageFilePath, maybeCropped.Value);
                        }
                        else
                        {
                            CropPngImage(contentDirectory, imageFilePath, maybeCropped.Value);
                        }
                        imageCropInfo.Add(imageFilePath, maybeCropped.Value.Location);
                    }
                }
                bool changed = false;
                foreach (var behavior in ponyBase.Behaviors)
                {
                    changed = FixCustomCenter(imageCropInfo, behavior, true) || changed;
                    changed = FixCustomCenter(imageCropInfo, behavior, false) || changed;
                }
                if (changed)
                    ponyBase.Save();
            }
        }

        private static bool FixCustomCenter(Dictionary<string, Point> imageCropInfo, DesktopPonies.Behavior behavior, bool left)
        {
            var image = left ? behavior.LeftImage : behavior.RightImage;
            if (image.CustomCenter.HasValue && imageCropInfo.ContainsKey(image.Path))
            {
                var center = image.CustomCenter.Value;
                image.CustomCenter -= imageCropInfo[image.Path];
                Console.WriteLine("Moved center {0} to {1} for {2} image in behavior {3}",
                    center, image.CustomCenter, left ? "left" : "right", behavior.Name);
                return true;
            }
            return false;
        }

        /// <remarks>Adapted from http://stackoverflow.com/questions/4820212/automatically-trim-a-bitmap-to-minimum-size by user Thomas
        /// Levesque http://stackoverflow.com/users/98713/thomas-levesque </remarks>
        private static Rectangle? GetCroppedRegion(string contentDirectory, string filePath)
        {
            Bitmap[] bitmaps;
            if (Path.GetExtension(filePath) == ".gif")
                using (var reader = new FileStream(filePath, FileMode.Open))
                    bitmaps = DesktopSprites.SpriteManagement.GifImage.OfBitmap(reader).Frames.Select(f => f.Image).ToArray();
            else
                bitmaps = new Bitmap[] { new Bitmap(filePath) };

            var area = new Rectangle(0, 0, bitmaps[0].Width, bitmaps[0].Height);
            Rectangle? cropLimits = null;
            foreach (var sourceBitmap in bitmaps)
            {
                var bitmap = sourceBitmap.Clone(area, PixelFormat.Format32bppArgb);
                Rectangle srcRect;
                BitmapData data = null;
                try
                {
                    #region Determine Bitmap Crop Area
                    data = bitmap.LockBits(area, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    byte[] buffer = new byte[data.Height * data.Stride];
                    Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

                    int xMin = int.MaxValue,
                        xMax = int.MinValue,
                        yMin = int.MaxValue,
                        yMax = int.MinValue;

                    bool foundPixel = false;

                    // Find xMin
                    for (int x = 0; x < data.Width; x++)
                    {
                        bool stop = false;
                        for (int y = 0; y < data.Height; y++)
                        {
                            byte alpha = buffer[y * data.Stride + 4 * x + 3];
                            if (alpha != 0)
                            {
                                xMin = x;
                                stop = true;
                                foundPixel = true;
                                break;
                            }
                        }
                        if (stop)
                            break;
                    }

                    // Image is empty...
                    if (!foundPixel)
                    {
                        srcRect = Rectangle.Empty;
                    }
                    else
                    {
                        // Find yMin
                        for (int y = 0; y < data.Height; y++)
                        {
                            bool stop = false;
                            for (int x = xMin; x < data.Width; x++)
                            {
                                byte alpha = buffer[y * data.Stride + 4 * x + 3];
                                if (alpha != 0)
                                {
                                    yMin = y;
                                    stop = true;
                                    break;
                                }
                            }
                            if (stop)
                                break;
                        }

                        // Find xMax
                        for (int x = data.Width - 1; x >= xMin; x--)
                        {
                            bool stop = false;
                            for (int y = yMin; y < data.Height; y++)
                            {
                                byte alpha = buffer[y * data.Stride + 4 * x + 3];
                                if (alpha != 0)
                                {
                                    xMax = x;
                                    stop = true;
                                    break;
                                }
                            }
                            if (stop)
                                break;
                        }

                        // Find yMax
                        for (int y = data.Height - 1; y >= yMin; y--)
                        {
                            bool stop = false;
                            for (int x = xMin; x <= xMax; x++)
                            {
                                byte alpha = buffer[y * data.Stride + 4 * x + 3];
                                if (alpha != 0)
                                {
                                    yMax = y;
                                    stop = true;
                                    break;
                                }
                            }
                            if (stop)
                                break;
                        }

                        srcRect = Rectangle.FromLTRB(xMin, yMin, xMax + 1, yMax + 1);
                    }
                    #endregion
                }
                finally
                {
                    if (data != null)
                        bitmap.UnlockBits(data);
                }
                if (cropLimits == null)
                    cropLimits = srcRect;
                else
                    cropLimits = Rectangle.Union(cropLimits.Value, srcRect);
            }
            if (cropLimits != area && !(cropLimits == Rectangle.Empty && area == new Rectangle(0, 0, 1, 1)))
                return cropLimits;

            return null;
        }

        private static void CropGifImage(string contentDirectory, string filePath, Rectangle cropArea)
        {
            string tempFilePath = Path.GetTempFileName();
            File.Delete(tempFilePath);
            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "gifsicle.exe");
                if (!File.Exists(process.StartInfo.FileName))
                {
                    Console.WriteLine("Missing gifsicle.exe in current directory.");
                    return;
                }
                string trimmedSourcePath = filePath.Replace(contentDirectory, "");
                Console.WriteLine("Cropping " + trimmedSourcePath);
                File.Copy(filePath, tempFilePath, true);
                process.StartInfo.Arguments = "-b --crop-transparency \"" + tempFilePath + "\"";
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    Console.WriteLine("gifsicle exited with code " + process.ExitCode + " for " + trimmedSourcePath);
                else
                    RetryActionWithDelay(() => File.Replace(tempFilePath, filePath, null), 3, TimeSpan.FromSeconds(0.5));
            }
        }

        private static void CropPngImage(string contentDirectory, string filePath, Rectangle cropArea)
        {
            string trimmedSourcePath = filePath.Replace(contentDirectory, "");
            Console.WriteLine("Cropping " + trimmedSourcePath);
            Bitmap copiedBitmap = null;
            try
            {
                using (var bitmap = new Bitmap(filePath))
                    copiedBitmap = new Bitmap(bitmap);
                General.FullCollect();
                using (var croppedBitmap = copiedBitmap.Clone(cropArea, copiedBitmap.PixelFormat))
                    croppedBitmap.Save(filePath, ImageFormat.Png);
            }
            finally
            {
                if (copiedBitmap != null)
                    copiedBitmap.Dispose();
            }
        }

        private static int CompressGifs(string sourceDirectory)
        {
            string tempFilePath = Path.GetTempFileName();
            File.Delete(tempFilePath);
            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                int gifsOptimized = 0;
                process.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "gifsicle.exe");
                if (!File.Exists(process.StartInfo.FileName))
                {
                    Console.WriteLine("Missing gifsicle.exe in current directory.");
                    return 0;
                }
                Console.WriteLine();
                foreach (var sourceFilePath in Directory.EnumerateFiles(sourceDirectory, "*.gif", SearchOption.AllDirectories))
                {
                    string trimmedSourcePath = sourceFilePath.Replace(sourceDirectory, "");
                    ConsoleReplacePreviousLine("Optimizing " + trimmedSourcePath);
                    bool optimized = false;
                    bool optimizedOnPass;
                    do
                    {
                        optimizedOnPass = false;
                        FileInfo sourceFile = new FileInfo(sourceFilePath);
                        sourceFile.CopyTo(tempFilePath, true);
                        process.StartInfo.Arguments =
                            "-b -O3 --no-comments --no-extensions --no-names \"" + tempFilePath + "\"";
                        process.Start();
                        process.WaitForExit();
                        if (process.ExitCode != 0)
                        {
                            Console.WriteLine("gifsicle exited with code " + process.ExitCode + " for " + trimmedSourcePath);
                            Console.WriteLine();
                        }
                        else
                        {
                            FileInfo tempFile = new FileInfo(tempFilePath);
                            if (tempFile.Length < sourceFile.Length)
                            {
                                Console.WriteLine(
                                    "Optimized " + sourceFile.Length + " to " + tempFile.Length + " for " + trimmedSourcePath);
                                RetryActionWithDelay(() => tempFile.Replace(sourceFilePath, null), 3, TimeSpan.FromSeconds(0.5));
                                optimizedOnPass = true;
                                optimized = true;
                            }
                        }
                    } while (optimizedOnPass);
                    if (optimized)
                    {
                        Console.WriteLine();
                        gifsOptimized++;
                    }
                }
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
                ConsoleReplacePreviousLine(gifsOptimized + " GIFs optimized.");
                return gifsOptimized;
            }
        }

        private static void CompressPngs(string sourceDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "pngout.exe");
                if (!File.Exists(process.StartInfo.FileName))
                {
                    Console.WriteLine("Missing pngout.exe in current directory.");
                    return;
                }
                Console.WriteLine();
                foreach (var sourceFilePath in Directory.EnumerateFiles(sourceDirectory, "*.png", SearchOption.AllDirectories))
                {
                    string trimmedSourcePath = sourceFilePath.Replace(sourceDirectory, "");
                    ConsoleReplacePreviousLine("Optimizing " + trimmedSourcePath);

                    process.StartInfo.Arguments = "\"" + sourceFilePath + "\" /q";
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine("pngout exited with code " + process.ExitCode + " for " + trimmedSourcePath);
                        Console.WriteLine();
                    }
                }
                ConsoleReplacePreviousLine("PNGs optimized");
            }
        }

        private static void CleanPdbFiles(string sourceDirectory, string solutionDirectory)
        {
            string solutionParentDirectory = solutionDirectory.Substring(0, solutionDirectory.IndexOf("Desktop Ponies"));
            string solutionParentDirectoryLower = solutionParentDirectory.ToLowerInvariant();
            string cleanedDirectory = new string('_', solutionParentDirectory.Length);
            foreach (var fileName in Directory.EnumerateFiles(sourceDirectory, "*.pdb"))
            {
                string contents = File.ReadAllText(fileName);
                contents = contents.Replace(solutionParentDirectory, cleanedDirectory);
                contents = contents.Replace(solutionParentDirectoryLower, cleanedDirectory);
                File.WriteAllText(fileName, contents);
            }
        }

        private static void PackageReleaseFiles(string sourceDirectory, string destinationDirectory, string version)
        {
            string destinationFilename = Path.Combine(destinationDirectory, "Desktop Ponies v" + version + ".zip");
            if (File.Exists(destinationFilename))
            {
                if (ConsoleReadYesNoQuit("Replace existing package?"))
                    File.Delete(destinationFilename);
                else
                    return;
            }
            ZipFile.CreateFromDirectory(sourceDirectory, destinationFilename, CompressionLevel.Optimal, false);
            using (var zip = ZipFile.Open(destinationFilename, ZipArchiveMode.Update))
                foreach (var entryToRemove in new[]{
                    "Desktop Ponies.vshost.exe", 
                    "Desktop Ponies.vshost.exe.config",
                    "Desktop Ponies.vshost.exe.manifest",
                    "Desktop Ponies.xml",
                    "Desktop Sprites.xml"})
                {
                    var entry = zip.GetEntry(entryToRemove);
                    if (entry != null)
                        entry.Delete();
                }
            Console.WriteLine("Package output to " + destinationFilename);
        }

        private static void RetryActionWithDelay(Action action, int attempts, TimeSpan delay)
        {
            int currentAttempts = 0;
            while (++currentAttempts < attempts)
                try
                {
                    action();
                    return;
                }
                catch (Exception)
                {
                    Thread.Sleep(delay);
                }
            action();
        }

        private static bool ConsoleReadYesNoQuit(string question)
        {
            Console.WriteLine(question + " y/n or q");
            while (true)
                switch (Console.ReadLine())
                {
                    case "y": return true;
                    case "n": return false;
                    case "q": Environment.Exit(0); break;
                }
        }

        private static void ConsoleReplacePreviousLine(string text)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(text);
            if (text.Length < Console.WindowWidth)
                Console.Write(new string(' ', Console.WindowWidth - text.Length));
        }
    }
}
