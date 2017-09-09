﻿namespace Scanning
{
    using System;
    using System.Threading;
    using Microsoft.Kinect;

    /// <summary>
    /// Delegate for the <see cref="FrameHandler.Finished"/> event.
    /// </summary>
    /// <param name="sender">The sender of an event.</param>
    /// <param name="e">Arguments of an event.</param>
    public delegate void FinishedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// An instance of the <see cref="FrameHandler"/> class reppresents a frame handler that is used to process a given number of incoming frames.
    /// It creates new <see cref="Thread"/>, everytime a frame is processed, and call a user-specified action.
    /// The class creates an event when the execution of requested number of frames is finished to inform everybody who is listening.
    /// </summary>
    public class FrameHandler
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The requested number of frames.
        /// </summary>
        private uint numberOfFrames;

        /// <summary>
        /// The counter of processed frames.
        /// </summary>
        private int framesCounter = 0;

        /// <summary>
        /// The user-specified action performed with every frame.
        /// </summary>
        private Action<Frame> frameAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameHandler"/> class.
        /// </summary>
        /// <param name="numberOfFrames">Used to stop the frame processing.</param>
        /// <param name="frameAction">Performed with every frame.</param>
        /// <seealso cref="BinFrameWriter"/>
        public FrameHandler(uint numberOfFrames, Action<Frame> frameAction)
        {
            Log.Debug("Number of frames: " + numberOfFrames);
            this.numberOfFrames = numberOfFrames;
            this.frameAction = frameAction;
        }

        /// <summary>
        /// The event that fires when the <see cref="FrameHandler"/> processed all requested frames.
        /// </summary>
        public event FinishedEventHandler Finished;

        /// <summary>
        /// Handles incoming frames from <see cref="MultiSourceFrameReader"/>.
        /// </summary>
        /// <param name="sender">The sender of an event.</param>
        /// <param name="e">Arguments of an event.</param>
        /// <seealso cref="Kinect"/>
        public void Handler_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
                if (multiSourceFrame != null)
                {
                    Frame frame = new Frame(multiSourceFrame);
                    if (frame.AcquireData())
                    {
                        frame.ID = ++this.framesCounter;
                        Log.Info("Frame number " + this.framesCounter + " was read properly.");
                        if (this.framesCounter <= this.numberOfFrames)
                        {
                            Log.Info("Starting the given action in a new thread.");
                            Thread thread = new Thread(new ThreadStart(() => this.frameAction.Invoke(frame)));
                            thread.Start();
                        }
                        else
                        {
                            Log.Info("Scanning is finished.");
                            this.OnFinished(EventArgs.Empty);
                        }
                    } 
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Frame was not read properly.");
                Log.Error(exception.Message + " Frame was not read properly.");
            }
        }

        /// <summary>
        /// Invokes the <see cref="Finished"/> event.
        /// </summary>
        /// <param name="e">Arguments of an event.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            this.Finished?.Invoke(this, e);
        }
    }
}