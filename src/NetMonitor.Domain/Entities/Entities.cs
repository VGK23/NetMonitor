using NetMonitor.Domain.Enums;

namespace NetMonitor.Domain.Entities;

public class DeviceGroup
{
    public int Id {get; set;} // key chính
    public string Name {get; set;} = default!; // default! : báo cho trình biên dịch giá trị sẽ được gán sau, đừng cảnh báo
    public string? Description {get; set;} // string? : giá trị được phép null
    public List<Device> Devices {get; set;} = new(); 
}

public class Device
{
    public int Id {get; set;}
    public string Name {get; set;} = default!;
    public string IdAddress {get; set;} = default!;
    public int? Port {get; set;} //null = chỉ ping ICMP
    public DeviceType Type {get; set;}
    public string? Location {get; set;}
    public DeviceStatus Status {get; set;}
    public DateTime? LastCheckedAt {get; set;}
    public bool IsActive {get; set;} = true;
    public int? GroupId {get; set;} // key ngoại
    public DeviceGroup? Group {get; set;}
    public List<CheckLog> CheckLogs {get; set;} = new();
}

public class CheckLog
{
    public long Id {get; set;}
    public int DeviceId {get; set;}
    public Device Device {get; set;} = default!;
    public bool IsUp {get; set;}
    public int? LatencyMs {get; set;}
    public string? Error {get; set;}
    public DateTime CheckedAt {get; set;}
}

public class User
{
    public int Id {get; set;}
    public string UserName {get; set;} = default!;
    public string PasswordHash {get; set;} = default!; // lưu mật khẩu đã được băm, không lưu bản gốc
    public UserRole Role {get; set;}
}