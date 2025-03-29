## Giao diện

### Danh sách sản phẩm
![](wwwroot/images/repo/1.png)

### Chi tiết sản phẩm
![](wwwroot/images/repo/2.png)

### Thêm sản phẩm mới
![](wwwroot/images/repo/3.png)

### Tìm kiếm sản phẩm
![](wwwroot/images/repo/4.png)

### Lọc sản phẩm theo danh mục
![chưa có]()

## Hướng dẫn chạy dự án

1. **Cài đặt các công cụ cần thiết**:
   - [.NET SDK](https://dotnet.microsoft.com/download)
   - SQL Server

2. **Cấu hình cơ sở dữ liệu**:
   - Cập nhật chuỗi kết nối trong file `appsettings.json`.

3. **Chạy ứng dụng**:
   - Mở terminal và chạy lệnh:
     ```bash
     dotnet run
     ```
   - Truy cập ứng dụng tại `http://localhost:5000`.

4. **Tạo cơ sở dữ liệu**:
   - Chạy lệnh sau để áp dụng migrations:
     ```bash
     dotnet ef database update
     ```

## Đóng góp

Nếu bạn muốn đóng góp cho dự án, vui lòng tạo một pull request hoặc mở một issue mới.
