namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using DesktopSprites.Collections;
    using DesktopSprites.Core;

    /// <summary>
    /// Provides methods to create and draw animated images based on a time index.
    /// </summary>
    /// <typeparam name="T">The type of frames in this image.</typeparam>
    public sealed class AnimatedImage<T> : Disposable where T : Frame
    {
        /// <summary>
        /// Gets the path to the image file.
        /// </summary>
        public string FilePath { get; private set; }
        /// <summary>
        /// Gets the size of the image.
        /// </summary>
        public Size Size { get; private set; }
        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public int Width
        {
            get { return Size.Width; }
        }
        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public int Height
        {
            get { return Size.Height; }
        }

        /// <summary>
        /// Gets a value indicating the total running time of this animation, in milliseconds.
        /// </summary>
        public int ImageDuration { get; private set; }
        /// <summary>
        /// Gets a value indicating how many times this animation loops. A value of 0 indicates an endless loop.
        /// </summary>
        public int LoopCount { get; private set; }
        /// <summary>
        /// Gets the total number of frames in this animation, excluding any frames of zero duration.
        /// </summary>
        public int FrameCount
        {
            get { return frameIndexes != null ? frameIndexes.Length : frames.Length; }
        }

        /// <summary>
        /// If the image has a common frame duration, indicates the duration each frame is shown for, otherwise -1.
        /// </summary>
        private int commonFrameDuration;

        /// <summary>
        /// The collection of unique frames that make up the image.
        /// </summary>
        private ImmutableArray<T> frames;
        /// <summary>
        /// The duration of each frame in milliseconds.
        /// </summary>
        private ImmutableArray<int> durations;
        /// <summary>
        /// Maps a frame index in an animation to a frame object. This allows the same frame object to be used many times.
        /// </summary>
        private ImmutableArray<int> frameIndexes;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.AnimatedImage`1"/> class from a given file.
        /// </summary>
        /// <param name="path">The path to the file which contains the image to be loaded.</param>
        /// <param name="staticImageFactory">The method that creates a TFrame object for a non-GIF file.</param>
        /// <param name="frameFactory">The method used to construct a TFrame object for each frame in a GIF animation.</param>
        /// <param name="allowableDepths">The allowable set of depths for the raw buffer. Use as many as your output format permits. The
        /// Indexed8Bpp format is required.</param>
        public AnimatedImage(string path, Func<string, T> staticImageFactory,
            BufferToImage<T> frameFactory, BitDepths allowableDepths)
        {
            FilePath = path;

            if (Path.GetExtension(path) == ".gif")
                AnimatedImageFromGif(frameFactory, allowableDepths);
            else
                AnimatedImageFromStaticFormat(staticImageFactory);
        }

        /// <summary>
        /// Creates an <see cref="T:DesktopSprites.SpriteManagement.AnimatedImage`1"/> from a GIF file.
        /// </summary>
        /// <param name="frameFactory">The method used to construct a TFrame object for each frame in a GIF animation.</param>
        /// <param name="allowableDepths">The allowable set of depths for the raw buffer. Use as many as your output format permits. The
        /// Indexed8Bpp format is required.</param>
        private void AnimatedImageFromGif(BufferToImage<T> frameFactory, BitDepths allowableDepths)
        {
            GifImage<T> gifImage;
            using (FileStream imageStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                gifImage = new GifImage<T>(imageStream, frameFactory, allowableDepths);

            Size = gifImage.Size;
            LoopCount = gifImage.Iterations;
            ImageDuration = gifImage.Duration;

            int frameCount = gifImage.Frames.Length;
            var framesList = new List<T>(frameCount);
            var durationsList = new List<int>(frameCount);
            var frameIndexesList = new List<int>(frameCount);
            var frameHashesList = new List<int>(frameCount);
            for (int sourceFrame = 0; sourceFrame < frameCount; sourceFrame++)
            {
                int frameDuration = gifImage.Frames[sourceFrame].Duration;

                // Decoding the GIF may have produced frames of zero duration, we can safely drop these.
                // If the file has all-zero durations, we're into the land of undefined behavior for animations.
                // If we get to the last frame and we have nothing so far, we'll use that just so there is something to display.
                // Alternatively, in an image with only one frame, then only this frame ever need be displayed.
                if (frameDuration == 0 && !(ImageDuration == 0 && sourceFrame == frameCount - 1))
                {
                    // Dispose of unused frame.
                    if (gifImage.Frames[sourceFrame].Image is IDisposable disposable)
                        disposable.Dispose();
                    continue;
                }

                durationsList.Add(frameDuration);

                // Determine if all frames share the same duration.
                if (sourceFrame == 0)
                    commonFrameDuration = frameDuration;
                else if (commonFrameDuration != frameDuration)
                    commonFrameDuration = -1;

                // Calculate the frame hash to check if a duplicate frame exists.
                // This will update our collection and given hash list appropriately.
                AddOrReuseFrame(gifImage.Frames[sourceFrame], framesList, frameIndexesList, frameHashesList);
            }

            frames = framesList.ToImmutableArray();
            if (frames.Length != frameIndexesList.Count)
                frameIndexes = frameIndexesList.ToImmutableArray();
            if (commonFrameDuration == -1)
                durations = durationsList.ToImmutableArray();
        }

        /// <summary>
        /// Checks to see if the hash of the frame is already known, in order to potentially re-use the frame image and save on memory.
        /// </summary>
        /// <param name="frame">The frame whose image is to be checked.</param>
        /// <param name="frames">The collection of unique frames to which new frames should be added.</param>
        /// <param name="frameIndexes">The collection of lookup indexes.</param>
        /// <param name="frameHashes">The collection of hashes for each frame.</param>
        private static void AddOrReuseFrame(GifFrame<T> frame, List<T> frames, List<int> frameIndexes, List<int> frameHashes)
        {
            int frameHash = frame.Image.GetFrameHashCode();

            // Search our existing hashes for a match.
            bool foundMatchingBitmap = false;
            for (int i = 0; i < frameHashes.Count && !foundMatchingBitmap; i++)
                if (frameHash == frameHashes[i])
                {
                    // We found a match, we can reuse the existing bitmap.
                    frameIndexes.Add(i);
                    foundMatchingBitmap = true;
                }

            if (!foundMatchingBitmap)
            {
                // If we didn't find any matches, we can add the bitmap to our frames.
                frames.Add(frame.Image);
                frameHashes.Add(frameHash);
                frameIndexes.Add(frames.Count - 1);
            }
            else
            {
                // Dispose of duplicate frame.
                if (frame.Image is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// Creates an <see cref="T:DesktopSprites.SpriteManagement.AnimatedImage`1"/> from a static image file.
        /// </summary>
        /// <param name="staticImageFactory">The method the creates a <typeparamref name="T"/> object for a non-GIF file.</param>
        private void AnimatedImageFromStaticFormat(Func<string, T> staticImageFactory)
        {
            Size = ImageSize.GetSize(FilePath);
            LoopCount = 0;
            frames = new T[] { staticImageFactory(FilePath) }.ToImmutableArray();
        }

        /// <summary>
        /// Gets the frame at the given index in the animation.
        /// </summary>
        /// <param name="index">The index of the animation from which to retrieve a frame.</param>
        /// <returns>The frame that should be displayed for the given time index in the animation.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is negative or greater than or equal to
        /// FrameCount.</exception>
        public T this[int index]
        {
            get { return frames[FrameIndexFromLogicalIndex(index)]; }
        }

        /// <summary>
        /// Gets the frame for the given time index in the animation.
        /// </summary>
        /// <param name="time">The time index of the animation from which to retrieve a frame. The number of loops in the animation is
        /// respected when seeking frames.</param>
        /// <returns>The frame that should be displayed for the given time index in the animation.</returns>
        public T this[TimeSpan time]
        {
            get { return this[time, false]; }
        }

        /// <summary>
        /// Gets the frame for the given time index in the animation.
        /// </summary>
        /// <param name="time">The time index of the animation from which to retrieve a frame.</param>
        /// <param name="preventLoop">Indicates if the number of loops in the animation is respected when seeking frames. If set, the
        /// number of loops set by the animation is ignored and the animation will not loop once it ends.</param>
        /// <returns>The frame that should be displayed for the given time index in the animation.</returns>
        public T this[TimeSpan time, bool preventLoop]
        {
            get { return frames[FrameIndexFromTimeIndex(time, preventLoop)]; }
        }

        /// <summary>
        /// Gets the duration the specified frame in milliseconds.
        /// </summary>
        /// <param name="frameIndex">The index of the animation from which to retrieve a frame duration.</param>
        /// <returns>The duration the specified frame in milliseconds.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="frameIndex"/> is negative or greater than or equal to
        /// FrameCount.</exception>
        public int GetDuration(int frameIndex)
        {
            Argument.EnsureNonnegative(frameIndex, "frameIndex");
            if (frameIndex >= FrameCount)
                throw new ArgumentOutOfRangeException("frameIndex", frameIndex, "frameIndex must be less than FrameCount.");
            return commonFrameDuration != -1 ? commonFrameDuration : durations[frameIndex];
        }

        /// <summary>
        /// Gets the frame index for the given time index in the animation.
        /// </summary>
        /// <param name="time">The time index of the animation from which to retrieve a frame.</param>
        /// <param name="preventLoop">Indicates if the number of loops in the animation is respected when seeking frames. If set, the
        /// number of loops set by the animation is ignored and the animation will not loop once it ends.</param>
        /// <returns>The frame index that should be displayed for the given time index in the animation.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="time"/> is negative.</exception>
        private int FrameIndexFromTimeIndex(TimeSpan time, bool preventLoop)
        {
            Argument.EnsureNonnegative(time, "time");
            EnsureNotDisposed();

            int frame = 0;

            // Find the frame we need.
            if (FrameCount > 1)
            {
                // Get overall time to find in milliseconds.
                int timeToSeek = (int)time.TotalMilliseconds;
                if (preventLoop && timeToSeek >= ImageDuration)
                {
                    // We do not want to loop, so we want the final frame.
                    frame = FrameCount - 1;
                }
                else
                {
                    // Use integer division to find out how many whole loops will run in that time.
                    int completeLoops = timeToSeek / ImageDuration;

                    if (LoopCount != 0 && completeLoops >= LoopCount)
                    {
                        // We have reached the end of looping, and thus want the final frame.
                        frame = FrameCount - 1;
                    }
                    else
                    {
                        // Subtract the complete loops leaving us with a duration into one run of the animation.
                        int durationToSeek = timeToSeek - (completeLoops * ImageDuration);

                        // Determine which frame we want.
                        if (commonFrameDuration != -1)
                            frame = durationToSeek / commonFrameDuration;
                        else
                            while (durationToSeek > durations[frame])
                                durationToSeek -= durations[frame++];
                    }
                }
            }

            // Return the frame required.
            return FrameIndexFromLogicalIndex(frame);
        }

        /// <summary>
        /// Gets the frame index for the given logical index in the animation.
        /// </summary>
        /// <param name="index">The logical index of the animation from which to retrieve a frame index.</param>
        /// <returns>The frame index that should be displayed for the given time index in the animation.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is negative or greater than or equal to
        /// FrameCount.</exception>
        private int FrameIndexFromLogicalIndex(int index)
        {
            Argument.EnsureNonnegative(index, "index");
            if (index >= FrameCount)
                throw new ArgumentOutOfRangeException("index", index, "index must be less than FrameCount.");
            if (frameIndexes != null)
                return frameIndexes[index];
            else
                return index;
        }

        /// <summary>
        /// Cleans up any resources being used.
        /// </summary>
        /// <param name="disposing">Indicates if managed resources should be disposed in addition to unmanaged resources; otherwise, only
        /// unmanaged resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (T frame in frames)
                {
                    if (frame is IDisposable disposable)
                        disposable.Dispose();
                }
            }
        }
    }
}
