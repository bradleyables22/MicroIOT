using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Server.Services
{
	public class DeviceTracker
	{
		public ConcurrentDictionary<string, DeviceRecord> Devices { get; } = new();

		public event Action<string, ChangeType>? OnDeviceChanged;

		private void NotifyDeviceChanged(string deviceId, ChangeType type)
			=> OnDeviceChanged?.Invoke(deviceId, type);

		public void DeviceConnected(string deviceId)
		{
			Devices.AddOrUpdate(deviceId,
				id => new DeviceRecord(),
				(id, record) =>
				{
					record.Disconnected = false;
					record.ConnectedOn = DateTime.UtcNow;
					return record;
				});

			NotifyDeviceChanged(deviceId, ChangeType.Connected);
		}

		public void DeviceDisConnected(string deviceId)
		{
			if (Devices.TryGetValue(deviceId, out var record))
			{
				record.Disconnected = true;
				NotifyDeviceChanged(deviceId, ChangeType.Disconnected);
			}
		}

		public void AddOrUpdateTopic(string deviceId, string topic)
		{
			Devices.AddOrUpdate(deviceId,
				id => new DeviceRecord
				{
					Topics = new List<string> { topic },
					LastTopic = topic
				},
				(id, record) =>
				{
					if (!record.Topics.Contains(topic))
						record.Topics.Add(topic);

					record.LastTopic = topic;
					return record;
				});

			NotifyDeviceChanged(deviceId, ChangeType.UpdatedTopic);
		}

		public void RemoveTopic(string deviceId, string topic)
		{
			if (Devices.TryGetValue(deviceId, out var record))
			{
				record.Topics.Remove(topic);
				NotifyDeviceChanged(deviceId, ChangeType.UpdatedTopic);
			}
		}

		public void UpdateLastMessageTime(string deviceId)
		{
			if (Devices.TryGetValue(deviceId, out var record))
			{
				record.LastMessageOn = DateTime.UtcNow;
				NotifyDeviceChanged(deviceId, ChangeType.UpdatedLastMessage);
			}
		}

		public DeviceRecord? GetDevice(string deviceId)
		{
			Devices.TryGetValue(deviceId, out var record);
			return record;
		}
	}

	public class DeviceRecord
	{
		public DateTime ConnectedOn { get; set; } = DateTime.UtcNow;
		public DateTime? LastMessageOn { get; set; }
		public string LastTopic { get; set; } = string.Empty;
		public List<string> Topics { get; set; } = new();
		public bool Disconnected { get; set; } = false;
	}

	public enum ChangeType
	{
		Connected,
		Disconnected,
		UpdatedTopic,
		UpdatedLastMessage
	}
}
