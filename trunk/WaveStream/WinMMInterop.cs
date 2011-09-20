using System;
using System.Runtime.InteropServices;

namespace WaveStream
{
	/// <summary>
	/// Contains interop structures and method calls for Windows 
	/// Multimedia (WinMM).
	/// </summary>
	internal class WinMMInterop
	{

		/// <summary>
		/// Structure describing the format of the data in a Wave file.
		/// </summary>
		[StructLayoutAttribute(LayoutKind.Sequential, Pack=2)]
		public struct WAVEFORMATEX
		{
			/// <summary>
			/// Waveform-audio format type. Format tags are registered with Microsoft 
			/// Corporation for many compression algorithms.
			/// </summary>
			public short wFormatTag;
			/// <summary>
			/// Number of audio channels
			/// </summary>
			public short nChannels;
			/// <summary>
			/// Number of samples per second for the audio data.
			/// </summary>
			public int nSamplesPerSec;
			/// <summary>
			/// Average number of bytes per second in the audio data.
			/// For non-compressed audio, the average number of bytes
			/// is nSamplesPerSec * nBlockAlign.
			/// </summary>
			public int nAvgBytesPerSec;
			/// <summary>
			/// Alignment of samples in bits.
			/// </summary>
			public short nBlockAlign;
			/// <summary>
			/// Number of bits used for each audio sample.
			/// </summary>
			public short wBitsPerSample;
			/// <summary>
			/// Size of the structure (typically not used).
			/// </summary>
			public short cbSize;
		}

		/// <summary>
		/// Contains information about a file opened using multimedia IO.
		/// </summary>
		[StructLayoutAttribute(LayoutKind.Sequential)]
		public struct MMIOINFO
		{
			/// <summary>
			/// Flags specifying how a file was opened. The following values are defined: 
			/// </summary>
			public int dwFlags;
			/// <summary>
			/// Four-character code identifying the file's I/O procedure. If the I/O procedure 
			/// is not an installed I/O procedure, this member is NULL. 
			/// </summary>
			public int fccIOProc;
			/// <summary>
			/// Pointer to file's IO procedure. 
			/// </summary>
			public IntPtr pIOProc;
			/// <summary>
			/// Extended error value from the mmioOpen function if it returns NULL.
			/// </summary>
			public int wErrorRet;
			/// <summary>
			/// Handle to a local I/O procedure. Media Control Interface (MCI) devices that 
			/// perform file I/O in the background and need an I/O procedure can locate 
			/// a local I/O procedure with this handle. 
			/// </summary>
			public IntPtr hTask;
			/// <summary>
			/// Size, in bytes, of the file's I/O buffer. If the file does not have an I/O buffer, 
			/// this member is zero. 
			/// </summary>
			public int cchBuffer;
			/// <summary>
			/// Pointer to the file's I/O buffer. If the file is unbuffered, this member is NULL. 
			/// </summary>
			public IntPtr pchBuffer;
			/// <summary>
			/// Pointer to the next location in the I/O buffer to be read or written. If no more 
			/// bytes can be read without calling the mmioAdvance or mmioRead function, this 
			/// member points to the pchEndRead member. If no more bytes can be written without 
			/// calling the mmioAdvance or mmioWrite function, this member points to the 
			/// pchEndWrite member. 
			/// </summary>
			public IntPtr pchNext;
			/// <summary>
			/// Pointer to the location that is 1 byte past the last location in the buffer 
			/// that can be read. 
			/// </summary>
			public IntPtr pchEndRead;
			/// <summary>
			/// Pointer to the location that is 1 byte past the last location in the buffer 
			/// that can be written. 
			/// </summary>
			public IntPtr pchEndWrite;
			/// <summary>
			/// Reserved. 
			/// </summary>
			public int lBufOffset;
			/// <summary>
			/// Current file position, which is an offset, in bytes, from the beginning of 
			/// the file. I/O procedures are responsible for maintaining this member. 
			/// </summary>
			public int lDiskOffset;
			/// <summary>
			/// State information maintained by the I/O procedure. I/O procedures can also 
			/// use these members to transfer information from the application to the I/O 
			/// procedure when the application opens a file. 
			/// </summary>
			public int adwInfo0;
			/// <summary>
			/// State information maintained by the I/O procedure. I/O procedures can also 
			/// use these members to transfer information from the application to the I/O 
			/// procedure when the application opens a file. 
			/// </summary>
			public int adwInfo1;
			/// <summary>
			/// State information maintained by the I/O procedure. I/O procedures can also 
			/// use these members to transfer information from the application to the I/O 
			/// procedure when the application opens a file. 
			/// </summary>
			public int adwInfo2;
			/// <summary>
			/// State information maintained by the I/O procedure. I/O procedures can also 
			/// use these members to transfer information from the application to the I/O 
			/// procedure when the application opens a file. 
			/// </summary>
			public int adwInfo3;
			/// <summary>
			/// Reserved. 
			/// </summary>
			public int dwReserved1;
			/// <summary>
			/// Reserved. 
			/// </summary>
			public int dwReserved2;
			/// <summary>
			/// Handle to the open file, as returned by the mmioOpen function. I/O 
			/// procedures can use this handle when calling other multimedia file I/O 
			/// functions. 
			/// </summary>
			public IntPtr hMMIO;
		}

		/// <summary>
		/// Information about a chunk in a RIFF file.
		/// </summary>
		[StructLayoutAttribute(LayoutKind.Sequential)]
		public struct MMCKINFO
		{
			/// <summary>
			/// Chunk identifier. 
			/// </summary>
			public int ckid;
			/// <summary>
			/// Size, in bytes, of the data member of the chunk. The size of the data 
			/// member does not include the 4-byte chunk identifier, the 4-byte 
			/// chunk size, or the optional pad byte at the end of the data member. 
			/// </summary>
			public int ckSize;
			/// <summary>
			/// Form type for "RIFF" chunks or the list type for "LIST" chunks. 
			/// </summary>
			public int fccType;
			/// <summary>
			/// File offset of the beginning of the chunk's data member, relative 
			/// to the beginning of the file. 
			/// </summary>
			public int dwDataOffset;
			/// <summary>
			/// Flags specifying additional information about the chunk. Typically 
			/// zero but can be MMIO_DIRTY indicating the size of the chunk has
			/// changed.
			/// </summary>
			public int dwFlags;
		}

		/// <summary>
		/// Closes a file opened using mmioOpen
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="uFlags">flags</param>
		/// <returns>Error code</returns>
		[DllImport("winmm")]
		public extern static int mmioClose (
			IntPtr hmmio, 
			int uFlags);

		/// <summary>
		/// Descends into a chunk within the file
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="lpck">Chunk to descend into</param>
		/// <param name="lpckParent">Parent chunk to descend from</param>
		/// <param name="uFlags">Flags controlling how to descend</param>
		/// <returns>Error code</returns>
		[DllImport("winmm")]
		public extern static int mmioDescend (
			IntPtr hmmio, 
			ref MMCKINFO lpck, 
			ref MMCKINFO lpckParent, 
			int uFlags);

		/// <summary>
		/// Same as <see cref="mmioDescend"/> but allows descent into
		/// the top level chunk.
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="lpck">Chunk to descend into</param>
		/// <param name="x">Reserved, must be set to 0.</param>
		/// <param name="uFlags">Flags controlling how to descend</param>
		/// <returns>Error code</returns>
		[DllImport("winmm", EntryPoint="mmioDescend")]
		public extern static int mmioDescendParent (
			IntPtr hmmio, 
			ref MMCKINFO lpck, 
			int x, 
			int uFlags);

		/// <summary>
		/// Opens a multimedia RIFF file
		/// </summary>
		/// <param name="szFileName">File name</param>
		/// <param name="lpMmioInfo">Pointer to a <see cref="MMIOINFO"/> structure</param>
		/// <param name="dwOpenFlags">Flags controlling open type</param>
		/// <returns>Handle to open file, or zero if open fails.</returns>
		[DllImport("winmm", CharSet=CharSet.Auto)]
		public extern static IntPtr mmioOpen(
			string szFileName, 
			IntPtr lpMmioInfo,
			int dwOpenFlags);

		/// <summary>
		/// Reads from the file into a buffer.
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="pch">Pointer to the buffer</param>
		/// <param name="cch">Number of bytes to read</param>
		/// <returns>Number of bytes read</returns>
		[DllImport("winmm")]
		public extern static int mmioRead (
			IntPtr hmmio, 
			IntPtr pch, 
			int cch);

		/// <summary>
		/// Writes data to the file from a buffer
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="pch">Pointer to the buffer</param>
		/// <param name="cch">Number of bytes to write</param>
		/// <returns>Number of bytes written</returns>
		[DllImport("winmm")]
		public extern static int mmioWrite (
			IntPtr hmmio, 
			IntPtr pch, 
			int cch);
		
		/// <summary>
		/// Reads a <see cref="WAVEFORMATEX"/> from the file.
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="format">format structure to read</param>
		/// <param name="cch">Number of bytes to read</param>
		/// <returns>Number of bytes read</returns>
		[DllImport("winmm", EntryPoint="mmioRead")]
		public extern static int mmioReadWaveFormat (
			IntPtr hmmio, 
			ref WAVEFORMATEX format, 
			int cch);

		/// <summary>
		/// Writes a <see cref="WAVEFORMATEX"/> to the file.
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="format">format structure to write</param>
		/// <param name="cch">Number of bytes to write</param>
		/// <returns>Number of bytes written</returns>
		[DllImport("winmm", EntryPoint="mmioWrite")]
		public extern static int mmioWriteWaveFormat (
			IntPtr hmmio, 
			ref WAVEFORMATEX format, 
			int cch);

		/// <summary>
		/// Seeks to the specified point in the file
		/// </summary>
		/// <param name="hmmio">Handle to the open file</param>
		/// <param name="iOrigin">SEEK_* flag specifying the origin of the seek</param>
		/// <param name="lOffset">Number of bytes to seek</param>
		/// <returns>Error code</returns>
		[DllImport("winmm")]
		public extern static int mmioSeek (
			IntPtr hmmio, 
			int lOffset, 
			int iOrigin);

		/// <summary>
		/// Converts a string representing a FOURCC code to an actual
		/// FOURCC code.
		/// </summary>
		/// <param name="sz">String containing the FOURCC code bytes</param>
		/// <param name="uFlags">Flags controlling the conversion</param>
		/// <returns>FOURCC code.</returns>
		[DllImport("winmm", CharSet=CharSet.Auto)]
		public extern static int mmioStringToFOURCC(
			[MarshalAs(UnmanagedType.LPTStr)]
			string sz, 
			int uFlags);

		/// <summary>
		/// Ascends from a chunk in a multimedia file.
		/// </summary>
		/// <param name="hmmio">Handle to open file</param>
		/// <param name="lpck">Chunk to ascend from</param>
		/// <param name="uFlags">Flags controlling ascend</param>
		/// <returns>Error code</returns>
		[DllImport("winmm")]
		public extern static int mmioAscend(
			IntPtr hmmio, 
			ref MMCKINFO lpck, 
			int uFlags);

		/// <summary>
		/// Creates a new chunk in a multimedia file.
		/// </summary>
		/// <param name="fuCreate">Flags describing how to create the chunk</param>
		/// <param name="hmmio">Handle to open file</param>
		/// <param name="pmmcki">Chunk to create</param>
		[DllImport("winmm")]
		public extern static int mmioCreateChunk(
			IntPtr hmmio, 
			ref MMCKINFO pmmcki, 
			int fuCreate);


		/// <summary>
		/// File read access
		/// </summary>
		public const int MMIO_READ = 0x0;
		/// <summary>
		/// File write access
		/// </summary>
		public const int MMIO_WRITE = 0x1;
		/// <summary>
		///  File read/write access
		/// </summary>
		public const int MMIO_READWRITE = 0x2;
		/// <summary>
		/// Find a chunk in a file
		/// </summary>
		public const int MMIO_FINDCHUNK = 0x10;
		/// <summary>
		///  Find RIFF header
		/// </summary>
        public const int MMIO_FINDRIFF = 0x20;
		/// <summary>
		///  Create RIFF header
		/// </summary>
		public const int MMIO_CREATERIFF = 0x20;
		/// <summary>
		///  Allocate buffer
		/// </summary>
		public const int MMIO_ALLOCBUF = 0x10000;
		/// <summary>
		/// Create file
		/// </summary>
		public const int MMIO_CREATE = 0x1000;

		/// <summary>
		/// Error code representing success
		/// </summary>
        public const int MMSYSERR_NOERROR = 0;
		/// <summary>
		/// Seek from current position
		/// </summary>
        public const int SEEK_CUR = 1;
		/// <summary>
		/// Seek from end of file
		/// </summary>
        public const int SEEK_END = 2;
		/// <summary>
		/// Seek from beginning of file
		/// </summary>
        public const int SEEK_SET = 0;
		
		/// <summary>
		/// Prevent instantiation
		/// </summary>
		private WinMMInterop()
		{
		}

	}
}
