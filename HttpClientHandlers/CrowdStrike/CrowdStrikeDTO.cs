using System.Text.Json.Serialization;

namespace HttpClientHandlers.CrowdStrike;

public class CrowdStrikeToken
{
    public string? AccessToken { get; init; }
    public DateTime? ExpirationUtc { get; set; }
}

public class Meta
{
    public double query_time { get; set; }
    public Pagination pagination { get; set; }
    public string powered_by { get; set; }
    public string trace_id { get; set; }
}

public class Pagination
{
    public int total { get; set; }
    public string offset { get; set; }
    public long expires_at { get; set; }
}

public class CsDeviceIdResponse
{
    public Meta meta { get; set; }
    public List<string>? resources { get; set; }
    public List<object> errors { get; set; }
}

public class DeviceControl
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }
}

public class DevicePolicies
{
    [JsonPropertyName("prevention")] public Prevention Prevention { get; set; }

    [JsonPropertyName("sensor_update")] public SensorUpdate SensorUpdate { get; set; }

    [JsonPropertyName("device_control")] public DeviceControl DeviceControl { get; set; }

    [JsonPropertyName("global_config")] public GlobalConfig GlobalConfig { get; set; }

    [JsonPropertyName("remote_response")] public RemoteResponse RemoteResponse { get; set; }

    [JsonPropertyName("fim")] public Fim Fim { get; set; }

    [JsonPropertyName("firewall")] public Firewall Firewall { get; set; }

    [JsonPropertyName("system-tray")] public SystemTray SystemTray { get; set; }
}

public class Fim
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("settings_hash")] public string SettingsHash { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public DateTime? AppliedDate { get; set; }
}

public class Firewall
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }

    [JsonPropertyName("rule_set_id")] public string RuleSetId { get; set; }
}

public class GlobalConfig
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("settings_hash")] public string SettingsHash { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }
}

public class CsDeviceDetailMeta
{
    [JsonPropertyName("query_time")] public double? QueryTime { get; set; }

    [JsonPropertyName("powered_by")] public string PoweredBy { get; set; }

    [JsonPropertyName("trace_id")] public string TraceId { get; set; }

    [JsonPropertyName("version")] public string Version { get; set; }

    [JsonPropertyName("version_string")] public string VersionString { get; set; }
}

public class Policy
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("settings_hash")] public string SettingsHash { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }

    [JsonPropertyName("rule_groups")] public List<object> RuleGroups { get; set; }
}

public class Prevention
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("settings_hash")] public string SettingsHash { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }

    [JsonPropertyName("rule_groups")] public List<object> RuleGroups { get; set; }
}

public class RemoteResponse
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("settings_hash")] public string SettingsHash { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }
}

public class CsDeviceDetailResource
{
    [JsonPropertyName("device_id")] public string DeviceId { get; set; }

    [JsonPropertyName("cid")] public string Cid { get; set; }

    [JsonPropertyName("agent_load_flags")] public string AgentLoadFlags { get; set; }

    [JsonPropertyName("agent_local_time")] public DateTime? AgentLocalTime { get; set; }

    [JsonPropertyName("agent_version")] public string AgentVersion { get; set; }

    [JsonPropertyName("bios_manufacturer")]
    public string BiosManufacturer { get; set; }

    [JsonPropertyName("bios_version")] public string BiosVersion { get; set; }

    [JsonPropertyName("build_number")] public string BuildNumber { get; set; }

    [JsonPropertyName("config_id_base")] public string ConfigIdBase { get; set; }

    [JsonPropertyName("config_id_build")] public string ConfigIdBuild { get; set; }

    [JsonPropertyName("config_id_platform")]
    public string ConfigIdPlatform { get; set; }

    [JsonPropertyName("cpu_signature")] public string CpuSignature { get; set; }

    [JsonPropertyName("cpu_vendor")] public string CpuVendor { get; set; }

    [JsonPropertyName("external_ip")] public string ExternalIp { get; set; }

    [JsonPropertyName("mac_address")] public string MacAddress { get; set; }

    [JsonPropertyName("hostname")] public string Hostname { get; set; }

    [JsonPropertyName("first_seen")] public DateTime? FirstSeen { get; set; }

    [JsonPropertyName("last_login_timestamp")]
    public DateTime? LastLoginTimestamp { get; set; }

    [JsonPropertyName("last_login_user")] public string LastLoginUser { get; set; }

    [JsonPropertyName("last_login_user_sid")]
    public string LastLoginUserSid { get; set; }

    [JsonPropertyName("last_seen")] public DateTime? LastSeen { get; set; }

    [JsonPropertyName("local_ip")] public string LocalIp { get; set; }

    [JsonPropertyName("machine_domain")] public string MachineDomain { get; set; }

    [JsonPropertyName("major_version")] public string MajorVersion { get; set; }

    [JsonPropertyName("minor_version")] public string MinorVersion { get; set; }

    [JsonPropertyName("os_version")] public string OsVersion { get; set; }

    [JsonPropertyName("os_build")] public string OsBuild { get; set; }

    [JsonPropertyName("ou")] public List<string> Ou { get; set; }

    [JsonPropertyName("platform_id")] public string PlatformId { get; set; }

    [JsonPropertyName("platform_name")] public string PlatformName { get; set; }

    [JsonPropertyName("policies")] public List<Policy> Policies { get; set; }

    [JsonPropertyName("reduced_functionality_mode")]
    public string ReducedFunctionalityMode { get; set; }

    [JsonPropertyName("rtr_state")] public string RtrState { get; set; }

    [JsonPropertyName("device_policies")] public DevicePolicies DevicePolicies { get; set; }

    [JsonPropertyName("groups")] public List<object> Groups { get; set; }

    [JsonPropertyName("group_hash")] public string GroupHash { get; set; }

    [JsonPropertyName("product_type")] public string ProductType { get; set; }

    [JsonPropertyName("product_type_desc")]
    public string ProductTypeDesc { get; set; }

    [JsonPropertyName("provision_status")] public string ProvisionStatus { get; set; }

    [JsonPropertyName("serial_number")] public string SerialNumber { get; set; }

    [JsonPropertyName("service_pack_minor")]
    public string ServicePackMinor { get; set; }

    [JsonPropertyName("pointer_size")] public string PointerSize { get; set; }

    [JsonPropertyName("site_name")] public string SiteName { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("system_manufacturer")]
    public string SystemManufacturer { get; set; }

    [JsonPropertyName("system_product_name")]
    public string SystemProductName { get; set; }

    [JsonPropertyName("tags")] public List<object> Tags { get; set; }

    [JsonPropertyName("modified_timestamp")]
    public DateTime? ModifiedTimestamp { get; set; }

    [JsonPropertyName("meta")] public Meta Meta { get; set; }

    [JsonPropertyName("kernel_version")] public string KernelVersion { get; set; }

    [JsonPropertyName("os_product_name")] public string OsProductName { get; set; }

    [JsonPropertyName("chassis_type")] public string ChassisType { get; set; }

    [JsonPropertyName("chassis_type_desc")]
    public string ChassisTypeDesc { get; set; }

    [JsonPropertyName("last_reboot")] public DateTime? LastReboot { get; set; }

    [JsonPropertyName("connection_ip")] public string ConnectionIp { get; set; }

    [JsonPropertyName("default_gateway_ip")]
    public string DefaultGatewayIp { get; set; }

    [JsonPropertyName("connection_mac_address")]
    public string ConnectionMacAddress { get; set; }
}

public class CsDeviceDetail
{
    [JsonPropertyName("meta")] public CsDeviceDetailMeta Meta { get; set; }

    [JsonPropertyName("resources")] public List<CsDeviceDetailResource> Resources { get; set; }

    [JsonPropertyName("errors")] public List<object> Errors { get; set; }
}

public class SensorUpdate
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("settings_hash")] public string SettingsHash { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }

    [JsonPropertyName("uninstall_protection")]
    public string UninstallProtection { get; set; }
}

public class SystemTray
{
    [JsonPropertyName("policy_type")] public string PolicyType { get; set; }

    [JsonPropertyName("policy_id")] public string PolicyId { get; set; }

    [JsonPropertyName("applied")] public bool? Applied { get; set; }

    [JsonPropertyName("settings_hash")] public string SettingsHash { get; set; }

    [JsonPropertyName("assigned_date")] public string AssignedDate { get; set; }

    [JsonPropertyName("applied_date")] public string AppliedDate { get; set; }
}