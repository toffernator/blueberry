#!/bin/bash
set -e
TARGET=$1
FILES=$(ls $(pwd)/*.Tests/TestResults/*/coverage.cobertura.xml | xargs | sed 's| |;|g')
EXCLUDE_CLASSES=$(cat /EXCLUDE_CLASSES | xargs | sed 's| |;-|g')
reportgenerator "-reports:${FILES}" "-targetdir:${TARGET}" "-classfilters:-${EXCLUDE_CLASSES}"
if [ -n $2 ]
then
    chown -R $2:$2 ${TARGET}
    find ${TARGET} -type f -exec chmod 0664 {} +
    find ${TARGET} -type d -exec chmod 0775 {} +
else
    find ${TARGET} -type f -exec chmod 0666 {} +
    find ${TARGET} -type d -exec chmod 0777 {} +
fi