using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WaveStream
{
	/// <summary>
	/// A <c>Stream</c> implementation which wraps the Windows Multi-media
	/// IO to write a Wave file.
	/// </summary>
	public class WaveStreamWriter : Stream
	{
		private string waveFile;
		private IntPtr hMmio = IntPtr.Zero;
		private bool disposed = false;
		private WinMMInterop.WAVEFORMATEX format;
		private int dataOffset = 0;
		private int audioLength = 0;
		private WinMMInterop.MMCKINFO mmckInfoChild;
		private WinMMInterop.MMCKINFO mmckInfoParent;

		/// <summary>
		/// Default constructor: 16bit, 44.1kHz, Stereo.  No file is created.
		/// </summary>
		public WaveStreamWriter() : base()
		{
			format.cbSize = 0;
			format.nChannels = 2;
			format.nSamplesPerSec = 44100;
			format.wBitsPerSample = 16;
			format.nBlockAlign = 4;
			format.wFormatTag = 1;
		}

		/// <summary>
		/// Constructor: 16bit, 44.1kHz, Stereo,  file is created.
		/// </summary>
		/// <param name="file">File name of the wave file to write</param>
		public WaveStreamWriter(string file) : this()
		{
			Filename = file;
		}

		/// <summary>
		/// Constructor: 16bit, Stereo,  file is created.
		/// </summary>
		/// <param name="file">File name of the wave file to write</param>
		/// <param name="samplingFrequency">Sampling frequency</param>
		public WaveStreamWriter(string file, int samplingFrequency) : this()
		{
			SamplingFrequency = samplingFrequency;
			Filename = file;
		}

		/// <summary>
		/// Constructor: 16bit, file is created.
		/// </summary>	
		/// <param name="file">File name of the wave file to write</param>
		/// <param name="samplingFrequency">Sampling frequency</param>
		/// <param name="channels">Number of audio channels</param>
		public WaveStreamWriter(string file, int samplingFrequency, short channels) : this()
		{
			format.nSamplesPerSec = samplingFrequency;
			format.nChannels = channels;
			Filename = file;
		}

		/// <summary>
		/// Constructor: file is created.
		/// </summary>
		/// <param name="file">File name of the wave file to write</param>
		/// <param name="samplingFrequency">Sampling frequency</param>
		/// <param name="channels">Number of audio channels</param>
		/// <param name="bitsPerSample">Number of bits per sample</param>
		public WaveStreamWriter(string file, int samplingFrequency, short channels, short bitsPerSample) : this()
		{
			format.nSamplesPerSec = samplingFrequency;
			format.nChannels = channels;
			format.wBitsPerSample = bitsPerSample;
			Filename = file;
		}

		/// <summary>
		/// Destructor: ensures that the wave file handle is closed.
		/// </summary>
		~WaveStreamWriter()
		{
			Dispose(false);
		}

		/// <summary>
		/// Clears up resources associated with this class.
		/// </summary>
		new public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Gets the Multi-media IO handle to the wave file.
		/// </summary>
		protected virtual IntPtr Handle
		{
			get
			{
				return hMmio;
			}
		}

		/// <summary>
		/// Clears up resources associated with this class.
		/// </summary>
		/// <param name="disposing"><code>true</code> if disposing from the <c>Dispose</c>
		/// method, otherwise <c>false</c>.</param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// nothing to do
				}
				CloseWaveFile();
				disposed = true;
			}
		}
		/// <summary>
		/// Flush
		/// </summary>
		public override void Flush()
		{
			//
		}

		/// <summary>
		/// Gets/sets the wave file name.
		/// </summary>
		public string Filename
		{
			get
			{
				return waveFile;
			}
			set
			{
				if (hMmio != IntPtr.Zero)
				{
					CloseWaveFile();
				}
				waveFile = value;
				CreateWaveFile();
			}
		}	
	
		/// <summary>
		/// Gets/sets the number of audio channels in the file.
		/// </summary>
		/// <exception cref="InvalidOperationException">If attempt made to change the value when
		/// a file is open.  Must set this prior to setting the filename.</exception>
		public short Channels
		{
			get
			{
				return format.nChannels;
			}
			set
			{
				if (hMmio != IntPtr.Zero)
				{
					throw new InvalidOperationException("Cannot change number of audio channels on an open file.");
				}
				format.nChannels = value;
			}
		}

		/// <summary>
		/// Gets/sets the sample frequency of the file.
		/// </summary>
		/// <exception cref="InvalidOperationException">If attempt made to change the value when
		/// a file is open.  Must set this prior to setting the filename.</exception>
		public int SamplingFrequency
		{
			get
			{
				return format.nSamplesPerSec;
			}
			set
			{
				if (hMmio != IntPtr.Zero)
				{
					throw new InvalidOperationException("Cannot change sampling frequency on an open file.");
				}
				format.nSamplesPerSec = value;
			}
		}

		/// <summary>
		/// Gets/sets the number of bits per sample in the wave file.
		/// </summary>
		/// <exception cref="InvalidOperationException">If attempt made to change the value when
		/// a file is open.  Must set this prior to setting the filename.</exception>
		public short BitsPerSample
		{
			get
			{
				return format.wBitsPerSample;
			}
			set
			{
				if (hMmio != IntPtr.Zero)
				{
					throw new InvalidOperationException("Cannot change bits/sample on an open file.");
				}
				format.wBitsPerSample = value;
			}
		}
		

		/// <summary>
		/// Gets whether the stream can be read or not (true whenever a wave file
		/// is open).
		/// </summary>
		public override bool CanRead
		{
			get
			{
				return (hMmio != IntPtr.Zero);
			}
		}

		/// <summary>
		/// Gets whether the stream is seekable or not (true whenever a wave file
		/// is open).
		/// </summary>
		public override bool CanSeek
		{
			get
			{
				return (hMmio != IntPtr.Zero);
			}
		}

		/// <summary>
		/// Gets whether the stream can be written or not (true whenever a wave file
		/// is open).
		/// </summary>
		public override bool CanWrite
		{
			get
			{
				return (hMmio != IntPtr.Zero);
			}
		}

		/// <summary>
		/// Gets the length of this wave file, in bytes.
		/// </summary>
		public override long Length
		{
			get
			{
				return audioLength;
			}
		}

		/// <summary>
		/// Throws an exception; setting the length of the stream is meaningless
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown exception</exception>
		public override void SetLength(long length)
		{
			throw new InvalidOperationException(
				"This class can only read files.  Use the WaveStreamWriter class to write files.");
		}

		/// <summary>
		/// Gets/sets the position within the wave file.
		/// </summary>
		public override long Position
		{
			get
			{
				return 0;
			}
			set
			{
				Seek(value, SeekOrigin.Begin);
			}
		}

		/// <summary>
		/// Reads <c>count</c> bytes into the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read into</param>
		/// <param name="count">Number of bytes to read</param>
		/// <returns>Number of bytes read.</returns>
		public virtual int Read ( byte[] buffer, int count )
		{
			return Read(buffer, 0, count);
		}

		/// <summary>
		/// Reads <c>count</c> bytes into the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read into</param>
		/// <param name="count">Number of bytes to read</param>
		/// <param name="offset">Offset from the current file position to start reading from</param>
		/// <returns>Number of bytes read.</returns>
		public override int Read ( byte[] buffer , int offset , int count )
		{
			if (hMmio == IntPtr.Zero)
			{
				throw new InvalidOperationException("No wave data is open");
			}

			if (offset != 0)
			{
				Seek((long) offset, SeekOrigin.Current);
			}

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);				
			IntPtr ptrBuffer = handle.AddrOfPinnedObject();

			int dataRemaining = (dataOffset + audioLength - 
				WinMMInterop.mmioSeek(hMmio, 0, WinMMInterop.SEEK_CUR));
			int read = 0;
			if (count < dataRemaining)
			{
				read = WinMMInterop.mmioRead(hMmio, ptrBuffer, count);
			}
			else if (dataRemaining > 0)
			{
				read = WinMMInterop.mmioRead(hMmio, ptrBuffer, dataRemaining);
			}

			if (handle.IsAllocated)
			{
				handle.Free();
			}
			return read;
		}

		/// <summary>
		/// Reads <c>count</c> shorts into the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read into</param>
		/// <param name="count">Number of shorts to read</param>
		/// <returns>Number of bytes read.</returns>
		public virtual int Read (short[] buffer, int count)
		{
			return Read(buffer, 0, count);
		}

		/// <summary>
		/// Reads <c>count</c> shorts into the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read into</param>
		/// <param name="count">Number of shorts to read</param>
		/// <param name="offset">Offset in shorts (2 bytes) from the current file position to start 
		/// reading from</param>
		/// <returns>Number of bytes read.</returns>
		public virtual int Read ( short[] buffer, int offset, int count )
		{
			if (hMmio == IntPtr.Zero)
			{
				throw new InvalidOperationException("No wave data is open");
			}

			if (offset != 0)
			{
				Seek((long) (offset * 2), SeekOrigin.Current);
			}

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);				
			IntPtr ptrBuffer = handle.AddrOfPinnedObject();

			int dataRemaining = (dataOffset + audioLength - 
				WinMMInterop.mmioSeek(hMmio, 0, WinMMInterop.SEEK_CUR)) / 2;
			int read = 0;
			if (count < dataRemaining)
			{
				read = WinMMInterop.mmioRead(hMmio, ptrBuffer, count * 2);
			}
			else if (dataRemaining > 0)
			{
				read = WinMMInterop.mmioRead(hMmio, ptrBuffer, dataRemaining * 2);
			}

			if (handle.IsAllocated)
			{
				handle.Free();
			}
			return read;
		}

		/// <summary>
		/// Writes <c>count</c> bytes from the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read from</param>
		/// <param name="count">Number of bytes to write</param>
		public virtual void Write( byte[] buffer, int count)
		{
			Write(buffer, 0, count);
		}
			
		/// <summary>
		/// Writes <c>count</c> bytes from the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read from</param>
		/// <param name="count">Number of bytes to write</param>
		/// <param name="offset">Offset from the current file position to start writing</param>
		public override void Write( byte[] buffer, int offset, int count)
		{
			if (hMmio == IntPtr.Zero)
			{
				throw new InvalidOperationException("No wave file is open");
			}

			if (offset != 0)
			{
				Seek((long) offset, SeekOrigin.Current);
			}
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);				
			IntPtr ptrBuffer = handle.AddrOfPinnedObject();
	
			int write = WinMMInterop.mmioWrite(hMmio, ptrBuffer, count);
			if (write != count)
			{
				throw new IOException(String.Format(
					"Data truncation: only wrote {0} of {1} requested bytes", write, count));
			}

			if (handle.IsAllocated)
			{
				handle.Free();
			}

		}

		/// <summary>
		/// Writes <c>count</c> shorts from the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read from</param>
		/// <param name="count">Number of shorts to write</param>
		public virtual void Write( short[] buffer, int count)
		{
			Write(buffer, 0, count);
		}
			
		/// <summary>
		/// Writes <c>count</c> shorts from the buffer.
		/// </summary>
		/// <param name="buffer">Buffer to read from</param>
		/// <param name="count">Number of shorts to write</param>
		/// <param name="offset">Offset from the current file position to start writing</param>
		/// <returns>Number of shorts write.</returns>
		public virtual void Write( short[] buffer, int offset, int count)
		{
			if (hMmio == IntPtr.Zero)
			{
				throw new InvalidOperationException("No wave file is open");
			}

			if (offset != 0)
			{
				Seek((long) (offset * 2), SeekOrigin.Current);
			}
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);				
			IntPtr ptrBuffer = handle.AddrOfPinnedObject();
	
			int write = WinMMInterop.mmioWrite(hMmio, ptrBuffer, count * 2);
			if (write != (count * 2))
			{
				throw new IOException(String.Format(
					"Data truncation: only wrote {0} of {1} requested bytes", write, count));
			}

			if (handle.IsAllocated)
			{
				handle.Free();
			}

		}

		/// <summary>
		/// Seeks to the specified position in the stream, in bytes
		/// </summary>
		/// <param name="position">Position to seek to</param>
		/// <param name="origin">Specifies the starting postion of the seek</param>
		public override long Seek(long position, SeekOrigin origin)
		{
			if (hMmio == IntPtr.Zero)
			{
				throw new InvalidOperationException("No wave file is open");
			}
			
			int offset = (int) position;
			int mmOrigin = WinMMInterop.SEEK_CUR;
			if (origin == SeekOrigin.Begin)
			{
				offset += dataOffset;
				mmOrigin = WinMMInterop.SEEK_SET;
			}
			else if (origin == SeekOrigin.End)
			{
				mmOrigin = WinMMInterop.SEEK_END;
			}
			int result = WinMMInterop.mmioSeek(hMmio, offset, mmOrigin);
			if (result == -1)
			{
				throw new WaveStreamException(
					String.Format("Failed to seek to position {0} in file", position));
			}
			return (long) result;
		}
		
		private void CreateWaveFile()
		{
			CloseWaveFile();

			hMmio = WinMMInterop.mmioOpen(waveFile, IntPtr.Zero, 
				WinMMInterop.MMIO_ALLOCBUF | WinMMInterop.MMIO_READWRITE | WinMMInterop.MMIO_CREATE);
			if (hMmio == IntPtr.Zero)
			{
				throw new IOException(
					String.Format("Could not open file {0}", waveFile));
			}

			CreateWaveFormatHeader();

		}

		private void CreateWaveFormatHeader()
		{
			int result = 0;

			// Set derived fields for PCM
			format.nBlockAlign =  (short) ((format.nChannels * format.wBitsPerSample) / 8);
			format.nAvgBytesPerSec = format.nSamplesPerSec * format.nBlockAlign;

			// Write the WAVE header:
			mmckInfoParent = new WinMMInterop.MMCKINFO();
			mmckInfoParent.fccType = WinMMInterop.mmioStringToFOURCC("WAVE", 0);

			result = WinMMInterop.mmioCreateChunk(hMmio, ref mmckInfoParent, 
				WinMMInterop.MMIO_CREATERIFF);
			if (result != WinMMInterop.MMSYSERR_NOERROR)
			{
				CloseWaveFile();
				throw new WaveStreamException("Could not write the WAVE RIFF header chunk to the file.");
			}

			// Create the format chunk and write the format out:
			mmckInfoChild = new WinMMInterop.MMCKINFO();
			mmckInfoChild.ckid = WinMMInterop.mmioStringToFOURCC("fmt", 0);
			mmckInfoChild.ckSize = Marshal.SizeOf(typeof(WinMMInterop.WAVEFORMATEX));
		
			result = WinMMInterop.mmioCreateChunk(hMmio, ref mmckInfoChild, 0);
			if (result != WinMMInterop.MMSYSERR_NOERROR)
			{
				CloseWaveFile();
				throw new WaveStreamException("Could not write the 'fmt' header chunk to the file.");
			}

			int size = WinMMInterop.mmioWriteWaveFormat(hMmio, ref format, mmckInfoChild.ckSize);
			if (size != mmckInfoChild.ckSize)
			{
				CloseWaveFile();
				throw new WaveStreamException("Could not write the format information into the 'fmt' header chunk of the file.");
			}

			// Back out to the WAVE header:
			result = WinMMInterop.mmioAscend(hMmio, ref mmckInfoChild, 0);
			if (result != WinMMInterop.MMSYSERR_NOERROR)
			{
				CloseWaveFile();
				throw new WaveStreamException("Could not ascend out of 'fmt' header chunk.");
			}

			// Create the data chunk:
			mmckInfoChild.ckid = WinMMInterop.mmioStringToFOURCC("data", 0);
			result = WinMMInterop.mmioCreateChunk(hMmio, ref mmckInfoChild, 0);
			if (result != WinMMInterop.MMSYSERR_NOERROR)
			{
				CloseWaveFile();
				throw new WaveStreamException("Could not create the 'data' chunk for the audio data.");
			}
		
			// Stay in the data chunk for writing.
			dataOffset = WinMMInterop.mmioSeek(hMmio, 0, WinMMInterop.SEEK_CUR);
		}

		/// <summary>
		/// Closes the wave file.
		/// </summary>
		public void CloseWaveFile()
		{
			if (hMmio != IntPtr.Zero)
			{
				int result;

				// Ascend the output file out of the output chunk:
				result = WinMMInterop.mmioAscend(hMmio, ref mmckInfoChild, 0);
				// Ascend the output file out of the 'RIFF' chunk:
				result = WinMMInterop.mmioAscend(hMmio, ref mmckInfoParent, 0);

				// Close the file
				WinMMInterop.mmioClose(hMmio, 0);
				hMmio = IntPtr.Zero;
				audioLength = 0;
			}
		}
	}
}
