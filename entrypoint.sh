#!/bin/sh
set -e
FIRST_RUN_FILE="/has_run"
if [ ! -f ${FIRST_RUN_FILE} ]
then
    dotnet blueberry.Server.dll seed
    touch ${FIRST_RUN_FILE}
fi
dotnet blueberry.Server.dll