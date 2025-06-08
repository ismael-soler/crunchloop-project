#!/bin/bash

# Ensures script will exit if a command fails
set -e

# Apply DB mgrations
echo "Applying Database migartions.."
dotnet ef database update

# execute the command passes to the script
# (dotnet blabla.dll) to start de api
exec "$@"
