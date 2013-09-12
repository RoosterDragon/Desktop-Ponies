namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DesktopSprites.Core;

    /// <summary>
    /// Manages a mapping of RGB colors to ARGB colors and provides methods to load and save the data.
    /// </summary>
    public class AlphaRemappingTable
    {
        /// <summary>
        /// The extension for Alpha Remapping Table files.
        /// </summary>
        public const string FileExtension = ".art";

        /// <summary>
        /// Maps RGB colors to ARGB colors.
        /// </summary>
        private readonly Dictionary<RgbColor, ArgbColor> map = new Dictionary<RgbColor, ArgbColor>();

        /// <summary>
        /// Loads a mapping from an Alpha Remapping Table file.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The file extension of <paramref name="path"/> does not match
        /// <see cref="F:DesktopSprites.SpriteManagement.AlphaRemappingTable.FileExtension"/>.</exception>
        /// <exception cref="T:System.IO.InvalidDataException">The length of the file was not valid.</exception>
        public void LoadMap(string path)
        {
            Argument.EnsureNotNull(path, "path");

            if (Path.GetExtension(path) != FileExtension)
                throw new ArgumentException("path must point to a " + FileExtension + " file.", "path");

            using (FileStream file = File.OpenRead(path))
            {
                if (file.Length % 7 != 0)
                    throw new InvalidDataException("Length of file was not the expected multiple of 7 bytes.");

                map.Clear();

                while (file.Position < file.Length)
                    map.Add(
                        new RgbColor((byte)file.ReadByte(), (byte)file.ReadByte(), (byte)file.ReadByte()),
                        new ArgbColor((byte)file.ReadByte(), (byte)file.ReadByte(), (byte)file.ReadByte(), (byte)file.ReadByte()));
            }
        }

        /// <summary>
        /// Saves the current mapping to an Alpha Remapping Table file, or deletes the file if the mapping is now empty.
        /// </summary>
        /// <param name="path">The path to the file to save.</param>
        /// <returns>Returns true if the mapping was saved; returns false if the file was deleted due to an empty map.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The file extension of <paramref name="path"/> does not match
        /// <see cref="F:DesktopSprites.SpriteManagement.AlphaRemappingTable.FileExtension"/>.</exception>
        public bool SaveMap(string path)
        {
            Argument.EnsureNotNull(path, "path");

            if (Path.GetExtension(path) != FileExtension)
                throw new ArgumentException("path must point to a " + FileExtension + " file.", "path");

            if (map.Count == 0)
            {
                File.Delete(path);
                return false;
            }

            using (FileStream file = File.Create(path))
                foreach (KeyValuePair<RgbColor, ArgbColor> mapping in map)
                {
                    file.WriteByte(mapping.Key.R);
                    file.WriteByte(mapping.Key.G);
                    file.WriteByte(mapping.Key.B);
                    file.WriteByte(mapping.Value.A);
                    file.WriteByte(mapping.Value.R);
                    file.WriteByte(mapping.Value.G);
                    file.WriteByte(mapping.Value.B);
                }
            return true;
        }

        /// <summary>
        /// Removes all mappings currently defined.
        /// </summary>
        public void ResetMap()
        {
            map.Clear();
        }

        /// <summary>
        /// Adds a mapping from the given RGB color to the given ARGB color.
        /// </summary>
        /// <param name="source">The source RGB color.</param>
        /// <param name="destination">The destination ARGB color.</param>
        /// <exception cref="T:System.ArgumentException">The <paramref name="source"/> and <paramref name="destination"/> colors are
        /// identical, and thus this mapping is trivial.</exception>
        public void AddMapping(RgbColor source, ArgbColor destination)
        {
            if (source.ToArgb() == destination.ToArgb())
                throw new ArgumentException(
                    "source and destination colors are identical. Trivial mappings may not be explicitly defined.");

            map.Add(source, destination);
        }

        /// <summary>
        /// Gets the ARGB color that is mapped to from the given RGB color.
        /// </summary>
        /// <param name="source">The source RGB color.</param>
        /// <param name="destination">When this method returns, contains resulting ARGB color, if a mapping exists; otherwise, the default
        /// ARGB color. This parameter is passed uninitialized.</param>
        /// <returns>Returns true if a mapping was found; otherwise, false.</returns>
        public bool TryGetMapping(RgbColor source, out ArgbColor destination)
        {
            return map.TryGetValue(source, out destination);
        }
    }
}
