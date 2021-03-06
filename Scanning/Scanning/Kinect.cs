﻿namespace Scanning
{
    using System;
    using Microsoft.Kinect;

    /// <summary>
    /// An instance of the <see cref="Kinect"/> class represents a wrapper for the <see cref="KinectSensor"/> class.
    /// The <see cref="KinectSensor"/> class is wrapped to avoid possible problems with other connected Kinect sensors.
    /// The <see cref="Kinect"/> class has private constructor and is made as a singleton to guarantee usage of only one Kinect sensor for whole run time.
    /// </summary>
    /// <seealso cref="KinectSensor"/>
    public class Kinect
    {
        /// <summary>
        /// The only instance of the <see cref="Kinect"/> class.
        /// </summary>
        public static readonly Kinect Instance = new Kinect();

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The instance of the <see cref="KinectSensor"/> class.
        /// </summary>
        private KinectSensor kinect;

        /// <summary>
        /// Initializes a new instance of the <see cref="Kinect"/> class.
        /// </summary>
        private Kinect()
        {
            this.kinect = KinectSensor.GetDefault();
        }

        /// <summary>
        /// Opens new <see cref="MultiSourceFrameReader"/> and registers <paramref name="eventHandler"/> to it. The reader produces only <see cref="DepthFrame"/> and <see cref="ColorFrame"/>.
        /// </summary>
        /// <param name="eventHandler">Called whenever a new frame appears.</param>
        public void OpenMultiSourceFrameReader(EventHandler<MultiSourceFrameArrivedEventArgs> eventHandler)
        {
            Log.Info("Opening a frame reader.");
            MultiSourceFrameReader multiSourceFrameReader = this.kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color);
            multiSourceFrameReader.MultiSourceFrameArrived += eventHandler;
        }

        /// <summary>
        /// Starts the <see cref="KinectSensor"/>.
        /// </summary>
        public void Start()
        {
            if (!this.kinect.IsAvailable)
            {
                Log.Info("Starting the Kinect.");
                this.kinect.Open();
            }
        }

        /// <summary>
        /// Stops the <see cref="KinectSensor"/>.
        /// </summary>
        public void Stop()
        {
            if (this.kinect.IsAvailable)
            {
                Log.Info("Stoping the Kinect.");
                this.kinect.Close();
            }
        }
    }
}
