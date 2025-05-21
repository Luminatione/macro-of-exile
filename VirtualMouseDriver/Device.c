#include "driver.h"
#include "device.tmh"

#ifdef ALLOC_PRAGMA
#pragma alloc_text (PAGE, VirtualDeviceDriverCreateDevice)
#endif

VOID VirtualMouseVhfCleanup(_In_ PVOID VhfClientContext)
{
	PDEVICE_CONTEXT deviceContext = (PDEVICE_CONTEXT)VhfClientContext;
	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "%!FUNC! Entry");
	if (deviceContext->VhfHandle != NULL)
	{
		deviceContext->VhfHandle = NULL;
	}

	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "%!FUNC! Exit");
}

NTSTATUS VirtualDeviceDriverCreateDevice(_Inout_ PWDFDEVICE_INIT DeviceInit)
{
	WDF_OBJECT_ATTRIBUTES deviceAttributes;
	PDEVICE_CONTEXT deviceContext;
	WDFDEVICE device;
	NTSTATUS status;
	VHF_CONFIG vhfConfig;

	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "%!FUNC! Entry");

	PAGED_CODE();

	WDF_OBJECT_ATTRIBUTES_INIT_CONTEXT_TYPE(&deviceAttributes, DEVICE_CONTEXT);
	deviceAttributes.EvtCleanupCallback = VirtualDeviceDriverEvtDriverContextCleanup;

	UNICODE_STRING deviceName;
	RtlInitUnicodeString(&deviceName, DEVICE_NAME);
	status = WdfDeviceInitAssignName(DeviceInit, &deviceName);
	if (!NT_SUCCESS(status))
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_DEVICE, "Failed to assign name to device %!STATUS!", status);
		return status;
	}

	status = WdfDeviceCreate(&DeviceInit, &deviceAttributes, &device);

	if (NT_SUCCESS(status))
	{
		deviceContext = DeviceGetContext(device);
		PDEVICE_OBJECT deviceObj = WdfDeviceWdmGetDeviceObject(device);
		VHF_CONFIG_INIT(&vhfConfig, deviceObj, sizeof(InputReportDescriptor), InputReportDescriptor);
		vhfConfig.VhfClientContext = deviceContext;
		vhfConfig.EvtVhfCleanup = VirtualMouseVhfCleanup;
		TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "VHF_CONFIG values - DeviceObject: %p", vhfConfig.DeviceObject);
		TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "device: %p", device);
		TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "device context: %p", deviceContext);
		TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "VHF_Handle: %p", deviceContext->VhfHandle);
		TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "DeviceObject: %p", deviceObj);

		status = VhfCreate(&vhfConfig, &deviceContext->VhfHandle);
		if (!NT_SUCCESS(status))
		{
			TraceEvents(TRACE_LEVEL_ERROR, TRACE_DEVICE, "VhfCreate failed %!STATUS!", status);
			return status;
		}

		status = VhfStart(deviceContext->VhfHandle);
		if (!NT_SUCCESS(status))
		{
			TraceEvents(TRACE_LEVEL_ERROR, TRACE_DEVICE, "VhfStart failed %!STATUS!", status);
			return status;
		}

		UNICODE_STRING deviceLinkName;
		RtlInitUnicodeString(&deviceLinkName, DEVICE_LINK_NAME);
		status = WdfDeviceCreateSymbolicLink(device, &deviceLinkName);
		if (!NT_SUCCESS(status))
		{
			TraceEvents(TRACE_LEVEL_ERROR, TRACE_DEVICE, "Symbolic link creation failed %!STATUS!", status);
			return status;
		}

		TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "Symbolic link created");
		VirtualDeviceDriverQueueInitialize(device);
	}

	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_DEVICE, "Device created");
	return status;
}
