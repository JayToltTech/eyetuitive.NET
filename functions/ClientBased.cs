using GazeFirst;
using System;

namespace GazeFirst.functions
{
    /// <summary>
    /// Base class for all classes that are based on a client
    /// </summary>
    public class ClientBased
    {
        /// <summary>
        /// The client
        /// </summary>
        internal Eyetracker.EyetrackerClient _client;

        /// <summary>
        /// Deadline for blocking calls that take no timeout from the caller
        /// </summary>
        internal static readonly TimeSpan DefaultCallTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// ClientBased constructor
        /// </summary>
        /// <param name="client"></param>
        internal ClientBased(Eyetracker.EyetrackerClient client) 
        {
            _client = client;
        }

        /// <summary>
        /// Reconnect to the eye tracker and restart all the stream tasks
        /// </summary>
        internal virtual void Reconnect() { }

        /// <summary>
        /// Update the client
        /// </summary>
        /// <param name="client"></param>
        internal void UpdateClient(Eyetracker.EyetrackerClient client)
        {
            _client = client;
            Reconnect();
        }
    }
}
