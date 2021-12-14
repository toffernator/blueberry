FROM mcr.microsoft.com/dotnet/nightly/sdk:6.0
WORKDIR /src
RUN dotnet tool install --tool-path /usr/bin dotnet-reportgenerator-globaltool
COPY ./generate-report.sh /
COPY ./EXCLUDE_CLASSES /
ENTRYPOINT [ "/generate-report.sh" ]