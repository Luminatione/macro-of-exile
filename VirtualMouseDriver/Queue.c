#include "driver.h"
#include "queue.tmh"

#pragma warning(disable: 4100)

#ifdef ALLOC_PRAGMA
#pragma alloc_text (PAGE, VirtualDeviceDriverQueueInitialize)
#endif

NTSTATUS VirtualDeviceDriverQueueContextDestroy(WDFOBJECT Object)
{
	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_QUEUE, "%!FUNC! Entry");
	PQUEUE_CONTEXT queueContext = QueueGetContext(Object);
	if (queueContext != NULL)
	{
		if (queueContext->VhfHandle != NULL)
		{
			VhfDelete(queueContext->VhfHandle, TRUE);
			queueContext->VhfHandle = NULL;
		}

		WdfObjectDelete(queueContext);
	}

	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_QUEUE, "%!FUNC! Exit");
	return STATUS_SUCCESS;
}

PHID_XFER_PACKET GetPacket(PUCHAR report, ULONG reportSize, UCHAR reportId)
{
	PHID_XFER_PACKET packet = (PHID_XFER_PACKET) ExAllocatePoolWithTag(NonPagedPool, sizeof(*packet), 'HIDP');
	if (packet == NULL)
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "Failed to allocate memory for HID_XFER_PACKET");
		return NULL;
	}

	packet->reportId = reportId;
	packet->reportBuffer = report;
	packet->reportBufferLen = reportSize;

	return packet;
}

MouseHidReport* GetMouseReport(PINPUT_MESSAGE message)
{
	MouseHidReport* report = (MouseHidReport*) ExAllocatePoolWithTag(NonPagedPool, sizeof(*report), 'MHR');
	if (report == NULL)
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "Failed to allocate memory for MouseHidReport");
		return NULL;
	}

	RtlZeroMemory(report, sizeof(*report));
	report->reportId = MOUSE_REPORT_ID;
	report->xAxis = message->xAxis;
	report->yAxis = message->yAxis;
	report->button1 = (message->buttons & LMB_MESSAGE_BIT) ? 1 : 0;
	report->button2 = (message->buttons & RMB_MESSAGE_BIT) ? 1 : 0;
	report->button3 = (message->buttons & MMB_MESSAGE_BIT) ? 1 : 0;

	return report;
}

KeyboardHidReport* GetKeyboardReport(PINPUT_MESSAGE message)
{
	KeyboardHidReport* report = (KeyboardHidReport*) ExAllocatePoolWithTag(NonPagedPool, sizeof(*report), 'KHR');
	if (report == NULL)
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "Failed to allocate memory for KeyboardHidReport");
		return NULL;
	}

	RtlZeroMemory(report, sizeof(*report));
	report->reportId = KEYBOARD_REPORT_ID;
	report->key[0] = (UCHAR)message->key;
	report->key[1] = 0xE1;
	report->modifiers = message->modifiers;
	return report;
}

NTSTATUS SendInput(PQUEUE_CONTEXT context, PHID_XFER_PACKET packet)
{
	PDEVICE_CONTEXT deviceContext = (PDEVICE_CONTEXT) context;

	NTSTATUS status = VhfReadReportSubmit(deviceContext->VhfHandle, packet);
	if (!NT_SUCCESS(status))
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "Failed to submit report %!STATUS!", status);
	}
	else
	{
		TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_QUEUE, "Successfully submitted report");
	}

	return status;
}

NTSTATUS VirtualDeviceDriverQueueInitialize(_In_ WDFDEVICE Device)
{
	WDFQUEUE queue;
	NTSTATUS status;
	WDF_IO_QUEUE_CONFIG queueConfig;
	WDF_OBJECT_ATTRIBUTES queueAttributes;

	PAGED_CODE();

	WDF_IO_QUEUE_CONFIG_INIT_DEFAULT_QUEUE(&queueConfig, WdfIoQueueDispatchParallel);

	queueConfig.EvtIoDeviceControl = VirtualDeviceDriverEvtIoDeviceControl;
	queueConfig.EvtIoStop = VirtualDeviceDriverEvtIoStop;

	WDF_OBJECT_ATTRIBUTES_INIT_CONTEXT_TYPE(&queueAttributes, QUEUE_CONTEXT);

	status = WdfIoQueueCreate(Device, &queueConfig, &queueAttributes, &queue);

	PQUEUE_CONTEXT queueContext = QueueGetContext(queue);
	PDEVICE_CONTEXT deviceContext = DeviceGetContext(Device);

	if (queueContext == NULL)
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "Failed to get queue context");
		return STATUS_INSUFFICIENT_RESOURCES;

	}

	if (deviceContext == NULL)
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "Failed to get device context");
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	queueContext->VhfHandle = deviceContext->VhfHandle;
	queueContext->Data = deviceContext->Data;

	if (!NT_SUCCESS(status)) 
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "WdfIoQueueCreate failed %!STATUS!", status);
		return status;
	}

	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_QUEUE, "Queue initialized");
	WdfIoQueueStart(queue);
	return status;
}

NTSTATUS HandleMessage(PQUEUE_CONTEXT context, PINPUT_MESSAGE message, ULONG controlCode)
{
	PUCHAR report = NULL;
	UCHAR reportId = 0;
	ULONG reportSize = 0;
	switch (controlCode)
	{
	case IOCTL_MOUSE_INPUT_SEND:
		report = (PUCHAR) GetMouseReport(message);
		reportId = MOUSE_REPORT_ID;
		reportSize = sizeof(MouseHidReport);
		break;

	case IOCTL_KEYBOARD_INPUT_SEND:
		report = (PUCHAR) GetKeyboardReport(message);
		reportId = KEYBOARD_REPORT_ID;
		reportSize = sizeof(KeyboardHidReport);
		break;

	default:
	{
		TraceEvents(TRACE_LEVEL_WARNING, TRACE_DEVICE, "Ignoring unknown request");
		return STATUS_SUCCESS;
	}
	}

	if (report == NULL)
	{
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	PHID_XFER_PACKET packet = GetPacket(report, reportSize, reportId);
	if (packet == NULL)
	{
		ExFreePool(report);
		return STATUS_INSUFFICIENT_RESOURCES;
	}

	NTSTATUS status = SendInput(context, packet);
	ExFreePool(report);
	ExFreePool(packet);
	return status;
}

VOID VirtualDeviceDriverEvtIoDeviceControl(_In_ WDFQUEUE Queue, _In_ WDFREQUEST Request, _In_ size_t OutputBufferLength, _In_ size_t InputBufferLength, _In_ ULONG IoControlCode)
{
	LONG status;
	PINPUT_MESSAGE message = NULL;

	status = WdfRequestRetrieveInputBuffer(Request, sizeof(*message), &message, NULL);

	if (!NT_SUCCESS(status))
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "WdfRequestRetrieveInputBuffer failed: 0x%x", status);
	}

	status = HandleMessage(QueueGetContext(Queue), message, IoControlCode);
	if (!NT_SUCCESS(status))
	{
		TraceEvents(TRACE_LEVEL_ERROR, TRACE_QUEUE, "HandleMessage failed: 0x%x", status);
	}

	WdfRequestComplete(Request, status);
}

VOID VirtualDeviceDriverEvtIoStop(_In_ WDFQUEUE Queue, _In_ WDFREQUEST Request, _In_ ULONG ActionFlags)
{
	TraceEvents(TRACE_LEVEL_INFORMATION, TRACE_QUEUE, "%!FUNC! Queue 0x%p, Request 0x%p ActionFlags %d", Queue, Request, ActionFlags);
	return;
}
