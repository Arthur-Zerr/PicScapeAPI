FROM mcr.microsoft.com/mssql/server

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Start123!
ENV MSSQL_PID=Developer
ENV MSSQL_COLLATION=LATIN1_GENERAL_CI_AS

COPY ./InitDb.sql /var

RUN /opt/mssql/bin/sqlservr --accept-eula & sleep 10 \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Start123!' -i /var/InitDB.sql \
    && pkill sqlservr 
