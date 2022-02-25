#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["../N5.Exam.Consumer/N5.Exam.Consumer.csproj", "../N5.Exam.Consumer/"]
RUN dotnet restore "../N5.Exam.Consumer/N5.Exam.Consumer.csproj"
COPY . .
WORKDIR "/src/../N5.Exam.Consumer"
RUN dotnet build "N5.Exam.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "N5.Exam.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "N5.Exam.Consumer.dll"]