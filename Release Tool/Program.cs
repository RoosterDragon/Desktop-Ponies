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
        public static void Main()
        {
            string dpVersion = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(DesktopPonies.Direction)).Location).FileVersion;
            Console.WriteLine("Current version of Desktop Ponies is " + dpVersion);

            string toolDirectory = Environment.CurrentDirectory;
            string solutionDirectory = toolDirectory.Remove(toolDirectory.IndexOf("Release Tool"));
            string contentDirectory = Path.Combine(solutionDirectory, "Content");
            string releaseDirectory = Path.Combine(solutionDirectory, "Desktop Ponies", "bin", "Release");

            Environment.CurrentDirectory = contentDirectory;

            bool contentChanged = false;
            if (ConsoleReadYesNoQuit("Run image optimizers?"))
            {
                if (File.Exists(Path.Combine(toolDirectory, "gifsicle.exe")))
                {
                    CropImages(toolDirectory, contentDirectory);
                    contentChanged = CompressGifs(toolDirectory, contentDirectory) > 0 || contentChanged;
                }
                else
                {
                    Console.WriteLine("Missing gifsicle.exe in current directory. Add a copy of this program to optimize GIFs.");
                }
                if (File.Exists(Path.Combine(toolDirectory, "pngout.exe")))
                {
                    CompressPngs(toolDirectory, contentDirectory);
                }
                else
                {
                    Console.WriteLine("Missing pngout.exe in current directory. Add a copy of this program to optimize PNGs.");
                }
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

        private static void CropImages(string toolDirectory, string contentDirectory)
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
                var imageFilePaths = Directory.GetFiles(ponyDirectory, "*.gif").Concat(Directory.GetFiles(ponyDirectory, "*.png"))
                    .Select(filePath => filePath.Replace(contentDirectory + Path.DirectorySeparatorChar, ""));

                // Ignore any images involved in effects or behavior chains when set up by follow targets, as the transparent borders may
                // be used for alignment.
                // TODO: An improved method would not be to ignore these images, but consider them more carefully.
                // For effects, each side could be considered individually and the minimum of any shared padding between the behavior and
                // effect could be cropped.
                // For images in sequences, the union of the minimums for each image in the sequences for both the follower and followee
                // could be cropped.
                var imagesToIgnore = ponyBase.Effects.Select(
                    e => new[] { e.LeftImage.Path, e.RightImage.Path, e.GetBehavior().LeftImage.Path, e.GetBehavior().RightImage.Path })
                .Concat(
                ponyBase.Behaviors.Where(b => !string.IsNullOrEmpty(b.FollowTargetName)).SelectMany(
                    b =>
                    {
                        var behaviorChain = new List<DesktopPonies.Behavior>();
                        while (b != null)
                        {
                            behaviorChain.Add(b);
                            b = b.GetLinkedBehavior();
                        }
                        return behaviorChain;
                    }).Select(
                    b => new[] { b.LeftImage.Path, b.RightImage.Path }))
                    .SelectMany(images => images);
                var imageFilePathsToUse = new HashSet<string>(imageFilePaths, PathEquality.Comparer);
                imageFilePathsToUse.ExceptWith(imagesToIgnore);
                var imageCropInfo = new Dictionary<string, Point>(PathEquality.Comparer);
                foreach (var imageFilePath in imageFilePathsToUse)
                {
                    Rectangle? maybeCropped = GetCroppedRegion(imageFilePath);
                    if (maybeCropped.HasValue)
                    {
                        if (Path.GetExtension(imageFilePath) == ".gif")
                            CropGifImage(toolDirectory, contentDirectory, imageFilePath, maybeCropped.Value);
                        else
                            CropPngImage(contentDirectory, imageFilePath, maybeCropped.Value);
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
            if (currentLine == Console.CursorTop)
                ConsoleReplacePreviousLine("");
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
        private static Rectangle? GetCroppedRegion(string filePath)
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

        private static void CropGifImage(string toolDirectory, string contentDirectory, string filePath, Rectangle cropArea)
        {
            string tempFilePath = Path.GetTempFileName();
            File.Delete(tempFilePath);
            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.FileName = Path.Combine(toolDirectory, "gifsicle.exe");
                if (!File.Exists(process.StartInfo.FileName))
                {
                    Console.WriteLine("Missing gifsicle.exe in current directory.");
                    return;
                }
                string trimmedSourcePath = filePath.Replace(contentDirectory, "");
                Console.WriteLine("Cropping " + trimmedSourcePath);
                File.Copy(filePath, tempFilePath, true);
                process.StartInfo.Arguments = "-b --crop {1},{2}+{3}x{4} \"{0}\"".FormatWith(
                    tempFilePath, cropArea.X, cropArea.Y, cropArea.Width, cropArea.Height);
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

        private static int CompressGifs(string toolDirectory, string sourceDirectory)
        {
            string tempFilePath = Path.GetTempFileName();
            File.Delete(tempFilePath);
            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                int gifsOptimized = 0;
                process.StartInfo.FileName = Path.Combine(toolDirectory, "gifsicle.exe");
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

        private static void CompressPngs(string toolDirectory, string sourceDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.FileName = Path.Combine(toolDirectory, "pngout.exe");
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
            var encoding = System.Text.Encoding.UTF8;

            string solutionParentDirectory = solutionDirectory.Substring(0, solutionDirectory.IndexOf("Desktop Ponies"));
            byte[] solutionParentDirectoryUpperEncoded = encoding.GetBytes(solutionParentDirectory.ToUpperInvariant());
            byte[] solutionParentDirectoryLowerEncoded = encoding.GetBytes(solutionParentDirectory.ToLowerInvariant());
            byte[] cleanedDirectoryEncoded = encoding.GetBytes(new string('_', solutionParentDirectory.Length));
            if (solutionParentDirectoryUpperEncoded.Length != solutionParentDirectoryLowerEncoded.Length ||
                solutionParentDirectoryUpperEncoded.Length != cleanedDirectoryEncoded.Length)
                throw new Exception("Cannot clean PDB files. Directory encoding lengths do not match.");

            string tempDirectory = Path.GetTempPath();
            byte[] tempDirectoryUpperEncoded = encoding.GetBytes(tempDirectory.ToUpperInvariant());
            byte[] tempDirectoryLowerEncoded = encoding.GetBytes(tempDirectory.ToLowerInvariant());
            byte[] cleanedTempDirectoryEncoded = encoding.GetBytes(new string('_', tempDirectory.Length));
            if (tempDirectoryUpperEncoded.Length != tempDirectoryLowerEncoded.Length ||
                tempDirectoryUpperEncoded.Length != cleanedTempDirectoryEncoded.Length)
                throw new Exception("Cannot clean PDB files. Directory encoding lengths do not match.");

            foreach (var fileName in Directory.EnumerateFiles(sourceDirectory, "*.pdb"))
            {
                byte[] contents = File.ReadAllBytes(fileName);
                contents = Replace(contents,
                    solutionParentDirectoryUpperEncoded, solutionParentDirectoryLowerEncoded, cleanedDirectoryEncoded);
                contents = Replace(contents,
                    tempDirectoryUpperEncoded, tempDirectoryLowerEncoded, cleanedTempDirectoryEncoded);
                File.WriteAllBytes(fileName, contents);
            }
        }

        private static byte[] Replace(byte[] source, byte[] oldValue1, byte[] oldValue2, byte[] newValue)
        {
            List<byte> destination = new List<byte>(source.Length);
            int contentsIndex = 0;
            int nextMatch;
            while ((nextMatch = IndexOf(source, oldValue1, oldValue2, contentsIndex)) != -1)
            {
                destination.AddRange(source.Skip(contentsIndex).Take(nextMatch - contentsIndex));
                destination.AddRange(newValue);
                contentsIndex = nextMatch + oldValue1.Length;
            }
            destination.AddRange(source.Skip(contentsIndex));
            return destination.ToArray();
        }

        private static int IndexOf(byte[] source, byte[] value1, byte[] value2, int startIndex)
        {
            for (int sourceIndex = startIndex; sourceIndex < source.Length - value1.Length; sourceIndex++)
            {
                int src = sourceIndex;
                int val = 0;
                while (source[src] == value1[val] || source[src] == value2[val])
                {
                    src++;
                    val++;
                    if (val >= value1.Length)
                        return sourceIndex;
                }
            }
            return -1;
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
                foreach (var entryToRemove in new[]
                {
                    "Desktop Ponies.vshost.exe",
                    "Desktop Ponies.vshost.exe.config",
                    "Desktop Ponies.vshost.exe.manifest",
                    "Desktop Ponies.xml",
                    "Desktop Sprites.xml"
                })
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
