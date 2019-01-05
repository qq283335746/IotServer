
Source Local Dir:
/home/tygasoft/Tyga/GitHub/IotServer
/home/tygasoft/Tyga/GitHub/IotClient

.net core 部署：
使用 IIS 在 Windows 上托管 ASP.NET Core：
https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/iis/index?view=aspnetcore-2.1 （关键：当前 .NET Core 托管捆绑包安装程序（直接下载））

Sqlite:
Rowid,INTEGER PRIMARY KEY AUTOINCREMENT

EFCore+Sqlite:
https://docs.microsoft.com/zh-cn/ef/core/get-started/aspnetcore/new-db?tabs=netcore-cli

dotnet ef migrations add InitialCreate  --new db
dotnet ef migrations add AddProductReviews  --自动迁移
dotnet ef migrations remove
dotnet ef migrations add Users  --old db
dotnet ef migrations add Orders 
dotnet ef database update

Install .NET Core SDK on Linux Ubuntu 16.04 x64:
https://dotnet.microsoft.com/download/linux-package-manager/ubuntu16-04/sdk-current

.net core cli:
https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-run?tabs=netcore21
.NET Core RID 目录:
https://docs.microsoft.com/zh-cn/dotnet/core/rid-catalog
Windows 7 / Windows Server 2008 R2:win7-x64、win7-x86

dotnet add package Microsoft.EntityFrameworkCore.Sqlite -v 2.1.1
dotnet add package Microsoft.EntityFrameworkCore.Design -v 2.1.1

dotnet build:
dotnet run --project ./CATest/TygaSoft.CATest.csproj

dotnet publish:
dotnet publish -c release -r win7-x64 
dotnet publish -f netcoreapp2.1 -c release -r win7-x64 

dotnet new sln -o TygaSoft
dotnet new webapi -o Api -n TygaSoft.Api
dotnet new classlib -o Model -n TygaSoft.Model
dotnet new classlib -o SysUtility -n TygaSoft.SysUtility
dotnet new classlib -o SysException -n TygaSoft.SysException
dotnet new classlib -o NetSharper -n TygaSoft.NetSharper
dotnet new classlib -o IServices -n TygaSoft.IServices
dotnet new classlib -o Services -n TygaSoft.Services
dotnet new classlib -o IRepositories -n TygaSoft.IRepositories
dotnet new classlib -o Repositories -n TygaSoft.Repositories
dotnet new console -o CATest -n TygaSoft.CATest

dotnet sln TygaSoft.sln add Model/TygaSoft.Model.csproj
dotnet sln TygaSoft.sln add SysUtility/TygaSoft.SysUtility.csproj
dotnet sln TygaSoft.sln add SysException/TygaSoft.SysException.csproj
dotnet sln TygaSoft.sln add CATest/TygaSoft.CATest.csproj
dotnet sln TygaSoft.sln add NetSharper/TygaSoft.NetSharper.csproj
dotnet sln TygaSoft.sln add IServices/TygaSoft.IServices.csproj
dotnet sln TygaSoft.sln add Services/TygaSoft.Services.csproj
dotnet sln TygaSoft.sln add IRepositories/TygaSoft.IRepositories.csproj
dotnet sln TygaSoft.sln add Repositories/TygaSoft.Repositories.csproj
dotnet sln TygaSoft.sln add Api/TygaSoft.Api.csproj

dotnet add ./Services/TygaSoft.Services.csproj reference ./Model/TygaSoft.Model.csproj 
dotnet add ./CATest/TygaSoft.CATest.csproj reference ./SysUtility/TygaSoft.SysUtility.csproj
dotnet add ./Api/TygaSoft.Api.csproj reference ./Model/TygaSoft.Model.csproj 
dotnet add ./Api/TygaSoft.Api.csproj reference ./IServices/TygaSoft.IServices.csproj 
dotnet add ./Api/TygaSoft.Api.csproj reference ./Services/TygaSoft.Services.csproj 
dotnet add ./Api/TygaSoft.Api.csproj reference ./IRepositories/TygaSoft.IRepositories.csproj 
dotnet add ./Api/TygaSoft.Api.csproj reference ./Repositories/TygaSoft.Repositories.csproj 
dotnet add ./Api/TygaSoft.Api.csproj reference ./IRepositories/TygaSoft.IRepositories.csproj 
dotnet add ./Services/TygaSoft.Services.csproj reference ./IRepositories/TygaSoft.IRepositories.csproj 
dotnet add ./Services/TygaSoft.Services.csproj reference ./Repositories/TygaSoft.Repositories.csproj 
dotnet add ./Repositories/TygaSoft.Repositories.csproj reference ./IRepositories/TygaSoft.IRepositories.csproj 

getpostman：
https://learning.getpostman.com/docs/postman/launching_postman/installation_and_updates/#linux

数据表更新记录：
Orders: