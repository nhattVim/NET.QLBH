dotnet ef dbcontext scaffold "Server=172.28.182.33;Database=QLBH;User Id=sa;Password=nhattVim2*;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --force

dotnet aspnet-codegenerator controller -name ProductController -api -m Product -dc QlbhContext -outDir Controllers --useAsyncActions

dotnet aspnet-codegenerator controller -name UserController -api -m User -dc QlbhContext -outDir Controllers --useAsyncActions

httpgenerator http://localhost:5000/swagger/v1/swagger.json -o Utils/Http