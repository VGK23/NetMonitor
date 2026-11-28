# NetMonitor

API quản lý và theo dõi thiết bị mạng (router, switch, server), viết bằng ASP.NET Core.

Mình làm dự án này để tự học backend .NET và tận dụng kiến thức mạng đã học ở trường. Ý tưởng: lưu danh sách thiết bị vào database, cứ vài phút ping thử xem máy nào còn sống, ghi lại lịch sử, rồi tính tỷ lệ uptime và báo khi có thiết bị ngừng hoạt động. Dự án đang làm dần theo từng ngày, tiến độ ghi ở mục Lộ trình bên dưới.

## Chức năng

Đã có:

- Khung solution chia 4 lớp (Domain, Application, Infrastructure, Api)
- 4 bảng trong SQL Server: DeviceGroup, Device, CheckLog, User
- Migration tạo database và dữ liệu mẫu (1 nhóm, 3 thiết bị)
- Swagger để xem và thử API

Dự kiến:

- Quản lý thiết bị: thêm, sửa, xóa, xem danh sách có phân trang và tìm kiếm
- Đăng nhập bằng JWT, phân quyền Admin / Viewer
- Service chạy nền kiểm tra thiết bị định kỳ (ping ICMP, kiểm tra cổng TCP) và lưu lịch sử
- Thống kê uptime, thiết bị hay lỗi nhất

## Công nghệ

- C#, ASP.NET Core Web API (.NET 10)
- Entity Framework Core 10, SQL Server 2022 (chạy bằng Docker)
- Swagger (Swashbuckle), Serilog
- xUnit, Moq (viết test ở các ngày sau)

## Cấu trúc dự án

```
NetMonitor/
├── src/
│   ├── NetMonitor.Domain/          entity, enum
│   ├── NetMonitor.Application/     logic nghiệp vụ, DTO, interface
│   ├── NetMonitor.Infrastructure/  EF Core, DbContext
│   └── NetMonitor.Api/             controller, Program.cs
├── tests/
│   └── NetMonitor.Tests/           unit test
└── docs/                           ghi chép từng ngày
```

Các lớp ngoài phụ thuộc vào lớp lõi: Api dùng Application và Infrastructure, Infrastructure dùng Application, Application dùng Domain. Domain không tham chiếu project nào khác.

## Mô hình dữ liệu

- `DeviceGroup` 1 - n `Device`: một nhóm có nhiều thiết bị. Xóa nhóm thì thiết bị giữ lại, chỉ bỏ liên kết nhóm.
- `Device` 1 - n `CheckLog`: mỗi lần kiểm tra thiết bị ghi một dòng log. Xóa thiết bị thì log xóa theo.
- `User`: tài khoản đăng nhập, mật khẩu chỉ lưu dạng băm (hash).

## Chạy thử

### Cần có

- .NET SDK 10
- Docker Desktop (đang chạy)
- Git
- Công cụ migration: `dotnet tool install --global dotnet-ef --version "10.0.*"`

### Các bước

1. Lấy code về:

```bash
git clone https://github.com/<ten-tai-khoan>/NetMonitor.git
cd NetMonitor
```

2. Chạy SQL Server bằng Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<mat-khau>" -p 1433:1433 --name sql-dev -d mcr.microsoft.com/mssql/server:2022-latest
```

Mật khẩu cần ít nhất 8 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt, nếu không container sẽ tự tắt. Kiểm tra bằng `docker ps`, phải thấy `sql-dev` ở trạng thái `Up`.

3. Lưu chuỗi kết nối bằng user-secrets để không lộ mật khẩu (file mẫu ở `src/NetMonitor.Api/appsettings.Example.json`):

```bash
dotnet user-secrets init --project src/NetMonitor.Api
dotnet user-secrets set "ConnectionStrings:Default" "Server=localhost,1433;Database=NetMonitor;User Id=sa;Password=<mat-khau>;TrustServerCertificate=True" --project src/NetMonitor.Api
```

4. Tạo database:

```bash
dotnet ef database update -p src/NetMonitor.Infrastructure -s src/NetMonitor.Api
```

5. Chạy API:

```bash
dotnet run --project src/NetMonitor.Api
```

Mở `http://localhost:<port>/swagger` để thử API. Cổng xem ở dòng `Now listening on` trong terminal (máy mình là 5145).

### Gặp lỗi?

Các lỗi mình đã gặp khi dựng dự án (Docker, xung đột phiên bản package, chuỗi kết nối rỗng...) kèm cách sửa nằm trong [ghi chép Ngày 1](docs/ngay-01.docx).

## Lộ trình

- [x] Ngày 1: dựng khung solution, entity, DbContext, migration, dữ liệu mẫu
- [ ] Ngày 2: CRUD thiết bị, DTO, phân trang, tìm kiếm
- [ ] Xử lý lỗi tập trung, validation
- [ ] Đăng nhập JWT, phân quyền Admin / Viewer
- [ ] Service chạy nền kiểm tra thiết bị định kỳ
- [ ] API thống kê uptime
- [ ] Unit test (xUnit + Moq)
- [ ] Docker Compose (API + SQL Server)

## Nhật ký học

Ghi chép từng ngày nằm trong thư mục [docs](docs):

- [Ngày 1: dựng khung dự án](docs/ngay-01.docx)

## Tác giả

Võ Gia Kiệt - [GitHub](https://github.com/VGK23)
