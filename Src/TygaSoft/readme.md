EFCore+Sqlite:
https://docs.microsoft.com/zh-cn/ef/core/get-started/aspnetcore/new-db?tabs=netcore-cli
dotnet ef migrations add InitialCreate  --new db
dotnet ef migrations add  --old db
dotnet ef database update

.net core cli:
https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-run?tabs=netcore21

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

dotnet build
dotnet run --project ./CATest/TygaSoft.CATest.csproj