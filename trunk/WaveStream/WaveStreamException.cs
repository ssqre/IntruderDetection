using System;

namespace WaveStream
{
	/// <summary>
	/// Summary description for WaveStreamException.
	/// </summary>
	public class WaveStreamException : Exception
	{
		/// <summary>
		/// Constructs a new, default exception
		/// </summary>
		public WaveStreamException() : base()
		{
		}

		/// <summary>
		/// Constructs a new exception.
		/// </summary>
		/// <param name="message">Error message</param>
		public WaveStreamException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructs a new exception
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Exception leading to this exception</param>
		public WaveStreamException(string message, Exception innerException) : base(message, innerException)
		{
		}


	}
}
