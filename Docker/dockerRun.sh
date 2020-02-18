#!/bin/sh

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Start123!' -e 'MSSQL_PID=Express' -p 1432:1433 -d picscape_base
