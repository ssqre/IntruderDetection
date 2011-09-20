using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WaveStream
{
	/// <summary>
	/// A <c>Stream</c> implementation which wraps the Windows Multi-media
	/// IO to read a Wave file.
	/// </summary>
	public class WaveStreamReader : Stream, IDisposable
	{
		private string waveFile;
		private IntPtr hMmio = IntPtr.Zero;
		private bool disposed = false;
		private WinMMInterop.WAVEFORMATEX format;
		private int dataOffset = 0;
		private int audioLength = 0;

		/// <summary>
		/// Default constructor
		/// </summary>
		public WaveStreamReader() : base()
		{
		}

		/// <summary>
		/// Constructor for a particular wave file.
		/// </summary>
		/// <param name="file">File name of the wave file to read</param>
		public WaveStreamReader(string file) : this()
		{
			Filename = file;
		}

		/// <summary>
		/// Destructor: ensures that the wave file handle is closed.
		/// </summary>
		~WaveStreamReader()
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
				OpenWaveFile();
			}
		}	
	
		/// <summary>
		/// Gets the number of audio channels in the file.
		/// </summary>
		public short Channels
		{
			get
			{
				return format.nChannels;
			}
		}

		/// <summary>
		/// Gets the sample frequency of the file.
		/// </summary>
		public int SamplingFrequency
		{
			get
			{
				return format.nSamplesPerSec;
			}
		}

		/// <summary>
		/// Gets the number of bits per sample in the wave file.
		/// </summary>
		public short BitsPerSample
		{
			get
			{
				return format.wBitsPerSample;
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
		/// is open)
		/// </summary>
		public override bool CanSeek
		{
			get
			{
				return (hMmio != IntPtr.Zero);
			}
		}

		/// <summary>
		/// Returns false; this is a read-only stream
		/// </summary>
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Throws an exception; this is a read-only stream
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown exception</exception>
		public override void Flush()
		{
			throw new InvalidOperationException(
				"This class can only read files.  Use the WaveStreamWriter class to write files.");
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
		/// Throws an exception; this is a read-only stream
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
				Seek(value + dataOffset, SeekOrigin.Begin);
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
				Seek((long) (offset / 2), SeekOrigin.Current);
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
			return read / 2;
		}

		/// <summary>
		/// Throws an exception; this is a read-only stream
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown exception</exception>
		public override void Write( byte[] buffer, int offset, int count)
		{
			throw new InvalidOperationException(
				"This class can only read files.  Use the WaveStreamWriter class to write wave files.");
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
				throw new InvalidOperationException("No wave data is open");
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
		
		private void OpenWaveFile()
		{
			CloseWaveFile();

			if (!File.Exists(waveFile))
			{
				throw new FileNotFoundException(
					String.Format("The file {0} does not exist", waveFile));
			}

			hMmio = WinMMInterop.mmioOpen(waveFile, IntPtr.Zero, WinMMInterop.MMIO_READ);
			if (hMmio == IntPtr.Zero)
			{
				throw new IOException(
					String.Format("Could not open file {0}", waveFile));
			}

			GetWaveData();

		}

		private void GetWaveData()
		{
			int result = 0;
			WinMMInterop.MMCKINFO mmckInfoParent = new WinMMInterop.MMCKINFO();

			// Descend into the wave header:
			mmckInfoParent.fccType = WinMMInterop.mmioStringToFOURCC("WAVE", 0);
			result = WinMMInterop.mmioDescendParent(hMmio, ref mmckInfoParent, 0, 
				WinMMInterop.MMIO_FINDRIFF);
			if (result != WinMMInterop.MMSYSERR_NOERROR)
			{
				CloseWaveFile();
				throw new WaveStreamException(
					String.Format("The file {0} is not a wave file", waveFile));
			}

			// Descend into the fmt chunk:
			WinMMInterop.MMCKINFO mmckSubChunkIn = new WinMMInterop.MMCKINFO();
			mmckSubChunkIn.ckid = WinMMInterop.mmioStringToFOURCC("fmt", 0);
			result = WinMMInterop.mmioDescend(hMmio, ref mmckSubChunkIn, ref mmckInfoParent, 
				WinMMInterop.MMIO_FINDCHUNK);
			if (result != WinMMInterop.MMSYSERR_NOERROR)
			{
				CloseWaveFile();
				throw new WaveStreamException(
					String.Format("Unable to locate the format chunk in file {0}", waveFile));
			}

			format = new WinMMInterop.WAVEFORMATEX();
			result = WinMMInterop.mmioReadWaveFormat(hMmio, ref format, mmckSubChunkIn.ckSize);
			if (result == -1)
			{
				CloseWaveFile();
				throw new WaveStreamException(
					String.Format("Unable to read the wave format from file {0}", waveFile));				
			}

			// Find the data subchunk
			result = WinMMInterop.mmioAscend(hMmio, ref mmckSubChunkIn, 0);

			mmckSubChunkIn.ckid = WinMMInterop.mmioStringToFOURCC("data", 0);
			result = WinMMInterop.mmioDescend(hMmio, ref mmckSubChunkIn, ref mmckInfoParent, 
				WinMMInterop.MMIO_FINDCHUNK);
			if (result != WinMMInterop.MMSYSERR_NOERROR)
			{
				CloseWaveFile();
				throw new WaveStreamException(
					String.Format("Unable to locate the data chunk in file {0}", waveFile));
			}

			dataOffset = WinMMInterop.mmioSeek(hMmio, 0, WinMMInterop.SEEK_CUR);
			audioLength = mmckSubChunkIn.ckSize;
		}

		private void CloseWaveFile()
		{
			if (hMmio != IntPtr.Zero)
			{
				WinMMInterop.mmioClose(hMmio, 0);
				hMmio = IntPtr.Zero;
				audioLength = 0;
			}
		}

	}
}
