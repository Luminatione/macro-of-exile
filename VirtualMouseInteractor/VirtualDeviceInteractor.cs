﻿using Shared.Target;
using System;
using System.Runtime.InteropServices;

namespace VirtualDeviceInteractor
{
	public class VirtualDeviceInteractor : IDisposable, ITarget
	{
		protected static readonly uint MouseInputEventControlCode = CTL_CODE(FILE_DEVICE_UNKNOWN, 0x800, METHOD_BUFFERED, FILE_ANY_ACCESS);
		protected static readonly uint KeyboardInputEventControlCode = CTL_CODE(FILE_DEVICE_UNKNOWN, 0x801, METHOD_BUFFERED, FILE_ANY_ACCESS);

		protected const uint FILE_DEVICE_UNKNOWN = 0x00000022;
		protected const uint METHOD_BUFFERED = 0;
		protected const uint FILE_ANY_ACCESS = 0;
		private const uint GENERIC_READ = 0x80000000;
		private const uint GENERIC_WRITE = 0x40000000;
		private const uint FILE_SHARE_READ = 0x00000001;
		private const uint FILE_SHARE_WRITE = 0x00000002;
		private const uint OPEN_EXISTING = 3;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

		protected IntPtr device;

        public VirtualDeviceInteractor(string driverPath)
		{
			device = CreateFile(driverPath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
		}

		protected static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
		{
			return ((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method);
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		protected static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		protected static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		protected static extern bool CloseHandle(IntPtr hObject);

		public virtual void Dispose()
		{
			if (device != IntPtr.Zero)
			{
				CloseHandle(device);
				device = IntPtr.Zero;
			}
		}

		public void MoveMouse(int x, int y)
		{
			InputMessageBuilder builder = new InputMessageBuilder();
			builder.Move((char)x, (char)y);
			SendInputState(builder.Build(), MouseInputEventControlCode);
		}

		public void SetKeyState(int key, int state)
		{
            InputMessageBuilder builder = new InputMessageBuilder();
			builder.Key(key, state);
			SendInputState(builder.Build(), KeyboardInputEventControlCode);
		}

		public void SetButtonState(int button, int state)
		{
            InputMessageBuilder builder = new InputMessageBuilder();
			builder.Button(button, state);
            SendInputState(builder.Build(), MouseInputEventControlCode);
		}

		public int GetMilisBetweenActions()
		{
			return 20;
		}

		protected void SendInputState(InputMessage inputState, uint canal)
		{
			int size = Marshal.SizeOf<InputMessage>();
			IntPtr buffer = Marshal.AllocHGlobal(size);
			try
			{
				Marshal.StructureToPtr(inputState, buffer, false);
				uint returned = 0;
				DeviceIoControl(device, canal, buffer, (uint)size, IntPtr.Zero, 0, ref returned, IntPtr.Zero);
			}
			finally
			{
				Marshal.FreeHGlobal(buffer);
			}
		}
	}
}
