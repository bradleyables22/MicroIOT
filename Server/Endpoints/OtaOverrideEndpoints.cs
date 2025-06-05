﻿using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Server.Data.DTOs.OtaOverride;
using Server.Data.Models;
using Server.Extensions;
using Server.Models.Mqtt.Ota;
using Server.Repositories;
using Server.Services;
using System.Text.Json;

namespace Server.Endpoints
{
	public static class OtaOverrideEndpoints
	{
		public static WebApplication MapOtaOverridesEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/Overrides").WithTags("OTA Overrides");

			group.MapGet("", async (IOtaOverrideRepository _repo) =>
			{
				var result = await _repo.GetAll();
				return result.AsResponse();
			})
				.Produces<List<OtaOverrideRecord>>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("AllOverrides")
				.WithDescription("Get all device overrides")
				.WithSummary("All")
			    .WithName("AllOverrides")
				;

			group.MapGet("Details/{deviceID}", async (IOtaOverrideRepository _repo, string deviceID) =>
			{
				var result = await _repo.GetById(deviceID);
				return result.AsResponse();
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("OverrideByDevice")
				.WithDescription("Check to see if the given device has a registered OTA override it should use.")
				.WithSummary("By Device")
				.WithName("OverrideByDevice")
				;


			group.MapPut("Update", async (IOtaOverrideRepository _repo, IOtaManifestRepository _manifest, MqttService _mqtt, UpdateOtaOverrideDTO update) =>
			{
				var result = await _repo.Update(new OtaOverrideRecord(update));

				if (result.Success)
				{

					var otaRecord = await _manifest.GetById(update.OtaRecordID);

					if (otaRecord.Success && otaRecord.Data != null)
					{
						var json = JsonSerializer.Serialize(new  OtaMessage(otaRecord.Data,"update"));

						var message = new InjectedMqttApplicationMessage(
						   new MqttApplicationMessage
						   {
							   Topic = $"ota/devicegroup/{update.DeviceGroupID}/device/{update.DeviceID}",
							   Payload = json.GetByteSequence(),
							   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
							   Retain = false
						   });


						await _mqtt.PublishAsync(message);
					}
				}
				return result.AsResponse();
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateOverrideRecord")
				.WithDescription("""
					Update an OTA override record for a device. This will be published to the MQTT Topic 'ota/devicegroup/{DeviceGroupID}/device/{DeviceID}' in JSON format:

					{
					  "version": "1.2.3",
					  "url": "https://example.com/firmware.bin",
					  "deviceTypeID": 123,
					  "action": "update"
					}
					""")
				.WithSummary("Update")
				.WithName("UpdateOverrideRecord")
				;

			group.MapPost("Create", async (IOtaOverrideRepository _repo, IOtaManifestRepository _manifest, MqttService _mqtt, CreateOtaOverrideDTO create) =>
			{
				var result = await _repo.Create(new OtaOverrideRecord(create));

				if (result.Success)
				{

					var otaRecord = await _manifest.GetById(create.OtaRecordID);

					if (otaRecord.Success && otaRecord.Data != null)
					{
						var json = JsonSerializer.Serialize(new OtaMessage(otaRecord.Data,"assigned"));

						var message = new InjectedMqttApplicationMessage(
						   new MqttApplicationMessage
						   {
							   Topic = $"ota/devicegroup/{create.DeviceGroupID}/device/{create.DeviceID}",
							   Payload = json.GetByteSequence(),
							   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
							   Retain = false
						   });


						await _mqtt.PublishAsync(message);
					}
				}

				return result.AsResponse();
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateOverrideRecord")
				.WithDescription("""
					Create an OTA override record for a device. This will be published to the MQTT Topic 'ota/devicegroup/{DeviceGroupID}/device/{DeviceID}' in JSON format:

					{
					  "version": "1.2.3",
					  "url": "https://example.com/firmware.bin",
					  "deviceTypeID": 123,
					  "action": "create"
					}
					""")
				.WithSummary("Create")
				.WithName("CreateOverrideRecord")
				;

			group.MapDelete("Delete/{id}", async (IOtaOverrideRepository _repo, IOtaManifestRepository _manifest, MqttService _mqtt, long id) =>
			{
				var result = await _repo.Delete(id);

				if (result.Success)
				{

					var otaRecord = await _manifest.GetById(result.Data.OtaRecordID);

					if (otaRecord.Success && otaRecord.Data != null)
					{
						var defaultRecords = await _manifest.GetWhere(x => x.DeviceTypeID == otaRecord.Data.DeviceTypeID && x.Default == true);

						if (defaultRecords.Success && defaultRecords.Data != null && defaultRecords.Data.Any())
						{
							var defaultRecord = defaultRecords.Data.FirstOrDefault();
							var json = JsonSerializer.Serialize(new OtaMessage(otaRecord.Data,"revert"));

							var message = new InjectedMqttApplicationMessage(
							   new MqttApplicationMessage
							   {
								   Topic = $"ota/devicegroup/{result.Data.DeviceGroupID}/device/{result.Data.DeviceID}",
								   Payload = json.GetByteSequence(),
								   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
								   Retain = false
							   });


							await _mqtt.PublishAsync(message);
						}
					}
				}

				return result.AsResponse();
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteOverrideRecord")
				.WithDescription("""
					Delete an OTA override record. The default firmware information will be published to the MQTT Topic 'ota/devicegroup/{DeviceGroupID}/device/{DeviceID}' in JSON format:

					{
					  "version": "1.2.3",
					  "url": "https://example.com/firmware.bin",
					  "deviceTypeID": 123,
					  "action": "revert"
					}
					""")
				.WithSummary("Delete")
				.WithName("DeleteOverrideRecord")
				;

			return app;
		}
	}
}
